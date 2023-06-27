import { trigger, state, style, transition, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { CommonDataSourceRequest, ResponseResult, StaffDataSourceRequest } from 'src/app/_Models/common/common.model';
import { InspectionOccupancyMasterModel, InspectionOccupancyModel } from 'src/app/_Models/report/inspection-occupancy.model';
import { EmployeeTypes } from 'src/app/_Models/statistics/manday-dashboard.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { EmailConfigurationService } from 'src/app/_Services/email-send/email-config.service';
import { HrService } from 'src/app/_Services/hr/hr.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ReportService } from 'src/app/_Services/Report/report.service';
import { FinanceDashboardService } from 'src/app/_Services/statistics/finance-dashboard.service';
import { Validator } from '../common';
import { EmployeeType, ListSize, PageSizeCommon } from '../common/static-data-common';
import { SummaryComponent } from '../common/summary.component';

@Component({
  selector: 'app-inspection-occupancy',
  templateUrl: './inspection-occupancy.component.html',
  styleUrls: ['./inspection-occupancy.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '24px',
        'padding-bottom': '24px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class InspectionOccupancyComponent extends SummaryComponent<InspectionOccupancyModel>  {

  masterModel: InspectionOccupancyMasterModel;
  isFilterOpen: boolean = true;
  selectedPageSize = PageSizeCommon[0];
  pagesizeitems = PageSizeCommon;
  componentDestroyed$: Subject<boolean> = new Subject();
  _EmployeeType = EmployeeType;
  searchLoading: boolean;

  constructor(
    private hrService: HrService, private reportService: ReportService,
    private referenceService: ReferenceService, private locationService: LocationService, private financeDashboardService: FinanceDashboardService, private emailConfigurationService: EmailConfigurationService,
    router: Router, public validator: Validator, route: ActivatedRoute, protected translate?: TranslateService, protected toastr?: ToastrService,
    public utility?: UtilityService) {
    super(router, validator, route, translate, toastr, utility);

    this.masterModel = new InspectionOccupancyMasterModel();
    this.model = new InspectionOccupancyModel();
    this.model.pageSize = this.selectedPageSize;

    this.validator.setJSON("inspection-occupancy/inspection-occupancy-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
  }

  onInit(): void {
    this.getOfficeList();
    this.getOfficeCountryList();
    this.getEmployeeTypeList();
    this.getStaffListBySearch();
  }

  getData(): void {
    this.searchLoading = true;
    this.model.noFound = false;
    this.masterModel.utilizationRate = this.model.utilizationRate;
    this.reportService.getInspectionOccupancySummary(this.model)
      .subscribe(
        response => {
          if (response && response.result === ResponseResult.Success) {
            this.mapPageProperties(response);
            this.model.items = response.data;
            this.masterModel.statusList = response.statusList;
          }
          else if (response && response.result === ResponseResult.NoDataFound) {
            this.model.noFound = true;
            this.model.items = [];
          }
          else {
            this.error = response.result;
          }
          this.searchLoading = false;
        },
        error => {
          this.setError(error);
          this.searchLoading = false;
        });
  }

  getPathDetails(): string {
    return "";
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  getOfficeList() {
    this.masterModel.officeLoading = true;
    this.referenceService.getOfficeList()
      .subscribe(res => {
        if (res.result === ResponseResult.Success) {
          this.masterModel.officeList = res.dataSourceList;
        }
        this.masterModel.officeLoading = false;
      })
  }

  getOfficeCountryList() {
    this.masterModel.officeCountryLoading = true;
    this.locationService.getOfficeCountryDataSourceList(this.masterModel.officeCountryRequest).subscribe(res => {
      if (res.length > 0) {
        this.masterModel.officeCountryList = res;
      }
      this.masterModel.officeCountryLoading = false;
    })
  }

  getEmployeeTypeList() {
    this.masterModel.employeeTypeLoading = true;
    this.financeDashboardService.getEmployeeTypes().subscribe(res => {
      if (res.result === ResponseResult.Success) {
        this.masterModel.employeeTypeList = res.dataSourceList;
      }
      this.masterModel.employeeTypeLoading = false;
    })
  }

  getOutSourceCompanyList() {
    this.masterModel.outSourceCompanyLoading = true;
    this.hrService.getHROutSourceCompanyDataList().subscribe(res => {
      if (res.result === ResponseResult.Success) {
        this.masterModel.outSourceCompanyList = res.hrOutSourceCompanyList;
      }
      this.masterModel.outSourceCompanyLoading = false;
    })
  }

  getStaffListBySearch() {

    if (this.model.employeeType && this.model.employeeType > 0) {
      this.masterModel.requestStaffDataSource.employeeType = this.model.employeeType;
    }
    else {
      this.masterModel.requestStaffDataSource.employeeType = null;
    }

    if (this.model.outSourceCompany && this.model.outSourceCompany > 0) {
      this.masterModel.requestStaffDataSource.outSourceCompanyId = this.model.outSourceCompany;
    }
    else {
      this.masterModel.requestStaffDataSource.outSourceCompanyId = null;
    }

    this.masterModel.staffInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => {
        this.masterModel.staffLoading = true
      }),
      switchMap(term =>
        term ? this.hrService.getQCDataSource(this.masterModel.requestStaffDataSource, term)
          : this.hrService.getQCDataSource(this.masterModel.requestStaffDataSource)
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => {
                this.masterModel.staffLoading = false
              }))
      ))
      .subscribe(data => {
        this.masterModel.staffList = data;
        this.masterModel.staffLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getStaffData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.masterModel.requestStaffDataSource.searchText = this.masterModel.staffInput.getValue();
      this.masterModel.requestStaffDataSource.skip = this.masterModel.staffList.length;
      this.masterModel.staffLoading = true;
    }
    this.hrService.getQCDataSource(this.masterModel.requestStaffDataSource).
      subscribe(response => {
        if (IsVirtual) {
          this.masterModel.requestStaffDataSource.skip = 0;
          this.masterModel.requestStaffDataSource.take = ListSize;
          if (response && response.length > 0) {
            this.masterModel.staffList =
              this.masterModel.staffList.concat(response);
          }
          this.masterModel.staffLoading = false;
        }

      }),
      (error) => {
        this.masterModel.staffLoading = false;
        this.setError(error);
      };
  }

  onChangeEmployeeType() {
    if (this.model.employeeType === EmployeeType.Outsource) {
      this.getOutSourceCompanyList();
    }
    else {
      this.model.outSourceCompany = null;
    }
    this.model.qA = null;
    this.getStaffListBySearch();
  }
  export() {
    this.masterModel.exportLoading = true;


    this.reportService.exportInspectionOccupancy(this.model)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Inspection_Occupancy.xlsx");
        this.masterModel.exportLoading = false;
      },
        error => {
          this.masterModel.exportLoading = false;
        });
  }

  getStatusColor(statusId) {
    if (this.masterModel.statusList != null && this.masterModel.statusList.length > 0 && statusId != null) {
      var result = this.masterModel.statusList.find(x => x.inspectionOccupancyCategory === statusId);
      if (result)
        return result.color;
    }
  }

  async downloadFile(data: BlobPart, mimeType: string, fileName: string) {
    const blob = new Blob([data], { type: mimeType });
    let windowNavigator: any = window.Navigator;
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, fileName);
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fileName;
      a.href = url;
      a.click();
    }
  }

  //clear date input
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "FromDate": {
        this.model.fromDate = null;
        break;
      }
      case "ToDate": {
        this.model.toDate = null;
        break;
      }
    }
  }

  //when select or unselect the inspection occupancy status
  filterInspectionOccupancyCategory(inspectionOccupancyCategory) {
    if (inspectionOccupancyCategory && inspectionOccupancyCategory > 0) {

      //if it contains the value 
      var isValueExists = this.model.inspectionOccupancyCategories.includes(inspectionOccupancyCategory);

      //if category unselect
      if (isValueExists) {
        //remove from the array
        this.model.inspectionOccupancyCategories = this.model.inspectionOccupancyCategories.filter(x => x != inspectionOccupancyCategory);
        this.model.inspectionOccupancyCategories = [...this.model.inspectionOccupancyCategories];

      }
      else {
        //push the array
        this.model.inspectionOccupancyCategories.push(inspectionOccupancyCategory);
        this.model.inspectionOccupancyCategories = [...this.model.inspectionOccupancyCategories];
      }
    }
  }
  //check inspection occupancy category selected or not
  checkInspectionOccupancyCategory(id) {
    return this.model.inspectionOccupancyCategories.includes(id);
  }

  //reset the filter
  reset() {
    this.onInit();
    this.validator.isSubmitted = false;
    this.model = new InspectionOccupancyModel();
  }

  clearStaff() {
    this.masterModel.requestStaffDataSource = new StaffDataSourceRequest();
    this.getStaffListBySearch();
  }

  clearEmployeeType() {
    this.model.qA = null;
  }

  changeOutsourceCompany() {
    this.model.qA = null;
    this.getStaffListBySearch();
  }
}

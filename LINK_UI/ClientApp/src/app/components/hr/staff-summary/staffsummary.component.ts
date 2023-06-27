import { Component, NgModule, OnChanges, SimpleChanges, OnInit  } from '@angular/core';
import { HrService } from '../../../_Services/hr/hr.service'
import { StaffSummaryModel, StaffSummaryItemModel, StaffToRemove, StaffSummaryMasterModel } from '../../../_Models/hr/staffsummary.model'
import { first, switchMap } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Validator } from '../../common/validator'
import { Router, ActivatedRoute } from '@angular/router';
import { SummaryComponent } from '../../common/summary.component';
import { TranslateService } from '@ngx-translate/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { HRStaffStatus } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-staffSummary',
  templateUrl: './staffsummary.component.html',
  styleUrls: ['./staffsummary.component.css'],
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

export class StaffSummaryComponent extends SummaryComponent<StaffSummaryModel> {

  public data: any;
  loading = true;
  error = '';
  public exportDataLoading = false;
  public model: StaffSummaryModel;
  public modelRemove: StaffToRemove; 
  masterModel: StaffSummaryMasterModel;
  private modelRef: NgbModalRef;
  // public searchIsLeft = false; 
  public isStaffDetails: boolean = false;
  idCurrentStaff: number; 
  public _searchloader:boolean=false;
  isFilterOpen: boolean;
  toggleFormSection: boolean;
  public onJobStatus=HRStaffStatus.filter(a => a.id == 1)[0].id;
  constructor(private service: HrService, private modalService: NgbModal, public validator: Validator, 
    translate: TranslateService,router: Router, route: ActivatedRoute,public utility: UtilityService) {
    super(router, validator, route,translate);

    this.model = new StaffSummaryModel();
    this.modelRemove=new StaffToRemove();
    this.masterModel = new StaffSummaryMasterModel();
    this.validator.setJSON("hr/remove-staff.valid.json");
    this.validator.setModelAsync(() => this.modelRemove);
    this.validator.isSubmitted = false;
    this.isFilterOpen = true;
    this.toggleFormSection = false;
  }

  onInit() {
    this.getStatusList();

    this.data = this.service.getStaffSummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.loading = false;
          }
          else {
            this.error = data.result;
            this.loading = false;
          }
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getPathDetails() { return "staffedit/edit-staff" };

  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  getData() {
    this._searchloader=true;
    // this.searchIsLeft=this.model.isLeft?true:false;
    this.service.getDataSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {

            this.mapPageProperties(response); 

            this.model.items = response.data.map((x) => {

              var tabItem: StaffSummaryItemModel = {
                id: x.id,
                countryName: x.countryName,
                staffName: x.staffName,
                positionName: x.positionName,
                departmentName: x.departmentName,
                officeName: x.officeName,
                joinDate: x.joinDate,
                employeeType: x.employeeType,
                isChecked: false,
                statusName: x.statusName,
                statusColor: HRStaffStatus.filter(a => a.id == x.statusId) && HRStaffStatus.filter(a => a.id == x.statusId).length > 0 ?
                HRStaffStatus.find(a => a.id == x.statusId).color : "",
                statusId:x.statusId
              }

              return tabItem;
            });
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
          this._searchloader=false;
        },
        error => {
          this.setError(error);
          this.loading = false;
          this._searchloader=false;
        });
  }

  openConfirm(id, name, content) {
   // this.validator.isSubmitted = false;
    this.modelRemove = {
      id: id,
      staffName: name ,
      leaveDate: null ,
      reason: "",
      statusId: null
    };
    this.modelRef = this.modalService.open(content, { windowClass : "mdModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }
  deleteStaff(item: StaffToRemove) {

    this.validator.isSubmitted = true;

    if (this.validator.isFormValid()) {
      this.service.deleteStaff(item)
        .pipe()
        .subscribe(
          response => {
            if (response && (response.result == 1)) {
              // refresh
              this.refresh();
            }
            if (response && (response.result == 3)) {
              this.showError('STAFF_SUMMARY.TITLE', 'STAFF_SUMMARY.MSG_USER_ACCOUNT_NOT_DELETE');
            }
            else {
              this.error = response.result;

              this.loading = false;
              // TODO check error from result
            }
          },
          error => {
            this.error = error;
            this.loading = false;
        });

      this.modelRef.close();
    }
  }

  return(isDetails) {
    this.isStaffDetails = isDetails;
  }


  addStaff() {
    this.isStaffDetails = true;
    this.idCurrentStaff = null;
  }

  export() {
    this.exportDataLoading = true;
    let currentModel: StaffSummaryModel = {
      countryValues: this.model.countryValues,
      departmentValues: this.model.departmentValues,
      employeeNumber: this.model.employeeNumber,
      employeeTypeValues: this.model.employeeTypeValues,
      // isLeft: this.model.isLeft,
      positionValues: this.model.positionValues,
      staffName: this.model.staffName,
      officeValues: this.model.officeValues,
      index: 1,
      items: [],
      noFound: false,
      pageCount: 0,
      pageSize: 99999,
      totalCount : 0 
    }

    this.service.exportStaff(currentModel)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }


  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }

    this.exportDataLoading = false;
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }

  getStatusList() {
    this.masterModel.statusLoading = true;
    this.service.getStatusList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.statusList = response.dataSourceList;
            //remove the "on job" status in delete pop up
            this.masterModel.statusList.forEach( (item, index) => {
              if(item.id === this.onJobStatus) this.masterModel.statusList.splice(index,1);
            });

          this.masterModel.statusLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.statusLoading = false;
        });
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, CountryDataSourceRequest, QcDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { OtherMandayEditModel, OtherMandayEditSummaryModel, OtherMandayModel, OtherMandayResult, OtherMandaySummaryModel } from 'src/app/_Models/otherManday/otherManday.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { OtherMandayService } from 'src/app/_Services/otherManday/otherManday.service';
import { ScheduleService } from 'src/app/_Services/Schedule/schedule.service';
import { ListSize, PageSizeCommon } from '../common/static-data-common';
import { SummaryComponent } from '../common/summary.component';
import { Validator } from '../common/validator';

@Component({
  selector: 'app-other-manday',
  templateUrl: './other-manday.component.html',
  styleUrls: ['./other-manday.component.scss']
})
export class OtherMandayComponent extends SummaryComponent<OtherMandayModel> {

  public componentDestroyed$: Subject<boolean> = new Subject;
  public summaryModel: OtherMandaySummaryModel;
  public model: OtherMandayModel;
  public isFilterOpen: boolean;
  public toggleFormSection: boolean = false;
  public editSummaryModel: OtherMandayEditSummaryModel;
  public editModel: OtherMandayEditModel;
  public modelRef: NgbModalRef;
  public idToDelete: number;
  public pagesizeitems = PageSizeCommon;
  isEditManday: false;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService, public summaryValidator: Validator,
    toastr: ToastrService, private customerService: CustomerService, private locationService: LocationService,
    private service: OtherMandayService, public modalService: NgbModal, private serviceSCH: ScheduleService) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.initialize();
  }

  getData(): void {
    this.getSearchData(false);
  }
  getPathDetails(): string {
    return "";
  }

  initialize() {
    this.summaryModel = new OtherMandaySummaryModel();
    this.model = new OtherMandayModel();

    this.validator.setJSON("otherManday/other-manday-summary.valid.json");
    this.validator.setModelAsync(() => this.model);

    // this.summaryValidator.setJSON("otherManday/other-manday-summary.valid.json");
    // this.summaryValidator.setModelAsync(() => this.model);


    this.getCustomerListBySearch();
    this.getOfficeCountryListBySearch();
    this.getOperationalCountryListBySearch();
    this.getQcListBySearch();
  }

  SearchDetails(fromEdit: boolean) {
    this.model.pageSize = this.summaryModel.selectedPageSize;
    this.model.index = 1;
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.getSearchData(fromEdit);

  }
  getCustomerListBySearch() {

    this.summaryModel.customerRequest = new CommonDataSourceRequest();
    //push the customerid to  customer id list 
    if (this.model.customerId) {
      this.summaryModel.customerRequest.id = this.model.customerId;
    }
    else {
      this.summaryModel.customerRequest.idList = null;
    }

    this.summaryModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.summaryModel.customerList = data;
        this.summaryModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.summaryModel.customerRequest.searchText = this.summaryModel.customerInput.getValue();
      this.summaryModel.customerRequest.skip = this.summaryModel.customerList.length;
    }

    this.summaryModel.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.summaryModel.customerList = this.summaryModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.summaryModel.customerRequest.skip = 0;
          this.summaryModel.customerRequest.take = ListSize;
        }
        this.summaryModel.customerLoading = false;
      }),
      error => {
        this.summaryModel.customerLoading = false;
        this.setError(error);
      };
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  getOperationalCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.summaryModel.operationalCountryRequest.searchText = this.summaryModel.operationalCountryInput.getValue();
      this.summaryModel.operationalCountryRequest.skip = this.summaryModel.operationalCountryList.length;
    }

    this.summaryModel.operationalCountryLoading = true;
    this.locationService.getCountryDataSourceList(this.summaryModel.operationalCountryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.operationalCountryList = this.summaryModel.operationalCountryList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.summaryModel.operationalCountryRequest.skip = 0;
          this.summaryModel.operationalCountryRequest.take = ListSize;
        }
        this.summaryModel.operationalCountryLoading = false;
      }),
      error => {
        this.summaryModel.operationalCountryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getOperationalCountryListBySearch() {
    this.summaryModel.operationalCountryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.operationalCountryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.summaryModel.operationalCountryRequest, term)
        : this.locationService.getCountryDataSourceList(this.summaryModel.operationalCountryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.operationalCountryLoading = false))
      ))
      .subscribe(data => {
        this.summaryModel.operationalCountryList = data;
        this.summaryModel.operationalCountryLoading = false;
      });
  }

  getOfficeCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.summaryModel.officeCountryRequest.searchText = this.summaryModel.officeCountryInput.getValue();
      this.summaryModel.officeCountryRequest.skip = this.summaryModel.officeCountryList.length;
    }

    this.summaryModel.officeCountryLoading = true;
    this.locationService.getOfficeCountryDataSourceList(this.summaryModel.officeCountryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.officeCountryList = this.summaryModel.officeCountryList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.summaryModel.officeCountryRequest.skip = 0;
          this.summaryModel.officeCountryRequest.take = ListSize;
        }
        this.summaryModel.officeCountryLoading = false;
      }),
      error => {
        this.summaryModel.officeCountryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getOfficeCountryListBySearch() {
    this.summaryModel.officeCountryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.officeCountryLoading = true),
      switchMap(term => term
        ? this.locationService.getOfficeCountryDataSourceList(this.summaryModel.officeCountryRequest, term)
        : this.locationService.getOfficeCountryDataSourceList(this.summaryModel.officeCountryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.officeCountryLoading = false))
      ))
      .subscribe(data => {
        this.summaryModel.officeCountryList = data;
        this.summaryModel.officeCountryLoading = false;
      });
  }

  openNewPopUp(content, isEdit) {
    this.isEditManday = isEdit;
    this.validator.initTost();
    this.validator.isSubmitted = false;
    this.validator.setJSON("otherManday/add-other-manday.valid.json");
    this.validator.setModelAsync(() => this.editModel);

    this.editSummaryModel = new OtherMandayEditSummaryModel();
    this.editModel = new OtherMandayEditModel();

    if (!isEdit) {
      this.getEditCustomerListBySearch();
      this.getEditOfficeCountryListBySearch();
      this.getEditOperationalCountryListBySearch();
      this.getEditQcListBySearch();
    }
    this.getPurposeList();

    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  async getSearchData(fromEdit: boolean) {

    this.validator.setModelAsync(() => this.model);
    this.validator.setJSON("otherManday/other-manday-summary.valid.json");
    this.validator.isSubmitted = !fromEdit;
    if (this.summaryFormValid()) {
      this.summaryModel.searchloading = true;

      let res = await this.service.getOtherMandaySummary(this.model);

      if (res.result == ResponseResult.Success) {
        this.model.noFound = false;
        this.mapPageProperties(res);
        this.model.items = res.data;
      }
      else {
        this.model.items = [];
        this.model.noFound = true;
      }
      this.summaryModel.searchloading = false;
    }
  }

  summaryFormValid() {
    let isOk = false;
    isOk = this.validator.isValid("serviceFromDate")
      && this.validator.isValid("serviceToDate")
    return isOk;
  }

  getEditCustomerListBySearch() {

    this.editSummaryModel.customerRequest = new CommonDataSourceRequest();
    //push the customerid to  customer id list 
    if (this.editModel.customerId) {
      this.editSummaryModel.customerRequest.id = this.editModel.customerId;
    }
    else {
      this.editSummaryModel.customerRequest.idList = null;
    }

    this.editSummaryModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editSummaryModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.editSummaryModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.editSummaryModel.customerRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.editSummaryModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.editSummaryModel.customerList = data;
        this.editSummaryModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getEditCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.editSummaryModel.customerRequest.searchText = this.editSummaryModel.customerInput.getValue();
      this.editSummaryModel.customerRequest.skip = this.editSummaryModel.customerList.length;
    }

    this.editSummaryModel.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.editSummaryModel.customerRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.editSummaryModel.customerList = this.editSummaryModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.editSummaryModel.customerRequest.skip = 0;
          this.editSummaryModel.customerRequest.take = ListSize;
        }
        this.editSummaryModel.customerLoading = false;
      }),
      error => {
        this.editSummaryModel.customerLoading = false;
        this.setError(error);
      };
  }

  getEditOperationalCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.editSummaryModel.operationalCountryRequest.searchText = this.editSummaryModel.operationalCountryInput.getValue();
      this.editSummaryModel.operationalCountryRequest.skip = this.editSummaryModel.operationalCountryList.length;
    }

    this.editSummaryModel.operationalCountryLoading = true;
    this.locationService.getCountryDataSourceList(this.editSummaryModel.operationalCountryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.editSummaryModel.operationalCountryList = this.editSummaryModel.operationalCountryList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.editSummaryModel.operationalCountryRequest.skip = 0;
          this.editSummaryModel.operationalCountryRequest.take = ListSize;
        }
        this.editSummaryModel.operationalCountryLoading = false;
      }),
      error => {
        this.editSummaryModel.operationalCountryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getEditOperationalCountryListBySearch() {
    this.editSummaryModel.operationalCountryRequest = new CountryDataSourceRequest();

    if (this.editModel.operationalCountryId > 0) {
      this.editSummaryModel.operationalCountryRequest.countryIds.push(this.editModel.operationalCountryId);
    }

    this.editSummaryModel.operationalCountryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editSummaryModel.operationalCountryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.editSummaryModel.operationalCountryRequest, term)
        : this.locationService.getCountryDataSourceList(this.editSummaryModel.operationalCountryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.editSummaryModel.operationalCountryLoading = false))
      ))
      .subscribe(data => {
        this.editSummaryModel.operationalCountryList = data;
        this.editSummaryModel.operationalCountryLoading = false;
      });
  }

  getEditOfficeCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.editSummaryModel.officeCountryRequest.searchText = this.editSummaryModel.officeCountryInput.getValue();
      this.editSummaryModel.officeCountryRequest.skip = this.editSummaryModel.officeCountryList.length;
    }

    this.editSummaryModel.officeCountryLoading = true;
    this.locationService.getOfficeCountryDataSourceList(this.editSummaryModel.officeCountryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.editSummaryModel.officeCountryList = this.editSummaryModel.officeCountryList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.editSummaryModel.officeCountryRequest.skip = 0;
          this.editSummaryModel.officeCountryRequest.take = ListSize;
        }
        this.editSummaryModel.officeCountryLoading = false;
      }),
      error => {
        this.editSummaryModel.officeCountryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getEditOfficeCountryListBySearch() {
    this.editSummaryModel.officeCountryRequest = new CountryDataSourceRequest();

    if (this.editModel.officeCountryId > 0) {
      this.editSummaryModel.officeCountryRequest.countryIds.push(this.editModel.officeCountryId);
    }
    this.editSummaryModel.officeCountryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editSummaryModel.officeCountryLoading = true),
      switchMap(term => term
        ? this.locationService.getOfficeCountryDataSourceList(this.editSummaryModel.officeCountryRequest, term)
        : this.locationService.getOfficeCountryDataSourceList(this.editSummaryModel.officeCountryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.editSummaryModel.officeCountryLoading = false))
      ))
      .subscribe(data => {
        this.editSummaryModel.officeCountryList = data;
        this.editSummaryModel.officeCountryLoading = false;
      });
  }
  //fetch the Qc names data with virtual scroll
  getEditQcData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.editSummaryModel.qcRequest.searchText = this.editSummaryModel.qcInput.getValue();
      this.editSummaryModel.qcRequest.skip = this.editSummaryModel.qcList.length;
    }

    this.editSummaryModel.qcLoading = true;
    this.serviceSCH.getQcDataSourceList(this
      .editSummaryModel.qcRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.editSummaryModel.qcList = this.editSummaryModel.qcList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.editSummaryModel.qcRequest.skip = 0;
          this.editSummaryModel.qcRequest.take = ListSize;
        }
        this.editSummaryModel.qcLoading = false;
      }),
      error => {
        this.editSummaryModel.qcLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 Qc on load
  getEditQcListBySearch() {
    this.editSummaryModel.qcRequest = new QcDataSourceRequest();
    if (this.editModel.officeCountryId)
      this.editSummaryModel.qcRequest.officeCountryIds.push(this.editModel.officeCountryId);
    if (this.editModel.qcId)
      this.editSummaryModel.qcRequest.qcIds.push(this.editModel.qcId);
    this.editSummaryModel.qcInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editSummaryModel.qcLoading = true),
      switchMap(term => term
        ? this.serviceSCH.getQcDataSourceList(this.editSummaryModel.qcRequest, term)
        : this.serviceSCH.getQcDataSourceList(this.editSummaryModel.qcRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.editSummaryModel.qcLoading = false))
      ))
      .subscribe(data => {
        this.editSummaryModel.qcList = data;
        this.editSummaryModel.qcLoading = false;
      });
  }

  //fetch the Qc names data with virtual scroll
  getQcData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.summaryModel.qcRequest.searchText = this.summaryModel.qcInput.getValue();
      this.summaryModel.qcRequest.skip = this.summaryModel.qcList.length;
    }

    this.summaryModel.qcLoading = true;
    this.serviceSCH.getQcDataSourceList(this
      .summaryModel.qcRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.qcList = this.summaryModel.qcList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.summaryModel.qcRequest.skip = 0;
          this.summaryModel.qcRequest.take = ListSize;
        }
        this.summaryModel.qcLoading = false;
      }),
      error => {
        this.summaryModel.qcLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 Qc on load
  getQcListBySearch() {
    this.summaryModel.qcRequest = new QcDataSourceRequest();
    if (this.model.officeCountryId)
      this.summaryModel.qcRequest.officeCountryIds.push(this.model.officeCountryId);
    this.summaryModel.qcInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.qcLoading = true),
      switchMap(term => term
        ? this.serviceSCH.getQcDataSourceList(this.summaryModel.qcRequest, term)
        : this.serviceSCH.getQcDataSourceList(this.summaryModel.qcRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.qcLoading = false))
      ))
      .subscribe(data => {
        this.summaryModel.qcList = data;
        this.summaryModel.qcLoading = false;
      });
  }

  editOfficeCountryChange() {
    this.editModel.qcId = null;
    this.getEditQcListBySearch();
  }

  async save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      if (this.editModel.manday > 0) {
        this.editSummaryModel.saveloading = true;

        let res = await this.service.saveOtherManday(this.editModel);

        if (res.result == OtherMandayResult.Success) {
          this.showSuccess('OTHER_MANDAY.LBL_TITLE', 'COMMON.MSG_SAVED_SUCCESS');

        }
        else if (res.result == OtherMandayResult.AlreadyExists) {
          this.showError('OTHER_MANDAY.LBL_TITLE', 'OTHER_MANDAY.MSG_ALREADY_EXISTS');
        }
        else {
          this.showError('OTHER_MANDAY.LBL_TITLE', 'OTHER_MANDAY.LBL_SAVE_FAILED');
        }
        this.editSummaryModel.saveloading = false;
        this.modelRef.close();
        this.validator.isSubmitted = false;
        //this.validator.initTost();  
        this.validator.setModelAsync(() => this.model);
        this.validator.setJSON("otherManday/other-manday-summary.valid.json");
        if (this.model && this.model.serviceFromDate)
          this.SearchDetails(true);
      } else {
        this.showWarning("OTHER_MANDAY.LBL_TITLE", 'OTHER_MANDAY.MSG_MANDAY_GREATER_THAN_ZERO');
      }
    }
  }

  formValid() {
    let isOk = this.validator.isValid("officeCountryId") &&
      this.validator.isValid("qcId") &&
      this.validator.isValid("operationalCountryId") &&
      this.validator.isValid("purposeId") &&
      this.validator.isValid("serviceDate") &&
      this.validator.isValid("manday")

    return isOk;
  }

  async getPurposeList() {
    let res = await this.service.getPurposeList();

    if (res.result == ResponseResult.Success) {
      this.editSummaryModel.purposeList = res.dataSourceList;
    }
    else {
      this.editSummaryModel.purposeList = [];
    }
  }

  async editOtherManday(id, content) {
    let res = await this.service.editOtherManday(id)
    if (res.result == ResponseResult.Success) {
      this.openNewPopUp(content, true);
      this.mapEditData(res.data);
      this.getEditCustomerListBySearch();
      this.getEditOfficeCountryListBySearch();
      this.getEditOperationalCountryListBySearch();
      this.getEditQcListBySearch();
    }
    else {
      this.model.items = [];
      this.model.noFound = true;
    }
    this.editSummaryModel.saveloading = false;

  }

  mapEditData(item) {
    this.editModel = {
      id: item.id,
      officeCountryId: item.officeCountryId,
      customerId: item.customerId > 0 ? item.customerId : null,
      qcId: item.qcId,
      operationalCountryId: item.operationalCountryId,
      purposeId: item.purposeId,
      serviceDate: item.serviceDateObject,
      manday: item.manday,
      remarks: item.remarks
    };
  }

  openDeletePopUp(content, id) {
    this.idToDelete = id;
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
  }

  async delete() {
    this.editSummaryModel.deleteLoading = true;

    let res = await this.service.deleteOtherManday(this.idToDelete);
    if (res.result == OtherMandayResult.Success) {
      this.showSuccess('OTHER_MANDAY.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
    }
    else {
      this.showError('OTHER_MANDAY.LBL_TITLE', 'OTHER_MANDAY.MSG_DELETE_FAILEd');
    }
    this.editSummaryModel.deleteLoading = false;
    this.modelRef.close();
    this.SearchDetails(false);
  }

  cancel() {
    this.validator.isSubmitted = false;
    this.validator.initTost();
    this.validator.setJSON("otherManday/other-manday-summary.valid.json");
    this.validator.setModelAsync(() => this.model);

    this.modelRef.close();
  }

  deleteWorks() {
    this.delete();
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "serviceFromDate": {
        this.model.serviceFromDate = null;
        break;
      }
      case "serviceToDate": {
        this.model.serviceToDate = null;
        break;
      }
      case "serviceDate": {
        this.editModel.serviceDate = null;
        break;
      }
    }
  }

  officeCountryChange() {
    this.model.qcId = null;
    this.getQcListBySearch();
  }

  reset() {
    this.validator.isSubmitted = false;
    this.model = new OtherMandayModel();
  }

  export() {
    this.summaryModel.exportLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.summaryModel.exportLoading = false;
        });
  }


  downloadFile(data, mimeType) {
    let windowNavigator: any = window.Navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "Other_Manday_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Other_Manday_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.summaryModel.exportLoading = false;
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { InvoiceDataAccessEditModel, InvoiceDataAccessModel, InvoiceDataAccessMasterModel, InvoiceDataAccessEditMasterModel, InvoiceDataAccessResult } from 'src/app/_Models/invoice/invoice-data-access.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { InoiceDataAccessService } from 'src/app/_Services/invoice/invoicedataaccess.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ScheduleService } from 'src/app/_Services/Schedule/schedule.service';
import { ListSize, PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from '../../common/validator';

@Component({
  selector: 'app-invoice-data-access',
  templateUrl: './invoice-data-access.component.html',
  styleUrls: ['./invoice-data-access.component.scss']
})
export class InvoiceDataAccessComponent extends SummaryComponent<InvoiceDataAccessModel> {

  public componentDestroyed$: Subject<boolean> = new Subject;
  public summaryMasterModel: InvoiceDataAccessMasterModel;
  public model: InvoiceDataAccessModel;
  public isFilterOpen: boolean;
  public toggleFormSection: boolean = false;
  public editModel: InvoiceDataAccessEditModel;
  public editMasterModel: InvoiceDataAccessEditMasterModel;
  public modelRef: NgbModalRef;
  public idToDelete: number;
  public pagesizeitems = PageSizeCommon;
  isEditInvDataAccess: false;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService, public summaryValidator: Validator,
    toastr: ToastrService, private customerService: CustomerService, private locationService: LocationService, private referenceService: ReferenceService,
    private service: InoiceDataAccessService, public modalService: NgbModal, private serviceSCH: ScheduleService) {
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
    this.summaryMasterModel = new InvoiceDataAccessMasterModel();
    this.model = new InvoiceDataAccessModel();

    this.editMasterModel = new InvoiceDataAccessEditMasterModel();
    this.editModel = new InvoiceDataAccessEditModel();

    this.getCustomerListBySearch();
    this.getStaffListBySearch();
    this.getOfficeList();
    this.getInvoiceTypeList();
  }

  SearchDetails(fromEdit: boolean) {
    this.model.pageSize = this.summaryMasterModel.selectedPageSize;
    this.model.index = 1;
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.getSearchData(fromEdit);

  }

  getInvoiceTypeList() {
    this.summaryMasterModel.invoiceTypeLoading = true;
    this.customerService.getInvoiceType()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.summaryMasterModel.invoiceTypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.summaryMasterModel.invoiceTypeLoading = false;

        },
        error => {
          this.summaryMasterModel.invoiceTypeLoading = false;
          this.setError(error);
        });
  }


  getStaffListBySearch() {

    this.summaryMasterModel.staffRequest = new CommonDataSourceRequest();

    if (this.model.staffId) {
      this.summaryMasterModel.staffRequest.id = this.model.staffId;
    }
    else {
      this.summaryMasterModel.staffRequest.id = null;
    }

    this.summaryMasterModel.staffInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryMasterModel.staffLoading = true),
      switchMap(term => term
        ? this.referenceService.getStaffList(this.summaryMasterModel.staffRequest, term)
        : this.referenceService.getStaffList(this.summaryMasterModel.staffRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])),
            tap(() => this.summaryMasterModel.staffLoading = false))
      ))
      .subscribe(data => {
        this.summaryMasterModel.staffList = data;
        this.summaryMasterModel.staffLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getStaffData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.summaryMasterModel.staffRequest.searchText = this.summaryMasterModel.staffInput.getValue();
      this.summaryMasterModel.staffRequest.skip = this.summaryMasterModel.staffList.length;
    }

    this.summaryMasterModel.staffLoading = true;
    this.referenceService.getStaffList(this.summaryMasterModel.staffRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(staffData => {

        if (staffData && staffData.length > 0) {
          this.summaryMasterModel.staffList = this.summaryMasterModel.staffList.concat(staffData);
        }
        if (isDefaultLoad) {
          this.summaryMasterModel.staffRequest.skip = 0;
          this.summaryMasterModel.staffRequest.take = ListSize;
        }
        this.summaryMasterModel.staffLoading = false;
      }),
      error => {
        this.summaryMasterModel.staffLoading = false;
        this.setError(error);
      };
  }

  getCustomerListBySearch() {

    this.summaryMasterModel.customerRequest = new CommonDataSourceRequest();
    //push the customerid to  customer id list 
    if (this.model.customerIdList) {
      this.summaryMasterModel.customerRequest.idList = this.model.customerIdList;
    }
    else {
      this.summaryMasterModel.customerRequest.idList = null;
    }

    this.summaryMasterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryMasterModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.summaryMasterModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.summaryMasterModel.customerRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryMasterModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.summaryMasterModel.customerList = data;
        this.summaryMasterModel.customerLoading = false;
      });
  }

  //get office list
  getOfficeList() {
    this.summaryMasterModel.officeLoading = true;
    this.referenceService.getOfficeList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryMasterModel.officeList = response.dataSourceList;
          this.summaryMasterModel.officeLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryMasterModel.officeLoading = false;
        });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.summaryMasterModel.customerRequest.searchText = this.summaryMasterModel.customerInput.getValue();
      this.summaryMasterModel.customerRequest.skip = this.summaryMasterModel.customerList.length;
    }

    this.summaryMasterModel.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.summaryMasterModel.customerRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.summaryMasterModel.customerList = this.summaryMasterModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.summaryMasterModel.customerRequest.skip = 0;
          this.summaryMasterModel.customerRequest.take = ListSize;
        }
        this.summaryMasterModel.customerLoading = false;
      }),
      error => {
        this.summaryMasterModel.customerLoading = false;
        this.setError(error);
      };
  }


  // Edit part load

  getEditInvoiceTypeList() {
    this.editMasterModel.invoiceTypeLoading = true;
    this.customerService.getInvoiceType()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.editMasterModel.invoiceTypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.editMasterModel.invoiceTypeLoading = false;

        },
        error => {
          this.editMasterModel.invoiceTypeLoading = false;
          this.setError(error);
        });
  }


  getEditStaffListBySearch() {

    this.editMasterModel.staffRequest = new CommonDataSourceRequest();

    if (this.editModel.staffId) {
      this.editMasterModel.staffRequest.id = this.editModel.staffId;
    }
    else {
      this.editMasterModel.staffRequest.idList = null;
    }

    this.editMasterModel.staffInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editMasterModel.staffLoading = true),
      switchMap(term => term
        ? this.referenceService.getStaffList(this.editMasterModel.staffRequest, term)
        : this.referenceService.getStaffList(this.editMasterModel.staffRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])),
            tap(() => this.editMasterModel.staffLoading = false))
      ))
      .subscribe(data => {
        this.editMasterModel.staffList = data;
        this.editMasterModel.staffLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getEditStaffData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.editMasterModel.staffRequest.searchText = this.editMasterModel.staffInput.getValue();
      this.editMasterModel.staffRequest.skip = this.editMasterModel.staffList.length;
    }

    this.editMasterModel.staffLoading = true;
    this.referenceService.getStaffList(this.editMasterModel.staffRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(staffData => {

        if (staffData && staffData.length > 0) {
          this.editMasterModel.staffList = this.editMasterModel.staffList.concat(staffData);
        }
        if (isDefaultLoad) {
          this.editMasterModel.staffRequest.skip = 0;
          this.editMasterModel.staffRequest.take = ListSize;
        }
        this.editMasterModel.staffLoading = false;
      }),
      error => {
        this.editMasterModel.staffLoading = false;
        this.setError(error);
      };
  }

  getEditCustomerListBySearch() {

    this.editMasterModel.customerRequest = new CommonDataSourceRequest();
    if (this.editModel.customerIdList) {
      this.editMasterModel.customerRequest.idList = this.editModel.customerIdList;
    }
    else {
      this.editMasterModel.customerRequest.idList = null;
    }

    this.editMasterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editMasterModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.editMasterModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.editMasterModel.customerRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.editMasterModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.editMasterModel.customerList = data;
        this.editMasterModel.customerLoading = false;
      });
  }

  //get office list
  getEditOfficeList() {
    this.editMasterModel.officeLoading = true;
    this.referenceService.getOfficeList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.editMasterModel.officeList = response.dataSourceList;
          this.editMasterModel.officeLoading = false;

        },
        error => {
          this.setError(error);
          this.editMasterModel.officeLoading = false;
        });
  }

  clearEditStaff() {
    this.editModel.staffId = null;
    this.getEditStaffListBySearch();
  }

  clearEditCustomer() {
    this.editModel.customerIdList = [];
    this.getEditCustomerListBySearch();
  }

  //fetch the customer data with virtual scroll
  getEditCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.editMasterModel.customerRequest.searchText = this.editMasterModel.customerInput.getValue();
      this.editMasterModel.customerRequest.skip = this.editMasterModel.customerList.length;
    }

    this.editMasterModel.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.editMasterModel.customerRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.editMasterModel.customerList = this.editMasterModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.editMasterModel.customerRequest.skip = 0;
          this.editMasterModel.customerRequest.take = ListSize;
        }
        this.editMasterModel.customerLoading = false;
      }),
      error => {
        this.editMasterModel.customerLoading = false;
        this.setError(error);
      };
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }


  openNewPopUp(content, isEdit) {
    this.isEditInvDataAccess = isEdit;
    this.validator.initTost();
    this.validator.isSubmitted = false;
    this.validator.setJSON("invoice/invoice-data-access.valid.json");
    this.validator.setModelAsync(() => this.editModel);

    this.editMasterModel = new InvoiceDataAccessEditMasterModel();
    this.editModel = new InvoiceDataAccessEditModel();

    if (!isEdit) {
      this.getEditCustomerListBySearch();
      this.getEditInvoiceTypeList();
      this.getEditOfficeList();
      this.getEditStaffListBySearch();
    }

    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  async getSearchData(fromEdit: boolean) {
    this.summaryMasterModel.searchloading = true;
    let res = await this.service.getInvoiceDataAccessSummary(this.model);
    if (res.result == ResponseResult.Success) {
      this.model.noFound = false;
      this.mapPageProperties(res);
      this.model.items = res.invoiceDataAccessSummaryList;
    }
    else {
      this.model.items = [];
      this.model.noFound = true;
    }
    this.summaryMasterModel.searchloading = false;
  }


  async save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      if (this.editModel.staffId > 0) {
        this.editMasterModel.saveloading = true;

        let res = await this.service.save(this.editModel);

        this.editMasterModel.saveloading = false;

        if (res.result == InvoiceDataAccessResult.Success) {
          this.showSuccess('INVOICE_ACCESS.LBL_TITLE', 'COMMON.MSG_SAVED_SUCCESS');
          this.modelRef.close();
          this.validator.isSubmitted = false;
          this.SearchDetails(true);

        }
        else if (res.result == InvoiceDataAccessResult.Exists) {
          this.showError('INVOICE_ACCESS.LBL_TITLE', 'INVOICE_ACCESS.MSG_ALREADY_EXISTS');
        }
        else {
          this.showError('INVOICE_ACCESS.LBL_TITLE', 'OTHER_MANDAY.LBL_SAVE_FAILED');
        }      
 
      } else {
        this.showWarning("INVOICE_ACCESS.LBL_TITLE", 'INVOICE_ACCESS.MSG_STAFFID_GREATER_THAN_ZERO');
      }
    }
  }

  formValid() {
    let isOk = this.validator.isValid("staffId");
    return isOk;
  }


  async editInvoiceDataAccess(id, content) {
    let res = await this.service.edit(id)
    if (res.result == ResponseResult.Success) {
      this.openNewPopUp(content, true);
      this.mapEditData(res.invoiceDataAccess);
      this.getEditCustomerListBySearch();
      this.getEditInvoiceTypeList();
      this.getEditOfficeList();
      this.getEditStaffListBySearch();
    }
    else {
      this.model.items = [];
      this.model.noFound = true;
    }
    this.editMasterModel.saveloading = false;

  }

  mapEditData(item) {
    this.editModel = {
      id: item.id,
      staffId: item.staffId,
      customerIdList: item.customerList,
      invoiceTypeIdList: item.invoiceTypeList,
      officeIdList: item.officeList
    };
  }

  openDeletePopUp(content, id) {
    this.idToDelete = id;
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
  }

  async delete() {
    this.editMasterModel.deleteLoading = true;
    let res = await this.service.deleteInvoiceDataAccess(this.idToDelete);
    if (res == ResponseResult.Success) {
      this.showSuccess('INVOICE_ACCESS.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
    }
    else {
      this.showError('INVOICE_ACCESS.LBL_TITLE', 'OTHER_MANDAY.MSG_DELETE_FAILEd');
    }
    this.editMasterModel.deleteLoading = false;
    this.modelRef.close();
    this.SearchDetails(false);
  }

  cancel() {
    this.modelRef.close();
  }

  deleteWorks() {
    this.delete();
  }


  reset() {
    this.validator.isSubmitted = false;
    this.model = new InvoiceDataAccessModel();
  }

}

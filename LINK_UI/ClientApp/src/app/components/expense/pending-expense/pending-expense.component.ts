import { is } from '@amcharts/amcharts4/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Reference } from '@angular/compiler/src/render3/r3_ast';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, CountryDataSourceRequest, DataSource, QcDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { PendingExpenseModel, PendingExpenseStatusList, PendingExpenseTypeList } from 'src/app/_Models/expense/pendingexpense.model';
import { invoicedatetypelst, invoiceSearchtypelst } from 'src/app/_Models/invoice/invoicestatus.model';
import { ExpenseService } from 'src/app/_Services/expense/expense.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ScheduleService } from 'src/app/_Services/Schedule/schedule.service';
import { DefaultDateType, PageSizeCommon, SearchType } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from '../../common/validator';

@Component({
  selector: 'app-pending-expense',
  templateUrl: './pending-expense.component.html',
  styleUrls: ['./pending-expense.component.scss'],
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
export class PendingExpenseComponent extends SummaryComponent<PendingExpenseModel> {

  public componentDestroyed$: Subject<boolean> = new Subject;
  public model: PendingExpenseModel;
  public isFilterOpen: boolean;
  public toggleFormSection: boolean = false;
  public modelRef: NgbModalRef;
  public popUpOpen: boolean = false;
  public idToDelete: number;
  pagesizeitems = PageSizeCommon;
  searchLoading: boolean = false;
  saveLoading: boolean = false;
  isExpenseVisible: boolean = false;
  selectedPageSize: any;
  public qcRequest: QcDataSourceRequest;

  searchId = SearchType.BookingNo;
  _customValidationForInvoiceNo: boolean = false;
  selectAllCheckbox: boolean = false;

  invoiceSearchTypeList: any = invoiceSearchtypelst;
  invoiceDateTypeList: any = invoicedatetypelst;

  public qcList: any;
  qcListName: string;
  qcInput: BehaviorSubject<string>;
  qcLoading: boolean;

  officeList: Array<DataSource>;
  officeLoading: boolean;
  officeInput: BehaviorSubject<string>;
  officeModelRequest: CommonDataSourceRequest;

  statusList: any;
  expenseTypeList: any;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService, private refService: ReferenceService,
    toastr: ToastrService, private locationService: LocationService,
    private officeService: OfficeService,
    private serviceSCH: ScheduleService, private service: ExpenseService, public modalService: NgbModal) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
    // this.validator.setJSON("expense/pending-expense.valid.json");
    // this.validator.setModelAsync(() => this.model);
    // this.validator.isSubmitted = false;
  }

  onInit(): void {

    this.initialize();
    
  }

  getData(): void {
    this.getSearchData();
  }
  getPathDetails(): string {
    return "";
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  initialize() {
    this.model = new PendingExpenseModel();
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.searchTypeId = this.invoiceSearchTypeList[0].id;
    this.selectedPageSize = PageSizeCommon[0];
    this.qcInput = new BehaviorSubject<string>("");
    this.qcLoading = false;
    this.officeLoading = false;
    this.officeInput = new BehaviorSubject<string>("");
    this.qcRequest = new QcDataSourceRequest();
    this.officeModelRequest = new CommonDataSourceRequest();

    this.getOfficeListBySearch();
    this.getQcListBySearch();
    this.getStatusList();
    this.getExpenseTypeList();
  }

  getStatusList() {
    this.statusList = PendingExpenseStatusList;
  }

  getExpenseTypeList() {
    this.expenseTypeList = PendingExpenseTypeList;
  }


  IsDateValidationRequired(): boolean {

    let isOk = false;
    if (!this.model.startDate) {
      this.validator.isValid('startDate');
    }
    else if (this.model.startDate && !this.model.endDate) {
      this.validator.isValid('endDate');
    }
    return isOk;
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "startDate": {
        this.model.startDate = null;
        break;
      }
      case "endDate": {
        this.model.endDate = null;
        break;
      }
    }
  }

  SetSearchTypemodel(searchtype) {
    this.model.searchTypeId = searchtype;
  }

  BookingNoValidation() {
    return this._customValidationForInvoiceNo = this.model.searchTypeId == this.searchId
      && this.model.searchTypeText != null && this.model.searchTypeText.trim() != "" && isNaN(Number(this.model.searchTypeText));
  }

  //check box header
  changeCheckBoxSelectAll() {
    for (var i = 0; i < this.model.items.length; i++) {
      if (this.model.items[i].status) {
        this.model.items[i].isExpenseSelected = this.selectAllCheckbox;
      }
    }
    this.isExpenseButtonVisible();
  }

  //check box change
  changeCheckBoxModel() {
    this.isRowSelected();
  }

  //check box change detect
  isRowSelected() {
    var selectedcount = this.model.items.filter(x => x.isExpenseSelected).length;

    if (selectedcount < this.model.items.filter(x => x.status).length) {
      this.selectAllCheckbox = false;
    }
    else
      this.selectAllCheckbox = true;

    this.isExpenseButtonVisible();

  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  reset() {
    this.model.statusId = null;
    this.model.startDate = null;
    this.model.endDate = null;
    this.model.noFound = false;
  }

  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.model.index = 1;
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.getSearchData();
  }

  //office change event
  changeOffice(event) {
    this.qcRequest = new QcDataSourceRequest();

    if (event) {
      //office id assigned to filter QC
      this.qcRequest.officeCountryIds = this.model.officeIdList;
    }

    this.qcList = [];
    this.model.qcIdList = [];
    this.getQcData(false);
  }


  //fetch the Qc names data with virtual scroll
  getQcData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.qcRequest.searchText = this.qcInput.getValue();
      this.qcRequest.skip = this.qcList.length;
    }

    this.qcLoading = true;
    this.serviceSCH.getQcDataSourceList(this.qcRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.qcList = this.qcList.concat(customerData);
        }
        if (isDefaultLoad)
          this.qcRequest = new QcDataSourceRequest();
        this.qcLoading = false;
      }),
      error => {
        this.qcLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 Qc on load
  getQcListBySearch() {
    this.qcInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.qcLoading = true),
      switchMap(term => term
        ? this.serviceSCH.getQcDataSourceList(this.qcRequest, term)
        : this.serviceSCH.getQcDataSourceList(this.qcRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.qcLoading = false))
      ))
      .subscribe(data => {
        this.qcList = data;
        this.qcLoading = false;
      });
  }


  getSearchData() {
    this.validator.initTost();
    // this.validator.isSubmitted = true;
    if (this.isSummaryFormValid()) {
      this.searchLoading = true;

      this.service.getPendingExpenseList(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {

          this.mapPageProperties(res);

          if (res.result == ResponseResult.Success) {
            this.model.items = res.qcPendingExpenseData;
            this.isExpenseButtonVisible();
          }
          else {
            this.model.items = [];
            this.model.noFound = true;
          }
          this.searchLoading = false;
        },
          error => {
            this.searchLoading = false;
          })
    }
  }


  SavePendingExpenseData(pendingExpenseList) {
    if (this.isSummaryFormValid()) {
      this.searchLoading = true;
      this.service.saveQcPendingExpenseData(pendingExpenseList)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {
          if (res) {
            this.showSuccess("PendingExpense", "Successfully updated.");
            this.getSearchData();
          }
          else {
            this.showError("PendingExpense", "Input is not in a correct format");
          }
          this.searchLoading = false;
        },
          error => {
            this.searchLoading = false;
          })
    }
  }

  //get office list
  getOfficeData() {
    this.officeModelRequest.searchText = this.officeInput.getValue();
    this.officeModelRequest.skip = this.officeList.length;
    this.officeLoading = true;
    this.officeService.getOfficeListByOfficeAccess(this.officeModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.officeList = this.officeList.concat(data);
        }
        this.officeModelRequest = new CommonDataSourceRequest();
        this.officeLoading = false;
      }),
      error => {
        this.officeLoading = false;
      };
  }

  //fetch the first 10 office on load
  getOfficeListBySearch() {
    this.officeInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.officeLoading = true),
      switchMap(term => term
        ? this.officeService.getOfficeListByOfficeAccess(this.officeModelRequest, term)
        : this.officeService.getOfficeListByOfficeAccess(this.officeModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.officeLoading = false))
      ))
      .subscribe(data => {
        this.officeList = data;
        this.officeLoading = false;
      });
  }




  cancel() {
    if (this.popUpOpen) {
      this.validator.isSubmitted = false;
      this.modelRef.close();
    }
  }


  // expense button visible
  isExpenseButtonVisible() {
    if (this.model.items.filter(x => x.isExpenseSelected) &&
      this.model.items.filter(x => x.isExpenseSelected).length > 0) {
      this.isExpenseVisible = true;
    }
    else {
      this.isExpenseVisible = false;
    }
  }

  savePendingExpense() {

    if (this.model.items.filter(x => x.status && x.isExpenseSelected).length > 0) {
      this.SavePendingExpenseData(this.model.items.filter(x => x.status && x.isExpenseSelected));

    }
    else {
      this.showError("Pending Expense", "Please select atleast one record to update the pending Expense");
    }
  }

  openDeletePopUp(content, id) {
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
    this.idToDelete = id;
  }

  formValid(): boolean {
    let isOk = !this.BookingNoValidation() && this.validator.isValidIf('endDate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('startDate', this.IsDateValidationRequired())

    return isOk;
  }

  isSummaryFormValid() {
    // let isOk = !this.BookingNoValidation() && this.validator.isValidIf('endDate', this.IsDateValidationRequired()) && this.validator.isValidIf('startDate', this.IsDateValidationRequired())

    // return isOk;
    let isOk = true;
    if (this.model.startDate) {
      if (!this.model.endDate) {
        this.showWarning('Validation Error', 'FOOD_ALLOWANCE.MSG_END_DATE_REQUIRED');
        return isOk = false;
      }
      let sDate = new Date(this.model.startDate.year, this.model.startDate.month, this.model.startDate.day);
      let eDate = new Date(this.model.endDate.year, this.model.endDate.month, this.model.endDate.day);
      if (sDate > eDate) {
        this.showWarning('Validation Error', 'End date cannot be less than from date');
        return isOk = false;
      }
    }
    return isOk;
  }
}

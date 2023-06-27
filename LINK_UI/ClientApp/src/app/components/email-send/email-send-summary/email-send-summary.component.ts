import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, CountryDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { CustomerCollectionModel } from 'src/app/_Models/customer/customercollection.model';
import { EmailSendRequestModel, EmailSendSummaryItem, EmailSendSummaryModel, Parameter } from 'src/app/_Models/email-send/email-send-summary.model';
import { UserModel } from 'src/app/_Models/user/user.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { EmailSendSummaryService } from 'src/app/_Services/email-send/email-send-summary.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { APIService, BookingStatus, datetypelst, DefaultDateType, emailSendSummaryDatetypelst, extrafeeSearchtypelst, ListSize, PageSizeCommon, SearchType, SupplierType } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from '../../common/validator';

@Component({
  selector: 'app-email-send-summary',
  templateUrl: './email-send-summary.component.html',
  styleUrls: ['./email-send-summary.component.scss'],//,
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
export class EmailSendSummaryComponent extends SummaryComponent<EmailSendRequestModel> {
  componentDestroyed$: Subject<boolean> = new Subject();
  _customvalidationforbookid: boolean = false;
  bookingSearchtypelst: any = extrafeeSearchtypelst;
  summaryModel: EmailSendSummaryModel;
  _booksearttypeid = SearchType.BookingNo;
  datetypelst: any = emailSendSummaryDatetypelst;
  model: EmailSendRequestModel;
  toggleFormSection: boolean;
  isFilterOpen: boolean;
  resultTable: any;  
  selectedPageSize;
  _statuslist: any;
  selectAllChecked: boolean = false;
  isBookingForEmailSelected: boolean;
  private currentRoute: Router;
  currentUser: UserModel;
  _translate: TranslateService;
  pagesizeitems = PageSizeCommon;

  constructor(public validator: Validator, router: Router, translate: TranslateService, route: ActivatedRoute, private customerService: CustomerService,
    private supService: SupplierService, private referenceService: ReferenceService, private service: EmailSendSummaryService, private locationService: LocationService,
    private authserve: AuthenticationService, private customerBrandService: CustomerBrandService, private customerBuyerService: CustomerbuyerService,
    private customerDepartmentService: CustomerDepartmentService, private customerCollectionService: CustomerCollectionService,toastr: ToastrService,public utility: UtilityService) {
    super(router, validator, route, translate,toastr);
    this.validator.setJSON("emailsend/emailsendsummary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.toggleFormSection = false;
    this.isFilterOpen = true;
    this.currentRoute = router;
    this.currentUser = authserve.getCurrentUser();
    this.translate = translate;
  }

  getData(): void {
    this.getSearchData();
  }
  getPathDetails() {
    return 'emailsend/email-send';
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit() {
    this.Initialize();
    
    this.selectedPageSize = PageSizeCommon[0];
  }

  Initialize() {
    this.summaryModel = new EmailSendSummaryModel();
    this.model = new EmailSendRequestModel();
    this.model.searchTypeId = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.isBookingForEmailSelected = false;
    this.selectAllChecked = false;

    //this.getCustomerListBySearch();
    //this.getSupListBySearch();
    //this.getFactoryListBySearch();
    this.getStatusList();
    this.getServiceTypeList();
    this.getOfficeList();
    this.getAeList();
    this.getServiceList();
    this.getReportResults();
    //this.getCountryListBySearch();
  }

  ngAfterViewInit() {
    this.getCustomerListBySearch();
    this.getSupListBySearch();
    this.getFactoryListBySearch();
    this.getCountryListBySearch();

    if(this.model.selectedBrandIdList != null && this.model.selectedBrandIdList.length > 0){
      this.getCustomerBrandListList();
    }


    if(this.model.selectedBuyerIdList != null && this.model.selectedBuyerIdList.length > 0){
    this.getCustomerBuyerList();
    }

    if(this.model.selectedCollectionIdList != null && this.model.selectedCollectionIdList.length > 0){
    this.getCustomerCollectionList();
    }

    if(this.model.selectedDeptIdList != null && this.model.selectedDeptIdList.length > 0){
    this.getCustomerDepartmentList();
    }
  }
  
  //search type radio button
  SetSearchTypemodel(searchtype) {
    this.model.searchTypeId = searchtype;
    this._customvalidationforbookid = this.model.searchTypeId == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
  }

  //date radio button set
  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }

  //check booking no is valid
  BookingNoValidation(bookingText) {

    this._customvalidationforbookid = this.model.searchTypeId == this._booksearttypeid
      && bookingText && bookingText.trim() != "" && isNaN(Number(bookingText));
  }

  //validate the from and to date
  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchtypetext != null &&
      this.model.searchtypetext.trim() == "" ? true : false;

    if (this.model.searchtypetext == null || this.model.searchtypetext == "") {
      if (!this.model.fromdate)
        this.validator.isValid('fromdate');

      else if (this.model.fromdate && !this.model.todate)
        this.validator.isValid('todate');
    }
    return isOk;
  }

  //customer dropdown validation
  customerValidation(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchtypetext != null &&
    this.model.searchtypetext.trim() == "" ? true : false;

  if (this.model.searchtypetext == null || this.model.searchtypetext == "") {
    if (!this.model.customerId)
      this.validator.isValid('customerId');
  }
  return isOk;
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "Fromdate": {
        this.model.fromdate = null;
        break;
      }
      case "Todate": {
        this.model.todate = null;
        break;
      }
    }
  }

  //fetch customer dropdown list
  getCustomerListBySearch() {
    if (this.model.customerId && this.model.customerId > 0) {
      this.summaryModel.customerRequest.id = this.model.customerId;
    }
    else{
      this.summaryModel.customerRequest.id = null;
    }
    this.summaryModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.customerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryModel.customerList = data;
        this.summaryModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {
    
      this.summaryModel.customerRequest.searchText = this.summaryModel.customerInput.getValue();
      this.summaryModel.customerRequest.skip = this.summaryModel.customerList.length;
    

    this.summaryModel.customerLoading = true;

    this.summaryModel.customerRequest.id = 0;
    this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest)
    .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.customerList = this.summaryModel.customerList.concat(customerData);
        }
        this.summaryModel.customerRequest = new CommonDataSourceRequest();
        this.summaryModel.customerLoading = false;
      }),
      error => {
        this.summaryModel.customerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {

    if (this.model.supplierId && this.model.supplierId > 0) {
      this.summaryModel.supRequest.id = this.model.supplierId;
    }
    else {
      this.summaryModel.supRequest.id = null;
    }

    this.summaryModel.supRequest.customerId = this.model.customerId;
    this.summaryModel.supRequest.supplierType = SupplierType.Supplier;
    this.summaryModel.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.supLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.summaryModel.supRequest, term)
        : this.supService.getFactoryDataSourceList(this.summaryModel.supRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.supLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryModel.supplierList = data;
        this.summaryModel.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.summaryModel.supRequest.searchText = this.summaryModel.supInput.getValue();
    this.summaryModel.supRequest.skip = this.summaryModel.supplierList.length;

    this.summaryModel.supRequest.customerId = this.model.customerId;
    this.summaryModel.supRequest.supplierType = SupplierType.Supplier;
    this.summaryModel.supLoading = true;
    this.supService.getFactoryDataSourceList(this.summaryModel.supRequest)
    .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.supplierList = this.summaryModel.supplierList.concat(customerData);
        }
        this.summaryModel.supRequest.skip = 0;
        this.summaryModel.supRequest.take = ListSize;
        this.summaryModel.supLoading = false;
      }),
      error => {
        this.summaryModel.supLoading = false;
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getFactoryListBySearch() {

    if (this.model.factoryidlst && this.model.factoryidlst.length > 0) {
      this.summaryModel.factoryRequest.idList = this.model.factoryidlst;
    }
    else {
      this.summaryModel.factoryRequest.idList = null;
    }

    this.summaryModel.factoryRequest.customerId = this.model.customerId;
    this.summaryModel.factoryRequest.supplierType = SupplierType.Factory;
    this.summaryModel.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.factoryLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.summaryModel.factoryRequest, term)
        : this.supService.getFactoryDataSourceList(this.summaryModel.factoryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.factoryLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryModel.factoryList = data;
        this.summaryModel.factoryLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData() {
    this.summaryModel.factoryRequest.searchText = this.summaryModel.factoryInput.getValue();
    this.summaryModel.factoryRequest.skip = this.summaryModel.supplierList.length;

    this.summaryModel.factoryRequest.customerId = this.model.customerId;
    this.summaryModel.factoryRequest.supplierType = SupplierType.Factory;
    this.summaryModel.factoryLoading = true;
    this.supService.getFactoryDataSourceList(this.summaryModel.factoryRequest)
    .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.factoryList = this.summaryModel.factoryList.concat(customerData);
        }
        this.summaryModel.factoryRequest.skip = 0;
        this.summaryModel.factoryRequest.take = ListSize;
        this.summaryModel.factoryLoading = false;
      }),
      error => {
        this.summaryModel.factoryLoading = false;
      };
  }

  //get office list
  getOfficeList() {
    this.summaryModel.officeLoading = true;
    this.referenceService.getOfficeList()
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryModel.officeList = response.dataSourceList;
          this.summaryModel.officeLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.officeLoading = false;
        });
  }

  //get inspection status list
  getStatusList() {
    this.summaryModel.statusLoading = true;
    this.service.getBookingStatus()
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryModel.statusList = response.dataSourceList;
            this.CheckRequestStatus()
          this.summaryModel.statusLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.statusLoading = false;
        });
  }

  //get service type list
  getServiceTypeList() {
    this.summaryModel.serviceTypeLoading = true;
    this.referenceService.getServiceTypeList()
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryModel.serviceTypeList = response.dataSourceList;
          this.summaryModel.serviceTypeLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.serviceTypeLoading = false;
        });
  }

  //fetch the country data with virtual scroll
  getCountryData() {
    this.summaryModel.countryRequest.searchText = this.summaryModel.countryInput.getValue();
    this.summaryModel.countryRequest.skip = this.summaryModel.countryList.length;

    this.summaryModel.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.countryList = this.summaryModel.countryList.concat(customerData);
        }
        this.summaryModel.countryRequest = new CountryDataSourceRequest();
        this.summaryModel.countryLoading = false;
      }),
      error => {
        this.summaryModel.countryLoading = false;
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {

    if (this.model.selectedCountryIdList && this.model.selectedCountryIdList.length > 0) {
      this.summaryModel.countryRequest.countryIds = this.model.selectedCountryIdList;
    }
    else {
      this.summaryModel.countryRequest.countryIds = null;
    }

    this.summaryModel.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.countryLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryModel.countryList = data;
        this.summaryModel.countryLoading = false;
      });
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  //get AE List
  getAeList() {
    this.summaryModel.aeLoading = true;
    this.service.getAeList()
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)

             this.summaryModel.aeList = response.data.filter(
              (thing, i, arr) => arr.findIndex(t => t.id === thing.id) === i
            );
          this.summaryModel.aeLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.aeLoading = false;
        });
  }
    
  //get service list
    getServiceList() {
      this.summaryModel.serviceLoading = true;
      this.referenceService.getAPIServices()
      .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success)
              this.summaryModel.serviceList = response.dataSourceList;
              this.model.serviceId = APIService.Inspection;
            this.summaryModel.serviceLoading = false;
  
          },
          error => {
            this.setError(error);
            this.summaryModel.serviceLoading = false;
          });
    }

    //search summary
    getSearchData() {
      
    this.summaryModel.searchLoading = true;
    this.model.noFound = false;
    this.isBookingForEmailSelected = false;
    this.selectAllChecked = false;
    
   this.service.searchEmailSummary(this.model)
   .pipe(takeUntil(this.componentDestroyed$), first())
   .subscribe(response => {
     if(response.result == ResponseResult.Success){       
      this._statuslist = response.inspectionStatuslst;
      this.mapPageProperties(response);
        this.model.items = response.data.map(x => {
          var item: EmailSendSummaryItem = {
            bookingId: x.bookingId,
            bookingCustomerNumer: x.customerBookingNo ? x.bookingId + " /" + x.customerBookingNo : x.bookingId,
            customerName: x.customerName,
            supplierName: x.supplierName,
            factoryName: x.factoryName,
            officeName: x.office,
            serviceTypeName: x.serviceType,
            serviceDate: x.serviceDate,
            showCheckBox: x.statusId,
            reportCount: x.reportCount,
            successReportCount:x.successReportCount,
            pendingCount:(x.reportCount-x.successReportCount),
            isBookingSelected: false,
            customerId: x.customerId,
            statusId: x.statusId
          }
          return item; 
          });
          this.summaryModel.searchLoading = false;          
          this.validator.isSubmitted = false;
     }
     else if (response && response.result == ResponseResult.NoDataFound) {
      this.model.items = [];
      this.model.noFound = true;
      this.summaryModel.searchLoading = false;        
      this.validator.isSubmitted = false;
    }
   }, 
   error => {
    this.setError(error);  
  });
  }

   //search details
   SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  SearchByStatus(id) {
    this.model.statusidlst = [];
    this.model.statusidlst.push(id);
    this.SearchDetails();
  }

  formValid(): boolean {
    let isOk = !this._customvalidationforbookid && (this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired()) && this.validator.isValidIf('customerId', this.customerValidation()))

    return isOk;
  }
  
  //triggered everytime a check box is checked or unchecked
  checkAllSelected(item){
    var selectedcount = this.model.items.filter(x => x.isBookingSelected).length;
    var showCheckBoxTrue = this.model.items.filter(x => x.showCheckBox).length;

    if(selectedcount == showCheckBoxTrue){
      this.selectAllChecked = true;
      this.isBookingForEmailSelected = true;
    }
    else if(selectedcount > 0){
      this.selectAllChecked = false;
      this.isBookingForEmailSelected = true;
    }
    else{
      this.selectAllChecked = false;
      this.isBookingForEmailSelected = false;
    }

  }

  //select all chackbox change event
  SelectAllCheckBox() {
    var selectedcount = this.model.items.filter(x => x.isBookingSelected).length;
    if (selectedcount != this.model.items.filter(x => x.showCheckBox).length) {
      this.model.items.forEach(element => {
        if(element.showCheckBox)
        element.isBookingSelected = true;
      });
      this.isBookingForEmailSelected = true; //show the Send Email
      this.selectAllChecked = true; //check the select all
    }

    else {
      this.model.items.forEach(element => {
        element.isBookingSelected = false;
      });
      this.isBookingForEmailSelected = false; //hide the Send Email
      this.selectAllChecked = false; //uncheck the select all
    }
  }

  //set the initial status selection
  CheckRequestStatus() {
    this.model.statusidlst = this.summaryModel.statusList.filter(x => x.id == BookingStatus.ReportSent || x.id == BookingStatus.Validated || x.id == BookingStatus.Inspected).map(x => x.id);
  }

 mapPageProperties(response: any) {

    this.model.index = response.index;
    this.model.pageSize = response.pageSize;
    this.model.totalCount = response.totalCount;
    this.model.pageCount = response.pageCount;
  }

  GetSearchLoadingFlag(){
    this.summaryModel.searchLoading = this.summaryModel.customerLoading && this.summaryModel.countryLoading && this.summaryModel.supLoading &&
    this.summaryModel.factoryList &&this.summaryModel.officeList && this.summaryModel.serviceTypeList && this.summaryModel.brandLoading
    && this.summaryModel.departmentLoading && this.summaryModel.collectionLoading && this.summaryModel.buyerLoading
  }

  reset() {
    this.Initialize();
    this.validator.isSubmitted = false;
    this.getCustomerListBySearch();
    this.getSupListBySearch();
    this.getFactoryListBySearch();
  }

  //validate the customer for email rule
  ReportSendValidation(){
    this.summaryModel.sendEmailLoading = true;
    var selectedBookingCount = this.model.items.filter(x => x.isBookingSelected).map(x => x.bookingId);

    this.model.customerId = this.model.customerId ? this.model.customerId : this.model.items.map(x => x.customerId)[0];

    this.service.validateMutipleEmailSendByCustomer(this.model.customerId, this.model.serviceId)
    .pipe(takeUntil(this.componentDestroyed$), first())
    .subscribe(res => 
      {
        if(res.ruleFound) {
        if((selectedBookingCount.length > 1 && res.sendMultipleEmail) || (selectedBookingCount.length == 1)){
          var paramenterObject = new Parameter();
          this.model.bookingIdList = selectedBookingCount;
          paramenterObject.serviceId = this.model.serviceId;

          this.summaryModel.sendEmailLoading = false;

          this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}`], {queryParams:{ paramParent: encodeURI(JSON.stringify(this.model)) }});
        }
        else{
          this.summaryModel.sendEmailLoading = false;
          this.showError('EMAIL_SEND_SUMMARY.TITLE', 'EMAIL_SEND_SUMMARY.MSG_CANT_SEND_MULTIPLE_EMAILS');
        }
      }
      else{
        this.summaryModel.sendEmailLoading = false;
        this.showError('EMAIL_SEND_SUMMARY.TITLE', 'EMAIL_SEND_SUMMARY.MSG_EMAIL_RULE_NOT_FOUND');
      }
      },
      error=> {
        this.summaryModel.sendEmailLoading = false;
      })
  }

   //get department list
   getCustomerDepartmentList() {
    this.summaryModel.departmentLoading = true;
    if(this.model.customerId) {
    this.customerDepartmentService.getCustomerDepartment(this.model.customerId)
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryModel.departmentList = response.customerDepartmentList;
          this.summaryModel.departmentLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.departmentLoading = false;
        });
      }
      this.summaryModel.departmentList = [];
      this.summaryModel.departmentLoading = false;
  }

   //get brand list
   getCustomerBrandListList() {
    this.summaryModel.brandLoading = true;
    if(this.model.customerId) {
    this.customerBrandService.getEditCustomerBrand(this.model.customerId)
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response)
            this.summaryModel.brandList = response.customerBrands;
          this.summaryModel.brandLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.brandLoading = false;
        });
      }
      this.summaryModel.brandList = [];
      this.summaryModel.brandLoading = false;
  }

   //get service list
   getCustomerBuyerList() {
    this.summaryModel.buyerLoading = true;
    if(this.model.customerId) {
    this.customerBuyerService.getCustomerBuyer(this.model.customerId)
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryModel.buyerList = response.customerBuyerList;
          this.summaryModel.buyerLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.buyerLoading = false;
        });
      }
      this.summaryModel.buyerList = [];
      this.summaryModel.buyerLoading = false;

  }

   //get service list
   getCustomerCollectionList() {
    this.summaryModel.collectionLoading = true;
    if(this.model.customerId) {
      var request = new CustomerCollectionModel(); 
      request.id = this.model.customerId;
    this.customerCollectionService.getEditCustomerCollection(request)
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.summaryModel.collectionList = response.customerCollectionList;
          this.summaryModel.collectionLoading = false;

        },
        error => {
          this.setError(error);
          this.summaryModel.collectionLoading = false;
        });
      }
      this.summaryModel.collectionList = [];
      this.summaryModel.collectionLoading = false;
  }

  customerChange(){

    this.summaryModel.supplierList = [];
    this.summaryModel.factoryList = [];
    this.model.supplierId = null;
    this.model.factoryidlst = [];
    this.model.selectedBrandIdList = [];
    this.model.selectedDeptIdList = [];
    this.model.selectedCollectionIdList = [];
    this.model.selectedBuyerIdList = [];    

    this.getCustomerBrandListList();
    this.getCustomerBuyerList();
    this.getCustomerCollectionList();
    this.getCustomerDepartmentList();
    this.getSupListBySearch();
    this.getFactoryListBySearch();
  }

   //get manday by customer export
   emailSendSummaryExport() {
    this.summaryModel.exportLoading = true;


    this.service.emailSendSummaryExport(this.model)
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Email_Send_Summary_Export.xlsx");
        this.summaryModel.exportLoading = false;
      },
        error => {
          this.summaryModel.exportLoading = false;
        });
  }

  
  //download the excel file
  async downloadFile(data, mimeType, fileName) {
    const blob = new Blob([data], { type: mimeType });

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, fileName);
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fileName;
      a.href = url;
      a.click();
    }
  }

  clearCustomer() {
    this.summaryModel.customerRequest.searchText=null;
    this.getCustomerListBySearch();
  }

  clearSupplier(){
    this.getSupListBySearch();
  }

  clearFactoryData(){
    this.getFactoryListBySearch();
  }

  clearCountryData(){
    this.getCountryListBySearch();
  }

  GetStatusColor(statusId) {
    if (this._statuslist != null && statusId > 0) {
      var result = this._statuslist.find(x => x.id == statusId);
      if (result)
        return result.statusColor;
    }
  }

  //get the fullbridge report results
  getReportResults() {
    this.summaryModel.apiResultLoading = true;
    this.referenceService.getFullBridgeResultData()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
      data => {
          this.processFullBridgeReportResults(data);
          this.summaryModel.apiResultLoading = false;
        },
      error => {
          this.setError(error);
          this.summaryModel.apiResultLoading = false;
        });
  }
  //process the fullbridge report results
  processFullBridgeReportResults(data) {
    if (data && data.result == ResponseResult.Success) {
      this.summaryModel.apiResultList = data.dataSourceList;
    }
    else if (data && data.result == ResponseResult.NoDataFound) {
      this.showError('EMAIL_SEND_SUMMARY.TITLE', 'EMAIL_SEND_SUMMARY.MSG_FULLBRIDGE_REPORT_RESULTS_NOT_FOUND');
    }
  }

}

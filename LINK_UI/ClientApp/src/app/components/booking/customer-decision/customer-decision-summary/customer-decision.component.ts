import { CustomerCommonDataSourceRequest } from './../../../../_Models/common/common.model';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { APP_INITIALIZER, Component, OnInit } from '@angular/core';
import { Validator } from "../../../common/validator"
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SummaryComponent } from '../../../common/summary.component';
import { AdvanceSearchType, APIService, bookingAdvanceSearchtypelst, bookingSearchtypelst, BookingStatus, bookingStatusList, CustomerDecisionDdl, DefaultDateType, extrafeedatetypelst, FbReportResultType, InspectedStatusList, ListSize, PageSizeCommon, RoleEnum, SearchType, SupplierType, UserType } from '../../../common/static-data-common';
import { CustomerDecisionMasterData, CustomerDecisionRequestModel } from 'src/app/_Models/booking/customerdecision.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { of, Subject } from 'rxjs';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { InspectionCustomerDecisionService } from 'src/app/_Services/booking/inspectioncustomerdecision.service';
import { InspectionBookingsummarymodel } from 'src/app/_Models/booking/inspectionbookingsummarymodel';
import { MandayTerm } from 'src/app/_Models/statistics/manday-dashboard.model';
import { NgbCalendar, NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-customer-decision',
  templateUrl: './customer-decision.component.html',
  styleUrls: ['./customer-decision.component.scss'],
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
export class CustomerDecisionComponent extends SummaryComponent<CustomerDecisionRequestModel> {

  componentDestroyed$: Subject<boolean> = new Subject;
  isFilterOpen: boolean;
  bookingSearchtypelst: any = bookingSearchtypelst;
  _customvalidationforbookingid: boolean = false;
  _booksearttypeid = SearchType.BookingNo;
  datetypelst: any = extrafeedatetypelst;
  model: CustomerDecisionRequestModel;
  _IsInternalUser: boolean = false;
  _IsCustomerUser: boolean = false;
  currentUser: UserModel;
  masterData: CustomerDecisionMasterData;
  toggleFormSection: boolean = false;
  bookingAdvanceSearchtypelst: any = bookingAdvanceSearchtypelst;
  ddlList: any = CustomerDecisionDdl;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  _statuslist: any[] = [];
  editCustomerDecision: boolean = false;
  viewCustomerDecision: boolean = false;
  _roleEnum = RoleEnum;
  currentRoute: Router;
  currentProductList: any;
  modelRef: NgbModalRef;
  requestCustomerModel : CustomerCommonDataSourceRequest;

  constructor(route: ActivatedRoute, translate: TranslateService, validator: Validator, router: Router, private authService: AuthenticationService,
    private supService: SupplierService, private customerService: CustomerService, private collectionService: CustomerCollectionService,
    private brandService: CustomerBrandService, private buyerService: CustomerbuyerService, private deptService: CustomerDepartmentService,
    private officeService: OfficeService, public utility: UtilityService, private locationService: LocationService, private refService: ReferenceService, public modalService: NgbModal,
    private service: InspectionCustomerDecisionService,public pathroute: ActivatedRoute,public calendar: NgbCalendar) {

    super(router, validator, route, translate);
    this.currentUser = authService.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this._IsCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();
    const isAccess = this.currentUser.roles.some(x => x.id == RoleEnum.EditInspectionCustomerDecision || x.id == RoleEnum.ViewInspectionCustomerDecision);
    if (!this._IsInternalUser && !isAccess)
      authService.redirectToLanding();

    this.isFilterOpen = true;
    this.currentRoute = router;

    this.validator.setJSON("booking/booking-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
  }

  onInit(): void {
    this.getIsMobile();
    this.objectInitialize();
    this.selectedPageSize = PageSizeCommon[0];
    this.checkAndSetRoles();
    this.checkCustomerDescision();
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  checkAndSetRoles() {
    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.EditInspectionCustomerDecision).length > 0) {
      this.editCustomerDecision = true;
    }

    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.ViewInspectionCustomerDecision).length > 0) {
      this.viewCustomerDecision = true;
    }
  }

  ngAfterViewInit() {
    this.getCustomerListBySearch();
    this.getSupplierListBySearch();
    this.getFactListBySearch();
    this.getOfficeLocationList();
    this.getCountryListBySearch();

    if (this.model.customerid) {
      this.getServiceTypeList();
      this.getBrandListBySearch();
      this.getDeptListBySearch();
      this.getBuyerListBySearch();
      this.getCollectionListBySearch();
    }

    if (this.model.showFbResult) {
      this.getCustomerDecisionResultList(this.ddlList[0])
    }
  }

  objectInitialize() {
    this.masterData = new CustomerDecisionMasterData();
    this.model = new CustomerDecisionRequestModel();
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.advancedSearchtypeid = AdvanceSearchType.ProductName;
    this._statuslist = [];

    //fetch only report sent bookings ALD-3052
    this.model.statusidlst = [InspectedStatusList.ReportSent];

    if (this._IsCustomerUser) {
      this.model.customerid = this.currentUser.customerid;
    }
  }

  BookingNoValidation(bookingText) {

    this._customvalidationforbookingid = this.model.searchtypeid == this._booksearttypeid
      && bookingText && bookingText.trim() != "" && ((isNaN(Number(bookingText))) || (bookingText.trim().length > 9));
  }

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

  SetSearchTypemodel(searchtype) {
    this.model.searchtypeid = searchtype;
    this._customvalidationforbookingid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
  }

  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }
  SetSearchTypeText(searchtypetext) {
    this.model.advancedSearchtypeid = searchtypetext;
  }

  getData(): void {
    this.getSearchData();
  }
  getPathDetails(): string {

    return "";
  }

  getCustomerListBySearch() {

    //push the customerid to  customer id list
    if (this.model.customerid) {
      this.masterData.requestCustomerModel.idList.push(this.model.customerid);
    }
    else {
      this.masterData.requestCustomerModel.idList = null;
    }

    this.masterData.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerListByUserType(this.requestCustomerModel, this.currentUser.usertype, term)
        : this.customerService.getCustomerListByUserType(this.requestCustomerModel, this.currentUser.usertype)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.customerLoading = false))
      ))
      .subscribe(data => {
        this.masterData.customerList = data;
        this.masterData.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterData.requestCustomerModel.searchText = this.masterData.customerInput.getValue();
      this.masterData.requestCustomerModel.skip = this.masterData.customerList.length;
    }

    this.masterData.customerLoading = true;
    this.customerService.getCustomerListByUserType(this.requestCustomerModel, this.currentUser.usertype).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.masterData.customerList = this.masterData.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.masterData.requestCustomerModel.skip = 0;
          this.masterData.requestCustomerModel.take = ListSize;
        }
        this.masterData.customerLoading = false;
      }),
      error => {
        this.masterData.customerLoading = false;
        this.setError(error);
      };
  }

  clearCustomer() {
    this.model.customerid = null;
    this.model.supplierid = null;
    this.masterData.supplierList = [];
    this.model.factoryidlst = [];
    this.model.serviceTypelst = null;
    this.masterData.serviceTypeList = [];
    this.masterData.brandList = [];
    this.masterData.deptList = [];
    this.masterData.buyerList = [];
    this.masterData.supplierList = [];
    this.masterData.collectionList = [];
    this.masterData.factoryList = [];
    this.model.selectedBuyerIdList = [];
    this.model.selectedBrandIdList = [];
    this.model.selectedCollectionIdList = [];
    this.model.selectedDeptIdList = [];
    this.getCustomerListBySearch();
    this.getSupplierListBySearch();
    this.getFactListBySearch();

    if (this.model.showFbResult) {
      this.masterData.fbReportResultList = [];
      this.model.fbReportResultList = [];
      this.getCustomerDecisionResultList(this.ddlList[0])
    }
  }

  ChangeCustomer(cusitem) {

    if (cusitem != null && cusitem.id != null) {
      //clear the list
      this.masterData.brandList = [];
      this.masterData.deptList = [];
      this.masterData.supplierList = [];
      this.masterData.collectionList = [];
      this.masterData.buyerList = [];
      //clear the selected values
      this.model.selectedBrandIdList = [];
      this.model.selectedDeptIdList = [];
      this.model.supplierid = null;
      this.model.selectedCollectionIdList = [];
      this.model.selectedBuyerIdList = [];
      this.model.factoryidlst = [];

      this.getSupplierListBySearch();
      this.getFactListBySearch();
      this.getBrandListBySearch();
      this.getDeptListBySearch();
      this.getCollectionListBySearch();
      this.getBuyerListBySearch();
      this.getServiceTypeList();

      if (this.model.showFbResult) {
        this.masterData.fbReportResultList = [];
        this.model.fbReportResultList = [];
        this.getCustomerDecisionResultList(this.ddlList[0])
      }
    }

  }

  getSupplierListBySearch() {
    this.masterData.supsearchRequest = new CommonDataSourceRequest();
    if (this.model.customerid)
      this.masterData.supsearchRequest.customerId = this.model.customerid;
    this.masterData.supsearchRequest.supplierType = SupplierType.Supplier;
    if (this.model.supplierid)
      this.masterData.supsearchRequest.supplierId = this.model.supplierid;
    this.masterData.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.supLoading = true),
      switchMap(term => term
        ? this.supService.GetSupplierList(this.masterData.supsearchRequest, term)
        : this.supService.GetSupplierList(this.masterData.supsearchRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.supLoading = false))
      ))
      .subscribe(data => {
        this.masterData.supplierList = data;
        this.masterData.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    console.log(this.masterData.supInput.getValue());
    this.masterData.supsearchRequest.searchText = this.masterData.supInput.getValue();
    this.masterData.supsearchRequest.skip = this.masterData.supplierList.length;

    this.masterData.supsearchRequest.customerId = this.model.customerid;
    this.masterData.supsearchRequest.supplierType = SupplierType.Supplier;
    this.masterData.supLoading = true;
    this.supService.GetSupplierList(this.masterData.supsearchRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterData.supplierList = this.masterData.supplierList.concat(customerData);
        }
        this.masterData.supsearchRequest.skip = 0;
        this.masterData.supsearchRequest.take = ListSize;
        this.masterData.supLoading = false;
      }),
      (error: any) => {
        this.masterData.supLoading = false;
      };
  }

  ChangeSupplier(supitem) {
    this.model.factoryidlst = [];
    if (supitem != null && supitem.id != null) {
      this.getFactListBySearch();
    }
  }

  clearSupplier() {
    this.model.supplierid = null;
    this.model.factoryidlst = [];
    this.getSupplierListBySearch();
  }

  clearFactory() {
    this.model.factoryidlst = [];
    this.getFactListBySearch();
  }

  //fetch the first 10 suppliers for the customer on load
  getFactListBySearch() {

    this.masterData.facsearchRequest = new CommonDataSourceRequest();

    if (this.model.factoryidlst && this.model.factoryidlst.length > 0) {
      this.masterData.facsearchRequest.idList = this.model.factoryidlst;
    }
    if (this.model.supplierid) {
      this.masterData.facsearchRequest.supplierId = this.model.supplierid;
    }
    else {
      this.masterData.facsearchRequest.supplierId = 0;
    }

    this.masterData.factoryList = [];
    this.masterData.facsearchRequest.customerId = this.model.customerid;
    this.masterData.facsearchRequest.supplierType = SupplierType.Factory;
    this.masterData.facInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.facLoading = true),
      switchMap(term => term
        ? this.supService.GetFactoryList(this.masterData.facsearchRequest, term)
        : this.supService.GetFactoryList(this.masterData.facsearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.facLoading = false))
      ))
      .subscribe(data => {
        this.masterData.factoryList = data;
        this.masterData.facLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterData.facsearchRequest.searchText = this.masterData.facInput.getValue();
      this.masterData.facsearchRequest.skip = this.masterData.factoryList.length;
    }
    this.masterData.facsearchRequest.customerId = this.masterData.facsearchRequest.customerId;
    this.masterData.facsearchRequest.supplierType = SupplierType.Factory;
    this.masterData.facLoading = true;
    this.supService.GetFactoryList(this.masterData.facsearchRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterData.factoryList = this.masterData.factoryList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.masterData.facsearchRequest.skip = 0;
          this.masterData.facsearchRequest.take = ListSize;
        }
        this.masterData.facLoading = false;
      }),
      error => {
        this.masterData.facLoading = false;
        this.setError(error);
      };
  }

  changeFactory() {
    this.getFactListBySearch();
  }

  //fetch the brand data with virtual scroll
  getBrandData() {
    this.masterData.brandSearchRequest.searchText = this.masterData.brandInput.getValue();
    this.masterData.brandSearchRequest.skip = this.masterData.brandList.length;

    this.masterData.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.masterData.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.masterData.brandList = this.masterData.brandList.concat(brandData);
        }
        this.masterData.brandSearchRequest = new CommonCustomerSourceRequest();
        this.masterData.brandLoading = false;
      }),
      error => {
        this.masterData.brandLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    if (this.model.selectedBrandIdList && this.model.selectedBrandIdList.length > 0) {
      this.masterData.brandSearchRequest.idList = this.model.selectedBrandIdList;
    }
    else {
      this.masterData.brandSearchRequest.idList = null;
    }
    this.masterData.brandSearchRequest = new CommonCustomerSourceRequest();
    this.masterData.brandSearchRequest.customerId = this.model.customerid;
    this.masterData.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.masterData.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.masterData.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.brandLoading = false))
      ))
      .subscribe(data => {
        this.masterData.brandList = data;
        this.masterData.brandLoading = false;
      });
  }

  clearBrand() {
    this.model.selectedBrandIdList = [];
    this.getBrandListBySearch();
  }
  clearBuyer() {
    this.model.selectedBuyerIdList = [];
    this.getBuyerListBySearch();
  }
  clearCollection() {
    this.model.selectedCollectionIdList = [];
    this.getCollectionListBySearch();
  }
  clearDept() {
    this.model.selectedDeptIdList = [];
    this.getDeptListBySearch();
  }

  //fetch the brand data with virtual scroll
  getDeptData() {
    this.masterData.deptSearchRequest.searchText = this.masterData.deptInput.getValue();
    this.masterData.deptSearchRequest.skip = this.masterData.deptList.length;

    this.masterData.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.masterData.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.masterData.deptList = this.masterData.deptList.concat(deptData);
        }
        this.masterData.deptSearchRequest = new CommonCustomerSourceRequest();
        this.masterData.deptLoading = false;
      }),
      error => {
        this.masterData.deptLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {
    this.masterData.deptSearchRequest = new CommonCustomerSourceRequest();

    this.masterData.deptSearchRequest.customerId = this.model.customerid;

    if (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) {
      this.masterData.deptSearchRequest.idList = this.model.selectedDeptIdList;
    }
    else {
      this.masterData.deptSearchRequest.idList = null;
    }

    this.masterData.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.masterData.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.masterData.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.deptLoading = false))
      ))
      .subscribe(data => {
        this.masterData.deptList = data;
        this.masterData.deptLoading = false;
      });
  }

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.masterData.buyerSearchRequest.searchText = this.masterData.buyerInput.getValue();
    this.masterData.buyerSearchRequest.skip = this.masterData.buyerList.length;

    this.masterData.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.masterData.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.masterData.buyerList = this.masterData.buyerList.concat(buyerData);
        }
        this.masterData.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.masterData.buyerLoading = false;
      }),
      error => {
        this.masterData.buyerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.masterData.buyerSearchRequest = new CommonCustomerSourceRequest();

    this.masterData.buyerSearchRequest.customerId = this.model.customerid;

    if (this.model.selectedBuyerIdList && this.model.selectedBuyerIdList.length > 0) {
      this.masterData.buyerSearchRequest.idList = this.model.selectedBuyerIdList;
    }
    else {
      this.masterData.buyerSearchRequest.idList = null;
    }

    this.masterData.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.masterData.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.masterData.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.buyerLoading = false))
      ))
      .subscribe(data => {
        this.masterData.buyerList = data;
        this.masterData.buyerLoading = false;
      });
  }

  //fetch the collection data with virtual scroll
  getCollectionData() {
    this.masterData.collectionSearchRequest.searchText = this.masterData.collectionInput.getValue();
    this.masterData.collectionSearchRequest.skip = this.masterData.collectionList.length;

    this.masterData.collectionLoading = true;
    this.collectionService.getCollectionListByCustomerId(this.masterData.collectionSearchRequest).
      subscribe(collectionData => {
        if (collectionData && collectionData.length > 0) {
          this.masterData.collectionList = this.masterData.collectionList.concat(collectionData);
        }
        this.masterData.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.masterData.collectionLoading = false;
      }),
      error => {
        this.masterData.collectionLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count collection on load
  getCollectionListBySearch() {
    this.masterData.collectionSearchRequest = new CommonCustomerSourceRequest();

    this.masterData.collectionSearchRequest.customerId = this.model.customerid;

    if (this.model.selectedCollectionIdList && this.model.selectedCollectionIdList.length > 0) {
      this.masterData.collectionSearchRequest.idList = this.model.selectedCollectionIdList;
    }
    else {
      this.masterData.collectionSearchRequest.idList = null;
    }
    this.masterData.collectionInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.collectionLoading = true),
      switchMap(term => term
        ? this.collectionService.getCollectionListByCustomerId(this.masterData.collectionSearchRequest, term)
        : this.collectionService.getCollectionListByCustomerId(this.masterData.collectionSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.collectionLoading = false))
      ))
      .subscribe(data => {
        this.masterData.collectionList = data;
        this.masterData.collectionLoading = false;
      });
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  //get the office list
  getOfficeLocationList() {
    this.masterData.officeLoading = true;
    this.officeService.getOfficeDetails()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.masterData.officeList = response.dataSourceList;
          this.masterData.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.officeLoading = false;
        });
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.masterData.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.masterData.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.masterData.countryRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.countryLoading = false))
      ))
      .subscribe(data => {
        this.masterData.countryList = data;
        this.masterData.countryLoading = false;
      });
  }

  //fetch the country data with virtual scroll
  getCountryData() {

    this.masterData.countryRequest.searchText = this.masterData.countryInput.getValue();
    this.masterData.countryRequest.skip = this.masterData.countryList.length;

    this.masterData.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.masterData.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterData.countryList = this.masterData.countryList.concat(customerData);
        }

        this.masterData.countryRequest = new CountryDataSourceRequest();
        this.masterData.countryLoading = false;
      }),
      error => {
        this.masterData.countryLoading = false;
        this.setError(error);
      };
  }

  getServiceTypeList() {

    this.masterData.serviceTypeLoading = true;
    this.refService.getServiceTypelist(this.model.customerid, APIService.Inspection)
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.masterData.serviceTypeList = data.dataSourceList;
            this.masterData.serviceTypeLoading = false;
          }
          else {
            this.error = data.result;
            this.masterData.serviceTypeLoading = false;
          }

        },
        error => {
          this.masterData.serviceTypeLoading = false;
          this.setError(error);

        });
  }

  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
    console.log(this.isMobile);
  }

  getCustomerDecisionResultList(item) {
    if (item) {
      this.masterData.fbReportResultLoading = true;
      if (item.id == 1) {
        this.model.showFbResult = true;

        this.service.GetInspectionCustomerDecision(this.model.customerid)
          .pipe(takeUntil(this.componentDestroyed$))
          .subscribe(res => {
            if (res.result == ResponseResult.Success) {
              this.masterData.fbReportResultList = res.customerDecisionList;
              this.masterData.fbReportResultLoading = false;
            }
            else {
              this.model.fbReportResultList = [];
              this.masterData.fbReportResultLoading = false;
            }
          },
            error => {
              this.masterData.fbReportResultList = [];
              this.masterData.fbReportResultLoading = false;
            });
      }
      else {
        this.model.fbReportResultList = [];
        this.model.showFbResult = false;
        this.masterData.fbReportResultLoading = false;
      }
    }
    else {
      this.model.fbReportResultList = [];
      this.model.showFbResult = false;
    }
  }

  getSearchData() {
    this.masterData.searchloading = true;
    this.service.CustomerDecisionSummary(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.model.items = res.data;
          this.mapPageProperties(res);
          this._statuslist = res.inspectionStatuslst;
          this.masterData.searchloading = false;
        }
        else {
          this.masterData.searchloading = false;
          this.model.noFound = true;
          this._statuslist = [];
        }
      },
        error => {
          this.masterData.searchloading = false;
          this.model.noFound = true;
          this._statuslist = [];
        })
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  GetStatusColor(statusid?) {
    if (this._statuslist != null && this._statuslist.length > 0 && statusid != null) {
      var result = this._statuslist.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }

  mapPageProperties(response: any) {

    this.model.index = response.index;
    this.model.pageSize = response.pageSize;
    this.model.totalCount = response.totalCount;
    this.model.pageCount = response.pageCount;

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

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  formValid(): boolean {
    let isOk = !this._customvalidationforbookingid && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())

    if (this._IsInternalUser) {
      isOk = this.validator.isValidIf('customerid', this.model.advancedsearchtypetext)
    }

    if (this._IsInternalUser && this.model.advancedsearchtypetext && !this.model.customerid) {
      isOk = this.validator.isValid('customerid');
    }

    if (this._IsInternalUser && (this.model.searchtypetext && this.model.searchtypeid != 1) && !this.model.customerid) {
      isOk = this.validator.isValid('customerid');
    }

    return isOk;
  }

  getCustomerDecisionView(bookingId, reportId, containerId) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/customerdecision/edit-customer-decision/${bookingId}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
    //this.currentRoute.navigate([`/${this.utility.getEntityName()}/customerdecision/edit-customer-decision`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  Reset() {

    this.objectInitialize();
    this.ngAfterViewInit();
  }

  openReportPopUp(item, content) {
    this.currentProductList = item.productResultList;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
  }

  exportCustomerDecisionSummary() {

    this.masterData.exportLoading = true;

    this.service.exportCustomerDecisionSummary(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer_Decision_Export.xlsx");
        this.masterData.exportLoading = false;
      },
        error => {
          this.masterData.exportLoading = false;
        });
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

  checkCustomerDescision() {
    let customerId = Number(this.pathroute.snapshot.paramMap.get("customerId"));
    let customerdecisionId = this.pathroute.snapshot.paramMap.get("customerdecision");
    let fromdate: NgbDate = this.calendar.getNext(this.calendar.getToday(), 'd', -30);
    let todate: NgbDate = this.calendar.getNext(this.calendar.getToday());

    //check page comes from customer descision dashboard
    if (customerdecisionId) {
      let customerDecision = CustomerDecisionDdl.find(x => x.id == parseInt(customerdecisionId));

      this.model.fromdate = fromdate;
      this.model.todate = todate;
      this.model.cusDecisionGiven = parseInt(customerdecisionId);
      this.getCustomerDecisionResultList(customerDecision);
      if(customerId > 0)
        this.model.customerid = customerId;
      this.SearchDetails();
    }
  }
}

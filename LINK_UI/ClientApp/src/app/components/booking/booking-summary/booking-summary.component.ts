import { Component, OnInit, ViewChild, ElementRef, DebugElement } from '@angular/core';
import { debounceTime, distinctUntilChanged, first, retry, switchMap, tap, catchError, takeUntil } from 'rxjs/operators';
import { PageSizeCommon, SearchType, AdvanceSearchType, DefaultDateType, bookingSearchtypelst, bookingAdvanceSearchtypelst, datetypelst, UserType, BookingSearchRedirectPage, BookingStatus, APIService, InspectedStatusList, InspectionServiceType, Url, ListSize, SupplierType, Service, MobileViewFilterCount, SupplierNameTrim, BookingSummaryNameTrim, datetypes, supplierTypeList, BookingTypes, FbReportResultType } from '../../common/static-data-common'
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute, NavigationStart } from '@angular/router';
import { UserModel } from '../../../_Models/user/user.model';
import { QuotationFromBooking } from '../../../_Models/quotation/quotation.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { InspectionBookingsummarymodel, BookingItem, HolidayRequest, BookingInfo, BookingSummaryMasterData, BookingSummaryStatusResponseResult } from '../../../_Models/booking/inspectionbookingsummarymodel'//'src/app/_Models/Booking/inspectionBookingsummarymodel';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { TranslateService } from '@ngx-translate/core';
import { QuotationService } from 'src/app/_Services/quotation/quotation.service';
import { NgxGalleryOptions, NgxGalleryImage } from 'ngx-gallery-9';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { of, Subject } from 'rxjs';
import { CityListSearchRequest, CommonCustomerSourceRequest, CountryDataSourceRequest, ProvinceListSearchRequest, ProvinceResult, ResponseResult } from 'src/app/_Models/common/common.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ToastrService } from 'ngx-toastr';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
@Component({
  selector: 'app-booking-summary',
  templateUrl: './booking-summary.component.html',
  styleUrls: ['./booking-summary.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '0',
        'padding-bottom': '16px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ]),
    trigger('expandCollapseMobileAd', [
      state('open', style({
        'height': '*',
        'opacity': 1
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})

export class BookingSummaryComponent extends SummaryComponent<InspectionBookingsummarymodel> {
  @ViewChild('scrollableTable') scrollableTable: ElementRef;
  public model: InspectionBookingsummarymodel;
  public data: any;
  public searchloading: boolean = false;
  public error: string = "";
  public modelRef: NgbModalRef;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  bookingSearchtypelst: any = bookingSearchtypelst;
  datetypelst: any = datetypelst;
  Initialloading: boolean = false;
  suploading: boolean = false;
  factloading: boolean = false;
  public exportDataLoading = false;
  bookingAdvanceSearchtypelst: any = bookingAdvanceSearchtypelst;
  public _rowCheckBoxSelected: any[] = [];

  public _customvalidationforbookid: boolean = false;
  _booksearttypeid = SearchType.BookingNo;
  public _statuslist: any[] = [];
  public _quotationStatusList: any[] = [];
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  _IsCustomerUser: boolean = false;
  _IsInspectionConfirmed: boolean = false;
  _bookingredirectpage = BookingSearchRedirectPage;
  _redirectpath: string;
  private currentRoute: Router;
  public _redirecttype: any;
  _BookingStatus = BookingStatus;
  _InspectedStatusList = InspectedStatusList;
  _searchbookingtid: any;
  isReInspection: boolean = false;
  disableStatusList: boolean = false;
  isReBooking: boolean = false;
  isQuotationPending: boolean = false;
  isBookingSelected: boolean = false;
  cancelLoading: boolean;
  titlePopup: string;
  public _quotationloading: boolean = false;
  @ViewChild('cancelresheduleBooking') cancelresheduleBooking: ElementRef;
  public isRequestedSummary: boolean;
  public isVerifiedSummary: boolean;
  toggleFormSection: boolean;
  public productGalleryOptions: NgxGalleryOptions[];
  public productGalleryImages: NgxGalleryImage[] = [];
  public aeList: any[] = [];
  public containerMasterList: any[] = [];
  public inspectionServiceType = InspectionServiceType;
  bookingInfo: BookingInfo;
  bookingTypes = BookingTypes;
  isFilterOpen: boolean;
  bookingMasterData: BookingSummaryMasterData;
  userTypeEnum = UserType;
  countryRequest: CountryDataSourceRequest;
  brandSearchRequest: CommonCustomerSourceRequest;
  deptSearchRequest: CommonCustomerSourceRequest;
  collectionSearchRequest: CommonCustomerSourceRequest;
  buyerSearchRequest: CommonCustomerSourceRequest;
  priceCategoryRequest: CommonCustomerSourceRequest;
  componentDestroyed$: Subject<boolean> = new Subject();
  filterDataShown: boolean;
  filterCount: number;
  supplierTypeList: any = supplierTypeList;
  fbReportResult: any = FbReportResultType;
  getData(): void {
    this.GetSearchData(false);
  }
  getPathDetails(): string {

    return this._redirectpath;
  }

  constructor(private service: BookingService, public validator: Validator, router: Router, private qu_service: QuotationService,
    route: ActivatedRoute, authserve: AuthenticationService, public accService: UserAccountService, public modalService: NgbModal, public calendar: NgbCalendar,
    public dateparser: NgbDateParserFormatter, public pathroute: ActivatedRoute, public utility: UtilityService, translate: TranslateService,
    public locationService: LocationService,
    public officeService: OfficeService,
    public brandService: CustomerBrandService,
    public deptService: CustomerDepartmentService,
    public collectionService: CustomerCollectionService,
    public buyerService: CustomerbuyerService,
    public referenceService: ReferenceService,
    public toastr: ToastrService,
    public customerService: CustomerService, private supService: SupplierService,
    public refService: ReferenceService) {
    super(router, validator, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this._IsCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;
    this.currentRoute = router;
    this.isRequestedSummary = false;
    this.isVerifiedSummary = false;
    this.toggleFormSection = false;
    this.isFilterOpen = true;
    this.getIsMobile();
  }
  onInit(id?: any): void {
    var type = this.pathroute.snapshot.paramMap.get("type");
    this.isRequestedSummary = this.currentRoute.url.indexOf("booking-pendingverification") >= 0;
    this.isVerifiedSummary = this.currentRoute.url.indexOf("booking-pendingconfirmation") >= 0;

    //decides reinspection/rebooking/quotation pending
    if (type) {
      this.isReInspection = (type == "1") ? true : false;
      this.isReBooking = (type == "2") ? true : false;
      this.isQuotationPending = (type == "3") ? true : false;
    }

    //get the booking id if it navigates after returns from edit booking save/update
    this._searchbookingtid = this.pathroute.snapshot.paramMap.get("id");
    //remove first date for external user
    if (!this._IsInternalUser) {
      this.datetypelst = datetypelst.filter(x => x.id != datetypes.FirstserviceDate);
    }
    //initialize the booking summary source values
    this.initialize();
    this.validator.setJSON("booking/booking-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
    this.productGalleryOptions =
      [

        {
          "previewRotate": true, "thumbnails": false, "imageArrows": false, width: '800px',
          height: '500px'
        }

      ];
    this.getInspectionBookingTypes(id);
  }

  //initialize the objects
  objectInitialization() {
    this.model = new InspectionBookingsummarymodel();


    this.model.statusidlst = [];

    this.bookingInfo = new BookingInfo();
    this.validator.isSubmitted = false;
    this.Initialloading = true;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.advancedSearchtypeid = AdvanceSearchType.ProductName;
    this.model.pageSize = 10;
    this.model.index = 0;
    this.data = [];
    this._statuslist = [];
    this._quotationStatusList = [];
    this.bookingMasterData = new BookingSummaryMasterData();
    this.countryRequest = new CountryDataSourceRequest();
    this.brandSearchRequest = new CommonCustomerSourceRequest();
    this.deptSearchRequest = new CommonCustomerSourceRequest();
    this.collectionSearchRequest = new CommonCustomerSourceRequest();
    this.buyerSearchRequest = new CommonCustomerSourceRequest();
    this.priceCategoryRequest = new CommonCustomerSourceRequest();
    this.filterDataShown = false;
    this.bookingMasterData.DateName = datetypelst[0].name;
    this.isShownColumn();
  }

  //search by bookingid
  searchByBookingId() {
    if (this._searchbookingtid) {
      this.model.searchtypetext = this._searchbookingtid;
      this.GetSearchData(false);
    }
  }

  initialize() {
    //initialize the objects
    this.objectInitialization();
    this.bookingMasterData.entityId= parseInt(this.utility.getEntityId());
    this.assignSupplierDetails();

    this.bookingMasterData.selectedNumber = this.bookingSearchtypelst.find(x => x.id == 1) ?
      this.bookingSearchtypelst.find(x => x.id == 1).shortName : "";

    this.bookingMasterData.selectedNumberPlaceHolder = this.bookingSearchtypelst.find(x => x.id == 1) ?
      "Enter " + this.bookingSearchtypelst.find(x => x.id == 1).name : "";

    //search by booking id
    this.searchByBookingId();

    //set quotation status by default
    this.bookingMasterData.quotationStatusList.push({ id: 10, name: "Quotation Pending" });

    //populate the container name list
    this.getContainerList();

    this.getCountryListBySearch();
    this.getOfficeLocationList();
    this.getStatusList();
    this.getAEUserList();
    this.GetServiceTypelist();
  }

  ngAfterViewInit() {
    //get the customer details
    this.getCustomerListBySearch();
    if (this._IsCustomerUser && !this.model.customerid)
      this.model.customerid = this.currentUser.customerid;
    //get the customer based details
    this.getCustomerBasedDetails(this.model.customerid);
  }

  getCustomerBasedDetails(customerId) {
    if (customerId) {
      this.getSupplierListBySearch();
      if (this.model.supplierid)
        this.getFactoryListBySearch();
      this.GetServiceTypelist();
      this.brandSearchRequest.customerId = customerId;
      this.deptSearchRequest.customerId = customerId;
      this.buyerSearchRequest.customerId = customerId;
      this.collectionSearchRequest.customerId = customerId;
      this.priceCategoryRequest.customerId = customerId;
      this.getBrandListBySearch();
      this.getBuyerListBySearch();
      this.getCollectionListBySearch();
      this.getDeptListBySearch();
      this.getPriceCategoryListBySearch();

      var customerDetails = this.bookingMasterData.customerList.find(x => x.id == customerId);
      if (customerDetails)
        this.bookingMasterData.customerName =
          customerDetails.name.length > BookingSummaryNameTrim ?
            customerDetails.name.substring(0, BookingSummaryNameTrim) + "..." : customerDetails.name;
    }
    else {
      this.bookingMasterData.customerName = "";
    }
  }

  assignSupplierDetails() {
    this.bookingMasterData.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    this.model.supplierTypeId = SearchType.SupplierName;
  }

  ChangeCustomer(cusitem) {

    if (cusitem != null && cusitem.id != null) {
      this.assignSupplierDetails();
      //clear the list
      this.bookingMasterData.brandList = [];
      this.bookingMasterData.deptList = [];
      this.bookingMasterData.supplierList = [];
      this.bookingMasterData.factoryList = [];
      this.bookingMasterData.collectionList = [];
      this.bookingMasterData.buyerList = [];
      this.bookingMasterData.priceCategoryList = [];
      //clear the selected values
      this.model.selectedBrandIdList = [];
      this.model.selectedDeptIdList = [];
      this.model.supplierid = null;
      this.model.selectedCollectionIdList = [];
      this.model.selectedBuyerIdList = [];
      this.model.selectedPriceCategoryIdList = [];
      this.model.factoryidlst = [];
      this.model.serviceTypelst = [];

      this.getCustomerBasedDetails(cusitem.id);

    }

  }

  SaveMasterDataToFb(bookingId) {
    this.service.SaveMasterDataToFb(bookingId)
      .subscribe(
        response => {
          //  this.s("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");  
          // if(response!=null)
          // {
          //   //window.open("https://testing.qualitycontrol.tools/api/backend/reports?token="+response.fbUserId+"", "_blank");
          // }

          alert("success");
        },
        error => {
          this.suploading = false;
        }
      );
  }

  getCustomerListBySearch() {

    //push the customerid to  customer id list 
    if (this.model.customerid) {
      this.bookingMasterData.requestCustomerModel.idList.push(this.model.customerid);
    }
    else {
      this.bookingMasterData.requestCustomerModel.idList = null;
    }

    this.bookingMasterData.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerListByUserType(this.bookingMasterData.requestCustomerModel, null, term)
        : this.customerService.getCustomerListByUserType(this.bookingMasterData.requestCustomerModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.customerLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.customerList = data;
        //this.model.customerid = this.bookingMasterData.customerList[0].id;
        this.bookingMasterData.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.bookingMasterData.requestCustomerModel.searchText = this.bookingMasterData.customerInput.getValue();
      this.bookingMasterData.requestCustomerModel.skip = this.bookingMasterData.customerList.length;
    }

    this.bookingMasterData.customerLoading = true;
    this.customerService.getCustomerListByUserType(this.bookingMasterData.requestCustomerModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.bookingMasterData.customerList = this.bookingMasterData.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.bookingMasterData.requestCustomerModel.skip = 0;
          this.bookingMasterData.requestCustomerModel.take = ListSize;
        }
        this.bookingMasterData.customerLoading = false;
      }),
      error => {
        this.bookingMasterData.customerLoading = false;
        this.setError(error);
      };
  }

  getSupplierListBySearch() {
    this.bookingMasterData.supsearchRequest.supplierId = null;
    if (this.model.customerid)
      this.bookingMasterData.supsearchRequest.customerId = this.model.customerid;
    this.bookingMasterData.supsearchRequest.supplierType = SupplierType.Supplier;
    if (this.model.supplierid)
      this.bookingMasterData.supsearchRequest.supplierId = this.model.supplierid;
    this.bookingMasterData.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.supLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.bookingMasterData.supsearchRequest, term)
        : this.supService.getFactoryDataSourceList(this.bookingMasterData.supsearchRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.supLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.supplierList = data;
        this.bookingMasterData.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.bookingMasterData.supsearchRequest.searchText = this.bookingMasterData.supInput.getValue();
    this.bookingMasterData.supsearchRequest.skip = this.bookingMasterData.supplierList.length;

    this.bookingMasterData.supsearchRequest.customerId = this.model.customerid;
    this.bookingMasterData.supsearchRequest.supplierType = SupplierType.Supplier;
    this.bookingMasterData.supLoading = true;
    this.supService.getFactoryDataSourceList(this.bookingMasterData.supsearchRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.bookingMasterData.supplierList = this.bookingMasterData.supplierList.concat(customerData);
        }
        this.bookingMasterData.supsearchRequest.skip = 0;
        this.bookingMasterData.supsearchRequest.take = ListSize;
        this.bookingMasterData.supLoading = false;
      }),
      (error: any) => {
        this.bookingMasterData.supLoading = false;
      };
  }

  applyFactoryRelatedFilters() {
    this.bookingMasterData.facsearchRequest.customerIds = [];
    this.bookingMasterData.facsearchRequest.supplierIds = [];
    if (this.model.customerid)
      this.bookingMasterData.facsearchRequest.customerIds.push(this.model.customerid);
    this.bookingMasterData.facsearchRequest.supplierType = SupplierType.Factory;
    if (this.model.supplierid)
      this.bookingMasterData.facsearchRequest.supplierIds.push(this.model.supplierid);
    else
      this.bookingMasterData.facsearchRequest.supplierIds = null;
  }

  getFactoryListBySearch() {

    this.applyFactoryRelatedFilters();
    this.bookingMasterData.facInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.facLoading = true),
      switchMap(term => term
        ? this.supService.getSupplierDataSource(this.bookingMasterData.facsearchRequest, null, term)
        : this.supService.getSupplierDataSource(this.bookingMasterData.facsearchRequest, null)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.facLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.factoryList = data;
        this.bookingMasterData.facLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData() {

    this.applyFactoryRelatedFilters();
    this.bookingMasterData.facsearchRequest.searchText = this.bookingMasterData.facInput.getValue();
    this.bookingMasterData.facsearchRequest.skip = this.bookingMasterData.factoryList.length;
    this.bookingMasterData.facLoading = true;
    this.supService.getSupplierDataSource(this.bookingMasterData.facsearchRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.bookingMasterData.factoryList = this.bookingMasterData.factoryList.concat(customerData);
        }
        this.bookingMasterData.facsearchRequest.skip = 0;
        this.bookingMasterData.facsearchRequest.take = ListSize;
        this.bookingMasterData.facLoading = false;
      }),
      (error: any) => {
        this.bookingMasterData.facLoading = false;
      };
  }

  //get the office list
  getOfficeLocationList() {
    this.bookingMasterData.officeLoading = true;
    this.officeService.getOfficeDetails()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processOfficeLocationResponse(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.officeLoading = false;
        });
  }

  //process the office location response
  processOfficeLocationResponse(response) {
    if (response && response.result == ResponseResult.Success)
      this.bookingMasterData.officeList = response.dataSourceList;
    else if (response && response.result == ResponseResult.NoDataFound)
      this.showError('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.TITLE.MSG_OFFICE_NOT_FOUND');
    this.bookingMasterData.officeLoading = false;
  }

  getAEUserList() {
    this.bookingMasterData.aeUserLoading = true;
    this.service.getAEUserList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.bookingMasterData.aeUserList = response.dataSourceList;
          this.bookingMasterData.aeUserLoading = false;

        },
        error => {
          this.setError(error);
          this.bookingMasterData.aeUserLoading = false;
        });
  }

  //assign the enable the booking status for the reinspection
  assignReInspectionStatus() {
    this.model.statusidlst = [];
    for (const status in this._InspectedStatusList) {
      if (Number(status))
        this.model.statusidlst.push(Number(status));
      this.model.statusidlst.push(Number(BookingStatus.AllocateQC));
    }
    this.disableStatusList = true;
  }

  //get the booking status and quotation status list
  getSummaryStatusList() {
    this.service.getBookingSummaryStatusList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processSummaryStatusList(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.statusLoading = false;
        });
  }

  //process the summary status list
  processSummaryStatusList(response) {
    if (response && response.result == BookingSummaryStatusResponseResult.Success) {
      this.bookingMasterData.bookingStatusList = response.statusList;
      this.bookingMasterData.quotationStatusList = response.quotationStatusList;
      if (this.isReInspection)
        this.assignReInspectionStatus();

      if (this.isVerifiedSummary || this.isRequestedSummary || this.isQuotationPending) {
        this.checkRequestStatus();
      }
      this.bookingMasterData.statusLoading = false;
    }
    else if (response && response.result == BookingSummaryStatusResponseResult.StatusListNotFound) {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'BOOKING_SUMMARY.MSG_BOOKING_STATUS_NOT_FOUND');
    }
    else if (response && response.result == BookingSummaryStatusResponseResult.QuotationStatusListNotFound) {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'BOOKING_SUMMARY.MSG_QUOTATION_STATUS_NOT_FOUND');
    }
  }

  getInspectionBookingTypes(id) {
    this.refService.getInspectionBookingTypes()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processInspectionBookingTypes(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.inspectionBookingTypeVisible = false;
        }
      );
  }

  //process the inspection type
  processInspectionBookingTypes(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.inspectionBookingTypes = response.inspectionBookingTypeList;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.inspectionBookingTypeVisible = false;
    }
  }

  //get the booking status and quotation status list
  getStatusList() {
    this.bookingMasterData.statusLoading = true;
    this.getSummaryStatusList();
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  ChangeSupplier(supitem) {
    this.model.factoryidlst = [];
    if (supitem != null && supitem.id != null) {
      this.getFactoryListBySearch();

      var supplierDetails = this.bookingMasterData.supplierList.find(x => x.id == supitem.id);
      if (supplierDetails)
        this.bookingMasterData.supplierName =
          supplierDetails.name.length > BookingSummaryNameTrim ?
            supplierDetails.name.substring(0, BookingSummaryNameTrim) + "..." : supplierDetails.name;
    }
    else {
      this.bookingMasterData.supplierName = "";
    }
  }
  GetSupplierlist(id) {
    if (id) {
      this.suploading = true;
      this.service.Getsupplierbycusid(id)
        .subscribe(
          response => {
            this.GetSupplierlistSuccessResponse(response);
          },
          error => {
            this.suploading = false;
          }
        );
    }
    else {
      this.data.factoryList = [];
      this.model.factoryidlst = [];
      this.data.supplierList = [];
      this.model.supplierid = null;
    }
  }
  GetSupplierlistSuccessResponse(response) {
    if (response && response.result == 1) {
      this.bookingMasterData.supplierList = response.data;
    }
    else {
      this.bookingMasterData.supplierList = [];
    }
    this.model.supplierid = null;
    this.data.factoryList = [];
    this.model.factoryidlst = [];
    this.suploading = false;
  }
  GetFactoryList(supId, cusId) {
    if (supId && cusId) {
      this.factloading = true;
      this.service.GetFactoryDetailsByCusSupId(supId, cusId)
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.data.factoryList = response.data;
            }
            else
              this.data.factoryList = [];
            this.model.factoryidlst = [];
            this.factloading = false;
          },
          error => {
            this.factloading = false;
          }
        );
    }
    else {
      this.data.factoryList = [];
      this.model.factoryidlst = [];
    }
  }
  BookingNoValidation(bookingText) {
    this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && bookingText && bookingText.trim() != "" && ((isNaN(Number(bookingText))) || (bookingText.trim().length > 9));

    this.bookingMasterData.isShowSearchLens = bookingText && bookingText.trim() != "";
  }


  Reset() {
    this.initialize();
    this.ngAfterViewInit();
  }
  SetSearchTypemodel(item) {
    this.model.searchtypeid = item.id;
    this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
    this.bookingMasterData.selectedNumber = item.shortName;

    if (item.id == SearchType.BookingNo) {
      this.bookingMasterData.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.PoNo) {
      this.bookingMasterData.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.CustomerBookingNo) {
      this.bookingMasterData.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.ProductId) {
      this.bookingMasterData.selectedNumberPlaceHolder = "Enter " + item.name;
    }
  }
  SetSearchDatetype(item) {
    this.model.datetypeid = item.id;
    this.bookingMasterData.DateName = item.name;
  }

  SetSearchTypeText(searchtypetext) {
    this.model.advancedSearchtypeid = searchtypetext;
  }
  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
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
  formValid(): boolean {
    let isOk = !this._customvalidationforbookid && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
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

  getSearchDetails() {
    var searchLoading = true;
    if (this.model.searchtypetext && this.model.searchtypetext != '') {
      if (!this.isFilterOpen)
        this.isFilterOpen = this.model.searchtypeid != this._booksearttypeid && !(this.model.customerid > 0);
      this.SearchDetails();
    }
  }

  GetSearchData(searchLoading: boolean) {
    this.searchloading = true;
    //open the filter
    if (this.isFilterOpen && !this.isQuotationPending)
      this.isFilterOpen = false;
    this.filterDataShown = this.filterTextShown();
    this.service.SearchInspectionBookingSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this._statuslist = response.inspectionStatuslst;
            this._quotationStatusList = response.quotationStatuslst;
            this.model.isBookingRequestRole = response.internalUserRole.isBookingRequestRole;
            this.model.isBookingConfirmRole = response.internalUserRole.isBookingConfirmRole;
            this.model.isBookingVerifyRole = response.internalUserRole.isBookingVerifyRole;
            this.model.isQuotationRequestRole = response.internalUserRole.isQuotationRequestRole;
            this.model.items = response.data.map((x) => {
              var _cancelbtnshow = this.isCancelVisible(x);
              var _rescheduleBtnShow = this.isRescheduleVisible(x);
              var _editBtnText = this.isEditShow(x);
              var _editImagePath = this.isEditImagePath(x);
              var _quotationStatusName = '';
              var item: BookingItem = {
                bookingId: x.bookingId,
                poNumber: x.poNumber,
                serviceDateFrom: x.serviceDateFrom,
                serviceDateTo: x.serviceDateTo,
                firstServiceDateFrom: x.firstServiceDateFrom,
                firstServiceDateTo: x.firstServiceDateTo,
                serviceTypeId: x.serviceTypeId,
                statusId: x.statusId,
                bookingCreatedBy: x.bookingCreatedBy,
                customerId: x.customerId,
                internalReferencePo: x.internalReferencePo,
                isPicking: x.isPicking,
                isEAQF: x.isEAQF,
                previousBookingNo: x.previousBookingNo,
                factoryId: x.factoryId,
                cancelBtnShow: _cancelbtnshow,
                rescheduleBtnShow: _rescheduleBtnShow,
                editBtnText: _editBtnText,
                countryId: x.countryId,
                quotationStatus: x.quotationStatusName,
                isQuotSelected: false,
                productCategory: x.productCategory,
                supplierId: x.supplierId,
                officeId: x.officeId,
                isPickingButtonVisible: x.isPickingButtonVisible,
                isSplitBookingButtonVisible: x.isSplitBookingButtonVisible,
                applyDate: x.applyDate,
                productList: [],
                statusList: [],
                reportSummaryLink: x.reportSummaryLink,
                customerBookingNo: x.customerBookingNo,
                deptNames: x.deptNames,
                editImagePath: _editImagePath,
                isCombineVisible: x.isCombineVisible,
                productCount: x.productCount,
                isRowSelected: false,
                statusName: x.statusName,
                bookingType: x.bookingType,
                serviceType: x.serviceType && x.serviceType.length > 40 ? x.serviceType : "", // length 40
                csName: x.csName && x.csName.length > 24 ? x.csName : "", //24 length
                createdByName: x.bookingCreatedByName && x.bookingCreatedByName.length > 41 ? x.bookingCreatedByName : "", //41 length
                office: x.office && x.office > 31 ? x.office : "", //31 length

                serviceTypeTrim: x.serviceType && x.serviceType.length > 40 ? x.serviceType.substring(0, 40) + "..." : x.serviceType, // length 40
                AETrim: x.csName && x.csName.length > 24 ? x.csName.substring(0, 24) + "..." : x.csName, //24 length
                CreatedByTrim: x.bookingCreatedByName && x.bookingCreatedByName.length > 41 ? x.bookingCreatedByName.substring(0, 41) +
                  "..." : x.bookingCreatedByName, //41 length
                OfficeTrim: x.office && x.office > 31 ? x.office.substring(0, 31) + "..." : x.office, //31 length

                isServiceTypeShow: x.serviceType && x.serviceType.length > 40,
                isAETooltipShow: x.csName && x.csName.length > 24,
                isCreatedByShow: x.bookingCreatedByName && x.bookingCreatedByName.length > 41,
                isOfficeTooltipShow: x.office && x.office > 31,

                customerName: x.customerName.length > 46 ?
                  x.customerName : "",
                supplierName: x.supplierName.length > 46 ?
                  x.supplierName : "",
                factoryName: x.factoryName && x.factoryName.length > 46 ?
                  x.factoryName : "",
                customerNameTrim: x.customerName.length > 46 ?
                  x.customerName.substring(0, 46) + "..." : x.customerName,
                supplierNameTrim: x.supplierName.length > 46 ?
                  x.supplierName.substring(0, 46) + "..." : x.supplierName,
                factoryNameTrim: x.factoryName && x.factoryName.length > 46 ?
                  x.factoryName.substring(0, 46) + "..." : x.factoryName,
                isCustomerTooltipShow: x.customerName.length > 46,
                isSupplierTooltipShow: x.supplierName.length > 46,
                isFactoryTooltipShow: x.factoryName && x.factoryName.length > 46,
                bookingCreatedFirstName: x.bookingCreatedFirstName?.trim(),
                isAllProductSelected: false,
                productRefId: x.productRefId
              }
              return item;
            });
            this.bookingMasterData.setIconWidth = this.iconShowMorethanOneIncreaseWidth();
            setTimeout(() => {
              this.limitTableWidth();
            }, 500);
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this._statuslist = [];
          }
          else {
            this.error = response.result;
          }
          this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.searchloading = false;
        });
  }

  //set icon width
  iconShowMorethanOneIncreaseWidth(): string {
    var setIconWidth: string;
    this.model.items.forEach(element => {
      if ((this._IsInternalUser && element.statusId != BookingStatus.Cancel &&
        element.isCombineVisible &&
        element.serviceTypeId != InspectionServiceType.Container)
        || element.isPickingButtonVisible) {
        setIconWidth = "width-100";
        return setIconWidth;
      }
      else {
        setIconWidth = "width-60";
      }
    });
    return setIconWidth;
  }

  Getbookingids(item) {
    var selectedcount = this.model.items.filter(x => x.isQuotSelected).length;
    if (selectedcount && selectedcount > 0) {
      var count = this.model.items.filter(x => x.isQuotSelected && x.customerId == item.customerId && x.supplierId == item.supplierId && x.factoryId == item.factoryId).length;
      if ((count == selectedcount || count == 0)) {
        this.isBookingSelected = true;
      }
      else {
        this.model.items.filter(x => x.bookingId == item.bookingId)[0].isQuotSelected = false;
        this.isBookingSelected = false;
        this.showError('BOOKING_SUMMARY.LBL_ERROR', 'BOOKING_SUMMARY.MSG_QUOERROR');
      }
    }
    else {
      this.isBookingSelected = false;
    }
  }


  GetQuotationStatusColor(statusid?) {
    if (this._quotationStatusList != null && this._quotationStatusList.length > 0 && statusid != null) {
      var result = this._quotationStatusList.find(x => x.statusName == statusid);
      if (result)
        return result.statusColor;
    }
  }

  clickStatus(id) {
    if (id && id > 0) {

      //if it contains the value
      var isValueExists = this.model.statusidlst.includes(id);

      //open the filter
      if (!this.isFilterOpen)
        this.isFilterOpen = true;

      //open the advance search
      //      if (!this.toggleFormSection)
      //        this.toggleFormSection = true;
      this.toggleFormSection = false;

      if (isValueExists) {
        this.model.statusidlst = this.model.statusidlst.filter(x => x != id);
        this.model.statusidlst = [...this.model.statusidlst];

      }
      else {
        this.model.statusidlst.push(id);
        this.model.statusidlst = [...this.model.statusidlst];
      }
    }
  }

  changeStatus(id) {
    return this.model.statusidlst.includes(id);
  }

  GetStatusColor(statusid?) {
    if (this._statuslist != null && this._statuslist.length > 0 && statusid != null) {
      var result = this._statuslist.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }
  export() {
    this.exportDataLoading = true;
    this.service.exportSummary(this.model)
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
      window.navigator.msSaveOrOpenBlob(blob, "booking_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "booking_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }
  RedirectPage(type, item) {
    switch (type) {
      case this._bookingredirectpage.Edit:
        {
          this._redirectpath = "inspedit/edit-booking";
          super.getDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.Cancel:
        {
          //have to control 1 working day before for external user show popup
          this.holidayExistsShowPopup(item, this._bookingredirectpage.Cancel);
          break;
        }
      case this._bookingredirectpage.Reschedule:
        {
          this.holidayExistsShowPopup(item, this._bookingredirectpage.Reschedule);
          break;
        }
      case this._bookingredirectpage.Report:
        {
          this._redirectpath = "booking-report";
          super.getDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.Schedule:
        {
          this._redirectpath = "inspedit/edit-booking";
          this._redirecttype = this._bookingredirectpage.Schedule;
          this.RedirectTo(item.bookingId, this.model);
          break;
        }
      case this._bookingredirectpage.CombineProduct:
        {
          this._redirectpath = "inspcombine/edit-combineorders";
          this.getCombineProductDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.SplitBooking:
        {
          this._redirectpath = "inspsplit/split-booking";
          this.getCombineProductDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.ReInspectionBooking:
        {
          this._redirectpath = "inspedit/editreinspection-booking";
          this._redirecttype = this._bookingredirectpage.ReInspectionBooking;
          this.RedirectTo(item.bookingId, this.model);
          break;
        }
      case this._bookingredirectpage.ReBooking:
        {
          this._redirectpath = "inspedit/editre-booking";
          this._redirecttype = this._bookingredirectpage.ReBooking;
          this.RedirectTo(item.bookingId, this.model);
          break;
        }
      case this._bookingredirectpage.Quotation:
        {
          this._redirectpath = "quotations/new-quotation";
          this._redirecttype = this._bookingredirectpage.Quotation;
          this.MapQuotationmodel();
          this.RedirectTo(0, this.model, true);
          break;
        }
      case this._bookingredirectpage.ViewBookingForQuotation:
        {
          this._redirectpath = "inspedit/viewQuotebooking";
          this._redirecttype = this._bookingredirectpage.ViewBookingForQuotation;
          this.RedirectTo(item.bookingId, this.model);
          break;
        }
    }
  }
  MapQuotationmodel() {
    try {
      this._quotationloading = true;
      var data = this.model.items.filter(x => x.isQuotSelected);
      if (data) {
        var bookid = [];
        data.forEach(x => bookid.push(x.bookingId));
        var item: QuotationFromBooking = {
          customerid: data[0].customerId,
          factorycountryid: data[0].countryId,
          factoryid: data[0].factoryId,
          supplierid: data[0].supplierId,
          bookingitems: bookid,
          service: APIService.Inspection,
          officeid: data[0].officeId
        }
        this.model.Quotdetails = item;
      }
      else
        this.model.Quotdetails = null;
    }
    catch (e) {
      this._quotationloading = false;
    }
  }
  //HolidayRequest map the model value
  mapModel(item, serviceFromDate): HolidayRequest {
    var model: HolidayRequest = {
      factoryCountryId: item.countryId > 0 ? item.countryId : 0,
      factoryId: item.factoryId,
      serviceDateFrom: serviceFromDate
    };
    return model;
  }
  //cancel/reshedule page redirect
  redirectPage(bookingId, type) {
    this._redirectpath = "inspcancel/cancel-booking";
    this._redirecttype = type == this._bookingredirectpage.Cancel ?
      this._bookingredirectpage.Cancel :
      this._bookingredirectpage.Reschedule;
    this.RedirectTo(bookingId, this.model);
  }
  //before one working day logic
  holidayExistsShowPopup(item, type) {
    var todaydate = this.calendar.getToday()
    var date = this.dateparser.parse(item.serviceDateFrom);
    var serviceFromDate = new NgbDate(date.year, date.month, date.day);
    var servicedaybefore = todaydate.after(serviceFromDate);
    var servicedateequaltodaydate = todaydate.equals(serviceFromDate)
    if (this._IsInternalUser) {
      this.redirectPage(item.bookingId, type);
    }
    else if (servicedaybefore || servicedateequaltodaydate) {
      this.infoPopup(this.cancelresheduleBooking, type);
    }
    else {
      this.cancelLoading = true;
      this.isHolidayExists(this.mapModel(item, serviceFromDate), type, item.bookingId);
    }
  }
  //ajax call for get holiday condition based on holidays and service date
  isHolidayExists(model: HolidayRequest, type, bookingId) {
    this.service.IsHolidayExists(model)
      .pipe()
      .subscribe(
        res => {
          this.isHolidayExistsSuccessHandler(res, type, bookingId);
        },
        error => {
          this.cancelLoading = false;
          this.setError(error);
        });
  }
  //isHolidayExists - success handler
  isHolidayExistsSuccessHandler(res, type, bookingId) {
    if (!res) {
      this.infoPopup(this.cancelresheduleBooking, type);
    }
    else {
      this.redirectPage(bookingId, type);
    }
    this.cancelLoading = false;
  }
  infoPopup(content, type) {
    this.titlePopup = (this._bookingredirectpage.Cancel == type) ?
      this.utility.textTranslate('BOOKING_SUMMARY.LBL_CANCEL')
      : this.utility.textTranslate('BOOKING_SUMMARY.LBL_POSTPONE');
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  RedirectTo(id, model, customnavigate: boolean = false) {
    var data = Object.keys(model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = model[item];
    }
    if (!customnavigate) {
      this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}/${this._redirecttype}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
    }
    else {
      this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
    }
  }
  isRescheduleVisible(bookingItem) {
    var date = this.dateparser.parse(bookingItem.serviceDateFrom);
    var serviceFromDate = new NgbDate(date.year, date.month, date.day);
    var todayDate = this.calendar.getToday();
    var afterToday = todayDate.before(serviceFromDate);
    var isRescheduleVisble = false;
    if (bookingItem.statusId != BookingStatus.Cancel && !this.utility.checkBookingStatus(bookingItem.statusId)
      && bookingItem.statusId != BookingStatus.Requested) {
      if (this._IsInternalUser && this.model.isBookingConfirmRole && (afterToday
        || todayDate.equals(serviceFromDate))) { //  service from date greater than or equal to today date
        isRescheduleVisble = true;
      }
      else if (!this._IsInternalUser &&
        bookingItem.bookingCreatedByType == this.currentUser.usertype && afterToday) {
        isRescheduleVisble = true;
      }

      //Reschedule button has to be visible even after service date but if the status is verified & Confirmed 
      else if (!afterToday && this._IsInternalUser && this.model.isBookingConfirmRole && (bookingItem.statusId == BookingStatus.Verified || bookingItem.statusId == BookingStatus.Confirmed || bookingItem.statusId == BookingStatus.AllocateQC || bookingItem.statusId == BookingStatus.Rescheduled))
        isRescheduleVisble = true;
    }
    return isRescheduleVisble;
  }
  isCancelVisible(bookingItem) {
    var date = this.dateparser.parse(bookingItem.serviceDateFrom);
    var serviceFromDate = new NgbDate(date.year, date.month, date.day);
    var isCancelVisible = false;
    if (!this.utility.checkBookingStatus(bookingItem.statusId)) {
      if (this._IsInternalUser &&
        ((bookingItem.statusId == BookingStatus.Requested &&
          (this.model.isBookingRequestRole || this.model.isBookingVerifyRole)) ||
          ((bookingItem.statusId != BookingStatus.Requested) && this.model.isBookingVerifyRole))) {
        isCancelVisible = true;
      }
      else if (!this._IsInternalUser &&
        bookingItem.statusId != BookingStatus.Cancel) {
        if (bookingItem.bookingCreatedBy == this.currentUser.id
          && (this.calendar.getToday().before(serviceFromDate))) {
          isCancelVisible = true;
        }
      }
    }
    return isCancelVisible;
  }

  isEditShow(bookingItem) {
    var editTextBtn = "Edit";
    if (this._IsInternalUser) {
      if (this.isQuotationPending && this.model.isQuotationRequestRole)
        editTextBtn = this.utility.textTranslate('BOOKING_SUMMARY.LBL_BTN_VIEW');
      if (this.model.isBookingConfirmRole && (bookingItem.statusId == BookingStatus.Hold || bookingItem.statusId == BookingStatus.Verified || bookingItem.statusId == BookingStatus.Rescheduled))
        editTextBtn = this.utility.textTranslate('BOOKING_SUMMARY.LBL_CONFIRM');
      else if (this.model.isBookingVerifyRole && bookingItem.statusId == BookingStatus.Requested)
        editTextBtn = this.utility.textTranslate('BOOKING_SUMMARY.LBL_VERIFY');
    }
    else if (!this._IsInternalUser) {
      if (bookingItem.bookingCreatedBy != this.currentUser.id)
        editTextBtn = this.utility.textTranslate('BOOKING_SUMMARY.LBL_BTN_VIEW');
    }
    return editTextBtn;
  }
  isEditImagePath(bookingItem) {
    var editImagePath = "assets/images/new-set/edit-dark.svg";
    if (this._IsInternalUser) {
      if (this.isQuotationPending && this.model.isQuotationRequestRole)
        editImagePath = "assets/images/new-set/view-dark.svg";
      if (this.model.isBookingConfirmRole && (bookingItem.statusId == BookingStatus.Hold || bookingItem.statusId == BookingStatus.Verified || bookingItem.statusId == BookingStatus.Rescheduled))
        editImagePath = "assets/images/new-set/conform-dark.svg";
      else if (this.model.isBookingVerifyRole && bookingItem.statusId == BookingStatus.Requested)
        editImagePath = "assets/images/new-set/verify-dark.svg";
    }
    else if (!this._IsInternalUser) {
      if (bookingItem.bookingCreatedBy != this.currentUser.id)
        editImagePath = "assets/images/new-set/view-dark.svg";
    }
    return editImagePath;
  }
  IsReportVisible(isheader, statusid) {
    if (this._IsInternalUser && isheader)
      return true;
    else if (this._IsInternalUser) {
      if (statusid == this._BookingStatus.Scheduled || statusid == this._BookingStatus.Confirmed)
        return true;
      else
        return false;
    }
    else
      return false;
  }
  IsScheduleBookingVisible(statusid) {
    if (this._IsInternalUser) {
      if (statusid != this._BookingStatus.Cancel && statusid != this._BookingStatus.Confirmed)
        return true;
      else
        return false;
    }
    else
      return false;
  }

  getCombineProductDetails(id) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  getBookingDetailsView(productList) {
    var containerId = 0;
    var reportId = 0
    var productData = [];

    if (productList && productList.length > 0) {
      //container 
      if (productList.filter(x => x.containerId != null && x.selected == true).length > 0) {
        productData = productList.filter(x => x.containerId != null && x.selected == true);
        if (productData.length == 1) {
          containerId = productData[0].containerId;
          reportId = productData[0].apiReportId;
        }
      }
      //products
      else if (productList.filter(x => x.containerId == null && x.isParentProduct && x.selected == true).length > 0) {
        productData = productList.filter(x => x.containerId == null && x.isParentProduct && x.selected == true);
        reportId = productData.length == 1 ? productData[0].reportId : 0;
      }
    }

    if (productData.length == 1) {

      var data = Object.keys(this.model);
      var currentItem: any = {};

      for (let item of data) {
        if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
          currentItem[item] = this.model[item];
      }
      this.currentRoute.navigate([`/${this.utility.getEntityName()}/bookingDetail/booking-detail/${productData[0].bookingId}/${reportId}/${containerId}`],
        { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
    }
    else {
      this.showWarning('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.LBL_SELECT_ONE_REPORT');
    }
  }

  getPickingProductDetails(id, customerId, statusId) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}/${customerId}/${statusId}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  redirectPickingProducts(bookingid, customerId, customerName, statusId) {
    this._redirectpath = "insppicking/edit-inspectionpicking";
    this.getPickingProductDetails(bookingid, customerId, statusId);
  }

  clearCustomer() {
    this.model.customerid = null;
    this.model.supplierid = null;
    this.bookingMasterData.supplierList = [];
    this.model.factoryidlst = [];
    this.model.serviceTypelst = null;
    this.bookingMasterData.serviceTypeList = [];
    this.bookingMasterData.brandList = [];
    this.bookingMasterData.deptList = [];
    this.bookingMasterData.buyerList = [];
    this.bookingMasterData.collectionList = [];
    this.bookingMasterData.priceCategoryList = [];
    this.bookingMasterData.factoryList = [];
    this.model.serviceTypelst = [];
    this.getCustomerListBySearch();
    this.GetServiceTypelist();
    this.assignSupplierDetails();
  }
  clearSupplier() {
    this.model.supplierid = null;
    this.data.factoryList = null;
    this.model.factoryidlst = [];
    this.getSupplierListBySearch();
  }

  checkRequestStatus() {
    if (this.isRequestedSummary) {
      this.model.statusidlst = this.bookingMasterData.bookingStatusList.filter(x => x.id == BookingStatus.Requested).map(x => x.id);
    }
    else if (this.isVerifiedSummary) {
      this.model.statusidlst = this.bookingMasterData.bookingStatusList.filter(x => x.id == BookingStatus.Verified).map(x => x.id);
    }
    else if (this.isQuotationPending) {
      this.model.statusidlst = this.bookingMasterData.bookingStatusList.filter(x => x.id == BookingStatus.Verified || x.id == BookingStatus.Confirmed || x.id == BookingStatus.AllocateQC || x.id == BookingStatus.Rescheduled).map(x => x.id);
      this.model.isQuotationSearch = true;
    }
    this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.model.todate = this.calendar.getNext(this.calendar.getToday(), 'm', 3);
    this.search();
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
    this.filterDataShown = false;
  }

  GetServiceTypelist() {
    this.bookingMasterData.serviceTypeLoading = true;
    let request = this.generateServiceTypeRequest();

    this.referenceService.getServiceTypes(request)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {

          this.bookingMasterData.serviceTypeList = response.serviceTypeList;
          this.bookingMasterData.serviceTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.bookingMasterData.serviceTypeList = [];
          this.bookingMasterData.serviceTypeLoading = false;
        }
      );
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerid ?? 0;
    serviceTypeRequest.serviceId = Service.Inspection;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  limitTableWidth() {
    let width = this.scrollableTable ?
      this.scrollableTable.nativeElement.offsetWidth : 0;
    if (width > 0 && width < this.scrollableTable.nativeElement.scrollWidth) {
      this.scrollableTable.nativeElement.classList.add('scroll-x');
    }
  }

  toggleExpandBooking(event, index, rowItem) {
    this.bookingMasterData.productList = [];
    rowItem.isPlaceHolderVisible = true;
    rowItem.productList = [];
    rowItem.statusList = [];
    rowItem.containerList = [];

    var firstElem = document.querySelector('[data-expand-id="booking' + index + '"]');
    firstElem.classList.toggle('open');

    if (firstElem.classList.contains('open')) {
      rowItem.isRowSelected = true;
      var content: any;
      this.getProductOrContainerListByBooking(rowItem, content, false);
    }
    else {
      rowItem.isRowSelected = false;
      rowItem.isPlaceHolderVisible = false;
      rowItem.isAllProductSelected = false;
    }
  }

  toggleExpandRowContainer(modalOpen, rowItem, content, bookingItem) {
    if (modalOpen)
      this.bookingMasterData.pageLoader = true;
    rowItem.isPlaceHolderVisible = true;
    rowItem.productList = [];
    this.getProductListByContainerAndBooking(modalOpen, rowItem, content, bookingItem);
  }


  toggleExpandRowProduct(modalOpen, event, rowItem, content) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.poList = [];
    var containerId = this.bookingMasterData.containerItem.id;
    var serviceTypeId = this.bookingMasterData.bookingItem.serviceTypeId;
    this.getPoListByBookingAndProduct(modalOpen, rowItem, containerId, serviceTypeId, content);
  }

  // get booking product list by booking id
  getProductOrContainerListByBooking(rowItem, content, isModalOpen) {

    if (rowItem.serviceTypeId != this.inspectionServiceType.Container) {
      this.service.GetProductDetailsByBooking(rowItem.bookingId)
        .subscribe(res => {
          this.bookingMasterData.pageLoader = false;
          if (res.result == 1) {
            rowItem.productList = res.bookingProductsList;
            rowItem.productList.forEach(element => {
              element.poNumberCountDisplay = element.poNumberCount - 1 > 0 ? ", +" + (element.poNumberCount - 1) : '';
              element.poNumberShow = element.poNumberList && element.poNumberList.length > 0 ? element.poNumberList[0] : '';
              element.productDescTrim = element.productDescription.length > 46 ?
                element.productDescription.substring(0, 46) + "..." : element.productDescription;
              element.isProductDescTooltipShow = element.productDescription.length > 46;
            });
            rowItem.statusList = res.bookingStatusList;
            this.bookingMasterData.statusList = res.bookingStatusList;
            this.bookingMasterData.productList = rowItem.productList;
            if (isModalOpen)
              this.modalService.open(content, { windowClass: 'order-tracking-wrapper stick-to-bottom' });

          }
        },
          error => {
            this.bookingMasterData.pageLoader = false;
            rowItem.isPlaceHolderVisible = false;
            this.exportDataLoading = false;
          });
    }
    else {
      this.service.GetContainerDetailsByBooking(rowItem.bookingId)
        .subscribe(res => {
          this.bookingMasterData.pageLoader = false;
          if (res.result == 1) {
            rowItem.containerList = res.bookingContainerList;
            rowItem.statusList = res.bookingStatusList;
            this.bookingMasterData.statusList = res.bookingStatusList;
            if (isModalOpen)
              this.modalService.open(content, { windowClass: 'order-tracking-wrapper stick-to-bottom' });
          }
        },
          error => {
            this.bookingMasterData.pageLoader = false;
            rowItem.isPlaceHolderVisible = false;
            this.exportDataLoading = false;
          });
    }
  }

  // get po list by booking and product
  getPoListByBookingAndProduct(modalOpen, rowItem, containerId, serviceTypeId, content) {
    if (serviceTypeId != this.inspectionServiceType.Container) {
      this.bookingMasterData.pageLoader = true;
      this.service.GetPODetailsByBookingAndProduct(rowItem.bookingId, rowItem.id)
        .subscribe(res => {
          this.bookingMasterData.pageLoader = false;
          if (res.result == 1) {
            rowItem.isPlaceHolderVisible = false;
            this.bookingMasterData.poList = res.bookingProductPoList;
            if (modalOpen)
              this.modalService.open(content, { windowClass: 'product-detail-wrapper stick-to-bottom' });
          }

        },
          error => {
            this.bookingMasterData.pageLoader = false;
            rowItem.isPlaceHolderVisible = false;
          });
    }
    else {
      this.service.GetPODetailsByBookingAndConatinerAndProduct(rowItem.bookingId, containerId, rowItem.id)
        .subscribe(res => {
          if (res.result == 1) {
            rowItem.poList = res.bookingProductPoList;
            this.bookingMasterData.productName = rowItem.productName;
            this.bookingMasterData.containerPOList = res.bookingProductPoList;
            this.bookingMasterData.bookingNumber = this.bookingMasterData.containerPOList && this.bookingMasterData.containerPOList[0] ?
              this.bookingMasterData.containerPOList[0].bookingId : '';
            this.bookingMasterData.isproductOrPODetails = false;
          }
        },
          error => {
            rowItem.isPlaceHolderVisible = false;
          });
    }
  }
  // get product list by container and booking
  getProductListByContainerAndBooking(modalOpen, rowItem, content, bookingItem) {
    this.service.GetProductListByBookingAndContainer(rowItem.bookingId, rowItem.id)
      .subscribe(res => {
        this.bookingMasterData.pageLoader = false;
        if (res.result == 1) {
          rowItem.productList = res.bookingProductList;
          this.bookingMasterData.containerProductList = res.bookingProductList;

          this.bookingMasterData.containerProductList.forEach(element => {
            element.productDescTrim = element.productDescription.length > 78 ?
              element.productDescription.substring(0, 78) + "..." : element.productDescription;
            element.isProductDescTooltipShow = element.productDescription.length > 78;
          });

          this.bookingMasterData.isproductOrPODetails = true;

          this.bookingMasterData.bookingItem = bookingItem;
          this.bookingMasterData.containerItem = rowItem;

          if (modalOpen)
            this.modalService.open(content, { windowClass: 'product-detail-wrapper stick-to-bottom' });
        }
      },
        error => {
          this.bookingMasterData.pageLoader = false;
          rowItem.isPlaceHolderVisible = false;

        });
  }

  downloadFbReport(productList) {
    if (productList && productList.length > 0) {
      productList.forEach(element => {
        //open non combine , combine and container reports
        if (element.selected == true && ((element.combineProductId != null && element.combineProductId == 0) ||
          (element.combineProductId != null && element.combineProductId > 0 && element.isParentProduct) ||
          (element.containerId != null && element.containerId > 0))) {
          if (element.finalManualReportPath && element.finalManualReportPath != "") {
            window.open(element.finalManualReportPath, "_blank");
          }
          else if (element.reportPath && element.reportPath != "") {
            window.open(element.reportPath, "_blank");
          }
        }
      });
    }
  }

  // create container list
  getContainerList() {

    for (let index = 1; index <= 100; index++) {

      this.containerMasterList.push({ "id": index, "name": "container - " + index });

    }
  }

  getContainerName(containerId) {
    return this.containerMasterList.length > 0 && containerId != null && containerId != "" ?
      this.containerMasterList.filter(x => x.id == containerId)[0].name : ""
  }

  getPreviewProductImage(imageUrl, modalcontent) {
    this.bookingMasterData.pageLoader = true;
    if (imageUrl && imageUrl != null && imageUrl != "") {

      this.productGalleryImages = [];

      this.productGalleryImages.push(
        {
          small: imageUrl,
          medium: imageUrl,
          big: imageUrl,
        });

      this.bookingMasterData.pageLoader = false;
      this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }

  }

  RedirectToReportSummaryLink(reportlink) {
    window.open(reportlink);
  }

  RedirectToSupplier(supplierid) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.SupplierEdit + supplierid;
    window.open(editPage);
  }

  RedirectToEdit(bookingId) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditBooking + bookingId;
    window.open(editPage);
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
    if (window.innerWidth < 450) {

      if (this.isFilterOpen) {
        document.body.classList.add('disable-scroll');
      }
      else {
        document.body.classList.remove('disable-scroll');
      }
    }
  }

  //fetch the country data with virtual scroll
  getCountryData() {

    this.countryRequest.searchText = this.bookingMasterData.countryInput.getValue();
    this.countryRequest.skip = this.bookingMasterData.countryList.length;

    this.bookingMasterData.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.bookingMasterData.countryList = this.bookingMasterData.countryList.concat(customerData);
        }

        this.countryRequest = new CountryDataSourceRequest();
        this.bookingMasterData.countryLoading = false;
      }),
      error => {
        this.bookingMasterData.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.bookingMasterData.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.countryRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.countryLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.countryList = data;
        this.bookingMasterData.countryLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getBrandData() {
    this.brandSearchRequest.searchText = this.bookingMasterData.brandInput.getValue();
    this.brandSearchRequest.skip = this.bookingMasterData.brandList.length;

    this.bookingMasterData.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.bookingMasterData.brandList = this.bookingMasterData.brandList.concat(brandData);
        }
        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.brandSearchRequest.customerId = this.model.customerid;

        this.bookingMasterData.brandLoading = false;
      }),
      error => {
        this.bookingMasterData.brandLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.bookingMasterData.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.brandLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.brandList = data;
        this.bookingMasterData.brandLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getDeptData() {
    this.deptSearchRequest.searchText = this.bookingMasterData.deptInput.getValue();
    this.deptSearchRequest.skip = this.bookingMasterData.deptList.length;

    this.bookingMasterData.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.bookingMasterData.deptList = this.bookingMasterData.deptList.concat(deptData);
        }
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest.customerId = this.model.customerid;

        this.bookingMasterData.deptLoading = false;
      }),
      error => {
        this.bookingMasterData.deptLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {
    if (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) {
      this.deptSearchRequest.idList = this.model.selectedDeptIdList;
    }
    else {
      this.deptSearchRequest.idList = null;
    }

    this.bookingMasterData.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.deptLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.deptList = data;
        this.bookingMasterData.deptLoading = false;
      });
  }

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.buyerSearchRequest.searchText = this.bookingMasterData.buyerInput.getValue();
    this.buyerSearchRequest.skip = this.bookingMasterData.buyerList.length;

    this.bookingMasterData.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.bookingMasterData.buyerList = this.bookingMasterData.buyerList.concat(buyerData);
        }
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.buyerSearchRequest.customerId = this.model.customerid;

        this.bookingMasterData.buyerLoading = false;
      }),
      error => {
        this.bookingMasterData.buyerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.bookingMasterData.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.buyerLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.buyerList = data;
        this.bookingMasterData.buyerLoading = false;
      });
  }

  //fetch the collection data with virtual scroll
  getCollectionData() {
    this.collectionSearchRequest.searchText = this.bookingMasterData.collectionInput.getValue();
    this.collectionSearchRequest.skip = this.bookingMasterData.collectionList.length;

    this.bookingMasterData.collectionLoading = true;
    this.collectionService.getCollectionListByCustomerId(this.collectionSearchRequest).
      subscribe(collectionData => {
        if (collectionData && collectionData.length > 0) {
          this.bookingMasterData.collectionList = this.bookingMasterData.collectionList.concat(collectionData);
        }
        this.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.collectionSearchRequest.customerId = this.model.customerid;

        this.bookingMasterData.collectionLoading = false;
      }),
      error => {
        this.bookingMasterData.collectionLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count collection on load
  getCollectionListBySearch() {
    this.bookingMasterData.collectionInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.collectionLoading = true),
      switchMap(term => term
        ? this.collectionService.getCollectionListByCustomerId(this.collectionSearchRequest, term)
        : this.collectionService.getCollectionListByCustomerId(this.collectionSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.collectionLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.collectionList = data;
        this.bookingMasterData.collectionLoading = false;
      });
  }


  //fetch the collection data with virtual scroll
  getPriceCategoryData() {
    this.priceCategoryRequest.searchText = this.bookingMasterData.priceCategoryInput.getValue();
    this.priceCategoryRequest.skip = this.bookingMasterData.priceCategoryList.length;

    this.bookingMasterData.priceCategoryLoading = true;
    this.customerService.getPriceCategoryListByCustomerId(this.priceCategoryRequest).
      subscribe(priceCategoryData => {
        if (priceCategoryData && priceCategoryData.length > 0) {
          this.bookingMasterData.priceCategoryList = this.bookingMasterData.priceCategoryList.concat(priceCategoryData);
        }
        this.priceCategoryRequest = new CommonCustomerSourceRequest();
        this.priceCategoryRequest.customerId = this.model.customerid;

        this.bookingMasterData.priceCategoryLoading = false;
      }),
      error => {
        this.bookingMasterData.priceCategoryLoading = false;
        this.setError(error);
      };
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

  //fetch the first take (variable) count collection on load
  getPriceCategoryListBySearch() {
    this.bookingMasterData.priceCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.priceCategoryLoading = true),
      switchMap(term => term
        ? this.customerService.getPriceCategoryListByCustomerId(this.priceCategoryRequest, term)
        : this.customerService.getPriceCategoryListByCustomerId(this.priceCategoryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.priceCategoryLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.priceCategoryList = data;
        this.bookingMasterData.priceCategoryLoading = false;
      });
  }

  isShownColumn() {
    this.bookingMasterData.isShowColumn = this.bookingMasterData.isShowColumn == true ?
      false : true;
    if (!this.bookingMasterData.isShowColumn) {
      this.bookingMasterData.isShowColumnImagePath = "assets/images/new-set/table-expand.svg";
      this.bookingMasterData.showColumnTooltip = "Expand";
    }
    else {
      this.bookingMasterData.isShowColumnImagePath = "assets/images/new-set/table-collapsea.svg";
      this.bookingMasterData.showColumnTooltip = "Collapse";
    }
  }

  openProductDetail(content, index, rowItem, containerId, serviceTypeId, bookingId) {
    this.bookingMasterData.pageLoader = true;
    this.bookingMasterData.poList = [];
    this.bookingMasterData.bookingNumber = bookingId;
    this.bookingMasterData.productId = rowItem.productId;

    this.getPoListByBookingAndProduct(true, rowItem, containerId, serviceTypeId, content);
  }

  openOrderTracking(content, iteminfo) {
    this.bookingMasterData.pageLoader = true;
    this.getProductOrContainerListByBooking(iteminfo, content, true);
    this.bookingMasterData.bookingNumber = iteminfo.bookingId + (iteminfo.customerBookingNo && iteminfo.customerBookingNo != '' ? " / " + iteminfo.customerBookingNo : "");

  }

  selectAllProducts(productList, i, status) {
    for (var j = 0; j < productList.length; j++) {
      //      if (productList[i].reportStatus == 'Validated' && productList[i].isParentProduct) {
      if (productList[j].reportStatus == 'Validated') {
        productList[j].selected = status;
      }
      //      }
    }
  }

  checkIfAllProductsSelected(productList, bookingitem, product_Container_Item, event) {
    var selectedcount = 0;
    for (var j = 0; j < productList.length; j++) {
      var item = productList[j];
      if (item.reportStatus == 'Validated') {
        if (product_Container_Item) {
          if (item.reportId == product_Container_Item.reportId
            && item.combineProductId == product_Container_Item.combineProductId)
            if (item.reportStatus == 'Validated') {
              item.selected = event;
            }
        }
        if (item.selected == true) {
          selectedcount++;
        }
      }
    }
    bookingitem.isAllProductSelected = selectedcount == productList.length ? true : false;
  }

  reportBtnShow(productList): boolean {
    var isOk: boolean = false;
    isOk = productList && productList.length > 0 && ((productList.filter(x => x.containerId != null && x.selected == true).length == 1)
      || (productList.filter(x => x.containerId == null && x.isParentProduct && x.selected == true).length == 1)) ? true : false;

    return isOk;
  }

  downloadBtnShow(productList): boolean {
    var isOk: boolean = false;
    isOk = productList && productList.length > 0 ? productList.filter(x => x.selected == true).length > 0 : false;
    return isOk;
  }

  headerCheckboxShow(productList): boolean {
    var isOk: boolean = false;
    isOk = productList && productList.length > 0 ? productList.filter(x => x.reportStatus == 'Validated').length > 0 : false;
    return isOk;
  }
  filterTextShown() {
    var isFilterDataSelected = false;

    if ((this.model.fromdate && this.model.fromdate != '') ||
      (this.model.todate && this.model.todate != '') || this.model.customerid > 0 || this.model.supplierid > 0
      || (this.model.factoryidlst && this.model.factoryidlst.length > 0) ||
      (this.model.selectedCountryIdList && this.model.selectedCountryIdList.length > 0) ||
      (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) ||
      (this.model.selectedBrandIdList && this.model.selectedBrandIdList.length > 0) ||
      (this.model.officeidlst && this.model.officeidlst.length > 0) ||
      (this.model.serviceTypelst && this.model.serviceTypelst.length > 0) ||
      (this.model.selectedBuyerIdList && this.model.selectedBuyerIdList.length > 0) ||
      (this.model.statusidlst && this.model.statusidlst.length > 0) ||
      (this.model.userIdList && this.model.userIdList.length > 0) ||
      (this.model.selectedCollectionIdList && this.model.selectedCollectionIdList.length > 0) ||
      (this.model.selectedPriceCategoryIdList && this.model.selectedPriceCategoryIdList.length > 0) ||
      (this.model.quotationsStatusIdlst && this.model.quotationsStatusIdlst.length > 0) ||
      (this.model.barcode && this.model.barcode != '') ||
      (this.model.advancedsearchtypetext && this.model.advancedsearchtypetext != '') ||
      this.model.isPicking || this.model.isEcoPack) {

      //desktop version
      if (!this.isMobile) {
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;
        //date add
        count = MobileViewFilterCount + count;

        if (this.model.customerid > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.supplierid > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.factoryidlst && this.model.factoryidlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedCountryIdList && this.model.selectedCountryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedBrandIdList && this.model.selectedBrandIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.officeidlst && this.model.officeidlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.serviceTypelst && this.model.serviceTypelst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedBuyerIdList && this.model.selectedBuyerIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.statusidlst && this.model.statusidlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.userIdList && this.model.userIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedCollectionIdList && this.model.selectedCollectionIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedPriceCategoryIdList && this.model.selectedPriceCategoryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.quotationsStatusIdlst && this.model.quotationsStatusIdlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.barcode && this.model.barcode != '') {
          count = MobileViewFilterCount + count;
        }
        if (this.model.advancedsearchtypetext && this.model.advancedsearchtypetext != '') {
          count = MobileViewFilterCount + count;
        }
        if (this.model.isPicking) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.isEcoPack) {
          count = MobileViewFilterCount + count;
        }

        this.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.filterCount = 0;
        this.bookingMasterData.countryNameList = [];
        this.bookingMasterData.supplierName = "";
        this.bookingMasterData.customerName = "";
        this.bookingMasterData.factoryNameList = [];
        this.bookingMasterData.officeNameList = [];
        this.bookingMasterData.serviceTypeNameList = [];
        this.bookingMasterData.brandNameList = [];
        this.bookingMasterData.buyerNameList = [];
        this.bookingMasterData.deptNameList = [];
        this.bookingMasterData.statusList = [];
      }
    }

    return isFilterDataSelected;
  }

  officeChange(item) {
    if (item) {

      if (this.model.officeidlst && this.model.officeidlst.length > 0) {

        var officeLength = this.model.officeidlst.length;

        var officeDetails = [];
        var maxLength = officeLength > 1 ? 1 : officeLength;
        for (var i = 0; i < maxLength; i++) {

          officeDetails.push(this.bookingMasterData.officeList.find(x => x.id == this.model.officeidlst[i]).name);
        }

        if (officeLength > 1) {
          officeDetails.push(" " + (officeLength - 1) + "+");
        }
        this.bookingMasterData.officeNameList = officeDetails;
      }
      else {
        this.bookingMasterData.officeNameList = [];
      }
    }
  }
  statusChange(item) {
    if (item) {

      if (this.model.statusidlst && this.model.statusidlst.length > 0) {

        var statusLength = this.model.statusidlst.length;

        var statusDetails = [];
        var maxLength = statusLength > 1 ? 1 : statusLength;

        for (var i = 0; i < maxLength; i++) {
          statusDetails.push(this.bookingMasterData.bookingStatusList.find(x => x.id == this.model.statusidlst[i]).statusName);
        }

        if (statusLength > 1) {
          statusDetails.push(" " + (statusLength - 1) + "+");
        }
        this.bookingMasterData.statusNameList = statusDetails;
      }
      else {
        this.bookingMasterData.statusNameList = [];
      }
    }
  }

  isfilterData() {
    if ((this.model.fromdate && this.model.fromdate != '') ||
      (this.model.todate && this.model.todate != '') || this.model.customerid > 0 || this.model.supplierid > 0
      || (this.model.factoryidlst && this.model.factoryidlst.length > 0) ||
      (this.model.selectedCountryIdList && this.model.selectedCountryIdList.length > 0) ||
      (this.model.selectedProvinceIdList && this.model.selectedProvinceIdList.length > 0) ||
      (this.model.selectedCityIdList && this.model.selectedCityIdList.length > 0) ||
      (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) ||
      (this.model.selectedBrandIdList && this.model.selectedBrandIdList.length > 0) ||
      (this.model.officeidlst && this.model.officeidlst.length > 0) ||
      (this.model.serviceTypelst && this.model.serviceTypelst.length > 0) ||
      (this.model.selectedBuyerIdList && this.model.selectedBuyerIdList.length > 0) ||
      (this.model.statusidlst && this.model.statusidlst.length > 0) ||
      (this.model.userIdList && this.model.userIdList.length > 0) ||
      (this.model.selectedCollectionIdList && this.model.selectedCollectionIdList.length > 0) ||
      (this.model.selectedPriceCategoryIdList && this.model.selectedPriceCategoryIdList.length > 0) ||
      (this.model.quotationsStatusIdlst && this.model.quotationsStatusIdlst.length > 0) ||
      (this.model.barcode && this.model.barcode != '') ||
      (this.model.advancedsearchtypetext && this.model.advancedsearchtypetext != '') ||
      this.model.isPicking || this.model.isEcoPack || this.model.isEAQF) {
      this.filterDataShown = true;
    }
    else {
      this.filterDataShown = false;
    }
  }

  changeCountryData() {
    this.isfilterData();
    this.getProvinceByCountryList();
  }

  changeProvinceData() {
    this.isfilterData();
    this.getCityByProvinceList();
  }

  changeCityData() {
    this.isfilterData();
    this.getProvinceByCountryList();
  }

  //get the province list
  getProvinceByCountryList() {
    this.bookingMasterData.provinceLoading = true;
    var request = new ProvinceListSearchRequest();
    if (this.model.selectedCountryIdList)
      request.countryIds = this.model.selectedCountryIdList;

    this.locationService.getProvinceByCountryIds(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response)
          this.processProvinceListResponse(response);
        this.bookingMasterData.provinceLoading = false;
      }),
      error => {
        this.bookingMasterData.provinceLoading = false;
        this.setError(error);
      };
  }

  //process the province list
  processProvinceListResponse(response) {
    if (response.result == ProvinceResult.Success) {
      this.bookingMasterData.provinceList = response.dataSourceList;
    }
    else if (response.result == ProvinceResult.DataNotFound) {
      this.showWarning('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.MSG_PROVINCE_NOT_FOUND');
    }
  }

  //get the city list
  getCityByProvinceList() {
    this.bookingMasterData.cityLoading = true;

    var request = new CityListSearchRequest();
    if (this.model.selectedProvinceIdList)
      request.provinceIds = this.model.selectedProvinceIdList;

    this.locationService.getCityByProvinceIds(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        this.processCityListResponse(response);
        this.bookingMasterData.cityLoading = false;
      }),
      error => {
        this.bookingMasterData.cityLoading = false;
        this.setError(error);
      };
  }

  //process the city list response
  processCityListResponse(response) {
    if (response.result == ResponseResult.Success) {
      this.bookingMasterData.cityList = response.dataSourceList;
    }
    else if (response.result == ProvinceResult.DataNotFound) {
      this.showWarning('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.MSG_CITY_NOT_FOUND');
    }
  }

  changeSupplierType(item) {
    this.bookingMasterData.supLoading = true;
    this.bookingMasterData.supplierList = [];
    this.model.supplierid = null;
    this.bookingMasterData.factoryList = [];
    this.model.factoryidlst = []
    if (item.id == SearchType.SupplierCode) {
      this.bookingMasterData.supsearchRequest.supSearchTypeId = SearchType.SupplierCode;
    }
    else if (item.id == SearchType.SupplierName) {
      this.bookingMasterData.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    }
    this.getSupplierData();
  }
}

import { CustomerCommonDataSourceRequest } from './../../../../_Models/common/common.model';
import { Component, OnInit, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import * as c3 from 'c3';
const mapboxgl = require('mapbox-gl');
import {
  CustomerDashboard, CustomerInfo, CustomerCsContact, CustomerFilterPopupData, CustomerTaskEnum, CustomerDashBoardLoader,
  CustomerDashBoardDataFound, InspectionManDayOverview, ManDayResult, CustomerDashBoardGraphData, CustomerBusinessOVDashboard,
  CustomerAPIRADashboard, ProductCategoryDashboard, CustomerResultDashboard, InspectionRejectDashboard,
  SupplierPerformanceDashboard, QuotationTaskData, MapGeoLocationResult
} from '../../../../_Models/dashboard/customerdashboard.model';
import { CustomerDashboardFilterModel, CustomerDashboardFilterMaster, InspectionListRequest, DashboardMapFilterRequest, DashboardType } from '../../../../_Models/dashboard/customerdashboardfilterrequest.model';
import { DashBoardService } from '../../../../_Services/dashboard/dashboard.service'
import { SupplierService } from '../../../../_Services/supplier/supplier.service'
import { AuthenticationService } from "../../../../_Services/user/authentication.service";
import { Router } from '@angular/router';
import { NgbCalendar, NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { QuotationCustomerTaskType, QuotationPageType } from '../../../../_Models/quotation/quotationsummary.model'
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { MandayYearChart, MandayYear, MandayDashboardResult, MandayDashboardRequest } from 'src/app/_Models/statistics/manday-dashboard.model';
import { first, debounceTime, tap, distinctUntilChanged, catchError, switchMap, takeUntil } from 'rxjs/operators';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { APIService, bookingStatusList, CusDashboardProductNameTrim, CusDashboardProductLength, FBResultNotApplicableName, FBResultNAName, MobileViewFiltersCount, MobileViewFilterCount, CountrySelectedFilterTextCount, SupplierType, SupplierNameTrim, ListSize, dashboardSupplierNameCodeList, SearchType, BookingStatus, RoleEnum, FbReportResultType, CustomerDecisionDdl, DataLengthTrim } from 'src/app/components/common/static-data-common';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ProductDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { Validator } from 'src/app/components/common'
import { of, concat, Subject, Observable } from 'rxjs';
import { supplierToRemove } from 'src/app/_Models/supplier/suppliersummary.model';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { QuantitativeDashBoardService } from 'src/app/_Services/statistics/quantitativedashboard.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserModel } from 'src/app/_Models/user/user.model';
// export = mapboxgl;

am4core.useTheme(am4themes_animated);

@Component({
  selector: 'app-cusdashboard',
  templateUrl: './cusdashboard.component.html',
  styleUrls: ['./cusdashboard.component.scss']
})
export class CusdashboardComponent implements OnInit {

  componentDestroyed$: Subject<boolean> = new Subject();
  mapObj: any;
  isMobile: boolean;
  mapHeight: number;
  isLoading: boolean
  filterOpen: boolean;
  searchQuery: string;
  searchActive: boolean;
  toDate: NgbDate | null;
  fromDate: NgbDate | null;
  hasSearchResult: boolean;
  showFactoryResult: boolean;
  selectedSearchResult: object = { 'factoryName': '', 'factoryID': null };
  searchResultData: Array<object>;
  hoveredDate: NgbDate | null = null;
  private chart: am4charts.XYChart;
  mandayYearChart: Array<MandayYearChart>;
  monthYearXAxis: Array<MandayYear>;
  toggleFormSection: boolean;
  isFullscreen: boolean;
  @ViewChild('leftColumn') leftColumn: ElementRef;
  @ViewChild('sideOverlay') sideOverlay: ElementRef;
  @ViewChild('mapCard') mapCard: ElementRef;

  @ViewChild('datepicker') datepicker;

  @ViewChild('searchTrigger') searchTrigger: ElementRef;
  @ViewChild('searchContainer') searchContainer: ElementRef;
  @ViewChild('buinessOverviewCard') buinessOverviewCard: ElementRef;
  @ViewChild('mobileSearchContainer') mobileSearchContainer: ElementRef;
  @ViewChild('businessOverviewTrigger') businessOverviewTrigger: ElementRef;

  customerFilterModel: CustomerDashboardFilterModel;
  customerDashBoardGraphData: CustomerDashBoardGraphData;
  customerFilterPopupData: CustomerFilterPopupData;
  customerDashboard: CustomerDashboard;
  customerDashBoardLoader: CustomerDashBoardLoader;
  customerDashBoardDataFound: CustomerDashBoardDataFound;
  customerId: number;
  currentYear: number;
  lastYear: number;
  dashBoardFilterMaster: CustomerDashboardFilterMaster;
  requestCustomerModel: CustomerCommonDataSourceRequest;
  currentUser: UserModel;
  filterState: boolean;

  public bookingLeadTime: number;
  public etdLeadTime: number;
  public bookingRevisons: number;
  public customerInfo: CustomerInfo;
  public manDayType: number;
  public _manDayEnum = ManDayResult;
  public _customerTaskEnum = CustomerTaskEnum;
  public _quotationCustomerTaskType = QuotationCustomerTaskType;
  public _quotationPageType = QuotationPageType;
  public dataLengthTrim = DataLengthTrim;
  totalReports: number;
  requestModel: CommonDataSourceRequest;
  countryRequest: CountryDataSourceRequest;
  brandSearchRequest: CommonCustomerSourceRequest;
  deptSearchRequest: CommonCustomerSourceRequest;
  collectionSearchRequest: CommonCustomerSourceRequest;
  buyerSearchRequest: CommonCustomerSourceRequest;
  mandayByYearModel: MandayDashboardRequest;
  factoryList = [];

  public error;
  public factoryInspectionDetails;
  public customerLoading: boolean;
  public searchLoading: boolean;
  public showFactoryDataLoader: boolean;
  public centreLatitude = 30.77;
  public centreLongitude = 11.25;
  public markers = [];
  public mapLoading: boolean;
  public isQuotationConfirmationRole: boolean;
  dashboardSupplierNameCodeList: any = dashboardSupplierNameCodeList;
  inspectionRequest: InspectionListRequest;
  isEditInspectionCustomerDecisionRole: boolean;
  customerDecisionCount: any;

  constructor(private cdr: ChangeDetectorRef, private authService: AuthenticationService, public validator: Validator, public calendar: NgbCalendar, public cusService: CustomerService,
    private service: DashBoardService, private router: Router, private translate: TranslateService, private toastr: ToastrService,
    public formatter: NgbDateParserFormatter, private quantitativeService: QuantitativeDashBoardService, public supService: SupplierService, public locationService: LocationService,
    public brandService: CustomerBrandService,
    public deptService: CustomerDepartmentService,
    public collectionService: CustomerCollectionService,
    public buyerService: CustomerbuyerService,
    public customerProductService: CustomerProduct,
    public utility: UtilityService) {
    this.isLoading = false;
    this.filterState = false;
    this.isQuotationConfirmationRole = false;
    this.customerDashBoardGraphData = new CustomerDashBoardGraphData();
    this.customerDashBoardGraphData.isCollapsed = false;
    this.customerDashBoardGraphData.togglePhoneContainer = false;
    this.validator.setJSON("customer/customer-dashboardsummary.valid.json");
    this.validator.setModelAsync(() => this.customerFilterModel);
    this.validator.isSubmitted = false;

    this.getIsMobile();
    this.mapHeight = 380;
    this.filterOpen = false;
    this.searchActive = false;
    this.showFactoryResult = false;
    am4core.addLicense("CH238479116");
    this.toggleFormSection = false;

    this.currentUser = this.authService.getCurrentUser();
  }


  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  //Initialize the customer dashboard objects
  initializeDashBoardObjects() {
    this.customerDashBoardGraphData = new CustomerDashBoardGraphData();
    this.customerFilterModel = new CustomerDashboardFilterModel();
    this.customerInfo = new CustomerInfo();
    this.customerDashboard = new CustomerDashboard();
    this.customerDashboard.businessOVDashboard = new CustomerBusinessOVDashboard();
    this.customerDashboard.apiraDashboard = new Array<CustomerAPIRADashboard>();
    this.customerDashboard.productCategoryDashboard = new Array<ProductCategoryDashboard>();
    this.customerDashboard.customerResultDashboard = new Array<CustomerResultDashboard>();
    this.customerDashboard.inspectionRejectDashboard = new Array<InspectionRejectDashboard>();
    this.customerDashboard.supplierPerformanceDashboard = new SupplierPerformanceDashboard();
    this.customerDashboard.quotationTaskData = new QuotationTaskData();
    this.customerDashboard.inspectionManDayOverview = new InspectionManDayOverview();
    this.customerDashboard.customerCsContact = new CustomerCsContact();
    this.customerDashBoardLoader = new CustomerDashBoardLoader();
    this.customerDashBoardDataFound = new CustomerDashBoardDataFound();
    this.requestModel = new CommonDataSourceRequest();
    this.countryRequest = new CountryDataSourceRequest();
    this.brandSearchRequest = new CommonCustomerSourceRequest();
    this.deptSearchRequest = new CommonCustomerSourceRequest();
    this.collectionSearchRequest = new CommonCustomerSourceRequest();
    this.buyerSearchRequest = new CommonCustomerSourceRequest();
    this.dashBoardFilterMaster = new CustomerDashboardFilterMaster();
    this.mandayByYearModel = new MandayDashboardRequest();
    this.dashBoardFilterMaster.filterCount = 0;
    //initialize loader methods
    this.customerDashBoardLoader.apiresultLoading = true;
    this.customerDashBoardLoader.customerResultLoading = true;
    this.customerDashBoardLoader.productOverviewLoading = true;
    this.customerDashBoardLoader.supplierPerformanceLoading = true;
    this.customerDashBoardLoader.inspectionRejectLoading = true;
    this.customerDashBoardLoader.manDaysLoading = true;
    this.dashBoardFilterMaster.searchtypeid = SearchType.SupplierName;

    this.setDataFoundFlags();

    this.mapLoading = true;
    this.customerLoading = false;
    this.searchLoading = false;
    this.showFactoryDataLoader = false;
    this.manDayType = this._manDayEnum.Monthly;
  }


  setDataFoundFlags() {
    this.customerDashBoardDataFound.taskDataFound = true;
    this.customerDashBoardDataFound.manDaysFound = true;
    this.customerDashBoardDataFound.inspectedBookingFound = true;
    this.customerDashBoardDataFound.businessOverviewFound = true;
    // this.customerDashBoardDataFound.apiresultFound = true;
    this.customerDashBoardDataFound.customerResultFound = true;
    this.customerDashBoardDataFound.inspectionRejectFound = true;
    this.customerDashBoardDataFound.productOverviewFound = true;
    this.customerDashBoardDataFound.supplierPerformanceFound = true;
  }

  //calling ng initialize method
  ngOnInit() {
    this.init();
  }

  //Initialization Function
  init() {
    this.mandayYearChart = new Array<MandayYearChart>();
    this.monthYearXAxis = new Array<MandayYear>();

    this.initializeDashBoardObjects();
    this.assignDate();
    this.assignCustomerInfo();
    this.getCustomerList();
    this.getCustomerDashBoard();
    this.getCustomerCsContact(this.customerInfo.id);
    this.getSupListBySearch();
    this.getCountryListBySearch();
    this.getProductCategoryData();
    this.getProductListBySearch();
  }

  //Set Current Year and Last Year Data Values
  assignDate() {
    var date = new Date();
    this.currentYear = date.getFullYear();
    this.lastYear = date.getFullYear() - 1;
  }

  //Make CustomerInfo which will be used to send to child page
  assignCustomerInfo() {
    let user = this.authService.getCurrentUser();
    if (user) {
      this.customerInfo.id = user.customerid;
      this.customerInfo.name = user.fullName;
      let quotationConfirmation = user.roles.find(x => x.id == RoleEnum.QuotationConfirmation);
      if (quotationConfirmation)
        this.isQuotationConfirmationRole = true;
      let editInspectionCustomerDecision = user.roles.find(x => x.id == RoleEnum.EditInspectionCustomerDecision);
      if (editInspectionCustomerDecision)
        this.isEditInspectionCustomerDecisionRole = true;
    }
  }

  //set default filter data
  setDefaultFilter() {
    this.customerFilterModel.customerId = this.customerInfo.id;
    //this.customerFilterModel.supplierId = null;
    this.customerFilterModel.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.customerFilterModel.serviceDateTo = this.calendar.getToday();
  }

  //Get all the dashboard data with all the bookingids
  getBaseDashBoardData() {
    this.service.getBookingDetails(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {

          if (response != null) {
            let inspectionIds = response.map(x => x.inspectionId);
            this.getAPIRADashBoard();
            this.getCustomerResultDashBoard();
            this.getInspectionRejectDashBoard();
            this.getProductCatgoryDashBoard();

            this.setmapcountry();

          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');

        });
  }

  //Get the business overview data
  getBusinessOverviewData() {
    this.customerDashBoardLoader.businessOverviewLoading = true;
    this.service.getCustomerBusinessOverview(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response != null) {
            this.customerDashboard.businessOVDashboard = response;
            this.customerDashBoardLoader.businessOverviewLoading = false;
            setTimeout(() => {
              //this.customerDashBoardGraphData.mapCardHeight = this.mapCard.nativeElement.offsetHeight;
            }, 10);
          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.customerDashBoardLoader.businessOverviewLoading = false;
        });
  }

  //Get the Quotation Tasks
  getQuotationTasks() {
    this.customerDashBoardLoader.taskLoading = true;
    this.service.getPendingQuotations(this.customerFilterModel.customerId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response != null) {

            this.customerDashboard.quotationTaskData = response;
            this.customerDashBoardLoader.taskLoading = false;
            setTimeout(() => {
              //this.customerDashBoardGraphData.overlayHeight = this.leftColumn.nativeElement.offsetHeight;
            }, 10);

          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.customerDashBoardLoader.taskLoading = false;
        });
  }

  //Get the inspection and Manday overview data
  getInspectedBookings(request) {
    this.customerDashBoardLoader.inspectedBookingLoading = true;
    this.service.getInspectedBookings(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response != null) {
            this.customerDashboard.inspectionManDayOverview = response;

            this.customerDashboard.inspectionManDayOverview.averagePercentagePositive = Math.abs(this.customerDashboard.inspectionManDayOverview.averagePercentage);
            this.customerDashboard.inspectionManDayOverview.totalManDaysPercentagePositive = Math.abs(this.customerDashboard.inspectionManDayOverview.totalManDaysPercentage);

            this.customerDashBoardLoader.inspectedBookingLoading = false;
            setTimeout(() => {
              //this.customerDashBoardGraphData.overlayHeight = this.leftColumn.nativeElement.offsetHeight;
            }, 10);
          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.customerDashBoardLoader.inspectedBookingLoading = false;
        });
  }

  //Get the customer dashboard call all the dashboards
  getCustomerDashBoard() {
    if (!this.customerFilterModel.customerId) {
      this.setDefaultFilter();
    }
    this.setDataFoundFlags();
    this.getBaseDashBoardData();
    this.getBusinessOverviewData();
    this.getMandaysByYear();
    this.getQuotationTasks();
    this.getCustomerDecisionCount();
    this.getSupplierPerformanceDashBoard(this.customerFilterModel);
    this.getInspectedBookings(this.customerFilterModel);
  }

  //Get the API Result Analysis Data
  getAPIRADashBoard() {
    this.customerDashBoardLoader.apiresultLoading = true;

    this.service.getAPIResult(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.length > 0) {

            let length = response.length;// < CusDashboardProductLength ? response.data.length : CusDashboardProductLength;
            for (var i = 0; i < length; i++) {
              var _sublength = CusDashboardProductLength < i ? 20 : 9;
              response[i].name =

                response[i].statusName &&
                  response[i].statusName.length > 11 ?
                  response[i].statusName.substring(0, _sublength) + ".." :
                  response[i].statusName;
            }

            this.customerDashboard.apiraDashboard = response;
            if (this.customerDashboard.apiraDashboard) {
              this.totalReports = this.customerDashboard.apiraDashboard.reduce((accum, item) => accum + item.totalCount, 0)
              var data = this.customerDashboard.apiraDashboard.find(x => x.statusName.toLowerCase() == FBResultNotApplicableName.toLowerCase());
              if (data) {
                //status name update not applicable to NA
                data.statusName = FBResultNAName;
              }
            }
            setTimeout(() => {
              this.renderPieChart('chartdiv', this.customerDashboard.apiraDashboard);
            }, 10);

          }
          else {
            this.customerDashboard.apiraDashboard = new Array<CustomerAPIRADashboard>();
            this.totalReports = 0;
          }
          this.customerDashBoardLoader.apiresultLoading = false;
        },
        error => {
          this.customerDashBoardLoader.apiresultLoading = false;
          this.customerDashBoardDataFound.apiresultFound = true;
        });
  }

  //Get the customer contact information
  getCustomerCsContact(customerId) {
    this.service.getCustomerCsContact(customerId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {

            this.customerDashboard.customerCsContact = response;
          }
        });
  }

  //Get the customer result dashboards
  getCustomerResultDashBoard() {
    this.customerDashBoardLoader.customerResultLoading = true;

    this.service.getCustomerResult(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.length > 0) {

            let length = response.length;// < CusDashboardProductLength ? response.data.length : CusDashboardProductLength;
            for (var i = 0; i < length; i++) {
              var _sublength = CusDashboardProductLength < i ? 20 : 9;
              response[i].name =

                response[i].statusName &&
                  response[i].statusName.length > 11 ?
                  response[i].statusName.substring(0, _sublength) + ".." :
                  response[i].statusName;
            }
            this.customerDashboard.customerResultDashboard = response;
            this.customerDashBoardLoader.customerResultLoading = false;
            setTimeout(() => {
              this.renderPieChart('chartdiv1', this.customerDashboard.customerResultDashboard);
            }, 10);
          }
          else {
            this.customerDashboard.customerResultDashboard = new Array<CustomerResultDashboard>();
          }
          this.customerDashBoardLoader.customerResultLoading = false;
          this.customerDashBoardDataFound.customerResultFound = false;
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.customerDashBoardLoader.customerResultLoading = false;
          this.customerDashBoardDataFound.customerResultFound = true;
        });
  }

  //Get the inspection reject dashboards
  getInspectionRejectDashBoard() {

    this.service.getInspectionRejectResult(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.length > 0) {
            this.customerDashboard.inspectionRejectDashboard = response;
            setTimeout(() => {
              this.halfDonutInspectionRejectChart();
            }, 10);
            this.customerDashBoardDataFound.inspectionRejectFound = true;
          }
          else {
            this.customerDashboard.inspectionRejectDashboard = new Array<InspectionRejectDashboard>();
            this.customerDashBoardDataFound.inspectionRejectFound = false;
          }
          this.customerDashBoardLoader.inspectionRejectLoading = false;
        },
        error => {
          this.customerDashBoardLoader.inspectionRejectLoading = false;
          this.customerDashBoardLoader.inspectionRejectError = true;
        });
  }

  //Get the product category dashboard
  getProductCatgoryDashBoard() {

    this.service.getProductCategoryResult(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.length > 0) {

            let length = response.length < CusDashboardProductLength ? response.length : CusDashboardProductLength;
            for (var i = 0; i < length; i++) {
              response[i].productName =

                response[i].statusName &&
                  response[i].statusName.length > CusDashboardProductNameTrim ?
                  response[i].statusName.substring(0, CusDashboardProductNameTrim) + ".." :
                  response[i].statusName;
            }
            this.customerDashboard.productCategoryDashboard = response;

            this.productOverviewChartFrame();
          }
          else if (!response || response.length == 0) {
            this.customerDashBoardDataFound.productOverviewFound = false;
          }
          this.customerDashBoardLoader.productOverviewLoading = false;
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.customerDashBoardLoader.productOverviewLoading = false;
        });
  }

  //Get the supplier performance dashboard
  getSupplierPerformanceDashBoard(request) {
    this.service.getSupplierPerformance(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            this.customerDashboard.supplierPerformanceDashboard = response;
          }
          else if (!response) {
            this.customerDashBoardDataFound.supplierPerformanceFound = false;
          }
          this.customerDashBoardLoader.supplierPerformanceLoading = false;

        },
        error => {
          this.customerDashBoardLoader.supplierPerformanceLoading = false;
          this.customerDashBoardLoader.supplierPerformanceError = true;
        });
  }
  setmapcountry() {
    if (this.customerFilterModel) {
      const request: DashboardMapFilterRequest = {
        customerId: this.customerFilterModel.customerId,
        factoryIds: this.customerFilterModel.factoryId ? [this.customerFilterModel.factoryId] : [],
        productCategoryIds: this.customerFilterModel.prodCategoryList,
        productIds: this.customerFilterModel.productIdList,
        brandIds: this.customerFilterModel.selectedBrandIdList,
        buyerIds: this.customerFilterModel.selectedBrandIdList,
        collectionIds: this.customerFilterModel.selectedCollectionIdList,
        countryIds: this.customerFilterModel.selectedCountryIdList,
        serviceDateFrom: this.customerFilterModel.serviceDateFrom,
        serviceDateTo: this.customerFilterModel.serviceDateTo,
        departmentIds: this.customerFilterModel.selectedDeptIdList,
        statusIds: this.customerFilterModel.statusIdList,
        supplierId: this.customerFilterModel.supplierId,
        officeIds: [],
        supplierIds: [],
        dashboardType: DashboardType.CustomerDashboard
      }

      this.service.getInspCountryGeoCode(request)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            this.mapLoading = false;
            if (response.countryGeoCodeResult == MapGeoLocationResult.Failure) {
              this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_GEO_COUNTRY_FAILED');
            }
            if (response.provinceGeoCodeResult == MapGeoLocationResult.Failure) {
              this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_GEO_PROVINCE_FAILED');
            }
            setTimeout(() => {
              this.renderMap(response);
            }, 500);
          },
          error => {
            this.mapLoading = false;
            setTimeout(() => {

              this.renderMap();
            }, 100);
          });
    }
    else {
      this.mapLoading = false;
      setTimeout(() => {

        this.renderMap();
      }, 100);
    }
  }

  // function to toggle filter component
  toggleFilterState() {
    this.filterState = !this.filterState;
  }

  // function to toggle phone number container
  togglePhoneDetailContainer() {
    this.customerDashBoardGraphData.togglePhoneContainer = !this.customerDashBoardGraphData.togglePhoneContainer;
  }

  // toggle business card on the map
  toggleMapCard() {
    this.customerDashBoardGraphData.isCollapsed = !this.customerDashBoardGraphData.isCollapsed;
  }

  //navigate to quotation summary from tasks
  navigateQuotationSummary(taskType) {
    var path = "quotations/quotation-summary/";
    var entity: string = this.utility.getEntityName();
    if (taskType == this._customerTaskEnum.PendingQuotationTask) {
      if (this.customerDashboard.quotationTaskData.pendingQuotations > 0) {

        this.router.navigate([`/${entity}/${path}/${this._quotationPageType.CustomerTaskDashboard}/${this._quotationCustomerTaskType.PendingQuotations}`,
        ]);
      }
    }
    else if (taskType == this._customerTaskEnum.CompletedQuotationTask) {
      if (this.customerDashboard.quotationTaskData.completedQuotations > 0) {
        this.router.navigate([`/${entity}/${path}/${this._quotationPageType.CustomerTaskDashboard}/${this._quotationCustomerTaskType.CompletedQuotations}`,
        ]);
      }
    }



  }

  //receive filter model from the child and get the dashboard data
  receiveFilterModel($event) {
    this.customerFilterModel = $event;
    this.getCustomerDashBoard();
    this.filterState = !this.filterState;
  }
  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
  }

  public showError(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }

  /**
* Function to render map
*/
  renderMap(geoCodes?) {
    (mapboxgl as typeof mapboxgl).accessToken = 'pk.eyJ1IjoiYW1hbnNvbmkyMTEiLCJhIjoiY2sxdGpldXMyMDA0bDNubjgxZzV1cXhxYiJ9.lg92cAq6oc1paYlkwd0i8Q';
    var map = new mapboxgl.Map({
      container: 'mapView',
      style: 'mapbox://styles/amansoni211/ck1tjsmti0a4n1co8k4bfd77n',
      zoom: 1.30,
      center: [this.centreLongitude, this.centreLatitude],
      scrollZoom: !this.isMobile ? true : false,
      minZoom: 2
    });

    this.mapHeight = 380;
    var jsonGeo = new Array();
    if (geoCodes) {
      var i = 1;

      if (geoCodes.countryGeoCode && geoCodes.countryGeoCode.length > 0) {

        geoCodes.countryGeoCode.forEach(function (marker, index) {

          jsonGeo.push(
            {
              type: 'Feature',
              id: i,
              geometry: {
                type: 'Point',
                coordinates: [marker.longitude, marker.latitude]
              },
              properties: {
                id: i++,
                title: marker.factoryCountryName,
                count: marker.totalCount,
                type: "country",
                description: marker.factoryCountryName
              }
            })
        });
      }
      if (geoCodes.provinceGeoCode && geoCodes.provinceGeoCode.length > 0) {

        geoCodes.provinceGeoCode.forEach(function (marker, index) {

          jsonGeo.push(
            {
              type: 'Feature',
              id: i,
              geometry: {
                type: 'Point',
                coordinates: [marker.longitude, marker.latitude]
              },
              properties: {
                id: i++,
                title: marker.factoryProvinceName,
                count: marker.totalCount,
                type: "province",
                description: marker.factoryProvinceName
              }
            })
        });
      }

      if (geoCodes.factoryGeoCode && geoCodes.factoryGeoCode.length > 0) {

        geoCodes.factoryGeoCode.forEach(function (marker, index) {

          jsonGeo.push(
            {
              type: 'Feature',
              id: i,
              geometry: {
                type: 'Point',
                coordinates: [marker.longitude, marker.latitude]
              },
              properties: {
                id: i++,
                title: marker.factoryName,
                count: marker.totalCount,
                type: "factory",
                description: marker.factoryName
              }
            })
        });
      }

      var geoJson = {
        'type': 'FeatureCollection',
        'features': jsonGeo
      }

      //get the single array from array of arrays
      geoJson.features = [].concat(...geoJson.features);

      map.on('load', function () {
        map.addSource('points', {
          'type': 'geojson',
          'data': geoJson
        });

        // Add a symbol layer
        map.addLayer({
          'id': 'points',
          'type': 'circle',
          'source': 'points',
          'filter': ['any', ['all', ['<=', ['zoom'], 2], ['==', ['get', 'type'], 'country']], ['all', ['>', ['zoom'], 2], ['<=', ['zoom'], 3], ['==', ['get', 'type'], 'province']], ['all', ['>', ['zoom'], 3], ['==', ['get', 'type'], 'factory']]],
          'paint': {
            // 'circle-color': '#c9001f',
            'circle-color': ['case',
              ['boolean', ['feature-state', 'hover'], false],
              '#b70b0d',
              '#c9001f'
            ],
            'circle-radius': [
              'step',
              ['zoom'],
              25,
              4,
              20
            ],
            'circle-stroke-width': 6,
            'circle-stroke-color': '#ffffff'
          }
        });
        map.addLayer({
          id: 'cluster-count',
          type: 'symbol',
          source: 'points',
          filter: ['any', ['all', ['<=', ['zoom'], 2], ['==', ['get', 'type'], 'country']], ['all', ['>', ['zoom'], 2], ['<=', ['zoom'], 3], ['==', ['get', 'type'], 'province']], ['all', ['>', ['zoom'], 3], ['==', ['get', 'type'], 'factory']]],
          layout: {
            'text-field': '{count}',
            'text-font': ['Roboto Bold'],
            'text-size': 14,
          },
          paint: {
            "text-color": "#ffffff",
          }
        });

        // Create a popup, but don't add it to the map yet.
        var popup = new mapboxgl.Popup({
          closeButton: false,
          closeOnClick: false
        });

        map.on('mouseenter', 'points', function (e) {
          // Change the cursor style as a UI indicator.
          map.getCanvas().style.cursor = 'pointer';

          var coordinates = e.features[0].geometry.coordinates.slice();
          var description = e.features[0].properties.description;

          // Ensure that if the map is zoomed out such that multiple
          // copies of the feature are visible, the popup appears
          // over the copy being pointed to.
          while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
          }

          // Populate the popup and set its coordinates
          // based on the feature found.
          popup.setLngLat(coordinates)
            .setHTML(description)
            .addTo(map);

          // adding hover effect
          this.hoveredStateId = e.features[0].properties.id;
          map.setFeatureState({ source: 'points', id: this.hoveredStateId }, { hover: true });
        });

        map.on('mouseleave', 'points', function () {
          map.getCanvas().style.cursor = '';
          popup.remove();

          // removing hover effect
          map.setFeatureState({ source: 'points', id: this.hoveredStateId }, { hover: false });
        });

      });

      // Add zoom and rotation controls to the map
      // if (window.innerWidth > 1280) {
      //   map.addControl(new mapboxgl.NavigationControl());
      // }

      // map.addData(geoJson);
    }
    // Add zoom and rotation controls to the map
    if (window.innerWidth > 1280) {
      map.addControl(new mapboxgl.NavigationControl());
      //map.addControl(new mapboxgl.FullscreenControl());
    }
    this.mapObj = map;

  }

  /**
  * Function to render pie chart
  Create the API Result Analysis Graph
  * @param {[type]} container [description]
  */
  renderPieChart(container, data) {

    // Create chart instance
    var chart = am4core.create(container, am4charts.PieChart);

    var chartObj = [];
    let totalCount = 0;
    if (data && data.length > 0) {

      for (var i = 0; i < data.length; i++) {
        totalCount += data[i].totalCount;
        chartObj.push({
          "sector": data[i].statusName,
          "size": data[i].totalCount,
          "color": am4core.color(data[i].statusColor)
        });
      }
    }

    chart.data = chartObj;
    chart.innerRadius = 35;

    // Add and configure Series
    var pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "size";
    pieSeries.dataFields.category = "sector";
    pieSeries.labels.template.disabled = true;
    pieSeries.slices.template.propertyFields.fill = "color";

    pieSeries.tooltip.autoTextColor = false;
    pieSeries.tooltip.label.fill = am4core.color("#FFFFFF");

    let label = pieSeries.createChild(am4core.Label);
    label.text = totalCount.toString();
    label.fontSize = 16;
    label.verticalCenter = "middle";
    label.horizontalCenter = "middle";
    label.fontFamily = "roboto-medium";
  }

  /**
  * Function to render half donut chart
  Create the Inpection Reject Analysis Graph
  */
  halfDonutInspectionRejectChart() {
    let chart = am4core.create('halfdonutchart', am4charts.PieChart);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

    chart.data = this.customerDashboard.inspectionRejectDashboard;

    chart.radius = am4core.percent(70);
    chart.innerRadius = am4core.percent(40);
    chart.startAngle = 180;
    chart.endAngle = 360;

    let series = chart.series.push(new am4charts.PieSeries());
    series.dataFields.value = "totalCount";
    series.dataFields.category = "statusName";
    series.labels.template.disabled = true;

    series.slices.template.cornerRadius = 4;
    series.slices.template.innerCornerRadius = 7;
    series.slices.template.propertyFields.fill = "statusColor";
    series.slices.template.draggable = false;
    series.slices.template.inert = true;
    series.alignLabels = false;

    series.hiddenState.properties.startAngle = 90;
    series.hiddenState.properties.endAngle = 90;

    if (!this.isMobile) {
      chart.padding(0, 0, 0, -50);
    }
  }

  /**
  Create the product category graph
  */
  renderProductChart(chartObj) {

    var chart = am4core.create("pathchartdiv", am4charts.XYChart);

    chart.data = chartObj;

    // Create axes
    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "year";
    categoryAxis.renderer.grid.template.opacity = 0;
    categoryAxis.renderer.labels.template.disabled = true;
    categoryAxis.renderer.baseGrid.disabled = true;

    var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.renderer.grid.template.opacity = 0;
    valueAxis.renderer.ticks.template.strokeOpacity = 0.5;
    valueAxis.renderer.ticks.template.stroke = am4core.color("#495C43");
    valueAxis.renderer.ticks.template.length = 10;
    valueAxis.renderer.line.strokeOpacity = 0;
    valueAxis.renderer.baseGrid.disabled = true;
    valueAxis.renderer.minGridDistance = 40;
    valueAxis.renderer.labels.template.disabled = true;

    // Create series
    function createSeries(field, name) {
      var series = chart.series.push(new am4charts.ColumnSeries());
      series.dataFields.valueX = field;
      series.dataFields.categoryY = "year";
      series.stacked = true;
      series.name = name;
      series.columns.template.tooltipText = field + " : {name}";
      series.columns.template.tooltipX = am4core.percent(100);

      series.tooltip.autoTextColor = false;
      series.tooltip.label.fill = am4core.color("#FFFFFF");
    }

    for (var i = 0; i < this.customerDashboard.productCategoryDashboard.length; i++) {

      createSeries(this.customerDashboard.productCategoryDashboard[i].statusName, this.customerDashboard.productCategoryDashboard[i].totalCount);

    }
    chart.padding(0, -0, 0, 0);

  }

  openSearchBox(event) {

    if (!this.isMobile) {
      this.searchQuery = '';
      this.hasSearchResult = false;
      this.searchActive = !this.searchActive;
      event.currentTarget.classList.toggle('active');
      this.searchContainer.nativeElement.classList.toggle('active');
    }
    else {
      this.mobileSearchContainer.nativeElement.classList.toggle('active');
    }
  }
  /**
  * Function to handle search factory feature
  */
  searchFactory() {
    if (this.searchQuery.length > 3) {

      this.requestModel.searchText = this.searchQuery;
      this.requestModel.customerId = this.customerInfo.id;
      this.requestModel.skip = 0;

      this.hasSearchResult = false;
      this.requestModel.supplierType = SupplierType.Factory;
      this.supService.getFactoryDataSourceList(this.requestModel)
        .subscribe(customerData => {
          this.factoryList = [];
          if (customerData && customerData.length > 0) {
            this.factoryList = this.factoryList.concat(customerData);
          }
          this.hasSearchResult = true;
        }),
        error => {
          this.hasSearchResult = false;
          //this.setError(error);
        };
    }
    else {
      this.hasSearchResult = false;
    }
  }

  /**
  * setting selected result in variables
  * @param {object} selectedResult object from search result array
  */
  getFactoryBookingDetails(selectedResult) {
    this.hasSearchResult = false;
    this.showFactoryDataLoader = true;
    this.selectedSearchResult = selectedResult;
    this.searchQuery = this.selectedSearchResult['name'];

    this.customerFilterModel.factoryId = selectedResult.id;
    this.service.getinspectedbookingsByFactory(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response != null) {
            this.showFactoryResult = true;
            this.factoryInspectionDetails = response;

            this.mapHeight = this.showFactoryResult ? 510 : 380;
            //this.customerDashBoardLoader.inspectedBookingLoading = false;
            setTimeout(() => {
              this.mapObj.resize();
            }, 100);

            if (response.latitude && response.longitude) {
              var marker = new mapboxgl.Marker()
                .setLngLat([response.longitude, response.latitude])
                .addTo(this.mapObj);
              this.markers.push(marker);
              this.mapObj.jumpTo({ 'center': [response.longitude, response.latitude], 'zoom': 5 });
            }
            this.showFactoryDataLoader = false;
          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.customerDashBoardLoader.inspectedBookingLoading = false;
        });

    //this.showFactoryResult = true;


    setTimeout(() => {
      this.mapObj.resize();
    }, 100);

    // hiding mobile search popup when some result is selected
    if (this.isMobile) {
      this.mobileSearchContainer.nativeElement.classList.remove('active');
    }
  }

  undoFactorySelection() {
    this.showFactoryResult = false;
    this.searchQuery = null;
    this.factoryList = [];
    this.customerFilterModel.factoryId = null;
    if (this.markers[0]) { this.markers[0].remove(); this.markers.pop() }
    this.mapObj.jumpTo({ 'center': [this.centreLongitude, this.centreLatitude], 'zoom': 1.30 })
    this.mapHeight = this.showFactoryResult ? 510 : 380;
  }

  toggleBusinessOverview() {
    this.businessOverviewTrigger.nativeElement.classList.toggle('active');
    this.buinessOverviewCard.nativeElement.classList.toggle('active');
  }

  validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
    const parsed = this.formatter.parse(input);
    return parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
  }

  onDateSelection(date: NgbDate, isMobile: boolean) {
    if (!this.customerFilterModel.serviceDateFrom && !this.customerFilterModel.serviceDateTo) {
      this.customerFilterModel.serviceDateFrom = date;
    } else if (this.customerFilterModel.serviceDateFrom && !this.customerFilterModel.serviceDateTo && date && date.after(this.customerFilterModel.serviceDateFrom)) {
      this.customerFilterModel.serviceDateTo = date;
      this.datepicker.close();
    } else {
      this.customerFilterModel.serviceDateTo = null;
      this.customerFilterModel.serviceDateFrom = date;
    }

    if (this.customerFilterModel.serviceDateFrom != null && this.customerFilterModel.serviceDateTo != null && !isMobile)
      this.getCustomerDashBoard();
  }

  isHovered(date: NgbDate) {
    return this.customerFilterModel.serviceDateFrom && !this.customerFilterModel.serviceDateTo && this.hoveredDate && date.after(this.customerFilterModel.serviceDateFrom) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.customerFilterModel.serviceDateTo && date.after(this.customerFilterModel.serviceDateFrom) && date.before(this.customerFilterModel.serviceDateTo);
  }

  isRange(date: NgbDate) {
    return date.equals(this.customerFilterModel.serviceDateFrom) || (this.customerFilterModel.serviceDateTo && date.equals(this.customerFilterModel.serviceDateTo)) || this.isInside(date) || this.isHovered(date);
  }

  toggleFilter() {
    this.filterOpen = !this.filterOpen;
  }

  //get manday list for each year
  getMandaysByYear() {




    this.service.getmandayByYear(this.customerFilterModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == MandayDashboardResult.success) {
              this.mandayYearChart = response.data;
              this.monthYearXAxis = response.monthYearXAxis;
              this.mandayByYearChartFrame();
              this.customerDashBoardDataFound.manDaysFound = true;
            }
            else {
              this.mandayYearChart = new Array<MandayYearChart>();
              this.customerDashBoardDataFound.manDaysFound = false;
            }
          }
          this.customerDashBoardLoader.manDaysLoading = false;
        },
        error => {
          this.customerDashBoardLoader.manDaysLoading = false;
          this.customerDashBoardLoader.manDaysError = true;
        });
  }

  // frame the manday year
  mandayByYearChartFrame() {

    if (this.mandayYearChart && this.mandayYearChart.length > 0) {

      // to build the value2 we are declare the below 2
      let k: number = 2;
      var chartObj = [];

      //building below structure
      //{date:new Date(2019,1), value2:48, value3:51, value4:42}
      for (var i = 0; i < this.mandayYearChart.length; i++) {

        if (i == 0) {

          if (this.monthYearXAxis != null) {
            for (var l = 0; l < this.monthYearXAxis.length; l++) {

              chartObj.push({
                date: new Date(this.monthYearXAxis[l].year, this.monthYearXAxis[l].month)
              });
            }
          }
        }

        for (var m = 0; m < chartObj.length; m++) {

          var monthData = this.mandayYearChart[i].monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + 1));

          chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;
        }
        k = k + 1;
      }
    }
    setTimeout(() => {
      this.renderYearLineChart(chartObj, this.mandayYearChart);
    }, 100);
  }

  /**
  * Function to render line chart for manday by year
  */
  renderYearLineChart(chartObj, mandayYearChar: MandayYearChart[]) {

    let chart = am4core.create("linechartdiv", am4charts.XYChart);

    chart.data = chartObj;
    chart.dateFormatter.dateFormat = "dd/MM/yyyy";
    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.DateAxis());
    categoryAxis.dataFields.date = "date";
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.labels.template.fontSize = 8;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    categoryAxis.dateFormatter = new am4core.DateFormatter();
    categoryAxis.dateFormatter.dateFormat = "MM-dd";

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minGridDistance = 20;
    valueAxis.renderer.labels.template.fontSize = 8;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");


    for (var i = 0; i < mandayYearChar.length; i++) {
      // Create series
      let series = chart.series.push(new am4charts.LineSeries());
      series.legendSettings.valueText = "[bold]{valueY.close}[/]";
      series.stroke = am4core.color(mandayYearChar[i].color);
      series.dataFields.valueY = "value" + (i + 2);
      series.dataFields.dateX = "date";
      series.name = 'value' + (i + 1);
      series.strokeWidth = 2;
      series.tooltipText = "{dateX}: [b]{valueY}[/]";
      series.tooltip.getFillFromObject = false;
      series.tooltip.background.fill = am4core.color(mandayYearChar[i].color);
    }
    chart.padding(20, 0, 20, 0);
    chart.cursor = new am4charts.XYCursor();

    //this.exportChart(chart);

  }
  setError(error) {

    this.error = error;

    if (error == "Unauthorized")
      this.router.navigate(['/error/401']);
    else if (error == "NotFound")
      this.router.navigate(['/error/404']);
    else
      this.router.navigate([`/error/${error}`]);

  }

  getCustomerList() {
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();
    this.requestCustomerModel.isVirtualScroll = false;
    this.cusService.getCustomerListByUserType(this.requestCustomerModel, this.currentUser.usertype)
      .subscribe(res => {
        this.dashBoardFilterMaster.customerList = res;
        this.brandSearchRequest.customerId = this.customerFilterModel.customerId;
        this.deptSearchRequest.customerId = this.customerFilterModel.customerId;
        this.buyerSearchRequest.customerId = this.customerFilterModel.customerId;
        this.collectionSearchRequest.customerId = this.customerFilterModel.customerId;
        this.getBrandListBySearch();
        this.getBuyerListBySearch();
        this.getCollectionListBySearch();
        this.getDeptListBySearch();
      });
  }

  getCustomer(id) {
    this.customerLoading = true;
    this.cusService.GetCustomerByCustomerId(id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.brandSearchRequest.customerId = this.customerFilterModel.customerId;
            this.deptSearchRequest.customerId = this.customerFilterModel.customerId;
            this.buyerSearchRequest.customerId = this.customerFilterModel.customerId;
            this.collectionSearchRequest.customerId = this.customerFilterModel.customerId;
            this.getBrandListBySearch();
            this.getBuyerListBySearch();
            this.getCollectionListBySearch();
            this.getDeptListBySearch();
          }
          else {
            this.error = data.result;
          }
          this.customerLoading = false;
        },
        error => {
          //this.setError(error);
          this.customerLoading = false;
        });
  }

  cancelFilter() {
    this.customerFilterModel.supplierId = null;
    this.customerFilterModel.selectedCountryIdList = null;
    this.setDefaultFilter();
    this.getCustomerDashBoard();
    this.filterOpen = false;
    this.dashBoardFilterMaster.filterDataShown = false;
    this.customerFilterModel.selectedBrandIdList = [];
    this.customerFilterModel.selectedBuyerIdList = [];
    this.customerFilterModel.selectedCollectionIdList = [];
    this.customerFilterModel.selectedDeptIdList = [];
    this.dashBoardFilterMaster.supplierName = null;
    this.dashBoardFilterMaster.countryListName = null;

  }

  async search(isMobile: boolean) {
    var filterChanged = false;
    this.dashBoardFilterMaster.filterDataShown = this.filterTextShown();
    this.mapLoading = true;
    if (isMobile) {
      this.validator.isSubmitted = true;
      if (this.formValid()) {
        if (this.filterOpen)
          this.filterOpen = false;

        if (this.customerFilterModel.supplierId || this.customerFilterModel.selectedCountryIdList || (this.customerFilterModel.serviceDateFrom && this.customerFilterModel.serviceDateTo)) {
          this.searchLoading = true;
          await this.getCustomerDashBoard();

          setTimeout(() => {
            this.searchLoading = false;
          }, 1000);
        }
        else
          this.searchLoading = false;
      }
    }
    else {

      this.searchLoading = true;
      await this.getCustomerDashBoard();

      setTimeout(() => {
        this.searchLoading = false;
      }, 1000);

    }
  }

  //frame the manday by employee chart
  productOverviewChartFrame() {

    //building below structure
    // {
    // 	"year": "2020",
    // 	"europe": 2.5,
    // 	"namerica": 2.5,
    // 	"asia": 2.1,
    // 	"lamerica": 1.2,
    // 	"meast": 5.2,
    // 	"africa": 0.1
    // }
    if (this.customerDashboard.productCategoryDashboard
      && this.customerDashboard.productCategoryDashboard.length > 0) {

      var chartObj = [];
      var subchartObj = {};

      subchartObj["year"] = new Date().getFullYear().toString();
      for (var i = 0; i < this.customerDashboard.productCategoryDashboard.length; i++) {


        subchartObj[this.customerDashboard.productCategoryDashboard[i].statusName] =
          this.customerDashboard.productCategoryDashboard[i].totalCount;

      }
      chartObj.push(subchartObj);

      setTimeout(() => {
        this.renderProductChart(chartObj);
      }, 300);
    }
  }

  //filter details has to show
  filterTextShown() {
    var isFilterDataSelected = false;
    //|| this.customerFilterModel.customerId > 0
    if (this.customerFilterModel.supplierId > 0
      || (this.customerFilterModel.selectedCountryIdList && this.customerFilterModel.selectedCountryIdList.length > 0) ||
      (this.customerFilterModel.selectedDeptIdList && this.customerFilterModel.selectedDeptIdList.length > 0) ||
      (this.customerFilterModel.selectedBrandIdList && this.customerFilterModel.selectedBrandIdList.length > 0) ||
      (this.customerFilterModel.selectedCollectionIdList && this.customerFilterModel.selectedCollectionIdList.length > 0) ||
      (this.customerFilterModel.selectedBuyerIdList && this.customerFilterModel.selectedBuyerIdList.length > 0)) {

      //desktop version
      if (!this.isMobile) {
        if (this.customerFilterModel.customerId) {
          var customerDetails = this.dashBoardFilterMaster.customerList.find(x => x.id == this.customerFilterModel.customerId);
          this.dashBoardFilterMaster.customerName = customerDetails ? customerDetails.name : "";
        }
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;

        if (this.customerFilterModel.supplierId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.customerFilterModel.selectedCountryIdList && this.customerFilterModel.selectedCountryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.customerFilterModel.selectedBrandIdList && this.customerFilterModel.selectedBrandIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.customerFilterModel.selectedBuyerIdList && this.customerFilterModel.selectedBuyerIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.customerFilterModel.selectedCollectionIdList && this.customerFilterModel.selectedCollectionIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.customerFilterModel.selectedDeptIdList && this.customerFilterModel.selectedDeptIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        this.dashBoardFilterMaster.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.dashBoardFilterMaster.filterCount = 0;
        this.dashBoardFilterMaster.countryListName = "";
        this.dashBoardFilterMaster.supplierName = "";
        this.dashBoardFilterMaster.customerName = "";
      }
    }

    return isFilterDataSelected;
  }
  //country change event
  countryChange(countryItem) {

    if (countryItem) {

      if (this.customerFilterModel.selectedCountryIdList && this.customerFilterModel.selectedCountryIdList.length > 0) {
        this.dashBoardFilterMaster.countryListName = "";

        var customerLength = this.customerFilterModel.selectedCountryIdList.length < CountrySelectedFilterTextCount ?
          this.customerFilterModel.selectedCountryIdList.length : CountrySelectedFilterTextCount;
        for (var i = 0; i < customerLength; i++) {

          var countryDetails = this.dashBoardFilterMaster.countryList.find(x => x.id == this.customerFilterModel.selectedCountryIdList[i]);

          if (i != customerLength - 1) {
            this.dashBoardFilterMaster.countryListName += countryDetails.name + ", ";
          }
          else {
            if (customerLength < CountrySelectedFilterTextCount) {
              this.dashBoardFilterMaster.countryListName += countryDetails.name;
            }
            else {
              this.dashBoardFilterMaster.countryListName += countryDetails.name + "...";
            }
          }
        }
      }
      else {
        this.dashBoardFilterMaster.countryListName = "";
      }
    }
  }
  //supplier change event
  supplierChange(supplierItem) {
    if (supplierItem && supplierItem.id > 0) {
      var supplierDetails = this.dashBoardFilterMaster.supplierList.find(x => x.id == supplierItem.id);
      if (supplierDetails)
        this.dashBoardFilterMaster.supplierName =
          supplierDetails.name.length > SupplierNameTrim ?
            supplierDetails.name.substring(0, SupplierNameTrim) + "..." : supplierDetails.name;

      this.dashBoardFilterMaster.productList = [];
      this.customerFilterModel.productIdList = [];
      this.getProductListBySearch();
    }
    else {
      this.dashBoardFilterMaster.supplierName = "";
    }
  }

  formValid() {

    var e = Date.parse(this.customerFilterModel.serviceDateFrom);
    return this.validator.isValid('serviceDateTo') &&
      this.validator.isValid('serviceDateFrom');

  }

  //fetch the country data with virtual scroll
  getCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.countryRequest.searchText = this.dashBoardFilterMaster.countryInput.getValue();
      this.countryRequest.skip = this.dashBoardFilterMaster.countryList.length;
    }

    this.dashBoardFilterMaster.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.countryList = this.dashBoardFilterMaster.countryList.concat(customerData);
        }
        if (isDefaultLoad)
          this.countryRequest = new CountryDataSourceRequest();
        this.dashBoardFilterMaster.countryLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.dashBoardFilterMaster.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.countryLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.countryList = data;
        this.dashBoardFilterMaster.countryLoading = false;
      });
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.requestModel.supSearchTypeId = this.dashBoardFilterMaster.searchtypeid;
    this.requestModel.customerId = this.customerInfo.id;
    this.requestModel.supplierType = SupplierType.Supplier;
    this.dashBoardFilterMaster.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.supLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.requestModel, term)
        : this.supService.getFactoryDataSourceList(this.requestModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.supLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.supplierList = data;
        this.dashBoardFilterMaster.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestModel.searchText = this.dashBoardFilterMaster.supInput.getValue();
      this.requestModel.skip = this.dashBoardFilterMaster.supplierList.length;
    }
    this.requestModel.customerId = this.customerInfo.id;
    this.requestModel.supplierType = SupplierType.Supplier;
    this.dashBoardFilterMaster.supLoading = true;
    this.requestModel.supSearchTypeId = this.dashBoardFilterMaster.searchtypeid;
    this.supService.getFactoryDataSourceList(this.requestModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.supplierList = this.dashBoardFilterMaster.supplierList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.requestModel.skip = 0;
          this.requestModel.take = ListSize;
        }
        this.dashBoardFilterMaster.supLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.supLoading = false;
        this.setError(error);
      };
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }


  //fetch the brand data with virtual scroll
  getBrandData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.brandSearchRequest.searchText = this.dashBoardFilterMaster.brandInput.getValue();
      this.brandSearchRequest.skip = this.dashBoardFilterMaster.brandList.length;
    }

    this.dashBoardFilterMaster.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.dashBoardFilterMaster.brandList = this.dashBoardFilterMaster.brandList.concat(brandData);
        }
        if (isDefaultLoad)
          this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.dashBoardFilterMaster.brandLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.brandLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.dashBoardFilterMaster.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.brandLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.brandList = data;
        this.dashBoardFilterMaster.brandLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getDeptData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.deptSearchRequest.searchText = this.dashBoardFilterMaster.deptInput.getValue();
      this.deptSearchRequest.skip = this.dashBoardFilterMaster.deptList.length;
    }

    this.dashBoardFilterMaster.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.dashBoardFilterMaster.deptList = this.dashBoardFilterMaster.deptList.concat(deptData);
        }
        if (isDefaultLoad)
          this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.dashBoardFilterMaster.deptLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.deptLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {
    this.dashBoardFilterMaster.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.deptLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.deptList = data;
        this.dashBoardFilterMaster.deptLoading = false;
      });
  }

  //fetch the buyer data with virtual scroll
  getBuyerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.buyerSearchRequest.searchText = this.dashBoardFilterMaster.buyerInput.getValue();
      this.buyerSearchRequest.skip = this.dashBoardFilterMaster.buyerList.length;
    }

    this.dashBoardFilterMaster.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.dashBoardFilterMaster.buyerList = this.dashBoardFilterMaster.buyerList.concat(buyerData);
        }
        if (isDefaultLoad)
          this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.dashBoardFilterMaster.buyerLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.buyerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.dashBoardFilterMaster.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.buyerLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.buyerList = data;
        this.dashBoardFilterMaster.buyerLoading = false;
      });
  }

  //fetch the collection data with virtual scroll
  getCollectionData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.collectionSearchRequest.searchText = this.dashBoardFilterMaster.collectionInput.getValue();
      this.collectionSearchRequest.skip = this.dashBoardFilterMaster.collectionList.length;
    }

    this.dashBoardFilterMaster.collectionLoading = true;
    this.collectionService.getCollectionListByCustomerId(this.collectionSearchRequest).
      subscribe(collectionData => {
        if (collectionData && collectionData.length > 0) {
          this.dashBoardFilterMaster.collectionList = this.dashBoardFilterMaster.collectionList.concat(collectionData);
        }
        if (isDefaultLoad)
          this.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.dashBoardFilterMaster.collectionLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.collectionLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count collection on load
  getCollectionListBySearch() {
    this.dashBoardFilterMaster.collectionInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.collectionLoading = true),
      switchMap(term => term
        ? this.collectionService.getCollectionListByCustomerId(this.collectionSearchRequest, term)
        : this.collectionService.getCollectionListByCustomerId(this.collectionSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.collectionLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.collectionList = data;
        this.dashBoardFilterMaster.collectionLoading = false;
      });
  }
  fullscreenMap() {

    if (this.isFullscreen) {
      this.mapHeight = 380;
    }
    else {
      this.mapHeight = window.innerHeight - 65;
    }

    this.isFullscreen = !this.isFullscreen;

    setTimeout(() => {
      this.mapObj.resize();
    }, 100);
  }

  //fetch the product category data with virtual scroll
  getProductCategoryData() {
    this.dashBoardFilterMaster.productCategoryListLoading = true;
    this.quantitativeService.getProductCategorySummary()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response && response.result == ResponseResult.Success) {
          this.dashBoardFilterMaster.productCategoryList = response.productCategoryList;
        }
        this.dashBoardFilterMaster.productCategoryListLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.productCategoryListLoading = false;
      };
  }

  getProductData() {
    this.dashBoardFilterMaster.productRequest.searchText = this.dashBoardFilterMaster.productInput.getValue();
    this.dashBoardFilterMaster.productRequest.skip = this.dashBoardFilterMaster.productList.length;
    this.dashBoardFilterMaster.productRequest.customerIds.push(this.customerFilterModel.customerId);

    this.dashBoardFilterMaster.productRequest.productIds = [];
    this.dashBoardFilterMaster.productLoading = true;
    this.customerProductService.getProductDataSource(this.dashBoardFilterMaster.productRequest).
      subscribe(productData => {
        if (productData && productData.length > 0) {
          this.dashBoardFilterMaster.productList = this.dashBoardFilterMaster.productList.concat(productData);
        }

        this.dashBoardFilterMaster.productRequest = new ProductDataSourceRequest();
        this.dashBoardFilterMaster.productRequest.customerIds.push(this.customerFilterModel.customerId);
        this.dashBoardFilterMaster.productLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.productLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getProductListBySearch() {

    this.dashBoardFilterMaster.productRequest = new ProductDataSourceRequest();

    this.dashBoardFilterMaster.productRequest.customerIds.push(this.customerFilterModel.customerId);
    if (this.customerFilterModel.supplierId > 0) {
      this.dashBoardFilterMaster.productRequest.supplierIdList.push(this.customerFilterModel.supplierId);
    }
    else {
      this.dashBoardFilterMaster.productRequest.supplierIdList = [];
    }
    //this.dashBoardFilterMaster.productRequest.supplierIdList.push(this.customerFilterModel.factoryId);
    this.dashBoardFilterMaster.productInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.productLoading = true),
      switchMap(term => term
        ? this.customerProductService.getProductDataSource(this.dashBoardFilterMaster.productRequest, term)
        : this.customerProductService.getProductDataSource(this.dashBoardFilterMaster.productRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.productLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.productList = data;
        this.dashBoardFilterMaster.productLoading = false;

      });
  }

  SetSearchTypemodel(item) {
    this.customerFilterModel.supplierId = null;
    this.getSupListBySearch();
  }


  clearProductRef() {
    this.getProductListBySearch();
  }

  changeProductRef() {
    this.getProductListBySearch();
  }

  //get Customer Decision Count
  getCustomerDecisionCount() {
    this.customerDashBoardLoader.customerDecisionLoading = true;
    this.service.getCustomerDecisionCount(this.customerFilterModel.customerId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response != null) {
            this.customerDecisionCount = response;
            this.customerDashBoardLoader.customerDecisionLoading = false;
          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.customerDashBoardLoader.customerDecisionLoading = false;
        });
  }

  //navigate to customer decision summary
  navigateCustomerDecisionSummary() {
    let path = "customerdecision/customer-decision/";
    let entity: string = this.utility.getEntityName();
    let customerDecisionDdl = CustomerDecisionDdl.find(x => x.name == 'Not Given');
    if (this.customerDecisionCount.pendingDecisionCount > 0 && customerDecisionDdl) {
      this.router.navigate([`/${entity}/${path}/${this.customerFilterModel.customerId}/${customerDecisionDdl.id}`,]);
    }
  }
}


//not used
 // getSuppliersbyCustomers(customerId) {

  //   this.supService.getSuppliersbyCustomer(customerId)
  //     .pipe()
  //     .subscribe(
  //       res => {
  //         if (res && res.result == 1) {
  //           this.dashBoardFilterMaster.supplierList = res.data;
  //         }
  //         else {
  //           this.dashBoardFilterMaster.supplierList = [];
  //         }
  //       },
  //       error => {
  //         this.dashBoardFilterMaster.supplierList = [];
  //       });
  // }

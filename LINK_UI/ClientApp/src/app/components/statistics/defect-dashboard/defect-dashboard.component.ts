import { Component } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { Validator } from '../../common/validator';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CountryDefectModel, DefectCategoryModel, DefectCountList, DefectDashboardModel, DefectDashboardResult, DefectMasterModel, DefectPhotoList, DefectYearCountDataModel, FactoryDataSourceRequest, ParetoModel, SupFactDefectAnalysis } from 'src/app/_Models/statistics/defect-dashboard.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, CustomerCommonDataSourceRequest, ProductDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { DefectDashboardCountryTextCount, DefectDashboardSupplierNameTrim, ListSize, MobileViewFilterCount, DefectDashboardDataLengthTrim, SupplierType, amCoreLicense, DefectDashboardParetoNameTrim, DefectNameList, DefectCritical, DefectMajor, DefectMinor, DefectCriticalId, DefectMajorId, DefectMinorId, DashboardCountryDefectNameTrim, DashboardDefectNamecPopupTrim, UserType, supplierTypeList, SearchType, GroupByFilter } from '../../common/static-data-common';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { NgbCalendar, NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
// NgbDateParserFormatter
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { DefectDashboardService } from 'src/app/_Services/statistics/defect-dashboard.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { commonDataSource } from 'src/app/_Models/booking/inspectionbooking.model';
import { GroupByFilterEnum } from 'src/app/_Models/statistics/rejectiondashborad.model';

@Component({
  selector: 'app-defect-dashboard',
  templateUrl: './defect-dashboard.component.html',
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
export class DefectDashboardComponent extends DetailComponent {

  componentDestroyed$: Subject<boolean> = new Subject();
  isFilterOpen: boolean;
  model: DefectDashboardModel;
  masterModel: DefectMasterModel;
  toggleFormSection: boolean;
  requestCustomerModel: CustomerCommonDataSourceRequest;
  currentUser: UserModel;
  LoadFirstTime: boolean = true;
  userTypeEnum = UserType;
  // _SupplierType: SupplierType;
  supplierTypeList: any = supplierTypeList;
  defectLandingRequest: DefectDashboardModel;
  constructor(router: Router, route: ActivatedRoute, public validator: Validator, translate: TranslateService, toastr: ToastrService,
    public locationService: LocationService, public supService: SupplierService, public cusService: CustomerService,
    private buyerService: CustomerbuyerService, private brandService: CustomerBrandService,
    private deptService: CustomerDepartmentService, private collectionService: CustomerCollectionService,
    private productManagementService: ProductManagementService,
    private customerProductService: CustomerProduct,
    private authService: AuthenticationService, public calendar: NgbCalendar, public defectDashboardService: DefectDashboardService,
    public utility: UtilityService, public modalService: NgbModal) {

    super(router, route, translate, toastr);

    am4core.addLicense(amCoreLicense);
    this.currentUser = authService.getCurrentUser();
    this.model = new DefectDashboardModel();
    this.masterModel = new DefectMasterModel();
    this.defectLandingRequest = new DefectDashboardModel();
    this.model.supplierTypeId = SearchType.SupplierName;
    this.masterModel.supplierRequest.supSearchTypeId = SearchType.SupplierName;

    this.validator.setJSON("statistics/defect-dashboard/defect-dashboard.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();
    this.masterModel.defectOptions = [
      {
        width: '800px',
        height: '500px',
        "preview": false,
        "imageDescription": true
      },
      { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
      { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 }
    ];
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(id?: any, inputparam?: ParamMap): void {

    this.getIsMobile();

    this.defectWordFrame();
    this.defectTitleFrame();

    this.placeHolderLoad();
    this.setDefaultFilter();

    if (this.currentUser.usertype == UserType.Supplier) {
      this.model.supplierId = this.currentUser.supplierid;
      this.getSupListBySearch();
    }

    if (this.currentUser.usertype == UserType.Factory) {
      this.getSupListBySearch();
      this.model.factoryIds.push(this.currentUser.factoryid);
      this.getFactListBySearch();
    }

    this.getCustomerListBySearch();

    this.getCountryListBySearch();

    this.DefectDateChange();

    this.getProductCategoryData();

    this.getDefectListBySearch();
  }

  //defect word frame supplier or factory
  defectWordFrame() {
    if (this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.typeId == SupplierType.Factory) {
      this.masterModel.defectwordFrame = this.utility.textTranslate('DEFECT_DASHBOARD.LBL_FACTORIES');
    }
    else {
      this.masterModel.defectwordFrame = this.utility.textTranslate('DEFECT_DASHBOARD.LBL_SUPPLIERS');
    }
  }

  //defect title frame suupplier or factory
  defectTitleFrame() {
    if (this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.typeId == SupplierType.Factory) {
      this.masterModel.defectTitle = this.utility.textTranslate('DEFECT_DASHBOARD.LBL_FACTORY');
    }
    else {
      this.masterModel.defectTitle = this.utility.textTranslate('DEFECT_DASHBOARD.LBL_SUPPLIER');
    }
  }

  //toggle advance search section
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  //search filters shown
  toggleFilter() {
    this.isFilterOpen = !this.isFilterOpen;
  }


  //Make CustomerInfo with customer id and name
  assignCustomerInfo() {
    if (this.currentUser) {
      this.masterModel.customerInfo.id = this.currentUser.customerid;
      this.masterModel.customerInfo.name = this.currentUser.fullName;
    }
  }

  //set default filter data
  setDefaultFilter() {
    this.model.fromDate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.model.toDate = this.calendar.getToday();
  }

  //reset the page
  reset() {
    this.model.supplierId = null;
    this.model.factoryCountryIds = [];
    this.model.factoryIds = [];
    this.masterModel.supplierName = null;
    this.masterModel.countryListName = null;
    this.model.selectedBrandIdList = null;
    this.model.selectedBuyerIdList = null;
    this.model.selectedCollectionIdList = null;
    this.model.selectedDeptIdList = null;
    this.setDefaultFilter();
    this.toggleFilter();
    this.masterModel.filterDataShown = false;
    this.placeHolderLoad();
    this.noDataFoundLoadAllCharts();

    if (this.currentUser.usertype == UserType.Supplier)
      this.model.supplierId = this.currentUser.supplierid;

    if (this.currentUser.usertype == UserType.Factory) {
      this.model.factoryIds.push(this.currentUser.factoryid);
      this.getFactListBySearch();
    }
    this.getCustomerListBySearch();

  }

  getCustomerListBySearch() {

    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term =>
        this.cusService.getCustomerListByUserType(this.requestCustomerModel, null, term)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.customerLoading = false))
      ))
      .subscribe(data => {
        if (!this.LoadFirstTime)
          this.masterModel.customerList = data;
        else {
          if (data && data.length > 0) {
            this.masterModel.customerList = data;
          }
          this.model.customerId = this.masterModel.customerList[0].id;
          this.masterModel.supplierRequest.customerId = this.model.customerId;
          this.masterModel.brandSearchRequest.customerId = this.model.customerId;
          this.masterModel.deptSearchRequest.customerId = this.model.customerId;
          this.masterModel.buyerSearchRequest.customerId = this.model.customerId;
          this.masterModel.collectionSearchRequest.customerId = this.model.customerId;
          this.getBrandListBySearch();
          this.getBuyerListBySearch();
          this.getCollectionListBySearch();
          this.getDeptListBySearch();
          this.getProductListBySearch();

          if (this.currentUser.usertype == UserType.InternalUser || this.currentUser.usertype == UserType.Customer)
            this.getSupListBySearch();

          if (this.currentUser.usertype == UserType.Supplier)
            this.getFactListBySearch();

          this.getBaseBookingReportData();
          this.getLowPerformanceDefectList();
          this.getCountryDefectList();
          this.getBookingChangeYearDetails();
          this.LoadFirstTime = false;
        }
        this.masterModel.customerLoading = false;
        this.stopPlaceHolderLoad();
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.requestCustomerModel.searchText = this.masterModel.customerInput.getValue();
      this.requestCustomerModel.skip = this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.cusService.getCustomerListByUserType(this.requestCustomerModel).
      subscribe(customerData => {
        if (IsVirtual) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
            this.masterModel.customerList = this.masterModel.customerList.concat(customerData);
          }
        }
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }
  changeCustomerData(item) {
    if (item && item.id > 0) {
      this.masterModel.supplierList = [];
      if (this.currentUser.usertype != UserType.Supplier)
        this.model.supplierId = null;
      this.masterModel.supplierRequest.supSearchTypeId = SearchType.SupplierName;
      this.model.supplierTypeId = SearchType.SupplierName;
      this.masterModel.supplierRequest.customerId = item.id;
      this.masterModel.brandSearchRequest.customerId = item.id;
      this.masterModel.deptSearchRequest.customerId = item.id;
      this.masterModel.buyerSearchRequest.customerId = item.id;
      this.masterModel.collectionSearchRequest.customerId = item.id;
      this.getBrandListBySearch();
      this.getBuyerListBySearch();
      this.getCollectionListBySearch();
      this.getDeptListBySearch();
      this.getProductListBySearch();
      this.getFactListBySearch();
      this.getSupListBySearch();
    }
  }
  //country change event get the value to show in strip
  countryChange(countryItem) {

    if (countryItem) {

      if (this.model.factoryCountryIds && this.model.factoryCountryIds.length > 0) {
        this.masterModel.countryListName = "";

        var customerLength = this.model.factoryCountryIds.length < DefectDashboardCountryTextCount ?
          this.model.factoryCountryIds.length : DefectDashboardCountryTextCount;
        for (var i = 0; i < customerLength; i++) {

          var countryDetails = this.masterModel.countryList.find(x => x.id == this.model.factoryCountryIds[i]);

          if (i != customerLength - 1) {
            this.masterModel.countryListName += countryDetails.name + ", ";
          }
          else {
            if (customerLength < DefectDashboardCountryTextCount) {
              this.masterModel.countryListName += countryDetails.name;
            }
            else {
              this.masterModel.countryListName += countryDetails.name + "...";
            }
          }
        }
      }
      else {
        this.masterModel.countryListName = "";
      }
    }
  }


  //fetch the country data with virtual scroll
  getCountryData() {
    this.masterModel.countryRequest.searchText = this.masterModel.countryInput.getValue();
    this.masterModel.countryRequest.skip = this.masterModel.countryList.length;

    this.masterModel.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.masterModel.countryRequest).
      pipe(takeUntil(this.componentDestroyed$))
      .subscribe(CountryData => {
        if (CountryData && CountryData.length > 0) {
          this.masterModel.countryList = this.masterModel.countryList.concat(CountryData);
        }
        this.masterModel.countryRequest = new CountryDataSourceRequest();
        this.masterModel.countryLoading = false;
      }),
      error => {
        this.masterModel.countryLoading = false;
        this.setError(error);
      };
  }


  //fetch the defect data with virtual scroll
  getDefectData() {
    this.masterModel.defectRequest.searchText = this.masterModel.defectInput.getValue();
    this.masterModel.defectRequest.skip = this.masterModel.defectList.length;

    this.masterModel.defectLoading = true;
    this.defectDashboardService.getDefectDataSourceList(this.masterModel.defectRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.defectList = this.masterModel.defectList.concat(data);
        }
        this.masterModel.defectRequest.skip = 0;
        this.masterModel.defectRequest.take = ListSize;

        this.masterModel.defectLoading = false;
      }),
      error => {
        this.masterModel.defectLoading = false;
        this.setError(error);
      };
  }

  //fetch the product category data
  getProductCategoryData() {
    this.masterModel.productCategoryListLoading = true;
    this.productManagementService.getProductCategorySummary()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response && response.result == ResponseResult.Success) {
          this.masterModel.productCategoryList = response.productCategoryList;
        }
        this.masterModel.productCategoryListLoading = false;
      }),
      error => {
        this.masterModel.productCategoryListLoading = false;
      };
  }


  // get product list by search
  getProductListBySearch() {

    this.masterModel.productRequest = new ProductDataSourceRequest();

    this.masterModel.productRequest.customerIds.push(this.model.customerId);
    if (this.model.supplierId > 0) {
      this.masterModel.productRequest.supplierIdList.push(this.model.supplierId);
    }
    else {
      this.masterModel.productRequest.supplierIdList = [];
    }
    //this.dashBoardFilterMaster.productRequest.supplierIdList.push(this.customerFilterModel.factoryId);
    this.masterModel.productInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.productLoading = true),
      switchMap(term => term
        ? this.customerProductService.getProductDataSource(this.masterModel.productRequest, term)
        : this.customerProductService.getProductDataSource(this.masterModel.productRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.productLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.productList = data;
        this.masterModel.productLoading = false;

      });
  }

  //get product data
  getProductData() {
    this.masterModel.productRequest.searchText = this.masterModel.productInput.getValue();
    this.masterModel.productRequest.skip = this.masterModel.productList.length;
    this.masterModel.productRequest.customerIds.push(this.model.customerId);

    this.masterModel.productRequest.productIds = [];
    this.masterModel.productLoading = true;
    this.customerProductService.getProductDataSource(this.masterModel.productRequest).
      subscribe(productData => {
        if (productData && productData.length > 0) {
          this.masterModel.productList = this.masterModel.productList.concat(productData);
        }

        this.masterModel.productRequest = new ProductDataSourceRequest();
        this.masterModel.productRequest.customerIds.push(this.model.customerId);
        this.masterModel.productLoading = false;
      }),
      error => {
        this.masterModel.productLoading = false;
        this.setError(error);
      };
  }

  clearProductRef() {
    this.getProductListBySearch();
  }

  changeProductRef() {
    this.getProductListBySearch();
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.masterModel.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.masterModel.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.masterModel.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.countryLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.countryList = data;
        this.masterModel.countryLoading = false;
      });
  }

  //fetch the first 10 defect on load
  getDefectListBySearch() {
    this.masterModel.defectInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.defectLoading = true),
      switchMap(term => term
        ? this.defectDashboardService.getDefectDataSourceList(this.masterModel.defectRequest, term)
        : this.defectDashboardService.getDefectDataSourceList(this.masterModel.defectRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.defectLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.defectList = data;
        this.masterModel.defectLoading = false;
      });
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.masterModel.supplierRequest.customerId = this.model.customerId;

    //get the supplier data by id
    if (this.currentUser.usertype == UserType.Supplier)
      this.masterModel.supplierRequest.id = this.currentUser.supplierid;

    //get the supplier data by factory id
    if (this.currentUser.usertype == UserType.Factory)
      this.masterModel.supplierRequest.factoryId = this.currentUser.factoryid;

    this.masterModel.supplierRequest.supplierType = SupplierType.Supplier;
    this.masterModel.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.supplierLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterModel.supplierRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterModel.supplierRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.supplierLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.supplierList = data;
        this.masterModel.supplierLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.masterModel.supplierRequest.searchText = this.masterModel.supplierInput.getValue();
    this.masterModel.supplierRequest.skip = this.masterModel.supplierList.length;

    this.masterModel.supplierRequest.customerId = this.model.customerId;
    this.masterModel.supplierRequest.supplierType = SupplierType.Supplier;
    //get the supplier by factory id
    if (this.currentUser.usertype == UserType.Factory)
      this.masterModel.supplierRequest.factoryId = this.currentUser.factoryid;

    this.masterModel.supplierLoading = true;
    this.supService.getFactoryDataSourceList(this.masterModel.supplierRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(SupplierData => {
        if (SupplierData && SupplierData.length > 0) {
          this.masterModel.supplierList = this.masterModel.supplierList.concat(SupplierData);
        }
        this.masterModel.supplierRequest.skip = 0;
        this.masterModel.supplierRequest.take = ListSize;

        this.masterModel.supplierLoading = false;
      }),
      error => {
        this.masterModel.supplierLoading = false;
        this.setError(error);
      };
  }

  //supplier change event get supplier selected name shown in strip
  supplierChange(supplierItem) {
    if (supplierItem && supplierItem.id > 0) {
      var supplierDetails = this.masterModel.supplierList.find(x => x.id == supplierItem.id);
      if (supplierDetails)
        this.masterModel.supplierName =
          supplierDetails.name.length > DefectDashboardSupplierNameTrim ?
            supplierDetails.name.substring(0, DefectDashboardSupplierNameTrim) + "..." : supplierDetails.name;
      this.masterModel.factoryList = [];
      this.model.factoryIds = [];
      if (this.currentUser.usertype == UserType.Factory)
        this.model.factoryIds.push(this.currentUser.factoryid);
      this.getFactListBySearch();
    }
    else {
      this.masterModel.supplierName = "";
    }
  }

  //fetch the facotry data with virtual scroll
  getFactoryData() {
    this.masterModel.factoryRequest.searchText = this.masterModel.factoryInput.getValue();
    this.masterModel.factoryRequest.skip = this.masterModel.factoryList.length;

    this.masterModel.factoryRequest.supplierType = SupplierType.Factory;
    this.masterModel.factoryLoading = true;
    this.masterModel.factoryRequest.supplierId = this.model.supplierId;

    this.supService.getFactoryOrSupplierDataSource(this.masterModel.factoryRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.factoryList = this.masterModel.factoryList.concat(data);
        }
        this.masterModel.factoryRequest = new FactoryDataSourceRequest();
        this.masterModel.factoryLoading = false;
      }),
      error => {
        this.masterModel.factoryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 fact for the supplier on load
  getFactListBySearch() {
    this.masterModel.factoryList = null;
    this.masterModel.factoryRequest.supplierType = SupplierType.Factory;
    this.masterModel.factoryRequest.supplierId = this.model.supplierId;

    //add the customer filter for factory data
    this.masterModel.factoryRequest.customerId = this.model.customerId;

    //get the supplier data by id
    if (this.currentUser.usertype == UserType.Supplier) {
      this.masterModel.factoryRequest.supplierId = this.currentUser.supplierid;
      this.model.supplierId = this.currentUser.supplierid;
    }

    //get the supplier data by factory id
    if (this.currentUser.usertype == UserType.Factory)
      this.masterModel.supplierRequest.factoryId = this.currentUser.factoryid;

    this.masterModel.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.factoryLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryOrSupplierDataSource(this.masterModel.factoryRequest, term)
        : this.supService.getFactoryOrSupplierDataSource(this.masterModel.factoryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.factoryLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.factoryList = data;
        this.masterModel.factoryLoading = false;
        this.stopPlaceHolderLoad();
      });
  }

  //factory change event  get factory selected name for shown in strip
  factoryChange(factoryItem) {
    if (factoryItem && factoryItem.id > 0) {
      var factoryDetails = this.masterModel.factoryList.find(x => x.id == factoryItem.id);
      if (factoryDetails)
        this.masterModel.factoryName =
          factoryDetails.name.length > DefectDashboardSupplierNameTrim ?
            factoryDetails.name.substring(0, DefectDashboardSupplierNameTrim) + "..." : factoryDetails.name;
    }
    else {
      this.masterModel.factoryName = "";
    }
  }
  //fetch the brand data with virtual scroll
  getBrandData() {
    this.masterModel.brandSearchRequest.searchText = this.masterModel.brandInput.getValue();
    this.masterModel.brandSearchRequest.skip = this.masterModel.brandList.length;

    this.masterModel.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.masterModel.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.masterModel.brandList = this.masterModel.brandList.concat(brandData);
        }
        this.masterModel.brandSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.brandLoading = false;
      }),
      (error: any) => {
        this.masterModel.brandLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.masterModel.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.masterModel.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.masterModel.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.brandLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.brandList = data;
        this.masterModel.brandLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getDeptData() {
    this.masterModel.deptSearchRequest.searchText = this.masterModel.deptInput.getValue();
    this.masterModel.deptSearchRequest.skip = this.masterModel.deptList.length;

    this.masterModel.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.masterModel.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.masterModel.deptList = this.masterModel.deptList.concat(deptData);
        }
        this.masterModel.deptSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.deptLoading = false;
      }),
      (error: any) => {
        this.masterModel.deptLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {
    this.masterModel.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.masterModel.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.masterModel.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.deptLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.deptList = data;
        this.masterModel.deptLoading = false;
      });
  }

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.masterModel.buyerSearchRequest.searchText = this.masterModel.buyerInput.getValue();
    this.masterModel.buyerSearchRequest.skip = this.masterModel.buyerList.length;

    this.masterModel.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.masterModel.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.masterModel.buyerList = this.masterModel.buyerList.concat(buyerData);
        }
        this.masterModel.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.buyerLoading = false;
      }),
      (error: any) => {
        this.masterModel.buyerLoading = false;
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.masterModel.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.masterModel.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.masterModel.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.buyerLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.buyerList = data;
        this.masterModel.buyerLoading = false;
      });
  }

  //fetch the collection data with virtual scroll
  getCollectionData() {
    this.masterModel.collectionSearchRequest.searchText = this.masterModel.collectionInput.getValue();
    this.masterModel.collectionSearchRequest.skip = this.masterModel.collectionList.length;

    this.masterModel.collectionLoading = true;
    this.collectionService.getCollectionListByCustomerId(this.masterModel.collectionSearchRequest).
      subscribe(collectionData => {
        if (collectionData && collectionData.length > 0) {
          this.masterModel.collectionList = this.masterModel.collectionList.concat(collectionData);
        }
        this.masterModel.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.collectionLoading = false;
      }),
      (error: any) => {
        this.masterModel.collectionLoading = false;
      };
  }

  //fetch the first take (variable) count collection on load
  getCollectionListBySearch() {
    this.masterModel.collectionInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.collectionLoading = true),
      switchMap(term => term
        ? this.collectionService.getCollectionListByCustomerId(this.masterModel.collectionSearchRequest, term)
        : this.collectionService.getCollectionListByCustomerId(this.masterModel.collectionSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.collectionLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.collectionList = data;
        this.masterModel.collectionLoading = false;
      });
  }
  //clear selected supplier based on cascading data also to clear
  clearSupplier() {
    this.masterModel.factoryList = [];
    if (this.currentUser.usertype == UserType.Factory) {
      this.model.factoryIds.push(this.currentUser.factoryid);
      this.getFactListBySearch();
    }
  }

  //get view by mobile or any other device
  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
  }

  //different stip shown mobile and other devices
  filterTextShown(): boolean {
    var isFilterDataSelected = false;

    if (this.model.supplierId > 0 || (this.model.factoryIds && this.model.factoryIds.length > 0)
      || (this.model.factoryCountryIds && this.model.factoryCountryIds.length > 0)) {
      //desktop version
      if (!this.isMobile) {
        if (this.model.customerId) {
          var customerDetails = this.masterModel.customerList.find(x => x.id == this.model.customerId);
          this.masterModel.customerName = customerDetails ? customerDetails.name : "";
        }
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;

        if (this.model.supplierId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.factoryIds && this.model.factoryIds.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.factoryCountryIds && this.model.factoryCountryIds.length > 0) {
          count = MobileViewFilterCount + count;
        }

        this.masterModel.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.masterModel.filterCount = 0;
        this.masterModel.countryListName = "";
        this.masterModel.supplierName = "";
        this.masterModel.customerName = "";
      }
    }

    return isFilterDataSelected;
  }

  //form validation
  isFormValid(): boolean {
    return this.validator.isValid('fromDate') && this.validator.isValid('toDate');
  }

  //year dropdown change to get details
  yearChange(event) {

    if (event && event && event.id > 0) {
      this.model.innerDefectYearId = event.id;
    }
    else {
      this.model.innerDefectYearId = null;
    }
    this.masterModel.lineGraphLoadingChart = true;
    this.getBookingChangeYearDetails();
  }

  //search data by filters
  search() {
    this.masterModel.searchLoading = true;
    this.masterModel.filterDataShown = this.filterTextShown();
    this.validator.initTost();
    this.validator.isSubmitted = true;
    this.placeHolderLoad();
    this.DefectDateChange(); // added by nixon . if date range
    if (this.isFormValid()) {

      this.getBaseBookingReportData();

      this.getLowPerformanceDefectList();
      this.getCountryDefectList();
      this.getBookingChangeYearDetails();
    }
  }

  //Added placeholder in all charts
  placeHolderLoad() {
    this.masterModel.defectCategoryLoading = true;
    this.masterModel.lineGraphDefectLoading = true;
    this.masterModel.paretoLoading = true;
    this.masterModel.lowPerformanceLoading = true;
    this.masterModel.countryDefectLoading = true;
  }

  stopPlaceHolderLoad() {
    this.masterModel.defectCategoryLoading = false;
    this.masterModel.lineGraphDefectLoading = false;
    this.masterModel.paretoLoading = false;
    this.masterModel.lowPerformanceLoading = false;
    this.masterModel.countryDefectLoading = false;
  }

  //no data found shown in page level
  noDataFoundLoadAllCharts() {
    this.masterModel.lineGraphDefectLoading = false;
    this.masterModel.lineGraphDefectList = new Array<DefectYearCountDataModel>();
    this.masterModel.lineGraphChartDefectList = new Array<DefectYearCountDataModel>();
    this.masterModel.lineGraphDefectFound = false;

    this.masterModel.defectCategoryLoading = false;
    this.masterModel.defectCategoryList = new Array<DefectCategoryModel>();
    this.masterModel.defectCategoryFound = false;

    this.masterModel.paretoLoading = false;
    this.masterModel.paretoList = new Array<ParetoModel>();
    this.masterModel.paretoFound = false;

    this.masterModel.lowPerformanceLoading = false;
    this.masterModel.lowPerformanceList = new Array<SupFactDefectAnalysis>();
    this.masterModel.lowPerformanceFound = false;

    this.masterModel.countryDefectLoading = false;
    this.masterModel.countryDefectFound = false;
    this.masterModel.countryDefectList = [];
    this.masterModel.countryListXAxis = [];
    this.masterModel.searchLoading = false;
  }

  //drop down defect change to fetch the table details
  defectChange(event) {

    if (event && event.name) {
      this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.defectName = event.name;
    }
    else {
      this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.defectName = "";
    }

    this.getLowPerformanceDefectList();
  }

  //get report details by filters and apply those details in all charts
  getBaseBookingReportData() {
    this.defectDashboardService.getBookingReportDetails(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response != null) {
            if (response.result == DefectDashboardResult.Success) {

              this.masterModel.reportIdList = response.bookingReportModel.map(x => x.reportId);
              this.masterModel.defectYear = response.monthXAxis;

              this.getDefectCategoryList();
              this.getParetoDefectList();
            }
            else if (response.result == DefectDashboardResult.NotFound) {
              this.noDataFoundLoadAllCharts();
            }
          }
        },
        error => {
          this.showError('DEFECT_DASHBOARD.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        });
  }

  //Line graph change event call to bind the data
  getBookingChangeYearDetails() {
    this.masterModel.lineGraphDefectLoading = true;
    this.defectDashboardService.getBookingChangeYearDetails(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == DefectDashboardResult.Success) {

              this.masterModel.lineGraphDefectList = response.defectCountList;
              this.masterModel.defectYear = response.monthXAxis;
              this.masterModel.lineGraphChartDefectList = this.masterModel.lineGraphDefectList.filter(x => x.defectMonthList && x.defectMonthList.length > 0);

              setTimeout(() => {
                this.DefectYearChartFrame();
              }, 10);

              this.masterModel.lineGraphDefectFound = true;
            }
            else {
              this.masterModel.lineGraphDefectList = new Array<DefectYearCountDataModel>();
              this.masterModel.lineGraphChartDefectList = new Array<DefectYearCountDataModel>();
              this.masterModel.lineGraphDefectFound = false;
            }
          }
          this.masterModel.lineGraphLoadingChart = false;
          this.masterModel.lineGraphDefectLoading = false;
        },
        error => {
          this.masterModel.lineGraphLoadingChart = false;
          this.masterModel.lineGraphDefectLoading = false;
          this.masterModel.lineGraphDefectError = true;
        });
  }

  //get defect category list
  getDefectCategoryList() {
    this.defectDashboardService.getDefectCategoryList(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == DefectDashboardResult.Success) {

              if (response.defectCategoryList.length > 0) {
                response.defectCategoryList.slice(0, this.masterModel.defectCategoryDataLengthTrim).forEach(element => {
                  element.name = element.categoryName;
                  element.categoryName = element.categoryName.length > DashboardCountryDefectNameTrim ?
                    element.categoryName.substring(0, DashboardCountryDefectNameTrim) + "..." : element.categoryName
                });
              }
              if (response.defectCategoryList.length > this.masterModel.defectCategoryDataLengthTrim) {
                response.defectCategoryList.slice(this.masterModel.defectCategoryDataLengthTrim, response.defectCategoryList.length).forEach(element => {
                  element.name = element.categoryName;
                  element.categoryName = element.categoryName.length > DashboardDefectNamecPopupTrim ?
                    element.categoryName.substring(0, DashboardDefectNamecPopupTrim) + "..." : element.categoryName
                });
              }
              this.masterModel.defectCategoryList = response.defectCategoryList;
              setTimeout(() => {
                this.halfDonutChartDefectCategory('halfdonutchart');
              }, 10);
              this.masterModel.defectCategoryFound = true;
            }
            else {
              this.masterModel.defectCategoryList = new Array<DefectCategoryModel>();
              this.masterModel.defectCategoryFound = false;
            }
          }

          this.masterModel.defectCategoryLoading = false;
        },
        error => {
          this.masterModel.defectCategoryLoading = false;
          this.masterModel.defectCategoryError = true;
          // this.showError('DEFECT_DASHBOARD.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        });
  }

  // defect date change selected by default
  DefectDateChange() {
    if (this.model.fromDate != null && this.model.toDate != null) {
      this.masterModel.yearList = [];

      if (this.model.fromDate.year < this.model.toDate.year) {
        for (var i = this.model.fromDate.year; i <= this.model.toDate.year; i++) {

          this.masterModel.yearList.push(
            {
              id: i, name: "" + i
            }
          );
        }
        this.model.innerDefectYearId = this.masterModel.yearList[this.masterModel.yearList.length - 1].id;
        // this.model.innerDefectYearId = 2020;
      }
      else if (this.model.fromDate.year === this.model.toDate.year) {
        this.model.innerDefectYearId = this.model.fromDate.year;
      }
    }
  }

  //from date change - frame the year list - if date from and to have more than one year
  fromDateChange(fromDate) {

    if (this.isFormValid()) {
      var serviceFromDate: NgbDate;
      if (fromDate != null && this.model.toDate != null) {

        serviceFromDate = new NgbDate(fromDate.year, fromDate.month, fromDate.day);

        this.masterModel.yearList = [];

        if (serviceFromDate.year < this.model.toDate.year) {
          for (var i = serviceFromDate.year; i <= this.model.toDate.year; i++) {

            this.masterModel.yearList.push(
              {
                id: i, name: "" + i
              }
            );
          }
        }

      }

    }
  }

  //to date change - frame the year list - if date from and to have more than one year
  toDateChange(toDate) {

    if (this.isFormValid()) {
      var _toDate: NgbDate;
      if (toDate != null && this.model.fromDate != null) {
        _toDate = new NgbDate(toDate.year, toDate.month, toDate.day);

        this.masterModel.yearList = [];

        if (this.model.fromDate.year < _toDate.year) {
          for (var i = this.model.fromDate.year; i <= _toDate.year; i++) {

            this.masterModel.yearList.push({
              id: i, name: "" + i
            });
          }
        }
      }
    }
  }
  /**
  * Function to render half donut chart by defect category */
  halfDonutChartDefectCategory(graphContainerName) {
    let chart = am4core.create(graphContainerName, am4charts.PieChart);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

    chart.data = this.masterModel.defectCategoryList;

    chart.radius = am4core.percent(70);
    chart.innerRadius = am4core.percent(40);
    chart.startAngle = 180;
    chart.endAngle = 360;

    let series = chart.series.push(new am4charts.PieSeries());
    series.dataFields.value = "defectCountByCategory";
    series.dataFields.category = "categoryName";
    series.labels.template.disabled = true;

    series.slices.template.cornerRadius = 4;
    series.slices.template.innerCornerRadius = 7;
    series.slices.template.propertyFields.fill = "color";
    series.slices.template.draggable = false;
    series.slices.template.inert = true;
    series.alignLabels = false;

    series.hiddenState.properties.startAngle = 90;
    series.hiddenState.properties.endAngle = 90;

    chart.padding(0, 0, 0, 0);
  }

  // frame the defect line graph
  DefectYearChartFrame() {

    if (this.masterModel.lineGraphChartDefectList && this.masterModel.lineGraphChartDefectList.length > 0) {

      let k: number = 2;
      var chartObj = [];

      //building below structure
      //{date:new Date(2019,1), value2:48, value3:51, value4:42}
      for (var i = 0; i < this.masterModel.lineGraphChartDefectList.length; i++) {

        if (i == 0) {

          if (this.masterModel.defectYear != null) {
            for (var l = 0; l < this.masterModel.defectYear.length; l++) {

              chartObj.push({
                date: new Date(this.masterModel.defectYear[l].year, this.masterModel.defectYear[l].month)
              });
            }
          }
        }

        for (var m = 0; m < chartObj.length; m++) {

          var monthData = this.masterModel.lineGraphChartDefectList[i].defectMonthList.filter(x => x.month == (chartObj[m].date.getMonth() + 1));

          chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].defectMonthCount : 0;
        }
        k = k + 1;
      }
    }
    setTimeout(() => {
      this.renderYearLineChart(chartObj, this.masterModel.lineGraphChartDefectList);
    }, 100);
  }


  /**
  * Function to render line chart
  */
  renderYearLineChart(chartObj, lineGraphDefectList) {
    let chart = am4core.create("chartdiv", am4charts.XYChart);

    // Add data
    chart.data = chartObj;

    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.DateAxis());
    categoryAxis.dataFields.date = "date";
    categoryAxis.renderer.minGridDistance = 10;
    categoryAxis.renderer.labels.template.fontSize = 8;
    // categoryAxis.dateFormats.setKey("month", "MMMyy");
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minGridDistance = 20;
    valueAxis.renderer.labels.template.fontSize = 8;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    for (var i = 0; i < lineGraphDefectList.length; i++) {
      // Create series
      let series = chart.series.push(new am4charts.LineSeries());
      series.legendSettings.valueText = "[bold]{valueY.close}[/]";
      series.stroke = am4core.color(lineGraphDefectList[i].color);
      series.dataFields.valueY = "value" + (i + 2);
      series.dataFields.dateX = "date";
      series.name = 'value' + (i + 1);
      series.strokeWidth = 2;
      series.tooltipText = "{dateX}: [b]{valueY}[/]";
      series.tooltip.getFillFromObject = false;
      series.tooltip.background.fill = am4core.color(lineGraphDefectList[i].color);
    }
    chart.padding(20, 0, 20, 0);
    chart.cursor = new am4charts.XYCursor();
  }

  // get pareto defect list
  getParetoDefectList() {
    this.defectDashboardService.getParetoDefectList(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == DefectDashboardResult.Success) {

              response.paretoList.forEach(element => {
                element.name = element.defectName;
                element.defectName = element.defectName.length > DefectDashboardParetoNameTrim ?
                  element.defectName.substring(0, DefectDashboardParetoNameTrim) + "..." : element.defectName
              });

              this.masterModel.paretoList = response.paretoList;

              setTimeout(() => {
                this.halfDonutChartDefectPareto('piechart');
              }, 10);
              this.masterModel.paretoFound = true;
            }
            else {
              this.masterModel.paretoList = new Array<ParetoModel>();
              this.masterModel.paretoFound = false;
            }
          }
          this.masterModel.paretoLoading = false;
        },
        error => {
          this.masterModel.paretoLoading = false;
          this.masterModel.paretoError = true;
        });
  }

  //supplier tab click event - get low performance supplier details
  clickSupplier() {
    this.masterModel.defectTabActive = true;
    this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.typeId = SupplierType.Supplier;
    this.defectWordFrame();
    this.defectTitleFrame();
    this.getLowPerformanceDefectList();
  }

  //factory tab click event -  get low performance factory details
  clickFactory() {
    this.masterModel.defectTabActive = false;
    this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.typeId = SupplierType.Factory;
    this.defectWordFrame();
    this.defectTitleFrame();
    this.getLowPerformanceDefectList();
  }

  //defect change type - hide the column based on defect checked values
  defectTypeChange(event) {
    if (event.name == DefectCritical) {
      if (event.checked) {
        this.masterModel.hideDefectCritical = true;
      }
      else {
        this.masterModel.hideDefectCritical = false;
      }
    }

    if (event.name == DefectMinor) {
      if (event.checked) {
        this.masterModel.hideDefectMinor = true;
      }
      else {
        this.masterModel.hideDefectMinor = false;
      }
    }
    if (event.name == DefectMajor) {
      if (event.checked) {
        this.masterModel.hideDefectMajor = true;
      }
      else {
        this.masterModel.hideDefectMajor = false;
      }
    }
    this.getLowPerformanceDefectList();
  }

  //get low performance defect list by supplier id or factory id
  getLowPerformanceDefectList() {
    this.masterModel.lowPerformanceLoading = true;

    this.masterModel.defectPerformanceAnalysis.topPerformanceFilter = this.model;

    this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.defectSelected =
      this.masterModel.defectNameList.filter(x => x.checked).map(x => x.id);

    this.masterModel.lowPerformanceError = false;
    this.defectDashboardService.getLowPerformanceDefectList(this.masterModel.defectPerformanceAnalysis)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {          
          if (response) {
            if (response.result == DefectDashboardResult.Success) {
              this.masterModel.lowPerformanceList = response.performanceDefectList;
              this.masterModel.supplierOrFactoryListCount = response.performanceDefectList.length;

              this.masterModel.hideDefectCritical = this.masterModel.defectNameList.filter(x => x.name == DefectCritical).map(x => x.checked)[0];
              this.masterModel.hideDefectMajor = this.masterModel.defectNameList.filter(x => x.name == DefectMajor).map(x => x.checked)[0];
              this.masterModel.hideDefectMinor = this.masterModel.defectNameList.filter(x => x.name == DefectMinor).map(x => x.checked)[0];
              this.masterModel.lowPerformanceFound = true;
            }
            else {
              this.masterModel.lowPerformanceList = new Array<SupFactDefectAnalysis>();
              this.masterModel.lowPerformanceFound = false;
            }
          }
          this.masterModel.lowPerformanceLoading = false;
        },
        error => {
          this.masterModel.lowPerformanceLoading = false;
          this.masterModel.lowPerformanceError = true;
        });
  }

  //get low performance defect list by supplier id or factory id export
  getLowPerformanceExport() {
    this.masterModel.lowPerformanceExportLoading = true;

    this.defectDashboardService.getLowPerformanceExport(this.masterModel.defectPerformanceAnalysis)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Low_Performance_Defect`_Details.xlsx");
        this.masterModel.lowPerformanceExportLoading = false;
      },
        error => {
          this.masterModel.lowPerformanceExportLoading = false;
        });
  }

  //defect list with count show in graph
  halfDonutChartDefectPareto(container) {
    var data = this.masterModel.paretoList;
    // Create chart instance
    var chart = am4core.create(container, am4charts.PieChart);

    var chartObj = [];
    let totalCount = 0;
    if (data && data.length > 0) {

      for (var i = 0; i < data.length; i++) {
        totalCount += data[i].defectCount;
        chartObj.push({
          "sector": data[i].defectName,
          "size": data[i].defectCount,
          "color": am4core.color(data[i].color)
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

  //get line graph chart defect export
  getDefectLineChartExport() {
    this.masterModel.lineGraphDefectExportLoading = true;

    this.defectDashboardService.getDefectYearCountExport(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Defect_Year_Count_Details.xlsx");
        this.masterModel.lineGraphDefectExportLoading = false;
      },
        error => {
          this.masterModel.lineGraphDefectExportLoading = false;
        });
  }

  //get defect category list export
  getDefectCategoryListChartExport() {
    this.masterModel.defectExportLoading = true;

    this.defectDashboardService.getDefectCategoryListChartExport(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Defect_Category_Details.xlsx");
        this.masterModel.defectExportLoading = false;
      },
        error => {
          this.masterModel.defectExportLoading = false;
        });
  }

  //download the excel file
  async downloadFile(data, mimeType, fileName) {
    const blob = new Blob([data], { type: mimeType });
    let windowNavigator: any = window.navigator;

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

  //get pareto defect list for export
  getParetoDefectListExport() {
    this.masterModel.paretoExportLoading = true;

    this.defectDashboardService.getParetoDefectListExport(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Defect_Pareto_Details.xlsx");
        this.masterModel.paretoExportLoading = false;
      },
        error => {
          this.masterModel.paretoExportLoading = false;
        });
  }

  //get defect country list by defect count for export
  getDefectCountryListExport() {
    this.masterModel.countryDefectExportLoading = true;

    this.defectDashboardService.getCountryDefectListExport(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Defect_Country_Details.xlsx");
        this.masterModel.countryDefectExportLoading = false;
      },
        error => {
          this.masterModel.countryDefectExportLoading = false;
        });
  }

  //get defect list by country
  getCountryDefectList() {
    this.defectDashboardService.getCountryDefectList(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == DefectDashboardResult.Success) {
              // response.data.forEach(element => {
              //   element.defectName = element.defectName.length > DashboardCountryDefectNameTrim ?
              //     element.defectName.substring(0, DashboardCountryDefectNameTrim) + "..." : element.defectName
              // });
              if (response.data.length > 0) {
                response.data.slice(0, this.masterModel.defectCategoryDataLengthTrim).forEach(element => {
                  element.name = element.defectName;
                  element.defectName = element.defectName.length > DashboardCountryDefectNameTrim ?
                    element.defectName.substring(0, DashboardCountryDefectNameTrim) + "..." : element.defectName
                });
              }
              if (response.data.length > this.masterModel.defectCategoryDataLengthTrim) {
                response.data.slice(this.masterModel.defectCategoryDataLengthTrim, response.data.length).forEach(element => {
                  element.name = element.defectName;
                  element.defectName = element.defectName.length > DashboardDefectNamecPopupTrim ?
                    element.defectName.substring(0, DashboardDefectNamecPopupTrim) + "..." : element.defectName
                });
              }
              this.masterModel.countryDefectList = response.data;
              this.masterModel.countryListXAxis = response.countryList;
              this.countryDataFrame();

              this.masterModel.countryDefectFound = true;
            }
            else {
              this.masterModel.countryDefectList = new Array<CountryDefectModel>();
              this.masterModel.countryDefectFound = false;
            }
          }
          this.masterModel.countryDefectLoading = false;
          this.masterModel.searchLoading = false;
        },
        error => {
          this.masterModel.countryDefectLoading = false;
          this.masterModel.countryDefectError = true;
        });
  }

  //photo list for critical, major, minor
  getDefectPhotoList(supOrFactId: number, imagepopup, defectId: number) {

    this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.supOrFactId = supOrFactId;
    this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.defectSelect = defectId;
    this.masterModel.defectPhotoLoading = true;
    this.masterModel.defectCountLoading = true;
    this.defectDashboardService.getLowPerformanceDefectPhotoList(this.masterModel.defectPerformanceAnalysis)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == DefectDashboardResult.Success) {
              this.masterModel.defectPhotoList = response.performanceDefectList;
              this.getPreviewDefectImage(this.masterModel.defectPhotoList, imagepopup);
            }
            else {
              this.masterModel.defectPhotoList = new Array<DefectPhotoList>();
              this.showInfo('DEFECT_DASHBOARD.LBL_TITLE', 'DEFECT_DASHBOARD.LBL_NO_PHOTOS');
            }
          }
          this.masterModel.defectPhotoLoading = false;
          this.masterModel.defectCountLoading = false;
        },
        error => {
          this.masterModel.defectPhotoLoading = false;
          this.masterModel.defectCountLoading = false;
        });
  }

  //get defect name with count by supplier id or factory id
  getDefectCountList(supOrFactId: number, defectpopup, defectId: number) {

    this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.supOrFactId = supOrFactId;
    this.masterModel.defectPerformanceAnalysis.innerPerformanceFilter.defectSelect = defectId;
    this.masterModel.defectCountLoading = true;

    this.defectDashboardService.getLowPerformanceDefectCountList(this.masterModel.defectPerformanceAnalysis)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == DefectDashboardResult.Success) {
              this.modalService.open(defectpopup, { windowClass: "mdModelWidth", backdrop: 'static', centered: true });

              this.masterModel.defectCountList = response.performanceDefectList;

            }
            else {
              this.masterModel.defectCountList = new Array<DefectCountList>();
              this.showInfo('DEFECT_DASHBOARD.LBL_TITLE', 'DEFECT_DASHBOARD.LBL_NO_DEFECT_COUNT');

            }
          }
          this.masterModel.defectCountLoading = false;
        },
        error => {
          this.masterModel.defectCountLoading = false;
        });
  }

  //photo show in popup using gallery
  getPreviewDefectImage(imageList: Array<DefectPhotoList>, modalcontent) {
    this.masterModel.defectImages = [];
    imageList.forEach(url => {
      this.masterModel.defectImages.push(
        {
          small: url.defectPhotoPath,
          medium: url.defectPhotoPath,
          big: url.defectPhotoPath,
          description: url.description
        });
    });

    this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  //frame the defect by country chart
  countryDataFrame() {

    //building below structure
    // {
    // 	"month": "Jan",
    // 	"europe": 2.5,
    // 	"namerica": 2.5,
    // 	"asia": 2.1,
    // 	"lamerica": 1.2,
    // 	"meast": 5.2,
    // 	"africa": 0.1
    // }
    if (this.masterModel.countryListXAxis && this.masterModel.countryListXAxis.length > 0 &&
      this.masterModel.countryDefectList && this.masterModel.countryDefectList.length > 0) {

      var chartObj = [];

      for (var i = 0; i < this.masterModel.countryListXAxis.length; i++) {

        var subchartObj = {};

        subchartObj["month"] = this.masterModel.countryListXAxis[i].countryName;

        for (var j = 0; j < this.masterModel.countryDefectList.length; j++) {

          var countryDefect = this.masterModel.countryDefectList[j].countryDefectData.filter(x => x.countryId ==
            (this.masterModel.countryListXAxis[i].countryId));

          if (countryDefect.length > 0) {

            const sum = countryDefect.reduce((sum, current) => sum + current.count, 0);

            subchartObj[this.masterModel.countryDefectList[j].defectName.toLowerCase()] = sum;
          }
          else {
            subchartObj[this.masterModel.countryDefectList[j].defectName.toLowerCase()] = 0;
          }
        }
        chartObj.push(subchartObj);
      }

      setTimeout(() => {
        this.renderCountryChart(chartObj);
      }, 300);
    }
  }

  /**
  * Function to render country chart */
  renderCountryChart(chartObj) {



    // Create chart instance
    let chart = am4core.create("singlineChart", am4charts.XYChart);

    // Add data
    chart.data = chartObj;

    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "month";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.cellStartLocation = 0.1;
    categoryAxis.renderer.cellEndLocation = 0.9;
    categoryAxis.renderer.labels.template.fontSize = 8;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.max = this.masterModel.countryDefectList.reduce((sum, currentItem) => sum + currentItem.count, 0);
    valueAxis.renderer.labels.template.fontSize = 8;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
    let datasize = this.masterModel.countryListXAxis ? this.masterModel.countryListXAxis.length : 0;

    for (var i = 0; i < this.masterModel.countryDefectList.length; i++) {
      let stacked = true;
      //create series
      this.createSeries(chart, this.masterModel.countryDefectList[i].color, this.masterModel.countryDefectList[i].defectName.toLowerCase(), this.masterModel.countryDefectList[i].defectName, stacked, datasize);
    }

    chart.padding(20, 0, 20, 0);
  }

  // Create series
  createSeries(chart, color, field, name, stacked, datasize: number) {
    var graphwidth = datasize >= 5 ? 95 : datasize >= 3 ? 60 : 30;
    let series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = field;
    series.dataFields.categoryX = "month";
    series.name = name;
    series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]";
    series.stacked = stacked;
    series.columns.template.width = am4core.percent(graphwidth);
    series.columns.template.fill = am4core.color(color);
  }

  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }
  //download the report
  openReport(reportData) {
    if (reportData && reportData.finalManualReportLink)
      window.open(reportData.finalManualReportLink);
    else if (reportData && reportData.reportLink)
      window.open(reportData.reportLink);
  }
  changeSupplierType(item) {
    this.masterModel.supplierLoading = true;
    this.masterModel.supplierList = [];
    if (this.currentUser.usertype != UserType.Supplier)
      this.model.supplierId = null;
    if (this.currentUser.usertype != UserType.Factory)
      this.model.factoryIds = [];
    this.masterModel.factoryList = [];
    if (item.id == SearchType.SupplierCode) {
      this.masterModel.supplierRequest.supSearchTypeId = SearchType.SupplierCode;
    }
    else if (item.id == SearchType.SupplierName) {
      this.masterModel.supplierRequest.supSearchTypeId = SearchType.SupplierName;
    }
    this.getSupplierData();
  }

  //make Defect Landing Request
  getDefectLandingRequest() {
    const groupByFilter = GroupByFilter.filter(x => x.id != GroupByFilterEnum.Brand);
    groupByFilter.forEach(element => {
      this.defectLandingRequest.groupByFilter.push(element.id);
    });
    this.defectLandingRequest.fromDate = this.model.fromDate;
    this.defectLandingRequest.toDate = this.model.toDate;
    this.defectLandingRequest.customerId = this.model.customerId;
    this.defectLandingRequest.supplierTypeId = this.model.supplierTypeId;
    this.defectLandingRequest.supplierId = this.model.supplierId;
    this.defectLandingRequest.factoryIds = this.model.factoryIds;
    this.defectLandingRequest.selectedBrandIdList = this.model.selectedBrandIdList;
    this.defectLandingRequest.selectedProdCategoryIdList = this.model.selectedProdCategoryIdList;
    this.defectLandingRequest.factoryCountryIds = this.model.factoryCountryIds;
    this.defectLandingRequest.searchtypeid = SearchType.BookingNo;
    this.defectLandingRequest.pageSize = 10;
    this.defectLandingRequest.index = 0;
  }

  navigateDefectPareto() {
    this.getDefectLandingRequest();
    const path = "defectpareto/defectpareto/";
    const entity: string = this.utility.getEntityName();
    const basedUrl = window.location.href.replace(this.router.url,'');
    const url = this.router.createUrlTree([`/${entity}/${path}`], {
      queryParams: { param: encodeURI(JSON.stringify(this.defectLandingRequest)) }
    })
    window.open(basedUrl + url);
  }
}

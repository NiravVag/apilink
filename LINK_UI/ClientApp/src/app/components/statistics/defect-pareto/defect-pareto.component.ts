import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { NgbCalendar, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, CustomerCommonDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { DefectDashboardModel, DefectMasterModel, FactoryDataSourceRequest } from 'src/app/_Models/statistics/defect-dashboard.model';
import { DetailComponent } from '../../common/detail.component';
import { amCoreLicense, inspectionSearchlst, DefectDashboardCountryTextCount, DefectDashboardSupplierNameTrim, GroupByFilter, ListSize, MobileViewFilterCount, PageSizeCommon, SearchType, Service, SupplierType, UserType, supplierTypeList } from '../../common/static-data-common';
import { Validator } from '../../common/validator';
import * as am4core from "@amcharts/amcharts4/core";
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { of, Subject } from 'rxjs';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { GroupByFilterEnum } from 'src/app/_Models/statistics/rejectiondashborad.model';
import { DefectDashboardService } from 'src/app/_Services/statistics/defect-dashboard.service';
import { SummaryComponent } from '../../common/summary.component';

@Component({
  selector: 'app-defect-pareto',
  templateUrl: './defect-pareto.component.html',
  styleUrls: ['./defect-pareto.component.scss'],
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
export class DefectParetoComponent extends SummaryComponent<DefectDashboardModel> {
  componentDestroyed$: Subject<boolean> = new Subject();
  isFilterOpen: boolean;
  model: DefectDashboardModel;
  masterModel: DefectMasterModel;
  toggleFormSection: boolean;
  requestCustomerModel: CustomerCommonDataSourceRequest;
  currentUser: UserModel;
  LoadFirstTime: boolean;
  userTypeEnum = UserType;
  selectedPageSize;
  pagesizeitems = PageSizeCommon;
  bookingSearchtypelst: any = inspectionSearchlst;
  _booksearttypeid = SearchType.BookingNo;
  _customvalidationforbookid: boolean = false;
  supplierTypeList: any = supplierTypeList;
  constructor(router: Router, route: ActivatedRoute, public validator: Validator, translate: TranslateService, toastr: ToastrService,
    public calendar: NgbCalendar, private authService: AuthenticationService, public cusService: CustomerService, public pathroute: ActivatedRoute,
    public supService: SupplierService, private brandService: CustomerBrandService, private productManagementService: ProductManagementService,
    public locationService: LocationService, public referenceService: ReferenceService, public defectDashboardService: DefectDashboardService) {
    super(router, validator, route, translate, toastr);

    am4core.addLicense(amCoreLicense);
    this.currentUser = authService.getCurrentUser();
    this.model = new DefectDashboardModel();
    this.masterModel = new DefectMasterModel();

    this.validator.setJSON("statistics/defect-pareto/defect-pareto.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.isFilterOpen = true;
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.LoadFirstTime = true;
    this.initialize();
    this.getIsMobile();
    this.setDefaultFilter();
    this.groupByFilter();
    this.isShownColumn();
    this.defaultGroupBy();
    this.getCustomerListBySearch();
    this.selectedPageSize = PageSizeCommon[0];
    this.model.pageSize = 10;
    this.model.index = 0;
    this.model.searchtypeid = SearchType.BookingNo;
    if (this.currentUser.usertype == UserType.Supplier) {
      this.model.supplierId = this.currentUser.supplierid;
      this.getSupListBySearch();
    }

    if (this.currentUser.usertype == UserType.Factory) {
      this.getSupListBySearch();
      this.model.factoryIds.push(this.currentUser.factoryid);
      this.getFactListBySearch();
    }

    this.getCountryListBySearch();

    this.DefectDateChange();

    this.getProductCategoryData();
    //assign the selected parent data if it comes from the defect dashboard page
    this.assignParentData();
  }

  defaultGroupBy() {
    let data = this.masterModel.groupByFilter.filter(x=>x.id != GroupByFilterEnum.Brand);
    data.forEach(element => {
      this.model.groupByFilter.push(element.id);
    });
  }

  isShownColumn() {
    this.masterModel.isShowColumn = this.masterModel.isShowColumn == true ?
      false : true;
    if (!this.masterModel.isShowColumn) {
      this.masterModel.isShowColumnImagePath = "assets/images/new-set/table-expand.svg";
      this.masterModel.showColumnTooltip = "Expand";
    }
    else {
      this.masterModel.isShowColumnImagePath = "assets/images/new-set/table-collapsea.svg";
      this.masterModel.showColumnTooltip = "Collapse";
    }
  }

  groupByFilter() {
    this.masterModel.groupByFilter = GroupByFilter;
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

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
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

  getData(): void {
    this.getSearchData();
  }

  getPathDetails(): string {
    return "";
  }

  //search data by filters
  getSearchData() {
    this.model.noFound = false;
    this.resetGroupByFilter();
    this.validator.initTost();
    this.validator.isSubmitted = true;
    this.DefectDateChange(); // added by nixon . if date range
    if (this.isFormValid()) {
      this.applyGroupByFilter();
      this.getReportDefectPareto(this.model);
    }
  }

  getReportDefectPareto(request: DefectDashboardModel) {
    this.masterModel.searchLoading = true;
    this.defectDashboardService.getReportDefectPareto(request)
      .subscribe(
        response => {
          if (response != null && response.result == ResponseResult.Success) {
            this.mapPageProperties(response);
            this.model.items = response.data;
            this.masterModel.searchLoading = false;
            if (this.model.items.length == 0)
              this.model.noFound = true;
          }
          else {
            this.model.noFound = true;
            this.masterModel.searchLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.searchLoading = false;
        });
  }

  getDefectPercentage(item, defect) {
    return defect.defectCount + " " + "(" + parseFloat(((defect.defectCount / item.totalDefectCount) * 100).toFixed(2)).toString() + " %)";
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

  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchtypetext != null &&
      this.model.searchtypetext.trim() == "" ? true : false;

    if (this.model.searchtypetext == null || this.model.searchtypetext == "") {
      if (!this.model.fromDate)
        this.validator.isValid('fromDate');

      else if (this.model.fromDate && !this.model.toDate)
        this.validator.isValid('toDate');
    }
    return isOk;
  }

  //form validation
  isFormValid(): boolean {
    let isOk = !this._customvalidationforbookid && this.validator.isValidIf('fromdate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('toDate', this.IsDateValidationRequired())
    if (this.model.searchtypetext == null || this.model.searchtypetext == "")
      isOk = this.validator.isValidIf('customerId', true) && this.validator.isValid("groupByFilter");
    if (this.model.searchtypetext && this.model.searchtypetext != "" && this.model.searchtypeid != 1)
      isOk = this.validator.isValidIf('customerId', true) && this.validator.isValid("groupByFilter");
    if (this.model.searchtypetext && this.model.searchtypetext != "" && this.model.searchtypeid == 1)
      isOk = this.validator.isValid("groupByFilter");
    return isOk;
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

  //reset the page
  reset() {
    this.onInit();
  }

  initialize() {
    this.model = new DefectDashboardModel();
    this.masterModel = new DefectMasterModel();
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();
    this.model.supplierTypeId = SearchType.SupplierName;
    this.masterModel.supplierRequest.supSearchTypeId = SearchType.SupplierName;
    this.masterModel.selectedNumber = this.bookingSearchtypelst.find(x => x.id == 1) ?
      this.bookingSearchtypelst.find(x => x.id == 1).shortName : "";

    this.masterModel.selectedNumberPlaceHolder = this.bookingSearchtypelst.find(x => x.id == 1) ?
      "Enter " + this.bookingSearchtypelst.find(x => x.id == 1).name : "";
  }

  getCustomerListBySearch() {
    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.cusService.getCustomerListByUserType(this.requestCustomerModel, null, term)
        : this.cusService.getCustomerListByUserType(this.requestCustomerModel)
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
          this.getBrandListBySearch();
          this.getServiceTypelist();
          if (this.currentUser.usertype == UserType.InternalUser || this.currentUser.usertype == UserType.Customer)
            this.getSupListBySearch();

          if (this.currentUser.usertype == UserType.Supplier || this.model.supplierId > 0)
            this.getFactListBySearch();

          this.LoadFirstTime = false;
        }
        this.masterModel.customerLoading = false;
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

  //supplier change event get supplier selected name shown in strip
  supplierChange(supplierItem) {
    if (supplierItem && supplierItem.id > 0) {
      var supplierDetails = this.masterModel.supplierList.find(x => x.id == supplierItem.id);
      if (supplierDetails)
        this.masterModel.supplierName =
          supplierDetails.name.length > DefectDashboardSupplierNameTrim ?
            supplierDetails.name.substring(0, DefectDashboardSupplierNameTrim) + "..." : supplierDetails.name;

      this.getFactListBySearch();
    }
    else {
      this.masterModel.supplierName = "";
    }
  }

  //fetch the first 10 fact for the supplier on load
  getFactListBySearch() {
    this.masterModel.factoryList = null;
    this.masterModel.factoryRequest.supplierType = SupplierType.Factory;
    this.masterModel.factoryRequest.supplierId = this.model.supplierId;

    //add the customer filter for factory data
    this.masterModel.factoryRequest.customerId = this.model.customerId;

    //add factory id filter
    if (this.currentUser.usertype == UserType.Factory)
      this.masterModel.factoryRequest.id = this.currentUser.factoryid;

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
      });
  }

  //toggle advance search section
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  //search filters shown
  toggleFilter() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  //set default filter data
  setDefaultFilter() {
    this.model.fromDate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.model.toDate = this.calendar.getToday();
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.requestCustomerModel.searchText = this.masterModel.customerInput.getValue();
      this.requestCustomerModel.skip = this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.cusService.getCustomerListByUserType(this.requestCustomerModel).
    pipe(takeUntil(this.componentDestroyed$), first()).  
    subscribe(customerData => {
      if (customerData && customerData.length > 0) {
        this.masterModel.customerList = this.masterModel.customerList.concat(customerData);
      }
        if (IsVirtual) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
        }
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
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

  //fetch the brand data with virtual scroll
  getBrandData() {
    this.masterModel.brandSearchRequest.searchText = this.masterModel.brandInput.getValue();
    this.masterModel.brandSearchRequest.skip = this.masterModel.brandList.length;

    //add the customer filter for brand data
    this.masterModel.brandSearchRequest.customerId = this.model.customerId;

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

  changeCustomerData(item) {
    if (item && item.id > 0) {
      //clear the list
      this.masterModel.supplierList = null;
      this.masterModel.factoryList = null;
      this.masterModel.brandList = null;
      this.masterModel.serviceTypeList = null;

      if (this.currentUser.usertype != UserType.Supplier)
        this.model.supplierId = null;
      if (this.currentUser.usertype != UserType.Factory)
        this.model.factoryIds = [];
      this.model.selectedBrandIdList = [];
      this.model.serviceTypelst = [];

      this.masterModel.supplierRequest.customerId = item.id;
      this.model.supplierTypeId = SearchType.SupplierName;
      this.masterModel.supplierRequest.supSearchTypeId = SearchType.SupplierName;
      this.getSupListBySearch();
      this.getFactListBySearch();
      this.getBrandListBySearch();
      this.getServiceTypelist();
    }
  }

  getServiceTypelist() {
    this.masterModel.serviceTypeLoading = true;
    let request = this.generateServiceTypeRequest();
    request.customerId = this.model.customerId;
    this.referenceService.getServiceTypes(request)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.masterModel.serviceTypeList = response.serviceTypeList;
          this.masterModel.serviceTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.serviceTypeList = [];
          this.masterModel.serviceTypeLoading = false;
        }
      );
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerId ?? 0;
    serviceTypeRequest.serviceId = Service.Inspection;
    return serviceTypeRequest;
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.masterModel.brandList = null;
    this.masterModel.brandSearchRequest.customerId = this.model.customerId;
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

  //fetch the facotry data with virtual scroll
  getFactoryData() {
    this.masterModel.factoryRequest.searchText = this.masterModel.factoryInput.getValue();
    this.masterModel.factoryRequest.skip = this.masterModel.factoryList.length;

    this.masterModel.factoryRequest.supplierType = SupplierType.Factory;
    this.masterModel.factoryLoading = true;
    this.masterModel.factoryRequest.supplierId = this.model.supplierId;

    //add the customer filter for factory data
    this.masterModel.factoryRequest.customerId = this.model.customerId;

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

  //clear selected supplier based on cascading data also to clear
  clearSupplier() {
    if (this.currentUser.usertype != UserType.Factory)
      this.model.factoryIds = [];
    this.masterModel.factoryList = [];
    this.masterModel.supplierRequest.idList = [];
    this.getSupplierData();
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

  resetGroupByFilter() {
    this.masterModel.factoryCountryGroup = false;
    this.masterModel.supplierGroup = false;
    this.masterModel.factoryGroup = false;
    this.masterModel.brandGroup = false;
  }

  applyGroupByFilter() {
    if (this.model.groupByFilter != null) {
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.FactoryCountry))
        this.masterModel.factoryCountryGroup = true;
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Supplier))
        this.masterModel.supplierGroup = true;
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Factory))
        this.masterModel.factoryGroup = true;
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Brand))
        this.masterModel.brandGroup = true;
    }
  }

  getSearchDetails() {
    if (this.model.searchtypetext && this.model.searchtypetext != '') {
      if (!this.isFilterOpen)
        this.isFilterOpen = this.model.searchtypeid != this._booksearttypeid && !(this.model.customerId > 0);
      this.SearchDetails();
    }
  }

  SetSearchTypemodel(item) {
    this.model.searchtypeid = item.id;
    this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
    this.masterModel.selectedNumber = item.shortName;

    if (item.id == SearchType.BookingNo) {
      this.masterModel.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.PoNo) {
      this.masterModel.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.CustomerBookingNo) {
      this.masterModel.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.ProductId) {
      this.masterModel.selectedNumberPlaceHolder = "Enter " + item.name;
    }
  }

  BookingNoValidation(bookingText) {
    this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && bookingText && bookingText.trim() != "" && ((isNaN(Number(bookingText))) || (bookingText.trim().length > 9));

    this.masterModel.isShowSearchLens = bookingText && bookingText.trim() != "";
  }

  clearCustomer() {
    this.masterModel.supplierList = null;
    this.masterModel.factoryList = null;
    this.masterModel.brandList = null;
    this.masterModel.serviceTypeList = null;
    if (this.currentUser.usertype != UserType.Supplier)
      this.model.supplierId = null;
    if (this.currentUser.usertype != UserType.Factory)
      this.model.factoryIds = null;
    this.model.selectedBrandIdList = null;
    this.model.serviceTypelst = null;
    this.requestCustomerModel.idList = null;
    this.getCustomerListBySearch();
  }

  exportReportDefectPareto() {
    this.masterModel.exportDataLoading = true;
    this.defectDashboardService.exportReportDefectPareto(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DefectPareto.xlsx");
      },
        error => {
          this.setError(error);
          this.masterModel.exportDataLoading = false;
        });
  }

  //download the excel file
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
    this.masterModel.exportDataLoading = false;
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

  assignParentData() {
    if (this.pathroute.snapshot.queryParams && this.pathroute.snapshot.queryParams.param) {
      let parentModel = JSON.parse(decodeURI(this.pathroute.snapshot.queryParams.param))
      if (parentModel) {
        if (parentModel.customerId)
          this.requestCustomerModel.idList.push(parentModel.customerId);

        if (parentModel.supplierTypeId > 0)
          this.masterModel.supplierRequest.supSearchTypeId = parentModel.supplierTypeId;

        if (parentModel.supplierId > 0)
          this.masterModel.supplierRequest.idList.push(parentModel.supplierId);

        if (parentModel.factoryIds && parentModel.factoryIds.length > 0)
          this.masterModel.factoryRequest.idList = parentModel.factoryIds;

        if (parentModel.factoryCountryIds && parentModel.factoryCountryIds.length > 0)
          this.masterModel.countryRequest.countryIds = parentModel.factoryCountryIds;

        if (parentModel.selectedBrandIdList && parentModel.selectedBrandIdList.length > 0)
          this.masterModel.brandSearchRequest.idList = parentModel.selectedBrandIdList;
      }
    }
  }

  clearFactory() {
    this.masterModel.factoryRequest = new FactoryDataSourceRequest();
    this.masterModel.factoryList = [];
    this.getFactoryData();
  }

  clearBrand() {
    this.masterModel.brandSearchRequest = new CommonCustomerSourceRequest();
    this.masterModel.brandList = [];
    this.getBrandData();
  }

  clearCountry() {
    this.masterModel.countryRequest = new CountryDataSourceRequest();
    this.masterModel.countryList = [];
    this.getCountryData();
  }
}

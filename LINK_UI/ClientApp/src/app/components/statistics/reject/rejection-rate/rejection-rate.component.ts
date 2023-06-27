import { Component, OnDestroy } from '@angular/core';
import * as am4core from "@amcharts/amcharts4/core";
import { trigger, state, style, transition, animate } from '@angular/animations';
import { GroupByFilterEnum, RejectionDashboardModel, RejectionDashboardRequest, RejectionRateGroupList, RejectionRateList, RejectionRateSearchRequest, ReportDecisionNameList, ReportResultNameList } from 'src/app/_Models/statistics/rejectiondashborad.model';
import { DetailComponent } from 'src/app/components/common/detail.component';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { NgbCalendar, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { amCoreLicense, ListSize, MobileViewFilterCount, SupplierType, UserType, fbReportResultList, FbReportResultType, SearchType, Service, GroupByFilter, PageSizeCommon, inspectionSearchlst, supplierTypeList, DefectDashboardSupplierNameTrim } from 'src/app/components/common/static-data-common';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, CustomerCommonDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { Validator } from 'src/app/components/common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { FactoryDataSourceRequest } from 'src/app/_Models/statistics/defect-dashboard.model';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { RejectionDashBoardService } from 'src/app/_Services/statistics/rejectiondashboard.service';
import { SummaryComponent } from 'src/app/components/common/summary.component';


@Component({
  selector: 'app-rejection-rate',
  templateUrl: './rejection-rate.component.html',
  styleUrls: ['./rejection-rate.component.scss'],
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
export class RejectionRateComponent extends SummaryComponent<RejectionDashboardRequest> {
  componentDestroyed$: Subject<boolean> = new Subject();
  masterModel: RejectionDashboardModel;
  model: RejectionDashboardRequest;
  toggleFormSection: boolean;
  requestCustomerModel: CustomerCommonDataSourceRequest;
  currentUser: UserModel;
  isFilterOpen: boolean;
  LoadFirstTime: boolean = true;
  FBReportResultList = fbReportResultList;
  userTypeEnum = UserType;
  rejectionRateGroupList: RejectionRateGroupList[];
  reportDecisionNameList: ReportDecisionNameList[];
  reportResultNameList: ReportResultNameList[];
  resultDataList: RejectionRateList;
  entityName: string;
  factoryCountryGroup: boolean;
  supplierGroup: boolean;
  factoryGroup: boolean;
  brandGroup: boolean;
  selectedPageSize;
  pagesizeitems = PageSizeCommon;
  bookingSearchtypelst: any = inspectionSearchlst;
  _booksearttypeid = SearchType.BookingNo;
  _customvalidationforbookid: boolean = false;
  supplierTypeList: any = supplierTypeList;

  constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    private calendar: NgbCalendar, private locationService: LocationService, private brandService: CustomerBrandService,
    private supService: SupplierService, private cusService: CustomerService, private rejectionDashBoardService: RejectionDashBoardService,
    public validator: Validator, public utility: UtilityService, authserve: AuthenticationService, public pathroute: ActivatedRoute,
    private productManagementService: ProductManagementService, public referenceService: ReferenceService,
    public modalService: NgbModal) {

    super(router, validator, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    am4core.addLicense(amCoreLicense);
    this.isFilterOpen = true;
  }

  onInit() {
    this.initialize();
    this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    this.model.supplierTypeId = SearchType.SupplierName;
    this.validator.setJSON("statistics/reject/rejection-rate/rejection-rate.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.selectedPageSize = PageSizeCommon[0];
    this.masterModel.fbReportResultId = FbReportResultType.Fail;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.pageSize = 10;
    this.model.index = 0;
    this.entityName = this.utility.getEntityName();
    this.isShownColumn();
    this.dateSelectionDefault();
    this.groupByFilter();
    this.defaultGroupBy();

    if (this.currentUser.usertype == UserType.Supplier) {
      this.model.supplierId = this.currentUser.supplierid;
      this.getSupListBySearch();
    }

    if (this.currentUser.usertype == UserType.Factory) {
      this.getSupListBySearch();
      this.model.selectedFactoryIdList.push(this.currentUser.factoryid);
      this.getFactListBySearch();
    }

    this.getCustomerListBySearch();
    this.getCountryListBySearch();
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

  getData(): void {
    this.getSearchData();
  }

  getPathDetails(): string {
    return "";
  }

  groupByFilter() {
    this.masterModel.groupByFilter = GroupByFilter;
  }

  initialize() {
    this.masterModel = new RejectionDashboardModel();
    this.model = new RejectionDashboardRequest();
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();

    this.masterModel.selectedNumber = this.bookingSearchtypelst.find(x => x.id == 1) ?
      this.bookingSearchtypelst.find(x => x.id == 1).shortName : "";

    this.masterModel.selectedNumberPlaceHolder = this.bookingSearchtypelst.find(x => x.id == 1) ?
      "Enter " + this.bookingSearchtypelst.find(x => x.id == 1).name : "";
    this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    this.model.supplierTypeId = SearchType.SupplierName;
  }

  dateSelectionDefault() {
    this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.model.serviceDateTo = this.calendar.getToday();
  }

  //fetch the country data with virtual scroll
  getCountryData() {
    this.masterModel.countryRequest.searchText = this.masterModel.countryInput.getValue();
    this.masterModel.countryRequest.skip = this.masterModel.countryList.length;

    this.masterModel.countryLoading = true;

    this.locationService.getCountryDataSourceList(this.masterModel.countryRequest).
      subscribe(CountryData => {
        if (CountryData && CountryData.length > 0) {
          this.masterModel.countryList = this.masterModel.countryList.concat(CountryData);
        }
        this.masterModel.countryRequest = new CountryDataSourceRequest();
        this.masterModel.countryLoading = false;
      }),
      (error: any) => {
        this.masterModel.countryLoading = false;
      };
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

  //get product category list
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
        this.setError(error);
        this.masterModel.productCategoryListLoading = false;
      };
  }

  GetServiceTypelist() {
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
          this.masterModel.brandSearchRequest.customerId = this.model.customerId;

          if (this.currentUser.usertype == UserType.InternalUser || this.currentUser.usertype == UserType.Customer)
            this.getSupListBySearch();

          this.getFactListBySearch();
          this.getBrandListBySearch();
          this.GetServiceTypelist();
          this.LoadFirstTime = false;
        }

        if (this.currentUser.usertype == UserType.Supplier || this.currentUser.usertype == UserType.Factory)
          this.model.customerId = this.masterModel.customerList[0].id;
        this.masterModel.customerLoading = false;
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

  changeCustomerData(item) {
    if (item && item.id > 0) {
      //clear the list
      this.masterModel.supplierList = null;
      this.masterModel.factoryList = null;
      this.masterModel.brandList = null;
      this.masterModel.serviceTypeList = null;

      //clear the selected values
      if (this.currentUser.usertype != UserType.Supplier)
        this.model.supplierId = null;
      if (this.currentUser.usertype != UserType.Factory)
        this.model.selectedFactoryIdList = null;
      this.model.selectedBrandIdList = null;
      this.model.selectedServiceTypeIdList = null;

      this.masterModel.brandSearchRequest.customerId = item.id;
      this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
      this.model.supplierTypeId = SearchType.SupplierName;
      this.getSupListBySearch();
      this.getFactListBySearch();
      this.getBrandListBySearch();
      this.GetServiceTypelist();
    }
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.masterModel.supsearchRequest.customerId = this.model.customerId;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;

    //add supplier id filter
    if (this.currentUser.usertype == UserType.Supplier)
      this.masterModel.supsearchRequest.id = this.currentUser.supplierid;

    //get supplier by factory data
    if (this.currentUser.usertype == UserType.Factory)
      this.masterModel.supsearchRequest.factoryId = this.currentUser.factoryid;

    this.masterModel.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.supLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.supLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.supplierList = data;
        this.masterModel.supLoading = false;
        if (this.currentUser.usertype == UserType.Supplier)
          this.getFactListBySearch();
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.masterModel.supsearchRequest.searchText = this.masterModel.supInput.getValue();
    this.masterModel.supsearchRequest.skip = this.masterModel.supplierList.length;

    this.masterModel.supsearchRequest.customerId = this.model.customerId;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;
    //get the supplier by factory id
    if (this.currentUser.usertype == UserType.Factory)
      this.masterModel.supsearchRequest.factoryId = this.currentUser.factoryid;

    this.masterModel.supLoading = true;
    this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.supplierList = this.masterModel.supplierList.concat(customerData);
        }
        this.masterModel.supsearchRequest.skip = 0;
        this.masterModel.supsearchRequest.take = ListSize;
        this.masterModel.supLoading = false;
      }),
      (error: any) => {
        this.masterModel.supLoading = false;
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

  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchtypetext != null &&
      this.model.searchtypetext.trim() == "" ? true : false;

    if (this.model.searchtypetext == null || this.model.searchtypetext == "") {
      if (!this.model.serviceDateFrom)
        this.validator.isValid('serviceDateFrom');

      else if (this.model.serviceDateFrom && !this.model.serviceDateTo)
        this.validator.isValid('serviceDateTo');
    }
    return isOk;
  }

  //form validation
  isFormValid(): boolean {
    let isOk = !this._customvalidationforbookid && this.validator.isValidIf('serviceDateFrom', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('serviceDateTo', this.IsDateValidationRequired())
    if (this.model.searchtypetext == null || this.model.searchtypetext == "") {
      isOk = this.validator.isValidIf('customerId', true) && this.validator.isValid("groupByFilter");
    }
    if (this.model.searchtypetext && this.model.searchtypetext != "" && this.model.searchtypeid != 1) {
      isOk = this.validator.isValidIf('customerId', true) && this.validator.isValid("groupByFilter");
    }
    if (this.model.searchtypetext && this.model.searchtypetext != "" && this.model.searchtypeid == 1) {
      isOk = this.validator.isValid("groupByFilter");
    }
    return isOk;
  }

  getSearchData() {
    this.model.noFound = false;
    this.model.isExport = false;
    this.resetGroupByFilter();
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.applyGroupByFilter();
      this.getRejectionRate(this.model);
    }
  }

  resetGroupByFilter() {
    this.factoryCountryGroup = false;
    this.supplierGroup = false;
    this.factoryGroup = false;
    this.brandGroup = false;
  }

  clearCustomer() {
    //clear the list
    this.masterModel.supplierList = null;
    this.masterModel.factoryList = null;
    this.masterModel.brandList = null;
    this.masterModel.serviceTypeList = null;

    //clear the selected values
    if (this.currentUser.usertype != UserType.Supplier)
      this.model.supplierId = null;
    if (this.currentUser.usertype != UserType.Factory)
      this.model.selectedFactoryIdList = null;
    this.model.selectedBrandIdList = null;
    this.model.selectedServiceTypeIdList = null;
    this.requestCustomerModel.idList = null;
    this.getCustomerListBySearch();
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  applyGroupByFilter() {
    if (this.model.groupByFilter != null) {
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.FactoryCountry))
        this.factoryCountryGroup = true;
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Supplier))
        this.supplierGroup = true;
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Factory))
        this.factoryGroup = true;
      if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Brand))
        this.brandGroup = true;
    }
  }

  exportRejectionRate() {
    this.masterModel.exportDataLoading = true;
    this.model.isExport = true;
    this.rejectionDashBoardService.exportReportRejectionRate(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RejectionRate.xlsx");
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

  getRejectionRate(request: RejectionDashboardRequest) {
    this.masterModel.searchLoading = true;
    this.rejectionDashBoardService.getReportRejectionRate(request)
      .subscribe(
        response => {
          if (response != null && response.result == ResponseResult.Success) {
            this.mapPageProperties(response);
            this.resultDataList = response.data.resultDataList;
            this.rejectionRateGroupList = response.data.rejectionRateGroupList;
            this.reportResultNameList = response.data.reportResultNameList;
            this.reportDecisionNameList = response.data.reportDecisionNameList;
            this.masterModel.searchLoading = false;
            if (this.rejectionRateGroupList.length == 0)
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


  GetResultCount(item: RejectionRateGroupList, resultId) {
    let data = this.resultDataList?.rejectionRateReportResultLists?.filter(x => x.resultId == resultId);

    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.FactoryCountry)) {
      data = data.filter(x => x.factoryCountryId == item.factoryCountryId)
    }
    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Supplier)) {
      data = data.filter(x => x.supplierId == item.supplierId)
    }
    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Brand)) {
      data = data.filter(x => x.brandId == item.brandId)
    }
    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Factory)) {
      data = data.filter(x => x.factoryId == item.factoryId)
    }

    if (data == undefined || data.length == 0)
      return 0;

    return data[0].totalCount + " " + "(" + parseFloat(((data[0].totalCount / item.totalCount) * 100).toFixed(2)).toString() + " %)";
  }

  GetCustomDecisionCount(item: RejectionRateGroupList, customerResultId) {
    let data = this.resultDataList.rejectionRateDecisionLists;

    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.FactoryCountry)) {
      data = data.filter(x => x.factoryCountryId == item.factoryCountryId)
    }
    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Supplier)) {
      data = data.filter(x => x.supplierId == item.supplierId)
    }
    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Brand)) {
      data = data.filter(x => x.brandId == item.brandId)
    }
    if (this.model.groupByFilter.find(x => x == GroupByFilterEnum.Factory)) {
      data = data.filter(x => x.factoryId == item.factoryId)
    }
    const totalCount = data.reduce((accumulator, obj) => {
      return accumulator + obj.totalDecisionCount;
    }, 0);
    data = data.filter(x => x.customerResultId == customerResultId)

    if (data == undefined || data.length == 0)
      return 0;

    return data[0].totalDecisionCount + " " + "(" + parseFloat(((data[0].totalDecisionCount / totalCount) * 100).toFixed(2)).toString() + " %)";
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  toggleFilter() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  onPager(event: any) {
    this.model.index = event;
    this.getData();
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

  cancelFilter() {
    this.model = new RejectionDashboardRequest();
    this.masterModel.filterDataShown = false;
  }

  reset() {
    this.initialize();
    this.groupByFilter();
    this.dateSelectionDefault();
    this.defaultGroupBy();
    this.LoadFirstTime = true;
    this.getCustomerListBySearch();
    this.getSupListBySearch();
    this.getCountryListBySearch();
  }

  //fetch the first 10 fact for the supplier on load
  getFactListBySearch() {
    this.masterModel.factoryList = null;
    this.masterModel.factoryRequest.supplierType = SupplierType.Factory;
    this.masterModel.factoryRequest.supplierId = this.model.supplierId;
    this.masterModel.factoryRequest.customerId = this.model.customerId;
    if (this.currentUser.usertype == UserType.Factory)
      this.masterModel.factoryRequest.idList.push(this.currentUser.factoryid);

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

  //fetch the facotry data with virtual scroll
  getFactoryData() {
    this.masterModel.factoryRequest.searchText = this.masterModel.factoryInput.getValue();
    this.masterModel.factoryRequest.skip = this.masterModel.factoryList.length;

    this.masterModel.factoryRequest.customerId = this.model.customerId;
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

  changeSupplierType(item) {
    this.masterModel.supLoading = true;
    this.masterModel.supplierList = [];
    if (this.currentUser.usertype != UserType.Supplier)
      this.model.supplierId = null;
    if (this.currentUser.usertype != UserType.Factory)
      this.model.selectedFactoryIdList = [];
    this.masterModel.factoryList = [];
    this.masterModel.supsearchRequest.idList = [];
    if (item.id == SearchType.SupplierCode) {
      this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierCode;
    }
    else if (item.id == SearchType.SupplierName) {
      this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    }
    this.getSupplierData();
  }

  clearSupplier() {
    if (this.currentUser.usertype != UserType.Factory)
      this.model.selectedFactoryIdList = [];
    this.masterModel.factoryList = [];
    this.masterModel.supsearchRequest.idList = [];
    this.masterModel.supplierList = [];
    this.getSupplierData();
  }

  //supplier change event get supplier selected name shown in strip
  supplierChange(supplierItem) {
    if (supplierItem && supplierItem.id > 0) {
      let supplierDetails = this.masterModel.supplierList.find(x => x.id == supplierItem.id);
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

  assignParentData() {
    if (this.pathroute.snapshot.queryParams && this.pathroute.snapshot.queryParams.param) {
      let parentModel = JSON.parse(decodeURI(this.pathroute.snapshot.queryParams.param))
      if (parentModel) {
        if (parentModel.customerId)
          this.requestCustomerModel.idList.push(parentModel.customerId);

        if (parentModel.supplierTypeId > 0)
          this.masterModel.supsearchRequest.supSearchTypeId = parentModel.supplierTypeId;

        if (parentModel.supplierId > 0)
          this.masterModel.supsearchRequest.idList.push(parentModel.supplierId);

        if (parentModel.selectedFactoryIdList && parentModel.selectedFactoryIdList.length > 0)
          this.masterModel.factoryRequest.idList = parentModel.selectedFactoryIdList;

        if (parentModel.selectedCountryIdList && parentModel.selectedCountryIdList.length > 0)
          this.masterModel.countryRequest.countryIds = parentModel.selectedCountryIdList;

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

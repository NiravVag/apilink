import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { BuyerDataSourceRequest, CountryDataSourceRequest, CustomerContactSourceRequest, CustomerDataSourceRequest, ProductCategorySourceRequest, ProductSubCategorySourceRequest, ResponseResult, SupplierDataSourceRequest } from 'src/app/_Models/common/common.model';
import { GenericAPIGETRequest, GenericAPIPOSTRequest } from 'src/app/_Models/genericapi/genericapirequest.model';
import { SupplierScoreMaster, SupplierScoreModel } from 'src/app/_Models/tcf/supplier-score-model';
import { TCFStageResponse } from 'src/app/_Models/tcf/tcfcommon.model';
import { SupplierScoreTermEnum, TCFFilterRequest, TCFTermEnum } from 'src/app/_Models/tcf/tcfdashboard.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { TCFService } from 'src/app/_Services/tcfdata/tcf.service';
import { Validator } from '../../common';
import { amCoreLicense, APIService, DataLengthTrim, DefectDashboardParetoNameTrim, NumberOne, PageSizeCommon, SupplierScoreTermList, tcfSearchtypelst, UserType } from '../../common/static-data-common';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import { userTokenRequest } from 'src/app/_Models/tcf/tcflanding.model';
import { SummaryComponent } from '../../common/summary.component';
import { ActivatedRoute, Router } from '@angular/router';
import { EditBookingCustomerResult } from 'src/app/_Models/booking/inspectionbooking.model';
const config = require("src/assets/appconfig/appconfig.json");
@Component({
  selector: 'app-supplier-score',
  templateUrl: './supplier-score.component.html',
  styleUrls: ['./supplier-score.component.scss'],
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
export class SupplierScoreComponent extends SummaryComponent<SupplierScoreModel> {
  isFilterOpen: boolean;
  isMobile: boolean;
  isTablet: boolean;
  bar1Config: { width: number; offset: number; };
  bar2Config: { width: number; offset: number; };
  supplierScoreMaster: SupplierScoreMaster;
  model: SupplierScoreModel;
  tcfSearchtypelst: any = tcfSearchtypelst;
  genericGetRequest: GenericAPIGETRequest;
  user: any;
  toggleFormSection: boolean;
  userTokenRequest: userTokenRequest;
  supplierFilterRequest: TCFFilterRequest;
  error: any;
  pagesizeitems = PageSizeCommon;
  dataLengthTrim: number = DataLengthTrim;
  clusteredColumChart: am4charts.XYChart;
  suppName: am4charts.XYChart;
  selectedPageSize

  constructor(public calendar: NgbCalendar, public validator: Validator, private tcfService: TCFService, private customerService: CustomerService, private supplierService: SupplierService,
    private customerProductService: CustomerProduct, private locationService: LocationService, router: Router, route: ActivatedRoute, protected translate?: TranslateService, protected toastr?: ToastrService) {
    super(router, validator, route, translate);
    am4core.addLicense(amCoreLicense);
    this.toggleFormSection = false;
    //intialize the objects
    this.objectInitialization();
    //get the current user from the local storage
    if (localStorage.getItem('currentUser'))
      this.user = JSON.parse(localStorage.getItem('currentUser'));
  }

  objectInitialization() {
    this.model = new SupplierScoreModel();
    this.supplierScoreMaster = new SupplierScoreMaster();
    this.genericGetRequest = new GenericAPIGETRequest();
    this.selectedPageSize = PageSizeCommon[0];
    this.model.pageSize = PageSizeCommon[0];
    this.model.index = NumberOne;
  }

  onInit(): void {
    this.validator.setJSON("tcf/tcf-dashboard.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.getTCFTermList();
    this.getTCFStatusList();
    this.getCustomerListBySearch();
    this.getSupplierListBySearch();
    this.getBuyerListBySearch();
    this.getCustomerContactListBySearch();
    this.getProductCategoryListBySearch();
    this.getCountryOriginListBySearch();
    this.getCountryDestinationListBySearch();
  }

  ngAfterViewInit() {
    this.SearchDetails();
  }

  getData(): void {
    this.getSupplierScoreList();
  }
  getPathDetails(): string {
    throw new Error('Method not implemented.');
  }

  //get mandayterm list
  getTCFTermList() {
    this.supplierScoreMaster.supplierScoreTermList = SupplierScoreTermList;
    this.model.globalDateTypeId = SupplierScoreTermEnum.LastTenDay;
    this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'd', 10);
    this.model.todate = this.calendar.getToday();
  }

  //get the tcf status list
  getTCFStatusList() {
    const config = require("src/assets/appconfig/appconfig.json");
    this.genericGetRequest.requestUrl = config.TCF.tcfStatusList;
    this.genericGetRequest.isGenericToken = true;
    this.supplierScoreMaster.statusLoading = true;
    this.tcfService.getData(this.genericGetRequest).subscribe(data => {
      this.processTCFStatusResponse(data);
    },
      error => {
        this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
        this.supplierScoreMaster.statusLoading = false;
      });
  }

  //process the tcf status response
  processTCFStatusResponse(data) {
    if (data && data.status && data.status == TCFStageResponse.Success) {
      this.supplierScoreMaster.statusList = data.data;
      this.supplierScoreMaster.statusLoading = false;
    } else if (data && data.status && data.status == TCFStageResponse.NotFound) {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.LBL_TCF_STATUS_NOT_FOUND');
      this.supplierScoreMaster.statusLoading = false;
    } else {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
      this.supplierScoreMaster.statusLoading = false;
    }
  }

  changeTerm(event) {
    if (event.id == SupplierScoreTermEnum.LastTenDay) {
      this.supplierScoreMaster.isDateShow = false;
      this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'd', 10);
      this.model.todate = this.calendar.getToday();
    } else if (event.id == SupplierScoreTermEnum.LastThreeMonth) {
      this.supplierScoreMaster.isDateShow = false;
      this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
      this.model.todate = this.calendar.getToday();
    } else if (event.id == SupplierScoreTermEnum.LastSixMonth) {
      this.supplierScoreMaster.isDateShow = false;
      this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'm', 6);
      this.model.todate = this.calendar.getToday();
    } else if (event.id == SupplierScoreTermEnum.Custom) {
      this.supplierScoreMaster.isDateShow = true;
      this.model.fromdate = null;
      this.model.todate = null;
    }
    this.validator.isSubmitted = false;
  }

  //toggling the filter
  toggleFilter() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  reset() {
    this.objectInitialization();
    this.supplierScoreMaster.isDateShow = false;
    this.ngOnInit();
  }

  onPager(event: any) {
    this.model.index = event;
    this.getData();
  }

  SearchList() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.index = NumberOne;
      this.model.pageSize = this.selectedPageSize;
      this.getData();
    }
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.getDashboardData();
    }
  }
  SetSearchTypemodel(searchtype) {
    this.model.searchTypeId = searchtype;
  }

  getDashboardData() {
    //make the user token request
    this.generateUserTokenRequest();
    //make the user token post request
    const request = this.generatePostRequest(config.TCF.userAuthentication, this.userTokenRequest, "");
    this.getDashboardDatatDataWithUserToken(request);
  }

  //validate the form
  formValid(): boolean {
    let isOk = this.validator.isValidIf('todate', this.IsDateValidationRequired()) && this.validator.isValidIf('fromdate', this.IsDateValidationRequired())
    return isOk;
  }

  //validate the from date and to date
  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchTypeText != null &&
      this.model.searchTypeText.trim() == "" ? true : false;

    if (this.model.searchTypeText == null || this.model.searchTypeText == "") {
      if (!this.model.fromdate)
        this.validator.isValid('fromdate');

      else if (this.model.fromdate && !this.model.todate)
        this.validator.isValid('todate');
    }
    return isOk;
  }

  //clear the date input
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

  //make the user token request
  generateUserTokenRequest() {
    this.userTokenRequest = new userTokenRequest();
    this.userTokenRequest.userId = this.user.id;
    switch (this.user.usertype) {
      case UserType.InternalUser:
        this.userTokenRequest.userType = UserType[UserType.InternalUser];
        break;
      case UserType.Customer:
        this.userTokenRequest.userType = UserType[UserType.Customer];
        break;
      case UserType.Supplier:
        this.userTokenRequest.userType = UserType[UserType.Supplier];
        break;
    }
  }

  //make the post request structure with required inputs
  generatePostRequest(requestUrl, requestInput, token) {
    const request = new GenericAPIPOSTRequest();
    request.requestUrl = requestUrl;
    request.requestData = requestInput;
    request.token = token;
    return request;
  }

  //get the dashboard data with the user token
  getDashboardDatatDataWithUserToken(request) {
    this.supplierScoreMaster.tokenLoading = true;
    this.tcfService.postData(request).subscribe(data => {
      this.processUserTokenResponse(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.supplierScoreMaster.tokenLoading = false;
      });
  }

  processUserTokenResponse(data) {
    if (data && data.result) {
      //parse ther response result to json
      const resultData = JSON.parse(data.result);
      //if response result is success and then assign the user token(this.userToken will be used for all subsequent requests)
      if (resultData && resultData.status && resultData.status == "1"
        && resultData.data && resultData.data.authorization) {
        this.supplierScoreMaster.userToken = resultData.data.authorization;
        this.getData();
        this.resetChartData();
        this.getSupplierScoreDocProvide();
        this.getSupplierScoreDocReviewed();
        this.getSupplierScoreDocReceived();
      }
      else {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
      }
      this.supplierScoreMaster.tokenLoading = false;
    }
    else {
      this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
      this.supplierScoreMaster.tokenLoading = false;
    }
  }

  resetChartData() {
    this.supplierScoreMaster.supplierScoreList = [];
    this.supplierScoreMaster.supplierScoreDocProvidelst = [];
    this.supplierScoreMaster.supplierScoreDocReviewedlst = [];
    this.supplierScoreMaster.supplierScoreDocReceivedlst = [];
  }

  //map the filter request
  mapFilterRequest() {
    this.supplierFilterRequest = new TCFFilterRequest();

    if (this.model.fromdate)
      this.supplierFilterRequest.fromDate = this.formatTCFDate(this.model.fromdate);
    if (this.model.todate)
      this.supplierFilterRequest.toDate = this.formatTCFDate(this.model.todate);
    if (this.model.searchTypeId)
      this.supplierFilterRequest.searchTypeId = this.model.searchTypeId;
    this.supplierFilterRequest.searchTypeText = this.model.searchTypeText;
    if (this.model.statusIds)
      this.supplierFilterRequest.statusIds = this.model.statusIds;
    this.supplierFilterRequest.customerIds = this.model.customerGLCodes;
    if (this.model.supplierIds)
      this.supplierFilterRequest.supplierIds = this.model.supplierIds;
    if (this.model.buyerIds)
      this.supplierFilterRequest.buyerIds = this.model.buyerIds;
    if (this.model.customerContactIds)
      this.supplierFilterRequest.customerContactIds = this.model.customerContactIds;
    if (this.model.productCategoryIds)
      this.supplierFilterRequest.productCategoryIds = this.model.productCategoryIds;
    if (this.model.productSubCategoryIds)
      this.supplierFilterRequest.productSubCategoryIds = this.model.productSubCategoryIds;
    if (this.model.countryOriginIds)
      this.supplierFilterRequest.countryOriginIds = this.model.countryOriginIds;
    if (this.model.countryDestinationIds)
      this.supplierFilterRequest.countryDestinationIds = this.model.countryDestinationIds;
    if (this.model.pageSize)
      this.supplierFilterRequest.pageSize = this.model.pageSize;
    if (this.model.index)
      this.supplierFilterRequest.index = this.model.index;

    this.supplierFilterRequest.globalDateTypeId = SupplierScoreTermEnum.Custom;
  }

  //format the tcf date
  formatTCFDate(dateValue) {
    return dateValue.year + "-" + dateValue.month + "-" + dateValue.day;
  }

  //get supplier score doc received
  getSupplierScoreDocReceived() {
    this.mapFilterRequest();
    const request = this.generatePostRequest(config.TCF.supplierScoreDocReceived, this.supplierFilterRequest, this.supplierScoreMaster.userToken);
    request.isGenericToken = false;
    this.supplierScoreMaster.supplierScoreDocReceivedLoading = true;
    this.tcfService.postData(request).subscribe(data => {

      if (data && data.result) {
        const resultData = JSON.parse(data.result);
        if (resultData && resultData.status && resultData.status == ResponseResult.Success) {
          this.mapSupplierScoreDocReceived(resultData.data);
        }
        this.supplierScoreMaster.supplierScoreDocReceivedLoading = false;
      }
      else {
        this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
        this.supplierScoreMaster.supplierScoreDocReceivedLoading = false;
      }
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.supplierScoreMaster.supplierScoreDocReceivedLoading = false;
      });
  }

  mapSupplierScoreDocReceived(data: any) {
    this.supplierScoreMaster.supplierScoreDocReceivedlst = [];
    this.supplierScoreMaster.supplierScoreDocReceivedSupplierNamelst = [];
    data.forEach(element => {
      const received_date = this.supplierScoreMaster.supplierScoreDocReceivedlst.find(x => x.date == element.received_date);
      if (received_date) {
        received_date[element.supplier_name] = element.total_doc;
      }
      else {
        const d = {};
        d["date"] = element.received_date;
        d[element.supplier_name] = element.total_doc;
        this.supplierScoreMaster.supplierScoreDocReceivedlst.push(d);
      }
      const suname = this.supplierScoreMaster.supplierScoreDocReceivedSupplierNamelst.find(x => x == element.supplier_name);
      if (suname == null) {
        this.supplierScoreMaster.supplierScoreDocReceivedSupplierNamelst.push(element.supplier_name);
      }
    });
    setTimeout(() => {
      this.renderColumnChart();
    },100)

  }

  //get supplier score doc received
  getSupplierScoreDocProvide() {
    this.mapFilterRequest();
    const request = this.generatePostRequest(config.TCF.supplierScoreDocProvide, this.supplierFilterRequest, this.supplierScoreMaster.userToken);
    request.isGenericToken = false;
    this.supplierScoreMaster.supplierScoreDocProvideLoading = true;
    this.supplierScoreMaster.supplierScoreDocProvideDataNotFound = false;
    this.tcfService.postData(request).subscribe(data => {
      if (data && data.result) {
        const resultData = JSON.parse(data.result);
        if (resultData && resultData.status && resultData.status == ResponseResult.Success) {
          this.mapSupplierScoreDocProvide(resultData.data);
          this.supplierScoreMaster.supplierScoreDocProvideLoading = false;
          if (resultData.data.length == 0)
            this.supplierScoreMaster.supplierScoreDocProvideDataNotFound = true;
        }
        else {
          this.supplierScoreMaster.supplierScoreDocProvideLoading = false;
          this.supplierScoreMaster.supplierScoreDocProvideDataNotFound = true;
        }
      }
      else {
        this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
        this.supplierScoreMaster.supplierScoreDocProvideLoading = false;
      }
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.supplierScoreMaster.supplierScoreDocProvideLoading = false;
      });
  }

  mapSupplierScoreDocProvide(data: any) {
    this.supplierScoreMaster.supplierScoreDocProvidelst = [];
    data.forEach(element => {
      const sup = this.supplierScoreMaster.supplierScoreDocProvidelst.find(x => x.supplier == element.supplier);
      if (sup) {
        sup[element.state] = element.total_doc;
      }
      else {
        const d = {};
        d["supplier"] = element.supplier;
        d[element.state] = element.total_doc;
        this.supplierScoreMaster.supplierScoreDocProvidelst.push(d);
      }
    });
    setTimeout(() => {
      this.renderHorizontalBarChart();
    },100)
  }

  //get supplier score doc received
  getSupplierScoreDocReviewed() {
    this.mapFilterRequest();
    const request = this.generatePostRequest(config.TCF.supplierScoreDocReviewed, this.supplierFilterRequest, this.supplierScoreMaster.userToken);
    request.isGenericToken = false;
    this.supplierScoreMaster.supplierScoreDocReviewedLoading = true;
    this.supplierScoreMaster.supplierScoreDocReviewedDataNotFound = false;
    this.tcfService.postData(request).subscribe(data => {
      if (data && data.result) {
        const resultData = JSON.parse(data.result);
        if (resultData && resultData.status && resultData.status == ResponseResult.Success) {
          this.mapSupplierScoreDocReviewed(resultData.data);
          this.supplierScoreMaster.supplierScoreDocReviewedLoading = false;
          if(resultData.data.length == 0)  
            this.supplierScoreMaster.supplierScoreDocReviewedDataNotFound = true;
        }
        else{
          this.supplierScoreMaster.supplierScoreDocReviewedLoading = false;
          this.supplierScoreMaster.supplierScoreDocReviewedDataNotFound = true;
        }
      }
      else {
        this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
        this.supplierScoreMaster.supplierScoreDocReviewedLoading = false;
      }
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.supplierScoreMaster.supplierScoreDocReviewedLoading = false;
      });
  }

  mapSupplierScoreDocReviewed(data: any) {
    this.supplierScoreMaster.supplierScoreDocReviewedlst = [];
    this.supplierScoreMaster.supplierScoreDocStatelst = [];
    data.forEach(element => {
      const reviewData = this.supplierScoreMaster.supplierScoreDocReviewedlst.find(x => x.orgdate == element.review_date);
      if (reviewData) {
        reviewData[element.state] = element.total_doc;
      }
      else {
        const d = {};
        d["orgdate"] = element.review_date;
        d["date"] = new Date(element.review_date);
        d[element.state] = element.total_doc;
        this.supplierScoreMaster.supplierScoreDocReviewedlst.push(d);
      }
      if (element.state != null) {
        let state = this.supplierScoreMaster.supplierScoreDocStatelst.find(x => x == element.state);
        if (state == null) {
          this.supplierScoreMaster.supplierScoreDocStatelst.push(element.state);
        }
      }
    });
    setTimeout(() => {
      this.renderLineChart('multiLineChart');
    },100)
  }

  //get supplier score doc received
  getSupplierScoreList() {
    this.mapFilterRequest();
    const request = this.generatePostRequest(config.TCF.supplierScoreList, this.supplierFilterRequest, this.supplierScoreMaster.userToken);
    request.isGenericToken = false;
    this.supplierScoreMaster.supplierScoreListLoading = true;
    this.model.noFound = false;
    this.tcfService.postData(request).subscribe(data => {
      this.processSupplierScoreListResponse(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.supplierScoreMaster.supplierScoreListLoading = false;
      });
  }

  //process the supplier score list response
  processSupplierScoreListResponse(data: any) {
    if (data && data.result) {
      const resultData = JSON.parse(data.result);
      if (resultData && resultData.status && resultData.status == ResponseResult.Success)
        this.mapSupplierScoreListResponse(resultData);
      else if (resultData && resultData.status && resultData.status == ResponseResult.NoDataFound)
        this.model.noFound = true;
      else
        this.error = resultData;
      this.supplierScoreMaster.supplierScoreListLoading = false;
    }
    else {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
      this.supplierScoreMaster.supplierScoreListLoading = false;
    }
  }

  mapSupplierScoreListResponse(resultData: any) {
    this.supplierScoreMaster.supplierScoreList = [];
    if (resultData) {
      this.model.totalCount = resultData.total_records;
      this.supplierScoreMaster.supplierScoreList = resultData.data;
    }
  }

  //fetch the first 10 customers on load
  getCustomerListBySearch() {
    this.supplierScoreMaster.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.customerLoading = true),
      switchMap(term => term ?
        this.customerService.getCustomerDataSource(this.supplierScoreMaster.requestCustomerModel, term) :
        this.customerService.getCustomerDataSource(this.supplierScoreMaster.requestCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.customerLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.customerList = data;
        this.supplierScoreMaster.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    this.supplierScoreMaster.requestCustomerModel.serviceId = APIService.Tcf;
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestCustomerModel.searchText = this.supplierScoreMaster.customerInput.getValue();
      this.supplierScoreMaster.requestCustomerModel.skip = this.supplierScoreMaster.customerList.length;
    }
    this.supplierScoreMaster.customerLoading = true;
    this.customerService.getCustomerDataSource(this.supplierScoreMaster.requestCustomerModel).subscribe(data => {
      if (data && data.length > 0)
        this.supplierScoreMaster.customerList = this.supplierScoreMaster.customerList.concat(data);
      this.supplierScoreMaster.customerLoading = false;
    }),
      error => {
        this.supplierScoreMaster.customerLoading = false;
      };
  }

  changeCustomerData() {
    this.resetCustomer();
    let isDefaultLoad = false;
    if (this.model.customerGLCodes && this.model.customerGLCodes.length > 0)
      isDefaultLoad = true;

    this.getSupplierData(isDefaultLoad);
    this.getBuyerData(isDefaultLoad);
    this.getCustomerContactData(isDefaultLoad);
  }

  clearCustomerSelection() {
    this.supplierScoreMaster.requestCustomerModel = new CustomerDataSourceRequest();
    this.model.customerGLCodes = [];
    this.supplierScoreMaster.customerList = [];
    this.resetCustomer();
    this.getCustomerData(false);
    this.getSupplierData(false);
    this.getBuyerData(false);
  }

  resetCustomer() {
    this.model.supplierIds = [];
    this.model.buyerIds = [];
    this.model.customerContactIds = [];
    this.supplierScoreMaster.supplierList = [];
    this.supplierScoreMaster.buyerList = [];
    this.supplierScoreMaster.customerContactList = [];
  }

  //fetch the first 10 customers on load
  getSupplierListBySearch() {
    this.supplierScoreMaster.requestSupplierModel.serviceId = APIService.Tcf;
    if (this.user.customerid)
      this.supplierScoreMaster.requestSupplierModel.customerIds.push(this.user.customerid);
    this.supplierScoreMaster.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.supplierLoading = true),
      switchMap(term => term
        ? this.supplierService.getSupplierDataSource(this.supplierScoreMaster.requestSupplierModel, APIService.Tcf, term)
        : this.supplierService.getSupplierDataSource(this.supplierScoreMaster.requestSupplierModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.supplierLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.supplierList = data;
        this.supplierScoreMaster.supplierLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getSupplierData(isDefaultLoad: boolean) {
    this.supplierScoreMaster.requestSupplierModel.serviceId = APIService.Tcf;
    if (this.model.customerGLCodes)
      this.supplierScoreMaster.requestSupplierModel.customerglCodes = this.model.customerGLCodes;
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestSupplierModel.searchText = this.supplierScoreMaster.supplierInput.getValue();
      this.supplierScoreMaster.requestSupplierModel.skip = this.supplierScoreMaster.supplierList.length;
    }
    this.supplierScoreMaster.supplierLoading = true;
    this.supplierService.getSupplierDataSource(this.supplierScoreMaster.requestSupplierModel, APIService.Tcf).
      subscribe(data => {
        if (data && data.length > 0)
          this.supplierScoreMaster.supplierList = this.supplierScoreMaster.supplierList.concat(data);
        this.supplierScoreMaster.supplierLoading = false;
      }),
      error => {
        this.supplierScoreMaster.supplierLoading = false;
      };
  }

  clearSupplierSelection() {
    this.supplierScoreMaster.requestSupplierModel = new SupplierDataSourceRequest();
    this.supplierScoreMaster.supplierList = [];
    this.getSupplierData(false);
    this.getDashboardData();
  }

  //fetch the first 10 buyers on load
  getBuyerListBySearch() {
    if (this.model.customerGLCodes)
      this.supplierScoreMaster.requestBuyerModel.customerglCodes = this.model.customerGLCodes;
    this.supplierScoreMaster.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.buyerLoading = true),
      switchMap(term => term ?
        this.customerService.getCustomerBuyerDataSource(this.supplierScoreMaster.requestBuyerModel, APIService.Tcf, term) :
        this.customerService.getCustomerBuyerDataSource(this.supplierScoreMaster.requestBuyerModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.buyerLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.buyerList = data;
        this.supplierScoreMaster.buyerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getBuyerData(isDefaultLoad: boolean) {
    if (this.model.customerGLCodes)
      this.supplierScoreMaster.requestBuyerModel.customerglCodes = this.model.customerGLCodes;
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestBuyerModel.searchText = this.supplierScoreMaster.buyerInput.getValue();
      this.supplierScoreMaster.requestBuyerModel.skip = this.supplierScoreMaster.buyerList.length;
    }
    this.supplierScoreMaster.buyerLoading = true;
    this.customerService.getCustomerBuyerDataSource(this.supplierScoreMaster.requestBuyerModel, APIService.Tcf).subscribe(data => {
      if (data && data.length > 0)
        this.supplierScoreMaster.buyerList = this.supplierScoreMaster.buyerList.concat(data);
      this.supplierScoreMaster.buyerLoading = false;
    }),
      error => {
        this.supplierScoreMaster.buyerLoading = false;
      };
  }

  clearBuyerSelection() {
    this.supplierScoreMaster.requestBuyerModel = new BuyerDataSourceRequest();
    this.supplierScoreMaster.buyerList = [];
    this.getBuyerData(false);
  }

  //fetch the first 10 customer Contact on load
  getCustomerContactListBySearch() {
    if (this.model.customerGLCodes)
      this.supplierScoreMaster.requestCustomerContactModel.customerglCodes = this.model.customerGLCodes;
    this.supplierScoreMaster.customerContactInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.customerContactLoading = true),
      switchMap(term => term ?
        this.customerService.getCustomerContactDataSource(this.supplierScoreMaster.requestCustomerContactModel, APIService.Tcf, term) :
        this.customerService.getCustomerContactDataSource(this.supplierScoreMaster.requestCustomerContactModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.customerContactLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.customerContactList = data;
        this.supplierScoreMaster.customerContactLoading = false;
      });
  }

  //fetch the customer Contact data with virtual scroll
  getCustomerContactData(isDefaultLoad: boolean) {
    if (this.model.customerGLCodes)
      this.supplierScoreMaster.requestCustomerContactModel.customerglCodes = this.model.customerGLCodes;
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestCustomerContactModel.searchText = this.supplierScoreMaster.customerContactInput.getValue();
      this.supplierScoreMaster.requestCustomerContactModel.skip = this.supplierScoreMaster.customerContactList.length;
    }
    this.supplierScoreMaster.customerContactLoading = true;
    this.customerService.getCustomerContactDataSource(this.supplierScoreMaster.requestCustomerContactModel, APIService.Tcf).subscribe(data => {
      if (data && data.length > 0)
        this.supplierScoreMaster.customerContactList = this.supplierScoreMaster.customerContactList.concat(data);
      this.supplierScoreMaster.customerContactLoading = false;
    }),
      error => {
        this.supplierScoreMaster.customerContactLoading = false;
      };
  }

  clearCustomerContactSelection() {
    this.supplierScoreMaster.requestCustomerContactModel = new CustomerContactSourceRequest();
    this.supplierScoreMaster.customerContactList = [];
    this.getCustomerContactData(false);
  }

  //fetch the first 10 product category on load
  getProductCategoryListBySearch() {
    this.supplierScoreMaster.requestProductCategoryModel.serviceId = APIService.Tcf;
    this.supplierScoreMaster.productCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.productCategoryLoading = true),
      switchMap(term => term ?
        this.customerProductService.getProductCategoryDataSource(this.supplierScoreMaster.requestProductCategoryModel, term) :
        this.customerProductService.getProductCategoryDataSource(this.supplierScoreMaster.requestProductCategoryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.productCategoryLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.productCategoryList = data;
        this.supplierScoreMaster.productCategoryLoading = false;
      });
  }

  //fetch the product category with virtual scroll
  getProductCategoryData(isDefaultLoad: boolean) {
    this.supplierScoreMaster.requestProductCategoryModel.serviceId = APIService.Tcf;
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestProductCategoryModel.searchText = this.supplierScoreMaster.productCategoryInput.getValue();
      this.supplierScoreMaster.requestProductCategoryModel.skip = this.supplierScoreMaster.productCategoryList.length;
    }
    this.supplierScoreMaster.productCategoryLoading = true;
    this.customerProductService.getProductCategoryDataSource(this.supplierScoreMaster.requestProductCategoryModel).subscribe(data => {
      if (data && data.length > 0)
        this.supplierScoreMaster.productCategoryList = this.supplierScoreMaster.productCategoryList.concat(data);
      this.supplierScoreMaster.productCategoryLoading = false;
    }),
      error => {
        this.supplierScoreMaster.productCategoryLoading = false;
      };
  }

  //change the product category data
  changeProductCategoryData() {
    if (this.model.productCategoryIds) {
      this.supplierScoreMaster.productSubCategoryList = [];
      this.getProductSubCategoryData(true);
    }
  }

  //clear the product category data
  clearProductCategorySelection() {
    this.supplierScoreMaster.requestProductCategoryModel = new ProductCategorySourceRequest();
    this.supplierScoreMaster.productCategoryList = [];
    this.getProductCategoryData(false);
  }

  //fetch the first 10 product subcategory on load
  getProductSubCategoryListBySearch() {
    this.supplierScoreMaster.requestProductSubCategoryModel.serviceId = APIService.Tcf;
    if (this.model.productCategoryIds)
      this.supplierScoreMaster.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
    this.supplierScoreMaster.productSubCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.productSubCategoryLoading = true),
      switchMap(term => term ?
        this.customerProductService.getProductSubCategoryDataSource(this.supplierScoreMaster.requestProductSubCategoryModel, term) :
        this.customerProductService.getProductSubCategoryDataSource(this.supplierScoreMaster.requestProductSubCategoryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.productSubCategoryLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.productSubCategoryList = data;
        this.supplierScoreMaster.productSubCategoryLoading = false;
      });
  }

  //fetch the product subcategory data with virtual scroll
  getProductSubCategoryData(isDefaultLoad: boolean) {
    this.supplierScoreMaster.requestProductSubCategoryModel.serviceId = APIService.Tcf;
    if (this.model.productCategoryIds)
      this.supplierScoreMaster.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestProductSubCategoryModel.searchText = this.supplierScoreMaster.productSubCategoryInput.getValue();
      this.supplierScoreMaster.requestProductSubCategoryModel.skip = this.supplierScoreMaster.productSubCategoryList.length;
    }
    this.supplierScoreMaster.productSubCategoryLoading = true;
    this.customerProductService.getProductSubCategoryDataSource(this.supplierScoreMaster.requestProductSubCategoryModel).subscribe(data => {
      if (data && data.length > 0)
        this.supplierScoreMaster.productSubCategoryList = this.supplierScoreMaster.productSubCategoryList.concat(data);
      this.supplierScoreMaster.productSubCategoryLoading = false;
    }),
      error => {
        this.supplierScoreMaster.productSubCategoryLoading = false;
      };
  }

  clearProductSubCategorySelection() {
    this.supplierScoreMaster.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();
    this.supplierScoreMaster.productSubCategoryList = [];
    this.getProductSubCategoryData(false);
  }

  //fetch the first 10 country origin on load
  getCountryOriginListBySearch() {
    this.supplierScoreMaster.countryOriginInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.countryOriginLoading = true),
      switchMap(term => term ?
        this.locationService.getCountryDataSourceList(this.supplierScoreMaster.requestCountryModel, term) :
        this.locationService.getCountryDataSourceList(this.supplierScoreMaster.requestCountryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.countryOriginLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.countryOriginList = data;
        this.supplierScoreMaster.countryOriginLoading = false;
      });
  }

  //fetch the country origin data with virtual scroll
  getCountryOriginData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestCountryModel.searchText = this.supplierScoreMaster.countryOriginInput.getValue();
      this.supplierScoreMaster.requestCountryModel.skip = this.supplierScoreMaster.countryOriginList.length;
    }
    this.supplierScoreMaster.countryOriginLoading = true;
    this.locationService.getCountryDataSourceList(this.supplierScoreMaster.requestCountryModel).subscribe(data => {
      if (data && data.length > 0)
        this.supplierScoreMaster.countryOriginList = this.supplierScoreMaster.countryOriginList.concat(data);
      this.supplierScoreMaster.countryOriginLoading = false;
    }),
      error => {
        this.supplierScoreMaster.countryOriginLoading = false;
      };
  }

  clearCountryOriginSelection() {
    this.supplierScoreMaster.requestCountryModel = new CountryDataSourceRequest();
    this.supplierScoreMaster.countryOriginList = [];
    this.getCountryOriginData(false);
  }

  //fetch the first 10 country destination on load
  getCountryDestinationListBySearch() {
    this.supplierScoreMaster.countryDestinationInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierScoreMaster.countryDestinationLoading = true),
      switchMap(term => term ?
        this.locationService.getCountryDataSourceList(this.supplierScoreMaster.requestCountryDestinationModel, term) :
        this.locationService.getCountryDataSourceList(this.supplierScoreMaster.requestCountryDestinationModel, term)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierScoreMaster.countryDestinationLoading = false))
      ))
      .subscribe(data => {
        this.supplierScoreMaster.countryDestinationList = data;
        this.supplierScoreMaster.countryDestinationLoading = false;
      });
  }

  //fetch the country destination data with virtual scroll
  getCountryDestinationData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.supplierScoreMaster.requestCountryDestinationModel.searchText = this.supplierScoreMaster.countryDestinationInput.getValue();
      this.supplierScoreMaster.requestCountryDestinationModel.skip = this.supplierScoreMaster.countryDestinationList.length;
    }
    this.supplierScoreMaster.countryDestinationLoading = true;
    this.locationService.getCountryDataSourceList(this.supplierScoreMaster.requestCountryDestinationModel).subscribe(data => {
      if (data && data.length > 0)
        this.supplierScoreMaster.countryDestinationList = this.supplierScoreMaster.countryDestinationList.concat(data);
      this.supplierScoreMaster.countryDestinationLoading = false;
    }),
      error => {
        this.supplierScoreMaster.countryOriginLoading = false;
      };
  }

  clearCountryDestinationSelection() {
    this.supplierScoreMaster.requestCountryDestinationModel = new CountryDataSourceRequest();
    this.supplierScoreMaster.countryDestinationList = [];
    this.getCountryDestinationData(false);
  }

  //show the error message
  public showError(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }

  //show the warning message
  public showWarning(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.warning(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }

  graphInitialization() {
    if (window.innerWidth < 1025) {
      if (window.innerWidth < 450) {
        this.isMobile = true;
        this.isTablet = false;
      }
      else {
        this.isMobile = false;
        this.isTablet = true;
      }
    }
    else {
      this.isMobile = false;
      this.isTablet = false;
    }

    this.bar1Config = {
      width: this.isTablet ? 140 : this.isMobile ? 140 : 300,
      offset: 0
    };

    this.bar2Config = {
      width: this.isTablet ? 140 : this.isMobile ? 140 : 300,
      offset: this.isTablet ? 140 : this.isMobile ? 140 : 300,
    }
  }

  /**
  * Function to render line chart
  */
  renderHorizontalBarChart() {
    let chart = am4core.create('horizontalBar-chart', am4charts.XYChart);
    chart.data = this.supplierScoreMaster.supplierScoreDocProvidelst;
    chart.colors.list = [
      am4core.color("#24c11e"),
      am4core.color("#f81538"),
      am4core.color("#ffc500"),
    ];
    let datasize = this.supplierScoreMaster.supplierScoreDocProvidelst  ?
      this.supplierScoreMaster.supplierScoreDocProvidelst.length : 0;

    let categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = 'supplier';
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.grid.template.opacity = 0;
    categoryAxis.renderer.labels.template.fontSize = 10;
    categoryAxis.renderer.labels.template.truncate = true;
    categoryAxis.renderer.labels.template.maxWidth = 120;
    categoryAxis.renderer.labels.template.tooltipText =  "{category}";
    categoryAxis.renderer.cellStartLocation = datasize <= 3 ? 0.2 : 0.2;
    categoryAxis.renderer.cellEndLocation = datasize <= 3 ? 0.6 : 0.8;

    var label = categoryAxis.renderer.labels.template;
    label.truncate = true;
    label.maxWidth = 140;
    label.tooltipText = "{category}";
    categoryAxis.tooltip.label.fontSize = 11;
    categoryAxis.tooltip.getFillFromObject = false;
    categoryAxis.tooltip.background.propertyFields.stroke = "color";
    categoryAxis.tooltip.background.fill = am4core.color("#4c698d");
    categoryAxis.tooltip.label.textAlign = "middle";
    categoryAxis.tooltip.label.maxWidth = 200;
    categoryAxis.tooltip.label.wrap = true;

    let valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
    //valueAxis.min = 0;
    //valueAxis.max = 2100;
    valueAxis.renderer.grid.template.opacity = 1;
    valueAxis.renderer.ticks.template.strokeOpacity = 0.5;
    valueAxis.renderer.ticks.template.stroke = am4core.color('#495C43');
    valueAxis.renderer.line.strokeOpacity = 0;

    valueAxis.renderer.labels.template.disabled = false;
    valueAxis.renderer.labels.template.fill = am4core.color('#AAAFB6');
    valueAxis.renderer.labels.template.fontSize = 10;

    function createSeries(field, name) {
      var graphwidth = datasize >= 5 ? 85 : datasize >= 3 ? 40 : 10;

      let series = chart.series.push(new am4charts.ColumnSeries());
      series.dataFields.valueX = field;
      series.columns.template.width = 10;//am4core.percent(graphwidth);
      series.dataFields.categoryY = 'supplier';
      series.stacked = true;
      series.name = name;
      series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
    }

    createSeries('validate', 'Validate');
    createSeries('reject', 'Reject');
    createSeries('pending', 'Pending');
  }

  renderColumnChart() {
    // Create chart instance
    let chart = am4core.create("documentchartdiv", am4charts.XYChart);
    let chartData = this.supplierScoreMaster.supplierScoreDocReceivedlst;
    // Add data
    chart.data = chartData;

    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "date";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.labels.template.fontSize = 12;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");


    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.renderer.labels.template.fontSize = 12;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    // Create series
    function createSeries(field, name) {

      // Set up series
      let series = chart.series.push(new am4charts.ColumnSeries());
      series.name = name.length > DefectDashboardParetoNameTrim ? name.substring(0, DefectDashboardParetoNameTrim) + "..." : name
      series.dataFields.valueY = field;
      series.dataFields.categoryX = "date";
      series.sequencedInterpolation = true;

      // Make it stacked
      series.stacked = true;

      // Configure columns
      series.columns.template.width = am4core.percent(60);
      series.columns.template.tooltipText = "[bold]{name}[/]\n[font-size:14px]{categoryX}: {valueY}";

      // Add label
      // let labelBullet = series.bullets.push(new am4charts.LabelBullet());
      // labelBullet.label.text = "{valueY}";
      // labelBullet.locationY = 0.5;
      // labelBullet.label.hideOversized = true;

      return series;
    }

    this.supplierScoreMaster.supplierScoreDocReceivedSupplierNamelst.forEach(element => {
      createSeries(element, element);
    });
    this.suppName = chart;
    console.log(this.suppName)
  }

  renderLineChart(container, type = null) {
    let chart = am4core.create(container, am4charts.XYChart);
    let chartData = this.supplierScoreMaster.supplierScoreDocReviewedlst;
    // Add data
    chart.data = chartData;

    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.DateAxis());
    categoryAxis.dataFields.date = "date";
    //categoryAxis.renderer.minGridDistance = 10;
    categoryAxis.renderer.labels.template.fontSize = 12;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minGridDistance = 20;
    valueAxis.renderer.labels.template.fontSize = 12;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    // Create series
    let series = chart.series.push(new am4charts.LineSeries());
    series.legendSettings.valueText = "[bold]{valueY.close}[/]";
    series.stroke = am4core.color("#ffc500");
    series.dataFields.valueY = "pending";
    series.dataFields.dateX = "date";
    series.name = 'pending';
    series.strokeWidth = 1;
    series.tooltipText = "{dateX}: [b]{valueY}[/]";
    if (type == 1) series.tensionX = 0.8;

    let bullet = series.bullets.push(new am4charts.CircleBullet());
    bullet.tooltipText = "{date}\n[bold font-size: 17px]value: {valueY}[/]";
    bullet.strokeWidth = 1;
    bullet.stroke = am4core.color("#fff")
    bullet.circle.fill = series.stroke;

    // Create series
    let series2 = chart.series.push(new am4charts.LineSeries());
    series2.legendSettings.valueText = "[bold]{valueY.close}[/]";
    series2.stroke = am4core.color("#f81538");
    series2.dataFields.valueY = "reject";
    series2.dataFields.dateX = "date";
    series2.name = 'reject';
    series2.strokeWidth = 1;
    series2.tooltipText = "{dateX}: [b]{valueY}[/]";
    if (type == 1) series2.tensionX = 0.8;

    let bullet2 = series2.bullets.push(new am4charts.CircleBullet());
    bullet2.tooltipText = "{date}\n[bold font-size: 17px]value: {valueY}[/]";
    bullet2.strokeWidth = 1;
    bullet2.stroke = am4core.color("#fff")
    bullet2.circle.fill = series2.stroke;

    // Create series
    let series3 = chart.series.push(new am4charts.LineSeries());
    series3.legendSettings.valueText = "[bold]{valueY.close}[/] ({valueY.close.formatNumber('#.0')}%)";
    series3.stroke = am4core.color("#24c11e");
    series3.dataFields.valueY = "validate";
    series3.dataFields.dateX = "date";
    series3.name = 'validate';
    series3.strokeWidth = 1;
    series3.tooltipText = "{dateX}: [b]{valueY}[/]";
    if (type == 1) series3.tensionX = 0.8;

    let bullet3 = series3.bullets.push(new am4charts.CircleBullet());
    bullet3.tooltipText = "{date}\n[bold font-size: 17px]value: {valueY}[/]";
    bullet3.strokeWidth = 1;
    bullet3.stroke = am4core.color("#fff")
    bullet3.circle.fill = series3.stroke;

    chart.padding(20, 0, 20, 0);
    chart.cursor = new am4charts.XYCursor();

    // chart.legend = new am4charts.Legend();
  }
}

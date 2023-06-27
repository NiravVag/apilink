import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import * as c3 from 'c3';
import * as d3 from 'd3';
import { concat, Observable, of, Subject, BehaviorSubject } from 'rxjs';
import { TCFService } from '../../../_Services/tcfdata/tcf.service';
import { distinctUntilChanged, debounceTime, switchMap, tap, catchError, filter, map } from 'rxjs/operators';
import { CustomerDataSourceRequest, SupplierDataSourceRequest, BuyerDataSourceRequest, CustomerContactSourceRequest, ProductCategorySourceRequest, ProductSubCategorySourceRequest, CommonDataSourceRequest, CountryDataSourceRequest } from 'src/app/_Models/common/common.model';
import { TCFMasterModel } from 'src/app/_Models/tcf/tcfmastermodel';
import { TCFDetail, TCFItem, TCFLandingRequest, TCFResponse, TrafficLightColor, TCFListRequest, TCFRequestData, userTokenRequest, TCFListResponse, TCFDetailsResponse, TCFValidDocumentResponse, TCFReportResponse } from 'src/app/_Models/tcf/tcflanding.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { LocationService } from 'src/app/_Services/location/location.service';


import { APIService, PageSizeCommon, tcfSearchtypelst, tcfDatetypelst, UserType, supplierTypeList, SearchType } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { ActivatedRoute, Router } from '@angular/router';
import { Validator } from '../../common';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { GenericAPIGETRequest, GenericAPIPOSTRequest } from 'src/app/_Models/genericapi/genericapirequest.model';
import { InspectionBookingsummarymodel } from '../../../_Models/booking/inspectionbookingsummarymodel'//'src/app/_Models/Booking/inspectionBookingsummarymodel';
import { TCFStage, TCFSummaryPageType, TCFTaskType, TCFStageResponse, TCFDepartmentResponse } from 'src/app/_Models/tcf/tcfcommon.model';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
const config = require("src/assets/appconfig/appconfig.json");

@Component({
  selector: 'app-tcf-list',
  templateUrl: './tcf-summary.component.html',
  styleUrls: ['./tcf-summary.component.scss'],
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

//export class TcfSummaryComponent extends SummaryComponent<TCFLandingRequest> {

export class TcfSummaryComponent extends SummaryComponent<TCFLandingRequest> {

  isDataAvailable: boolean;
  showSearchResult: boolean;
  toggleFormSection: boolean;
  donutChartSize: number;
  donutChartthickness: number;
  cars: any;
  customers = [];
  customersBuffer = [];
  bufferSize = 10;
  loading = false;
  input$ = new BehaviorSubject<string>("");
  masterData: TCFMasterModel;
  model: TCFLandingRequest;
  requestModel: TCFListRequest;
  tcfResponse: TCFResponse;

  trafficLightColor = TrafficLightColor;
  userTokenRequest: userTokenRequest;
  user: any;
  userToken: string;
  userType = UserType;
  private currentRoute: Router;

  public bookingSummaryModel: InspectionBookingsummarymodel;

  data: any;
  tcfDetail: any;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  supplierTypeList: any = supplierTypeList;
  tcfSearchtypelst: any = tcfSearchtypelst;
  tcfDatetypelst: any = tcfDatetypelst;
  genericGetRequest: GenericAPIGETRequest;
  genericAPIPOSTRequest: GenericAPIPOSTRequest;
  tcfStage = TCFStage;
  tcfRequestData: TCFRequestData;

  image: any;
  isFilterOpen: boolean;

  constructor(private cdr: ChangeDetectorRef, public validator: Validator, router: Router,
    route: ActivatedRoute, translate: TranslateService, public pathroute: ActivatedRoute,
    toastr: ToastrService, private tcfService: TCFService, public calendar: NgbCalendar,
    private customerService: CustomerService, private supplierService: SupplierService,
    public utility: UtilityService,
    private customerProductService: CustomerProduct, private locationService: LocationService) {

    super(router, validator, route, translate, toastr);
    this.isDataAvailable = true;
    this.showSearchResult = true;
    this.toggleFormSection = false;

    this.validator.isSubmitted = false;
    this.masterData = new TCFMasterModel();
    this.model = new TCFLandingRequest();
    this.tcfResponse = new TCFResponse();
    this.tcfResponse.tcfDetail = new TCFDetail();
    this.requestModel = new TCFListRequest();
    this.tcfRequestData = new TCFRequestData();
    this.genericGetRequest = new GenericAPIGETRequest();

    this.model.statusIds = [];

    //get the current user from the local storage
    if (localStorage.getItem('currentUser'))
      this.user = JSON.parse(localStorage.getItem('currentUser'));
    this.currentRoute = router;
    this.isFilterOpen = true;
    
  }

  //format date to YYYY-MM-DD
  formatDate(dateObject) {
    return dateObject.year + "-" + dateObject.month + "-" + dateObject.day;
  }

  onInit() {

    this.validator.setJSON("tcf/tcf-summary.valid.json");
    this.validator.setModelAsync(() => this.model);

    this.selectedPageSize = PageSizeCommon[0];

    //assign the selected parent data if it comes from the tcf detail page
    this.assignParentData();

    this.initialize();

    //preload the data with the status if it comes from the tcf dashboard
    var pageType = this.pathroute.snapshot.paramMap.get("pagetype");

    if (pageType && pageType == TCFSummaryPageType.FromDashboard.toString()) {
      //this.selectedPageSize = PageSizeCommon[0];
      var taskType = this.pathroute.snapshot.paramMap.get("tasktype");
      if (taskType == TCFTaskType.InProgress.toString()) {
        this.model.statusIds.push(this.tcfStage.InProgress);
        this.GetSearchData();
      }
      else if (taskType == TCFTaskType.Completed.toString()) {
        this.model.statusIds.push(this.tcfStage.Completed);
        this.GetSearchData();
      }
    }



  }

  //assign the selected parent data if it comes from the tcf detail page
  assignParentData() {
    if (this.pathroute.snapshot.queryParams && this.pathroute.snapshot.queryParams.param) {
      var parentModel = JSON.parse(decodeURI(this.pathroute.snapshot.queryParams.param))
      if (parentModel) {
        if (parentModel.supplierIds)
          this.masterData.requestSupplierModel.supplierIds = parentModel.supplierIds;

        if (parentModel.buyerIds)
          this.masterData.requestBuyerModel.buyerIds = parentModel.buyerIds;

        if (parentModel.customerContactIds)
          this.masterData.requestCustomerContactModel.contactIds = parentModel.customerContactIds;

        if (parentModel.countryOriginIds)
          this.masterData.requestCountryModel.countryIds = parentModel.countryOriginIds;

        if (parentModel.countryDestinationIds)
          this.masterData.requestCountryDestinationModel.countryIds = parentModel.countryDestinationIds;
        
        if (parentModel.customerGLCodes){
          this.model.customerGLCodes = parentModel.customerGLCodes;
          this.masterData.requestCustomerModel.customerglCodes = parentModel.customerGLCodes;
        }

        if (parentModel.supplierTypeId)
          this.model.supplierTypeId = parentModel.supplierTypeId;
        }
    }
    else {
      //initialize the objects
      this.objectInitialization();
    }
  }

  //intialize the data
  initialize() {

    //initialize the graph
    this.graphInitialization();
    //get the API Master Data
    this.getCustomerListBySearch();
    this.getSupplierListBySearch();
    this.getBuyerListBySearch();
    this.getCustomerContactListBySearch();
    this.getProductCategoryListBySearch();
    this.getCountryOriginListBySearch();
    this.getCountryDestinationListBySearch();
    //get the TCF Master Data
    this.getTCFStatusList();

  }

  //initialize the graph data
  graphInitialization() {
    if (window.innerWidth > 1400) { // big desktop

      this.donutChartSize = 125;
      this.donutChartthickness = 15;

    } if (window.innerWidth < 450) { // mobile

      this.donutChartSize = 125;
      this.donutChartthickness = 15;

    } else { // else all

      this.donutChartSize = 125;
      this.donutChartthickness = 15;
    }

    document.addEventListener("click", (e) => {
      let popup = document.querySelector('#tcf-download-trigger');

      if ((<HTMLInputElement>e.target).id != 'tcf-download-trigger') {
        // hiding all open popups
        let popupAll = document.querySelectorAll('.extra-action-container');
        popupAll.forEach(ele => {
          ele.classList.remove('open');
        });
      }
    });
  }

  //initialize the object data
  objectInitialization() {
    this.model = new TCFLandingRequest();
    this.selectedPageSize = PageSizeCommon[0];
    this.model.pageSize = 10;
    this.model.index = 1;
    this.model.dateTypeId = 10;
    this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.model.todate = this.calendar.getToday();
    this.GetSearchData();
    this.assignSupplierDetails();
  }

  //get the tcf detail data
  getTCFDetail(rowItem) {
    this.genericGetRequest.requestUrl = config.TCF.tcfLanding + rowItem.tcfId;
    this.genericGetRequest.token = this.userToken;
    this.genericGetRequest.isGenericToken = false;

    this.tcfService.getData(this.genericGetRequest).subscribe(response => {
      this.processTCFDetailResponse(response, rowItem);
    });
  }

  //process the tcf detail response
  processTCFDetailResponse(response, rowItem) {
    if (response && response.status && response.status == TCFDetailsResponse.Success) {
      this.tcfDetail = response.data[0];
      rowItem.tcfDetail = this.tcfDetail;
      rowItem.tcfDetail.productUrl = 'data:image/jpeg;base64,' + rowItem.tcfDetail.productImage;
      setTimeout(() => {
        this.createtcfDetailChart(rowItem);
      }, 100);
    }
    else if (response && response.status && response.status == TCFDetailsResponse.NotFound) {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.LBL_TCF_DETAIL_NOT_FOUND');
    }
  }

  //get the tcf status list
  getTCFStatusList() {
    this.genericGetRequest.requestUrl = config.TCF.tcfStatusList;
    this.genericGetRequest.isGenericToken=true;
    this.masterData.statusLoading = true;
    this.tcfService.getData(this.genericGetRequest).subscribe(data => {
      this.processTCFStatusResponse(data);
    },
      error => {
        this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.statusLoading = false;
      });
  }

  //process the tcf status response
  processTCFStatusResponse(data) {
    if (data && data.status && data.status == TCFStageResponse.Success) {
      this.masterData.statusList = data.data;
      this.masterData.statusLoading = false;
    }
    else if (data && data.status && data.status == TCFStageResponse.NotFound) {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.LBL_TCF_STATUS_NOT_FOUND');
      this.masterData.statusLoading = false;
    }
    else {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
      this.masterData.statusLoading = false;
    }
  }

  //get the tcf buyer department list
  getBuyerDepartmentList() {
    this.masterData.buyerdepartmentLoading = true;
    this.genericGetRequest.requestUrl = config.TCF.tcfDepartmentList;
    this.genericGetRequest.token = this.userToken;
    this.genericGetRequest.isGenericToken = false;
    this.tcfService.getData(this.genericGetRequest).subscribe(data => {
      this.processBuyerDepartmentResponse(data);
    },
      error => {
        this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.buyerdepartmentLoading = false;
      });
  }

  //process the buyer department response
  processBuyerDepartmentResponse(data) {
    if (data && data.status && data.status == TCFDepartmentResponse.Success) {
      this.masterData.buyerDepartmentList = data.data;
      this.masterData.buyerdepartmentLoading = false;
    }
    else if (data && data.status && data.status == TCFDepartmentResponse.NotFound) {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.LBL_TCF_BUYER_DEPT_NOT_FOUND');
      this.masterData.buyerdepartmentLoading = false;
    }
    else {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
      this.masterData.buyerdepartmentLoading = false;
    }
  }

  getData(): void {
    this.GetSearchData();
  }

  getPathDetails(): string {
    return '';
  }

  //assign the tcf list request data
  mapTCFListRequest() {
    this.requestModel = new TCFListRequest();
    if (this.model.searchTypeId)
      this.requestModel.searchTypeId = this.model.searchTypeId;
    this.requestModel.searchTypeText = this.model.searchTypeText;
    if (this.model.statusIds)
      this.requestModel.statusIds = this.model.statusIds;
    this.requestModel.customerIds = this.model.customerGLCodes;
    if (this.model.supplierIds)
      this.requestModel.supplierIds = this.model.supplierIds;
    if (this.model.buyerIds)
      this.requestModel.buyerIds = this.model.buyerIds;
    if (this.model.customerContactIds)
      this.requestModel.customerContactIds = this.model.customerContactIds;
    if (this.model.buyerDepartmentIds)
      this.requestModel.buyerDepartmentIds = this.model.buyerDepartmentIds;
    if (this.model.productCategoryIds)
      this.requestModel.productCategoryIds = this.model.productCategoryIds;
    if (this.model.productSubCategoryIds)
      this.requestModel.productSubCategoryIds = this.model.productSubCategoryIds;
    if (this.model.countryOriginIds)
      this.requestModel.countryOriginIds = this.model.countryOriginIds;
    if (this.model.countryDestinationIds)
      this.requestModel.countryDestinationIds = this.model.countryDestinationIds;
    if (this.model.dateTypeId) {
      if (this.model.dateTypeId == 7)
        this.requestModel.dateTypeId = 1;
      else if (this.model.dateTypeId == 8)
        this.requestModel.dateTypeId = 2;
      else if (this.model.dateTypeId == 9)
        this.requestModel.dateTypeId = 3;
      else if (this.model.dateTypeId == 10)
        this.requestModel.dateTypeId = 4;
    }
    if (this.model.fromdate)
      this.requestModel.fromDate = this.formatTCFDate(this.model.fromdate);
    if (this.model.todate)
      this.requestModel.toDate = this.formatTCFDate(this.model.todate);
    if (this.model.pictureUploaded)
      this.requestModel.pictureUploaded = this.model.pictureUploaded;
    if (this.model.pageSize)
      this.requestModel.pageSize = this.model.pageSize;
    if (this.model.index)
      this.requestModel.index = this.model.index;
  }

  formatTCFDate(dateValue) {
    return dateValue.year + "-" + dateValue.month + "-" + dateValue.day;
  }

  //map the tcf list response
  mapTCFListResponse(resultData) {
    this.tcfResponse.tcfList = [];
    if (resultData) {

      this.model.totalCount = resultData.total_records;
      this.tcfResponse.tcfList = resultData.data;
      this.getCustomerRefShortName();
      this.tcfResponse.tcfDetail = new TCFDetail();

      if (resultData.data.length == 0)
        this.model.noFound = true;
    }

    console.log(this.tcfResponse);
  }

  getCustomerRefShortName() {
    this.tcfResponse.tcfList.forEach(element => {
      if (element.customerRefName)
        element.customrRefShortName = element.customerRefName.slice(0, 15);
    });
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

  //validate the form
  formValid(): boolean {

    let isOk = this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())

    return isOk;
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
    var request = new GenericAPIPOSTRequest();
    request.requestUrl = requestUrl;
    request.requestData = requestInput;
    request.token = token;
    return request;
  }

  getTCFListDataWithUserToken(request) {
    this.masterData.searchloading = true;
    this.tcfService.postData(request).subscribe(data => {
      this.processUserTokenResponse(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.searchloading = false;
      });
  }

  processUserTokenResponse(data) {
    if (data && data.result) {
      //parse ther response result to json
      var resultData = JSON.parse(data.result);
      //if response result is success and then assign the user token(this.userToken will be used for all subsequent requests)
      if (resultData && resultData.status && resultData.status == "1"
        && resultData.data && resultData.data.authorization) {

        this.userToken = resultData.data.authorization;
        this.getBuyerDepartmentList();
        this.getTCFSearchData();

      }
      else {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
        this.masterData.searchloading = false;
      }
    }
    else {
      this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
      this.masterData.searchloading = false;
    }
  }

  //get the tcf list data
  getTCFSearchData() {
    this.mapTCFListRequest();
    var request = this.generatePostRequest(config.TCF.tcfLanding, this.requestModel, this.userToken);
    request.isGenericToken = false;
    this.masterData.searchloading = true;
    this.tcfResponse.tcfList = [];
    this.tcfService.postData(request).subscribe(data => {
      this.processTCFListResponse(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.searchloading = false;
      });
  }

  //process the tcf list response
  processTCFListResponse(data) {
    if (data && data.result) {
      var resultData = JSON.parse(data.result);
      if (resultData && resultData.status && resultData.status == TCFListResponse.Success) {
        this.mapTCFListResponse(resultData);
      }
      else if (resultData && resultData.status && resultData.status == TCFListResponse.NotFound) {
        this.model.noFound = true;
      }
      else {
        this.error = resultData;
      }
      this.masterData.searchloading = false;
    }
    else {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
      this.masterData.searchloading = false;
    }
  }

  GetSearchData() {
    this.searchTCFListData();
  }

  //search the tcf list data
  searchTCFListData() {
    //make the user token request
    this.generateUserTokenRequest();
    //make the user token post request
    var request = this.generatePostRequest(config.TCF.userAuthentication, this.userTokenRequest, "");
    this.getTCFListDataWithUserToken(request);
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  //fetch the first 10 customers on load
  getCustomerListBySearch() {
    if (this.model.customerGLCodes)
       this.masterData.requestCustomerModel.customerglCodes = this.model.customerGLCodes;
		
       this.masterData.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSource(this.masterData.requestCustomerModel, term)
        : this.customerService.getCustomerDataSource(this.masterData.requestCustomerModel)
          .pipe(
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
    this.masterData.requestCustomerModel.serviceId = APIService.Tcf;
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestCustomerModel.searchText = this.masterData.customerInput.getValue();
      this.masterData.requestCustomerModel.skip = this.masterData.customerList.length;
    }
    this.masterData.customerLoading = true;
    this.customerService.getCustomerDataSource(this.masterData.requestCustomerModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.customerList = this.masterData.customerList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.requestCustomerModel = new CustomerDataSourceRequest();
        this.masterData.customerLoading = false;
      }),
      error => {
        this.masterData.customerLoading = false;
      };
  }

  changeCustomerData() {

    this.model.supplierIds = [];
    this.model.buyerIds = [];
    this.model.customerContactIds = [];

    this.masterData.supplierList = [];
    this.masterData.buyerList = [];
    this.masterData.customerContactList = [];

    var isDefaultLoad = false;

    if (this.model.customerGLCodes && this.model.customerGLCodes.length > 0)
      isDefaultLoad = true;

    this.getSupplierData(isDefaultLoad);
    this.getBuyerData(isDefaultLoad);
    this.getCustomerContactData(isDefaultLoad);
  }

  clearCustomerSelection() {
    this.model.customerGLCodes = [];

    this.masterData.requestCustomerModel = new CustomerDataSourceRequest();
    this.masterData.customerList = [];
    this.model.supplierIds = [];
    this.model.buyerIds = [];
    this.model.customerContactIds = [];
    this.masterData.customerContactList = [];
    this.masterData.supplierList = [];
    this.masterData.buyerList = [];
    this.getCustomerData(false);
    this.getSupplierData(false);
    this.getBuyerData(false);
  }

  //fetch the first 10 customers on load
  getSupplierListBySearch() {
    if (this.model.supplierTypeId == SearchType.SupplierCode)
      this.masterData.requestSupplierModel.supSearchTypeId = SearchType.SupplierCode;
    else
      this.masterData.requestSupplierModel.supSearchTypeId = SearchType.SupplierName;

    this.masterData.requestSupplierModel.serviceId = APIService.Tcf;
    if (this.model.customerGLCodes)
      //this.masterData.requestSupplierModel.customerIds = this.model.customerIds;
      this.masterData.requestSupplierModel.customerglCodes = this.model.customerGLCodes;
    this.masterData.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.supplierLoading = true),
      switchMap(term => term
        ? this.supplierService.getSupplierDataSource(this.masterData.requestSupplierModel, APIService.Tcf, term)
        : this.supplierService.getSupplierDataSource(this.masterData.requestSupplierModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.supplierLoading = false))
      ))
      .subscribe(data => {
        this.masterData.supplierList = data;
        this.masterData.supplierLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getSupplierData(isDefaultLoad: boolean) {
    this.masterData.requestSupplierModel.serviceId = APIService.Tcf;
    if (this.model.supplierTypeId == SearchType.SupplierCode)
      this.masterData.requestSupplierModel.supSearchTypeId = SearchType.SupplierCode;
    else
      this.masterData.requestSupplierModel.supSearchTypeId = SearchType.SupplierName;
    /*  if (this.model.customerIds)
       this.masterData.requestSupplierModel.customerIds = this.model.customerIds; */
    if (this.model.customerGLCodes)
      this.masterData.requestSupplierModel.customerglCodes = this.model.customerGLCodes;
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestSupplierModel.searchText = this.masterData.supplierInput.getValue();
      this.masterData.requestSupplierModel.skip = this.masterData.supplierList.length;
    }
    this.masterData.supplierLoading = true;
    this.supplierService.getSupplierDataSource(this.masterData.requestSupplierModel, APIService.Tcf).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.supplierList = this.masterData.supplierList.concat(data);
        }
        // if (isDefaultLoad)
        //   this.masterData.requestSupplierModel = new SupplierDataSourceRequest();
        this.masterData.supplierLoading = false;
      }),
      error => {
        this.masterData.supplierLoading = false;
      };
  }

  clearSupplierSelection() {

    this.masterData.requestSupplierModel = new SupplierDataSourceRequest();
    this.masterData.supplierList = [];
    this.getSupplierData(false);
  }

  //fetch the first 10 buyers on load
  getBuyerListBySearch() {

    /* if (this.model.customerIds)
      this.masterData.requestBuyerModel.customerIds = this.model.customerIds; */
    if (this.model.customerGLCodes)
      this.masterData.requestBuyerModel.customerglCodes = this.model.customerGLCodes;

    this.masterData.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.buyerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerBuyerDataSource(this.masterData.requestBuyerModel, APIService.Tcf, term)
        : this.customerService.getCustomerBuyerDataSource(this.masterData.requestBuyerModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.buyerLoading = false))
      ))
      .subscribe(data => {
        this.masterData.buyerList = data;
        this.masterData.buyerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getBuyerData(isDefaultLoad: boolean) {
    /*  if (this.model.customerIds)
       this.masterData.requestBuyerModel.customerIds = this.model.customerIds; */
    if (this.model.customerGLCodes)
      this.masterData.requestBuyerModel.customerglCodes = this.model.customerGLCodes;
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestBuyerModel.searchText = this.masterData.buyerInput.getValue();
      this.masterData.requestBuyerModel.skip = this.masterData.buyerList.length;
    }
    this.masterData.buyerLoading = true;
    this.customerService.getCustomerBuyerDataSource(this.masterData.requestBuyerModel, APIService.Tcf).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.buyerList = this.masterData.buyerList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.requestBuyerModel = new BuyerDataSourceRequest();
        this.masterData.buyerLoading = false;
      }),
      error => {
        this.masterData.buyerLoading = false;
      };
  }

  clearBuyerSelection() {
    this.masterData.requestBuyerModel = new BuyerDataSourceRequest();
    this.masterData.buyerList = [];
    this.getBuyerData(false);
  }


  //fetch the first 10 buyers on load
  getCustomerContactListBySearch() {

    if (this.model.customerGLCodes)
      this.masterData.requestCustomerContactModel.customerglCodes = this.model.customerGLCodes;
    this.masterData.customerContactInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.customerContactLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerContactDataSource(this.masterData.requestCustomerContactModel, APIService.Tcf, term)
        : this.customerService.getCustomerContactDataSource(this.masterData.requestCustomerContactModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.customerContactLoading = false))
      ))
      .subscribe(data => {
        this.masterData.customerContactList = data;
        this.masterData.customerContactLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerContactData(isDefaultLoad: boolean) {
    if (this.model.customerGLCodes)
      this.masterData.requestCustomerContactModel.customerglCodes = this.model.customerGLCodes;
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestCustomerContactModel.searchText = this.masterData.customerContactInput.getValue();
      this.masterData.requestCustomerContactModel.skip = this.masterData.customerContactList.length;
    }
    this.masterData.customerContactLoading = true;
    this.customerService.getCustomerContactDataSource(this.masterData.requestCustomerContactModel, APIService.Tcf).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.customerContactList = this.masterData.customerContactList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.requestCustomerContactModel = new CustomerContactSourceRequest();
        this.masterData.customerContactLoading = false;
      }),
      error => {
        this.masterData.customerContactLoading = false;
      };
  }

  clearCustomerContactSelection() {
    this.masterData.requestCustomerContactModel = new CustomerContactSourceRequest();
    this.masterData.customerContactList = [];
    this.getCustomerContactData(false);
  }

  //fetch the first 10 buyers on load
  getProductCategoryListBySearch() {
    this.masterData.requestProductCategoryModel.serviceId = APIService.Tcf;
    this.masterData.productCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.productCategoryLoading = true),
      switchMap(term => term
        ? this.customerProductService.getProductCategoryDataSource(this.masterData.requestProductCategoryModel, term)
        : this.customerProductService.getProductCategoryDataSource(this.masterData.requestProductCategoryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.productCategoryLoading = false))
      ))
      .subscribe(data => {
        this.masterData.productCategoryList = data;
        this.masterData.productCategoryLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getProductCategoryData(isDefaultLoad: boolean) {
    this.masterData.requestProductCategoryModel.serviceId = APIService.Tcf;
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestProductCategoryModel.searchText = this.masterData.productCategoryInput.getValue();
      this.masterData.requestProductCategoryModel.skip = this.masterData.productCategoryList.length;
    }
    this.masterData.productCategoryLoading = true;
    this.customerProductService.getProductCategoryDataSource(this.masterData.requestProductCategoryModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.productCategoryList = this.masterData.productCategoryList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.requestProductCategoryModel = new ProductCategorySourceRequest();
        this.masterData.productCategoryLoading = false;
      }),
      error => {
        this.masterData.productCategoryLoading = false;
      };
  }

  //change the product category data
  changeProductCategoryData() {

    if (this.model.productCategoryIds) {
      this.masterData.productSubCategoryList = [];

      this.getProductSubCategoryData(true);
    }
  }

  //clear the product category data
  clearProductCategorySelection() {
    this.masterData.requestProductCategoryModel = new ProductCategorySourceRequest();
    this.masterData.productCategoryList = [];
    this.getProductCategoryData(false);
  }

  //fetch the first 10 buyers on load
  getProductSubCategoryListBySearch() {

    this.masterData.requestProductSubCategoryModel.serviceId = APIService.Tcf;
    if (this.model.productCategoryIds)
      this.masterData.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
    this.masterData.productSubCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.productSubCategoryLoading = true),
      switchMap(term => term
        ? this.customerProductService.getProductSubCategoryDataSource(this.masterData.requestProductSubCategoryModel, term)
        : this.customerProductService.getProductSubCategoryDataSource(this.masterData.requestProductSubCategoryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.productSubCategoryLoading = false))
      ))
      .subscribe(data => {
        this.masterData.productSubCategoryList = data;
        this.masterData.productSubCategoryLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getProductSubCategoryData(isDefaultLoad: boolean) {
    this.masterData.requestProductSubCategoryModel.serviceId = APIService.Tcf;
    if (this.model.productCategoryIds)
      this.masterData.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestProductSubCategoryModel.searchText = this.masterData.productSubCategoryInput.getValue();
      this.masterData.requestProductSubCategoryModel.skip = this.masterData.productSubCategoryList.length;
    }
    this.masterData.productSubCategoryLoading = true;
    this.customerProductService.getProductSubCategoryDataSource(this.masterData.requestProductSubCategoryModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.productSubCategoryList = this.masterData.productSubCategoryList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();
        this.masterData.productSubCategoryLoading = false;
      }),
      error => {
        this.masterData.productSubCategoryLoading = false;
      };
  }

  clearProductSubCategorySelection() {
    this.masterData.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();
    this.masterData.productSubCategoryList = [];
    this.getProductSubCategoryData(false);
  }

  //fetch the first 10 buyers on load
  getCountryOriginListBySearch() {

    this.masterData.countryOriginInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.countryOriginLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel, term)
        : this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.countryOriginLoading = false))
      ))
      .subscribe(data => {
        this.masterData.countryOriginList = data;
        this.masterData.countryOriginLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCountryOriginData(isDefaultLoad: boolean) {
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestCountryModel.searchText = this.masterData.countryOriginInput.getValue();
      this.masterData.requestCountryModel.skip = this.masterData.countryOriginList.length;
    }
    this.masterData.countryOriginLoading = true;
    this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.countryOriginList = this.masterData.countryOriginList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.requestCountryModel = new CountryDataSourceRequest();
        this.masterData.countryOriginLoading = false;
      }),
      error => {
        this.masterData.countryOriginLoading = false;
      };
  }

  clearCountryOriginSelection() {
    this.masterData.requestCountryModel = new CountryDataSourceRequest();
    this.masterData.countryOriginList = [];
    this.getCountryOriginData(false);
  }

  //fetch the first 10 buyers on load
  getCountryDestinationListBySearch() {

    this.masterData.countryDestinationInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.countryDestinationLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.masterData.requestCountryDestinationModel, term)
        : this.locationService.getCountryDataSourceList(this.masterData.requestCountryDestinationModel, term)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.countryDestinationLoading = false))
      ))
      .subscribe(data => {
        this.masterData.countryDestinationList = data;
        this.masterData.countryDestinationLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCountryDestinationData(isDefaultLoad: boolean) {
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.requestCountryDestinationModel.searchText = this.masterData.countryDestinationInput.getValue();
      this.masterData.requestCountryDestinationModel.skip = this.masterData.countryDestinationList.length;
    }
    this.masterData.countryDestinationLoading = true;
    this.locationService.getCountryDataSourceList(this.masterData.requestCountryDestinationModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.countryDestinationList = this.masterData.countryDestinationList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.requestCountryDestinationModel = new CountryDataSourceRequest();
        this.masterData.countryDestinationLoading = false;
      }),
      error => {
        this.masterData.countryOriginLoading = false;
      };
  }

  clearCountryDestinationSelection() {
    this.masterData.requestCountryDestinationModel = new CountryDataSourceRequest();
    this.masterData.countryDestinationList = [];
    this.getCountryDestinationData(false);
  }

  //create the tcf detail chart
  createtcfDetailChart(rowItem) {
    // donut chart settings
    if (rowItem.tcfDetail.documentsReceived || rowItem.tcfDetail.documentsReceived || rowItem.tcfDetail.documentsReceived) {
      if (!rowItem.tcfDetail.documentsReceived)
        rowItem.tcfDetail.documentsReceived = 0;
      if (!rowItem.tcfDetail.documentsRejected)
        rowItem.tcfDetail.documentsRejected = 0;
      if (!rowItem.tcfDetail.documentsUnderReview)
        rowItem.tcfDetail.documentsUnderReview = 0;

      let analyticsChart1 = c3.generate({
        bindto: '#analytics-chart' + rowItem.tcfId,
        size: {
          height: this.donutChartSize,
          width: this.donutChartSize
        },
        data: {
          columns: [
            ['Received', rowItem.tcfDetail.documentsReceived],
            ['Rejected', rowItem.tcfDetail.documentsRejected],
            ['Waiting', rowItem.tcfDetail.documentsWaiting],
            ['UnderReview', rowItem.tcfDetail.documentsUnderReview],
          ],
          type: 'donut',
          labels: false,
          colors: {
            Received: 'rgb(88, 190, 0)',
            Rejected: 'rgb(229, 81, 81)',
            UnderReview: 'rgb(243, 144, 15)',
            Waiting: 'rgba(36, 159, 230, 0.603)'
          },
          names: {
            Pass: 'Received ' + rowItem.tcfDetail.documentsReceived,
            Fail: 'Rejected ' + rowItem.tcfDetail.documentsRejected,
            Waiting: 'Waiting ' + rowItem.tcfDetail.documentsWaiting,
            Hold: 'UnderReview ' + rowItem.tcfDetail.documentsUnderReview
          }
        },
        donut: {
          title: rowItem.tcfDetail.documentsReceived + rowItem.tcfDetail.documentsRejected + rowItem.tcfDetail.documentsUnderReview,
          label: {
            threshold: 0.03,
            format: function (value, ratio, id) {
              return null;
            }
          },
          width: this.donutChartthickness
        },
        legend: {
          show: false
        },
      });

      this.cdr.detectChanges();
    }

  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  toggleExpandRow(event, index, rowItem) {

    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="tcfDetail' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getTCFDetail(rowItem);
    } else {
      event.target.innerHTML = '+';
    }
  }

  reset() {
    this.model = new TCFLandingRequest();
    this.tcfResponse.tcfList = [];
    this.initialize();
  }

  SetSearchTypemodel(searchtype) {
    this.model.searchTypeId = searchtype;
  }

  SetSearchDatetype(searchdatetype) {

    this.model.dateTypeId = searchdatetype;
  }

  redirectToTCFDetail(tcfId) {
    var data = Object.keys(this.model);
    var tcfDetailUrl = '/tcfdetail/tcf-detail';
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${tcfDetailUrl}/${tcfId}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  //get or download the TCF Report data
  getTCFReportData(tcfId) {
    this.genericGetRequest.requestUrl = config.TCF.downloadTCFReport + tcfId;
    this.genericGetRequest.token = this.userToken;
    this.genericGetRequest.isGenericToken = false;
    this.masterData.isProcessLoader = true;
    this.tcfService.getData(this.genericGetRequest).subscribe(data => {
      this.processTCFReportDataResponse(data);
    },
      error => {
        this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.isProcessLoader = false;
      });
  }

  //process the tcf report data download response
  processTCFReportDataResponse(data) {
    if (data && data.status && data.status == TCFReportResponse.Success) {
      this.downloadFile(data.data.attachment, data.data.mimetype, "pdf",data.data.filename);
      this.masterData.isProcessLoader = false;
    }
    else if (data && data.status && data.status == TCFReportResponse.NotFound) {
      this.showWarning('TCF_LIST.LBL_ERROR', 'TCF_LIST.NO_DOCUMENTS_FOUND');
      this.masterData.isProcessLoader = false;
    }
    else {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
      this.masterData.isProcessLoader = false;
    }
  }

  base64ToBlobFormat(base64, mimeType) {
    const binaryString = window.atob(base64);
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; ++i) {
      bytes[i] = binaryString.charCodeAt(i);
    }
    return new Blob([bytes], { type: mimeType });
  }

  downloadFile(data, mimetype, pathextension, fileName) {
    var blobData = this.base64ToBlobFormat(data, mimetype);
    //const blob = new Blob([blobData], { type: mimetype });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blobData, fileName);
    }
    else {
      const url = window.URL.createObjectURL(blobData);
      //window.open(url);
      var a = document.createElement('a');
      a.href = url;
      a.download = fileName;//url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }


  //gets or download the TCF Validated Document
  getTCFValidDocument(tcfId) {
    this.genericGetRequest.requestUrl = config.TCF.downloadAllValidDocument + tcfId;
    this.genericGetRequest.token = this.userToken;
    this.genericGetRequest.isGenericToken = false;

    this.masterData.isProcessLoader = true;
    this.tcfService.getData(this.genericGetRequest).subscribe(data => {
      this.processTCFValidDocument(data);
    },
      error => {
        this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.isProcessLoader = false;
      });
  }

  //process the tcf validate document
  processTCFValidDocument(data) {
    if (data && data.status && data.status == TCFValidDocumentResponse.Success) {
      this.downloadFile(data.data.attachment, data.data.mimetype, "zip", data.data.filename);
      this.masterData.isProcessLoader = false;
    }
    else if (data && data.status && data.status == TCFValidDocumentResponse.NotFound) {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.NO_DOCUMENTS_FOUND');
      this.masterData.isProcessLoader = false;
    }
    else {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
      this.masterData.isProcessLoader = false;
    }
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

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  changeSupplierType() {
    this.masterData.supplierLoading = true;
    this.masterData.supplierList = [];
    this.model.supplierIds = [];
    this.getSupplierListBySearch();
  }

  assignSupplierDetails() {
    this.model.supplierTypeId = SearchType.SupplierName;
  }
}

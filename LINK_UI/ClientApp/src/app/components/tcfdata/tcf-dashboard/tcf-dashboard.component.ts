import { Component, OnInit } from '@angular/core';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { trigger, state, style, transition, animate } from '@angular/animations';
import { RejectedAnalysisData, RejectedAnalysisResponse, TCFInProgressResponse, ScopeDashboardData, StatusDashBoardData, StatusDashboardResponse, TCFDashBoardMaster, TCFDashboardResponse, TCFDocumentRequestData, TCFFilterRequest, TCFScopeDashboardResponse, TCFTerm, TCFTermEnum } from 'src/app/_Models/tcf/tcfdashboard.model';
import { TCFLandingRequest, userTokenRequest } from 'src/app/_Models/tcf/tcflanding.model';
import { APIService, SearchType, supplierTypeList, tcfDatetypelst, tcfSearchtypelst, TCFTermList, TermList, UserType } from '../../common/static-data-common';
import { TCFStage, TCFStageResponse, TCFSummaryPageType, TCFTaskType } from 'src/app/_Models/tcf/tcfcommon.model';
import { TCFService } from 'src/app/_Services/tcfdata/tcf.service';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { GenericAPIGETRequest, GenericAPIPOSTRequest } from 'src/app/_Models/genericapi/genericapirequest.model';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { BuyerDataSourceRequest, CountryDataSourceRequest, CustomerContactSourceRequest, CustomerDataSourceRequest, ProductCategorySourceRequest, ProductSubCategorySourceRequest, SupplierDataSourceRequest } from 'src/app/_Models/common/common.model';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { Validator } from '../../common';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { LocationService } from 'src/app/_Services/location/location.service';

const config = require("src/assets/appconfig/appconfig.json");

am4core.useTheme(am4themes_animated);

@Component({
  selector: 'app-tcf-dashboard',
  templateUrl: './tcf-dashboard.component.html',
  styleUrls: ['./tcf-dashboard.component.scss'],
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
export class TcfDashboardComponent implements OnInit {

  bar1Config: any;
  bar2Config: any;
  isTablet: boolean;
  isMobile: boolean;
  isFilterOpen: boolean;
  tcfDocumentRequestData: TCFDocumentRequestData;
  userTokenRequest: userTokenRequest;
  user: any;
  userType = UserType;
  tcfDashboardMaster: TCFDashBoardMaster;
  tcfFilterRequest: TCFFilterRequest;
  tcfDashboardResponse: TCFDashboardResponse;
  tcfStage = TCFStage;
  tcfTaskType = TCFTaskType;
  tcfLandingRequest: TCFLandingRequest;
  model: TCFLandingRequest;
  genericGetRequest: GenericAPIGETRequest;
  tcfSearchtypelst: any = tcfSearchtypelst;
  tcfDatetypelst: any = tcfDatetypelst;
  supplierTypeList: any = supplierTypeList;
  toggleFormSection: boolean;
  clusteredRowchart: any;

  constructor(private tcfService: TCFService, public utility: UtilityService,public calendar: NgbCalendar, private router: Router,
    private supplierService: SupplierService, public validator: Validator, private customerService: CustomerService,
    private customerProductService: CustomerProduct, private locationService: LocationService,
    protected translate?: TranslateService,
    protected toastr?: ToastrService) {
    this.toggleFormSection = false;
    //intialize the objects
    this.objectInitialization();
    //add the amchart license
    am4core.addLicense("CH238479116");
    //get the current user from the local storage
    if (localStorage.getItem('currentUser'))
      this.user = JSON.parse(localStorage.getItem('currentUser'));
  }

  //object initialization
  objectInitialization() {
    this.tcfDocumentRequestData = new TCFDocumentRequestData();
    this.tcfDashboardMaster = new TCFDashBoardMaster();
    this.tcfDashboardResponse = new TCFDashboardResponse();
    this.tcfFilterRequest = new TCFFilterRequest();
    this.tcfLandingRequest = new TCFLandingRequest();
    this.model = new TCFLandingRequest();
    this.genericGetRequest = new GenericAPIGETRequest();
    this.tcfLandingRequest.statusIds = [];
    this.assignSupplierDetails();
  }

  //Initialization
  ngOnInit() {
    this.validator.setJSON("tcf/tcf-dashboard.valid.json");
    this.validator.setModelAsync(() => this.model);
    //initialize the graph
    this.graphInitialization();
    //get the dashboard data
    this.getDashboardData();

    this.getSupplierListBySearch();

    //get the TCF Master Data
    this.getTCFTermList();
    this.getTCFStatusList();
    this.getCustomerListBySearch();
    this.getBuyerListBySearch();
    this.getCustomerContactListBySearch();
    this.getProductCategoryListBySearch();
    this.getCountryOriginListBySearch();
    this.getCountryDestinationListBySearch();
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

  //toggling the filter
  toggleFilter() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  renderProgressBar() {
    let weekWidth = document.querySelector('.progress-header .head').clientWidth;
    let dayWidth = weekWidth / 6;

    // set the bar width variables on the basis on number of days
    // eg. bar days = 6 days
    // bar width = bar days * dayWidth

    // bar 2 offset will be width of bar 1
    // eg. bar 2 marginLeft = bar1Width
  }

  /**
   * Function to render pie chart
   * @param {[type]} container [description]
   */
  renderPieChart(container, data) {

    // Create chart instance
    var chart = am4core.create(container, am4charts.PieChart);

    var tcfValidateCount = 0;
    var tcfPendingCount = 0;
    var tcfUnderReviewCount = 0;
    var tcfRejectedCount = 0;

    if (data && data.countValidate)
      tcfValidateCount = data.countValidate;
    if (data && data.countPending)
      tcfPendingCount = data.countPending;
    if (data && data.countReject)
      tcfRejectedCount = data.countReject;
    if (data && data.countReview)
      tcfUnderReviewCount = data.countReview;

    // Add data
    chart.data = [
      { "sector": "Validated", "size": tcfValidateCount, "color": am4core.color("#24c11e") },
      { "sector": "Pending", "size": tcfPendingCount, "color": am4core.color("#9eb9ce") },
      { "sector": "UnderReview", "size": tcfUnderReviewCount, "color": am4core.color("#ff5626") },
      { "sector": "Rejected", "size": tcfRejectedCount, "color": am4core.color("#f81538") }
    ];

    chart.innerRadius = 45;

    // Add and configure Series
    var pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "size";
    pieSeries.dataFields.category = "sector";
    pieSeries.labels.template.disabled = true;
    pieSeries.slices.template.propertyFields.fill = "color";

    pieSeries.tooltip.autoTextColor = false;
    pieSeries.tooltip.label.fill = am4core.color("#FFFFFF");
  }

  /**
   * Function to render line chart
   */
  renderLineChart() {
    let chart = am4core.create("linechartdiv", am4charts.XYChart);

    // Add data
    chart.data = [
      { date: new Date(2019, 1), value2: 48, value3: 51, value4: 42 },
      { date: new Date(2019, 2), value2: 51, value3: 27, value4: 72 },
      { date: new Date(2019, 3), value2: 58, value3: 82, value4: 15 },
      { date: new Date(2019, 4), value2: 53, value3: 40, value4: 49 },
      { date: new Date(2019, 5), value2: 44, value3: 28, value4: 34 },
      { date: new Date(2019, 6), value2: 42, value3: 61, value4: 62 },
      { date: new Date(2019, 7), value2: 55, value3: 92, value4: 29 },
      { date: new Date(2019, 8), value2: 53, value3: 40, value4: 49 },
      { date: new Date(2019, 9), value2: 44, value3: 28, value4: 34 },
      { date: new Date(2019, 10), value2: 42, value3: 61, value4: 62 },
      { date: new Date(2019, 11), value2: 55, value3: 92, value4: 29 },
      { date: new Date(2019, 11), value2: 55, value3: 92, value4: 29 }
    ]

    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.DateAxis());
    categoryAxis.dataFields.date = "date";
    categoryAxis.renderer.minGridDistance = 10;
    categoryAxis.renderer.labels.template.fontSize = 8;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minGridDistance = 20;
    valueAxis.renderer.labels.template.fontSize = 8;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    // Create series
    let series3 = chart.series.push(new am4charts.LineSeries());
    series3.legendSettings.valueText = "[bold]{valueY.close}[/] ({valueY.close.formatNumber('#.0')}%)";
    series3.stroke = am4core.color("#d9dcfb");
    series3.dataFields.valueY = "value4";
    series3.dataFields.dateX = "date";
    series3.name = 'value3';
    series3.strokeWidth = 2;
    series3.tooltipText = "{dateX}: [b]{valueY}[/]";

    series3.tooltip.autoTextColor = false;
    series3.tooltip.label.fill = am4core.color("#FFFFFF");

    chart.padding(20, 0, 20, 0);
    chart.cursor = new am4charts.XYCursor();
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

  getDashboardData() {
    this.tcfDashboardMaster.tcfStatusNotFound=false;
    this.tcfDashboardMaster.tcfRejectedAnalysisNotFound=false;
    this.tcfDashboardMaster.tcfScopeDashboardNotFound=false;
    this.tcfDashboardMaster.tcfInProgressNotFound =false;
    //make the user token request
    this.generateUserTokenRequest();
    //make the user token post request
    var request = this.generatePostRequest(config.TCF.userAuthentication, this.userTokenRequest, "");
    this.getDashboardDatatDataWithUserToken(request);
  }

  //get the dashboard data with the user token
  getDashboardDatatDataWithUserToken(request) {
    this.tcfDashboardMaster.tokenLoading = true;
    this.tcfService.postData(request).subscribe(data => {
      this.processUserTokenResponse(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.tcfDashboardMaster.tokenLoading = false;
      });
  }

  processUserTokenResponse(data) {
    if (data && data.result) {
      //parse ther response result to json
      var resultData = JSON.parse(data.result);
      //if response result is success and then assign the user token(this.userToken will be used for all subsequent requests)
      if (resultData && resultData.status && resultData.status == "1"
        && resultData.data && resultData.data.authorization) {

        this.tcfDashboardMaster.userToken = resultData.data.authorization;
        this.getStatusDashBoardData();
        this.getRejectedAnalysisData();
        this.getTCFInProgress();
        this.getSupplierBoard();
        this.getTCFScope();
      }
      else {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
      }
      this.tcfDashboardMaster.tokenLoading = false;
    }
    else {
      this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
      this.tcfDashboardMaster.tokenLoading = false;
    }
  }

  //get the status dashboard data
  getStatusDashBoardData() {
    this.mapFilterRequest();
    var request = this.generatePostRequest(config.TCF.dashBoardStatusList, this.tcfFilterRequest, this.tcfDashboardMaster.userToken);
    request.isGenericToken = false;
    //var request = this.createTCFListSearchRequest();
    this.tcfDashboardMaster.tcfStatusLoading = true;
    //this.tcfResponse.tcfList = [];
    this.tcfService.postData(request).subscribe(data => {
      this.processStatusDashboardData(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.tcfDashboardMaster.tcfStatusLoading = false;
      });
  }

  //process the status dashboard data
  processStatusDashboardData(data) {
    if (data && data.result) {
      var resultData = JSON.parse(data.result);
      this.tcfDashboardResponse.statusDashBoardData.inProgressCount = 0;
      this.tcfDashboardResponse.statusDashBoardData.completedCount = 0;
      this.tcfDashboardResponse.statusDashBoardData.finlizedCount = 0;
      if (resultData && resultData.status && resultData.status == StatusDashboardResponse.Success) {
        this.mapTCFStatusResponse(resultData);
        this.tcfDashboardMaster.tcfStatusLoading = false;
      }
      else if (resultData && resultData.status && resultData.status == StatusDashboardResponse.NotFound) {
        this.tcfDashboardMaster.tcfStatusNotFound = true;
        this.tcfDashboardMaster.tcfStatusLoading = false;
      }
      else {
        this.tcfDashboardMaster.tcfStatusNotFound = true;
        this.tcfDashboardMaster.tcfStatusLoading = false;
      }
      this.tcfDashboardMaster.tcfStatusLoading = false;
    }
    else {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
      this.tcfDashboardMaster.tcfStatusLoading = false;
    }
  }

  //map the tcf status response
  mapTCFStatusResponse(resultData) {
    this.tcfDashboardResponse.statusDashBoardData.inProgressCount = 0;
    this.tcfDashboardResponse.statusDashBoardData.completedCount = 0;
    this.tcfDashboardResponse.statusDashBoardData.finlizedCount = 0;
    if (resultData && resultData.data && resultData.data.length > 0) {
      var inProgressData = resultData.data.find(x => x.stageId == this.tcfStage.InProgress);
      if (inProgressData && inProgressData.count)
        this.tcfDashboardResponse.statusDashBoardData.inProgressCount = inProgressData.count;
      var completedData = resultData.data.find(x => x.stageId == this.tcfStage.Completed);
      if (completedData && completedData.count)
        this.tcfDashboardResponse.statusDashBoardData.completedCount = completedData.count;
      var finlizedData = resultData.data.find(x => x.stageId == this.tcfStage.ToBeFinalized);
      if (finlizedData && finlizedData.count)
        this.tcfDashboardResponse.statusDashBoardData.finlizedCount = finlizedData.count;
    }
  }

  //get the rejected analysis data
  getRejectedAnalysisData() {
    this.mapFilterRequest();
    var request = this.generatePostRequest(config.TCF.rejectedAnalysisData, this.tcfFilterRequest, this.tcfDashboardMaster.userToken);
    request.isGenericToken = false;
    this.tcfDashboardMaster.tcfRejectedAnalysisLoading = true;
    this.tcfService.postData(request).subscribe(data => {
      this.processRejectedAnalysisDashboard(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.tcfDashboardMaster.tcfStatusLoading = false;
      });
  }

  //processs the rejected analysis data
  processRejectedAnalysisDashboard(data) {
    if (data && data.result) {
      var resultData = JSON.parse(data.result);
      if (resultData && resultData.status && resultData.status == RejectedAnalysisResponse.Success) {
        this.mapTCFRejectedAnalysis(resultData);
        this.tcfDashboardMaster.tcfRejectedAnalysisNotFound = false;
        this.tcfDashboardMaster.tcfRejectedAnalysisLoading = false;

        if (resultData.data.countPending == 0 && resultData.data.countReject == 0 && resultData.data.countReview == 0
          && resultData.data.countValidate == 0)
          this.tcfDashboardMaster.tcfRejectedAnalysisNotFound = true;

      }
      else if (resultData && resultData.status && resultData.status == RejectedAnalysisResponse.NotFound) {
        this.tcfDashboardMaster.tcfRejectedAnalysisNotFound = true;
        this.tcfDashboardMaster.tcfRejectedAnalysisLoading = false;
      }
      else {
        this.tcfDashboardMaster.tcfRejectedAnalysisNotFound = true;
        this.tcfDashboardMaster.tcfRejectedAnalysisLoading = false;
      }
      this.tcfDashboardMaster.tcfRejectedAnalysisLoading = false;
    }
    else {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
      this.tcfDashboardMaster.tcfRejectedAnalysisLoading = false;
    }
  }

  //map the tcf rejected analysis data
  mapTCFRejectedAnalysis(resultData) {
    this.tcfDashboardResponse.rejectedAnalysisData = new RejectedAnalysisData();
    if (resultData.data) {
      if (resultData.data.totalDocReceived)
        this.tcfDashboardResponse.rejectedAnalysisData.docReceived = resultData.data.totalDocReceived;
      if (resultData.data.countReview)
        this.tcfDashboardResponse.rejectedAnalysisData.docUnderReview = resultData.data.countReview;
      if (resultData.data.countReject)
        this.tcfDashboardResponse.rejectedAnalysisData.docRejected = resultData.data.countReject;
      if (resultData.data.countPending)
        this.tcfDashboardResponse.rejectedAnalysisData.docPending = resultData.data.countPending;
      if (resultData.data.countValidate)
        this.tcfDashboardResponse.rejectedAnalysisData.docValidated = resultData.data.countValidate;

      if (resultData.data.docUnderReviewPercentage)
        this.tcfDashboardResponse.rejectedAnalysisData.docUnderReviewedPerecentage = resultData.data.docUnderReviewPercentage;
      if (resultData.data.docPendingPercentage)
        this.tcfDashboardResponse.rejectedAnalysisData.docPendingPercentage = resultData.data.docPendingPercentage;
      if (resultData.data.docRejectedPercentage)
        this.tcfDashboardResponse.rejectedAnalysisData.docRejectedPercentage = resultData.data.docRejectedPercentage;
      if (resultData.data.docValidatedPercentage)
        this.tcfDashboardResponse.rejectedAnalysisData.docValidatedPercentage = resultData.data.docValidatedPercentage;

      setTimeout(() => {
        this.renderPieChart('chartdiv', resultData.data);
      }, 10);

    }
  }

  //get the supplier board data
  getSupplierBoard() {
    this.mapFilterRequest();
    var request = this.generatePostRequest(config.TCF.supplierScoreBoard, this.tcfFilterRequest, this.tcfDashboardMaster.userToken);
    request.isGenericToken = false;
    //var request = this.createTCFListSearchRequest();
    this.tcfDashboardMaster.tcfSupplierBoardLoading = true;
    //this.tcfResponse.tcfList = [];
    this.tcfService.postData(request).subscribe(data => {
      this.processSupplierBoard(data);
    },
      error => {
        this.showError('TCF_DASHBOARD.LBL_TITLE', 'TCF_DASHBOARD.MSG_UNKNONW_ERROR');
        this.tcfDashboardMaster.tcfStatusLoading = false;
      });
  }

  //process the supplier board data
  processSupplierBoard(data) {
    if (data && data.result) {
      var resultData = JSON.parse(data.result);
      if (resultData && resultData.status && resultData.status == "1") {
        this.mapSupplierScoreBoard(resultData);
        this.tcfDashboardMaster.tcfSupplierBoardLoading = false;
      }
      else if (resultData && resultData.status && resultData.status == "2") {
        this.tcfDashboardMaster.tcfSupplierBoardLoading = false;
        this.tcfDashboardMaster.tcfSupplierBoardNotFound = true;
      }
      else {
        this.tcfDashboardMaster.tcfSupplierBoardLoading = false;
        this.tcfDashboardMaster.tcfSupplierBoardNotFound = true;
      }
      this.tcfDashboardMaster.tcfSupplierBoardLoading = false;
    }
    else {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
      this.tcfDashboardMaster.tcfSupplierBoardLoading = false;
    }
  }

  //map the supplier score board
  mapSupplierScoreBoard(resultData) {
    if (resultData.data) {
      if (resultData.data.CompletionDays)
        this.tcfDashboardResponse.supplierScoreBoard.averageCompletionTime = resultData.data.CompletionDays;
      if (resultData.data.AverageDays)
        this.tcfDashboardResponse.supplierScoreBoard.averageFirstDocSubmission = resultData.data.AverageDays;
    }
  }

  //map the filter request
  mapFilterRequest() {
    this.tcfFilterRequest = new TCFFilterRequest();

    if (this.model.fromdate)
      this.tcfFilterRequest.fromDate = this.formatTCFDate(this.model.fromdate);
    if (this.model.todate)
      this.tcfFilterRequest.toDate = this.formatTCFDate(this.model.todate);
    if (this.model.searchTypeId)
      this.tcfFilterRequest.searchTypeId = this.model.searchTypeId;
    this.tcfFilterRequest.searchTypeText = this.model.searchTypeText;
    if (this.model.statusIds)
      this.tcfFilterRequest.statusIds = this.model.statusIds;
    this.tcfFilterRequest.customerIds = this.model.customerGLCodes;
    if (this.model.supplierIds)
      this.tcfFilterRequest.supplierIds = this.model.supplierIds;
    if (this.model.buyerIds)
      this.tcfFilterRequest.buyerIds = this.model.buyerIds;
    if (this.model.customerContactIds)
      this.tcfFilterRequest.customerContactIds = this.model.customerContactIds;
    if (this.model.buyerDepartmentIds)
      this.tcfFilterRequest.buyerDepartmentIds = this.model.buyerDepartmentIds;
    if (this.model.productCategoryIds)
      this.tcfFilterRequest.productCategoryIds = this.model.productCategoryIds;
    if (this.model.productSubCategoryIds)
      this.tcfFilterRequest.productSubCategoryIds = this.model.productSubCategoryIds;
    if (this.model.countryOriginIds)
      this.tcfFilterRequest.countryOriginIds = this.model.countryOriginIds;
    if (this.model.countryDestinationIds)
      this.tcfFilterRequest.countryDestinationIds = this.model.countryDestinationIds;
    if (this.model.pictureUploaded)
      this.tcfFilterRequest.pictureUploaded = this.model.pictureUploaded;

    this.tcfFilterRequest.globalDateTypeId = TCFTermEnum.Custom;
  }

  //format the tcf date
  formatTCFDate(dateValue) {
    return dateValue.year + "-" + dateValue.month + "-" + dateValue.day;
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

  //make TCF Landing Request
  getTCFLandingRequest(tcfStage) {
    this.tcfLandingRequest.fromdate = this.model.fromdate;
    this.tcfLandingRequest.todate = this.model.todate;
    this.tcfLandingRequest.customerGLCodes = this.tcfFilterRequest.customerIds;
    this.tcfLandingRequest.searchTypeId = this.tcfFilterRequest.searchTypeId;
    this.tcfLandingRequest.searchTypeText = this.tcfFilterRequest.searchTypeText;
    this.tcfLandingRequest.supplierTypeId = this.model.supplierTypeId;
    this.tcfLandingRequest.supplierIds = this.tcfFilterRequest.supplierIds;
    this.tcfLandingRequest.buyerIds = this.tcfFilterRequest.buyerIds;
    this.tcfLandingRequest.customerContactIds = this.tcfFilterRequest.customerContactIds;
    this.tcfLandingRequest.buyerDepartmentIds = this.tcfFilterRequest.buyerDepartmentIds;
    this.tcfLandingRequest.productCategoryIds = this.tcfFilterRequest.productCategoryIds;
    this.tcfLandingRequest.productSubCategoryIds = this.tcfFilterRequest.productSubCategoryIds;
    this.tcfLandingRequest.countryOriginIds = this.tcfFilterRequest.countryOriginIds;
    this.tcfLandingRequest.countryDestinationIds = this.tcfFilterRequest.countryDestinationIds;
    this.tcfLandingRequest.pictureUploaded = this.tcfFilterRequest.pictureUploaded;
    this.tcfLandingRequest.dateTypeId = 10;
    this.tcfLandingRequest.pageSize = 10;
    this.tcfLandingRequest.index = 1;
    this.tcfLandingRequest.statusIds.push(tcfStage);
  }

  //navigate to TCF Summary Page
  navigateTCFSummary(tcfStage) {

    this.getTCFLandingRequest(tcfStage);
    var path = "tcfsummary/tcf-summary/";
    var entity: string = this.utility.getEntityName();

    this.router.navigate([`/${entity}/${path}`], { queryParams: { param: encodeURI(JSON.stringify(this.tcfLandingRequest)) } });

  }

  //get the tcf scope details
  getTCFScope() {
    this.mapFilterRequest();
    var request = this.generatePostRequest(config.TCF.tcfScopeDashBoardData, this.tcfFilterRequest, this.tcfDashboardMaster.userToken);
    request.isGenericToken = false;
    this.tcfDashboardMaster.tcfScopeDashboardLoading = true;
    this.tcfService.postData(request).subscribe(data => {

      this.processTCFScopeDashboardData(data);
    },
      error => {
        this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_UNKNOWN_ERROR');
        this.tcfDashboardMaster.tcfStatusLoading = false;
      });
  }

  //processs the rejected analysis data
  processTCFScopeDashboardData(data) {
    if (data && data.result) {
      this.tcfDashboardResponse.scopeDashboardData=new ScopeDashboardData();
      var resultData = JSON.parse(data.result);
      if (resultData && resultData.status && resultData.status == TCFScopeDashboardResponse.Success) {
        this.tcfDashboardMaster.tcfScopeDashboardNotFound = false;
        this.mapTCFScopeData(resultData);
        this.tcfDashboardMaster.tcfScopeDashboardLoading = false;
      }
      else if (resultData && resultData.status && resultData.status == TCFScopeDashboardResponse.NotFound) {
        this.tcfDashboardMaster.tcfScopeDashboardNotFound = true;
        this.tcfDashboardMaster.tcfScopeDashboardLoading = false;
      }
      else {
        this.tcfDashboardMaster.tcfScopeDashboardNotFound = true;
        this.tcfDashboardMaster.tcfScopeDashboardLoading = false;
      }
    }
    else {
      this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
      this.tcfDashboardMaster.tcfScopeDashboardLoading = false;
    }
  }

  //map the tcf rejected analysis data
  mapTCFScopeData(resultData) {

    if (resultData.data) {
      if (resultData.data.countUnderReview)
        this.tcfDashboardResponse.scopeDashboardData.scopeUnderReview = resultData.data.countUnderReview;
      if (resultData.data.countWaiting)
        this.tcfDashboardResponse.scopeDashboardData.scopeWaiting = resultData.data.countWaiting;
      if (resultData.data.countConfirm)
        this.tcfDashboardResponse.scopeDashboardData.scopeConfirm = resultData.data.countConfirm;
      if (resultData.data.countExpired)
        this.tcfDashboardResponse.scopeDashboardData.scopeExpired = resultData.data.countExpired;
      if (resultData.data.countNotSet)
        this.tcfDashboardResponse.scopeDashboardData.scopeNotSet = resultData.data.countNotSet;

      if (resultData.data.percentUnderReview)
        this.tcfDashboardResponse.scopeDashboardData.scopeUnderReviewPerecentage = resultData.data.percentUnderReview;
      if (resultData.data.percentWaiting)
        this.tcfDashboardResponse.scopeDashboardData.scopeWaitingPercentage = resultData.data.percentWaiting;
      if (resultData.data.percentConfirm)
        this.tcfDashboardResponse.scopeDashboardData.scopeConfirmPercentage = resultData.data.percentConfirm;
      if (resultData.data.percentExpired)
        this.tcfDashboardResponse.scopeDashboardData.scopeExpiredPercentage = resultData.data.percentExpired;
      if (resultData.data.percentNotSet)
        this.tcfDashboardResponse.scopeDashboardData.scopeNotSetPercentage = resultData.data.percentNotSet;

      if (resultData.data.countUnderReview == 0 && resultData.data.countWaiting == 0 && resultData.data.countConfirm == 0 &&
        resultData.data.countNotSet == 0)
        this.tcfDashboardMaster.tcfScopeDashboardNotFound = true;

      setTimeout(() => {
        this.renderScopeChart('chartdiv1', resultData.data);
      }, 10);

    }
  }

  renderScopeChart(container, data) {

    // Create chart instance
    var chart = am4core.create(container, am4charts.PieChart);

    var scopeUnderReview = 0;
    var scopeWaiting = 0;
    var scopeConfirm = 0;
    var scopeExpired = 0;
    var scopeNotSet = 0;

    if (data && data.countUnderReview)
      scopeUnderReview = data.countUnderReview;
    if (data && data.countWaiting)
      scopeWaiting = data.countWaiting;
    if (data && data.countConfirm)
      scopeConfirm = data.countConfirm;
    if (data && data.countExpired)
      scopeExpired = data.countExpired;
    if (data && data.countNotSet)
      scopeNotSet = data.countNotSet;

    // Add data
    chart.data = [
      { "sector": "Confirmed", "size": scopeConfirm, "color": am4core.color("#24c11e") },
      { "sector": "Under Review", "size": scopeUnderReview, "color": am4core.color("#e04f4f") },
      { "sector": "Waiting", "size": scopeWaiting, "color": am4core.color("#e99485ee") },
      { "sector": "Expired", "size": scopeExpired, "color": am4core.color("#f81538") }
    ];

    chart.innerRadius = 45;

    // Add and configure Series
    var pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "size";
    pieSeries.dataFields.category = "sector";
    pieSeries.labels.template.disabled = true;
    pieSeries.slices.template.propertyFields.fill = "color";

    pieSeries.tooltip.autoTextColor = false;
    pieSeries.tooltip.label.fill = am4core.color("#FFFFFF");
  }

  //fetch the first 10 customers on load
  getSupplierListBySearch() {
    this.tcfDashboardMaster.requestSupplierModel.serviceId = APIService.Tcf;
    if (this.model.supplierTypeId == SearchType.SupplierCode)
      this.tcfDashboardMaster.requestSupplierModel.supSearchTypeId = SearchType.SupplierCode;
    else
      this.tcfDashboardMaster.requestSupplierModel.supSearchTypeId = SearchType.SupplierName;
    if (this.user.customerid)
      //this.masterData.requestSupplierModel.customerIds = this.model.customerIds;
      this.tcfDashboardMaster.requestSupplierModel.customerIds.push(this.user.customerid);
    this.tcfDashboardMaster.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.supplierLoading = true),
      switchMap(term => term
        ? this.supplierService.getSupplierDataSource(this.tcfDashboardMaster.requestSupplierModel, APIService.Tcf, term)
        : this.supplierService.getSupplierDataSource(this.tcfDashboardMaster.requestSupplierModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.supplierLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.supplierList = data;
        this.tcfDashboardMaster.supplierLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getSupplierData(isDefaultLoad: boolean) {
    this.tcfDashboardMaster.requestSupplierModel.serviceId = APIService.Tcf;
    if (this.model.supplierTypeId == SearchType.SupplierCode)
      this.tcfDashboardMaster.requestSupplierModel.supSearchTypeId = SearchType.SupplierCode;
    else
      this.tcfDashboardMaster.requestSupplierModel.supSearchTypeId = SearchType.SupplierName;
    /*  if (this.model.customerIds)
       this.masterData.requestSupplierModel.customerIds = this.model.customerIds; */
    if (this.model.customerGLCodes)
      this.tcfDashboardMaster.requestSupplierModel.customerglCodes = this.model.customerGLCodes;

    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestSupplierModel.searchText = this.tcfDashboardMaster.supplierInput.getValue();
      this.tcfDashboardMaster.requestSupplierModel.skip = this.tcfDashboardMaster.supplierList.length;
    }
    this.tcfDashboardMaster.supplierLoading = true;
    this.supplierService.getSupplierDataSource(this.tcfDashboardMaster.requestSupplierModel, APIService.Tcf).
      subscribe(data => {
        if (data && data.length > 0) {
          this.tcfDashboardMaster.supplierList = this.tcfDashboardMaster.supplierList.concat(data);
        }
        // if (isDefaultLoad)
        //   this.tcfDashboardMaster.requestSupplierModel = new SupplierDataSourceRequest();
        this.tcfDashboardMaster.supplierLoading = false;
      }),
      error => {
        this.tcfDashboardMaster.supplierLoading = false;
      };
  }

  clearSupplierSelection() {

    this.tcfDashboardMaster.requestSupplierModel = new SupplierDataSourceRequest();
    this.tcfDashboardMaster.supplierList = [];
    this.getSupplierData(false);
    this.getDashboardData();
  }

  SetSearchTypemodel(searchtype) {
    this.model.searchTypeId = searchtype;
  }

  SetSearchDatetype(searchdatetype) {
    this.model.dateTypeId = searchdatetype;
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

  //get the tcf status list
  getTCFStatusList() {
    this.genericGetRequest.requestUrl = config.TCF.tcfStatusList;
    this.genericGetRequest.isGenericToken = true;
    this.tcfDashboardMaster.statusLoading = true;
    this.tcfService.getData(this.genericGetRequest).subscribe(data => {
      this.processTCFStatusResponse(data);
    },
      error => {
        this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
        this.tcfDashboardMaster.statusLoading = false;
      });
  }

  //process the tcf status response
  processTCFStatusResponse(data) {
    if (data && data.status && data.status == TCFStageResponse.Success) {
      this.tcfDashboardMaster.statusList = data.data;
      this.tcfDashboardMaster.statusLoading = false;
    } else if (data && data.status && data.status == TCFStageResponse.NotFound) {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.LBL_TCF_STATUS_NOT_FOUND');
      this.tcfDashboardMaster.statusLoading = false;
    } else {
      this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
      this.tcfDashboardMaster.statusLoading = false;
    }
  }

  //fetch the first 10 customers on load
  getCustomerListBySearch() {
    this.tcfDashboardMaster.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.customerLoading = true),
      switchMap(term => term ?
        this.customerService.getCustomerDataSource(this.tcfDashboardMaster.requestCustomerModel, term) :
        this.customerService.getCustomerDataSource(this.tcfDashboardMaster.requestCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.customerLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.customerList = data;
        this.tcfDashboardMaster.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    this.tcfDashboardMaster.requestCustomerModel.serviceId = APIService.Tcf;
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestCustomerModel.searchText = this.tcfDashboardMaster.customerInput.getValue();
      this.tcfDashboardMaster.requestCustomerModel.skip = this.tcfDashboardMaster.customerList.length;
    }
    this.tcfDashboardMaster.customerLoading = true;
    this.customerService.getCustomerDataSource(this.tcfDashboardMaster.requestCustomerModel).subscribe(data => {
      if (data && data.length > 0) {
        this.tcfDashboardMaster.customerList = this.tcfDashboardMaster.customerList.concat(data);
      }
      if (isDefaultLoad)
        this.tcfDashboardMaster.requestCustomerModel = new CustomerDataSourceRequest();
      this.tcfDashboardMaster.customerLoading = false;
    }),
      error => {
        this.tcfDashboardMaster.customerLoading = false;
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
    this.tcfDashboardMaster.requestCustomerModel = new CustomerDataSourceRequest();
    this.model.customerGLCodes = [];
    this.tcfDashboardMaster.customerList = [];
    this.resetCustomer();

    this.getCustomerData(false);
    this.getSupplierData(false);
    this.getBuyerData(false);
  }

  resetCustomer() {
    this.model.supplierIds = [];
    this.model.buyerIds = [];
    this.model.customerContactIds = [];
    this.tcfDashboardMaster.supplierList = [];
    this.tcfDashboardMaster.buyerList = [];
    this.tcfDashboardMaster.customerContactList = [];
  }

  //fetch the first 10 buyers on load
  getBuyerListBySearch() {

    if (this.model.customerGLCodes)
      this.tcfDashboardMaster.requestBuyerModel.customerglCodes = this.model.customerGLCodes;

    this.tcfDashboardMaster.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.buyerLoading = true),
      switchMap(term => term ?
        this.customerService.getCustomerBuyerDataSource(this.tcfDashboardMaster.requestBuyerModel, APIService.Tcf, term) :
        this.customerService.getCustomerBuyerDataSource(this.tcfDashboardMaster.requestBuyerModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.buyerLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.buyerList = data;
        this.tcfDashboardMaster.buyerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getBuyerData(isDefaultLoad: boolean) {
    if (this.model.customerGLCodes)
      this.tcfDashboardMaster.requestBuyerModel.customerglCodes = this.model.customerGLCodes;
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestBuyerModel.searchText = this.tcfDashboardMaster.buyerInput.getValue();
      this.tcfDashboardMaster.requestBuyerModel.skip = this.tcfDashboardMaster.buyerList.length;
    }
    this.tcfDashboardMaster.buyerLoading = true;
    this.customerService.getCustomerBuyerDataSource(this.tcfDashboardMaster.requestBuyerModel, APIService.Tcf).subscribe(data => {
      if (data && data.length > 0) {
        this.tcfDashboardMaster.buyerList = this.tcfDashboardMaster.buyerList.concat(data);
      }
      if (isDefaultLoad)
        this.tcfDashboardMaster.requestBuyerModel = new BuyerDataSourceRequest();
      this.tcfDashboardMaster.buyerLoading = false;
    }),
      error => {
        this.tcfDashboardMaster.buyerLoading = false;
      };
  }

  clearBuyerSelection() {
    this.tcfDashboardMaster.requestBuyerModel = new BuyerDataSourceRequest();
    this.tcfDashboardMaster.buyerList = [];
    this.getBuyerData(false);
  }

  //fetch the first 10 buyers on load
  getCustomerContactListBySearch() {

    if (this.model.customerGLCodes)
      this.tcfDashboardMaster.requestCustomerContactModel.customerglCodes = this.model.customerGLCodes;
    this.tcfDashboardMaster.customerContactInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.customerContactLoading = true),
      switchMap(term => term ?
        this.customerService.getCustomerContactDataSource(this.tcfDashboardMaster.requestCustomerContactModel, APIService.Tcf, term) :
        this.customerService.getCustomerContactDataSource(this.tcfDashboardMaster.requestCustomerContactModel, APIService.Tcf)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.customerContactLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.customerContactList = data;
        this.tcfDashboardMaster.customerContactLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerContactData(isDefaultLoad: boolean) {
    if (this.model.customerGLCodes)
      this.tcfDashboardMaster.requestCustomerContactModel.customerglCodes = this.model.customerGLCodes;
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestCustomerContactModel.searchText = this.tcfDashboardMaster.customerContactInput.getValue();
      this.tcfDashboardMaster.requestCustomerContactModel.skip = this.tcfDashboardMaster.customerContactList.length;
    }
    this.tcfDashboardMaster.customerContactLoading = true;
    this.customerService.getCustomerContactDataSource(this.tcfDashboardMaster.requestCustomerContactModel, APIService.Tcf).subscribe(data => {
      if (data && data.length > 0) {
        this.tcfDashboardMaster.customerContactList = this.tcfDashboardMaster.customerContactList.concat(data);
      }
      if (isDefaultLoad)
        this.tcfDashboardMaster.requestCustomerContactModel = new CustomerContactSourceRequest();
      this.tcfDashboardMaster.customerContactLoading = false;
    }),
      error => {
        this.tcfDashboardMaster.customerContactLoading = false;
      };
  }

  clearCustomerContactSelection() {
    this.tcfDashboardMaster.requestCustomerContactModel = new CustomerContactSourceRequest();
    this.tcfDashboardMaster.customerContactList = [];
    this.getCustomerContactData(false);
  }

  //fetch the first 10 buyers on load
  getProductCategoryListBySearch() {
    this.tcfDashboardMaster.requestProductCategoryModel.serviceId = APIService.Tcf;
    this.tcfDashboardMaster.productCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.productCategoryLoading = true),
      switchMap(term => term ?
        this.customerProductService.getProductCategoryDataSource(this.tcfDashboardMaster.requestProductCategoryModel, term) :
        this.customerProductService.getProductCategoryDataSource(this.tcfDashboardMaster.requestProductCategoryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.productCategoryLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.productCategoryList = data;
        this.tcfDashboardMaster.productCategoryLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getProductCategoryData(isDefaultLoad: boolean) {
    this.tcfDashboardMaster.requestProductCategoryModel.serviceId = APIService.Tcf;
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestProductCategoryModel.searchText = this.tcfDashboardMaster.productCategoryInput.getValue();
      this.tcfDashboardMaster.requestProductCategoryModel.skip = this.tcfDashboardMaster.productCategoryList.length;
    }
    this.tcfDashboardMaster.productCategoryLoading = true;
    this.customerProductService.getProductCategoryDataSource(this.tcfDashboardMaster.requestProductCategoryModel).subscribe(data => {
      if (data && data.length > 0) {
        this.tcfDashboardMaster.productCategoryList = this.tcfDashboardMaster.productCategoryList.concat(data);
      }
      if (isDefaultLoad)
        this.tcfDashboardMaster.requestProductCategoryModel = new ProductCategorySourceRequest();
      this.tcfDashboardMaster.productCategoryLoading = false;
    }),
      error => {
        this.tcfDashboardMaster.productCategoryLoading = false;
      };
  }

  //change the product category data
  changeProductCategoryData() {
    if (this.model.productCategoryIds) {
      this.tcfDashboardMaster.productSubCategoryList = [];
      this.getProductSubCategoryData(true);
    }
  }

  //clear the product category data
  clearProductCategorySelection() {
    this.tcfDashboardMaster.requestProductCategoryModel = new ProductCategorySourceRequest();
    this.tcfDashboardMaster.productCategoryList = [];
    this.getProductCategoryData(false);
  }

  //fetch the first 10 buyers on load
  getProductSubCategoryListBySearch() {

    this.tcfDashboardMaster.requestProductSubCategoryModel.serviceId = APIService.Tcf;
    if (this.model.productCategoryIds)
      this.tcfDashboardMaster.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
    this.tcfDashboardMaster.productSubCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.productSubCategoryLoading = true),
      switchMap(term => term ?
        this.customerProductService.getProductSubCategoryDataSource(this.tcfDashboardMaster.requestProductSubCategoryModel, term) :
        this.customerProductService.getProductSubCategoryDataSource(this.tcfDashboardMaster.requestProductSubCategoryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.productSubCategoryLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.productSubCategoryList = data;
        this.tcfDashboardMaster.productSubCategoryLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getProductSubCategoryData(isDefaultLoad: boolean) {
    this.tcfDashboardMaster.requestProductSubCategoryModel.serviceId = APIService.Tcf;
    if (this.model.productCategoryIds)
      this.tcfDashboardMaster.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestProductSubCategoryModel.searchText = this.tcfDashboardMaster.productSubCategoryInput.getValue();
      this.tcfDashboardMaster.requestProductSubCategoryModel.skip = this.tcfDashboardMaster.productSubCategoryList.length;
    }
    this.tcfDashboardMaster.productSubCategoryLoading = true;
    this.customerProductService.getProductSubCategoryDataSource(this.tcfDashboardMaster.requestProductSubCategoryModel).subscribe(data => {
      if (data && data.length > 0) {
        this.tcfDashboardMaster.productSubCategoryList = this.tcfDashboardMaster.productSubCategoryList.concat(data);
      }
      if (isDefaultLoad)
        this.tcfDashboardMaster.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();
      this.tcfDashboardMaster.productSubCategoryLoading = false;
    }),
      error => {
        this.tcfDashboardMaster.productSubCategoryLoading = false;
      };
  }

  clearProductSubCategorySelection() {
    this.tcfDashboardMaster.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();
    this.tcfDashboardMaster.productSubCategoryList = [];
    this.getProductSubCategoryData(false);
  }

  //fetch the first 10 buyers on load
  getCountryOriginListBySearch() {
    this.tcfDashboardMaster.countryOriginInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.countryOriginLoading = true),
      switchMap(term => term ?
        this.locationService.getCountryDataSourceList(this.tcfDashboardMaster.requestCountryModel, term) :
        this.locationService.getCountryDataSourceList(this.tcfDashboardMaster.requestCountryModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.countryOriginLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.countryOriginList = data;
        this.tcfDashboardMaster.countryOriginLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCountryOriginData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestCountryModel.searchText = this.tcfDashboardMaster.countryOriginInput.getValue();
      this.tcfDashboardMaster.requestCountryModel.skip = this.tcfDashboardMaster.countryOriginList.length;
    }
    this.tcfDashboardMaster.countryOriginLoading = true;
    this.locationService.getCountryDataSourceList(this.tcfDashboardMaster.requestCountryModel).subscribe(data => {
      if (data && data.length > 0) {
        this.tcfDashboardMaster.countryOriginList = this.tcfDashboardMaster.countryOriginList.concat(data);
      }
      if (isDefaultLoad)
        this.tcfDashboardMaster.requestCountryModel = new CountryDataSourceRequest();
      this.tcfDashboardMaster.countryOriginLoading = false;
    }),
      error => {
        this.tcfDashboardMaster.countryOriginLoading = false;
      };
  }

  clearCountryOriginSelection() {
    this.tcfDashboardMaster.requestCountryModel = new CountryDataSourceRequest();
    this.tcfDashboardMaster.countryOriginList = [];
    this.getCountryOriginData(false);
  }

  //fetch the first 10 buyers on load
  getCountryDestinationListBySearch() {

    this.tcfDashboardMaster.countryDestinationInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.tcfDashboardMaster.countryDestinationLoading = true),
      switchMap(term => term ?
        this.locationService.getCountryDataSourceList(this.tcfDashboardMaster.requestCountryDestinationModel, term) :
        this.locationService.getCountryDataSourceList(this.tcfDashboardMaster.requestCountryDestinationModel, term)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.tcfDashboardMaster.countryDestinationLoading = false))
      ))
      .subscribe(data => {
        this.tcfDashboardMaster.countryDestinationList = data;
        this.tcfDashboardMaster.countryDestinationLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCountryDestinationData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.tcfDashboardMaster.requestCountryDestinationModel.searchText = this.tcfDashboardMaster.countryDestinationInput.getValue();
      this.tcfDashboardMaster.requestCountryDestinationModel.skip = this.tcfDashboardMaster.countryDestinationList.length;
    }
    this.tcfDashboardMaster.countryDestinationLoading = true;
    this.locationService.getCountryDataSourceList(this.tcfDashboardMaster.requestCountryDestinationModel).subscribe(data => {
      if (data && data.length > 0) {
        this.tcfDashboardMaster.countryDestinationList = this.tcfDashboardMaster.countryDestinationList.concat(data);
      }
      if (isDefaultLoad)
        this.tcfDashboardMaster.requestCountryDestinationModel = new CountryDataSourceRequest();
      this.tcfDashboardMaster.countryDestinationLoading = false;
    }),
      error => {
        this.tcfDashboardMaster.countryOriginLoading = false;
      };
  }

  clearCountryDestinationSelection() {
    this.tcfDashboardMaster.requestCountryDestinationModel = new CountryDataSourceRequest();
    this.tcfDashboardMaster.countryDestinationList = [];
    this.getCountryDestinationData(false);
  }

  reset() {
    this.model = new TCFLandingRequest();
    this.ngOnInit();
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  //get mandayterm list
  getTCFTermList() {
    this.tcfDashboardMaster.tcfTermList = TCFTermList;
    this.model.globalDateTypeId = TCFTermEnum.LastYear;
    this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'y', 1);
    this.model.todate = this.calendar.getToday();
  }

  //change TCF Term
  changeTCFTerm(event) {
    if (event.id == TCFTermEnum.LastYear) {
      this.tcfDashboardMaster.isDateShow = false;
      this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'y', 1);
      this.model.todate = this.calendar.getToday();
      // this.tcfTermDateChange()
    } else if (event.id == TCFTermEnum.LastSixMonth) {
      this.tcfDashboardMaster.isDateShow = false;
      this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'm', 6);
      this.model.todate = this.calendar.getToday();
      // this.tcfTermDateChange()
    } else if (event.id == TCFTermEnum.LastThreeMonth) {
      this.tcfDashboardMaster.isDateShow = false;
      this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
      this.model.todate = this.calendar.getToday();
      // this.tcfTermDateChange()
    } else if (event.id == TCFTermEnum.Custom) {
      this.tcfDashboardMaster.isDateShow = true;
      this.model.fromdate = null;
      this.model.todate = null;
      this.tcfDashboardMaster.isTCFTermShow = true;
    }
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.getDashboardData();
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

  getTCFInProgress() {
    this.mapFilterRequest();
    const request = this.generatePostRequest(config.TCF.tcfInProgress, this.tcfFilterRequest, this.tcfDashboardMaster.userToken);
    request.isGenericToken = false;
    this.tcfDashboardMaster.tcfInProgressLoading = true;
    this.tcfService.postData(request).subscribe(data => {
      this.processTCFInProgressData(data);
    });
  }

  processTCFInProgressData(data: any) {
    if (data && data.result) {
      const resultData = JSON.parse(data.result);
      if (resultData && resultData.status && resultData.status == TCFInProgressResponse.Success) {
        this.mapTCFInProgressAnalysis(resultData);
        this.tcfDashboardMaster.tcfInProgressNotFound = false;
        this.tcfDashboardMaster.tcfInProgressLoading = false;
        if (resultData.data.total == 0)
          this.tcfDashboardMaster.tcfInProgressNotFound = true;
      }
      else {
        this.tcfDashboardMaster.tcfInProgressNotFound = true;
        this.tcfDashboardMaster.tcfInProgressLoading = false;
      }
    }
    else {
      this.showError('COMMON.LBL_ERROR', 'TCF Error.Please refer the log data');
      this.tcfDashboardMaster.tcfInProgressLoading = false;
    }
  }

  mapTCFInProgressAnalysis(response: any) {
    const data = response.data;
    const yAxis = Object.keys(response.data);
    setTimeout(() => {
      this.barChartFrame(data, yAxis);
    }, 10);
  }

  barChartFrame(data: string | any[], yAxis: string | any[]) {

    if (yAxis && yAxis.length > 0 && data) {

      const chartObj = [];
      for (let i = yAxis.length - 1; i >= 0 ; i--) {
        const subchartObj = {};
        if (yAxis[i].includes('lessThan') || yAxis[i].includes('between') || yAxis[i].includes('moreThan')) {
          let category = null;
          if (yAxis[i].includes('lessThan'))
            category = yAxis[i].replace('lessThan', '< ') + " days"
          else if (yAxis[i].includes('moreThan'))
            category = yAxis[i].replace('moreThan', '> ') + " days"
          else if (yAxis[i].includes('between')) {
            category = yAxis[i].replace('between', '')
            category = category.replace('To', ' - ') + " days"
          }
          subchartObj["category"] = category;
          subchartObj["result"] = data[yAxis[i]];
          subchartObj["none"] = 0;
          chartObj.push(subchartObj);
        }
      }

      setTimeout(() => {
        this.tcfInProgressChart(chartObj);
      }, 10);
    }
  }

  tcfInProgressChart(chartObj: any[]) {
    this.clusteredRowchart = am4core.create("horizontalBar-chart", am4charts.XYChart);
    this.clusteredRowchart.maskBullets = false;
    this.clusteredRowchart.numberFormatter.numberFormat = "#.#"

    this.clusteredRowchart.data = chartObj;

    // Create axes
    let categoryAxis = this.clusteredRowchart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = 'category';
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.grid.template.opacity = 0;
    categoryAxis.renderer.baseGrid.disabled = true;
    categoryAxis.renderer.labels.template.fontSize = 12;
    categoryAxis.renderer.labels.template.fill = am4core.color('#A1A8B3');
    categoryAxis.renderer.cellStartLocation = 0.1;
    categoryAxis.renderer.cellEndLocation = 0.6;
    let label = categoryAxis.renderer.labels.template;
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

    let valueAxis = this.clusteredRowchart.xAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.renderer.grid.template.opacity = 1;
    valueAxis.renderer.ticks.template.strokeOpacity = 0.5;
    valueAxis.renderer.ticks.template.stroke = am4core.color('#495C43');
    valueAxis.renderer.line.strokeOpacity = 0;
    valueAxis.renderer.baseGrid.disabled = false;
    valueAxis.renderer.labels.template.disabled = false;
    valueAxis.renderer.labels.template.fill = am4core.color('#AAAFB6');
    valueAxis.renderer.labels.template.fontSize = 12;
    valueAxis.calculateTotals = true;

    // Create series
    let datasize = chartObj ? chartObj.length : 0;
    let graphwidth = 10;
    if(datasize >= 5)
      graphwidth = 85;
    else if(datasize >= 3)
      graphwidth = 40;
    let series = this.clusteredRowchart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueX = 'result';
    series.dataFields.categoryY = 'category';
    series.name = 'result';
    series.columns.template.tooltipText = '{result}';
    series.stacked = datasize;
    series.columns.template.width = am4core.percent(graphwidth);
    series.columns.template.fill = am4core.color('#007bff');
    series.columns.template.top = true;

    let totalSeries = this.clusteredRowchart.series.push(new am4charts.ColumnSeries());
    totalSeries.dataFields.valueX = "none";
    totalSeries.dataFields.categoryY = "category";
    totalSeries.stacked = true;
    totalSeries.hiddenInLegend = true;
    totalSeries.columns.template.strokeOpacity = 0;

    let totalBullet = totalSeries.bullets.push(new am4charts.LabelBullet());
    totalBullet.dx = 15;
    totalBullet.label.text = "{valueX.total}";
    totalBullet.label.hideOversized = false;
    totalBullet.label.fontSize = 12;
    totalBullet.label.truncate = false;
  }

  changeSupplierType() {
    this.tcfDashboardMaster.supplierLoading = true;
    this.tcfDashboardMaster.supplierList = [];
    this.model.supplierIds = [];
    this.getSupplierListBySearch();
  }

  assignSupplierDetails() {
    this.model.supplierTypeId = SearchType.SupplierName;
  }
}

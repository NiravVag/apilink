import { CustomerProduct } from './../../../../_Services/customer/customerproductsummary.service';
import { Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { trigger, state, style, transition, animate } from '@angular/animations';
import { CountryChartRequest, RejectChartRequest, PopUpListModel, RejectChartResponse, RejectionDashBoardDataFound, RejectionDashBoardLoader, RejectionDashboardModel, RejectionDashboardRequest, RejectionFactoryData, RejectChartSubcatogoryRequest, RejectChartSubcatogory2Request, ResultDashboard, GroupByFilterEnum } from 'src/app/_Models/statistics/rejectiondashborad.model';
import { RejectionDashBoardService } from 'src/app/_Services/statistics/rejectiondashboard.service';
import { DetailComponent } from 'src/app/components/common/detail.component';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { NgbCalendar, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { amCoreLicense, CountrySelectedFilterTextCount, CusDashboardProductLength, ListSize, MobileViewFilterCount, SupplierNameTrim, SupplierType, UserType, fbReportResultList, FbReportResultType, SearchType, supplierTypeList, DataLengthTrim, GroupByFilter } from 'src/app/components/common/static-data-common';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, CustomerCommonDataSourceRequest, ProductDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { LocationService } from 'src/app/_Services/location/location.service';
import { MandayYear } from 'src/app/_Models/statistics/manday-dashboard.model';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { Validator } from 'src/app/components/common';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CustomerInfo } from 'src/app/_Models/dashboard/customerdashboard.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { InvoiceRequestType } from 'src/app/_Models/customer/customer-price-card.model';
import { UserModel } from 'src/app/_Models/user/user.model';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { FactoryDataSourceRequest } from 'src/app/_Models/statistics/defect-dashboard.model';

@Component({
  selector: 'app-rejectdashboard',
  templateUrl: './rejectdashboard.component.html',
  styleUrls: ['./rejectdashboard.component.scss'],
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
export class RejectdashboardComponent extends DetailComponent implements OnDestroy {
  componentDestroyed$: Subject<boolean> = new Subject();
  model: RejectionDashboardModel;
  rejectionDashboardLoader: RejectionDashBoardLoader;
  rejectionRateLandingRequest: RejectionDashboardRequest;
  requestModel: RejectionDashboardRequest;
  bookingIdList: Array<number>;
  dataFound: RejectionDashBoardDataFound;
  clusteredColumChart: any;
  clusteredRowchart: any;
  clusteredRowCountryChart: any;
  popUpMonthData: MandayYear;
  rejectionCount: any;
  rejectionName: any;
  customerInfo: CustomerInfo;
  toggleFormSection: boolean;
  popUpModel: PopUpListModel;
  popUpTableData: Array<RejectionFactoryData>;
  loaderMsg: string;
  requestCustomerModel: CustomerCommonDataSourceRequest;
  currentUser: UserModel;
  chart: any;
  isFilterOpen: boolean;
  defectTableArray: Array<any>;
  rejectionPopupState: boolean;
  FBReportResultList = fbReportResultList;
  userTypeEnum = UserType;
  supplierTypeList: any = supplierTypeList;
  LoadFirstTime: boolean = true;
  dataLengthTrim = DataLengthTrim;
  constructor(private service: RejectionDashBoardService, router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    private calendar: NgbCalendar, private locationService: LocationService, private buyerService: CustomerbuyerService, private brandService: CustomerBrandService,
    private supService: SupplierService, private cusService: CustomerService, private deptService: CustomerDepartmentService, private collectionService: CustomerCollectionService,
    public validator: Validator, public utility: UtilityService, authserve: AuthenticationService,
    private productManagementService: ProductManagementService,
    private customerProductService: CustomerProduct,
    private zone: NgZone, public modalService: NgbModal) {

    super(router, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    am4core.addLicense(amCoreLicense);
    this.isFilterOpen = false;
    this.rejectionPopupState = false;

    this.popUpModel = new PopUpListModel();

    this.popUpModel.rejectOptions = [
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

  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }

  onInit() {
    this.initialize();
    this.model.supplierTypeId = SearchType.SupplierName;
    this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    this.validator.setJSON("dashboard/reject-dashboard.valid.json");
    this.validator.setModelAsync(() => this.requestModel);
    this.validator.isSubmitted = false;
    this.model.fbReportResultId = FbReportResultType.Fail;

    this.dateSelectionDefault();

    this.placeHolderLoad();

    if (this.currentUser.usertype == UserType.Supplier) {
      this.requestModel.supplierId = this.currentUser.supplierid;
      this.getSupListBySearch();
    }

    if (this.currentUser.usertype == UserType.Factory) {
      this.getSupListBySearch();
      this.requestModel.factoryId = this.currentUser.factoryid;
      this.model.factoryId = this.currentUser.factoryid;
      this.getFactListBySearch();
    }

    this.getCustomerListBySearch();

    this.getCountryListBySearch();
    this.getProductCategoryData();
  }

  initialize() {
    this.model = new RejectionDashboardModel();
    this.rejectionDashboardLoader = new RejectionDashBoardLoader();
    this.requestModel = new RejectionDashboardRequest();
    this.dataFound = new RejectionDashBoardDataFound();
    this.customerInfo = new CustomerInfo();
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();
    this.rejectionRateLandingRequest = new RejectionDashboardRequest();
    this.model.supplierTypeId = SearchType.SupplierName;
    this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
  }

  //Added placeholder in all charts
  placeHolderLoad() {
    this.rejectionDashboardLoader.apiResultDashboardLoading = true;
    this.rejectionDashboardLoader.customerResultDashboardLoading = true;
    this.rejectionDashboardLoader.countryDashboardLoading = true;
    this.rejectionDashboardLoader.productCategoryDashboardLoading = true;
    this.rejectionDashboardLoader.vendorDashboardLoading = true;
    this.rejectionDashboardLoader.rejectDashboardLoading = false;
    this.rejectionDashboardLoader.apiResultDashboardError = false;
    this.rejectionDashboardLoader.customerResultDashboardError = false;
    this.rejectionDashboardLoader.countryDashboardError = false;
    this.rejectionDashboardLoader.productCategoryDashboardError = false;
    this.rejectionDashboardLoader.vendorDashboardError = false;
    this.rejectionDashboardLoader.rejectDashboardError = false;
  }


  stopPlaceHolderLoad() {
    this.rejectionDashboardLoader.apiResultDashboardLoading = false;
    this.rejectionDashboardLoader.customerResultDashboardLoading = false;
    this.rejectionDashboardLoader.countryDashboardLoading = false;
    this.rejectionDashboardLoader.productCategoryDashboardLoading = false;
    this.rejectionDashboardLoader.vendorDashboardLoading = false;
    this.rejectionDashboardLoader.rejectDashboardLoading = false;
    this.rejectionDashboardLoader.apiResultDashboardError = false;
    this.rejectionDashboardLoader.customerResultDashboardError = false;
    this.rejectionDashboardLoader.countryDashboardError = false;
    this.rejectionDashboardLoader.productCategoryDashboardError = false;
    this.rejectionDashboardLoader.vendorDashboardError = false;
    this.rejectionDashboardLoader.rejectDashboardError = false;
  }

  //load the graph data
  async charLoadData() {
    await this.getAPIResultDashboardSummary(this.requestModel);

    this.getCustomerResultDashboardSummary(this.requestModel);
    this.getProductCategoryDashboardSummary(this.requestModel);
    this.getVendorDashboardSummary(this.requestModel);
    this.getCountryDashboardSummary(false);
    this.getRejectDashboardSummary();
    this.fbReportResultChange();
  }

  //fetch the API result chart
  async getAPIResultDashboardSummary(request: RejectionDashboardRequest) {
    var response = await this.service.getAPIResultDashboardSummary(request);

    if (response.result == ResponseResult.Success) {
      let length = response.data.length;// < CusDashboardProductLength ? response.data.length : CusDashboardProductLength;
      for (var i = 0; i < length; i++) {
        var _sublength = CusDashboardProductLength < i ? 20 : 9;
        response.data[i].name =

          response.data[i].statusName &&
            response.data[i].statusName.length > 11 ?
            response.data[i].statusName.substring(0, _sublength) + ".." :
            response.data[i].statusName;
      }

      setTimeout(() => {
        this.renderPieChart('chartdiv-circle', this.model.apiResultDashboard.dashboardData);
      }, 10);

      this.model.apiResultDashboard.dashboardData = response.data;
      this.model.apiResultDashboard.totalReports = response.totalReports;
      this.bookingIdList = response.bookingIdList;

      this.dataFound.apiResultDashboardDataFound = this.model.apiResultDashboard ? true : false;
      this.rejectionDashboardLoader.apiResultDashboardLoading = false;

      this.model.searchLoading = false;
    }
    else {
      this.rejectionDashboardLoader.apiResultDashboardLoading = false;
      this.dataFound.apiResultDashboardDataFound = false;
      this.bookingIdList = null;
    }

  }

  dateSelectionDefault() {
    this.requestModel.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.requestModel.serviceDateTo = this.calendar.getToday();
  }

  //fetch the customer result chart
  getCustomerResultDashboardSummary(request: RejectionDashboardRequest) {
    this.service.getCustomerResultDashboardSummary(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            let length = response.data.length;// < CusDashboardProductLength ? response.data.length : CusDashboardProductLength;
            for (var i = 0; i < length; i++) {
              var _sublength = CusDashboardProductLength < i ? 20 : 9;
              response.data[i].name =

                response.data[i].statusName &&
                  response.data[i].statusName.length > 11 ?
                  response.data[i].statusName.substring(0, _sublength) + ".." :
                  response.data[i].statusName;
            }
            this.model.customerResultDashboard.dashboardData = response.data;
            this.model.customerResultDashboard.totalReports = response.totalReports;

            setTimeout(() => {
              this.renderPieChart('chartdiv-circle_customer', this.model.customerResultDashboard.dashboardData);
            }, 10);
            this.rejectionDashboardLoader.customerResultDashboardLoading = false;
            this.dataFound.customerResultDashboardDataFound = true;
            this.rejectionDashboardLoader.customerResultDashboardExportLoading = false;
          }
          else {
            this.rejectionDashboardLoader.customerResultDashboardLoading = false;
            this.dataFound.customerResultDashboardDataFound = false;
          }
        },
        error => {
          //this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.rejectionDashboardLoader.customerResultDashboardLoading = false;
          this.rejectionDashboardLoader.customerResultDashboardError = true;
        });

  }

  //get api result export
  getApiResultExport() {
    this.rejectionDashboardLoader.apiResultDashboardExportLoading = true;


    this.service.getApiResultChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "API_Result.xlsx");
        this.rejectionDashboardLoader.apiResultDashboardExportLoading = false;
      },
        error => {
          this.rejectionDashboardLoader.apiResultDashboardExportLoading = false;
        });
  }

  //get customer result export
  getCustomerResultExport() {
    this.rejectionDashboardLoader.customerResultDashboardExportLoading = true;


    this.service.getCustomerResultChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Customer_Result.xlsx");
        this.rejectionDashboardLoader.customerResultDashboardExportLoading = false;
      },
        error => {
          this.rejectionDashboardLoader.customerResultDashboardExportLoading = false;
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
  }

  //fetch the result by product category chart
  getProductCategoryDashboardSummary(request: RejectionDashboardRequest) {

    this.service.getProductCategoryDashboardSummary(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.model.productCategoryDashboard.data = response.data;
            this.model.productCategoryDashboard.xAxis = response.productCategoryList;
            this.model.productCategoryDashboard.totalReports = response.totalReports;
            this.model.productCategoryDashboard.legendList = response.legendList;

            setTimeout(() => {
              this.productCategoryChartFrame();
            }, 10);
            this.rejectionDashboardLoader.productCategoryDashboardLoading = false;
            this.dataFound.productCategoryDashboardDataFound = true;
            this.rejectionDashboardLoader.productCategoryDashboardExportLoading = false;
          }
          else {
            this.rejectionDashboardLoader.productCategoryDashboardLoading = false;
            this.dataFound.productCategoryDashboardDataFound = false;
          }
        },
        error => {
          this.rejectionDashboardLoader.productCategoryDashboardLoading = false;
          this.rejectionDashboardLoader.productCategoryDashboardError = true;
        });

  }

  /**
  * Function to render pie chart
  * @param {[type]} container [description]
  */
  renderPieChart(container: string | HTMLElement, data: string | any[]) {

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
    chart.innerRadius = 45;

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

  productCategoryChartFrame() {

    if (this.model.productCategoryDashboard.xAxis && this.model.productCategoryDashboard.xAxis.length > 0 && this.model.productCategoryDashboard.data &&
      this.model.productCategoryDashboard.data.length > 0) {

      var chartObj = [];

      for (var i = 0; i < this.model.productCategoryDashboard.xAxis.length; i++) {

        var subchartObj = {};

        subchartObj["month"] = this.model.productCategoryDashboard.xAxis[i].name;

        for (var j = 0; j < this.model.productCategoryDashboard.data.length; j++) {

          var monthData = this.model.productCategoryDashboard.data[j].data.filter(x => x.id ==
            (this.model.productCategoryDashboard.xAxis[i].id));

          if (monthData.length > 0)
            subchartObj[this.model.productCategoryDashboard.data[j].resultName.toLowerCase()] = monthData.length == 1 ?
              monthData[0].totalCount : 0;

        }
        subchartObj["none"] = 0;
        chartObj.push(subchartObj);
      }

      setTimeout(() => {
        this.renderProductCategoryChart(chartObj);
      }, 300);
    }

  }

  /**
  *4th chart based on employee type render
  */
  renderProductCategoryChart(chartObj: any[]) {

    this.clusteredColumChart = am4core.create("barchartdiv", am4charts.XYChart);
    this.clusteredColumChart.maskBullets = false;
    this.clusteredColumChart.numberFormatter.numberFormat = "#.#"

    this.clusteredColumChart.data = chartObj;

    let datasize = this.model.productCategoryDashboard.data && this.model.productCategoryDashboard.data[0] ?
      this.model.productCategoryDashboard.data[0].data.length : 0;

    // Create axes
    let categoryAxis = this.clusteredColumChart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "month";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.cellStartLocation = 0.1;
    categoryAxis.renderer.cellEndLocation = 0.9;
    categoryAxis.renderer.labels.template.fontSize = 12;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    if (datasize >= 2) {

      var label = categoryAxis.renderer.labels.template;
      label.truncate = true;
      label.maxWidth = 85;
      label.tooltipText = "{month}";
      categoryAxis.tooltip.label.fontSize = 12;
      categoryAxis.tooltip.getFillFromObject = false;
      categoryAxis.tooltip.background.propertyFields.stroke = "color";
      categoryAxis.tooltip.background.fill = am4core.color("#4c698d");
      categoryAxis.tooltip.label.textAlign = "middle";
    }

    let valueAxis = this.clusteredColumChart.yAxes.push(new am4charts.ValueAxis());
    // valueAxis.min = 0;
    valueAxis.renderer.labels.template.fontSize = 12;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
    valueAxis.calculateTotals = true;

    for (var i = 0; i < this.model.productCategoryDashboard.data.length; i++) {

      let stacked = i == 0 ? false : true;

      this.createSeries(this.model.productCategoryDashboard.data[i].color, this.model.productCategoryDashboard.data[i].resultName.toLowerCase(), this.model.productCategoryDashboard.data[i].resultName, stacked, datasize);
    }

    var totalSeries = this.clusteredColumChart.series.push(new am4charts.ColumnSeries());
    totalSeries.dataFields.valueY = "none";
    totalSeries.dataFields.categoryX = "month";
    totalSeries.stacked = true;
    totalSeries.hiddenInLegend = true;
    totalSeries.columns.template.strokeOpacity = 0;

    let totalBullet = totalSeries.bullets.push(new am4charts.LabelBullet());
    totalBullet.dy = -10;
    totalBullet.label.text = "{valueY.total}";
    totalBullet.label.hideOversized = false;
    totalBullet.label.fontSize = 11;
    totalBullet.label.truncate = false;

  }

  // Create series
  createSeries(color: string | am4core.iRGB | am4core.Color, field: string, name: string, stacked: boolean, datasize: number) {
    var graphwidth = datasize >= 3 ? 80 : 40;
    let series = this.clusteredColumChart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = field;
    series.dataFields.categoryX = "month";
    series.sequencedInterpolation = true;
    series.name = name;
    series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]";
    series.stacked = true;
    series.columns.template.width = am4core.percent(graphwidth);
    series.columns.template.fill = am4core.color(color);
  }

  //get manday by customer export
  getProductCategoryExport() {
    this.rejectionDashboardLoader.productCategoryDashboardExportLoading = true;


    this.service.getProductCategoryChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Reject_Category.xlsx");
        this.rejectionDashboardLoader.productCategoryDashboardExportLoading = false;
      },
        error => {
          this.rejectionDashboardLoader.productCategoryDashboardExportLoading = false;
        });
  }

  //fetch the result based on supplier/ vendor
  getVendorDashboardSummary(request: RejectionDashboardRequest) {

    this.service.getVendorDashboardSummary(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.model.vendorDashboard.data = response.data;
            this.model.vendorDashboard.yAxis = response.supplierList;
            this.model.vendorDashboard.totalReports = response.totalReports;

            setTimeout(() => {
              this.barChartFrame(this.model.vendorDashboard.data, this.model.vendorDashboard.yAxis, true);
            }, 10);
            this.rejectionDashboardLoader.vendorDashboardLoading = false;
            this.dataFound.vendorDashboardDataFound = true;
            this.rejectionDashboardLoader.vendorDashboardExportLoading = false;
          }
          else {
            this.rejectionDashboardLoader.vendorDashboardLoading = false;
            this.dataFound.vendorDashboardDataFound = false;
          }
        },
        error => {
          this.rejectionDashboardLoader.vendorDashboardLoading = false;
          this.rejectionDashboardLoader.vendorDashboardError = true;
        });

  }

  barChartFrame(data: string | any[], yAxis: string | any[], isVendor: boolean) {

    if (yAxis && yAxis.length > 0 && data &&
      data.length > 0) {

      var chartObj = [];

      for (var i = 0; i < yAxis.length; i++) {

        var subchartObj = {};

        subchartObj["category"] = yAxis[i].name;//.substring(0, 20);


        for (var j = 0; j < data.length; j++) {

          var monthData = data[j].data.filter((x: { id: any; }) => x.id ==
            (yAxis[i].id));


          if (isVendor) {
            if (monthData.length > 0) {

              subchartObj[data[j].resultName.toLowerCase()] = monthData.length == 1 ?
                monthData[0].totalCount : 0;
            }
          }
          //if country chart, all the status except pass should be on the negative side of the graph
          else {
            if (monthData.length > 0) {
              if (data[j].resultName.toLowerCase() == "pass") {
                subchartObj[data[j].resultName.toLowerCase()] = monthData.length == 1 ?
                  monthData[0].percentage : 0;
              }
              //set the values of all the status except pass to nagative
              else {
                subchartObj[data[j].resultName.toLowerCase()] = monthData.length == 1 ?
                  -monthData[0].percentage : 0;
              }
            }
          }

        }
        subchartObj["none"] = 0;
        chartObj.push(subchartObj);
      }

      setTimeout(() => {
        isVendor ? this.renderVendorChart(chartObj) : this.renderCountryChart(chartObj);
      }, 10);
    }

  }


  /**
  *4th chart based on vendor render
  */
  renderVendorChart(chartObj: any[]) {

    this.clusteredRowchart = am4core.create("horizontalBar-chart", am4charts.XYChart);
    this.clusteredRowchart.maskBullets = false;
    this.clusteredRowchart.numberFormatter.numberFormat = "#.#"

    this.clusteredRowchart.data = chartObj;

    let datasize = this.model.vendorDashboard.data && this.model.vendorDashboard.data[0].data ?
      this.model.vendorDashboard.data[0].data.length : 0;

    // Create axes
    let categoryAxis = this.clusteredRowchart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = 'category';
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.grid.template.opacity = 0;
    categoryAxis.renderer.baseGrid.disabled = true;
    categoryAxis.renderer.labels.template.fontSize = 12;
    categoryAxis.renderer.labels.template.fill = am4core.color('#A1A8B3');
    categoryAxis.renderer.cellStartLocation = datasize <= 3 ? 0.2 : 0.1;
    categoryAxis.renderer.cellEndLocation = datasize <= 3 ? 0.6 : 0.9;

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

    var valueAxis = this.clusteredRowchart.xAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    //valueAxis.max = 2100;
    valueAxis.renderer.grid.template.opacity = 1;
    valueAxis.renderer.ticks.template.strokeOpacity = 0.5;
    valueAxis.renderer.ticks.template.stroke = am4core.color('#495C43');
    valueAxis.renderer.line.strokeOpacity = 0;
    valueAxis.renderer.baseGrid.disabled = false;
    valueAxis.renderer.labels.template.disabled = false;
    valueAxis.renderer.labels.template.fill = am4core.color('#AAAFB6');
    valueAxis.renderer.labels.template.fontSize = 12;
    valueAxis.calculateTotals = true;

    //valueAxis.max = this.model.vendorDashboard.maxXAxisValue;
    for (var i = 0; i < this.model.vendorDashboard.data.length; i++) {

      let stacked = i == 0 ? false : true;

      this.createSeriesHorizontalBar(this.model.vendorDashboard.data[i].color, this.model.vendorDashboard.data[i].resultName.toLowerCase(), this.model.vendorDashboard.data[i].resultName, stacked, datasize);
    }

    var totalSeries = this.clusteredRowchart.series.push(new am4charts.ColumnSeries());
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



  // Create series
  createSeriesHorizontalBar(color: string | am4core.iRGB | am4core.Color, field: string, name: string, stacked: boolean, datasize: number) {
    var graphwidth = datasize >= 5 ? 85 : datasize >= 3 ? 40 : 10;
    var series = this.clusteredRowchart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueX = field;
    series.dataFields.categoryY = "category";
    series.name = name;
    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
    series.stacked = stacked;
    series.columns.template.width = am4core.percent(graphwidth);
    series.columns.template.fill = am4core.color(color);
  }

  //get result by supplier/ vendor export
  getVendorExport() {
    this.rejectionDashboardLoader.vendorDashboardExportLoading = true;


    this.service.getVendorChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Reject_Vendor.xlsx");
        this.rejectionDashboardLoader.vendorDashboardExportLoading = false;
      },
        error => {
          this.rejectionDashboardLoader.vendorDashboardExportLoading = false;
        });
  }

  clearSupplier() {
    if (this.currentUser.usertype != UserType.Factory)
      this.requestModel.popUpFactoryId = null;
    this.model.factoryList = [];
    if (this.currentUser.usertype == UserType.Factory){
      this.getFactListBySearch();
    }
  }

  //fetch the result by country
  getCountryDashboardSummary(clearInnerDropdown: boolean) {

    if (this.bookingIdList) {

      //set the request with the selected country Id
      let request = new CountryChartRequest();
      request.searchRequest = this.requestModel;
      request.countryId = this.requestModel.countryId;
      request.clearSelection = clearInnerDropdown;

      this.service.getCountryDashboardSummary(request)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response.result == ResponseResult.Success) {
              this.model.countryDashboard.data = response.data;
              this.model.countryDashboard.yAxis = response.yAxisData;
              this.model.countryDashboard.totalReports = response.totalReports;

              this.model.innerCountryList = response.countryList.filter(
                (thing, i, arr) => arr.findIndex(t => t.id === thing.id) === i
              );

              this.requestModel.countryId = response.selectedCountryId;

              setTimeout(() => {
                this.barChartFrame(this.model.countryDashboard.data, this.model.countryDashboard.yAxis, false);
              }, 10);

              this.rejectionDashboardLoader.countryDashboardLoading = false;
              this.dataFound.countryDashboardDataFound = true;
              this.rejectionDashboardLoader.countryDashboardExportLoading = false;
            }
            else {
              this.rejectionDashboardLoader.countryDashboardLoading = false;
              this.dataFound.countryDashboardDataFound = false;
            }
          },
          error => {
            this.rejectionDashboardLoader.countryDashboardLoading = false;
            this.rejectionDashboardLoader.countryDashboardError = true;
          });
    }
    else {
      this.rejectionDashboardLoader.countryDashboardLoading = false;
      this.dataFound.countryDashboardDataFound = false;
      this.model.innerCountryList = [];
    }
  }

  /**
    *4th chart based on country render
    */
  renderCountryChart(chartObj: any[]) {

    this.clusteredRowCountryChart = am4core.create("chartdiv_country", am4charts.XYChart);

    this.clusteredRowCountryChart.data = chartObj;

    let datasize = this.model.countryDashboard.data && this.model.countryDashboard.data[0].data ?
      this.model.countryDashboard.data[0].data.length : 0;

    // Create axes
    let categoryAxis = this.clusteredRowCountryChart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "category";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.inversed = true;
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.axisFills.template.disabled = false;
    //categoryAxis.renderer.axisFills.template.fillOpacity = 0.05;
    categoryAxis.renderer.labels.template.fontSize = 12;
    categoryAxis.renderer.cellStartLocation = datasize <= 3 ? 0.2 : 0.1;
    categoryAxis.renderer.cellEndLocation = datasize <= 3 ? 0.6 : 0.9;
    categoryAxis.renderer.grid.template.opacity = 0;
    categoryAxis.renderer.baseGrid.disabled = true;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    //categoryAxis.columns.template.tooltipText = "fullName";

    let valueAxis = this.clusteredRowCountryChart.xAxes.push(new am4charts.ValueAxis());
    valueAxis.min = -100;
    valueAxis.max = 100;
    //valueAxis.renderer.minGridDistance = 20;
    //valueAxis.renderer.ticks.template.length = 5;
    //valueAxis.renderer.ticks.template.disabled = false;
    valueAxis.renderer.ticks.template.strokeOpacity = 0.5;
    valueAxis.renderer.labels.template.fontSize = 12;
    valueAxis.renderer.labels.template.adapter.add("text", function (text: any) {
      return text;
    })
    valueAxis.renderer.grid.template.opacity = 1;
    valueAxis.renderer.line.strokeOpacity = 0;
    valueAxis.renderer.baseGrid.disabled = false;
    valueAxis.renderer.labels.template.disabled = false;
    valueAxis.renderer.ticks.template.stroke = am4core.color('#495C43');
    valueAxis.renderer.labels.template.fill = am4core.color('#AAAFB6');

    // Use only absolute numbers
    this.clusteredRowCountryChart.numberFormatter.numberFormat = "#.#s";

    //valueAxis.max = this.model.vendorDashboard.maxXAxisValue;
    for (var i = 0; i < this.model.countryDashboard.data.length; i++) {

      let stacked = i == 0 ? false : true;

      this.createSeriesCountryHorizontalBar(this.model.countryDashboard.data[i].color, this.model.countryDashboard.data[i].resultName.toLowerCase(), this.model.countryDashboard.data[i].resultName, stacked, datasize);
    }
  }

  // Create series
  createSeriesCountryHorizontalBar(color: string, field: string, name: string, stacked: boolean, datasize: number) {
    var graphwidth = datasize >= 5 ? 85 : datasize >= 3 ? 40 : 10;
    let series = this.clusteredRowCountryChart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueX = field;
    series.dataFields.categoryY = "category";
    series.stacked = stacked;
    series.name = name;
    series.stroke = color;
    series.fill = color;
    series.columns.template.width = am4core.percent(graphwidth);
    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]%";
    return series;
  }

  //get manday by customer export
  getCountryResultChartExport() {
    this.rejectionDashboardLoader.countryDashboardExportLoading = true;


    this.service.getCountryChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Reject_Country.xlsx");
        this.rejectionDashboardLoader.countryDashboardExportLoading = false;
      },
        error => {
          this.rejectionDashboardLoader.countryDashboardExportLoading = false;
        });
  }

  //fetch the country data with virtual scroll
  getCountryData() {
    this.model.countryRequest.searchText = this.model.countryInput.getValue();
    this.model.countryRequest.skip = this.model.countryList.length;

    this.model.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.model.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.model.countryList = this.model.countryList.concat(customerData);
        }
        this.model.countryRequest = new CountryDataSourceRequest();
        this.model.countryLoading = false;
      }),
      (error: any) => {
        this.model.countryLoading = false;
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.model.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.model.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.model.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.countryLoading = false))
      ))
      .subscribe(data => {
        this.model.countryList = data;
        this.model.countryLoading = false;
      });
  }

  //get product category list
  getProductCategoryData() {
    this.model.productCategoryListLoading = true;
    this.productManagementService.getProductCategorySummary()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response && response.result == ResponseResult.Success) {
          this.model.productCategoryList = response.productCategoryList;
        }
        this.model.productCategoryListLoading = false;
      }),
      error => {
        this.model.productCategoryListLoading = false;
      };
  }

  getProductListBySearch() {

    this.model.productRequest = new ProductDataSourceRequest();

    this.model.productRequest.customerIds.push(this.requestModel.customerId);
    if (this.requestModel.supplierId > 0) {
      this.model.productRequest.supplierIdList.push(this.requestModel.supplierId);
    }
    else {
      this.model.productRequest.supplierIdList = [];
    }
    //this.dashBoardFilterMaster.productRequest.supplierIdList.push(this.customerFilterModel.factoryId);
    this.model.productInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.productLoading = true),
      switchMap(term => term
        ? this.customerProductService.getProductDataSource(this.model.productRequest, term)
        : this.customerProductService.getProductDataSource(this.model.productRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.productLoading = false))
      ))
      .subscribe(data => {
        this.model.productList = data;
        this.model.productLoading = false;

      });
  }

  //get product data 
  getProductData() {
    this.model.productRequest.searchText = this.model.productInput.getValue();
    this.model.productRequest.skip = this.model.productList.length;
    this.model.productRequest.customerIds.push(this.requestModel.customerId);

    this.model.productRequest.productIds = [];
    this.model.productLoading = true;
    this.customerProductService.getProductDataSource(this.model.productRequest).
      subscribe(productData => {
        if (productData && productData.length > 0) {
          this.model.productList = this.model.productList.concat(productData);
        }

        this.model.productRequest = new ProductDataSourceRequest();
        this.model.productRequest.customerIds.push(this.requestModel.customerId);
        this.model.productLoading = false;
      }),
      error => {
        this.model.productLoading = false;
        this.setError(error);
      };
  }

  //country change event
  innerFilterCountryChange(countryItem: any) {
    this.rejectionDashboardLoader.countryDashboardExportLoading = true;
    this.rejectionDashboardLoader.countryDashboardLoading = true;

    var clearSelectionFlag = (countryItem) ? false : true;

    this.getCountryDashboardSummary(clearSelectionFlag);
  }
  //Rejection Analatics FB Result Change
  innerFilterFBResultChange(fbResultItem: any) {

    this.model.fbReportResultId = fbResultItem.id;

    this.fbReportResultChange();
    this.getRejectDashboardSummary();
  }
  fbReportResultChange() {
    this.model.selectedReasonList = [];
    this.model.fbReportResultReasonList = [];
    this.model.selectedSubCatogoryList = [];
    this.model.fbReportResultSubCatogoryList = [];
    if (this.model.fbReportResultId == FbReportResultType.Fail) {
      this.model.lblfbReportHead = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_REJECTION_ANALYTICS");
      this.model.lblfbReportReason = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_REJECT_REASONS");
      this.model.lblfbReportCount = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_REJECTIONS");
    }
    else if (this.model.fbReportResultId == FbReportResultType.Pending) {
      this.model.lblfbReportHead = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_PENDING_ANALYTICS");
      this.model.lblfbReportReason = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_PENDING_REASONS");
      this.model.lblfbReportCount = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_PENDINGS");
    }
  }

  innerFilterFBResultReasonChange(fbResultReson: any) {

    this.model.selectedReasonList = fbResultReson;
    this.model.selectedSubCatogoryList = [];

    if (this.model.selectedReasonList.length > 0) {
      this.getRejectSubcatogorySummary();
    }
    else {
      this.model.fbReportResultSubCatogoryList = [];
      this.getRejectDashboardSummary();
    }
  }
  innerFilterFBResultSubCaotgoryChange(fbResultsubCatogory: any) {
    this.model.selectedSubCatogoryList = fbResultsubCatogory;
    if (this.model.selectedSubCatogoryList.length > 0)
      this.getRejectSubcatogory2Summary();
    else
      this.getRejectSubcatogorySummary();
  }
  //fetch the reject subgatogory reason based on month and year
  getRejectSubcatogory2Summary() {
    this.model.rejectDashboard = new RejectChartResponse();

    this.rejectionDashboardLoader.rejectDashboardLoading = true;

    if (this.bookingIdList) {
      let request = new RejectChartSubcatogory2Request();
      request.searchRequest = this.requestModel;
      request.fbResultId = this.model.fbReportResultId;
      request.ResultNames = this.model.selectedReasonList;
      request.SubCatogory = this.model.selectedSubCatogoryList;

      this.service.getRejectSubcatogory2Summary(request)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response.result == ResponseResult.Success) {
              this.model.rejectDashboard.data = response.data;
              this.model.rejectDashboard.rejectReasonList = response.rejectReasonList;
              this.model.rejectDashboard.monthNameList = response.monthNameList;

              this.rejectionDashboardLoader.rejectDashboardLoading = false;
              this.dataFound.rejectDashboardDataFound = true;
              this.rejectionDashboardLoader.rejectDashboardExportLoading = false;
            }
            else {
              this.model.rejectDashboard.data = [];
              this.model.rejectDashboard.rejectReasonList = [];
              this.rejectionDashboardLoader.rejectDashboardLoading = false;
              this.dataFound.rejectDashboardDataFound = false;
            }
          },
          error => {
            this.model.rejectDashboard.data = [];
            this.rejectionDashboardLoader.rejectDashboardLoading = false;
            this.rejectionDashboardLoader.rejectDashboardError = true;
          });
    }
    else {
      this.model.rejectDashboard.data = [];
      this.rejectionDashboardLoader.rejectDashboardLoading = false;
      this.dataFound.rejectDashboardDataFound = false;
    }
  }

  //fetch the reject subgatogory reason based on month and year
  getRejectSubcatogorySummary() {

    this.model.rejectDashboard = new RejectChartResponse();

    this.rejectionDashboardLoader.rejectDashboardLoading = true;

    if (this.bookingIdList) {
      let request = new RejectChartSubcatogoryRequest();
      request.searchRequest = this.requestModel;
      request.fbResultId = this.model.fbReportResultId;
      request.ResultNames = this.model.selectedReasonList;

      this.service.getRejectSubcatogorySummary(request)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response.result == ResponseResult.Success) {
              this.model.rejectDashboard.data = response.data;
              this.model.fbReportResultSubCatogoryList = response.rejectReasonList

              this.model.rejectDashboard.rejectReasonList = response.rejectReasonList;
              this.model.rejectDashboard.monthNameList = response.monthNameList;

              this.rejectionDashboardLoader.rejectDashboardLoading = false;
              this.dataFound.rejectDashboardDataFound = true;
              this.rejectionDashboardLoader.rejectDashboardExportLoading = false;
            }
            else {
              this.model.rejectDashboard.data = [];
              this.model.rejectDashboard.rejectReasonList = [];
              this.rejectionDashboardLoader.rejectDashboardLoading = false;
              this.dataFound.rejectDashboardDataFound = false;
            }
          },
          error => {
            this.model.rejectDashboard.data = [];
            this.rejectionDashboardLoader.rejectDashboardLoading = false;
            this.rejectionDashboardLoader.rejectDashboardError = true;
          });
    }
    else {
      this.model.rejectDashboard.data = [];
      this.rejectionDashboardLoader.rejectDashboardLoading = false;
      this.dataFound.rejectDashboardDataFound = false;
    }
  }

  //fetch the reject reason based on month and year
  getRejectDashboardSummary() {

    this.model.rejectDashboard = new RejectChartResponse();
    this.rejectionDashboardLoader.rejectDashboardLoading = true;

    if (this.bookingIdList) {

      let request = new RejectChartRequest();
      request.searchRequest = this.requestModel;
      request.fbResultId = this.model.fbReportResultId;

      this.service.getRejectDashboardSummary(request)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response.result == ResponseResult.Success) {
              this.model.rejectDashboard.data = response.data;
              this.model.fbReportResultReasonList = response.rejectReasonList
              this.model.rejectDashboard.rejectReasonList = response.rejectReasonList;
              this.model.rejectDashboard.monthNameList = response.monthNameList;

              this.rejectionDashboardLoader.rejectDashboardLoading = false;
              this.dataFound.rejectDashboardDataFound = true;
              this.rejectionDashboardLoader.rejectDashboardExportLoading = false;
            }
            else {
              this.model.rejectDashboard.data = [];
              this.rejectionDashboardLoader.rejectDashboardLoading = false;
              this.dataFound.rejectDashboardDataFound = false;
            }
          },
          error => {
            this.model.rejectDashboard.data = [];
            this.rejectionDashboardLoader.rejectDashboardLoading = false;
            this.rejectionDashboardLoader.rejectDashboardError = true;
          });
    }
    else {
      this.model.rejectDashboard.data = [];
      this.rejectionDashboardLoader.rejectDashboardLoading = false;
      this.dataFound.rejectDashboardDataFound = false;
    }
  }


  //fetch the valuse for each field by month, year and reason
  getRejectMonthValue(rejectReason: string, item: MandayYear) {
    var res = this.model.rejectDashboard.data.find(x => x.name == rejectReason);
    var data = res.monthlyData.find(x => x.monthName == item.monthName.substring(0, 3) && x.year == item.year && x.name == rejectReason);
    var result = data ? data.monthCount : "";
    return result;
  }
  getRejectReasonName(rejectReason: string, item: MandayYear) {
    var res = this.model.rejectDashboard.data.find(x => x.name == rejectReason);
    var result = res ? res.reasonName : "";
    return result;
  }
  getSubCatogoryName(rejectReason: string, item: MandayYear) {
    var res = this.model.rejectDashboard.data.find(x => x.name == rejectReason);
    var result = res ? res.subcatogory : "";
    return result;
  }


  //fetch the total value for each reject reason
  getRejectTotalValue(rejectReason: string) {
    var res = this.model.rejectDashboard.data.find(x => x.name == rejectReason);
    var result = res ? res.count : 0;
    return result;
  }

  //fetch the percentage for each reason
  getRejectPercentage(rejectReason: string) {
    var res = this.model.rejectDashboard.data.find(x => x.name == rejectReason);
    var result = res ? res.percentage : 0;
    return result;
  }

  //get the defect color
  getDefectColor(rejectReason: any, item: MandayYear) {

    var count = this.getRejectMonthValue(rejectReason, item);
    var _greaterColor = "rgba(248, 21, 56, 0.6)";
    var _lessColor = "rgba(248, 21, 56)";

    if (this.model.fbReportResultId == FbReportResultType.Fail) {
      _greaterColor = "rgba(248, 21, 56, 0.6)";
      _lessColor = "rgba(248, 21, 56)";
    }
    else if (this.model.fbReportResultId == FbReportResultType.Pending) {
      _greaterColor = "rgba(242, 154, 12, 0.6)";
      _lessColor = "rgba(242, 154, 12)";
    }
    if (count) {
      return count <= 10 ? _greaterColor : _lessColor;
    }
    else
      return "rgba(205, 213, 223, 1)";

  }

  //get the reject reason by month and year data
  getRejectDashboardPopUpSummary(request: RejectionDashboardRequest) {
    this.rejectionDashboardLoader.rejectDashboardPopUpLoading = true;

    if (this.model.fbReportResultId == FbReportResultType.Fail)
      this.loaderMsg = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_PROCESSING_REJECT_DATA");
    else if (this.model.fbReportResultId == FbReportResultType.Pending)
      this.loaderMsg = this.utility.textTranslate("REJECTION_DASHBOARD.LBL_PROCESSING_PENDING_DATA");

    this.service.getRejectDashboardPopUpSummary(request)
      .pipe()
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.model.rejectDashboardPopUpData = response.data;
            this.popUpTableData = this.model.rejectDashboardPopUpData.supplierData;
            this.popUpModel.supplierPopUpList = response.supplierList;
            this.popUpModel.factoryPopUpList = response.factoryList;

            this.rejectionDashboardLoader.rejectDashboardPopUpLoading = false;
            this.dataFound.rejectDashboardPopUpDataFound = true;
          }
          else {
            this.rejectionDashboardLoader.rejectDashboardPopUpLoading = false;
            this.dataFound.rejectDashboardPopUpDataFound = false;
          }
        },
        error => {
          this.rejectionDashboardLoader.rejectDashboardPopUpLoading = false;
          this.rejectionDashboardLoader.rejectDashboardPopUpError = true;
        });
  }

  //reject data opn pop up 
  openRejectionPopup(rejectReason: string, month: MandayYear) {
    this.rejectionCount = this.getRejectMonthValue(rejectReason, month);




    if (this.model.selectedSubCatogoryList != null && this.model.selectedSubCatogoryList.length > 0 && this.rejectionCount > 0) {
      this.popUpMonthData = month;
      this.rejectionPopupState = !this.rejectionPopupState;
      this.rejectionName = this.getRejectReasonName(rejectReason, month) + " -> " + this.getSubCatogoryName(rejectReason, month) + " -> " + rejectReason;
      this.requestModel.month = month.month;
      this.requestModel.year = month.year;
      this.requestModel.rejectReason = rejectReason;
      this.requestModel.FbResultId = this.model.fbReportResultId;
      this.requestModel.SummaryNames = this.model.selectedReasonList;
      this.requestModel.SubcatogoryList = this.model.selectedSubCatogoryList;
      this.requestModel.searchBy = "subcatogory2";
    }
    else if (this.model.selectedReasonList != null && this.model.selectedReasonList.length > 0 && this.rejectionCount > 0) {
      this.popUpMonthData = month;

      this.rejectionName = this.getRejectReasonName(rejectReason, month) + " -> " + rejectReason;
      this.rejectionPopupState = !this.rejectionPopupState;
      this.requestModel.month = month.month;
      this.requestModel.year = month.year;
      this.requestModel.rejectReason = rejectReason;
      this.requestModel.FbResultId = this.model.fbReportResultId;
      this.requestModel.SummaryNames = this.model.selectedReasonList;
      this.requestModel.searchBy = "subcatogory";
    }
    else if (this.rejectionCount > 0) {

      this.popUpMonthData = month;
      this.rejectionPopupState = !this.rejectionPopupState;

      this.rejectionName = rejectReason;
      this.requestModel.month = month.month;
      this.requestModel.year = month.year;
      this.requestModel.rejectReason = rejectReason;
      this.requestModel.FbResultId = this.model.fbReportResultId;
      this.requestModel.searchBy = "summary";
    }
    this.getRejectDashboardPopUpSummary(this.requestModel);


  }

  getCustomerListBySearch() {
    this.model.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.customerLoading = true),
      switchMap(term =>
        this.cusService.getCustomerListByUserType(this.requestCustomerModel, null, term)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.customerLoading = false))
      ))
      .subscribe(data => {
        if (!this.LoadFirstTime)
          this.model.customerList = data;
        else {
          if (data && data.length > 0) {
            this.model.customerList = data;
          }
          this.requestModel.customerId = this.model.customerList[0].id;
          this.model.brandSearchRequest.customerId = this.requestModel.customerId;
          this.model.deptSearchRequest.customerId = this.requestModel.customerId;
          this.model.buyerSearchRequest.customerId = this.requestModel.customerId;
          this.model.collectionSearchRequest.customerId = this.requestModel.customerId;

          if (this.currentUser.usertype == UserType.InternalUser || this.currentUser.usertype == UserType.Customer)
            this.getSupListBySearch();

          this.getBrandListBySearch();
          this.getBuyerListBySearch();
          this.getCollectionListBySearch();
          this.getDeptListBySearch();
          this.getProductListBySearch();
          this.charLoadData();
          this.LoadFirstTime = false;

          if (this.currentUser.usertype == UserType.Supplier || this.currentUser.usertype == UserType.Factory)
            this.requestModel.customerId = this.model.customerList[0].id;
          this.model.customerLoading = false;
        }
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.requestCustomerModel.searchText = this.model.customerInput.getValue();
      this.requestCustomerModel.skip = this.model.customerList.length;
    }

    this.model.customerLoading = true;
    this.cusService.getCustomerListByUserType(this.requestCustomerModel).
      subscribe(customerData => {
        if (IsVirtual) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
            this.model.customerList = this.model.customerList.concat(customerData);
          }
        }
        this.model.customerLoading = false;
      }),
      error => {
        this.model.customerLoading = false;
        this.setError(error);
      };
  }
  changeCustomerData(item) {
    if (item && item.id > 0) {
      this.model.supplierList = [];
      if (this.currentUser.usertype != UserType.Supplier)
        this.requestModel.popUpSupplierId = null;
      this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
      this.model.supplierTypeId = SearchType.SupplierName;
      this.model.supsearchRequest.customerId = item.id;
      this.model.brandSearchRequest.customerId = item.id;
      this.model.deptSearchRequest.customerId = item.id;
      this.model.buyerSearchRequest.customerId = item.id;
      this.model.collectionSearchRequest.customerId = item.id;
      this.getSupListBySearch();
      this.getBrandListBySearch();
      this.getBuyerListBySearch();
      this.getCollectionListBySearch();
      this.getDeptListBySearch();
      this.getProductListBySearch();
      this.getFactListBySearch();
    }
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.model.supsearchRequest.customerId = this.requestModel.customerId;
    this.model.supsearchRequest.supplierType = SupplierType.Supplier;

    //add supplier id filter
    if (this.currentUser.usertype == UserType.Supplier)
      this.model.supsearchRequest.id = this.currentUser.supplierid;

    //get supplier by factory data
    if (this.currentUser.usertype == UserType.Factory)
      this.model.supsearchRequest.factoryId = this.currentUser.factoryid;

    this.model.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.supLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.model.supsearchRequest, term)
        : this.supService.getFactoryDataSourceList(this.model.supsearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.supLoading = false))
      ))
      .subscribe(data => {
        this.model.supplierList = data;
        this.model.supLoading = false;
        if (this.currentUser.usertype == UserType.Supplier)
          this.getFactListBySearch();
        this.stopPlaceHolderLoad();
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.model.supsearchRequest.searchText = this.model.supInput.getValue();
    this.model.supsearchRequest.skip = this.model.supplierList.length;

    this.model.supsearchRequest.customerId = this.requestModel.customerId;
    this.model.supsearchRequest.supplierType = SupplierType.Supplier;
    this.model.supLoading = true;
    this.supService.getFactoryDataSourceList(this.model.supsearchRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.model.supplierList = this.model.supplierList.concat(customerData);
        }
        this.model.supsearchRequest.skip = 0;
        this.model.supsearchRequest.take = ListSize;
        this.model.supLoading = false;
      }),
      (error: any) => {
        this.model.supLoading = false;
      };
  }
  //fetch the brand data with virtual scroll
  getBrandData() {
    this.model.brandSearchRequest.searchText = this.model.brandInput.getValue();
    this.model.brandSearchRequest.skip = this.model.brandList.length;

    this.model.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.model.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.model.brandList = this.model.brandList.concat(brandData);
        }
        this.model.brandSearchRequest = new CommonCustomerSourceRequest();
        this.model.brandLoading = false;
      }),
      (error: any) => {
        this.model.brandLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.model.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.model.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.model.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.brandLoading = false))
      ))
      .subscribe(data => {
        this.model.brandList = data;
        this.model.brandLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getDeptData() {
    this.model.deptSearchRequest.searchText = this.model.deptInput.getValue();
    this.model.deptSearchRequest.skip = this.model.deptList.length;

    this.model.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.model.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.model.deptList = this.model.deptList.concat(deptData);
        }
        this.model.deptSearchRequest = new CommonCustomerSourceRequest();
        this.model.deptLoading = false;
      }),
      (error: any) => {
        this.model.deptLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {
    this.model.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.model.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.model.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.deptLoading = false))
      ))
      .subscribe(data => {
        this.model.deptList = data;
        this.model.deptLoading = false;
      });
  }

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.model.buyerSearchRequest.searchText = this.model.buyerInput.getValue();
    this.model.buyerSearchRequest.skip = this.model.buyerList.length;

    this.model.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.model.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.model.buyerList = this.model.buyerList.concat(buyerData);
        }
        this.model.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.model.buyerLoading = false;
      }),
      (error: any) => {
        this.model.buyerLoading = false;
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.model.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.model.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.model.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.buyerLoading = false))
      ))
      .subscribe(data => {
        this.model.buyerList = data;
        this.model.buyerLoading = false;
      });
  }

  //fetch the collection data with virtual scroll
  getCollectionData() {
    this.model.collectionSearchRequest.searchText = this.model.collectionInput.getValue();
    this.model.collectionSearchRequest.skip = this.model.collectionList.length;

    this.model.collectionLoading = true;
    this.collectionService.getCollectionListByCustomerId(this.model.collectionSearchRequest).
      subscribe(collectionData => {
        if (collectionData && collectionData.length > 0) {
          this.model.collectionList = this.model.collectionList.concat(collectionData);
        }
        this.model.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.model.collectionLoading = false;
      }),
      (error: any) => {
        this.model.collectionLoading = false;
      };
  }

  //fetch the first take (variable) count collection on load
  getCollectionListBySearch() {
    this.model.collectionInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.collectionLoading = true),
      switchMap(term => term
        ? this.collectionService.getCollectionListByCustomerId(this.model.collectionSearchRequest, term)
        : this.collectionService.getCollectionListByCustomerId(this.model.collectionSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.collectionLoading = false))
      ))
      .subscribe(data => {
        this.model.collectionList = data;
        this.model.collectionLoading = false;
      });
  }
  //Make CustomerInfo which will be used to send to child page
  assignCustomerInfo() {
    //let user = this.authService.getCurrentUser();
    if (this.currentUser) {
      if (this.currentUser.customerid > 0) {
        this.customerInfo.id = this.currentUser.customerid;
        this.customerInfo.name = this.currentUser.fullName;
        this.requestModel.customerId = this.customerInfo.id;
      }
    }
  }

  //form validation
  isFormValid(): boolean {
    return this.validator.isValid('serviceDateFrom') && this.validator.isValid('serviceDateTo');
  }

  search() {
    this.model.searchLoading = true;
    this.model.filterDataShown = this.filterTextShown();
    this.validator.initTost();
    this.validator.isSubmitted = true;
    this.requestModel.countryId = null;

    if (this.isFormValid()) {

      this.placeHolderLoad();
      this.charLoadData();
    }
    this.model.searchLoading = false;
  }

  //filter details has to show
  filterTextShown() {
    var isFilterDataSelected = false;

    if (this.requestModel.supplierId > 0
      || (this.requestModel.selectedCountryIdList && this.requestModel.selectedCountryIdList.length > 0) ||
      (this.requestModel.selectedDeptIdList && this.requestModel.selectedDeptIdList.length > 0) ||
      (this.requestModel.selectedBrandIdList && this.requestModel.selectedBrandIdList.length > 0) ||
      (this.requestModel.selectedCollectionIdList && this.requestModel.selectedCollectionIdList.length > 0) ||
      (this.requestModel.selectedBuyerIdList && this.requestModel.selectedBuyerIdList.length > 0)) {

      //desktop version
      if (!this.isMobile) {
        if (this.requestModel.customerId) {
          var customerDetails = this.model.customerList.find((x: { id: number; }) => x.id == this.requestModel.customerId);
          this.model.customerName = customerDetails ? customerDetails.name : "";
        }
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;

        if (this.requestModel.supplierId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.requestModel.selectedCountryIdList && this.requestModel.selectedCountryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.requestModel.selectedBrandIdList && this.requestModel.selectedBrandIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.requestModel.selectedBuyerIdList && this.requestModel.selectedBuyerIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.requestModel.selectedCollectionIdList && this.requestModel.selectedCollectionIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.requestModel.selectedDeptIdList && this.requestModel.selectedDeptIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        this.model.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.model.filterCount = 0;
        this.model.countryListName = "";
        this.model.supplierName = "";
        this.model.customerName = "";
      }
    }

    return isFilterDataSelected;
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  toggleFilter() {
    this.isFilterOpen = !this.isFilterOpen;
  }



  //supplier change event
  supplierChange(supplierItem: { id: number; }) {
    this.model.factoryList = [];
    this.model.factoryId = null;
    if (supplierItem && supplierItem.id > 0) {
      var supplierDetails = this.model.supplierList.find((x: { id: any; }) => x.id == supplierItem.id);
      if (supplierDetails)
        this.model.supplierName =
          supplierDetails.name.length > SupplierNameTrim ?
            supplierDetails.name.substring(0, SupplierNameTrim) + "..." : supplierDetails.name;
      this.getFactListBySearch();
    }
    else {
      this.model.supplierName = "";
    }
  }

  popUpSupplierChange(item: { id: number; }) {
    if (!this.requestModel.popUpSupplierId)
      this.model.rejectDashboardPopUpData.supplierData = this.popUpTableData;

    if (this.requestModel.popUpFactoryId) {
      this.model.rejectDashboardPopUpData.supplierData = this.model.rejectDashboardPopUpData.supplierData.filter(x => x.factoryId == this.requestModel.popUpFactoryId);
    }

    if (item) {
      this.model.rejectDashboardPopUpData.supplierData = this.model.rejectDashboardPopUpData.supplierData.filter(x => x.supplierId == item.id);
    }
  }


  //photo list for critical, major, minor
  getRejectPhotoList(imagepopup: any, supplierid: number, factoryid: number) {
    this.rejectionDashboardLoader.rejectDashboardPopUpLoading = true;
    this.popUpModel.imageLoading = true;
    this.requestModel.popUpSelectedPhotoSupplierId = supplierid;
    this.requestModel.popUpSelectedPhotoFactoryId = factoryid;

    this.service.getRejectImages(this.requestModel)
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              //this.popUpModel.imageList = response.data;
              this.getPreviewImage(response.data, imagepopup);
            }
            else {
              this.popUpModel.imageList = [];
              this.showInfo('DEFECT_DASHBOARD.LBL_TITLE', 'DEFECT_DASHBOARD.LBL_NO_PHOTOS');
            }
          }
          this.popUpModel.imageLoading = false;
          this.rejectionDashboardLoader.rejectDashboardPopUpLoading = false;
        },
        error => {
          this.popUpModel.imageLoading = false;
          this.rejectionDashboardLoader.rejectDashboardPopUpLoading = false;
        });
  }

  //photo show in popup using gallery
  getPreviewImage(imageList: any[], modalcontent: any) {

    //remove duplicates
    var images = imageList.filter(
      (thing, i, arr) => arr.findIndex(t => t.imagePath === thing.imagePath) === i
    );

    this.popUpModel.rejectionImages = [];
    images.forEach((url: { imagePath: any; description: any; }) => {
      this.popUpModel.rejectionImages.push(
        {
          small: url.imagePath,
          medium: url.imagePath,
          big: url.imagePath,
          description: url.description
        });
    });

    this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  popUpFactoryChange(item: { id: number; }) {
    if (!this.requestModel.popUpSupplierId)
      this.model.rejectDashboardPopUpData.supplierData = this.popUpTableData;

    if (this.requestModel.popUpSupplierId) {
      this.model.rejectDashboardPopUpData.supplierData = this.model.rejectDashboardPopUpData.supplierData.filter(x => x.supplierId == this.requestModel.popUpSupplierId);
    }

    if (item) {
      this.model.rejectDashboardPopUpData.supplierData = this.model.rejectDashboardPopUpData.supplierData.filter(x => x.factoryId == item.id);
    }
  }

  closePopUp() {
    this.rejectionPopupState = false;
    if (this.currentUser.usertype != UserType.Factory)
      this.requestModel.popUpFactoryId = null;
    if (this.currentUser.usertype != UserType.Supplier)
      this.requestModel.popUpSupplierId = null;
  }

  cancelFilter() {
    this.requestModel = new RejectionDashboardRequest();
    this.model.filterDataShown = false;
  }

  //country change event
  countryChange(countryItem: any) {

    if (countryItem) {

      if (this.requestModel.selectedCountryIdList && this.requestModel.selectedCountryIdList.length > 0) {
        this.model.countryListName = "";

        var customerLength = this.requestModel.selectedCountryIdList.length < CountrySelectedFilterTextCount ?
          this.requestModel.selectedCountryIdList.length : CountrySelectedFilterTextCount;
        for (var i = 0; i < customerLength; i++) {

          var countryDetails = this.model.countryList.find((x: { id: any; }) => x.id == this.requestModel.selectedCountryIdList[i]);

          if (i != customerLength - 1) {
            this.model.countryListName += countryDetails.name + ", ";
          }
          else {
            if (customerLength < CountrySelectedFilterTextCount) {
              this.model.countryListName += countryDetails.name;
            }
            else {
              this.model.countryListName += countryDetails.name + "...";
            }
          }
        }
      }
      else {
        this.model.countryListName = "";
      }
    }
  }

  reset() {
    this.initialize();

    this.dateSelectionDefault();
    this.assignCustomerInfo();
    this.placeHolderLoad();

    this.toggleFilter();

    if (this.currentUser.usertype == UserType.Supplier)
      this.requestModel.supplierId = this.currentUser.supplierid;

    if (this.currentUser.usertype == UserType.Factory) {
      this.requestModel.factoryId = this.currentUser.factoryid;
      this.getFactListBySearch();
    }

    this.getCustomerListBySearch();
    this.getSupListBySearch();

    //this.model.apiResultDashboard.dashboardData = new Array<ResultDashboard>();


    this.getCountryListBySearch();

  }
  //download the report
  openReport(reportData) {
    if (reportData && reportData.finalManualReportLink)
      window.open(reportData.finalManualReportLink);
    else if (reportData && reportData.reportLink)
      window.open(reportData.reportLink);
  }

  exportRejectionDashboardAnalysisTable() {

    this.rejectionDashboardLoader.rejectDashboardTableExportLoading = true;
    let request = new RejectChartSubcatogory2Request();
    request.searchRequest = this.requestModel;
    request.fbResultId = this.model.fbReportResultId;
    request.ResultNames = this.model.selectedReasonList;
    request.SubCatogory = this.model.selectedSubCatogoryList;

    this.service.exportRejectDashboardAnalysisTable(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RejectionDashboardAnalysisTable.xlsx");
        this.rejectionDashboardLoader.rejectDashboardTableExportLoading = false;
      },
        error => {
          this.rejectionDashboardLoader.rejectDashboardTableExportLoading = false;
        });
  }

  clearProductRef() {
    this.getProductListBySearch();
  }

  changeProductRef() {
    this.getProductListBySearch();
  }

  //fetch the first 10 fact for the supplier on load
  getFactListBySearch() {
    this.model.factoryList = null;
    this.model.factoryRequest.supplierType = SupplierType.Factory;
    this.model.factoryRequest.supplierId = this.requestModel.supplierId;

    this.model.factoryRequest.customerId = this.requestModel.customerId;

    if (this.currentUser.usertype == UserType.Factory) {
      this.model.factoryRequest.id = this.currentUser.factoryid;
      this.requestModel.factoryId = this.currentUser.factoryid;
      this.model.factoryId = this.currentUser.factoryid;
    }
    this.model.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.model.factoryLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryOrSupplierDataSource(this.model.factoryRequest, term)
        : this.supService.getFactoryOrSupplierDataSource(this.model.factoryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.model.factoryLoading = false))
      ))
      .subscribe(data => {
        this.model.factoryList = data;
        this.model.factoryLoading = false;
        this.stopPlaceHolderLoad();
      });
  }

  /*   //factory change event  get factory selected name for shown in strip
    factoryChange(factoryItem) {
      if (factoryItem && factoryItem.id > 0) {
        var factoryDetails = this.model.factoryList.find(x => x.id == factoryItem.id);
        if (factoryDetails)
          this.model.factoryName =
            factoryDetails.name.length > DefectDashboardSupplierNameTrim ?
              factoryDetails.name.substring(0, DefectDashboardSupplierNameTrim) + "..." : factoryDetails.name;
      }
      else {
        this.model.factoryName = "";
      }
    } */

  //fetch the facotry data with virtual scroll
  getFactoryData() {
    this.model.factoryRequest.searchText = this.model.factoryInput.getValue();
    this.model.factoryRequest.skip = this.model.factoryList.length;

    this.model.factoryRequest.supplierType = SupplierType.Factory;
    this.model.factoryLoading = true;
    this.model.factoryRequest.supplierId = this.requestModel.supplierId;

    this.supService.getFactoryOrSupplierDataSource(this.model.factoryRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        if (data && data.length > 0) {
          this.model.factoryList = this.model.factoryList.concat(data);
        }
        this.model.factoryRequest = new FactoryDataSourceRequest();
        this.model.factoryLoading = false;
      }),
      error => {
        this.model.factoryLoading = false;
        this.setError(error);
      };
  }
  changeSupplierType(item) {
    this.model.supLoading = true;
    this.model.supplierList = [];
    if (this.currentUser.usertype != UserType.Supplier)
      this.requestModel.popUpSupplierId = null;
    if (this.currentUser.usertype != UserType.Factory)
      this.requestModel.popUpFactoryId = null;
    if (item.id == SearchType.SupplierCode) {
      this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierCode;
    }
    else if (item.id == SearchType.SupplierName) {
      this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    }
    this.getSupplierData();
  }

  //make Defect Landing Request
  getRejectionRateLandingRequest() {
    const groupByFilter = GroupByFilter.filter(x => x.id != GroupByFilterEnum.Brand);
    groupByFilter.forEach(element => {
      this.rejectionRateLandingRequest.groupByFilter.push(element.id);
    });
    this.rejectionRateLandingRequest.serviceDateFrom = this.requestModel.serviceDateFrom;
    this.rejectionRateLandingRequest.serviceDateTo = this.requestModel.serviceDateTo;
    this.rejectionRateLandingRequest.customerId = this.requestModel.customerId;
    this.rejectionRateLandingRequest.supplierId = this.requestModel.supplierId;
    this.rejectionRateLandingRequest.supplierTypeId = this.model.supplierTypeId;
    this.rejectionRateLandingRequest.selectedFactoryIdList = this.requestModel.selectedFactoryIdList;
    this.rejectionRateLandingRequest.selectedBrandIdList = this.requestModel.selectedBrandIdList;
    this.rejectionRateLandingRequest.selectedProdCategoryIdList = this.requestModel.selectedProdCategoryIdList;
    this.rejectionRateLandingRequest.selectedCountryIdList = this.requestModel.selectedCountryIdList;
    this.rejectionRateLandingRequest.searchtypeid = SearchType.BookingNo;
    this.rejectionRateLandingRequest.pageSize = 10;
    this.rejectionRateLandingRequest.index = 0;
  }

  navigateRejectionRate() {
    this.getRejectionRateLandingRequest();
    const path = "rejectrate/rejection-rate/";
    const entity: string = this.utility.getEntityName();
    const basedUrl = window.location.href.replace(this.router.url,'');
    const url = this.router.createUrlTree([`/${entity}/${path}`], {
      queryParams: { param: encodeURI(JSON.stringify(this.rejectionRateLandingRequest)) }
    })
    window.open(basedUrl + url);
  }
}


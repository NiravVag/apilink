import { Component, Renderer2, OnInit, ViewChild, ElementRef } from '@angular/core';
import mapboxgl from 'mapbox-gl';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { trigger, state, style, transition, animate } from '@angular/animations';
import { NgbDate, NgbCalendar, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { ManagementDashBoardDataFound, ManagementDashboardFilterMaster, ManagementDashBoardLoader, ManagementDashboardModel, ManagementDashboardRequest, MandayChartYear, MandayDashboard, MandayYear, MandayYearChart, OverviewChartResponse, ResponseResult } from 'src/app/_Models/dashboard/managementdashboard.model';
import { Validator } from '../../../common'
import { ManagementDashBoardService } from 'src/app/_Services/dashboard/managementdashboard.service';
import { MapGeoLocationResult } from 'src/app/_Models/dashboard/customerdashboard.model';
import { DashBoardService } from 'src/app/_Services/dashboard/dashboard.service';
import { CommonDataSourceRequest, CountryDataSourceRequest } from 'src/app/_Models/common/common.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { bookingStatusList, CountrySelectedFilterTextCount, CusDashboardProductLength, CusDashboardProductNameTrim, ListSize, MobileViewFilterCount, SupplierNameTrim, SupplierType } from 'src/app/components/common/static-data-common';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { DateTimePickerControl } from 'src/app/_Models/dynamiccontrols/control-datepicker.model';
import { DashboardMapFilterRequest, DashboardType } from 'src/app/_Models/dashboard/customerdashboardfilterrequest.model';

@Component({
  selector: 'app-managementdashboard',
  templateUrl: './managementdashboard.component.html',
  styleUrls: ['./managementdashboard.component.scss'],
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
export class ManagementdashboardComponent {

  componentDestroyed$: Subject<boolean> = new Subject();
  [x: string]: any;//extends DetailComponent {

  requestModel: ManagementDashboardRequest;
  model: ManagementDashboardModel;
  dataFound: ManagementDashBoardDataFound;
  bookingIdList: Array<number>;
  managementDashboardLoader: ManagementDashBoardLoader;
  monthYearXAxis: Array<MandayYear>;
  centreLatitude = 30.77;
  centreLongitude = 11.25;
  markers = [];
  mapLoading: boolean;
  requestSupModel: CommonDataSourceRequest;
  requestFactModel: CommonDataSourceRequest;
  requestCustomerModel: CommonDataSourceRequest;
  countryRequest: CountryDataSourceRequest;
  dashBoardFilterMaster: ManagementDashboardFilterMaster;

  @ViewChild('leftColumn') leftColumn: ElementRef;
  @ViewChild('sideOverlay') sideOverlay: ElementRef;
  @ViewChild('mapCard') mapCard: ElementRef;

  @ViewChild('datepicker') datepicker;

  mapObj: any;
  error: boolean;
  isMobile: boolean;
  mapHeight: number;
  searchQuery: string;
  isFullscreen: boolean;
  isFilterOpen: boolean;
  searchActive: boolean;
  hasSearchResult: boolean;
  showFactoryResult: boolean;
  hoveredDate: NgbDate | null = null;


  constructor(private renderer: Renderer2, private calendar: NgbCalendar, public formatter: NgbDateParserFormatter, private dashboardService: DashBoardService,
    private supService: SupplierService, private service: ManagementDashBoardService, private locationService: LocationService, private customerService: CustomerService,
    public validator: Validator, private mandayDashboardService: MandayDashboardService) {

    this.getIsMobile();
    this.error = false;
    this.mapHeight = 380;
    this.isFilterOpen = false;
    this.searchActive = false;
    this.showFactoryResult = false;
    this.validator.setJSON("customer/customer-dashboardsummary.valid.json");
    this.validator.setModelAsync(() => this.requestModel);
    this.validator.isSubmitted = false;
    am4core.addLicense("CH238479116");
  }


  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  async onInit() {

    this.requestModel = new ManagementDashboardRequest();
    this.dashBoardFilterMaster = new ManagementDashboardFilterMaster();
    this.requestSupModel = new CommonDataSourceRequest();
    this.requestFactModel = new CommonDataSourceRequest();
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.countryRequest = new CountryDataSourceRequest();
    this.requestModel.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 7);
    this.requestModel.serviceDateTo = this.calendar.getToday();
    //this.requestModel.serviceDateTo = this.calendar.getPrev(this.calendar.getToday(), 'm', 8);
    this.initialize();
  }

  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }

  async initialize() {

    this.model = new ManagementDashboardModel();
    this.dataFound = new ManagementDashBoardDataFound();

    this.managementDashboardLoader = new ManagementDashBoardLoader();

    this.getSupListBySearch();
    this.getCountryListBySearch();
    this.getFactListBySearch();
    this.getCustomerListBySearch();
    this.getOfficeList();

    var response = await this.service.getDashboardSummary(this.requestModel)
    this.model.managementDashboardCount = response.data;
    this.bookingIdList = response.inspectionIdList;
    this.dataFound.managementDashboardCountDataFound = this.model.managementDashboardCount ? true : false;

    //get the office access list of the logged in user from the server side and select them in the office dropdown and display the list in ribbon
    if (!(this.requestModel.officeIdList != null && this.requestModel.officeIdList.length > 0)) {
      if (response.officeIds != null && response.officeIds.length > 0) {
        this.requestModel.officeIdList = response.officeIds;
        this.dashBoardFilterMaster.officeNameList = this.dashBoardFilterMaster.officeList.filter(x => response.officeIds.includes(x.id)).map(x => x.name);
        this.dashBoardFilterMaster.filterDataShown = true;
      }
    }

    this.getOverviewDashboardSummary(this.requestModel);
    this.getRejectDashboardSummary(this.requestModel);
    this.getProductCategoryDashboardSummary(this.requestModel);
    this.getServiceTypeDashboardSummary(this.requestModel);
    this.getResultDashboardSummary(this.requestModel);
    this.getMandaysByYear(this.requestModel);
    this.getBookingAverageTimeDashboardSummary(this.requestModel);
    this.getQuotationAverageTimeDashboardSummary(this.requestModel);
    this.setmapcountry();
  }

  getOverviewDashboardSummary(bookingIdList) {
    this.service.getOverviewDashboardSummary(bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.model.overviewChart = response.data;
            this.dataFound.overviewDataFound = true;
            this.managementDashboardLoader.overviewChartLoading = false;
          }
          else {
            this.dataFound.overviewDataFound = false;
            this.managementDashboardLoader.overviewChartLoading = false;
          }
        },
        error => {
          //this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.managementDashboardLoader.overviewChartLoading = false;
          this.managementDashboardLoader.overviewChartError = true;
        });
  }

  getRejectDashboardSummary(bookingIdList) {
    this.service.getRejectDashboardSummary(bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            let length = response.data.length < CusDashboardProductLength ? response.data.length : CusDashboardProductLength;
            for (var i = 0; i < length; i++) {
              response.data[i].name =

                response.data[i].statusName &&
                  response.data[i].statusName.length > 37 ?
                  response.data[i].statusName.substring(0, 37) + ".." :
                  response.data[i].statusName;
            }
            this.model.rejectChart = response.data;
            setTimeout(() => {
              this.halfDonutInspectionRejectChart();
            }, 10);
            this.dataFound.rejectDataFound = true;
            this.managementDashboardLoader.rejectChartLoading = false;
          }
          else {
            this.dataFound.rejectDataFound = false;
            this.managementDashboardLoader.rejectChartLoading = false;
          }
        },
        error => {
          //this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.managementDashboardLoader.rejectChartLoading = false;
          this.managementDashboardLoader.rejectChartError = true;
        });
  }

  getProductCategoryDashboardSummary(bookingIdList) {
    this.service.getProductCategoryDashboardSummary(bookingIdList)
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
            this.model.prodCategoryChart = response.data;

            setTimeout(() => {
              this.renderPieChart('chartdiv', this.model.prodCategoryChart);
            }, 10);

            this.dataFound.prodCategoryDataFound = true;
            this.managementDashboardLoader.prodCategoryChartLoading = false;
            this.managementDashboardLoader.productCategoryExportLoading = false;
          }
          else {
            this.dataFound.prodCategoryDataFound = false;
            this.managementDashboardLoader.prodCategoryChartLoading = false;
          }
        },
        error => {
          //this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.managementDashboardLoader.prodCategoryChartLoading = false;
          this.managementDashboardLoader.prodCategoryChartError = true;
        });
  }

  getResultDashboardSummary(bookingIdList) {
    this.service.getResultDashboardSummary(bookingIdList)
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
            this.model.resultDashboard = response.data;

            setTimeout(() => {
              this.renderPieChart('chartdiv-circle2', this.model.resultDashboard);
            }, 10);
            this.managementDashboardLoader.resultDashboardLoading = false;
            this.dataFound.resultDashboardDataFound = true;
            this.managementDashboardLoader.resultExportLoading = false;
          }
          else {
            this.managementDashboardLoader.resultDashboardLoading = false;
            this.dataFound.resultDashboardDataFound = false;
          }
        },
        error => {
          //this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.managementDashboardLoader.resultDashboardLoading = false;
          this.managementDashboardLoader.resultDashboardError = true;
        });
  }

  getServiceTypeDashboardSummary(bookingIdList) {
    this.service.getServiceTypeDashboardSummary(bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            let length = response.data.length;// < CusDashboardProductLength ? response.data.length : CusDashboardProductLength;
            for (var i = 0; i < length; i++) {
              var _sublength = CusDashboardProductLength < i ? 18 : 9;
              response.data[i].name =

                response.data[i].statusName &&
                  response.data[i].statusName.length > 11 ?
                  response.data[i].statusName.substring(0, _sublength) + ".." :
                  response.data[i].statusName;
            }
            this.model.serviceTypeDashboard = response.data;

            setTimeout(() => {
              this.renderPieChart('chartdiv-circle3', this.model.serviceTypeDashboard);
            }, 10);
            this.dataFound.serviceTypeDashboardDataFound = true;
            this.managementDashboardLoader.serviceTypeDashboardLoading = false;
            this.managementDashboardLoader.serviceTypeExportLoading = false;
          }
          else {
            this.dataFound.serviceTypeDashboardDataFound = false;
            this.managementDashboardLoader.serviceTypeDashboardLoading = false;
          }
        },
        error => {
          //this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.managementDashboardLoader.serviceTypeDashboardLoading = false;
          this.managementDashboardLoader.serviceTypeDashboardError = true;
        });
  }

  getMandaysByYear(request) {
    this.model.mandayDashboard = new MandayDashboard();
    this.service.getManDayDashboardSummary(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.model.mandayDashboard.mandayData = response.data;
              this.model.mandayDashboard.budget = response.budget;
              this.monthYearXAxis = response.monthYearXAxis;
              this.mandayByYearChartFrame();
              this.dataFound.manDayDashboardDataFound = true;
            }
            else {
              this.dataFound.manDayDashboardDataFound = false;
            }
          }
          this.managementDashboardLoader.manDayDashboardLoading = false;
        },
        error => {
          this.managementDashboardLoader.manDayDashboardLoading = false;
          this.managementDashboardLoader.manDayDashboardError = true;
        });
  }

  // frame the manday year
  mandayByYearChartFrame() {
    let k: number = 2;
    if (this.model.mandayDashboard.mandayData && this.model.mandayDashboard.mandayData.length > 0) {

      // to build the value2 we are declare the below 2  

      var chartObj = [];

      //building below structure
      //{date:new Date(2019,1), value2:48, value3:51, value4:42}
      for (var i = 0; i < this.model.mandayDashboard.mandayData.length; i++) {

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

          var monthData = this.model.mandayDashboard.mandayData[i].monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + 1));

          //logic to avoid showing 0 for future months data
          if (this.model.mandayDashboard.mandayData[i].year == this.calendar.getToday().year) {
            if ((chartObj[m].date.getMonth() + 1) <= this.calendar.getToday().month)
              chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;
          }

          else
            chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;
        }
        k = k + 1;
      }
    }

    for (var m = 0; m < chartObj.length; m++) {

      var monthData = this.model.mandayDashboard.budget.monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + 1));

      chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;
    }

    setTimeout(() => {
      this.renderYearLineChart(chartObj, this.model.mandayDashboard.mandayData);
    }, 100);
  }

  /**
  * Function to render line chart for manday by year
  */
  renderYearLineChart(chartObj, mandayYearChar: MandayYearChart[]) {

    let chart = am4core.create("chartdivManday", am4charts.XYChart);

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

    categoryAxis.dateFormats.setKey("month", "MMM");
    categoryAxis.periodChangeDateFormats.setKey("month", "MMM");

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
      series.tooltipText = mandayYearChar[i].year + ": [b]{valueY}[/]"; //"{dateX}: [b]{valueY}[/]";
      series.tooltip.getFillFromObject = false;
      series.tooltip.background.fill = am4core.color(mandayYearChar[i].color);
    }

    let series1 = chart.series.push(new am4charts.LineSeries());
    series1.legendSettings.valueText = "[bold]{valueY.close}[/]";
    series1.stroke = am4core.color(this.model.mandayDashboard.budget.color);
    series1.dataFields.valueY = "value" + (mandayYearChar.length + 2);
    series1.dataFields.dateX = "date";
    series1.strokeDasharray = "2,3";
    series1.name = 'value_budget';
    series1.strokeWidth = 2;
    series1.tooltipText = this.model.mandayDashboard.budget.year + ": [b]{valueY}[/]"; //"{dateX}: [b]{valueY}[/]";
    //series1.tooltipText = "{dateX}: [b]{valueY}[/]";
    series1.tooltip.getFillFromObject = false;
    series1.tooltip.background.fill = am4core.color(this.model.mandayDashboard.budget.color);

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

  halfDonutInspectionRejectChart() {
    let chart = am4core.create('halfdonutchart', am4charts.PieChart);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

    chart.data = this.model.rejectChart;

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

  setmapcountry() {
    if (this.requestModel) {
      const request: DashboardMapFilterRequest = {
        customerId: this.requestModel.customerId,
        factoryIds: this.requestModel.factoryIdList,
        productCategoryIds: [],
        productIds: [],
        brandIds: [],
        buyerIds: [],
        collectionIds: [],
        countryIds: [],
        serviceDateFrom: this.requestModel.serviceDateFrom,
        serviceDateTo: this.requestModel.serviceDateTo,
        departmentIds: [],
        statusIds: this.requestModel.statusIdList ?? bookingStatusList.map(x => x.id),
        supplierId: this.requestModel.supplierId,
        officeIds: this.requestModel.officeIdList,
        supplierIds: this.requestModel.selectedSupplierIdList,
        dashboardType: DashboardType.ManagementDashboard
      };
      this.dashboardService.getInspCountryGeoCode(request)
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
    this.requestSupModel.customerId = this.requestModel.customerId;
    this.requestSupModel.supplierType = SupplierType.Supplier;
    this.dashBoardFilterMaster.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.supLoading = true),
      switchMap(term => term
        ? this.supService.GetSupplierList(this.requestSupModel, term)
        : this.supService.GetSupplierList(this.requestSupModel)
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
      this.requestSupModel.searchText = this.dashBoardFilterMaster.supInput.getValue();
      this.requestSupModel.skip = this.dashBoardFilterMaster.supplierList.length;
    }
    this.requestSupModel.customerId = this.requestModel.customerId;
    this.requestSupModel.supplierType = SupplierType.Supplier;
    this.dashBoardFilterMaster.supLoading = true;
    this.supService.GetSupplierList(this.requestSupModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.supplierList = this.dashBoardFilterMaster.supplierList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.requestSupModel.skip = 0;
          this.requestSupModel.take = ListSize;
        }
        this.dashBoardFilterMaster.supLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.supLoading = false;
        this.setError(error);
      };
  }

  //country change event
  countryChange(countryItem) {

    if (countryItem) {

      if (this.requestModel.countryIdList && this.requestModel.countryIdList.length > 0) {

        var countryLength = this.requestModel.countryIdList.length;

        var countryDetails = [];
        for (var i = 0; i < countryLength; i++) {

          countryDetails.push(this.dashBoardFilterMaster.countryList.find(x => x.id == this.requestModel.countryIdList[i]).name);
        }
        this.dashBoardFilterMaster.countryNameList = countryDetails;
      }
      else {
        this.this.dashBoardFilterMaster.countryNameList = [];
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
    }
    else {
      this.dashBoardFilterMaster.supplierName = "";
    }
  }

  //fetch the first 10 suppliers for the customer on load
  getFactListBySearch() {
    this.requestSupModel.customerId = this.requestModel.customerId;
    this.requestFactModel.supplierType = SupplierType.Factory;
    this.dashBoardFilterMaster.factInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.factLoading = true),
      switchMap(term => term
        ? this.supService.GetFactoryList(this.requestFactModel, term)
        : this.supService.GetFactoryList(this.requestFactModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.factLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.factoryList = data;
        this.dashBoardFilterMaster.factLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestFactModel.searchText = this.dashBoardFilterMaster.factInput.getValue();
      this.requestFactModel.skip = this.dashBoardFilterMaster.factoryList.length;
    }
    this.requestSupModel.customerId = this.requestModel.customerId;
    this.requestFactModel.supplierType = SupplierType.Factory;
    this.dashBoardFilterMaster.factLoading = true;
    this.supService.GetFactoryList(this.requestFactModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.factoryList = this.dashBoardFilterMaster.factoryList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.requestFactModel.skip = 0;
          this.requestFactModel.take = ListSize;
        }
        this.dashBoardFilterMaster.factLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.factLoading = false;
        this.setError(error);
      };
  }

  //supplier change event
  factoryChange(item) {
    if (item) {

      if (this.requestModel.factoryIdList && this.requestModel.factoryIdList.length > 0) {

        var factoryLength = this.requestModel.factoryIdList.length;

        var factoryDetails = [];
        for (var i = 0; i < factoryLength; i++) {

          factoryDetails.push(this.dashBoardFilterMaster.factoryList.find(x => x.id == this.requestModel.factoryIdList[i]).name);
        }
        this.dashBoardFilterMaster.factoryNameList = factoryDetails;
      }
      else {
        this.this.dashBoardFilterMaster.factoryNameList = [];
      }
    }
  }

  getCustomerListBySearch() {

    this.dashBoardFilterMaster.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.requestCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.customerLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.customerList = data;
        this.dashBoardFilterMaster.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestCustomerModel.searchText = this.dashBoardFilterMaster.customerInput.getValue();
      this.requestCustomerModel.skip = this.dashBoardFilterMaster.customerList.length;
    }

    this.dashBoardFilterMaster.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.requestCustomerModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.customerList = this.dashBoardFilterMaster.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
        }
        this.dashBoardFilterMaster.customerLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.customerLoading = false;
        this.setError(error);
      };
  }

  changeCustomerData(item) {
    if (item && item.id > 0) {
      var customerDetails = this.dashBoardFilterMaster.customerList.find(x => x.id == item.id);
      if (customerDetails)
        this.dashBoardFilterMaster.customerName =
          customerDetails.name.length > SupplierNameTrim ?
            customerDetails.name.substring(0, SupplierNameTrim) + "..." : customerDetails.name;
    }
    else {
      this.dashBoardFilterMaster.customerName = "";
    }
  }

  getBookingAverageTimeDashboardSummary(bookingIdList) {
    this.service.getAverageBookingStatusChangeDashboardSummary(bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.model.bookingAverageTimeDashboard = response.data;
            this.dataFound.averageBookingTimeDashboardDataFound = true;
            this.managementDashboardLoader.bookingAverageTimeDashboardLoading = false;
          }
          else {
            this.dataFound.averageBookingTimeDashboardDataFound = false;
            this.managementDashboardLoader.bookingAverageTimeDashboardLoading = false;
          }
        },
        error => {
          this.managementDashboardLoader.bookingAverageTimeDashboardLoading = false;
          this.managementDashboardLoader.bookingAverageTimeDashboardError = true;
        });
  }

  getQuotationAverageTimeDashboardSummary(bookingIdList) {
    this.service.getAverageQuotationStatusChangeDashboardSummary(bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.model.quotationAverageTimeDashboard = response.data;
            this.dataFound.averageQuotationTimeDashboardDataFound = true;
            this.managementDashboardLoader.quotationAverageTimeDashboardLoading = false;
          }
          else {
            this.dataFound.averageQuotationTimeDashboardDataFound = false;
            this.managementDashboardLoader.quotationAverageTimeDashboardLoading = false;
          }
        },
        error => {
          this.managementDashboardLoader.quotationAverageTimeDashboardLoading = false;
          this.managementDashboardLoader.quotationAverageTimeDashboardError = true;
        });
  }

  changeMandayChartType() {
    this.getMandaysByYear(this.requestModel);
  }

  filterTextShown() {
    var isFilterDataSelected = false;
    //|| this.customerFilterModel.customerId > 0
    if ((this.requestModel.customerId != null && this.requestModel.customerId > 0) || (this.requestModel.supplierId != null && this.requestModel.supplierId > 0)
      || (this.requestModel.countryIdList && this.requestModel.countryIdList.length > 0) || (this.requestModel.officeIdList && this.requestModel.officeIdList.length > 0)
      || (this.requestModel.factoryIdList && this.requestModel.factoryIdList.length > 0)) {

      //desktop version
      if (!this.isMobile) {
        if (this.requestModel.customerId) {
          var customerDetails = this.dashBoardFilterMaster.customerList.find(x => x.id == this.requestModel.customerId);
          this.dashBoardFilterMaster.customerName = customerDetails ? customerDetails.name : "";
        }
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;

        if (this.requestModel.supplierId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.requestModel.countryIdList && this.requestModel.countryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        this.dashBoardFilterMaster.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.dashBoardFilterMaster.filterCount = 0;
        this.dashBoardFilterMaster.countryNameList = [];
        this.dashBoardFilterMaster.supplierName = "";
        this.dashBoardFilterMaster.customerName = "";
        this.dashBoardFilterMaster.officeNameList = [];
      }
    }

    return isFilterDataSelected;
  }

  cancelFilter() {
    this.requestModel.mandayChartType = 1;
    this.requestModel.countryIdList = [];
    this.requestModel.customerId = null;
    this.requestModel.supplierId = null;
    this.requestModel.factoryIdList = [];
    this.requestModel.officeIdList = [];
    //this.setDefaultFilter();
    //this.getCustomerDashBoard();
    this.isFilterOpen = false;
    this.dashBoardFilterMaster.filterDataShown = false;
    this.dashBoardFilterMaster.supplierName = null;
    this.dashBoardFilterMaster.countryNameList = [];
    this.dashBoardFilterMaster.factoryNameList = [];
    this.dashBoardFilterMaster.customerName = null;
    this.dashBoardFilterMaster.officeNameList = [];

  }

  isHovered(date: NgbDate) {
    return this.requestModel.serviceDateFrom && !this.requestModel.serviceDateTo && this.hoveredDate && date.after(this.requestModel.serviceDateFrom) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.requestModel.serviceDateTo && date.after(this.requestModel.serviceDateFrom) && date.before(this.requestModel.serviceDateTo);
  }

  isRange(date: NgbDate) {
    return date.equals(this.requestModel.serviceDateFrom) || (this.requestModel.serviceDateTo && date.equals(this.requestModel.serviceDateTo)) || this.isInside(date) || this.isHovered(date);
  }

  ngAfterViewInit() {
    this.renderMap();
    //this.renderLineChart();
    //this.renderPieChart('chartdiv-circle', 1);
    //this.renderPieChart_('chartdiv-circle2', 2);
    //this.renderPieChart_('chartdiv-circle3', 3);
    //this.halfdonutchart('halfdonutchart', 1);
  }

  onDateSelection(date: NgbDate, isMobile: boolean) {
    if (!this.requestModel.serviceDateFrom && !this.requestModel.serviceDateTo) {
      this.requestModel.serviceDateFrom = date;
    } else if (this.requestModel.serviceDateFrom && !this.requestModel.serviceDateTo && date && date.after(this.requestModel.serviceDateFrom)) {
      this.requestModel.serviceDateTo = date;
      this.datepicker.close();
    } else {
      this.requestModel.serviceDateTo = null;
      this.requestModel.serviceDateFrom = date;
    }

    if (this.requestModel.serviceDateFrom != null && this.requestModel.serviceDateTo != null && !isMobile)
      this.initialize();
  }

  search() {

    this.dashBoardFilterMaster.filterDataShown = this.filterTextShown();
    this.mapLoading = true;

    this.initialize();
  }

  getIsMobile() {
    this.onInit();
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
  }

  openSearchBox(event) {

    if (!this.isMobile) {
      this.searchQuery = '';
      this.hasSearchResult = false;
      this.searchActive = !this.searchActive;
      event.currentTarget.classList.toggle('active');
      //this.searchContainer.nativeElement.classList.toggle('active');
    }
    else {
      //this.mobileSearchContainer.nativeElement.classList.toggle('active');
    }
  }

  toggleFilter(mobile) {
    if (mobile) {
      this.validator.isSubmitted = true;
      if (this.formValid()) {
        this.isFilterOpen = !this.isFilterOpen;
        if (window.innerWidth < 450) {

          if (this.isFilterOpen) {
            document.body.classList.add('disable-scroll');
          }
          else {
            document.body.classList.remove('disable-scroll');
          }
        }
        this.search();
      }
    }

    else {
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
  }

  formValid() {

    var e = Date.parse(this.requestModel.serviceDateFrom);
    return this.validator.isValid('serviceDateTo') &&
      this.validator.isValid('serviceDateFrom');

  }

  //get manday by customer export
  getProductCategoryExport() {
    this.managementDashboardLoader.productCategoryExportLoading = true;


    this.service.getProductCategoryChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Product_Category_Data.xlsx");
        this.managementDashboardLoader.productCategoryExportLoading = false;
      },
        error => {
          this.managementDashboardLoader.productCategoryExportLoading = false;
        });
  }

  //get manday by customer export
  getServiceTypeExport() {
    this.managementDashboardLoader.serviceTypeExportLoading = true;


    this.service.getServiceTypeChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Service_Type_Data.xlsx");
        this.managementDashboardLoader.serviceTypeExportLoading = false;
      },
        error => {
          this.managementDashboardLoader.serviceTypeExportLoading = false;
        });
  }

  //get manday by customer export
  getResultExport() {
    this.managementDashboardLoader.resultExportLoading = true;


    this.service.getResultChartExport(this.requestModel)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Report_Result_Data.xlsx");
        this.managementDashboardLoader.resultExportLoading = false;
      },
        error => {
          this.managementDashboardLoader.resultExportLoading = false;
        });
  }
  //download the excel file
  async downloadFile(data, mimeType, fileName) {
    const blob = new Blob([data], { type: mimeType });

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, fileName);
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fileName;
      a.href = url;
      a.click();
    }
  }

  //get office list
  getOfficeList() {
    this.dashBoardFilterMaster.officeLoading = true;
    this.mandayDashboardService.getOfficeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.dashBoardFilterMaster.officeList = response.dataSourceList;
          this.dashBoardFilterMaster.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.dashBoardFilterMaster.officeLoading = false;
        });
  }

  changeOffice(officeItem) {
    if (officeItem) {

      if (this.requestModel.officeIdList && this.requestModel.officeIdList.length > 0) {

        var officeLength = this.requestModel.officeIdList.length;

        var officeDetails = [];
        for (var i = 0; i < officeLength; i++) {

          officeDetails.push(this.dashBoardFilterMaster.officeList.find(x => x.id == this.requestModel.officeIdList[i]).name);
        }
        this.dashBoardFilterMaster.officeNameList = officeDetails;
      }
      else {
        this.this.dashBoardFilterMaster.officeNameList = [];
      }
    }
  }


  convertDataToPositiveValues(value: number) {
    return Math.abs(value);
  }
}


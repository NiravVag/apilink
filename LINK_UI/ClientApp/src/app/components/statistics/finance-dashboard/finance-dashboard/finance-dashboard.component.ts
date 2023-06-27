import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, NgZone, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { DetailComponent } from 'src/app/components/common/detail.component';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { FinanceDashboardFilterMaster, FinanceDashboardLoader, FinanceDashboardModel, FinanceDashboardRequestModel, MandayYearChart, MandayYearChartItem, TableModel,RatioTableModel } from 'src/app/_Models/statistics/finance-dashboard.model';
import { FinanceDashboardService } from 'src/app/_Services/statistics/finance-dashboard.service';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import { MandayYear } from 'src/app/_Models/statistics/manday-dashboard.model';
import { CustomerTaskEnum } from 'src/app/_Models/dashboard/customerdashboard.model';
import { amCoreLicense, ListSize, MobileViewFilterCount, Service, SupplierNameTrim, SupplierType } from 'src/app/components/common/static-data-common';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { Validator } from 'src/app/components/common/validator';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';

@Component({
  selector: 'app-finance-dashboard',
  templateUrl: './finance-dashboard.component.html',
  styleUrls: ['./finance-dashboard.component.scss'],
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
export class FinanceDashboardComponent extends DetailComponent {

  cars: Array<any>;
  isMobile: boolean;
  isLoading: boolean;
  isFilterOpen: boolean;
  isAdvanceSearch: boolean;
  isRatioAnalysis:boolean;


  componentDestroyed$: Subject<boolean> = new Subject();
  requestModel: FinanceDashboardRequestModel;
  model: FinanceDashboardModel;
  monthYearXAxis: Array<MandayYear>;
  summaryModel: FinanceDashboardLoader;
  tableData: TableModel;
  bookingIdList: Array<number>;
  colorSet: any;
  dashBoardFilterMaster: FinanceDashboardFilterMaster;
  toggleFormSection: boolean;
  filterDataShown: boolean;
  filterCount: number = MobileViewFilterCount;
  mandayChartColorArray: any = ["#5bc0de", "#10ed39" , "#6b03a394"]

  constructor(private calendar: NgbCalendar, private service: FinanceDashboardService, private supService: SupplierService, private locationService: LocationService,
    router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService, private zone: NgZone, private customerService: CustomerService,
    private brandService: CustomerBrandService, private buyerService: CustomerbuyerService, private deptService: CustomerDepartmentService, 
    private refService: ReferenceService, public validator: Validator

  ) {
    super(router, route, translate, toastr);
    
    am4core.addLicense(amCoreLicense);
    this.validator.setJSON("statistics/finance-dashboard/finance-dashboard.valid.json");
    this.validator.setModelAsync(() => this.requestModel);
    this.validator.isSubmitted = false;
  }


  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(): void {
    this.getIsMobile();
    this.requestModel = new FinanceDashboardRequestModel();
    this.dateSelectionDefault();
    this.initialize();
    this.filterDataShown = true;
  }

  initialize() {
    this.model = new FinanceDashboardModel();

    this.summaryModel = new FinanceDashboardLoader();
    this.dashBoardFilterMaster = new FinanceDashboardFilterMaster();
    this.tableData = new TableModel();
    this.chartLoad();
    this.colorSet = new am4core.ColorSet();
    this.requestModel.isBilledMandayExport = !this.summaryModel.tableTabActive;
    this.filterListLoad();

  }

  async chartLoad() {

    this.getBilledMandayData();
    this.getMandayRateData();
    await this.getBookingIdList();
    this.clickMandayRate();
    if(this.bookingIdList && this.bookingIdList.length > 0){
    this.getFinanceDashboardTurnOverSummary();
    this.getChargeBackData();
    this.getQuotationData();
    this.getRatioAnalysis();
    this.getRatioCustomerListBySearch();
   
    }
    else{
      this.summaryModel.countryDataFound = false;
      this.summaryModel.prodCategoryDataFound = false;
      this.summaryModel.servicetypeDataFound = false;
      this.summaryModel.chargeBackDataFound = false;
      this.summaryModel.quotationDataFound = false;

      this.summaryModel.countryChartLoading = false;
      this.summaryModel.prodCategoryChartLoading = false;
      this.summaryModel.servicetypeChartLoading = false;
      this.summaryModel.chargeBackChartLoading = false;
      this.summaryModel.quotationchartLoading = false;

      this.summaryModel.countryChartExportLoading = false;
      this.summaryModel.prodCategoryChartExportLoading = false;
      this.summaryModel.servicetypeChartExportLoading = false;
    }
  }

  filterListLoad() {
    this.getSupListBySearch();
    this.getCountryListBySearch();
    this.getFactListBySearch();
    this.getCustomerListBySearch();
    this.getOfficeList();
    this.getServiceTypeList();
  }

  dateSelectionDefault() {
    this.requestModel.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 7);
    this.requestModel.serviceDateTo = this.calendar.getToday();
  }

  async getBookingIdList() {
    var response = await this.service.getBookingData(this.requestModel);
    this.bookingIdList = response.data;
  }

  getBilledMandayData() {
    this.service.getBilledMandayData(this.requestModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.model.billedMandayData.data = response.billedMandayData;
          this.model.billedMandayData.budget = response.billedMandayBudget;
          this.monthYearXAxis = response.monthYearXAxis;

          let i =0;
          this.model.billedMandayData.data.forEach(element => {
            element.color = this.mandayChartColorArray[i++];
       
          });

          this.model.billedMandayData.budget.color = this.mandayChartColorArray[i];


          this.mandayByYearChartFrame(this.model.billedMandayData.data, this.model.billedMandayData.budget, "billedManDayChart", true);
          this.summaryModel.billedMandayDataFound = true;
          this.summaryModel.billedMandayChartLoading = false;
          this.summaryModel.tableExportLoading = false;

          
          this.dashBoardFilterMaster.searchLoading = false;

        }
        else {
          this.summaryModel.billedMandayDataFound = false;
          this.summaryModel.billedMandayChartLoading = false;
          this.summaryModel.tableExportLoading = false;

          this.dashBoardFilterMaster.searchLoading = false;
        }
      },
        error => {
          this.summaryModel.billedMandayChartLoading = false;
          this.summaryModel.billedMandayChartError = true;
          this.summaryModel.tableExportLoading = false;

          this.dashBoardFilterMaster.searchLoading = false;
        });
  }

  getMandayRateData() {
    this.service.getMandayRateData(this.requestModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.model.mandayRateData.data = response.mandayRateData;
          this.model.mandayRateData.budget = response.mandayRateBudget;
          this.monthYearXAxis = response.monthYearXAxis;
          
          let i =0;
          this.model.mandayRateData.data.forEach(element => {
            element.color = this.mandayChartColorArray[i++];
       
          });

          this.model.mandayRateData.budget.color = this.mandayChartColorArray[i];

          setTimeout(() => {
            this.mandayByYearChartFrame(this.model.mandayRateData.data, this.model.mandayRateData.budget, "mandayRateChart", false);

          }, 100);
          this.summaryModel.mandayRateDataFound = true;
          this.summaryModel.mandayRateChartLoading = false;
        }
        else {

          this.summaryModel.mandayRateDataFound = false;
          this.summaryModel.mandayRateChartLoading = false;
        }
      },
        error => {

          this.summaryModel.mandayRateChartLoading = false;
          this.summaryModel.mandayRateChartError = true;
        });
  }

  getFinanceDashboardTurnOverSummary() {
    this.service.getFinanceDashboardTurnOverData(this.bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.model.countryChartData = response.countryData;
          this.model.prodCategoryChartData = response.productCategoryData;
          this.model.servicetypeChartData = response.servieTypeData;

          this.model.countryChartData.forEach(x => {
            x.Color = this.colorSet.next();
          });

          this.model.prodCategoryChartData.forEach(x => {
            x.Color = this.colorSet.next();
          });

          this.model.servicetypeChartData.forEach(x => {
            x.Color = this.colorSet.next();
          });

          setTimeout(() => {
            this.renderPieChart('country_piechart', this.model.countryChartData);
          }, 10);

          setTimeout(() => {
            this.renderPieChart('prodCat_piechart', this.model.prodCategoryChartData);
          }, 10);

          setTimeout(() => {
            this.renderPieChart('serviceType_piechart', this.model.servicetypeChartData);
          }, 10);

          this.summaryModel.countryDataFound = response.countryData && response.countryData.length > 0 ? true :false;
          this.summaryModel.countryChartLoading = false;

          this.summaryModel.prodCategoryDataFound = response.productCategoryData && response.productCategoryData.length > 0 ? true :false;
          this.summaryModel.prodCategoryChartLoading = false;

          this.summaryModel.servicetypeDataFound = response.servieTypeData && response.servieTypeData.length > 0 ? true :false;
          this.summaryModel.servicetypeChartLoading = false;
        }
        else {
          this.summaryModel.countryDataFound = false;
          this.summaryModel.countryChartLoading = false;

          this.summaryModel.prodCategoryDataFound = false;
          this.summaryModel.prodCategoryChartLoading = false;

          this.summaryModel.servicetypeDataFound = false;
          this.summaryModel.servicetypeChartLoading = false;
        }
      },
        error => {
          this.summaryModel.countryChartLoading = false;
          this.summaryModel.countryChartError = true;

          this.summaryModel.prodCategoryChartLoading = false;
          this.summaryModel.prodCategoryChartError = true;

          this.summaryModel.servicetypeChartLoading = false;
          this.summaryModel.servicetypeChartError = true;
        });
  }

  // frame the manday year
  mandayByYearChartFrame(data, budget, chartId, isBilledMandayData) {
    let k: number = 2;
    if (data && data.length > 0) {

      // to build the value2 we are declare the below 2  

      var chartObj = [];

      //building below structure
      //{date:new Date(2019,1), value2:48, value3:51, value4:42}
      for (var i = 0; i < 2; i++) {

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

          var monthData = data[i] ? data[i].monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + 1)): [];

          //logic to avoid showing 0 for future months data
          if (data[i] && data[i].year == this.calendar.getToday().year) {
            if ((chartObj[m].date.getMonth() + 1) <= this.calendar.getToday().month)
              chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;
          }

          else
            chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;


          let item;
          //item.month = m;
          //item.monthManDay = monthData.length == 1 ? monthData[0].monthManDay : 0;
          item = monthData.length == 1 ? monthData[0].monthManDay : 0;
          if (i == 0) {

            if (isBilledMandayData) {
              this.model.billedMandayTableData.actual.push(item);
            }

            else
              this.model.mandayRateTableData.actual.push(item);
          }
          else {
            if (isBilledMandayData)
              this.model.billedMandayTableData.lastYear.push(item);

            else
              this.model.mandayRateTableData.lastYear.push(item);
          }
        }
        k = k + 1;
      }
    }

    for (var m = 0; m < chartObj.length; m++) {

      var monthData = budget.monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + 1));

      chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;

      if (isBilledMandayData) {
        let item = monthData && monthData.length == 1 ? monthData[0].monthManDay : 0;
        this.model.billedMandayTableData.budget.push(item);
      }
      else {
        let item = monthData && monthData.length == 1 ? monthData[0].monthManDay : 0;
        this.model.mandayRateTableData.budget.push(item);
      }
    }

    this.tableData = this.model.mandayRateTableData;
    setTimeout(() => {
      this.renderYearLineChart(chartObj, data, budget, chartId);
    }, 100);
  }

  /**
  * Function to render line chart for manday by year
  */
  renderYearLineChart(chartObj, mandayYearChar: MandayYearChart[], budget: MandayYearChart, chartId) {

    let chart = am4core.create(chartId, am4charts.XYChart);

    chart.data = chartObj;
    chart.dateFormatter.dateFormat = "dd/MM/yyyy";
    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.DateAxis());
    categoryAxis.dataFields.date = "date";
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.labels.template.fontSize = 12;
    categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    categoryAxis.dateFormatter = new am4core.DateFormatter();
    categoryAxis.dateFormatter.dateFormat = "MM-dd";

    categoryAxis.dateFormats.setKey("month", "MMM");
    categoryAxis.periodChangeDateFormats.setKey("month", "MMM");

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minGridDistance = 20;
    valueAxis.renderer.labels.template.fontSize = 12;
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
      series.tooltipText = "{dateX.formatDate('MMM')}-" + mandayYearChar[i].year + ": [b]{valueY}[/]"; //"{dateX}: [b]{valueY}[/]";
      series.tooltip.getFillFromObject = false;
      series.tooltip.background.fill = am4core.color(mandayYearChar[i].color);
      
		if(chartId == "mandayRateChart")series.tensionX = 0.8;
    }

    let series1 = chart.series.push(new am4charts.LineSeries());
    series1.legendSettings.valueText = "[bold]{valueY.close}[/]";
    series1.stroke = am4core.color(budget.color);
    series1.dataFields.valueY = "value" + 4;
    series1.dataFields.dateX = "date";
    series1.strokeDasharray = "2,3";
    series1.name = 'value_budget';
    series1.strokeWidth = 2;
    series1.tooltipText = "{dateX.formatDate('MMM-yyyy')}: [b]{valueY}[/]"; //budget.year + "{dateX}: [b]{valueY}[/]";;
    //series1.tooltipText = "{dateX}: [b]{valueY}[/]";
    series1.tooltip.getFillFromObject = false;
    series1.tooltip.background.fill = am4core.color(budget.color);

    chart.padding(20, 0, 20, 0);
    chart.cursor = new am4charts.XYCursor();

    //this.exportChart(chart);

  }

  clickBilledManday() {
    this.isRatioAnalysis=false;
    this.summaryModel.tableTabActive = false;
    this.summaryModel.billedmandayTabActive = true;
    this.summaryModel.mandayrateTabActive = false;
    this.summaryModel.ratioTabActive = false;
    this.requestModel.isBilledMandayExport = true;
    this.tableData = this.model.billedMandayTableData;
  }

  clickMandayRate() {
    this.isRatioAnalysis=false;
    this.summaryModel.tableTabActive = true;
    this.summaryModel.billedmandayTabActive = false;
    this.summaryModel.mandayrateTabActive = true;
    this.summaryModel.ratioTabActive = false;
    this.requestModel.isBilledMandayExport = false;
    this.tableData = this.model.mandayRateTableData;
  }
  clickRationAnalysis() {
    this.isRatioAnalysis=true;
    this.summaryModel.tableTabActive = false;
    this.summaryModel.billedmandayTabActive = false;
    this.summaryModel.mandayrateTabActive = false;
    this.summaryModel.ratioTabActive = true;
    this.requestModel.isBilledMandayExport = true;
    this.getRatioAnalysis();
    this.getEmployeeTypes();
  }
  getRatioAnalysis(){
    this.dashBoardFilterMaster.tableLoading = true;

    this.service.getRatioAnalysisData(this.requestModel)
    .pipe(takeUntil(this.componentDestroyed$))
    .subscribe(response => {
      if (response.result == ResponseResult.Success) {
        this.mapProperties( response.data)
        this.dashBoardFilterMaster.tableLoading = false;
      }
      else {
        this.model.RatioAnalysisTableData =null;
        this.dashBoardFilterMaster.tableLoading = false;
      }
    },
      error => {
        this.model.RatioAnalysisTableData =null;
        this.dashBoardFilterMaster.tableLoading = false;
       
      });
  }
  mapProperties(data){
 
    this.model.RatioAnalysisTableData= data.map((x) => {
    return  {
      customerId:x.customerId,
      customer:x.customer,
      billedManday:x.billedManday,
      actualManday:x.actualManday.toFixed(2),
      productionManday:x.productionManday.toFixed(2),
      ratio :x.ratio.toFixed(2),
      revenue:x.revenue,
      billedAvgManday:x.billedAvgManday.toFixed(2),
      productionAvgManday :x.productionAvgManday.toFixed(2),
      isTotal:x.isTotal,
      chargeBack: x.chargeBack,
      totalExpense: x.totalExpense,
      netIncome: x.netIncome,
      netMDRate: x.netIncomeAvg
    };
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
    if (data && data.length > 0) {

      for (var i = 0; i < data.length; i++) {
        chartObj.push({
          "sector": data[i].name,
          "size": data[i].count,
          "color": data[i].Color
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
    label.text = "$ {values.value.sum.formatNumber('#.0a')}";
    label.fontSize = 16;
    label.verticalCenter = "middle";
    label.horizontalCenter = "middle";
    label.fontFamily = "roboto-medium";
  }

  getChargeBackData() {
    this.service.getChargeBackData(this.bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.model.chargeBackChartData = response.data;

          this.summaryModel.chargeBackDataFound = true;
          this.summaryModel.chargeBackChartLoading = false;
        }
        else {

          this.summaryModel.chargeBackDataFound = false;
          this.summaryModel.chargeBackChartLoading = false;
        }
      },
        error => {

          this.summaryModel.chargeBackChartLoading = false;
          this.summaryModel.chargeBackChartError = true;
        });
  }

  getQuotationData() {
    this.service.getQuotationData(this.bookingIdList)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this.model.quotationData = response.data;

          this.summaryModel.quotationDataFound = true;
          this.summaryModel.quotationchartLoading = false;
        }
        else {

          this.summaryModel.quotationDataFound = false;
          this.summaryModel.quotationchartLoading = false;
        }
      },
        error => {

          this.summaryModel.quotationchartLoading = false;
          this.summaryModel.quotationChartError = true;
        });
  }

  //get manday by customer export
  getBilledMandayChartExport() {
    this.summaryModel.tableExportLoading = true;

    if(!this.isRatioAnalysis){
        let fileName = this.requestModel.isBilledMandayExport ? "Billed_Manday.xlsx" : "Manday_Rate.xlsx";
        this.service.getBilledMandayChartExport(this.requestModel)
          .pipe(takeUntil(this.componentDestroyed$))
          .subscribe(res => {
            this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              fileName);
            this.summaryModel.tableExportLoading = false;
          },
            error => {
              this.summaryModel.tableExportLoading = false;
            });
      }
      else{
        this.service.exportRatioAnalysis(this.requestModel)
        .subscribe(res => {
          this.summaryModel.tableExportLoading = false;
          this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ratio_Analysis");
        },
          error => {
            this.summaryModel.tableExportLoading = false;
          });
      }
  }

  //download the excel file
  downloadFile(data, mimeType, fileName) {
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

  //get country turnover export
  getCountryChartExport() {
    this.summaryModel.countryChartExportLoading = true;

    this.service.getCountryTurnoverChartExport(this.requestModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe( res => {
         this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Country_Turnover.xlsx");
        this.summaryModel.countryChartExportLoading = false;
      },
        error => {
          this.summaryModel.countryChartExportLoading = false;
        });
  }

  //get product category turnover export
  getProductCategoryChartExport() {
    this.summaryModel.prodCategoryChartExportLoading = true;

    this.service.getProdCategoryTurnoverChartExport(this.requestModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "ProdCategory_Turnover.xlsx");
        this.summaryModel.prodCategoryChartExportLoading = false;
      },
        error => {
          this.summaryModel.prodCategoryChartExportLoading = false;
        });
  }

  //get service type turnover export
  getServiceTypeChartExport() {
    this.summaryModel.servicetypeChartExportLoading = true;

    this.service.getServiceTypeTurnoverChartExport(this.requestModel)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "ServiceType_Turnover.xlsx");
        this.summaryModel.servicetypeChartExportLoading = false;
      },
        error => {
          this.summaryModel.servicetypeChartExportLoading = false;
        });
  }

  //fetch the country data with virtual scroll
  getCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.dashBoardFilterMaster.countryRequest.searchText = this.dashBoardFilterMaster.countryInput.getValue();
      this.dashBoardFilterMaster.countryRequest.skip = this.dashBoardFilterMaster.countryList.length;
    }

    this.dashBoardFilterMaster.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.dashBoardFilterMaster.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.countryList = this.dashBoardFilterMaster.countryList.concat(customerData);
        }
        if (isDefaultLoad)
          this.dashBoardFilterMaster.countryRequest = new CountryDataSourceRequest();
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
        ? this.locationService.getCountryDataSourceList(this.dashBoardFilterMaster.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.dashBoardFilterMaster.countryRequest)
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
    this.dashBoardFilterMaster.supsearchRequest = new CommonDataSourceRequest();
    this.dashBoardFilterMaster.supplierList = [];
    this.dashBoardFilterMaster.supsearchRequest.customerId = this.requestModel.customerId;
    this.dashBoardFilterMaster.supsearchRequest.supplierType = SupplierType.Supplier;
    this.dashBoardFilterMaster.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.supLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.dashBoardFilterMaster.supsearchRequest, term)
        : this.supService.getFactoryDataSourceList(this.dashBoardFilterMaster.supsearchRequest)
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
  getSupplierData() {
    this.dashBoardFilterMaster.supsearchRequest.searchText = this.dashBoardFilterMaster.supInput.getValue();
    this.dashBoardFilterMaster.supsearchRequest.skip = this.dashBoardFilterMaster.supplierList.length;

    this.dashBoardFilterMaster.supsearchRequest.customerId = this.requestModel.customerId;
    this.dashBoardFilterMaster.supsearchRequest.supplierType = SupplierType.Supplier;
    this.dashBoardFilterMaster.supLoading = true;
    this.supService.getFactoryDataSourceList(this.dashBoardFilterMaster.supsearchRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.supplierList = this.dashBoardFilterMaster.supplierList.concat(customerData);
        }
        this.dashBoardFilterMaster.supsearchRequest.skip = 0;
        this.dashBoardFilterMaster.supsearchRequest.take = ListSize;
        this.dashBoardFilterMaster.supLoading = false;
      }),
      (error: any) => {
        this.dashBoardFilterMaster.supLoading = false;
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
        this.dashBoardFilterMaster.countryNameList = [];
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
      this.dashBoardFilterMaster.supplierList = [];
      this.getSupListBySearch();
    }
  }

  //fetch the first 10 suppliers for the customer on load
  getFactListBySearch() {
    
    this.dashBoardFilterMaster.requestFactModel = new CommonDataSourceRequest();
    this.dashBoardFilterMaster.factoryList = [];
    this.dashBoardFilterMaster.requestFactModel.customerId = this.requestModel.customerId;
    this.dashBoardFilterMaster.requestFactModel.supplierType = SupplierType.Factory;
    this.dashBoardFilterMaster.factInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.factLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.dashBoardFilterMaster.requestFactModel, term)
        : this.supService.getFactoryDataSourceList(this.dashBoardFilterMaster.requestFactModel)
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
      this.dashBoardFilterMaster.requestFactModel.searchText = this.dashBoardFilterMaster.factInput.getValue();
      this.dashBoardFilterMaster.requestFactModel.skip = this.dashBoardFilterMaster.factoryList.length;
    }
    this.dashBoardFilterMaster.requestFactModel.customerId = this.requestModel.customerId;
    this.dashBoardFilterMaster.requestFactModel.supplierType = SupplierType.Factory;
    this.dashBoardFilterMaster.factLoading = true;
    this.supService.getFactoryDataSourceList(this.dashBoardFilterMaster.requestFactModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.factoryList = this.dashBoardFilterMaster.factoryList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.dashBoardFilterMaster.requestFactModel.skip = 0;
          this.dashBoardFilterMaster.requestFactModel.take = ListSize;
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
    if (item && item.length > 0) {

      if (this.requestModel.factoryIdList && this.requestModel.factoryIdList.length > 0) {

        var factoryLength = this.requestModel.factoryIdList.length;

        var factoryDetails = [];
        for (var i = 0; i < factoryLength; i++) {

          factoryDetails.push(this.dashBoardFilterMaster.factoryList.find(x => x.id == this.requestModel.factoryIdList[i]).name);
        }
        this.dashBoardFilterMaster.factoryNameList = factoryDetails;
      }
      else {
        this.dashBoardFilterMaster.factoryNameList = [];
      }
    }
    else{
      this.dashBoardFilterMaster.factoryList = [];
      this.getFactListBySearch();
    }
  }

  getCustomerListBySearch() {

    this.dashBoardFilterMaster.requestCustomerModel = new CommonDataSourceRequest();
    this.dashBoardFilterMaster.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.dashBoardFilterMaster.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.dashBoardFilterMaster.requestCustomerModel)
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
      this.dashBoardFilterMaster.requestCustomerModel.searchText = this.dashBoardFilterMaster.customerInput.getValue();
      this.dashBoardFilterMaster.requestCustomerModel.skip = this.dashBoardFilterMaster.customerList.length;
    }

    this.dashBoardFilterMaster.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.dashBoardFilterMaster.requestCustomerModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.customerList = this.dashBoardFilterMaster.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.dashBoardFilterMaster.requestCustomerModel.skip = 0;
          this.dashBoardFilterMaster.requestCustomerModel.take = ListSize;
        }
        this.dashBoardFilterMaster.customerLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.customerLoading = false;
        this.setError(error);
      };
  }

  getRatioCustomerListBySearch() {
    if(this.requestModel.customerId){
      this.dashBoardFilterMaster.ratioCustomerList = this.dashBoardFilterMaster.customerList.filter(x => x.id == this.requestModel.customerId);
    }
    else {
    if (this.requestModel.ratioCustomerIdList && this.requestModel.ratioCustomerIdList.length > 0) {
      this.dashBoardFilterMaster.requestInnerRatioCustomerModel.idList = this.requestModel.ratioCustomerIdList;
    }
    else {
      this.dashBoardFilterMaster.requestInnerRatioCustomerModel.idList =null;
    }
    this.dashBoardFilterMaster.requestInnerRatioCustomerModel = new CommonDataSourceRequest();
    this.dashBoardFilterMaster.ratioCustomerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.ratioCustomerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.dashBoardFilterMaster.requestInnerRatioCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.dashBoardFilterMaster.requestInnerRatioCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.ratioCustomerLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.ratioCustomerList = data;
        this.dashBoardFilterMaster.ratioCustomerLoading = false;
      });
    }
  }

  //fetch the customer data for ration analysis with virtual scroll
  getRatioCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.dashBoardFilterMaster.requestInnerRatioCustomerModel.searchText = this.dashBoardFilterMaster.ratioCustomerInput.getValue();
      this.dashBoardFilterMaster.requestInnerRatioCustomerModel.skip = this.dashBoardFilterMaster.ratioCustomerList.length;
    }

    this.dashBoardFilterMaster.ratioCustomerLoading = true;
    this.customerService.getCustomerDataSourceList(this.dashBoardFilterMaster.requestInnerRatioCustomerModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dashBoardFilterMaster.ratioCustomerList = this.dashBoardFilterMaster.ratioCustomerList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.dashBoardFilterMaster.requestInnerRatioCustomerModel.skip = 0;
          this.dashBoardFilterMaster.requestInnerRatioCustomerModel.take = ListSize;
        }
        this.dashBoardFilterMaster.ratioCustomerLoading = false;
      }),
      error => {
        this.dashBoardFilterMaster.ratioCustomerLoading = false;
        this.setError(error);
      };
  }
  changeRatioCustomer(selectedValues){
    this.getRatioAnalysis();
  }
  clearRatioCustomer(){
    this.requestModel.ratioCustomerIdList=null;
    this.getRatioAnalysis();
  }

  changeCustomerData(item) {
    
    this.requestModel.supplierId = null;
    this.requestModel.factoryIdList = [];
    this.requestModel.brandIdList = [];
    this.requestModel.buyerIdList = [];
    this.requestModel.deptIdList = [];
    if(this.requestModel.ratioCustomerIdList && this.requestModel.ratioCustomerIdList.length > 0) {
      this.requestModel.ratioCustomerIdList = [];
      this.clickRationAnalysis();
    }
    this.getRatioCustomerListBySearch();
    

    this.getSupListBySearch();
    this.getFactListBySearch()
    this.getServiceTypeList();
    if (item && item.id > 0) {

      var customerDetails = this.dashBoardFilterMaster.customerList.find(x => x.id == item.id);
      if (customerDetails)
        this.dashBoardFilterMaster.customerName =
          customerDetails.name.length > SupplierNameTrim ?
            customerDetails.name.substring(0, SupplierNameTrim) + "..." : customerDetails.name;

      this.dashBoardFilterMaster.brandSearchRequest.customerId = item.id;
      this.dashBoardFilterMaster.deptSearchRequest.customerId = item.id;
      this.dashBoardFilterMaster.buyerSearchRequest.customerId = item.id;
      this.getBrandListBySearch();
      this.getBuyerListBySearch();
      this.getDeptListBySearch();
      this.dashBoardFilterMaster.isRatioCustomerVisible = false;
    }
    else {
      this.dashBoardFilterMaster.customerName = "";
      this.dashBoardFilterMaster.brandList = [];
      this.dashBoardFilterMaster.buyerList = [];
      this.dashBoardFilterMaster.deptList = [];
      this.dashBoardFilterMaster.requestCustomerModel.searchText= "";
      this.getCustomerListBySearch();
      this.dashBoardFilterMaster.isRatioCustomerVisible = true;
    }
  }

  ngAfterViewInit() {
    //this.renderLineChart("billedManDayChart");
    //this.renderLineChart("mandayRateChart", 1);
    // this.renderDonutChart("pieChart1");
    // this.renderDonutChart("pieChart2");
    // this.renderDonutChart("pieChart3");
  }

  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
  }

  toggleFilter() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  toggleAdvanceSearch() { }

  //toggle advance search section
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  reset() {
    this.requestModel = new FinanceDashboardRequestModel();
    this.dateSelectionDefault();

    this.dashBoardFilterMaster.customerName = "";
    this.dashBoardFilterMaster.countryNameList = [];
    this.dashBoardFilterMaster.supplierName = "";
    this.dashBoardFilterMaster.factoryNameList = [];
    this.dashBoardFilterMaster.officeNameList = [];
    this.dashBoardFilterMaster.serviceTypeNameList = [];
    this.dashBoardFilterMaster.brandNameList = [];
    this.dashBoardFilterMaster.buyerNameList = [];
    this.dashBoardFilterMaster.deptNameList = [];

    this.dashBoardFilterMaster.deptList=[];
    this.dashBoardFilterMaster.buyerList=[];
    this.dashBoardFilterMaster.brandList=[];
    this.filterListLoad();
   }

  search() {
    if(this.isMobile){
      this.isFilterOpen = !this.isFilterOpen;
    }
    this.validator.isSubmitted = true;
    
   
    if(this.isFormValid()) {
    this.requestModel.ratioCustomerIdList=[];
    if(this.requestModel.customerId)
    {
      this.requestModel.ratioCustomerIdList.push(this.requestModel.customerId);
    }
    this.dashBoardFilterMaster.searchLoading = true;
    this.isFilterOpen = false;
		this.filterDataShown = this.filterTextShown();
    this.model = new FinanceDashboardModel();
    this.summaryModel = new FinanceDashboardLoader();
    this.tableData = new TableModel();
    this.chartLoad();
    this.colorSet = new am4core.ColorSet();
    this.requestModel.isBilledMandayExport = !this.summaryModel.tableTabActive;
    }
   }

  //fetch the brand data with virtual scroll
  getBrandData() {
    this.dashBoardFilterMaster.brandSearchRequest.searchText = this.dashBoardFilterMaster.brandInput.getValue();
    this.dashBoardFilterMaster.brandSearchRequest.skip = this.dashBoardFilterMaster.brandList.length;

    this.dashBoardFilterMaster.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.dashBoardFilterMaster.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.dashBoardFilterMaster.brandList = this.dashBoardFilterMaster.brandList.concat(brandData);
        }
        this.dashBoardFilterMaster.brandSearchRequest = new CommonCustomerSourceRequest();
        this.dashBoardFilterMaster.brandLoading = false;
      }),
      (error: any) => {
        this.dashBoardFilterMaster.brandLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.dashBoardFilterMaster.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.dashBoardFilterMaster.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.dashBoardFilterMaster.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.brandLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.brandList = data;
        this.dashBoardFilterMaster.brandLoading = false;
      });
  }

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.dashBoardFilterMaster.buyerSearchRequest.searchText = this.dashBoardFilterMaster.buyerInput.getValue();
    this.dashBoardFilterMaster.buyerSearchRequest.skip = this.dashBoardFilterMaster.buyerList.length;

    this.dashBoardFilterMaster.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.dashBoardFilterMaster.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.dashBoardFilterMaster.buyerList = this.dashBoardFilterMaster.buyerList.concat(buyerData);
        }
        this.dashBoardFilterMaster.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.dashBoardFilterMaster.buyerLoading = false;
      }),
      (error: any) => {
        this.dashBoardFilterMaster.buyerLoading = false;
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.dashBoardFilterMaster.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.dashBoardFilterMaster.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.dashBoardFilterMaster.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.buyerLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.buyerList = data;
        this.dashBoardFilterMaster.buyerLoading = false;
      });
  }
  
  //fetch the brand data with virtual scroll
  getDeptData() {
    this.dashBoardFilterMaster.deptSearchRequest.searchText = this.dashBoardFilterMaster.deptInput.getValue();
    this.dashBoardFilterMaster.deptSearchRequest.skip = this.dashBoardFilterMaster.deptList.length;

    this.dashBoardFilterMaster.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.dashBoardFilterMaster.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.dashBoardFilterMaster.deptList = this.dashBoardFilterMaster.deptList.concat(deptData);
        }
        this.dashBoardFilterMaster.deptSearchRequest = new CommonCustomerSourceRequest();
        this.dashBoardFilterMaster.deptLoading = false;
      }),
      (error: any) => {
        this.dashBoardFilterMaster.deptLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {
    this.dashBoardFilterMaster.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dashBoardFilterMaster.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.dashBoardFilterMaster.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.dashBoardFilterMaster.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.dashBoardFilterMaster.deptLoading = false))
      ))
      .subscribe(data => {
        this.dashBoardFilterMaster.deptList = data;
        this.dashBoardFilterMaster.deptLoading = false;
      });
  }

    //get office list
    getOfficeList() {
      this.dashBoardFilterMaster.officeLoading = true;
      this.refService.getOfficeList()
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

    //get service type list
  getServiceTypeList() {
    this.requestModel.serviceTypeList = [];
        this.dashBoardFilterMaster.serviceTypeLoading = true;
        let request = this.generateServiceTypeRequest();
  
        this.refService.getServiceTypes(request)
          .pipe(takeUntil(this.componentDestroyed$), first())
          .subscribe(
            response => {
  
              this.dashBoardFilterMaster.serviceTypeList = response.serviceTypeList;
              this.dashBoardFilterMaster.serviceTypeLoading = false;
            },
            error => {
              this.setError(error);
              this.dashBoardFilterMaster.serviceTypeList = [];
              this.dashBoardFilterMaster.serviceTypeLoading = false;
            }
          );
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.requestModel.customerId ?? 0;
    serviceTypeRequest.serviceId = Service.Inspection;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  getColor(index){
    if(this.tableData.actual[index] < this.tableData.budget[index] && this.tableData.actual[index] > 0)

    return true;
  }

  //filter details has to show
	filterTextShown() {
		var isFilterDataSelected = true;

		if (this.requestModel.customerId > 0 || this.requestModel.supplierId > 0
			|| (this.requestModel.factoryIdList && this.requestModel.factoryIdList.length > 0) ||
      (this.requestModel.countryIdList && this.requestModel.countryIdList.length > 0) ||
			(this.requestModel.deptIdList && this.requestModel.deptIdList.length > 0) ||
			(this.requestModel.brandIdList && this.requestModel.brandIdList.length > 0) ||
			(this.requestModel.officeIdList && this.requestModel.officeIdList.length > 0) ||
      (this.requestModel.serviceTypeList && this.requestModel.serviceTypeList.length > 0) ||
			(this.requestModel.buyerIdList && this.requestModel.buyerIdList.length > 0)) {

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
				var count = 1;

        if (this.requestModel.customerId > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.supplierId > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.countryIdList && this.requestModel.countryIdList.length > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.brandIdList && this.requestModel.brandIdList.length > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.buyerIdList && this.requestModel.buyerIdList.length > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.officeIdList && this.requestModel.officeIdList.length > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.deptIdList && this.requestModel.deptIdList.length > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.serviceTypeList && this.requestModel.serviceTypeList.length > 0) {
					count = MobileViewFilterCount + count;
				}
				if (this.requestModel.factoryIdList && this.requestModel.factoryIdList.length > 0) {
					count = MobileViewFilterCount + count;
				}
				this.filterCount = count;

				isFilterDataSelected = true;
			}
			else {
				this.filterCount = 1;
				this.dashBoardFilterMaster.countryNameList = [];
				this.dashBoardFilterMaster.supplierName = "";
				this.dashBoardFilterMaster.customerName = "";
				this.dashBoardFilterMaster.factoryNameList = [];
				this.dashBoardFilterMaster.officeNameList = [];
				this.dashBoardFilterMaster.serviceTypeNameList = [];
				this.dashBoardFilterMaster.brandNameList = [];
				this.dashBoardFilterMaster.buyerNameList = [];
				this.dashBoardFilterMaster.deptNameList = [];
			}
		}

		return isFilterDataSelected;
	}

  officeChange(item){
    if (item) {

      if (this.requestModel.officeIdList && this.requestModel.officeIdList.length > 0) {

        var officeLength = this.requestModel.officeIdList.length;

        var officeDetails = [];
        var maxLength = officeLength > 2 ? 2 : officeLength;
        for (var i = 0; i < maxLength; i++) {

          officeDetails.push(this.dashBoardFilterMaster.officeList.find(x => x.id == this.requestModel.officeIdList[i]).name);
        }

        if(officeLength > 2){
          officeDetails.push(" " + (officeLength-2) + "more");
        }
        this.dashBoardFilterMaster.officeNameList = officeDetails;
      }
      else {
        this.dashBoardFilterMaster.officeNameList = [];
      }
    }
  }

  serviceTypeChange(item){
    if (item) {

      if (this.requestModel.serviceTypeList && this.requestModel.serviceTypeList.length > 0) {

        var serviceTypeLength = this.requestModel.serviceTypeList.length;

        var serviceTypeDetails = [];
        var maxLength = serviceTypeLength > 2 ? 2 : serviceTypeLength;
        for (var i = 0; i < maxLength; i++) {

          serviceTypeDetails.push(this.dashBoardFilterMaster.serviceTypeList.find(x => x.id == this.requestModel.serviceTypeList[i]).name);
        }

        if(serviceTypeLength > 2){
          serviceTypeDetails.push(" " + (serviceTypeLength-2) + "more");
        }
        this.dashBoardFilterMaster.serviceTypeNameList = serviceTypeDetails;
      }
      else {
        this.dashBoardFilterMaster.serviceTypeNameList = [];
      }
    }
  }
  brandChange(item){
    if (item) {

      if (this.requestModel.brandIdList && this.requestModel.brandIdList.length > 0) {

        var brandLength = this.requestModel.brandIdList.length;

        var brandDetails = [];
        for (var i = 0; i < brandLength; i++) {

          brandDetails.push(this.dashBoardFilterMaster.brandList.find(x => x.id == this.requestModel.brandIdList[i]).name);
        }
        this.dashBoardFilterMaster.brandNameList = brandDetails;
      }
      else {
        this.dashBoardFilterMaster.brandNameList = [];
      }
    }
  }
  buyerChange(item){
    if (item) {

      if (this.requestModel.buyerIdList && this.requestModel.buyerIdList.length > 0) {

        var buyerLength = this.requestModel.buyerIdList.length;

        var buyerDetails = [];
        for (var i = 0; i < buyerLength; i++) {

          buyerDetails.push(this.dashBoardFilterMaster.buyerList.find(x => x.id == this.requestModel.buyerIdList[i]).name);
        }
        this.dashBoardFilterMaster.buyerNameList = buyerDetails;
      }
      else {
        this.dashBoardFilterMaster.buyerNameList = [];
      }
    }
  }
  deptChange(item){
    if (item) {

      if (this.requestModel.deptIdList && this.requestModel.deptIdList.length > 0) {

        var deptLength = this.requestModel.deptIdList.length;

        var deptDetails = [];
        for (var i = 0; i < deptLength; i++) {

          deptDetails.push(this.dashBoardFilterMaster.deptList.find(x => x.id == this.requestModel.deptIdList[i]).name);
        }
        this.dashBoardFilterMaster.deptNameList = deptDetails;
      }
      else {
        this.dashBoardFilterMaster.deptNameList = [];
      }
    }
  }

  ribbonReset(){
    this.reset();
    this.search();
  }
  
  //form validation
  isFormValid(): boolean {
    return this.validator.isValid('serviceDateFrom') &&
			this.validator.isValid('serviceDateTo');
  }

  clearFactory(){
    this.dashBoardFilterMaster.factoryList = [];
      this.getFactListBySearch();
  }

  clearSupplier(){
    this.dashBoardFilterMaster.supplierList = [];
      this.getSupListBySearch();
  }

  getEmployeeTypes(){
    this.dashBoardFilterMaster.employeeTypeLoading = true;

    this.service.getEmployeeTypes()
    .pipe(first())
    .subscribe(res => {
      if(res.result == ResponseResult.Success){
        this.dashBoardFilterMaster.employeeTypeList = res.dataSourceList;
      }
      else{
        this.dashBoardFilterMaster.employeeTypeList = [];
      }
      this.dashBoardFilterMaster.employeeTypeLoading = false;
    },
    error =>{
      this.dashBoardFilterMaster.employeeTypeLoading = false;
      this.dashBoardFilterMaster.employeeTypeList = [];
    });
  }

  changeRatioEmployeeType(selectedValues){
    this.getRatioAnalysis();
  }
  clearRatioEmployeeType(){
    this.requestModel.ratioEmployeeTypeIdList=null;
    this.getRatioAnalysis();
  }
}

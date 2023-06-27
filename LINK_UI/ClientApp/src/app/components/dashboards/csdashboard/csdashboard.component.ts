import { Component } from '@angular/core';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import { trigger, state, style, transition, animate } from '@angular/animations';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { Validator } from '../../common/validator';
import { CSDashBoardService } from 'src/app/_Services/dashboard/csdashboard.service';
import { of, Subject } from 'rxjs';
import { CSDashboardMasterModel, CSDashboardRequest, CSDashboardResult, TaskCardState, TaskCountItem, TaskStatusList } from 'src/app/_Models/dashboard/csdashboard.model';
import { NgbCalendar, NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { CSDashboardBookingConfirmed, CSDashboardBookingVerified, CSDashboardCreated, CSDashboardCustomerConfirmed, CSDashboardOfficeTrimValue, CSDashboardQuotationApproved, CSDashboardQuotationModify, CSDashboardQuotationSent, CSDashboardServiceTypeTrimValue, CSDashboardStatusTrimValue, ListLengthOne, ListLengthThree, ListLengthTwo, ListSize, MobileViewFilterCount, QuotationStatusList, SupplierNameTrim, SupplierType, ZeroValue } from '../../common/static-data-common';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';

@Component({
  selector: 'app-csdashboard',
  templateUrl: './csdashboard.component.html',
  styleUrls: ['./csdashboard.component.scss'],
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
export class CsdashboardComponent extends DetailComponent {

  componentDestroyed$: Subject<boolean> = new Subject();
  cars: Array<any>;
  isMobile: boolean;
  isFilterOpen: boolean;
  sampleList: Array<any>;
  loadingLineChart: boolean;
  request: CSDashboardRequest;
  masterModel: CSDashboardMasterModel;
  toggleFormSection: boolean;
  filterDataShown: boolean;
  filterCount: number;
  colorSet: any;
  private currentRoute: Router;
  currentUser: UserModel;
    
  constructor(public validator: Validator, private csDashboardService: CSDashBoardService,
    public dateparser: NgbDateParserFormatter, private calendar: NgbCalendar, private customerService: CustomerService,
    private supService: SupplierService, router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    private locationService: LocationService, authserve: AuthenticationService,
    private refService: ReferenceService,public accService: UserAccountService,
    private brandService: CustomerBrandService, private buyerService: CustomerbuyerService,
    private deptService: CustomerDepartmentService,) {

    super(router, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this.currentRoute = router;
    am4core.addLicense("CH238479116");

    this.validator.setJSON("dashboard/csdashboard.valid.json");
    this.validator.setModelAsync(() => this.request);
    this.validator.isSubmitted = false;
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.request = new CSDashboardRequest();
    this.masterModel = new CSDashboardMasterModel();
    this.isFilterOpen = false;

    this.filterDataShown = true;
    this.colorSet = new am4core.ColorSet();

    this.dateSelectionDefault();
    this.filterListLoad();
    this.chartLoad();

  }

  getViewPath(): string {
    return '';
  }

  getEditPath(): string {
    return '';
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
    // this.request.customerId = 26;
    this.request.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 5);
    this.request.serviceDateTo = this.calendar.getNext(this.calendar.getToday(), 'd', 5);
  }

  renderReportData() {
    var chart = am4core.create("chartdiv1", am4charts.XYChart);

    // Add data
    // chart.data = this.generateChartData();
    chart.data = this.masterModel.reportCountDayList;
    // Create axes
    // chart.dateFormatter.dateFormat = "dd/MM/yyyy";

    let dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    // dateAxis.renderer.minGridDistance = 20;
    // dateAxis.renderer.grid.template.location = 0;
    dateAxis.renderer.labels.template.fontSize = 12;
    // dateAxis.renderer.ticks.template.disabled = true;
    // dateAxis.renderer.axisFills.template.disabled = true;
    // dateAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    // dateAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    // dateAxis.dateFormats.setKey("day", "dd");
    // dateAxis.periodChangeDateFormats.setKey("day", "dd"); 

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    // valueAxis.tooltip.disabled = true;
    // valueAxis.renderer.minGridDistance = 20;
    valueAxis.renderer.labels.template.fontSize = 12;
    // valueAxis.renderer.ticks.template.disabled = true;
    // valueAxis.renderer.axisFills.template.disabled = true;
    // valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    // valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    // var dateAxis = chart.xAxes.push(new am4charts.DateAxis());

    // var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.LineSeries());
    series.dataFields.valueY = "count";
    series.dataFields.dateX = "date";

    //Stroke (outline) thickness in pixels.
    series.strokeWidth = 1;

    //Minimal distance between data points in pixels.
    //If distance gets smaller than this, bullets are turned off to avoid overlapping.
    series.minBulletDistance = 10;

    //series.columns.template.showTooltipOn = "always";

    // series.tooltipText = "{valueY}";
    series.tooltipText = "{dateX}: [b]{valueY}[/]";

    // series.fillOpacity = 0.1;
    series.tooltip.pointerOrientation = "vertical";
    series.tooltip.background.cornerRadius = 20;
    series.tooltip.background.fillOpacity = 0.5;
    series.tooltip.label.padding(12, 12, 12, 12)

    //This method is available for each axis type. It takes a reference to particular series 
    //as a parameter, so the axis knows which series to apply range overrides to.
    var seriesRange = dateAxis.createSeriesRange(series);

    //value allows setting lengths of dashes, spacing between them, and even create intricate complex line styles.
    seriesRange.contents.strokeDasharray = "2,3";
    seriesRange.contents.stroke = chart.colors.getIndex(8);
    seriesRange.contents.strokeWidth = 1;

    var pattern = new am4core.LinePattern();

    //Pattern rotation in degrees.
    pattern.rotation = -45;

    //Pattern stroke (border) color.
    pattern.stroke = seriesRange.contents.stroke;

    //Pattern width in pixels.
    pattern.width = 1000;

    //Pattern height in pixels.
    pattern.height = 1000;

    //The pattern will automatically draw required number of lines to fill pattern area 
    //maintaining gap distance between them.
    pattern.gap = 1;
    seriesRange.contents.fill = pattern;
    seriesRange.contents.fillOpacity = 0.5;

    // Add scrollbar
    chart.scrollbarX = new am4core.Scrollbar();

    // Add cursor
    chart.cursor = new am4charts.XYCursor();
    chart.cursor.xAxis = dateAxis;
    chart.cursor.snapToSeries = series;
  }

  generateChartData() {
    var chartData = [];
    var firstDate = new Date();
    firstDate.setDate(firstDate.getDate() - 200);
    var visits = 1200;
    for (var i = 0; i < 200; i++) {
      // we create date objects here. In your data, you can have date strings
      // and then set format of your dates using chart.dataDateFormat property,
      // however when possible, use date objects, as this will speed up chart rendering.
      var newDate = new Date(firstDate);
      newDate.setDate(newDate.getDate() + i);

      visits += Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 10);

      chartData.push({
        date: newDate,
        visits: visits
      });
    }
    return chartData;
  }

  //render line chart - report count with date wise
  renderLineChart() {
    let chart = am4core.create("chartdiv1", am4charts.XYChart);
    chart.paddingLeft = 0;

    // let data = [];
    // data.push({ date: new Date(2021, 0, 1), value: 400 });
    // data.push({ date: new Date(2021, 0, 2), value: 400 });
    // data.push({ date: new Date(2021, 0, 3), value: 400 });
    // data.push({ date: new Date(2021, 0, 4), value: 400 });
    // data.push({ date: new Date(2021, 0, 5), value: 400 });
    // data.push({ date: new Date(2021, 0, 6), value: 100 });
    // data.push({ date: new Date(2021, 0, 7), value: 400 });
    // data.push({ date: new Date(2021, 0, 8), value: 400 });

    chart.data = this.masterModel.reportCountDayList;

    chart.dateFormatter.dateFormat = "dd/MMM/yyyy";

    let dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    dateAxis.renderer.minGridDistance = 20;
    dateAxis.renderer.grid.template.location = 0;
    dateAxis.renderer.labels.template.fontSize = 12;
    dateAxis.renderer.ticks.template.disabled = true;
    dateAxis.renderer.axisFills.template.disabled = true;
    dateAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    dateAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    dateAxis.dateFormats.setKey("day", "dd");
    dateAxis.periodChangeDateFormats.setKey("day", "dd");

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.tooltip.disabled = true;
    valueAxis.renderer.minGridDistance = 20;
    valueAxis.renderer.labels.template.fontSize = 12;
    valueAxis.renderer.ticks.template.disabled = true;
    valueAxis.renderer.axisFills.template.disabled = true;
    valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
    valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

    let series = chart.series.push(new am4charts.LineSeries());
    series.dataFields.dateX = "date";
    series.dataFields.valueY = "count";
    series.strokeWidth = 2;
    series.tooltipText = "{dateX}: [b]{valueY}[/]";

    // chart.scrollbarX = new am4core.Scrollbar();

    // set stroke property field
    series.propertyFields.stroke = "color";

    chart.cursor = new am4charts.XYCursor();
  }

  //render service type chart
  renderfullServiceTypeDonutChart(container) {
    // Create chart instance
    let chart = am4core.create(container, am4charts.PieChart);

    chart.data = this.masterModel.serviceTypeGraphList;

    // Add and configure Series
    let pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "count";
    pieSeries.dataFields.category = "name"; 
    pieSeries.slices.template.propertyFields.fill = "color";
    pieSeries.slices.template.stroke = am4core.color("#fff");
    pieSeries.slices.template.strokeOpacity = 1;
    pieSeries.labels.template.disabled = true;

    // This creates initial animation
    pieSeries.hiddenState.properties.opacity = 1;
    pieSeries.hiddenState.properties.endAngle = -90;
    pieSeries.hiddenState.properties.startAngle = -90;

    chart.hiddenState.properties.radius = am4core.percent(0);
  }


  //render manday by office chart
  renderfullMandayByOfficeDonutChart(container) {
    // Create chart instance
    let chart = am4core.create(container, am4charts.PieChart);

    chart.innerRadius = 45;
    // Add data
    chart.data = this.masterModel.mandayByOfficeList;

    // Add and configure Series
    let pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "count";
    pieSeries.dataFields.category = "name";    
    pieSeries.slices.template.propertyFields.fill = "color";
    pieSeries.slices.template.stroke = am4core.color("#fff");
    pieSeries.slices.template.strokeOpacity = 1;
    pieSeries.labels.template.disabled = true;

    // This creates initial animation
    pieSeries.hiddenState.properties.opacity = 1;
    pieSeries.hiddenState.properties.endAngle = -90;
    pieSeries.hiddenState.properties.startAngle = -90;

    chart.hiddenState.properties.radius = am4core.percent(0);
  }

  //get new count details of - booking, customer, supplier, factory, PO, Product
  getCountNewBookingRelatedDetails() {
    // this.model.mandayDashboard = new MandayDashboard();      
    this.csDashboardService.getCountNewBookingRelatedDetails(this.request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response) {
            if (response.result == CSDashboardResult.Success) {
              this.masterModel.newCountDashboardItem = response.csDashboardCountItem;
              this.masterModel.newCountDashboardDataFound = true;
            }
            else {
              this.masterModel.newCountDashboardDataFound = false;
            }
          }
          this.masterModel.newCountDashboardLoading = false;
        },
        error => {
          this.masterModel.newCountDashboardLoading = false;
          this.masterModel.newCountDashboardError = true;
        });
  }

  //get service type with count list
  getInspectionServiceTypeList() {
    // if (bookingIdList && bookingIdList.length > 0) {
    // }
    // else {
    // 	this.dataFound.serviceTypeInspectionDashboardDataFound = false;
    // 	this.quantitativeDashboardLoader.serviceTypeInspectionDashboardLoading = false;
    // }
    this.csDashboardService.getInspectionServiceTypeList(this.request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == CSDashboardResult.Success) {

            this.masterModel.serviceTypeGraphList = response.data;
            this.masterModel.serviceTypeGraphFound = true;
            this.masterModel.serviceTypeGraphLoading = false;

            for (var i = 0; i < this.masterModel.serviceTypeGraphList.length; i++) {
              var _sublength = CSDashboardServiceTypeTrimValue;
              this.masterModel.serviceTypeGraphList[i].trimmedName =

                this.masterModel.serviceTypeGraphList[i].name &&
                  this.masterModel.serviceTypeGraphList[i].name.length > _sublength ?
                  this.masterModel.serviceTypeGraphList[i].name.substring(0, _sublength) + ".." :
                  this.masterModel.serviceTypeGraphList[i].name;

              this.masterModel.serviceTypeGraphList[i].color = this.colorSet.next();

            }

            setTimeout(() => {
              this.renderfullServiceTypeDonutChart('chartdiv-circle1');
            }, 30);
          }
          else {
            this.masterModel.serviceTypeGraphFound = false;
            this.masterModel.serviceTypeGraphLoading = false;
          }
        },
        error => {
          this.masterModel.serviceTypeGraphLoading = false;
          this.masterModel.serviceTypeGraphError = true;
        });
  }


  // get manday count by office list
  getInspectionMandayByOfficeList() {
    this.csDashboardService.getInspectionMandayByOfficeList(this.request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == CSDashboardResult.Success) {

            this.masterModel.mandayByOfficeList = response.data;
            this.masterModel.mandayByOfficeFound = true;
            this.masterModel.mandayByOfficeLoading = false;

            for (var i = 0; i < this.masterModel.mandayByOfficeList.length; i++) {
              var _sublength = CSDashboardOfficeTrimValue;
              this.masterModel.mandayByOfficeList[i].trimmedName =

                this.masterModel.mandayByOfficeList[i].name &&
                  this.masterModel.mandayByOfficeList[i].name.length > _sublength ?
                  this.masterModel.mandayByOfficeList[i].name.substring(0, _sublength) + ".." :
                  this.masterModel.mandayByOfficeList[i].name;

              this.masterModel.mandayByOfficeList[i].color = this.colorSet.next();
            }
            setTimeout(() => {
              this.renderfullMandayByOfficeDonutChart('chartdiv-circle2');
            }, 10);
          }
          else {
            this.masterModel.mandayByOfficeFound = false;
            this.masterModel.mandayByOfficeLoading = false;
          }
        },
        error => {
          this.masterModel.mandayByOfficeLoading = false;
          this.masterModel.mandayByOfficeError = true;
        });
  }

  //get report count by dates
  getTotalReportsCountByDayList() {
    this.csDashboardService.getTotalReportsCountByDayList(this.request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == CSDashboardResult.Success) {

            response.data.forEach(element => {
              var date = this.dateparser.parse(element.date);
              var dateFormat = new Date(date.year, date.month-1, date.day);
              element.date = dateFormat;
            });

            this.masterModel.reportCountDayList = response.data;

            this.masterModel.reportCountFound = true;
            this.masterModel.reportCountLoading = false;

            setTimeout(() => {
              // this.renderReportData();
              this.renderLineChart();
            }, 10);
          }
          else {
            this.masterModel.reportCountFound = false;
            this.masterModel.reportCountLoading = false;
          }
          this.masterModel.searchLoading = false;
        },
        error => {
          this.masterModel.reportCountLoading = false;
          this.masterModel.reportCountError = true;
          this.masterModel.searchLoading = false;
        });
  }

  //get booking, quotation, allocation, report status
  getStatusByLoggedUserList() {
    this.csDashboardService.getStatusByLoggedUserList(this.request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {

          if (response.result == CSDashboardResult.Success) {

            this.processStatusSuccess(response);

            this.masterModel.statusCountLoading = false;
            this.masterModel.statusCountFound = true;
          }
          else {
            this.masterModel.statusCountFound = false;
            this.masterModel.statusCountLoading = false;
          }
        },
        error => {
          this.masterModel.statusCountLoading = false;
          this.masterModel.statusCountError = true;
        });
  }

  //redirect to respective summary
  redirectSummary(link, moduleId) {
    
    //4 - report module
    if(moduleId == 4) {
      window.open(link, "_blank");
    }
    else {
      this.currentRoute.navigate([`/${this.utility.getEntityName()}/${link}`]);
    }
  }

  //assign status name with trim
  assignStatusName(statusList) {

    for (var i = 0; i < statusList.length; i++) {
      var _sublength = CSDashboardStatusTrimValue;

      statusList[i].trimName = statusList[i].statusName;

      // statusList[i].trimName =
      //   statusList[i].statusName &&
      //     statusList[i].statusName.length > _sublength ?
      //     statusList[i].statusName.substring(0, _sublength) + ".." :
      //     statusList[i].statusName;
    }
    return statusList;

  }
  assignQuotationStatusName(statusList) {
    for (var i = 0; i < statusList.length; i++) {

      statusList[i].trimName = QuotationStatusList.find(x => x.id == statusList[i].id).name;
      //  statusList[i].statusName;
      // statusList[i].trimName = statusList[i].statusName &&
      //     statusList[i].statusName.length > _sublength ?
      //     statusList[i].statusName.substring(0, _sublength) + ".." :
      //     statusList[i].statusName;
    }

    return statusList;
  }

  //redirect to fb reports page
  redirectToFBReports(): string {
    var URL: string;
    this.accService.getUserTokenToFB()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response != null) {
            this.masterModel.reportTaskList.forEach(element => {
              element.taskLink = response.reportsUrl + "?token=" + response.token + "";
            });

            // URL = response.reportsUrl + "?token=" + response.token + "";
          }
        },
        error => {
        });
    return URL;
  }

  processStatusSuccess(response) {
    this.masterModel.bookingStatusList = this.assignStatusName(response.bookingStatusCount);    

    this.masterModel.quotationStatusList = this.assignQuotationStatusName(response.quotationStatusCount);  

    //allocation starts
    this.masterModel.allocationStatusList = response.allocationStatusCount;
   
    this.masterModel.reportStatusList = this.assignStatusName(response.reportStatusCount);
   

    var objBooking = this.AssignTaskStatusList(this.masterModel.bookingStatusList,
      this.masterModel.bookingTaskList, this.masterModel.bookingCardState);

    this.masterModel.bookingTaskList = objBooking.taskList;
    this.masterModel.bookingCardState = objBooking.cardState;
    this.masterModel.bookingStatusList = objBooking.statusList;

    var objQuotation = this.AssignTaskStatusList(this.masterModel.quotationStatusList,
      this.masterModel.quotationTaskList, this.masterModel.quotationCardState);

    this.masterModel.quotationTaskList = objQuotation.taskList;
    this.masterModel.quotationCardState = objQuotation.cardState;
    this.masterModel.quotationStatusList = objQuotation.statusList;


    var objAllocation = this.AssignTaskStatusList(this.masterModel.allocationStatusList,
      this.masterModel.allocationTaskList, this.masterModel.allocationCardState);

    this.masterModel.allocationTaskList = objAllocation.taskList;
    this.masterModel.allocationCardState = objAllocation.cardState;
    this.masterModel.allocationStatusList = objAllocation.statusList;

    var objReports = this.AssignTaskStatusList(this.masterModel.reportStatusList,
      this.masterModel.reportTaskList, this.masterModel.reportCardState);

    this.masterModel.reportTaskList = objReports.taskList;
    this.masterModel.reportCardState = objReports.cardState;
    this.masterModel.reportStatusList = objReports.statusList;

    if (this.masterModel.reportTaskList && this.masterModel.reportTaskList.length > 0) {
     this.redirectToFBReports();
      this.masterModel.reportStatusList = this.assignStatusTaskData(this.masterModel.reportStatusList);
    }
    
    if (this.masterModel.quotationTaskList && this.masterModel.quotationTaskList.length > 0) {
      this.assignQuotationTaskLink(this.masterModel.quotationTaskList);
      this.masterModel.quotationStatusList = this.assignStatusTaskData(this.masterModel.quotationStatusList);
      this.assignQuotationTaskLink(this.masterModel.quotationStatusList);
    }

    if (this.masterModel.bookingTaskList && this.masterModel.bookingTaskList.length > 0) {
      
      this.assignBookingTaskLink();

      this.masterModel.bookingStatusList = this.assignStatusTaskData(this.masterModel.bookingStatusList);
    }

    if (this.masterModel.allocationTaskList && this.masterModel.allocationTaskList.length > 0) {
    
      this.masterModel.allocationTaskList.forEach(element => {
        element.taskLink = 'schedule/schedule-pending';
      });
  
      this.masterModel.allocationStatusList = this.assignStatusTaskData(this.masterModel.allocationStatusList);
    }    

    this.masterModel.quotationStatusList.sort((a, b) => (a.trimName > b.trimName) ? 1
      : ((b.trimName > a.trimName) ? -1 : 0));

    this.masterModel.reportStatusList.sort((a, b) => (a.trimName > b.trimName) ? 1
      : ((b.trimName > a.trimName) ? -1 : 0));

  }

  assignQuotationTaskLink(data:TaskCountItem[]) {
    data.forEach(element => {

      if (element.taskName.toLocaleLowerCase() == CSDashboardCreated) {
        element.taskLink = '/inspsummary/quotation-pending/3';
      }
      else if (element.taskName.toLocaleLowerCase() == CSDashboardQuotationSent) {
        //sent-Quotation Sent
        element.taskLink = 'quotations/quotation-clientpending';
      }
      else if (element.taskName.toLocaleLowerCase() == CSDashboardCustomerConfirmed) {
        //validate - Customer Confirmed
        element.taskLink = '/quotations/quotation-confirm';
      }
      else if (element.taskName.toLocaleLowerCase() == CSDashboardQuotationApproved) {
        //manager approve  - Quotation Approved
        element.taskLink = '/quotations/quotation-approve';
      }
      else if (element.taskName.toLocaleLowerCase() == CSDashboardQuotationModify) {
        //create - Quotation Modify
        element.taskLink = '/quotations/quotation-rejected';
      }
      // element.taskLink = 'quotation/quotation-summary';
    });
  }

  assignBookingTaskLink() {
    this.masterModel.bookingTaskList.forEach(element => {
      if (element.taskName.toLocaleLowerCase() == CSDashboardBookingVerified) {
        element.taskLink = 'inspsummary/booking-pendingverification';
      }
      else if (element.taskName.toLocaleLowerCase() == CSDashboardBookingConfirmed) {
        element.taskLink = 'inspsummary/booking-pendingconfirmation';
      }
    });
  }

  assignStatusTaskData(statusList) {

    var _statusList = statusList;

    var statusNoTaskList = JSON.parse(JSON.stringify(statusList.filter(x => x.taskCount <= 0)));

    var statusDataList = JSON.parse(JSON.stringify(statusList.filter(x => x.taskCount > 0)));

    //if task less than two, we have remove that  task count from status list
    var statusLength = statusDataList.length > ListLengthTwo ? ListLengthTwo : statusDataList.length;
    for (let i = 0; i < statusLength; i++) {
      statusDataList[i].taskCount = 0;
    }
    if (statusLength > 0) {
      _statusList = statusDataList.concat(statusNoTaskList);
    }
    else {
      _statusList = statusList;
    }
    return _statusList;
  }

  //Assign task Status & Task List 
  AssignTaskStatusList(statusList, taskList, cardState) {

    var objStatusTask = new TaskStatusList();

    var _taskCountLength = statusList.filter(x => x.taskCount > ZeroValue).length;

    taskList = statusList.filter(x => x.taskCount > ZeroValue);

    // 1(cardstate) - 0 no task
    if (_taskCountLength == ZeroValue) {
      cardState = TaskCardState.noTask;
    }
    else if (_taskCountLength == ListLengthOne) { // 2 - 1 task
      cardState = TaskCardState.OneTask;
      taskList = taskList.slice(ZeroValue, ListLengthOne);
    }
    else if (_taskCountLength >= ListLengthTwo) { // 3 - 2 task
      cardState = TaskCardState.TwoTask;
      taskList = taskList.slice(ZeroValue, ListLengthTwo);
    }

    objStatusTask.cardState = cardState;
    objStatusTask.statusList = statusList;
    objStatusTask.taskList = taskList;

    return objStatusTask;
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.masterModel.supsearchRequest = new CommonDataSourceRequest();
    this.masterModel.supsearchRequest.customerId = this.request.customerId;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;
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
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.masterModel.supsearchRequest.searchText = this.masterModel.supInput.getValue();
    this.masterModel.supsearchRequest.skip = this.masterModel.supplierList.length;

    this.masterModel.supsearchRequest.customerId = this.request.customerId;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;
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

  //country change event
  countryChange(countryItem) {

    if (countryItem) {

      if (this.request.countryIdList && this.request.countryIdList.length > 0) {

        var countryLength = this.request.countryIdList.length;

        var countryDetails = [];
        for (var i = 0; i < countryLength; i++) {

          countryDetails.push(this.masterModel.countryList.find(x => x.id == this.request.countryIdList[i]).name);
        }
        this.masterModel.countryNameList = countryDetails;
      }
      else {
        this.masterModel.countryNameList = [];
      }
    }
  }
  //supplier change event
  supplierChange(supplierItem) {
    if (supplierItem && supplierItem.id > 0) {
      var supplierDetails = this.masterModel.supplierList.find(x => x.id == supplierItem.id);
      if (supplierDetails)
        this.masterModel.supplierName =
          supplierDetails.name.length > SupplierNameTrim ?
            supplierDetails.name.substring(0, SupplierNameTrim) + "..." : supplierDetails.name;
    }
    else {
      this.masterModel.supplierName = "";
    }
  }

  //fetch the first 10 suppliers for the customer on load
  getFactListBySearch() {
    this.masterModel.requestFactModel = new CommonDataSourceRequest();
    this.masterModel.requestFactModel.customerId = this.request.customerId;
    this.masterModel.requestFactModel.supplierType = SupplierType.Factory;
    this.masterModel.factInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.factLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterModel.requestFactModel, term)
        : this.supService.getFactoryDataSourceList(this.masterModel.requestFactModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.factLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.factoryList = data;
        this.masterModel.factLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.requestFactModel.searchText = this.masterModel.factInput.getValue();
      this.masterModel.requestFactModel.skip = this.masterModel.factoryList.length;
    }
    this.masterModel.supsearchRequest.customerId = this.request.customerId;
    this.masterModel.requestFactModel.supplierType = SupplierType.Factory;
    this.masterModel.factLoading = true;
    this.supService.getFactoryDataSourceList(this.masterModel.requestFactModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.factoryList = this.masterModel.factoryList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.masterModel.requestFactModel.skip = 0;
          this.masterModel.requestFactModel.take = ListSize;
        }
        this.masterModel.factLoading = false;
      }),
      error => {
        this.masterModel.factLoading = false;
        this.setError(error);
      };
  }

  //supplier change event
  factoryChange(item) {
    if (item) {

      if (this.request.factoryIdList && this.request.factoryIdList.length > 0) {

        var factoryLength = this.request.factoryIdList.length;

        var factoryDetails = [];
        for (var i = 0; i < factoryLength; i++) {

          factoryDetails.push(this.masterModel.factoryList.find(x => x.id == this.request.factoryIdList[i]).name);
        }
        this.masterModel.factoryNameList = factoryDetails;
      }
      else {
        this.masterModel.factoryNameList = [];
      }
    }
  }

  getCustomerListBySearch() {

    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.masterModel.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.masterModel.requestCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.requestCustomerModel.searchText = this.masterModel.customerInput.getValue();
      this.masterModel.requestCustomerModel.skip = this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.masterModel.requestCustomerModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.customerList = this.masterModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.masterModel.requestCustomerModel.skip = 0;
          this.masterModel.requestCustomerModel.take = ListSize;
        }
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  changeCustomerData(item) {

    this.request.supplierId = null;
    this.request.factoryIdList = [];
    this.request.brandIdList = [];
    this.request.buyerIdList = [];
    this.request.deptIdList = [];

    this.getSupListBySearch();
    this.getFactListBySearch()
    if (item && item.id > 0) {

      var customerDetails = this.masterModel.customerList.find(x => x.id == item.id);
      if (customerDetails)
        this.masterModel.customerName =
          customerDetails.name.length > SupplierNameTrim ?
            customerDetails.name.substring(0, SupplierNameTrim) + "..." : customerDetails.name;

      this.masterModel.brandSearchRequest.customerId = item.id;
      this.masterModel.deptSearchRequest.customerId = item.id;
      this.masterModel.buyerSearchRequest.customerId = item.id;
      this.getBrandListBySearch();
      this.getBuyerListBySearch();
      this.getDeptListBySearch();
    }
    else {
      this.masterModel.customerName = "";
      this.masterModel.brandList = [];
      this.masterModel.buyerList = [];
      this.masterModel.deptList = [];
      this.masterModel.requestCustomerModel.searchText = "";
      this.getCustomerListBySearch();
    }
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
    this.request = new CSDashboardRequest();
    this.dateSelectionDefault();

    this.masterModel.customerName = "";
    this.masterModel.countryNameList = [];
    this.masterModel.supplierName = "";
    this.masterModel.factoryNameList = [];
    this.masterModel.officeNameList = [];
    this.masterModel.serviceTypeNameList = [];
    this.masterModel.brandNameList = [];
    this.masterModel.buyerNameList = [];
    this.masterModel.deptNameList = [];
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

  //get office list
  getOfficeList() {
    this.masterModel.officeLoading = true;
    this.refService.getOfficeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.officeList = response.dataSourceList;
          this.masterModel.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.officeLoading = false;
        });
  }

  //get service type list
  getServiceTypeList() {
    this.masterModel.serviceTypeLoading = true;
    this.refService.getServiceTypeList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.serviceTypeList = response.dataSourceList;
          this.masterModel.serviceTypeLoading = false;

        },
        error => {
          this.setError(error);
          this.masterModel.serviceTypeLoading = false;
        });
  }

  //filter details has to show
  filterTextShown() {
    var isFilterDataSelected = true;

    if (this.request.customerId > 0 || this.request.supplierId > 0
      || (this.request.factoryIdList && this.request.factoryIdList.length > 0) ||
      (this.request.countryIdList && this.request.countryIdList.length > 0) ||
      (this.request.deptIdList && this.request.deptIdList.length > 0) ||
      (this.request.brandIdList && this.request.brandIdList.length > 0) ||
      (this.request.officeIdList && this.request.officeIdList.length > 0) ||
      (this.request.serviceTypeList && this.request.serviceTypeList.length > 0) ||
      (this.request.buyerIdList && this.request.buyerIdList.length > 0)) {

      //desktop version
      if (!this.isMobile) {
        if (this.request.customerId) {
          var customerDetails = this.masterModel.customerList.find(x => x.id == this.request.customerId);
          this.masterModel.customerName = customerDetails ? customerDetails.name : "";
        }
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;

        if (this.request.supplierId > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.request.countryIdList && this.request.countryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.request.brandIdList && this.request.brandIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.request.buyerIdList && this.request.buyerIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.request.officeIdList && this.request.officeIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.request.deptIdList && this.request.deptIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.request.serviceTypeList && this.request.serviceTypeList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.request.factoryIdList && this.request.factoryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        this.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.filterCount = 0;
        this.masterModel.countryNameList = [];
        this.masterModel.supplierName = "";
        this.masterModel.customerName = "";
        this.masterModel.factoryNameList = [];
        this.masterModel.officeNameList = [];
        this.masterModel.serviceTypeNameList = [];
        this.masterModel.brandNameList = [];
        this.masterModel.buyerNameList = [];
        this.masterModel.deptNameList = [];
      }
    }

    return isFilterDataSelected;
  }

  officeChange(item) {
    if (item) {

      if (this.request.officeIdList && this.request.officeIdList.length > 0) {

        var officeLength = this.request.officeIdList.length;

        var officeDetails = [];
        var maxLength = officeLength > 2 ? 2 : officeLength;
        for (var i = 0; i < maxLength; i++) {

          officeDetails.push(this.masterModel.officeList.find(x => x.id == this.request.officeIdList[i]).name);
        }

        if (officeLength > 2) {
          officeDetails.push(" " + (officeLength - 2) + "more");
        }
        this.masterModel.officeNameList = officeDetails;
      }
      else {
        this.masterModel.officeNameList = [];
      }
    }
  }

  serviceTypeChange(item) {
    if (item) {

      if (this.request.serviceTypeList && this.request.serviceTypeList.length > 0) {

        var serviceTypeLength = this.request.serviceTypeList.length;

        var serviceTypeDetails = [];
        var maxLength = serviceTypeLength > 2 ? 2 : serviceTypeLength;
        for (var i = 0; i < maxLength; i++) {

          serviceTypeDetails.push(this.masterModel.serviceTypeList.find(x => x.id == this.request.serviceTypeList[i]).name);
        }

        if (serviceTypeLength > 2) {
          serviceTypeDetails.push(" " + (serviceTypeLength - 2) + "more");
        }
        this.masterModel.serviceTypeNameList = serviceTypeDetails;
      }
      else {
        this.masterModel.serviceTypeNameList = [];
      }
    }
  }
  brandChange(item) {
    if (item) {

      if (this.request.brandIdList && this.request.brandIdList.length > 0) {

        var brandLength = this.request.brandIdList.length;

        var brandDetails = [];
        for (var i = 0; i < brandLength; i++) {

          brandDetails.push(this.masterModel.brandList.find(x => x.id == this.request.brandIdList[i]).name);
        }
        this.masterModel.brandNameList = brandDetails;
      }
      else {
        this.masterModel.brandNameList = [];
      }
    }
  }
  buyerChange(item) {
    if (item) {

      if (this.request.buyerIdList && this.request.buyerIdList.length > 0) {

        var buyerLength = this.request.buyerIdList.length;

        var buyerDetails = [];
        for (var i = 0; i < buyerLength; i++) {

          buyerDetails.push(this.masterModel.buyerList.find(x => x.id == this.request.buyerIdList[i]).name);
        }
        this.masterModel.buyerNameList = buyerDetails;
      }
      else {
        this.masterModel.buyerNameList = [];
      }
    }
  }
  deptChange(item) {
    if (item) {

      if (this.request.deptIdList && this.request.deptIdList.length > 0) {

        var deptLength = this.request.deptIdList.length;

        var deptDetails = [];
        for (var i = 0; i < deptLength; i++) {

          deptDetails.push(this.masterModel.deptList.find(x => x.id == this.request.deptIdList[i]).name);
        }
        this.masterModel.deptNameList = deptDetails;
      }
      else {
        this.masterModel.deptNameList = [];
      }
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

  //fetch the country data with virtual scroll
  getCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.countryRequest.searchText = this.masterModel.countryInput.getValue();
      this.masterModel.countryRequest.skip = this.masterModel.countryList.length;
    }

    this.masterModel.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.masterModel.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.countryList = this.masterModel.countryList.concat(customerData);
        }
        if (isDefaultLoad)
          this.masterModel.countryRequest = new CountryDataSourceRequest();
        this.masterModel.countryLoading = false;
      }),
      error => {
        this.masterModel.countryLoading = false;
        this.setError(error);
      };
  }

  // getColor(index){
  //   if(this.tableData.actual[index] < this.tableData.budget[index] && this.tableData.actual[index] > 0)
  //   return true;
  // }

  search() {

    // this.isFilterOpen = true;

    this.isFilterOpen = false;

    this.validator.isSubmitted = true;
    if(this.isFormValid()){
      this.masterModel.searchLoading = true;
    this.filterDataShown = this.filterTextShown();
    // this.request = new CSDashboardRequest();
    // this.summaryModel = new FinanceDashboardLoader();
    this.chartLoad();
    // this.colorSet = new am4core.ColorSet();
    // this.tableData = new TableModel();
    }
  }

  chartLoad() {

    this.masterModel.serviceTypeGraphLoading = true;
    this.masterModel.mandayByOfficeLoading = true;
    this.masterModel.reportCountLoading = true;
    this.masterModel.statusCountLoading = true;
    this.masterModel.newCountDashboardLoading = true;

    this.getStatusByLoggedUserList();
    this.getCountNewBookingRelatedDetails();
    this.getInspectionServiceTypeList();
    this.getInspectionMandayByOfficeList();
    this.getTotalReportsCountByDayList();


    // this.getBilledMandayData();
    // await this.getBookingIdList();
    // if(this.bookingIdList && this.bookingIdList.length > 0){
    // this.getFinanceDashboardTurnOverSummary();
    // }
    // else{
    //   this.masterModel.countryDataFound = false;
    //   this.masterModel.prodCategoryDataFound = false;
    //   this.masterModel.chargeBackDataFound = false;
    //   this.summaryModel.countryChartLoading = false;
    //   this.summaryModel.quotationchartLoading = false;
    // }
  }

  //form validation
  isFormValid(): boolean {
    return this.validator.isValid('serviceDateFrom') &&
			this.validator.isValid('serviceDateTo');
  }
  clearFactory(){
    this.masterModel.factoryList = [];
      this.getFactListBySearch();
  }
  clearSupplier(){
    this.masterModel.supplierList = [];
      this.getSupListBySearch();
  }
}

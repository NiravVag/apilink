import { Component, OnInit, ViewChild, ElementRef, NgZone } from '@angular/core';
import {  NgbCalendar, NgbModal, NgbModalRef, } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { DetailComponent } from '../../../common/detail.component';
import { ToastrService } from 'ngx-toastr';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { trigger, state, style, transition, animate } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { Validator } from '../../../common/validator';
import { TranslateService } from "@ngx-translate/core";
import { QcDashBoardService } from 'src/app/_Services/dashboard/qcdashboard.service';
import { QcDashboardCalendar,
	QcDashboardCalendarResponse,
	QcDashboardChartReportItem,
	QcDashboardChartReportResponse
	,QcDashboardCountResponse,
	QcRejectionReportsResponse,
	QcDashboard,QcDashboardSearchRequest
	} from '../../../../_Models/dashboard/qcdashboard.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
	 


am4core.useTheme(am4themes_animated);

@Component({
	selector: 'app-qcdashboard',
	templateUrl: './qcdashboard.component.html',
	styleUrls: ['./qcdashboard.component.scss']	,
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
export class QcdashboardComponent extends DetailComponent {
	
	componentDestroyed$: Subject<boolean> = new Subject();
	isLoading: boolean;
	slideWidth: number;
	totalSlideCount: number;
	currentTransform: number;
	visibleSlideCount: number;
	currentSlideCount: number;
	@ViewChild('slideContainer') slideContainer: ElementRef;
	dashboardCalendar: QcDashboardCalendarResponse;
	qcCalendar:QcDashboardCalendar[];
	productivityChartData:Array<QcDashboardChartReportItem>;
	QcRejectionChartModel:QcRejectionReportsResponse;
	masterModel:QcDashboard;
	dashboardCount:QcDashboardCountResponse;
	filterOpen: boolean;
	model:QcDashboardSearchRequest;
	public modelRef: NgbModalRef;
	public modelPopUpRef: NgbModalRef;

	constructor(router: Router, route: ActivatedRoute,private zone: NgZone ,
		public accService: UserAccountService,
		public utility: UtilityService,
		public validator: Validator, translate: TranslateService,  toastr: ToastrService,
		 public qcDashboardService: QcDashBoardService,public calendar: NgbCalendar)
	 {
		super(router, route, translate, toastr);
		if(window.innerWidth > 1024) { // desktop
			this.slideWidth = 246;
			this.visibleSlideCount = 5;
			this.currentSlideCount = 2;
		}
		else if(window.innerWidth < 400) { // mobile
			this.slideWidth = 243;
			this.visibleSlideCount = 1;
			this.currentSlideCount = 4;
		}
		else { // tablet
			this.slideWidth = 218;
			this.visibleSlideCount = 3;
			this.currentSlideCount = 3;
		}
		this.totalSlideCount = 9;
		this.filterOpen=false;
		this.validator.isSubmitted = false;
		this.validator.setJSON("statistics/qc-dashboard/qc-dashboard.valid.json");
		this.validator.setModelAsync(() => this.model);			
		am4core.addLicense("CH238479116");
	}
	

	ngOnDestroy(): void {
		this.componentDestroyed$.next(true);
		this.componentDestroyed$.complete(); 
	  }
  
	onInit(): void {
		this.dashboardCalendar=new QcDashboardCalendarResponse();
		this.QcRejectionChartModel=new QcRejectionReportsResponse();
		this.dashboardCount=new QcDashboardCountResponse;
		this.masterModel=new QcDashboard();
		this.productivityChartData=new Array<QcDashboardChartReportItem>();
		this.model=new QcDashboardSearchRequest();

		this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 30);
		this.model.serviceDateTo =this.calendar.getToday();
	
		this.getQcSchedule();
		this.getQcDashboardCountDetails();
		this.getQcProductivityDetails();
		this.getQcRejectionDetails();

		setTimeout(() => {
			this.zone.runOutsideAngular(() => {
			});
		}, 10);
	}
		getViewPath(): string {
		return "";
	}
	getEditPath(): string {
		return "";
	}

	ngAfterViewInit() {
		 
		this.calculateCurrentTransform();
		this.initialAlignSlide();
	}

	getQcSchedule()
	{
	 
		this.qcDashboardService.getQcSchedule()
        .pipe(takeUntil(this.componentDestroyed$))
		  .subscribe(
			response => {
				if (response != null) {
					this.mapUpdateProperties(response);
				}
				 
			},	 
			error => {
			 
			});
	}
	getQcProductivityDetails()
	{
		this.masterModel.productivityChartLoading=true;
		this.masterModel.isProductivityChartRender=true;
		this.model.customerId=0;
		  this.qcDashboardService.getQcProductivityDetails(this.model)
		  .pipe(takeUntil(this.componentDestroyed$))
		  .subscribe(
			response => {
				if (response != null) {

					this.mapProductivityChartData(response);
				}
				this.masterModel.productivityChartLoading=false;
			},	  
			error => {
				 
				this.masterModel.productivityChartLoading=false;
			});
	}
	getQcDashboardCountDetails()
	{
		this.masterModel.rejectionChartLoading=true;
		
		  this.qcDashboardService.getQcDashboardCountDetails(this.model)
		  .pipe(takeUntil(this.componentDestroyed$))
		  .subscribe(
			response => {
				if (response != null) {
				this.dashboardCount=response;
				}
				this.masterModel.rejectionChartLoading=false;
			},	 
			error => {
				this.masterModel.rejectionChartLoading=false;
			});
	}

	getQcRejectionDetails()
	{
		this.masterModel.rejectionChartLoading=true;
		
		  this.qcDashboardService.getQcRejectionDetails(this.model)
		  .pipe(takeUntil(this.componentDestroyed$))
		  .subscribe(
			response => {
				if (response != null) {
				this.mapRejectionChartData(response);
				}
				this.masterModel.rejectionChartLoading=false;
			},	 
			error => {
				this.masterModel.rejectionChartLoading=false;
			});
	}

	mapProductivityChartData(response:QcDashboardChartReportResponse){
		response.qcReportscount.map((x)=>
		{
		x.serviceDate=new Date(x.serviceDate)
		});
		this.productivityChartData=response.qcReportscount;
	 
		if(this.productivityChartData.length>0)
		{
			setTimeout(() => {
				this.renderProductivityChart();
			}, 200);
		}
		else{
			this.masterModel.isProductivityChartRender=false;
		}
		 
	}
	mapRejectionChartData(response:QcRejectionReportsResponse){
		this.QcRejectionChartModel=response;
		var chartObj = [];
			chartObj.push({
				category:"",
				apiRejection:response.rejectionBooking,
				yourRejection:response.qcRejectionBooking
			});
			setTimeout(() => {
				this.renderRejectionChart(chartObj);
			}, 200);
		
	}

	mapUpdateProperties(response:QcDashboardCalendarResponse)
	{
		response.qcCalendar.map((x)=>
		{
		x.calendarDayName=this.getCalendarDay(+x.calendarDay),
		 x.calendarMonthName=this.getCalendarMonth(+x.calendarMonth),
		 x.dayClass=this.getCalendarDayClass(+x.dayType),
		 x.qcCalendarSchedule.map((y)=>
				{
				 y.bookingIdClass="booking-number";
				 let bookingIds=y.bookingIds
				 let lstBookingId=bookingIds.split(',');
				 if(lstBookingId.length==1){
					y.bookingIds="#"+lstBookingId[0];	
				 }
				 else if(lstBookingId.length==2){
					y.bookingIds="#"+lstBookingId.slice(0, 2).join(", #");
				 }else if(lstBookingId.length>2){
					y.bookingIds="#"+lstBookingId.slice(0, 2).join(", #")+"  ...";
					y.tooltipIds="#"+lstBookingId.slice(2, lstBookingId.length).join(",#");
					y.bookingIdClass="booking-number category-cta small mr-2 common-tooltip common-tooltip-top-2";
				 }
				});
		});
		this.dashboardCalendar=response;
		this.qcCalendar=this.dashboardCalendar.qcCalendar;
	}

	
	reset() {
		this.onInit();
	}

	search() {
		this.validator.initTost();
		this.validator.isSubmitted = true;

		if (this.isFormValid()) {
			 
			 this.getQcDashboardCountDetails();
			 this.getQcProductivityDetails();
			 this.getQcRejectionDetails();
		}
	}
	RedirectToFbReports() {
		this.accService.getUserTokenToFB()
        .pipe(takeUntil(this.componentDestroyed$))
		  .subscribe(
			response => {
			  if (response != null) {
				  console.log(response.reportsUrl);
				window.open(response.reportsUrl + "?token=" + response.token + "", "_blank");
			  }
			},
			error => {
			   
			}
		  );
	  }
	getCalendarDay(dayNumber:number)
	{
		let _dayName: string = "";
		switch (dayNumber) {
			case 0:
				_dayName= this.utility.textTranslate('QC_DASHBOARD.LBL_SUNDAY');
			  break;
			  case 1:
				_dayName= this.utility.textTranslate('QC_DASHBOARD.LBL_MONDAY');
			  break;
			  case 2:
				_dayName= this.utility.textTranslate('QC_DASHBOARD.LBL_TUESDAY');
			  break;
			  case 3:
				_dayName= this.utility.textTranslate('QC_DASHBOARD.LBL_WEDNESDAY');
			  break;
			  case 4:
				_dayName= this.utility.textTranslate('QC_DASHBOARD.LBL_THURSDAY');
			  break;
			  case 5:
				_dayName= this.utility.textTranslate('QC_DASHBOARD.LBL_FRIDAY');
			  break;
			  case 6:
				_dayName= this.utility.textTranslate('QC_DASHBOARD.LBL_SATURDAY');
			  break;
		  }
		return _dayName;
	}

	getCalendarMonth(monthNumber:number)
	{
		let _monthName: string = "";
		switch (monthNumber) {
			case 1:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_JANUARY');
			  break;
			  case 2:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_FEBRUARY');
			  break;
			  case 3:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_MARCH');
			  break;
			  case 4:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_APRIL');
			  break;
			  case 5:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_MAY');
			  break;
			  case 6:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_JUNE');
			  break;
			  case 7:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_JULY');
			  break;
			  case 8:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_AUGUST');
			  break;
			  case 9:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_SEPTEMBER');
			  break;
			  case 10:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_OCTOBER');
			  break;
			  case 11:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_NOVEMBER');
			  break;
			  case 12:
				_monthName= this.utility.textTranslate('QC_DASHBOARD.LBL_DECEMBER');
			  break;
		  }
		return _monthName;
	}

	getCalendarDayClass(dayTpe:number)
	{
		let className="";
			switch (dayTpe) {
				case 0:
					className="slide previous-day";
				break;
				case 1:
					className="slide current-day";
				break;
				case 2:
					className="slide next-day";
				break;
			}
		return className;
	}


	initialAlignSlide() {
		let newValue = Math.round(this.currentTransform - (this.slideWidth * this.currentSlideCount));
		this.slideContainer.nativeElement.style.transform = `translate(${newValue}px)`;
		this.slideContainer.nativeElement.style.transition = 'ease all 0.2s';
	}

	calculateCurrentTransform() {
		let parentContainer = document.querySelector('.qc-calendar-slider-container');
		let style = window.getComputedStyle(parentContainer);
		let matrix = new WebKitCSSMatrix(style.webkitTransform);
		this.currentTransform = matrix.m41;
	}

	slideNext(){
		if(this.currentSlideCount == this.totalSlideCount - this.visibleSlideCount) { return; }
		this.calculateCurrentTransform();
		let newValue = Math.round(this.currentTransform - this.slideWidth);
		this.slideContainer.nativeElement.style.transform = `translate(${newValue}px)`;
		this.currentSlideCount++;
	}

	slidePrevious(){
		this.calculateCurrentTransform();
		let newValue = Math.round(this.currentTransform + this.slideWidth);

		if(this.currentSlideCount <= 0) { return; }
		this.slideContainer.nativeElement.style.transform = `translate(${newValue}px)`;
		this.currentSlideCount--;
	}
	
	renderProductivityChart() {
	 
		let chart = am4core.create("productivityGraph", am4charts.XYChart);
 
		chart.data =this.productivityChartData;//this.generateChartData();//
		//chart.data = this.generateChartData();
		let dateAxis = chart.xAxes.push(new am4charts.DateAxis());
		dateAxis.renderer.labels.template.disabled = true;
		dateAxis.renderer.minGridDistance = 20;
		dateAxis.renderer.labels.template.fontSize = 12;
		dateAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
		dateAxis.dateFormatter = new am4core.DateFormatter();
		dateAxis.dateFormatter.dateFormat = "MM-dd";
		dateAxis.renderer.grid.template.disabled = true;

		let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
		valueAxis.renderer.minGridDistance = 20;
		valueAxis.renderer.labels.template.fontSize = 12;
		valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
		valueAxis.renderer.grid.template.disabled = true;

		let series = chart.series.push(new am4charts.LineSeries());
		series.dataFields.valueY = "reportCount";
		series.dataFields.dateX = "serviceDate";
		series.strokeWidth = 2;
		 
		series.minBulletDistance = 10;
		series.fillOpacity = 0.1;
		series.tooltip.pointerOrientation = "vertical";
		series.tooltip.background.cornerRadius = 20;
		series.tooltip.background.fillOpacity = 0.5;
		series.tooltip.label.padding(12, 12, 12, 12)
		let bullet = series.bullets.push(new am4charts.CircleBullet());
		bullet.tooltipText = "{serviceDate.formatDate('d MMM')}[bold]:{reportCount} [/]";
		bullet.circle.radius = 2;
 


		let seriesRange = dateAxis.createSeriesRange(series);
		seriesRange.contents.strokeDasharray = "2,3";
		seriesRange.contents.stroke = chart.colors.getIndex(8);
		//seriesRange.contents.strokeWidth = 3;
	 

		let pattern = new am4core.LinePattern();
		pattern.rotation = -45;
		pattern.stroke = seriesRange.contents.stroke;
		pattern.width = 1000;
		pattern.height = 1000;
		pattern.gap = 6;
		seriesRange.contents.fill = pattern;
		seriesRange.contents.fillOpacity = 0.5;
	}
	 
	renderRejectionChart(chartObj) {
		let chart = am4core.create('rejectionChart', am4charts.XYChart)
		
		let xAxis = chart.xAxes.push(new am4charts.CategoryAxis())
		xAxis.renderer.labels.template.disabled = true;
		xAxis.dataFields.category = 'category'
		//xAxis.renderer.cellStartLocation = 0.1;
		//xAxis.renderer.cellEndLocation = 0.9;
		xAxis.renderer.labels.template.fontSize = 12;
		xAxis.renderer.grid.template.disabled = true;


		 
		let yAxis = chart.yAxes.push(new am4charts.ValueAxis());
		yAxis.renderer.labels.template.fontSize = 12;
		yAxis.renderer.grid.template.disabled = true;
		yAxis.min = 0;
		yAxis.renderer.grid.template.strokeDasharray = "5,2";

		chart.data =chartObj;

		let _apiRejection= this.utility.textTranslate('QC_DASHBOARD.LBL_API_REJECTION');
		let _yourRejection= this.utility.textTranslate('QC_DASHBOARD.LBL_YOUR_REJECTION');

		 
		this.createSeries(chart,'apiRejection', _apiRejection,'#68b8dc');
		this.createSeries(chart,'yourRejection', _yourRejection,'#6794dd');
	}
	createSeries(chart,value, name,color) {
		let series = chart.series.push(new am4charts.ColumnSeries())
		series.dataFields.valueY = value
		series.dataFields.categoryX = 'category'
		series.name = name
		//series.columns.template.width = am4core.percent(60);
		series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]";
		series.columns.template.fill = am4core.color(color);
		let bullet = series.bullets.push(new am4charts.LabelBullet())
		bullet.interactionsEnabled = false
		bullet.dy = 30;
		series.columns.template.strokeOpacity = 0;
		series.columns.template.column.cornerRadiusTopRight = 6;
		series.columns.template.column.cornerRadiusTopLeft = 6;
		return series;
	}
	isFormValid(): boolean {
		return this.validator.isValid('serviceDateFrom') &&
			this.validator.isValid('serviceDateTo');
	}
	toggleFilter() {
		this.filterOpen = !this.filterOpen;
	  }
	  clearDateInput(controlName:any){
		switch(controlName) {
		  case "Fromdate": { 
			this.model.serviceDateFrom=null;
			break; 
		 } 
		 case "Todate": { 
		  this.model.serviceDateTo =null;
			break; 
		 } 
		}
	  }
}

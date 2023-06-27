import { Component } from '@angular/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import * as am4core from "@amcharts/amcharts4/core";
import { Router, ActivatedRoute } from '@angular/router';
import { NgbDateParserFormatter, NgbCalendar, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'src/app/_Services/location/location.service';
import { DetailComponent } from '../../common/detail.component';
import { ManDayUtilizationDashboard, MandayUtilizationRequest, UtilizationDashboard, UtilizationGraphData } from 'src/app/_Models/statistics/manday-Utilization-dashboard.model';
import { MandayDashboardResult, MandayYearChart, MandayYear, MandayEmployeeTypeChart, MandayEmployeeTypeChartResponse,FilterModel } from 'src/app/_Models/statistics/manday-dashboard.model';
import { Validator } from '../../common/validator';
import { ResponseResult, CommonDataSourceRequest, DataSource, CountryDataSourceRequest } from 'src/app/_Models/common/common.model';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { first, distinctUntilChanged, debounceTime, switchMap, catchError, tap, map, takeUntil } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { APIService, UtilizationChartMinValue, UtilizationChartMaxValue } from '../../common/static-data-common';
import { MandayUtilisationDashboardService } from 'src/app/_Services/statistics/manday-utilization.service';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
	selector: 'app-manday-utilization-dashboard',
	templateUrl: './manday-utilization-dashboard.component.html',
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
 
export class MandayUtilizationDashboardComponent extends DetailComponent {

	componentDestroyed$: Subject<boolean> = new Subject();
	private chart: am4charts.XYChart;
	masterModel: ManDayUtilizationDashboard;
	model: MandayUtilizationRequest;
	mandayYearChart: Array<MandayYearChart>;
	monthYearXAxis: Array<MandayYear>;
	mandayEmployeeTypeChart: Array<MandayEmployeeTypeChart>;
	mandayEmployeeTypeXAxis: Array<MandayYear>;
	clusteredColumChart: any;
	utilizationData: Array<UtilizationDashboard>;
	utilizationGraphData: UtilizationGraphData;
	requestCountryModel: CountryDataSourceRequest;
	filterOpen: boolean;
	toggleAdvanceSearch: boolean;
	filterModel:FilterModel;
	constructor(router: Router, route: ActivatedRoute, public validator: Validator, translate: TranslateService, toastr: ToastrService,
		public dateparser: NgbDateParserFormatter, public calendar: NgbCalendar,
		public locationService: LocationService, private mandayDashboardService: MandayDashboardService,
		private mandayUtilisationService: MandayUtilisationDashboardService) {
		super(router, route, translate, toastr);
		this.filterOpen = false;
		this.toggleAdvanceSearch = false;
		this.validator.isSubmitted = false;
		this.validator.setJSON("statistics/manday-dashboard/manday-dashboard.valid.json");
		this.validator.setModelAsync(() => this.model);		
		am4core.addLicense("CH238479116");
	}

	ngOnDestroy(): void {
	  this.componentDestroyed$.next(true);
	  this.componentDestroyed$.complete(); 
	}

	onInit(): void {
		
		this.mandayYearChart = new Array<MandayYearChart>();
		this.monthYearXAxis = new Array<MandayYear>();
		this.mandayEmployeeTypeChart = new Array<MandayEmployeeTypeChart>();
		this.mandayEmployeeTypeXAxis = new Array<MandayYear>();
		this.utilizationGraphData = new UtilizationGraphData();
		this.utilizationData = new Array<UtilizationDashboard>();

		this.requestCountryModel = new CountryDataSourceRequest();
		this.model = new MandayUtilizationRequest();
		this.masterModel = new ManDayUtilizationDashboard();

		this.filterModel= new FilterModel();
		this.filterModel.OtherFilter=[];

		this.model.serviceId = APIService.Inspection;

		this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 7);
		this.model.serviceDateTo = this.calendar.getToday();
		
		this.getServiceList();
		this.getOfficeList();
		this.getCountryListBySearch();
		this.getMandaysByYear(true);
		this.getMandaysByEmployeeTypeChart(true);
		this.getMandaysByUtilisation(true);
	}

	getViewPath(): string {
		return "";
	}
	getEditPath(): string {
		return "";
	}

	setFilerModel(){
		let _otherfiler=[];
		this.filterModel.fromDate=this.model.serviceDateFrom.day+"/"+this.model.serviceDateFrom.month+"/"+this.model.serviceDateFrom.year;
		this.filterModel.toDate=this.model.serviceDateTo.day+"/"+this.model.serviceDateTo.month+"/"+this.model.serviceDateTo.year;
		
		if( this.model.officeIdList&& this.model.officeIdList.length>0){
			let _Office="";
			if( this.model.officeIdList.length==1){
				_Office= this.masterModel.officeList.filter(
				item => item.id === this.model.officeIdList[0])[0].name;
			}else{
				_Office=""+this.model.officeIdList.length+" Selected"
			}
			_otherfiler.push("Office :"+_Office);
		}
	
		if( this.model.countryIdList&& this.model.countryIdList.length>0){
			let _Country="";
			if( this.model.countryIdList.length==1){
				_Country= this.masterModel.countryList.filter(
					item => item.id === this.model.countryIdList[0])[0].name;
			}else{
				_Country=""+this.model.countryIdList.length+" Selected"
			}
			_otherfiler.push("Country :"+_Country);
		}

		if(this.masterModel.serviceList){
			let _service= this.masterModel.serviceList.filter(
				item => item.id === this.model.serviceId)[0].name;

			_otherfiler.push("Service Type :"+_service);
		}
		
		this.filterModel.OtherFilter=_otherfiler;
	}

	//get service type list
	getServiceList() {

		this.masterModel.serviceLoading = true;
		this.mandayDashboardService.getServiceList()
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response && response.result == ResponseResult.Success)
						this.masterModel.serviceList = response.dataSourceList;
						this.setFilerModel();
					this.masterModel.serviceLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.serviceLoading = false;
				});
	}

	//change office
	changeOffice(officeSelected: DataSource[]) {
		
		this.model.countryIdList = [];
		this.masterModel.countryList = [];

		if (officeSelected && officeSelected.length > 0) {
			this.getCountryListByOffice(officeSelected.map(x => x.id));
		}
		else {			
			this.getCountryData(false);
		}
	}

	//get office list
	getOfficeList() {
		this.masterModel.officeLoading = true;
		this.mandayDashboardService.getOfficeList()
        .pipe(takeUntil(this.componentDestroyed$))
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

	renderGaugeChart() {

		let data = this.utilizationGraphData;

		// create chart
		let chart = am4core.create("GaugeChartDiv", am4charts.GaugeChart);
		this.chartCircularValue(chart);
		
		//Normal axis
		let axis = chart.xAxes.push(new am4charts.ValueAxis<am4charts.AxisRendererCircular>());
		this.chartCircularAxisValue(axis);
		
		//Axis for ranges
		let axis2 = chart.xAxes.push(new am4charts.ValueAxis<am4charts.AxisRendererCircular>());
		this.chartCircularAxis2Value(axis2);
	
		// Ranges
		for (let grading of data.gradingData) {
			let range = axis2.axisRanges.create();
			range.axisFill.fill = am4core.color(grading.color);
			range.axisFill.fillOpacity = 0.8;
			range.axisFill.zIndex = -1;
			range.value = grading.lowScore > UtilizationChartMinValue ? grading.lowScore : UtilizationChartMinValue;
			range.endValue = grading.highScore < UtilizationChartMaxValue ? grading.highScore : UtilizationChartMaxValue;
			range.grid.strokeOpacity = 0;
			range.label.text = grading.title.toUpperCase();
			range.label.inside = true;
			range.label.location = 0.5;
			range.label.paddingBottom = -5; // ~half font size
			range.label.fontSize = "6px";
		}

		let matchingGrade = lookUpGrade(data.totalUtilization, data.gradingData);
		//Label 1
		let label = chart.radarContainer.createChild(am4core.Label);
		let label2 = chart.radarContainer.createChild(am4core.Label);

		if (matchingGrade) {

			label.isMeasured = false;
			label.fontSize = "20px";
			label.x = am4core.percent(50);
			label.paddingBottom = 15;
			label.horizontalCenter = "middle";
			label.verticalCenter = "bottom";
			label.text = data.totalUtilization.toString();
			label.fill = am4core.color(matchingGrade.color);

			//Label 2
			label2.isMeasured = false;
			label2.fontSize = "8px";
			label2.horizontalCenter = "middle";
			label2.verticalCenter = "bottom";
			label2.text = matchingGrade.title.toUpperCase();
			label2.fill = am4core.color(matchingGrade.color);
		}
			// Grading Lookup
		function lookUpGrade(lookupScore, grades) {
			// Only change code below this 
			for (var i = 0; i < grades.length; i++) {
				if (grades[i].lowScore < lookupScore && grades[i].highScore >= lookupScore) {
					return grades[i];
				}
			}
			return null;
		}

		// Hand
		let hand = chart.hands.push(new am4charts.ClockHand());
		hand.axis = axis2;
		hand.innerRadius = am4core.percent(35);
		hand.startWidth = 6;
		hand.pin.disabled = true;
		hand.value = data.totalUtilization;
		hand.fill = am4core.color("#444");
		hand.stroke = am4core.color("#000");

		hand.events.on("positionchanged", function () {
			label.text =data.totalUtilization.toString(); //axis2.positionToValue(hand.currentPosition).toString();
			let matchingGrade = lookUpGrade(axis.positionToValue(hand.currentPosition), data.gradingData);
			label2.text = matchingGrade.title.toUpperCase();
			label2.fill = am4core.color(matchingGrade.color);
			label2.stroke = am4core.color(matchingGrade.color);
			label.fill = am4core.color(matchingGrade.color);
		});

		chart.padding(0, 0, 0, 0);
	}

	getCountryListByOffice(officeIdList: number[]) {
		this.masterModel.countryLoading = true;
		this.locationService.GetCountriesByOffice(officeIdList)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(res => {
				if (res.result == ResponseResult.Success) {
					this.masterModel.countryList = res.dataSourceList;
				}
				this.masterModel.countryLoading = false;
			}),
			error => {
				this.masterModel.countryLoading = false;
				this.setError(error);
			};
	}

	getCountryData(isDefaultLoad: boolean) {
		if (isDefaultLoad) {
			this.requestCountryModel.searchText = this.masterModel.countryInput.getValue();
			this.requestCountryModel.skip = this.masterModel.countryList.length;
		}

		this.masterModel.countryLoading = true;
		this.locationService.getCountryDataSourceList(this.requestCountryModel)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(customerData => {
				if (customerData && customerData.length > 0) {
					this.masterModel.countryList = this.masterModel.countryList.concat(customerData);
				}
				if (isDefaultLoad)
					this.requestCountryModel = new CountryDataSourceRequest();
				this.masterModel.countryLoading = false;
			}),
			error => {
				this.masterModel.countryLoading = false;
				this.setError(error);
			};
	}

	getCountryListBySearch() {
		this.masterModel.countryInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterModel.countryLoading = true),
			switchMap(term => term
				? this.locationService.getCountryDataSourceList(this.requestCountryModel, term)
				: this.locationService.getCountryDataSourceList(this.requestCountryModel)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterModel.countryLoading = false))
			))
			.subscribe(data => {
				this.masterModel.countryList = data;
				this.masterModel.countryLoading = false;
			});
	}
	//date control from and to vaild to search
	isFormValid(): boolean {
		return this.validator.isValid('serviceDateFrom') &&
			this.validator.isValid('serviceDateTo');
	}

	//search the data
	search() {
		this.validator.initTost();
		this.validator.isSubmitted = true;

		if (this.isFormValid()) {
			if (this.masterModel.mandayEmployeeTypeSubYearList &&
				this.masterModel.mandayEmployeeTypeSubYearList.length > 1) {
				this.model.mandayEmployeeTypeSubYear = this.masterModel.mandayEmployeeTypeSubYearList[0].id;
			}

			this.getMandaysByYear(true);
			this.getMandaysByEmployeeTypeChart(true);
			this.getMandaysByUtilisation(true);
			this.setFilerModel();
		}
	}
	
	//get manday list for each year
	getMandaysByYear(pageLoading: boolean) {
		this.masterModel.mandayYearLoading = pageLoading;

		this.mandayUtilisationService.getmandayByYear(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response) {
						if (response.result == MandayDashboardResult.success) {
							this.mandayYearChart = response.data;
							this.monthYearXAxis = response.monthYearXAxis;
							this.mandayByYearChartFrame();
						}
						else {
							this.mandayYearChart = new Array<MandayYearChart>();
						}
					}
					this.masterModel.mandayYearLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayYearLoading = false;
				});
	}

	// frame the manday year
	mandayByYearChartFrame() {

		if (this.mandayYearChart && this.mandayYearChart.length > 0) {

			let k: number = 2;
			var chartObj = [];

			//building below structure
			//{date:new Date(2019,1), value2:48, value3:51, value4:42}
			for (var i = 0; i < this.mandayYearChart.length; i++) {

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

					var monthData = this.mandayYearChart[i].monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + 1));

					chartObj[m]["value" + (k)] = monthData.length == 1 ? monthData[0].monthManDay : 0;
				}
				k = k + 1;
			}
		}
		setTimeout(() => {
			this.renderYearLineChart(chartObj, this.mandayYearChart);
		}, 100);
	}

	/**
	* Function to render line chart for manday by year
	*/
	renderYearLineChart(chartObj, mandayYearChar: MandayYearChart[]) {

		let chart = am4core.create("chartdiv", am4charts.XYChart);

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
			series.tooltipText = "{dateX}: [b]{valueY}[/]";
			series.tooltip.getFillFromObject = false;
			series.tooltip.background.fill = am4core.color(mandayYearChar[i].color);
		}
		chart.padding(20, 0, 20, 0);
		chart.cursor = new am4charts.XYCursor();

		this.chart = chart;
	}

	//if top selected we are not showing the sub country filters
	isshowCountry() {
		return (!this.model.countryIdList || this.model.countryIdList.length <= 0);
	}

	//get manday by year export
	getMandayByYearExport() {
		this.masterModel.mandayByYearExportLoading = true;

		this.mandayUtilisationService.getMandayByYearExport(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Manday_Year_Details.xlsx");
				this.masterModel.mandayByYearExportLoading = false;
			},
				error => {
					this.masterModel.mandayByYearExportLoading = false;
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

	reset() {
		this.onInit();
	}

	//get manday by employee type(outsource,. etc)
	getMandaysByEmployeeTypeChart(pageLoading: boolean) {
		let response: MandayEmployeeTypeChartResponse;
		this.masterModel.mandayEmployeeTypeLoading = pageLoading;
		this.masterModel.mandayEmployeeTypeChartLoading = !pageLoading;
		this.masterModel.searchLoading = pageLoading;
		this.mandayDashboardService.getmandayByEmployeeTypeChart(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response) {
						if (response.result == MandayDashboardResult.success) {
							this.mandayEmployeeTypeChart = response.data;
							this.mandayEmployeeTypeXAxis = response.monthYearXAxis;
							this.mandayByEmployeeTypeChartFrame();
						}
						else {
							this.mandayEmployeeTypeChart = new Array<MandayEmployeeTypeChart>();
						}
					}
					this.masterModel.mandayEmployeeTypeLoading = false;
					this.masterModel.mandayEmployeeTypeChartLoading = false;
					this.masterModel.searchLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayEmployeeTypeLoading = false;
					this.masterModel.mandayEmployeeTypeChartLoading = false;
					this.masterModel.searchLoading = false;
				});
	}

	//frame the manday by employee chart 
	mandayByEmployeeTypeChartFrame() {

		//building below structure
		// {
		// 	"month": "Jan",
		// 	"europe": 2.5,
		// 	"namerica": 2.5,
		// 	"asia": 2.1,
		// 	"lamerica": 1.2,
		// 	"meast": 5.2,
		// 	"africa": 0.1
		// }
		if (this.mandayEmployeeTypeXAxis && this.mandayEmployeeTypeXAxis.length > 0 && this.mandayEmployeeTypeChart &&
			this.mandayEmployeeTypeChart.length > 0) {

			var chartObj = [];

			for (var i = 0; i < this.mandayEmployeeTypeXAxis.length; i++) {

				var subchartObj = {};

				subchartObj["month"] = this.mandayEmployeeTypeXAxis[i].monthName;

				for (var j = 0; j < this.mandayEmployeeTypeChart.length; j++) {

					var monthData = this.mandayEmployeeTypeChart[j].monthlyData.filter(x => x.month ==
						(this.mandayEmployeeTypeXAxis[i].month + 1));

					subchartObj[this.mandayEmployeeTypeChart[j].employeeType.toLowerCase()] = monthData.length == 1 ?
						monthData[0].mandayPercentage : 0;

				}
				chartObj.push(subchartObj);
			}

			setTimeout(() => {
				this.renderEmployeeTypeChart(chartObj);
			}, 300);
		}
	}

	/**
	*4th chart based on employee type render
	*/
	renderEmployeeTypeChart(chartObj) {

		this.clusteredColumChart = am4core.create("ganttchartdiv", am4charts.XYChart);

		this.clusteredColumChart.data = chartObj;

		// Create axes
		let categoryAxis = this.clusteredColumChart.xAxes.push(new am4charts.CategoryAxis());
		categoryAxis.dataFields.category = "month";
		categoryAxis.renderer.grid.template.location = 0;
		categoryAxis.renderer.minGridDistance = 20;
		categoryAxis.renderer.cellStartLocation = 0.1;
		categoryAxis.renderer.cellEndLocation = 0.9;
		categoryAxis.renderer.labels.template.fontSize = 8;
		categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
		categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

		let valueAxis = this.clusteredColumChart.yAxes.push(new am4charts.ValueAxis());
		valueAxis.renderer.labels.template.fontSize = 8;
		valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
		valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
		valueAxis.min = 0;
		valueAxis.max = 100;
		for (var i = 0; i < this.mandayEmployeeTypeChart.length; i++) {

			let stacked = i == 0 ? false : true;

			this.createSeries(this.mandayEmployeeTypeChart[i].color, this.mandayEmployeeTypeChart[i].employeeType.toLowerCase(), this.mandayEmployeeTypeChart[i].employeeType, stacked);
		}
	}

	// Create series
	createSeries(color, field, name, stacked) {
		let series = this.clusteredColumChart.series.push(new am4charts.ColumnSeries());
		series.dataFields.valueY = field;
		series.dataFields.categoryX = "month";
		series.name = name;
		series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]" + "%";
		series.stacked = stacked;
		series.columns.template.width = am4core.percent(85);
		series.columns.template.fill = am4core.color(color);
	}

	//year change for employee type chart
	mandayEmployeeTypeSubYearChange() {
		this.getMandaysByEmployeeTypeChart(false);
	}

	//from date change - frame the year list - if date from and to have more than one year 
	fromDateChange(fromDate) {

		if (this.isFormValid()) {
			var serviceFromDate: NgbDate;
			if (fromDate != null && this.model.serviceDateTo != null) {

				serviceFromDate = new NgbDate(fromDate.year, fromDate.month, fromDate.day);

				this.masterModel.mandayEmployeeTypeSubYearList = [];

				if (serviceFromDate.year < this.model.serviceDateTo.year) {
					for (var i = serviceFromDate.year; i <= this.model.serviceDateTo.year; i++) {

						this.masterModel.mandayEmployeeTypeSubYearList.push(
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
			if (toDate != null && this.model.serviceDateFrom != null) {
				_toDate = new NgbDate(toDate.year, toDate.month, toDate.day);

				this.masterModel.mandayEmployeeTypeSubYearList = [];

				if (this.model.serviceDateFrom.year < _toDate.year) {
					for (var i = this.model.serviceDateFrom.year; i <= _toDate.year; i++) {

						this.masterModel.mandayEmployeeTypeSubYearList.push({
							id: i, name: "" + i
						});
					}
				}
			}
		}
	}

	//get manday list for each year
	getMandaysByUtilisation(pageLoading: boolean) {
		this.masterModel.mandayUtilisationLoading = pageLoading;

		this.mandayUtilisationService.getmandayByUtilisation(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response) {
						if (response.result == MandayDashboardResult.success) {
							this.utilizationData = response.data;
							this.utilizationGraphData = response.graphData;
							setTimeout(() => {
								this.renderGaugeChart();
							}, 100);
						}
						else {
							this.utilizationGraphData = new UtilizationGraphData();
							this.utilizationData = new Array<UtilizationDashboard>();
							
						}
					}
					this.masterModel.mandayUtilisationLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayUtilisationLoading = false;
				});
	}

	//get maday by employee type export
	getMandayByEmployeeTypeExport() {
		this.masterModel.mandayByEmployeeTypeExportLoading = true;

		this.mandayDashboardService.getManDayEmployeeTypeChartExport(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Manday_EmployeeType_Details.xlsx");
				this.masterModel.mandayByEmployeeTypeExportLoading = false;
			},
				error => {
					this.masterModel.mandayByEmployeeTypeExportLoading = false;
				});
	}
	//get manday by year export
	getUtilisationExport() {
		this.masterModel.mandayByUtilisationExportLoading = true;

		this.mandayUtilisationService.getMandayUtilisationExport(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Manday_Utilisation_Details.xlsx");
				this.masterModel.mandayByUtilisationExportLoading = false;
			},
				error => {
					this.masterModel.mandayByUtilisationExportLoading = false;
				});
	}

	//map the axis properties
	chartCircularAxisValue(axis) {
		//Normal axis
		axis.min = UtilizationChartMinValue;
		axis.max = UtilizationChartMaxValue;
		axis.strictMinMax = true;
		axis.renderer.radius = am4core.percent(80);
		axis.renderer.inside = true;
		axis.renderer.line.strokeOpacity = 0.1;
		axis.renderer.ticks.template.disabled = false;
		axis.renderer.ticks.template.strokeOpacity = 1;
		axis.renderer.ticks.template.strokeWidth = 0.5;
		axis.renderer.ticks.template.length = 5;
		axis.renderer.grid.template.disabled = true;
		axis.renderer.labels.template.radius = am4core.percent(15);
		axis.renderer.labels.template.fontSize = "8px";
	}

	chartCircularAxis2Value(axis2) {
		axis2.min = UtilizationChartMinValue;
		axis2.max = UtilizationChartMaxValue;
		axis2.strictMinMax = true;
		axis2.renderer.labels.template.disabled = true;
		axis2.renderer.ticks.template.disabled = true;
		axis2.renderer.grid.template.disabled = false;
		axis2.renderer.grid.template.opacity = 0.5;
		axis2.renderer.labels.template.bent = true;
		axis2.renderer.labels.template.fill = am4core.color("#000");
		axis2.renderer.labels.template.fontWeight = "bold";
		axis2.renderer.labels.template.fillOpacity = 0.3;
	}

	chartCircularValue(chart) {
		chart.hiddenState.properties.opacity = 0;
		chart.fontSize = 11;
		chart.innerRadius = am4core.percent(80);
		chart.resizable = true;

	}
	toggleAdvanceSearchSection() {
		this.toggleAdvanceSearch = !this.toggleAdvanceSearch;
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
		  this.model.serviceDateTo=null;
			break; 
		 } 
		}
	  }
}


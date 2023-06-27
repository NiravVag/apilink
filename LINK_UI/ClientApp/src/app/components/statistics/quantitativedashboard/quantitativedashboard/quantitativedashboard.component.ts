import { Component, OnInit, NgZone } from '@angular/core';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import * as am4plugins_sunburst from "@amcharts/amcharts4/plugins/sunburst";
import { trigger, state, style, transition, animate } from '@angular/animations';
import { DashboardServiceDateList, MandayDashboard, MandayYear, MandayYearChart, ProductCategoryRequest, QuantitativeDashBoardDataFound, QuantitativeDashBoardLoader, QuantitativeDashboardModel, QuantitativeDashboardRequest, ResponseResult } from 'src/app/_Models/statistics/quantitativedashboard.model';
import { QuantitativeDashBoardService } from 'src/app/_Services/statistics/quantitativedashboard.service';
import { NgbCalendar, NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { amCoreLicense, CountrySelectedFilterTextCount, CusDashboardProductLength, DecMonth, ListSize, MobileViewFilterCount, NumberOne, QuantitativeDashboardCountryNameTrim, QuantitativeDashboardProductLength, SearchType, SupplierNameTrim, SupplierType, supplierTypeList, UserType, YearCount } from 'src/app/components/common/static-data-common';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ProductDataSourceRequest, CustomerCommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { CustomerInfo } from 'src/app/_Models/dashboard/customerdashboard.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { Validator } from 'src/app/components/common';
import { DetailComponent } from 'src/app/components/common/detail.component';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';

@Component({
	selector: 'app-quantitativedashboard',
	templateUrl: './quantitativedashboard.component.html',
	styleUrls: ['./quantitativedashboard.component.scss'],
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
export class QuantitativedashboardComponent extends DetailComponent {

	componentDestroyed$: Subject<boolean> = new Subject();
	isMobile: boolean;
	isFilterOpen: boolean;

	model: QuantitativeDashboardModel;
	dataFound: QuantitativeDashBoardDataFound;
	bookingIdList: Array<number>;
	requestModel: QuantitativeDashboardRequest;
	monthYearXAxis: Array<MandayYear>;
	quantitativeDashboardLoader: QuantitativeDashBoardLoader;
	clusteredColumChart: any;
	customerInfo: CustomerInfo;
	toggleFormSection: boolean;
	quantitativeDashboardProductLength: number;
	productCategoryRequestModel: ProductCategoryRequest;
	requestCustomerModel: CustomerCommonDataSourceRequest;
	currentUser: UserModel;
	supplierTypeList: any = supplierTypeList;
	LoadFirstTime: boolean = true;
	constructor(private zone: NgZone, private service: QuantitativeDashBoardService, private calendar: NgbCalendar,
		public cusService: CustomerService, public brandService: CustomerBrandService,
		public deptService: CustomerDepartmentService, public collectionService: CustomerCollectionService,
		public buyerService: CustomerbuyerService, public supService: SupplierService, public locationService: LocationService,
		private authService: AuthenticationService, public validator: Validator,
		router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
		public dateparser: NgbDateParserFormatter, private productManagementService: ProductManagementService,
		private customerProductService: CustomerProduct) {

		super(router, route, translate, toastr);
		am4core.addLicense(amCoreLicense);
		this.currentUser = authService.getCurrentUser();
		this.getIsMobile();
	}

	ngOnDestroy(): void {
		this.componentDestroyed$.next(true);
		this.componentDestroyed$.complete();
	}

	onInit(id?: any, inputparam?: ParamMap): void {
		this.initialize();

		this.dateSelectionDefault();
		this.placeHolderLoad();

		this.validator.setJSON("dashboard/quantitative-dashboard.valid.json");
		this.validator.setModelAsync(() => this.requestModel);
		this.validator.isSubmitted = false;

		this.getCustomerListBySearch()

		this.getSupListBySearch();
		this.getCountryListBySearch();
		this.getProductCategoryData();
	}

	initialize() {
		this.model = new QuantitativeDashboardModel();
		this.productCategoryRequestModel = new ProductCategoryRequest();
		this.dataFound = new QuantitativeDashBoardDataFound();
		this.requestModel = new QuantitativeDashboardRequest();
		this.quantitativeDashboardLoader = new QuantitativeDashBoardLoader();
		this.customerInfo = new CustomerInfo();
		this.quantitativeDashboardProductLength = QuantitativeDashboardProductLength;
		this.requestCustomerModel = new CustomerCommonDataSourceRequest();
		this.model.supplierTypeId = SearchType.SupplierName;
		this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
	}

	//Added placeholder in all charts
	placeHolderLoad() {
		this.quantitativeDashboardLoader.orderQuantityLoading = true;
		this.quantitativeDashboardLoader.productCategoryLoading = true;
		this.quantitativeDashboardLoader.serviceTypeInspectionDashboardLoading = true;
		this.quantitativeDashboardLoader.serviceTypeTurnOverDashboardLoading = true;
		this.quantitativeDashboardLoader.turnOverDashboardLoading = true;
		this.quantitativeDashboardLoader.mandayCountryDashboardLoading = true;
		this.quantitativeDashboardLoader.manDayDashboardLoading = true;
	}

	dateSelectionDefault() {
		this.requestModel.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'm', 6);
		this.requestModel.serviceDateTo = this.calendar.getToday();
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

							this.model.mandayDashboard.mandayData.forEach(element => {
								element.percentagePositive = Math.abs(element.percentage);
							});

							this.monthYearXAxis = response.monthYearXAxis;
							this.mandayByYearChartFrame();
							this.dataFound.manDayDashboardDataFound = true;
							this.quantitativeDashboardLoader.mandayExportLoading = false;
						}
						else {
							this.dataFound.manDayDashboardDataFound = false;
						}
					}
					this.quantitativeDashboardLoader.manDayDashboardLoading = false;
				},
				error => {
					this.quantitativeDashboardLoader.manDayDashboardLoading = false;
					this.quantitativeDashboardLoader.manDayDashboardError = true;
				});
	}

	dateFrame(): Array<DashboardServiceDateList> {

		var yearValue = 0;
		var dateList = new Array<DashboardServiceDateList>();

		//if we have same year we are minus the from  year value to 2. becoz, want to show the 3year data
		if (this.requestModel.serviceDateTo.year == this.requestModel.serviceDateFrom.year) {
			yearValue = YearCount;
		}
		else {
			yearValue = 0;
		}

		dateList.push({
			"dateName": "FromDate",
			"date": new NgbDate(this.requestModel.serviceDateFrom.year - yearValue, this.requestModel.serviceDateFrom.month, this.requestModel.serviceDateFrom.day)
		});

		dateList.push({
			"dateName": "ToDate",
			"date": new NgbDate(this.requestModel.serviceDateTo.year, this.requestModel.serviceDateTo.month, this.requestModel.serviceDateTo.day)
		});

		return dateList;
	}

	// frame the manday year
	mandayByYearChartFrame() {
		// to build the value2 we are declare the below 2
		let k: number = 2;
		var todaydate = this.calendar.getToday();

		if (this.model.mandayDashboard.mandayData && this.model.mandayDashboard.mandayData.length > 0) {

			var dateList = this.dateFrame();
			var serviceFromDate = dateList[0].date;
			var serviceToDate = dateList[1].date;

			var chartObj = [];

			//building below structure
			//{date:new Date(2019,1), value2:48, value3:51, value4:42}
			for (var i = 0; i < this.model.mandayDashboard.mandayData.length; i++) {

				if (i == 0) {
					if (this.monthYearXAxis != null) {
						//framing x - axis
						for (var l = 0; l < this.monthYearXAxis.length; l++) {

							chartObj.push({
								date: new Date(this.monthYearXAxis[l].year, this.monthYearXAxis[l].month)
							});
						}
					}
				}

				for (var m = 0; m < chartObj.length; m++) {

					//get month data
					var monthData = this.model.mandayDashboard.mandayData[i].monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + NumberOne));

					//frame manday date
					var _mandayDate = new NgbDate(this.model.mandayDashboard.mandayData[i].year, chartObj[m].date.getMonth() + NumberOne, NumberOne);

					//manday date should come between service from and to date
					var dateBetweenRange = (_mandayDate.equals(serviceFromDate) || _mandayDate.after(serviceFromDate))
						&& (_mandayDate.equals(serviceToDate) || _mandayDate.before(serviceToDate));

					//manday date in the service date range, we are assigning value property
					//if (dateBetweenRange &&
					//	(serviceToDate.year != _mandayDate.year ||
					//	this.requestModel.serviceDateTo.year ==
					//	this.requestModel.serviceDateFrom.year)) {
					// 	&&
					// (serviceToDate.month <= todaydate.month &&
					// 	this.requestModel.serviceDateTo.year == this.requestModel.serviceDateFrom.year
					// 	&& serviceToDate.year != _mandayDate.year)

					chartObj[m]["value" + (k)] = monthData.length > 0 ? monthData[0].monthManDay : 0;
					//	}

					//execute below at the end
					if (m == (chartObj.length - NumberOne)) {

						//if we have one month manday count we should add next month manday count as 0 to show line in UI
						if (this.model.mandayDashboard.mandayData[i].monthlyData.length == 1) {

							//if dec month has, we should add nov month manday count as 0
							if (this.model.mandayDashboard.mandayData[i].monthlyData[0].month == DecMonth) {
								chartObj.filter(x => (x.date.getMonth() + NumberOne) ==
									this.model.mandayDashboard.mandayData[i].monthlyData[0].month - NumberOne)[0]["value" + (k)] = 0;
							}
							//rest month will be increate
							else {
								chartObj.filter(x => x.date.getMonth() + NumberOne ==
									this.model.mandayDashboard.mandayData[i].monthlyData[0].month + NumberOne)[0]["value" + (k)] = 0;
							}
						}
					}
				}
				k = k + 1;
			}
		}
		setTimeout(() => {
			this.renderYearLineChart(chartObj, true);
		}, 100);
	}

	/**
	* Function to render line chart for manday by year
	*/
	renderYearLineChart(chartObj, isMandayChart: boolean) {
		let chart: any;

		if (isMandayChart) {
			chart = am4core.create("chartQuandiv", am4charts.XYChart);
		}
		else {
			chart = am4core.create("stackedColumnChartQuan", am4charts.XYChart);
		}

		chart.data = chartObj;
		chart.dateFormatter.dateFormat = "dd/MM/yyyy";
		// Create axes
		let categoryAxis = chart.xAxes.push(new am4charts.DateAxis());
		categoryAxis.dataFields.date = "date";
		categoryAxis.renderer.minGridDistance = 10;
		categoryAxis.renderer.labels.template.fontSize = 12;
		categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
		categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

		categoryAxis.dateFormats.setKey("month", "MMM");
		categoryAxis.periodChangeDateFormats.setKey("month", "MMM");

		let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
		valueAxis.renderer.minGridDistance = 20;
		valueAxis.renderer.labels.template.fontSize = 12;
		valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
		valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
		if (isMandayChart) {
			this.mandayDataFrameChart(chart);
		}
		else {
			this.quantityFrameChart(chart);
		}

		chart.padding(20, 0, 20, 0);
		chart.cursor = new am4charts.XYCursor();
	}
	quantityFrameChart(chart) {
		for (var i = 0; i < this.model.bookingQuantityDashboard.length; i++) {
			// Create series
			let series = chart.series.push(new am4charts.LineSeries());
			series.legendSettings.valueText = "[bold]{valueY.close}[/]";
			series.stroke = am4core.color(this.model.bookingQuantityDashboard[i].color);
			series.dataFields.valueY = "value" + (i + 2);
			series.dataFields.dateX = "date";
			series.name = 'value' + (i + 1);
			series.strokeWidth = 2;
			series.tooltipText = this.model.bookingQuantityDashboard[i].year + ": [b]{valueY}[/]";
			series.tooltip.getFillFromObject = false;
			series.tooltip.background.fill = am4core.color(this.model.bookingQuantityDashboard[i].color);
		}
	}

	mandayDataFrameChart(chart) {
		for (var i = 0; i < this.model.mandayDashboard.mandayData.length; i++) {
			// Create series
			let series = chart.series.push(new am4charts.LineSeries());
			series.legendSettings.valueText = "[bold]{valueY.close}[/]";
			series.stroke = am4core.color(this.model.mandayDashboard.mandayData[i].color);
			series.dataFields.valueY = "value" + (i + 2);
			series.dataFields.dateX = "date";
			series.name = 'value' + (i + 1);
			series.strokeWidth = 2;
			series.tooltipText = this.model.mandayDashboard.mandayData[i].year + ": [b]{valueY}[/]";
			series.tooltip.getFillFromObject = false;
			series.tooltip.background.fill = am4core.color(this.model.mandayDashboard.mandayData[i].color);
		}
	}

	//get manday by customer export
	getMandayYearExport() {
		this.quantitativeDashboardLoader.mandayExportLoading = true;


		this.service.getMandayYearChartExport(this.requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Manday_Year_Data.xlsx");
				this.quantitativeDashboardLoader.mandayExportLoading = false;
			},
				error => {
					this.quantitativeDashboardLoader.mandayExportLoading = false;
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

	getMandayByCountrySummary(request) {
		this.service.getManDayByCountrySummary(request)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response.result == ResponseResult.Success) {

						this.model.mandayByCountryDashboard = response.data;
						this.quantitativeDashboardLoader.mandayCountryDashboardLoading = false;
						this.dataFound.manDayCountryDashboardDataFound = true;
						let colorSet = new am4core.ColorSet();
						for (var i = 0; i < this.model.mandayByCountryDashboard.length; i++) {

							this.model.mandayByCountryDashboard[i].percentagePositive = Math.abs(this.model.mandayByCountryDashboard[i].percentage);

							var _sublength = CusDashboardProductLength < i ? 20 : 9;
							this.model.mandayByCountryDashboard[i].trimmedName =

								this.model.mandayByCountryDashboard[i].name &&
									this.model.mandayByCountryDashboard[i].name.length > 11 ?
									this.model.mandayByCountryDashboard[i].name.substring(0, _sublength) + ".." :
									this.model.mandayByCountryDashboard[i].name;
							this.model.mandayByCountryDashboard[i].color = colorSet.next();
						}

						this.quantitativeDashboardLoader.manDayCountryChartExportLoading = false;
						setTimeout(() => {
							this.mandayByCountryBarChart();
						}, 10);
					}
					else {
						this.quantitativeDashboardLoader.mandayCountryDashboardLoading = false;
						this.dataFound.manDayCountryDashboardDataFound = false;
					}
				},
				error => {
					this.quantitativeDashboardLoader.mandayCountryDashboardLoading = false;
					this.quantitativeDashboardLoader.manDayCountryChartError = true;
				});
	}

	createSeries(chart, color, stacked) {

		var graphwidth = this.model.mandayByCountryDashboard.length >= 5 ? 85 : this.model.mandayByCountryDashboard.length >= 3 ? 40 : 10;

		var series = chart.series.push(new am4charts.ColumnSeries());
		series.dataFields.valueY = "visits";
		series.dataFields.categoryX = "country";
		series.name = "Visits";
		series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
		series.columns.template.fillOpacity = 1;
		//series.stacked = stacked;
		series.columns.template.width = am4core.percent(graphwidth);
		series.columns.template.fill = am4core.color(color);
	}

	mandayByCountryBarChart() {

		var chart = am4core.create("piechartQuandiv", am4charts.XYChart);
		if (this.model.mandayByCountryDashboard && this.model.mandayByCountryDashboard.length > 0) {

			var chartObj = [];

			for (var i = 0; i < this.model.mandayByCountryDashboard.length; i++) {
				chartObj.push({
					"country": this.model.mandayByCountryDashboard[i].name,
					"visits": this.model.mandayByCountryDashboard[i].count,
					"color": this.model.mandayByCountryDashboard[i].color,
					"none": 0
				});
			}
		}

		chart.data = chartObj;

		// Create axes

		var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
		categoryAxis.dataFields.category = "country";
		categoryAxis.renderer.grid.template.location = 0;
		categoryAxis.renderer.minGridDistance = 20;
		categoryAxis.renderer.labels.template.fontSize = 12;
		categoryAxis.renderer.cellStartLocation = 0.1;
		categoryAxis.renderer.cellEndLocation = 0.9;
		categoryAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
		categoryAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");

		var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
		valueAxis.min = 0;
		valueAxis.renderer.labels.template.fontSize = 12;

		var graphwidth = this.model.mandayByCountryDashboard.length >= 5 ? 85 : this.model.mandayByCountryDashboard.length >= 3 ? 40 : 10;

		var series = chart.series.push(new am4charts.ColumnSeries());
		series.dataFields.valueY = "visits";
		series.dataFields.categoryX = "country";
		series.name = "Visits";
		series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
		series.columns.template.fillOpacity = 1;
		//series.stacked = stacked;
		series.columns.template.width = am4core.percent(graphwidth);
		valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
		valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
		//series.columns.template.fill = am4core.color("#e8bde7");

		let labelBullet = series.bullets.push(new am4charts.LabelBullet());
		labelBullet.label.verticalCenter = "bottom";
		labelBullet.label.dy = -10;
		labelBullet.label.text = "{values.valueY.workingValue}";
		labelBullet.label.fontSize = 12;

		// for (var i = 0; i < this.model.mandayByCountryDashboard.length; i++) {
		// 	let stacked = false;
		// 	//create series
		// 	this.createSeries(chart, this.model.mandayByCountryDashboard[i].color, stacked);
		//   }
	}

	//get manday by customer export
	getMandayCountryChartExport() {
		this.quantitativeDashboardLoader.manDayCountryChartExportLoading = true;


		this.service.getMandayCountryChartExport(this.requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Manday_Country_Data.xlsx");
				this.quantitativeDashboardLoader.manDayCountryChartExportLoading = false;
			},
				error => {
					this.quantitativeDashboardLoader.manDayCountryChartExportLoading = false;
				});
	}

	getTurnOverSummary(request) {
		this.service.getTurnOverSummary(request)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response.result == ResponseResult.Success) {
						this.model.turnOverDashboard = response.data.turnOverDataItem;

						this.model.turnOverDashboard.customerTurnOverPercentagePositive =
							Math.abs(this.model.turnOverDashboard.customerTurnOverPercentage);

						this.model.turnOverDashboard.supplierTurnOverPercentagePositive =
							Math.abs(this.model.turnOverDashboard.supplierTurnOverPercentage);

						this.model.turnOverDashboard.totalTurnOverPercentagePositive =
							Math.abs(this.model.turnOverDashboard.totalTurnOverPercentage);

						this.model.serviceTypeDashboard = response.data.serviceTypeChartData;

						this.getServiceTypeTurnOverDashboardSummary();
						this.quantitativeDashboardLoader.turnOverDashboardLoading = false;
						this.dataFound.turnOverDashboardDataFound = true;
					}
					else {
						this.quantitativeDashboardLoader.turnOverDashboardLoading = false;
						this.dataFound.turnOverDashboardDataFound = false;
					}
				},
				error => {
					this.quantitativeDashboardLoader.turnOverDashboardLoading = false;
					this.quantitativeDashboardLoader.turnOverDashboardError = true;
				});
	}

	getServiceTypeTurnOverDashboardSummary() {

		if (this.model.serviceTypeDashboard && this.model.serviceTypeDashboard.length > 0) {
			let colorSet = new am4core.ColorSet();
			for (var i = 0; i < this.model.serviceTypeDashboard.length; i++) {

				this.model.serviceTypeDashboard[i].percentagePositive = this.model.serviceTypeDashboard[i].percentage;
				var _sublength = 23;
				this.model.serviceTypeDashboard[i].trimmedName =

					this.model.serviceTypeDashboard[i].name &&
						this.model.serviceTypeDashboard[i].name.length > _sublength ?
						this.model.serviceTypeDashboard[i].name.substring(0, _sublength) + ".." :
						this.model.serviceTypeDashboard[i].name;
				this.model.serviceTypeDashboard[i].color = colorSet.next();
			}
			setTimeout(() => {
				this.renderPieChart('variablePieChart2Quan', this.model.serviceTypeDashboard, false);
			}, 10);
			// this.quantitativeDashboardLoader.serviceTypeTurnOverDashboardLoading = false;
			// this.dataFound.serviceTypeTurnOverDashboardDataFound = true;
			// this.quantitativeDashboardLoader.serviceTypeTurnOverDashboardExportLoading = false;
		}
		// else {
		// 	this.quantitativeDashboardLoader.serviceTypeTurnOverDashboardLoading = false;
		// 	this.dataFound.serviceTypeTurnOverDashboardDataFound = false;
		// }
	}

	renderPieChart(container, data, isTotalShown: boolean) {

		// Create chart instance
		var chart = am4core.create(container, am4charts.PieChart);

		var chartObj = [];
		let totalCount = 0;

		if (data && data.length > 0) {

			for (var i = 0; i < data.length; i++) {
				if (isTotalShown) {
					totalCount += data[i].count;
				}

				chartObj.push({
					"sector": data[i].name,
					"size": data[i].count,
					"color": data[i].color
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

		if (isTotalShown) {
			let label = pieSeries.createChild(am4core.Label);
			label.text = totalCount.toString();
			label.fontSize = 12;
			label.verticalCenter = "middle";
			label.horizontalCenter = "middle";
			label.fontFamily = "roboto-medium";
		}
	}

	//get turnover by service type
	getTurnOverByServiceTypeChartExport() {
		this.quantitativeDashboardLoader.serviceTypeTurnOverDashboardExportLoading = true;


		this.service.getTurnOverByServicetypeChartExport(this.requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"TurnOver_By_ServiceType_Data.xlsx");
				this.quantitativeDashboardLoader.serviceTypeTurnOverDashboardExportLoading = false;
			},
				error => {
					this.quantitativeDashboardLoader.serviceTypeTurnOverDashboardExportLoading = false;
				});
	}

	getInspectionServiceTypeDashboardSummary(request) {

		this.service.getInspectionServiceTypeDashboardSummary(request)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response.result == ResponseResult.Success) {
						let length = response.data.length;// < CusDashboardProductLength ? response.data.length : CusDashboardProductLength;
						let colorSet = new am4core.ColorSet();
						for (var i = 0; i < length; i++) {
							var _sublength = 30;
							response.data[i].trimmedName =

								response.data[i].name &&
									response.data[i].name.length > _sublength ?
									response.data[i].name.substring(0, _sublength) + ".." :
									response.data[i].name;
							response.data[i].statusColor = colorSet.next();
						}
						this.model.inspectionServiceTypeDashboard = response.data;

						setTimeout(() => {
							this.renderPieChart('variablePieChartQuan', this.model.inspectionServiceTypeDashboard, true);
						}, 10);
						this.dataFound.serviceTypeInspectionDashboardDataFound = true;
						this.quantitativeDashboardLoader.serviceTypeInspectionDashboardLoading = false;
						this.quantitativeDashboardLoader.serviceTypeInspectionDashboardExportLoading = false;
					}
					else {
						this.dataFound.serviceTypeInspectionDashboardDataFound = false;
						this.quantitativeDashboardLoader.serviceTypeInspectionDashboardLoading = false;
					}
				},
				error => {
					this.quantitativeDashboardLoader.serviceTypeInspectionDashboardLoading = false;
					this.quantitativeDashboardLoader.serviceTypeInspectionDashboardError = true;
				});

	}

	//get inspection by service type
	getInspectionyServiceTypeChartExport() {
		this.quantitativeDashboardLoader.serviceTypeInspectionDashboardExportLoading = true;


		this.service.getInspectionByServicetypeChartExport(this.requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Inspection_By_ServiceType_Data.xlsx");
				this.quantitativeDashboardLoader.serviceTypeInspectionDashboardExportLoading = false;
			},
				error => {
					this.quantitativeDashboardLoader.serviceTypeInspectionDashboardExportLoading = false;
				});
	}



	bookingQuantityChartFrame() {
		// to build the value2 we are declare the below 2
		let k: number = 2;
		if (this.model.bookingQuantityDashboard && this.model.bookingQuantityDashboard.length > 0) {

			var dateList = this.dateFrame();
			var serviceFromDate = dateList[0].date;
			var serviceToDate = dateList[1].date;

			var chartObj = [];

			//building below structure
			//{date:new Date(2019,1), value2:48, value3:51, value4:42}
			for (var i = 0; i < this.model.bookingQuantityDashboard.length; i++) {

				if (i == 0) {
					if (this.model.orderQuantityXAxis != null) {
						//framing x - axis
						for (var l = 0; l < this.model.orderQuantityXAxis.length; l++) {

							chartObj.push({
								date: new Date(this.model.orderQuantityXAxis[l].year, this.model.orderQuantityXAxis[l].month)
							});
						}
					}
				}

				for (var m = 0; m < chartObj.length; m++) {

					//get month data
					var monthData = this.model.bookingQuantityDashboard[i].monthlyData.filter(x => x.month == (chartObj[m].date.getMonth() + NumberOne));

					//frame quantity date
					var _mandayDate = new NgbDate(this.model.bookingQuantityDashboard[i].year, chartObj[m].date.getMonth() + NumberOne, NumberOne);

					//quantity date should come between service from and to date
					var dateBetweenRange = (_mandayDate.equals(serviceFromDate) || _mandayDate.after(serviceFromDate))
						&& (_mandayDate.equals(serviceToDate) || _mandayDate.before(serviceToDate));

					//quantity date in the service date range, we are assigning value property
					//	if (dateBetweenRange && (serviceToDate.year != _mandayDate.year ||
					//	this.requestModel.serviceDateTo.year ==
					//	this.requestModel.serviceDateFrom.year)) {
					chartObj[m]["value" + (k)] = monthData.length > 0 ? monthData[0].monthOrderQuantity : 0;
					//	}
					//execute below at the end
					if (m == (chartObj.length - NumberOne)) {

						//if we have one month quantity count we should add next month quantity count as 0 to show line in UI
						if (this.model.bookingQuantityDashboard[i].monthlyData.length == 1) {

							//if dec month has, we should add nov month manday count as 0
							if (this.model.bookingQuantityDashboard[i].monthlyData[0].month == DecMonth) {
								chartObj.filter(x => (x.date.getMonth() + NumberOne) ==
									this.model.bookingQuantityDashboard[i].monthlyData[0].month - NumberOne)[0]["value" + (k)] = 0;
							}
							//rest month will be increate
							else {
								chartObj.filter(x => x.date.getMonth() + NumberOne ==
									this.model.bookingQuantityDashboard[i].monthlyData[0].month + NumberOne)[0]["value" + (k)] = 0;
							}
						}
					}
				}
				k = k + 1;
			}
		}
		setTimeout(() => {
			this.renderYearLineChart(chartObj, false);
		}, 100);

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

	productCategoryFrame() {
		var chartObj = [];
		if (this.model.prodCategoryChart && this.model.prodCategoryChart.length > 0) {

			for (var i = 0; i < this.model.prodCategoryChart.length; i++) {
				chartObj.push({
					"sector": this.model.prodCategoryChart[i].statusName,
					"size": this.model.prodCategoryChart[i].totalCount,
					"color": am4core.color(this.model.prodCategoryChart[i].statusColor)
				});
			}
		}
		return chartObj;
	}
	renderDonutChart(container) {
		// Create chart instance
		var chart = am4core.create(container, am4charts.PieChart);

		chart.data = this.productCategoryFrame();

		// Add and configure Series
		var pieSeries = chart.series.push(new am4charts.PieSeries());
		pieSeries.dataFields.value = "size";
		pieSeries.dataFields.category = "sector";
		pieSeries.labels.template.disabled = true;
		pieSeries.slices.template.propertyFields.fill = "color";

		pieSeries.slices.template.stroke = am4core.color("#ffffff");
		pieSeries.slices.template.strokeWidth = 1;
		pieSeries.slices.template.strokeOpacity = 1;

		pieSeries.tooltip.autoTextColor = false;
		pieSeries.tooltip.label.fill = am4core.color("#FFFFFF");
	}

	getCustomerListBySearch() {

		this.model.customerInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.model.customerLoading = true),
			switchMap(term =>
				this.cusService.getCustomerListByUserType(this.requestCustomerModel, this.currentUser.usertype, term)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.model.customerLoading = false))
			))
			.subscribe(data => {
				this.getCustomerRelatedDetails(data);
				this.model.customerLoading = false;
			});
	}

	getCustomerRelatedDetails(data) {
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
			this.model.supsearchRequest.customerId = this.requestModel.customerId;
			this.getSupListBySearch();
			this.getBrandListBySearch();
			this.getBuyerListBySearch();
			this.getCollectionListBySearch();
			this.getDeptListBySearch();
			this.getProductListBySearch();
			this.charLoadData();
			this.LoadFirstTime = false;
		}
		this.model.customerLoading = false;
	}

	//fetch the customer data with virtual scroll
	getCustomerData(IsVirtual: boolean) {
		if (IsVirtual) {
			this.requestCustomerModel.searchText = this.model.customerInput.getValue();
			this.requestCustomerModel.skip = this.model.customerList.length;
		}

		this.model.customerLoading = true;
		this.cusService.getCustomerListByUserType(this.requestCustomerModel, this.currentUser.usertype).
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
			this.requestModel.supplierId = null;
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
		}
	}

	//fetch the first 10 suppliers for the customer on load
	getSupListBySearch() {
		this.model.supsearchRequest.customerId = this.requestModel.customerId;
		this.model.supsearchRequest.supplierType = SupplierType.Supplier;
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
			});
	}

	//fetch the supplier data with virtual scroll
	getSupplierData() {
		this.model.supsearchRequest.searchText = this.model.supInput.getValue();
		this.model.supsearchRequest.skip = this.model.supplierList.length;

		this.model.supsearchRequest.customerId = this.requestModel.customerId;
		this.model.supsearchRequest.supplierType = SupplierType.Supplier;
		this.model.supLoading = true;
		this.supService.getFactoryDataSourceList(this.model.supsearchRequest)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(customerData => {
				if (customerData && customerData.length > 0) {
					this.model.supplierList = this.model.supplierList.concat(customerData);
				}
				this.model.supsearchRequest.skip = 0;
				this.model.supsearchRequest.take = ListSize;
				this.model.supLoading = false;
			}),
			error => {
				this.model.supLoading = false;
			};
	}

	toggleSection() {
		this.toggleFormSection = !this.toggleFormSection;
	}

	//fetch the brand data with virtual scroll
	getBrandData() {
		this.model.brandSearchRequest.searchText = this.model.brandInput.getValue();
		this.model.brandSearchRequest.skip = this.model.brandList.length;

		this.model.brandLoading = true;
		this.brandService.getBrandListByCustomerId(this.model.brandSearchRequest)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(brandData => {
				if (brandData && brandData.length > 0) {
					this.model.brandList = this.model.brandList.concat(brandData);
				}
				this.model.brandSearchRequest = new CommonCustomerSourceRequest();
				this.model.brandLoading = false;
			}),
			error => {
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
			error => {
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
		this.buyerService.getBuyerListByCustomerId(this.model.buyerSearchRequest)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(buyerData => {
				if (buyerData && buyerData.length > 0) {
					this.model.buyerList = this.model.buyerList.concat(buyerData);
				}
				this.model.buyerSearchRequest = new CommonCustomerSourceRequest();
				this.model.buyerLoading = false;
			}),
			error => {
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
		this.collectionService.getCollectionListByCustomerId(this.model.collectionSearchRequest)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(collectionData => {
				if (collectionData && collectionData.length > 0) {
					this.model.collectionList = this.model.collectionList.concat(collectionData);
				}
				this.model.collectionSearchRequest = new CommonCustomerSourceRequest();
				this.model.collectionLoading = false;
			}),
			error => {
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

	//fetch the country data with virtual scroll
	getCountryData() {
		this.model.countryRequest.searchText = this.model.countryInput.getValue();
		this.model.countryRequest.skip = this.model.countryList.length;

		this.model.countryLoading = true;
		this.locationService.getCountryDataSourceList(this.model.countryRequest)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(customerData => {
				if (customerData && customerData.length > 0) {
					this.model.countryList = this.model.countryList.concat(customerData);
				}
				this.model.countryRequest = new CountryDataSourceRequest();
				this.model.countryLoading = false;
			}),
			error => {
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

	getViewPath(): string {
		return "";
	}
	getEditPath(): string {
		return "";
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

		if (this.isFormValid()) {

			this.placeHolderLoad();
			this.charLoadData();
		}
	}

	async charLoadData() {

		this.getMandaysByYear(this.requestModel);
		this.getMandayByCountrySummary(this.requestModel);
		this.getTurnOverSummary(this.requestModel);
		this.getOrderQuantityList();

		var response = await this.service.getDashboardSummary(this.requestModel)
		this.model.quantitativeDashboardCount = response.data;
		this.bookingIdList = response.inspectionIdList;
		this.dataFound.quantitativeDashboardCountDataFound = this.model.quantitativeDashboardCount ? true : false;

		this.getProductCategoryCountList();
		this.getInspectionServiceTypeDashboardSummary(this.requestModel);
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
					var customerDetails = this.model.customerList.find(x => x.id == this.requestModel.customerId);
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

	//country change event
	countryChange(countryItem) {

		if (countryItem) {

			if (this.requestModel.selectedCountryIdList && this.requestModel.selectedCountryIdList.length > 0) {
				this.model.countryListName = "";

				var customerLength = this.requestModel.selectedCountryIdList.length < CountrySelectedFilterTextCount ?
					this.requestModel.selectedCountryIdList.length : CountrySelectedFilterTextCount;
				for (var i = 0; i < customerLength; i++) {

					var countryDetails = this.model.countryList.find(x => x.id == this.requestModel.selectedCountryIdList[i]);

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

	//supplier change event
	supplierChange(supplierItem) {
		if (supplierItem && supplierItem.id > 0) {
			var supplierDetails = this.model.supplierList.find(x => x.id == supplierItem.id);
			if (supplierDetails)
				this.model.supplierName =
					supplierDetails.name.length > SupplierNameTrim ?
						supplierDetails.name.substring(0, SupplierNameTrim) + "..." : supplierDetails.name;
		}
		else {
			this.model.supplierName = "";
		}
	}

	getOrderQuantityList() {
		this.service.getOrderQuantityList(this.requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response.result == ResponseResult.Success) {
						this.model.bookingQuantityDashboard = response.data;
						this.model.orderQuantityXAxis = response.monthYearXAxis;
						this.bookingQuantityChartFrame();
						this.dataFound.orderQuantityFound = true;
					}
					else {
						this.dataFound.orderQuantityFound = false;
					}
					this.quantitativeDashboardLoader.orderQuantityLoading = false;
					this.model.searchLoading = false;
				},
				error => {
					this.quantitativeDashboardLoader.orderQuantityLoading = false;
					this.quantitativeDashboardLoader.orderQuantityError = true;
					this.model.searchLoading = false;
				});
	}

	//export the country based quantity details
	getOrderQuantityListExport() {
		this.quantitativeDashboardLoader.orderQuantityExportLoading = true;
		this.service.getOrderQuantityListExport(this.requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Order_Quantity_Details.xlsx");
				this.quantitativeDashboardLoader.orderQuantityExportLoading = false;
			},
				error => {
					this.quantitativeDashboardLoader.orderQuantityExportLoading = false;
				});
	}

	getProductCategoryCountList() {
		this.productCategoryRequestModel.searchRequest = this.requestModel;
		this.service.getProductCategoryCountList(this.productCategoryRequestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response.result == ResponseResult.Success) {
						let colorSet = new am4core.ColorSet();
						for (var i = 0; i < response.data.length; i++) {
							response.data[i].name =
								response.data[i].statusName &&
									response.data[i].statusName.length > 35 ?
									response.data[i].statusName.substring(0, 33) + "..." :
									response.data[i].statusName;
							response.data[i].statusColor = colorSet.next();
						}

						this.model.prodCategoryChart = response.data;

						setTimeout(() => {
							this.renderDonutChart("sunburstchartQuan");
						}, 10);
						this.dataFound.productCategoryFound = true;
					}
					else {
						this.dataFound.productCategoryFound = false;
					}
					this.quantitativeDashboardLoader.productCategoryLoading = false;
				},
				error => {
					this.quantitativeDashboardLoader.productCategoryLoading = false;
					this.quantitativeDashboardLoader.productCategoryError = true;
				});
	}

	//export the country based quantity details
	getProductCategoryCountListExport() {
		this.requestModel.productCategoryId = this.productCategoryRequestModel.productCategoryId;
		this.quantitativeDashboardLoader.productCategoryExportLoading = true;
		this.service.getProductCategoryCountListExport(this.requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(async res => {
				await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"Product_category_count_Details.xlsx");
				this.quantitativeDashboardLoader.productCategoryExportLoading = false;
			},
				error => {
					this.quantitativeDashboardLoader.productCategoryExportLoading = false;
				});
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

	reset() {
		this.initialize();

		this.dateSelectionDefault();
		this.placeHolderLoad();

		this.toggleFilter();

		this.getCustomerListBySearch();
		this.getSupListBySearch();
		this.getCountryListBySearch();
		this.getProductCategoryData();

	}

	changeProductCategory(item) {
		this.productCategoryRequestModel.searchRequest = this.requestModel;
		if (item)
			this.productCategoryRequestModel.productCategoryId = item.id;

		this.quantitativeDashboardLoader.productCategoryLoading = true;
		this.getProductCategoryCountList();
	}

	//fetch the product category data with virtual scroll
	getProductCategoryData() {
		this.model.productCategoryListLoading = true;
		this.service.getProductCategorySummary()
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

	clearProductRef() {
		this.getProductListBySearch();
	}

	changeProductRef() {
		this.getProductListBySearch();
	}

	changeSupplierType(item) {
		this.model.supLoading = true;
		this.model.supplierList = [];
		this.requestModel.supplierId = null;
		if (item.id == SearchType.SupplierCode) {
			this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierCode;
		}
		else if (item.id == SearchType.SupplierName) {
			this.model.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
		}
		this.getSupplierData();
	}
}

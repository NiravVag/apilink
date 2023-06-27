import { Component, NgZone } from '@angular/core';
import { NgbModal, NgbDateParserFormatter, NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { MandayDashboardRequest, ManDayDashboard, MandayDashboardResponse, MandayDashboardItem, MandayDashboardResult, MandayYearChartResponse, MandayYearChart, MandayYear, MandayTerm, MandayCustomerChartResponse, MandayCustomerChart, MandayCountryChartResponse, MandayCountryChart, MandayEmployeeTypeChartResponse, MandayEmployeeTypeChart, FilterModel } from 'src/app/_Models/statistics/manday-dashboard.model';
import { DetailComponent } from '../../common/detail.component';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { APIService, EntityAccess, MandayDropDown, MandayType } from '../../common/static-data-common';
import { of, concat, Subject } from 'rxjs';
import { first, switchMap, distinctUntilChanged, tap, catchError, timestamp, map, takeUntil } from 'rxjs/operators';
import { ResponseResult, DataSource } from 'src/app/_Models/common/common.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { Validator } from '../../common/validator';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

am4core.useTheme(am4themes_animated);

@Component({
	selector: 'app-manday-dashboard',
	templateUrl: './manday-dashboard.component.html',
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
export class MandayDashboardComponent extends DetailComponent {

	componentDestroyed$: Subject<boolean> = new Subject();
	modalRef: any;
	radarChart: any;
	_apiService = APIService;
	model: MandayDashboardRequest;
	masterModel: ManDayDashboard;
	mandayCountByService: MandayDashboardItem;
	mandayYearChart: Array<MandayYearChart>;
	mandayCustomerChart: Array<MandayCustomerChart>;
	monthYearXAxis: Array<MandayYear>;
	mandayEmployeeTypeChart: Array<MandayEmployeeTypeChart>;
	mandayEmployeeTypeXAxis: Array<MandayYear>;
	filterModel: FilterModel;
	pieChart: any;
	clusteredColumChart: any;
	private chart: am4charts.XYChart;
	filterOpen: boolean;
	selectedCustomer: DataSource[]
	selectedCountry: DataSource[];
	mandayCountryChartResponse: MandayCountryChartResponse;
	constructor(router: Router, route: ActivatedRoute, public validator: Validator, translate: TranslateService, toastr: ToastrService,
		public dateparser: NgbDateParserFormatter, public utility: UtilityService,
		private mandayDashboardService: MandayDashboardService, public calendar: NgbCalendar, public modalService: NgbModal,
		public locationService: LocationService) {
		super(router, route, translate, toastr, modalService, utility);
		am4core.addLicense("CH238479116");
		this.filterOpen = false;
	}

	ngOnDestroy(): void {
		this.componentDestroyed$.next(true);
		this.componentDestroyed$.complete();
	}

	onInit() {
		this.model = new MandayDashboardRequest();
		this.masterModel = new ManDayDashboard();
		this.mandayCountByService = new MandayDashboardItem();
		this.mandayYearChart = new Array<MandayYearChart>();
		this.monthYearXAxis = new Array<MandayYear>();
		this.mandayCustomerChart = new Array<MandayCustomerChart>();
		this.mandayCountryChartResponse = new MandayCountryChartResponse();		
		this.mandayEmployeeTypeChart = new Array<MandayEmployeeTypeChart>();
		this.mandayEmployeeTypeXAxis = new Array<MandayYear>();
		this.filterModel = new FilterModel();
		this.masterModel.mandayYearLoading = true;
		this.masterModel.mandayCountryLoading = true;
		this.masterModel.mandayCustomerLoading = true;
		this.masterModel.mandayEmployeeTypeLoading = true;

		this.masterModel.mandayYearChartLoading = true;
		this.model.countryIdList = [];
		this.filterModel.OtherFilter = [];

		this.model.serviceId = APIService.Inspection;
		this.masterModel.compareBy = 1;

		const entityId = this.utility.getEntityId();
		if (Number(entityId) == EntityAccess.API) {
			this.model.mandayType = MandayType.EstimatedManDay;
		}
		else {
			this.model.mandayType = MandayType.ActualManDay;
		}

		this.model.isCompareData = false;

		this.validator.isSubmitted = false;
		this.validator.setJSON("statistics/manday-dashboard/manday-dashboard.valid.json");
		this.validator.setModelAsync(() => this.model);

		this.getMandayTermList();
		this.getOfficeList();
		this.getServiceList();
		this.getCountryList();

		this.getCustomerList();
		this.getCountryListMandayByYear();
		this.getCountryListMandayByCustomer();
		this.getCountryListMandayByCountry();

		this.getCustomerListMandayByYear();
		this.getCountryListMandayByEmployeeType();

		this.getMandayCountByService();

		this.getMandaysByYear(true);
		this.getMandayByCustomerChart(true);
		this.getMandayByCountryChart(true);
		this.getMandaysByEmployeeTypeChart(true);
	}

	setFilerModel() {
		let _otherfiler = [];
		this.filterModel.fromDate = this.model.serviceDateFrom.day + "/" + this.model.serviceDateFrom.month + "/" + this.model.serviceDateFrom.year;
		this.filterModel.toDate = this.model.serviceDateTo.day + "/" + this.model.serviceDateTo.month + "/" + this.model.serviceDateTo.year;
		if (this.model.isCompareData) {
			this.filterModel.comparedFrom = this.model.comparedServiceDateFrom.day + "/" + this.model.comparedServiceDateFrom.month + "/" + this.model.comparedServiceDateFrom.year;
			this.filterModel.comparedTo = this.model.comparedServiceDateTo.day + "/" + this.model.comparedServiceDateTo.month + "/" + this.model.comparedServiceDateTo.year;
		}
		if (this.model.officeIdList && this.model.officeIdList.length > 0) {
			let _Office = "";
			if (this.model.officeIdList.length == 1) {
				_Office = this.masterModel.officeList.filter(
					item => item.id === this.model.officeIdList[0])[0].name;
			} else {
				_Office = "" + this.model.officeIdList.length + " Selected"
			}
			_otherfiler.push("Office :" + _Office);
		}

		if (this.masterModel.serviceList) {
			let _service = this.masterModel.serviceList.filter(
				item => item.id === this.model.serviceId)[0].name;

			_otherfiler.push("Service Type :" + _service);
		}

		// ------------Manday Type-----------
		if (this.masterModel.mandayTypeList) {
			let _mandayType = this.masterModel.mandayTypeList.find(item => item.id === this.model.mandayType)?.name;

			_otherfiler.push("Manday Type: " + _mandayType);
		}

		if (this.model.countryIdList && this.model.countryIdList.length > 0) {
			let _Country = "";
			if (this.model.countryIdList.length == 1) {
				_Country = this.selectedCountry[0].name;
			} else {
				_Country = "" + this.model.countryIdList.length + " Selected"
			}
			_otherfiler.push("Country :" + _Country);
		}

		if (this.model.customerIdList && this.model.customerIdList.length > 0) {
			let _customer = "";

			if (this.model.customerIdList.length == 1) {
				_customer = this.selectedCustomer[0].name;
			} else {
				_customer = "" + this.model.customerIdList.length + " Selected"
			}
			_otherfiler.push("Customer :" + _customer);
		}

		this.filterModel.OtherFilter = _otherfiler;
	}

	getViewPath() {
		return "";
	}
	getEditPath() {
		return "";
	}
	changeCustomer(selectedValues) {
		this.selectedCustomer = selectedValues;

	}
	changetoAbs(percentage: any) {
		if (percentage)
			return Math.abs(percentage)
		else percentage
	}
	changeCountry(selectedValues) {
		this.selectedCountry = selectedValues;

	}

	//customer change for manday year chart
	mandayYearSubCustomerChange(customerData) {
		// this.masterModel.mandayYearNoDataFound = false;
		// if (customerData && customerData.id) {
		// 	this.model.mandayYearSubCustomerId = customerData.id;
		// }

		this.getMandaysByYear(false);
	}

	//country change in top filter
	// countryChange(countryData) {

	// 	if (countryData && countryData.id) {
	// 		this.model.countryIdList = [countryData.id];
	// 	}
	// }

	//country change for manday year chart
	mandayYearSubCountryChange(countryData) {
		// this.masterModel.mandayYearNoDataFound = false;
		// if (countryData && countryData.id) {
		// 	this.model.mandayYearSubCountryId = countryData.id;
		// }
		this.getMandaysByYear(false);
	}

	//country change for manday customer chart
	mandayCustomerSubCountryChange(countryData) {
		// if (countryData && countryData.id) {
		// 	this.model.mandayCustomerSubCountryId = countryData.id;
		// }
		this.getMandayByCustomerChart(false);
	}

	// country change for manday country chart
	mandayCountrySubCountryChange(countryData) {
		this.model.mandayCountrySubProvinceId = null;
		this.masterModel.mandayCountrySubProvinceList = [];


		if (countryData && countryData.id) {

			if (this.model.countryIdList && this.model.countryIdList.length > 0 &&
				this.model.countryIdList.indexOf(countryData.id) >= 0) {
				this.masterModel.isMandayCountrySubProvinceShow = true;
				// this.model.mandayCountrySubCountryId = countryData.id;
				this.getProvinceList(countryData.id);
			}

			else if (this.model.countryIdList && this.model.countryIdList.length == 0) {
				this.masterModel.isMandayCountrySubProvinceShow = true;
				// this.model.mandayCountrySubCountryId = countryData.id;
				this.getProvinceList(countryData.id);
			}
			else {
				this.model.mandayCountrySubCountryId = null;
				this.masterModel.isMandayCountrySubProvinceShow = false;
			}
		}
		else {
			this.masterModel.isMandayCountrySubProvinceShow = false;
		}

		this.getMandayByCountryChart(false);
	}

	//province change for manday country chart
	mandayCountrySubProvinceChange(provinceData) {
		// if (provinceData && provinceData.id) {
		// 	this.model.mandayCountrySubProvinceId = provinceData.id;
		// }
		this.getMandayByCountryChart(false);
	}

	//customer change for employee type chart
	mandayEmployeeTypeSubCustomerChange(customerData) {
		// if (customerData && customerData.id) {
		// 	this.model.mandayEmployeeTypeSubCustomerId = customerData.id;
		// }
		this.getMandaysByEmployeeTypeChart(false);
	}

	//year change for employee type chart
	mandayEmployeeTypeSubYearChange(year) {
		// this.model.mandayEmployeeTypeSubYear = year;
		this.getMandaysByEmployeeTypeChart(false);
	}

	//date control from and to vaild to search
	isFormValid() {
		return this.validator.isValid('serviceDateFrom') &&
			this.validator.isValid('serviceDateTo');
	}
	isComparedateValid() {
		if (this.model.isCompareData) {
			if (this.model.comparedServiceDateFrom == null) {
				this.showWarning('MANDAY_DASHBOARD.LBL_TITLE', 'MANDAY_DASHBOARD.MSG_COMPARE_FROM_DATE');
				return false;
			}
			else if (this.model.comparedServiceDateTo == null) {
				this.showWarning('MANDAY_DASHBOARD.LBL_TITLE', 'MANDAY_DASHBOARD.MSG_COMPARE_TO_DATE');
				return false;
			}
		}
		else {
			return true;
		}
		return true;
	}
	isfilterData(event) {
		if (event) {
			this.model.comparedServiceDateFrom = null;
			this.model.comparedServiceDateTo = null;
		}
	}
	//search the all charts include the count details
	search() {

		this.resetChartFilter();
		if (this.model.serviceId == APIService.Inspection) {
			this.masterModel.isAuditShow = false;
		}
		else if (this.model.serviceId == APIService.Audit) {
			this.masterModel.isAuditShow = true;
		}

		this.validator.initTost();
		this.validator.isSubmitted = true;

		if (this.isFormValid() && this.isComparedateValid()) {

			if (this.masterModel.mandayEmployeeTypeSubYearList &&
				this.masterModel.mandayEmployeeTypeSubYearList.length > 1) {
				this.model.mandayEmployeeTypeSubYear = this.masterModel.mandayEmployeeTypeSubYearList[0].id;
			}

			this.changeServiceId();
			this.getMandayCountByService();
			this.getMandaysByYear(true);
			this.getMandayByCustomerChart(true);
			this.getMandayByCountryChart(true);
			this.getMandaysByEmployeeTypeChart(true);
			this.setFilerModel()
		}
	}

	//reset the page
	reset() {
		this.onInit();
	}

	//change mandayTerm
	changeMandayTerm(event) {

		if (event.id == MandayTerm.Week) {
			this.masterModel.isDateShow = false;
			this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 7);
			this.model.serviceDateTo = this.calendar.getToday();
			this.mandayTermDateChange()
		}
		else if (event.id == MandayTerm.Month) {
			this.masterModel.isDateShow = false;
			this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 30);
			this.model.serviceDateTo = this.calendar.getToday();
			this.mandayTermDateChange()
		}
		else if (event.id == MandayTerm.Year) {
			this.masterModel.isDateShow = false;
			this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 365);
			this.model.serviceDateTo = this.calendar.getToday();
			this.mandayTermDateChange()
		}
		else if (event.id == MandayTerm.Custom) {
			this.masterModel.isDateShow = true;
			this.model.serviceDateFrom = null;
			this.model.serviceDateTo = null;
			this.masterModel.isMandayTermShow = true;
		}

		if (event.id != MandayTerm.Custom) {
			//this.changeCompareBy();
		}
	}

	mandayTermDateChange() {

		if (this.model.serviceDateFrom != null && this.model.serviceDateTo != null) {


			this.masterModel.mandayEmployeeTypeSubYearList = [];

			if (this.model.serviceDateFrom.year < this.model.serviceDateTo.year) {
				for (var i = this.model.serviceDateFrom.year; i <= this.model.serviceDateTo.year; i++) {

					this.masterModel.mandayEmployeeTypeSubYearList.push(
						{
							id: i, name: "" + i
						}
					);
				}
			}

		}
	}

	//if service date range greater than 1 we need to show the year control
	// isYearShow(): boolean {
	// 	return ();
	// }

	//button visible
	isButtonDisable(): boolean {
		return (this.masterModel.mandayByYearExportLoading ||
			this.masterModel.mandayByCountryExportLoading ||
			this.masterModel.mandayByCustomerExportLoading ||
			this.masterModel.mandayByEmployeeTypeExportLoading ||
			this.masterModel.searchLoading);
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
				//this.changeCompareBy();

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
				//this.changeCompareBy();
			}

		}
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

	//get province list
	getProvinceList(countryId) {
		this.masterModel.mandayCountrySubProvinceLoading = true;
		this.locationService.getprovincebycountryid(countryId)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response && response.result == ResponseResult.Success)
						this.masterModel.mandayCountrySubProvinceList = response.data;
					this.masterModel.mandayCountrySubProvinceLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayCountrySubProvinceLoading = false;
				});

	}

	//auto complete the country list in top filter
	private getCountryList() {
		this.masterModel.factoryCountryList = concat(
			of([]), // default items
			this.masterModel.countryInput.pipe(
				distinctUntilChanged(),
				tap(() => this.masterModel.countryLoading = true),
				switchMap(term => this.mandayDashboardService.getCountryListByName(term).pipe(
					catchError(() => of([])), // empty list on error
					tap(() => this.masterModel.countryLoading = false)
				))
			)
		);
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
					this.model.serviceId = APIService.Inspection;

				},
				error => {
					this.setError(error);
					this.masterModel.officeLoading = false;
				});
	}

	//get mandayterm list
	getMandayTermList() {
		this.masterModel.mandayTermList = [{
			id: 1, name: 'Last 1 Week'
		}, {
			id: 2, name: 'Last 1 Month'
		}, {
			id: 3, name: 'Last 1 Year'
		}, {
			id: 4, name: 'Custom'
		}];
		this.masterModel.mandayTermId = 2;

		this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'd', 30);
		this.model.serviceDateTo = this.calendar.getToday();
		this.masterModel.compareBy = 1;
		//this.model.comparedServiceDateFrom = this.calendar.getPrev(this.model.serviceDateFrom, 'm', this.masterModel.compareBy);
		//this.model.comparedServiceDateTo = this.calendar.getPrev(this.model.serviceDateFrom, 'd');
	}

	//get country list for autocomplete control for manday by year chart
	private getCountryListMandayByYear() {
		this.masterModel.mandayYearSubCountryList = concat(
			of([]), // default items
			this.masterModel.mandayYearSubCountryInput.pipe(
				distinctUntilChanged(),
				tap(() => this.masterModel.mandayYearSubCountryLoading = true),
				switchMap(term => this.mandayDashboardService.getCountryListByName(term).pipe(
					catchError(() => of([])), // empty list on error
					tap(() => this.masterModel.mandayYearSubCountryLoading = false)
				))
			)
		);
	}

	//get customer list for autocomplete control for top level filter
	private getCustomerList() {


		this.masterModel.customerList = concat(
			of([]), // default items
			this.masterModel.customerInput.pipe(
				distinctUntilChanged(),
				tap(() => this.masterModel.customerLoading = true),
				switchMap(term => this.mandayDashboardService.getCustomerListByName(term).pipe(
					catchError(() => of([])), // empty list on error
					tap(() => this.masterModel.customerLoading = false)
				))
			)
		);

	}

	//get customer list for autocomplete control for manday by customer chart
	private getCountryListMandayByCustomer() {
		this.masterModel.mandayCustomerSubCountryList = concat(
			of([]), // default items
			this.masterModel.mandayCustomerSubCountryInput.pipe(
				distinctUntilChanged(),
				tap(() => this.masterModel.mandayCustomerSubCountryLoading = true),
				switchMap(term => this.mandayDashboardService.getCountryListByName(term).pipe(
					catchError(() => of([])), // empty list on error
					tap(() => this.masterModel.mandayCustomerSubCountryLoading = false)
				))
			)
		);
	}

	//get country list for autocomplete control for manday by country chart
	private getCountryListMandayByCountry() {
		this.masterModel.mandayCountrySubCountryList = concat(
			of([]), // default items
			this.masterModel.mandayCountrySubCountryInput.pipe(
				distinctUntilChanged(),
				tap(() => this.masterModel.mandayCountrySubCountryLoading = true),
				switchMap(term => this.mandayDashboardService.getCountryListByName(term).pipe(
					catchError(() => of([])), // empty list on error
					tap(() => this.masterModel.mandayCountrySubCountryLoading = false)
				))
			)
		);
	}

	//get manday by year export
	getMandayByYearExport() {
		this.validator.initTost();
		this.validator.isSubmitted = true;

		if (this.isFormValid() && this.isComparedateValid()) {
			this.masterModel.mandayByYearExportLoading = true;

			const requestModel = Object.assign({}, this.model);
			requestModel.serviceDateFrom = new NgbDate(this.model.serviceDateTo.year - 1, 1, 1);
			this.mandayDashboardService.getMandayByYearExport(requestModel)
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

	//get manday by customer export
	getMandayByCustomerExport() {
		this.validator.initTost();
		this.validator.isSubmitted = true;
		if (this.isFormValid() && this.isComparedateValid()) {
			this.masterModel.mandayByCustomerExportLoading = true;


			this.mandayDashboardService.getManDayCustomerChartExport(this.model)
				.pipe(takeUntil(this.componentDestroyed$))
				.subscribe(async res => {
					await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Manday_Customer_Details.xlsx");
					this.masterModel.mandayByCustomerExportLoading = false;
				},
					error => {
						this.masterModel.mandayByCustomerExportLoading = false;
					});
		}
	}

	//get maday by country export
	getMandayByCountryExport() {
		this.validator.initTost();
		this.validator.isSubmitted = true;
		if (this.isFormValid() && this.isComparedateValid()) {
			this.masterModel.mandayByCountryExportLoading = true;


			this.mandayDashboardService.getManDayCountryChartExport(this.model)
				.pipe(takeUntil(this.componentDestroyed$))
				.subscribe(async res => {
					await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Manday_Country_Details.xlsx");
					this.masterModel.mandayByCountryExportLoading = false;
				},
					error => {
						this.masterModel.mandayByCountryExportLoading = false;
					});
		}
	}

	//get maday by employee type export
	getMandayByEmployeeTypeExport() {
		this.validator.initTost();
		this.validator.isSubmitted = true;
		if (this.isFormValid() && this.isComparedateValid()) {

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
	}

	//manday count response
	getMandayCountByServiceResponse(response: MandayDashboardResponse) {
		if (response) {
			switch (response.result) {
				case MandayDashboardResult.success:
					{
						if (response.data != null) {
							this.mandayCountByService = response.data;
						}
						// else {
						// 	this.mandayCountByService = new MandayDashboardItem();
						// }
						break;
					}
				case MandayDashboardResult.notFound:
					{
						this.mandayCountByService.totalCount = 0;
						this.mandayCountByService.totalManday = 0;
						this.mandayCountByService.totalReportCount = 0;
						break;
					}
				default:
					this.mandayCountByService.totalCount = 0;
					this.mandayCountByService.totalManday = 0;
					this.mandayCountByService.totalReportCount = 0;
					break;
				// case MandayDashboardResult.fail:
				// 	{
				// 		this.showError('MANDAY_DASHBOARD.LBL_TITLE', 'MANDAY_DASHBOARD.MSG_FAILED');
				// 		break;
				// 	}

			}
		}
	}

	//manday count based on service
	getMandayCountByService() {
		let response: MandayDashboardResponse;
		this.masterModel.countLoading = true;
		this.masterModel.searchLoading = true;

		this.mandayDashboardService.getMandayDashboardSearch(this.model)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {

					this.getMandayCountByServiceResponse(response);
					this.masterModel.countLoading = false;
					this.masterModel.searchLoading = false;

				},
				error => {
					this.setError(error);
					this.masterModel.countLoading = false;
					this.masterModel.searchLoading = false;
				});
	}

	//get manday list for each year
	getMandaysByYear(pageLoading: boolean) {

		let response: MandayYearChartResponse;



		this.masterModel.mandayYearChartLoading = !pageLoading;
		this.masterModel.mandayYearLoading = pageLoading;

		const requestModel = Object.assign({}, this.model);
		requestModel.serviceDateFrom = new NgbDate(this.model.serviceDateTo.year - 1, 1, 1)
		this.mandayDashboardService.getmandayByYear(requestModel)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response) {
						if (response.result == MandayDashboardResult.success) {
							this.mandayYearChart = response.data;
							this.monthYearXAxis = response.monthYearXAxis;
							this.mandayByYearChartFrame();
						}
						else if (response.result == MandayDashboardResult.notFound) {
							this.mandayYearChart = new Array<MandayYearChart>();
							// this.monthYearXAxis = new Array<MandayYear>();
							// this.mandayByYearChartFrame();
						}
						else {
							this.mandayYearChart = new Array<MandayYearChart>();
							// this.monthYearXAxis = new Array<MandayYear>();
							// this.mandayByYearChartFrame();
						}
					}
					this.masterModel.mandayYearLoading = false;
					this.masterModel.mandayYearChartLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayYearLoading = false;
					this.masterModel.mandayYearChartLoading = false;
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

					chartObj[m]["value" + (k)] = monthData.length == 1 ? (this.model.mandayType == MandayType.EstimatedManDay ? monthData[0].monthManDay : monthData[0].monthActualManDay) : 0;
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

		//this.exportChart(chart);

		this.chart = chart;
	}

	//export charts in below formarts
	exportChart(chart) {

		chart.exporting.menu = new am4core.ExportMenu();
		chart.exporting.menu.align = "right";
		chart.exporting.menu.verticalAlign = "top";

		chart.exporting.menu.items = [{
			"label": "...",
			"menu": [
				{ "type": "jpg", "label": "JPG" }
			]
		}];
	}
	//get manday by top customers
	getMandayByCustomerChart(pageLoading: boolean) {
		let res: MandayCustomerChartResponse;

		this.masterModel.mandayCustomerLoading = pageLoading;
		this.masterModel.mandayCustomerChartLoading = !pageLoading;

		this.mandayDashboardService.getmandayByCustomerChart(this.model)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					res = response;
					if (res) {

						if (res.result == MandayDashboardResult.success) {
							this.mandayCustomerChart = res.data;
							setTimeout(() => {
								this.renderCustomerPieChart("piechartdiv");
							}, 300);
						}
						else if (res.result == MandayDashboardResult.notFound) {
							this.mandayCustomerChart = new Array<MandayCustomerChart>();
						}
						else {
							this.mandayCustomerChart = new Array<MandayCustomerChart>();
						}
					}
					this.masterModel.mandayCustomerLoading = false;
					this.masterModel.mandayCustomerChartLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayCustomerLoading = false;
					this.masterModel.mandayCustomerChartLoading = false;
				});

	}

	//get manday by countrires
	getMandayByCountryChart(pageLoading: boolean) {


		this.masterModel.mandayCountryLoading = pageLoading;
		this.masterModel.mandayCountryChartLoading = !pageLoading;
		this.mandayDashboardService.getmandayByCountryChart(this.model)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				res => {
					this.mandayCountryChartResponse = res;
					if (this.mandayCountryChartResponse) {
						if (res.result == MandayDashboardResult.success) {
							this.mandayCountryChartResponse.data = this.mandayCountryChartResponse.data;
							setTimeout(() => {
								this.renderCountryProvinceChart();
							}, 300);
						}
						else if (this.mandayCountryChartResponse.result == MandayDashboardResult.notFound) {
							this.mandayCountryChartResponse.data = new Array<MandayCountryChart>();
						}
						else {
							this.mandayCountryChartResponse.data = new Array<MandayCountryChart>();
						}
					}

					this.masterModel.mandayCountryLoading = false;
					this.masterModel.mandayCountryChartLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayCountryLoading = false;
					this.masterModel.mandayCountryChartLoading = false;
				});

	}

	//get manday by employee type(outsource,. etc)
	getMandaysByEmployeeTypeChart(pageLoading: boolean) {
		let response: MandayEmployeeTypeChartResponse;
		this.masterModel.mandayEmployeeTypeLoading = pageLoading;
		this.masterModel.mandayEmployeeTypeChartLoading = !pageLoading;

		this.mandayDashboardService.getmandayByEmployeeTypeChart(this.model)
			.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				res => {
					response = res;
					if (response) {
						if (response.result == MandayDashboardResult.success) {
							this.mandayEmployeeTypeChart = response.data;
							this.mandayEmployeeTypeXAxis = response.monthYearXAxis;
							this.mandayByEmployeeTypeChartFrame();

						}
						else if (response.result == MandayDashboardResult.notFound) {
							this.mandayEmployeeTypeChart = new Array<MandayEmployeeTypeChart>();
						}
						else {
							this.mandayEmployeeTypeChart = new Array<MandayEmployeeTypeChart>();
						}
					}

					this.masterModel.mandayEmployeeTypeLoading = false;
					this.masterModel.mandayEmployeeTypeChartLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.mandayEmployeeTypeLoading = false;
					this.masterModel.mandayEmployeeTypeChartLoading = false;
				});
	}

	//service change
	changeServiceId() {
		if (this.model.serviceId == APIService.Inspection) {
			// this.masterModel.isAuditShow = false;
			this.masterModel.mandayTypeList = MandayDropDown;
		}
		else if (this.model.serviceId == APIService.Audit || this.model.serviceId == APIService.Tcf) {
			// this.masterModel.isAuditShow = true;
			this.model.mandayType = MandayType.EstimatedManDay;
			this.masterModel.mandayTypeList = MandayDropDown.filter(x => x.id != MandayType.ActualManDay);
		}
	}

	//if top selected we are not showing the sub country filters
	isshowCountry() {
		return (!this.model.countryIdList || this.model.countryIdList.length <= 0);
	}

	//2nd chart based on  country and province
	renderCountryProvinceChart() {
		var chartCountryObj = [];

		this.radarChart = am4core.create("radarchartdiv", am4charts.PieChart);
		this.radarChart.hiddenState.properties.opacity = 0; // this creates initial fade-in
		let totalCount = 0;
		this.mandayCountryChartResponse.data.forEach(element => {
			// element.name = "chinchi (sdfsdfs)"
			totalCount += element.mandayCount;
			var subCountryObj = {
				country: element.name && element.name.split('(') && element.name.split('(').length > 0 &&
					element.name.split('(')[0].length > 14 ?
					(element.name.split('(')[0].substring(0, 14) + "..") : element.name.split('(')[0],
				visits: element.mandayCount,
				comparedPercentage: element.comparedPercentage
			};

			chartCountryObj.push(subCountryObj);
		});

		this.radarChart.data = chartCountryObj;

		this.radarChart.innerRadius = 45;

		//this.radarChart.depth = 17;

		//if country selected apply below condition for province
		// if (this.model.mandayCountrySubCountryId > 0) {
		// 	if (chartCountryObj.length < 3) {
		// 		this.radarChart.depth = 10;
		// 	}
		// 	else if (chartCountryObj.length < 5) {
		// 		this.radarChart.depth = 20;
		// 	}
		// 	else if (chartCountryObj.length >= 5) {
		// 		this.radarChart.depth = 80;
		// 	}
		// }
		// else {
		// 	this.radarChart.depth = 20;
		// }


		let series = this.radarChart.series.push(new am4charts.PieSeries());
		series.dataFields.value = "visits";
		//series.dataFields.depthValue = "visits";
		series.dataFields.category = "country";
		//series.slices.template.cornerRadius = 5;
		series.labels.template.disabled = true;
		series.ticks.template.disabled = true;
		//series.colors.step = 3;

		//	this.exportChart(this.radarChart);

		let label = series.createChild(am4core.Label);
		label.text = this.mandayCountryChartResponse.totalCount;
		label.fontSize = 16;
		label.verticalCenter = "middle";
		label.horizontalCenter = "middle";
		label.fontFamily = "roboto-medium";
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
		// valueAxis.min = 0;
		valueAxis.renderer.labels.template.fontSize = 8;
		valueAxis.renderer.labels.template.fill = am4core.color("#A1A8B3");
		valueAxis.renderer.grid.template.stroke = am4core.color("#A1A8B3");
		// valueAxis.renderer.inside = true;
		// valueAxis.renderer.labels.template.disabled = true;
		valueAxis.min = 0;
		valueAxis.max = 100;
		for (var i = 0; i < this.mandayEmployeeTypeChart.length; i++) {

			let stacked = i == 0 ? false : true;

			this.createSeries(this.mandayEmployeeTypeChart[i].color, this.mandayEmployeeTypeChart[i].employeeType.toLowerCase(), this.mandayEmployeeTypeChart[i].employeeType, stacked);
		}

		//this.exportChart(this.clusteredColumChart);

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

	/**
	* 2nd graph based on customer render the chart
	*/
	renderCustomerPieChart(graphContainerName) {
		var chartCustomerObj = [];

		this.pieChart = am4core.create(graphContainerName, am4charts.PieChart);
		this.pieChart.hiddenState.properties.opacity = 0; // this creates initial fade-in

		this.mandayCustomerChart.forEach(element => {
			// element.customerName = "cKDKDKKDKDKDKDKKKKDKDDKDKDhinchi"
			var subCustomerObj = {
				country: element.customerName && element.customerName.length > 27 ? (element.customerName.substring(0, 27) + "..") : element.customerName,
				value: element.mandayCount,
				percentage: element.percentage,
				comparedPercentage: element.comparedPercentage
			};
			chartCustomerObj.push(subCustomerObj);
		});

		this.pieChart.data = chartCustomerObj;
		this.pieChart.radius = am4core.percent(70);
		this.pieChart.innerRadius = am4core.percent(40);
		this.pieChart.startAngle = 180;
		this.pieChart.endAngle = 360;

		let series = this.pieChart.series.push(new am4charts.PieSeries());
		series.dataFields.value = "value";
		series.dataFields.category = "country";
		series.labels.template.disabled = true;

		// chartCustomerObj.forEach(element => {
		// 	series.slices.template.tooltipText = element.country + ": " +
		// 		element.percentage + "% (" + element.value + ")"
		// });

		series.slices.template.cornerRadius = 4;
		series.slices.template.innerCornerRadius = 7;
		series.slices.template.draggable = false;
		series.slices.template.inert = true;
		series.alignLabels = false;

		series.hiddenState.properties.startAngle = 90;
		series.hiddenState.properties.endAngle = 90;

		//this.exportChart(this.pieChart);
	}
	toggleAdvanceSearchSection() {
		this.masterModel.toggleFormSection = !this.masterModel.toggleFormSection;
	}

	toggleFilter() {
		this.filterOpen = !this.filterOpen;
	}
	clearDateInput(controlName: any) {
		switch (controlName) {
			case "Fromdate": {
				this.model.serviceDateFrom = null;
				break;
			}
			case "Todate": {
				this.model.serviceDateTo = null;
				break;
			}
			case "comparedServiceDateFrom": {
				this.model.comparedServiceDateFrom = null;
				break;
			}
			case "comparedServiceDateTo": {
				this.model.comparedServiceDateTo = null;
				break;
			}
		}
	}
	//get customer list for autocomplete control for manday by year chart
	private getCustomerListMandayByYear() {
		this.masterModel.mandayYearSubCustomerList = concat(
			of([]), // default items
			this.masterModel.mandayYearSubCustomerInput.pipe(
				distinctUntilChanged(),
				tap(() => this.masterModel.mandayYearSubCustomerLoading = true),
				switchMap(term => this.mandayDashboardService.getCustomerListByName(term).pipe(
					catchError(() => of([])), // empty list on error
					tap(() => this.masterModel.mandayYearSubCustomerLoading = false)
				))
			)
		);
	}
	//get country list for autocomplete control for manday by employee type chart
	private getCountryListMandayByEmployeeType() {
		this.masterModel.mandayEmployeeTypeSubCustomerList = concat(
			of([]), // default items
			this.masterModel.mandayEmployeeTypeSubCustomerInput.pipe(
				distinctUntilChanged(),
				tap(() => this.masterModel.mandayEmployeeTypeSubCustomerLoading = true),
				switchMap(term => this.mandayDashboardService.getCustomerListByName(term).pipe(
					catchError(() => of([])), // empty list on error
					tap(() => this.masterModel.mandayEmployeeTypeSubCustomerLoading = false)
				))
			)
		);
	}

	resetChartFilter() {
		this.masterModel.isMandayCountrySubProvinceShow = false;
		this.model.mandayCountrySubCountryId = undefined;
		this.model.mandayCountrySubProvinceId = undefined;
		this.model.mandayCustomerSubCountryId = undefined;
		this.model.mandayYearSubCountryId = undefined;
		this.model.mandayEmployeeTypeSubCustomerId = undefined;
	}
	//change mandayTerm
	// changeCompareBy() {
	// 	if(this.model.IsCompareData)
	// 	{
	// 	this.model.comparedServiceDateFrom = this.calendar.getPrev(this.model.serviceDateFrom, 'm', this.masterModel.compareBy);
	// 	this.model.comparedServiceDateTo = this.calendar.getPrev(this.model.serviceDateFrom, 'd');
	// 	}
	// }
}


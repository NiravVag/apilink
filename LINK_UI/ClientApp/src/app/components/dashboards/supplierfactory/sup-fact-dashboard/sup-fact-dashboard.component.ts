import { Component, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbCalendar, NgbDate, NgbDateParserFormatter, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from 'src/app/components/common/detail.component';
import mapboxgl from 'mapbox-gl';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { state, style, trigger, transition, animate } from '@angular/animations';
import { BookingDetails, CustomerBookingModel, DashboardResult, SupFactDashboardDataSourceRequest, SupFactDashboardMasterModel, SupFactDashBoardModel } from 'src/app/_Models/dashboard/supfactdashboard.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { DataLengthTrim, DefaultDateType, FBResultNAName, FBResultNotApplicableName, SupplierType, UserType } from 'src/app/components/common/static-data-common';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { DropdownResult } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { Validator } from 'src/app/components/common';
import { SupFactDashboardService } from 'src/app/_Services/dashboard/supfactdashboard.service';
import { DashBoardService } from 'src/app/_Services/dashboard/dashboard.service';
import { CustomerAPIRADashboard, InspectionRejectDashboard } from 'src/app/_Models/dashboard/customerdashboard.model';
import { CustomerDashboardFilterModel } from 'src/app/_Models/dashboard/customerdashboardfilterrequest.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';

am4core.useTheme(am4themes_animated);

@Component({
	selector: 'app-sup-fact-dashboard',
	templateUrl: './sup-fact-dashboard.component.html',
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
export class SupFactDashboardComponent extends DetailComponent {
	
	componentDestroyed$: Subject<boolean> = new Subject();
	masterModel: SupFactDashboardMasterModel;
	model: SupFactDashBoardModel;
	currentUser: UserModel;
	mapObj: any;
	public dataLengthTrim = DataLengthTrim;
	error: boolean;
	isMobile: boolean;
	isLoading: boolean
	searchQuery: string;
	cars: Array<any> = [];
	isFullscreen: boolean;
	isFilterOpen: boolean;
	searchActive: boolean;
	toDate: NgbDate | null;
	fromDate: NgbDate | null;
	hasSearchResult: boolean;
	loadingLineChart: boolean;
	loadingRadarChart: boolean;
	showFactoryResult: boolean;
	sampleList: Array<any> = [];
	selectedSearchResult: object = { 'factoryName': '', 'factoryID': null };
	searchResultData: Array<object>;
	hoveredDate: NgbDate | null = null;
	cusfilter:CustomerDashboardFilterModel =null;
	@ViewChild('datepicker') datepicker;

	@ViewChild('searchTrigger') searchTrigger: ElementRef;
	@ViewChild('searchContainer') searchContainer: ElementRef;
	@ViewChild('mobileSearchContainer') mobileSearchContainer: ElementRef;


	constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
		private calendar: NgbCalendar, public formatter: NgbDateParserFormatter,
		private supService: SupplierService, authserve: AuthenticationService,
		private cusService: CustomerService, public bookingService: BookingService,
		public validator: Validator, private supFactservice: SupFactDashboardService,
		private dashboardservice: DashBoardService, utility: UtilityService,
		modelservice:NgbModal) {

		super(router, route, translate, toastr, modelservice, utility);
		am4core.addLicense("CH238479116");
		this.model = new SupFactDashBoardModel();
		this.masterModel = new SupFactDashboardMasterModel();
		this.currentUser = authserve.getCurrentUser();
		this.masterModel.isSupplier = this.currentUser.usertype == UserType.Supplier ? true : false;
		this.masterModel.isFactory = this.currentUser.usertype == UserType.Factory ? true : false;
		this.getIsMobile();
		this.error = false;

		this.isLoading = false;
		this.isFullscreen = false;
		this.isFilterOpen = false;
		this.searchActive = false;
		this.showFactoryResult = false;

		//default date set
		this.model.fromDate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
		this.model.toDate = this.calendar.getToday();
	}

	ngOnDestroy(): void {
		this.componentDestroyed$.next(true);
		this.componentDestroyed$.complete(); 
	  }

	onInit() {
		this.masterModel.apiresultLoading = true;
		this.masterModel.customerDataLoading = true;
		this.masterModel.inspectionRejectLoading = true;
		this.masterModel.bookingDataLoading = true;

		this.getBookingStatusList();
		//if supplier login 
		if (this.masterModel.isSupplier) {
			this.masterModel.supplierModelRequest.id = this.currentUser.supplierid;
			this.masterModel.customerModelRequest.supplierId = this.currentUser.supplierid;
			this.masterModel.factoryModelRequest.supplierId = this.currentUser.supplierid;
			this.masterModel.customerModelRequest.supplierType = SupplierType.Supplier;
			this.model.supplierId = this.currentUser.supplierid;

			this.getSupListBySearch();
			this.getFactListBySearch();
			this.getCustomerListBySearch();
		}
		//if factory login
		else if (this.masterModel.isFactory) {

			this.masterModel.factoryModelRequest.id = this.currentUser.factoryid;
			this.masterModel.supplierModelRequest.factoryId = this.currentUser.factoryid;
			this.masterModel.customerModelRequest.factoryId = this.currentUser.factoryid;
			this.masterModel.customerModelRequest.supplierType = SupplierType.Factory;
			this.model.factoryId = this.currentUser.factoryid;

			this.getFactListBySearch();
			this.getSupListBySearch();
			this.getCustomerListBySearch();
		}
		this.validator.isSubmitted = false;
		this.validator.setJSON("dashboard/supfactdashboard.valid.json");
		this.validator.setModelAsync(() => this.model);
		this.getBookingIds();
	}

	//fetch the supplier data with virtual scroll
	getSupplierData() {
		this.masterModel.supplierLoading = true;

		this.masterModel.supplierModelRequest.searchText = this.masterModel.supplierInput.getValue();
		this.masterModel.supplierModelRequest.skip = this.masterModel.supplierList.length;
		this.masterModel.supplierModelRequest.supplierType = SupplierType.Supplier;
		this.masterModel.supplierModelRequest.id = this.model.supplierId;

		this.supService.getFactoryOrSupplierDataSource(this.masterModel.supplierModelRequest)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(data => {
				if (data && data.length > 0) {
					this.masterModel.supplierList = this.masterModel.supplierList.concat(data);
				}
				this.masterModel.supplierModelRequest = new SupFactDashboardDataSourceRequest();
				this.masterModel.supplierLoading = false;
			}),
			error => {
				this.masterModel.supplierLoading = false;
				this.setError(error);
			};
	}

	//fetch the first 10 suppliers for the customer on load
	getSupListBySearch() {
		this.masterModel.supplierList = null;
		this.masterModel.supplierModelRequest.supplierType = SupplierType.Supplier;
		this.masterModel.supplierInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterModel.supplierLoading = true),
			switchMap(term => term
				? this.supService.getFactoryOrSupplierDataSource(this.masterModel.supplierModelRequest, term)
				: this.supService.getFactoryOrSupplierDataSource(this.masterModel.supplierModelRequest)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterModel.supplierLoading = false))
			))
			.subscribe(data => {
				this.masterModel.supplierList = data;
				this.masterModel.supplierLoading = false;
			});
	}

	//fetch the facotry data with virtual scroll
	getFactoryData() {
		this.masterModel.factoryModelRequest.searchText = this.masterModel.factoryInput.getValue();
		this.masterModel.factoryModelRequest.skip = this.masterModel.factoryList.length;

		this.masterModel.factoryModelRequest.supplierType = SupplierType.Factory;
		this.masterModel.factoryLoading = true;
		this.masterModel.factoryModelRequest.supplierId = this.model.supplierId;

		this.supService.getFactoryOrSupplierDataSource(this.masterModel.factoryModelRequest).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterModel.factoryList = this.masterModel.factoryList.concat(data);
				}
				this.masterModel.factoryModelRequest = new SupFactDashboardDataSourceRequest();
				this.masterModel.factoryLoading = false;
			}),
			error => {
				this.masterModel.factoryLoading = false;
				this.setError(error);
			};
	}

	//fetch the first 10 fact for the supplier on load
	getFactListBySearch() {
		this.masterModel.factoryList = null;
		this.masterModel.factoryModelRequest.supplierType = SupplierType.Factory;
		this.masterModel.factoryInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterModel.factoryLoading = true),
			switchMap(term => term
				? this.supService.getFactoryOrSupplierDataSource(this.masterModel.factoryModelRequest, term)
				: this.supService.getFactoryOrSupplierDataSource(this.masterModel.factoryModelRequest)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterModel.factoryLoading = false))
			))
			.subscribe(data => {
				this.masterModel.factoryList = data;
				this.masterModel.factoryLoading = false;
			});
	}
	//fetch the customer data with virtual scroll
	getCustomerData() {

		this.masterModel.customerModelRequest.searchText = this.masterModel.customerInput.getValue();
		this.masterModel.customerModelRequest.skip = this.masterModel.customerList.length;
		this.masterModel.customerModelRequest.supplierId = this.model.supplierId;

		this.masterModel.customerLoading = true;
		this.cusService.getCustomerDataSourceBySupplier(this.masterModel.customerModelRequest)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(data => {
				if (data && data.length > 0) {
					this.masterModel.customerList = this.masterModel.customerList.concat(data);
				}

				this.masterModel.customerModelRequest = new SupFactDashboardDataSourceRequest();
				this.masterModel.customerLoading = false;
			}),
			error => {
				this.masterModel.customerLoading = false;
				this.setError(error);
			};
	}

	getCustomerListBySearch() {
		this.masterModel.customerModelRequest.customerId = 0;
		this.masterModel.customerInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterModel.customerLoading = true),
			switchMap(term => term
				? this.cusService.getCustomerDataSourceBySupplier(this.masterModel.customerModelRequest, term)
				: this.cusService.getCustomerDataSourceBySupplier(this.masterModel.customerModelRequest)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterModel.customerLoading = false))
			))
			.subscribe(data => {
				this.masterModel.customerList = data;
				this.masterModel.customerLoading = false;
			});
	}

	getBookingStatusList() {
		this.masterModel.bookingStatusLoading = true;
		this.bookingService.getBookingStatusList()
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				data => {
					if (data && data.result == DropdownResult.Success) {
						this.masterModel.bookingStatusList = data.dataSourceList;
					}
					this.masterModel.bookingStatusLoading = false;
				},
				error => {
					this.setError(error);
					this.masterModel.bookingStatusLoading = false;
				});
	}

	setmapcountry(inspectionIds) {
		this.supFactservice.getInspFactoryGeoCode(inspectionIds)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					
					this.masterModel.mapLoading = false;
					setTimeout(() => {
						
						this.renderMap(response);
					}, 500);
				},
				error => {
					this.masterModel.mapLoading = false;
					setTimeout(() => {
						
						this.renderMap();
					}, 100);
				});

	}

	//Get all bookingids
	getBookingIds() {
		this.supFactservice.getBookingIds(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response && response.result == DashboardResult.Success) {
						let inspectionIds = response.idList;

						if (inspectionIds && inspectionIds.length > 0) {
							this.setmapcountry(inspectionIds);
							this.getInspectionRejectDashBoard(inspectionIds);
							this.getAPIRADashBoard(inspectionIds);
							this.getBookingDetailsDashBoard(inspectionIds);
							this.getCustomerOverviewDashBoard(inspectionIds);
						}
						else {
							//default render map
							setTimeout(() => {
								this.renderMap();
							}, 100);
						}
					}
					else {
						setTimeout(() => {
							this.renderMap();
						}, 100);
						this.masterModel.bookingDataFound = false;
						this.masterModel.apiresultFound = false;
						this.masterModel.customerDataFound = false;
						this.masterModel.inspectionRejectFound = false;
						this.masterModel.searchLoading = false;
						this.masterModel.apiresultLoading = false;
						this.masterModel.customerDataLoading = false;
						this.masterModel.inspectionRejectLoading = false;
						this.masterModel.bookingDataLoading = false;
					}
				},
				error => {
					this.showError('SUPPLIER_FACTORY_DASHBOARD.LBL_DASHBOARD', 'COMMON.MSG_UNKNONW_ERROR');

				});
	}

	 search() {
		this.masterModel.searchLoading = true;
		 this.getBookingIds();
		// this.masterModel.searchLoading = false;

	}
	getIsMobile() {
		if (window.innerWidth < 450) {
			this.isMobile = true;
		} else {
			this.isMobile = false;
		}
	}

	/**
* Function to render map
*/
	renderMap(geoCodes?) {
		(mapboxgl as typeof mapboxgl).accessToken = 'pk.eyJ1IjoiYW1hbnNvbmkyMTEiLCJhIjoiY2sxdGpldXMyMDA0bDNubjgxZzV1cXhxYiJ9.lg92cAq6oc1paYlkwd0i8Q';
		var map = new mapboxgl.Map({
			container: 'mapView',
			style: 'mapbox://styles/amansoni211/ck1tjsmti0a4n1co8k4bfd77n',
			zoom: 1.30,
			center: [this.masterModel.centreLongitude, this.masterModel.centreLatitude],
			scrollZoom: !this.isMobile ? true : false,
			minZoom: 2
		});

		// this.masterModel.mapHeight = 380;
		var jsonGeo = new Array();
		if (geoCodes) {
			var i = 1;

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
					//'filter': ['any', ['all', ['<=', ['zoom'], 2], ['==', ['get', 'type'], 'factory']]],
					'paint': {
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
					//filter: ['any', ['all', ['<=', ['zoom'], 2], ['==', ['get', 'type'], 'factory']]],
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

				//   // Ensure that if the map is zoomed out such that multiple
				//   // copies of the feature are visible, the popup appears
				//   // over the copy being pointed to.
				  while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
				    coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
				  }

				//   // Populate the popup and set its coordinates
				//   // based on the feature found.
				  popup.setLngLat(coordinates)
				    .setHTML(description)
				    .addTo(map);

				//   // adding hover effect
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


	//Get the inspection reject dashboards
	getInspectionRejectDashBoard(inspectionIds) {
		this.cusfilter =new CustomerDashboardFilterModel();
		this.cusfilter.customerId=this.model.customerId;
		this.cusfilter.supplierId=this.model.supplierId;
		this.cusfilter.factoryId=this.model.factoryId;
		this.cusfilter.statusIdList=this.model.statusIdList;
		this.cusfilter.serviceDateFrom=this.model.fromDate;
		this.cusfilter.serviceDateTo=this.model.toDate;
		this.masterModel.inspectionRejectLoading = true;
		this.dashboardservice.getInspectionRejectResult(this.cusfilter)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response && response.length > 0) {
						this.masterModel.inspectionRejectDashboard = response;
						setTimeout(() => {
							this.halfDonutInspectionRejectChart();
						}, 10);
						this.masterModel.inspectionRejectFound = true;
					}
					else {
						this.masterModel.inspectionRejectDashboard = new Array<InspectionRejectDashboard>();
						this.masterModel.inspectionRejectFound = false;
					}
					this.masterModel.inspectionRejectLoading = false;
					this.masterModel.searchLoading = false;
				},
				error => {
					this.masterModel.inspectionRejectLoading = false;
					this.masterModel.inspectionRejectError = true;
				});
	}

	getCustomerOverviewDashBoard(inspectionIds: Array<number>) {
		this.masterModel.customerDataLoading = true;		
		this.supFactservice.getInspCustomerData(inspectionIds)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					
					if (response && response.result == DashboardResult.Success) {
						this.masterModel.customerDataFound = true;
						this.masterModel.customerDataDashboard = response.cusBookingDetails;
						setTimeout(() => {
							this.renderFullPieChart();
						}, 10);
					}
					else {
						this.masterModel.customerDataDashboard = new Array<CustomerBookingModel>();
						this.masterModel.customerDataFound = false;
					}
					this.masterModel.customerDataLoading = false;
					
				},
				error => {
					this.masterModel.customerDataLoading = false;
					this.masterModel.customerDataError = true;
				});
	}

	getBookingDetailsDashBoard(inspectionIds: Array<number>) {
		this.masterModel.bookingDataLoading = true;
		this.supFactservice.getInspDetails(inspectionIds)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response && response.result == DashboardResult.Success) {
						this.masterModel.bookingDataDashboard = response.bookingDetails;
						this.masterModel.bookingDataFound = true;
					}
					else {
						this.masterModel.bookingDataDashboard = new Array<BookingDetails>();
						this.masterModel.bookingDataFound = false;
					}
					this.masterModel.bookingDataLoading = false;
				},
				error => {
					this.masterModel.bookingDataLoading = false;
					this.masterModel.bookingDataError = true;
				});
	}

	//Get the API Result Analysis Data
	getAPIRADashBoard(inspectionIds) {
		this.cusfilter =new CustomerDashboardFilterModel();
		this.cusfilter.customerId=this.model.customerId;
		this.cusfilter.supplierId=this.model.supplierId;
		this.cusfilter.factoryId=this.model.factoryId;
		this.cusfilter.statusIdList=this.model.statusIdList;
		this.cusfilter.serviceDateFrom=this.model.fromDate;
		this.cusfilter.serviceDateTo=this.model.toDate;

		this.masterModel.apiresultLoading = true;
		this.dashboardservice.getAPIResult(this.cusfilter)
        .pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				response => {
					if (response && response.length > 0) {
						this.masterModel.apiraDashboard = response;
						if (this.masterModel.apiraDashboard) {
							var data = this.masterModel.apiraDashboard.find(x => x.statusName.toLowerCase() == FBResultNotApplicableName.toLowerCase());
							if (data) {
								//status name update not applicable to NA
								data.statusName = FBResultNAName;
							}
						}
						setTimeout(() => {
							this.renderPieChart('chartdiv');
						}, 10);
						this.masterModel.apiresultFound = true;
					}
					else {
						this.masterModel.apiraDashboard = new Array<CustomerAPIRADashboard>();
						this.masterModel.apiresultFound = false;
					}
					this.masterModel.apiresultLoading = false;
				},
				error => {
					this.masterModel.apiresultLoading = false;					  
					this.masterModel.apiresultError = true;
				});
	}

	renderPieChart(container) {

		var data = this.masterModel.apiraDashboard;
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

	/**	
	  * Function to render half donut chart
	  Create the Inpection Reject Analysis Graph
	  */
	halfDonutInspectionRejectChart() {
		let chart = am4core.create('halfdonutchart', am4charts.PieChart);
		chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

		chart.data = this.masterModel.inspectionRejectDashboard;

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

	renderFullPieChart() {

		// Create chart instance
		var chart = am4core.create('fullPieChart', am4charts.PieChart);

		var data = this.masterModel.customerDataDashboard;
		var chartObj = [];
		if (data && data.length > 0) {

			for (var i = 0; i < data.length; i++) {
				chartObj.push({
					"sector": data[i].customerName,
					"size": data[i].bookingCount,
					"color": am4core.color(data[i].statusColor)
				});
			}
		}
		// Add data
		chart.data = chartObj;

		chart.innerRadius = 45;

		// Add and configure Series
		var pieSeries = chart.series.push(new am4charts.PieSeries());
		pieSeries.dataFields.value = "size";
		pieSeries.dataFields.category = "sector";
		pieSeries.labels.template.disabled = true;
		pieSeries.slices.template.propertyFields.fill = 'color';

		pieSeries.tooltip.autoTextColor = false;
		pieSeries.tooltip.label.fill = am4core.color("#FFFFFF");
	}

	validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
		const parsed = this.formatter.parse(input);
		return parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
	}

	onDateSelection(date: NgbDate) {
		if (!this.model.fromDate && !this.model.toDate) {
			this.model.fromDate = date;
		} else if (this.model.fromDate && !this.model.toDate && date && date.after(this.model.fromDate)) {
			this.model.toDate = date;
			this.datepicker.close();
		} else {
			this.model.toDate = null;
			this.model.fromDate = date;
		}

		if(this.model.fromDate!=null && this.model.toDate!=null)
		{
			this.search();
		}
	}

	isHovered(date: NgbDate) {
		return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
	}

	isInside(date: NgbDate) {
		return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
	}

	isRange(date: NgbDate) {
		return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
	}

	
	isFormValid() {
		return this.validator.isValid('fromDate') &&
		  this.validator.isValid('toDate');
	
	  }
	
	//mobile version search
	mobileSearch() {
	
		this.validator.isSubmitted = true;
	
		if (this.isFormValid()) {
			
			this.getBookingIds();
			this.toggleFilter();
		}
	}

	toggleFilter() {
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

	getEditPath() {
		return "";
	}

	getViewPath(): string {
		return "";
	}

	//redirect to booking preivew page
	// redirectBookingEdit(bookingId: number) {
	// 	return ('inspedit/edit-booking/' + bookingId);
	// }
}

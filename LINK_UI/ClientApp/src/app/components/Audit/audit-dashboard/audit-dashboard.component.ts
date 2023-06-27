import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgbCalendar, NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { of, Subject } from 'rxjs';
import mapboxgl from 'mapbox-gl';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import { Validator } from '../../common/validator';
import { AuditDashboardDataFound, AuditDashboardItem, AuditDashboardLoader, AuditDashboardMasterData, ResponseResult } from 'src/app/_Models/Audit/auditdashboardmodel';
import { AuditSearchRedirectPage, AuditStatus, CusDashboardProductLength, DefaultDateType, FileContainerList, ListSize, MobileViewFilterCount, SearchType, SupplierNameTrim, SupplierType, UserType } from '../../common/static-data-common';
import { catchError, debounceTime, distinctUntilChanged, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CommonDataSourceRequest, CountryDataSourceRequest } from 'src/app/_Models/common/common.model';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { MandayDashboardService } from 'src/app/_Services/statistics/manday-dashboard.service';
import { MapGeoLocationResult } from 'src/app/_Models/dashboard/customerdashboard.model';
import { AuditDashboardService } from 'src/app/_Services/audit/audit-dashboard.service';
import { AuditService } from 'src/app/_Services/audit/audit.service';
import { Auditsummarymodel } from 'src/app/_Models/Audit/auditsummarymodel';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuditCusReportItem, auditcusreportrequest } from 'src/app/_Models/Audit/auditcusreportmodel';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';

@Component({
  selector: 'app-audit-dashboard',
  templateUrl: './audit-dashboard.component.html',
  styleUrls: ['./audit-dashboard.component.scss'],
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
export class AuditDashboardComponent {
  componentDestroyed$: Subject<boolean> = new Subject();
  [x: string]: any;

  model: Auditsummarymodel;

  centreLatitude = 30.77;
  centreLongitude = 11.25;
  mapLoading: boolean;
  requestSupModel: CommonDataSourceRequest;
  requestFactModel: CommonDataSourceRequest;
  requestCustomerModel: CommonDataSourceRequest;
  countryRequest: CountryDataSourceRequest;
  auditDashboardMasterData: AuditDashboardMasterData;
  dataFound: AuditDashboardDataFound;

  @ViewChild('leftColumn') leftColumn: ElementRef;
  @ViewChild('sideOverlay') sideOverlay: ElementRef;
  @ViewChild('mapCard') mapCard: ElementRef;

  @ViewChild('datepicker') datepicker;

  mapObj: any;
  error: boolean;
  isMobile: boolean;
  mapHeight: number;
  searchQuery: string;
  isFilterOpen: boolean;
  searchActive: boolean;
  hasSearchResult: boolean;
  showFactoryResult: boolean;
  hoveredDate: NgbDate | null = null;
  auditDashboardLoader: AuditDashboardLoader;
  currentUser: UserModel;
  public _IsInternalUser: boolean = false;
  _statuslist: any;
  _redirectpath: string;
  _redirecttype: any;
  _AuditStautus = AuditStatus;
  private currentRoute: Router;
  searchloading: boolean;

  constructor(public validator: Validator, private customerService: CustomerService, private supService: SupplierService,
    private locationService: LocationService, private mandayDashboardService: MandayDashboardService, private auditService: AuditService,
    private auditDashboardService: AuditDashboardService, private calendar: NgbCalendar, public formatter: NgbDateParserFormatter,
    public utility: UtilityService, private authserve: AuthenticationService, public fileStoreService: FileStoreService) {
    this.getIsMobile();
    this.error = false;
    this.mapHeight = 380;
    this.isFilterOpen = false;
    this.searchActive = false;
    this.showFactoryResult = false;
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this.validator.setJSON("audit/audit-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    am4core.addLicense("CH238479116");
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(): void {
    this.model = new Auditsummarymodel();
    this.auditDashboardMasterData = new AuditDashboardMasterData();
    this.requestSupModel = new CommonDataSourceRequest();
    this.requestFactModel = new CommonDataSourceRequest();
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.countryRequest = new CountryDataSourceRequest();
    this.dataFound = new AuditDashboardDataFound();
    this.auditDashboardLoader = new AuditDashboardLoader();
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'd', 7);
    this.model.todate = this.calendar.getToday();
    const user = this.authserve.getCurrentUser();
    if (user.usertype == UserType.Customer)
      this.model.customerid = user.customerid;
    else if (user.usertype == UserType.Supplier)
      this.model.supplierid = user.supplierid;
    else if (user.usertype == UserType.Factory)
      this.model.factoryidlst.push(user.factoryid);
    this.initialize();
  }

  async initialize() {
    this.getCustomerListBySearch();
    this.getSupListBySearch();
    this.getFactListBySearch();
    this.getCountryListBySearch();
    this.getOfficeList();
    this.setmapcountry();
    this.getServiceTypeAuditDashboardSummary();
    this.getOverviewDashboardSummary();
    this.getAuditTypeAuditDashboard();
    this.getAuditList();
    const response = await this.auditDashboardService.getAuditDashboardSummary(this.model)
    if (response.result == ResponseResult.Success)
      this.auditDashboardMasterData.auditDashboardCount = response.data;
    else
      this.auditDashboardMasterData.auditDashboardCount = new AuditDashboardItem();
    this.auditDashboardMasterData.filterDataShown = true;
  }

  filterTextShown() {
    var isFilterDataSelected = false;
    if ((this.model.customerid != null && this.model.customerid > 0) || (this.model.supplierid != null && this.model.supplierid > 0)
      || (this.model.factoryCountryIdList && this.model.factoryCountryIdList.length > 0) || (this.model.officeidlst && this.model.officeidlst.length > 0)
      || (this.model.factoryidlst && this.model.factoryidlst.length > 0)) {

      //desktop version
      if (!this.isMobile) {
        if (this.model.customerid) {
          var customerDetails = this.auditDashboardMasterData.customerList.find(x => x.id == this.model.customerid);
          this.auditDashboardMasterData.customerName = customerDetails ? customerDetails.name : "";
        }
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;

        if (this.model.supplierid > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.factoryCountryIdList && this.model.factoryCountryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        this.auditDashboardMasterData.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.auditDashboardMasterData.filterCount = 0;
        this.auditDashboardMasterData.countryNameList = [];
        this.auditDashboardMasterData.supplierName = "";
        this.auditDashboardMasterData.customerName = "";
        this.auditDashboardMasterData.officeNameList = [];
      }
    }

    return isFilterDataSelected;
  }

  cancelFilter() {
    this.model.mandayChartType = 1;
    this.model.factoryCountryIdList = [];
    this.model.customerid = null;
    this.model.supplierid = null;
    this.model.factoryidlst = [];
    this.model.officeidlst = [];
    this.isFilterOpen = false;
    this.auditDashboardMasterData.filterDataShown = false;
    this.auditDashboardMasterData.supplierName = null;
    this.auditDashboardMasterData.countryNameList = [];
    this.auditDashboardMasterData.factoryNameList = [];
    this.auditDashboardMasterData.customerName = null;
    this.auditDashboardMasterData.officeNameList = [];
  }

  isHovered(date: NgbDate) {
    return this.model.fromdate && !this.model.todate && this.hoveredDate && date.after(this.model.fromdate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.model.todate && date.after(this.model.fromdate) && date.before(this.model.todate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.model.fromdate) || (this.model.todate && date.equals(this.model.todate)) || this.isInside(date) || this.isHovered(date);
  }

  ngAfterViewInit() {
    this.renderMap();
  }

  onDateSelection(date: NgbDate, isMobile: boolean) {
    if (!this.model.fromdate && !this.model.todate) {
      this.model.fromdate = date;
    } else if (this.model.fromdate && !this.model.todate && date && date.after(this.model.fromdate)) {
      this.model.todate = date;
      this.datepicker.close();
    } else {
      this.model.todate = null;
      this.model.fromdate = date;
    }

    if (this.model.fromdate != null && this.model.todate != null && !isMobile)
      this.initialize();
  }

  search() {

    this.auditDashboardMasterData.filterDataShown = this.filterTextShown();
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

  formValid(): boolean {
    return this.validator.isValid('todate') && this.validator.isValid('fromdate')
  }

  setmapcountry() {
    if (this.model) {
      this.auditDashboardService.getAuditCountryGeoCode(this.model)
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

  getServiceTypeAuditDashboardSummary() {
    this.auditDashboardService.getServiceTypeAuditDashboardSummary(this.model)
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
            this.auditDashboardMasterData.serviceTypeDashboard = response.data;

            setTimeout(() => {
              this.renderPieChart('chartdiv-circle3', this.auditDashboardMasterData.serviceTypeDashboard);
            }, 1000);
            this.dataFound.serviceTypeDashboardDataFound = true;
            this.auditDashboardLoader.serviceTypeDashboardLoading = false;
            this.auditDashboardLoader.serviceTypeExportLoading = false;
          }
          else {
            this.dataFound.serviceTypeDashboardDataFound = false;
            this.auditDashboardLoader.serviceTypeDashboardLoading = false;
          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.auditDashboardLoader.serviceTypeDashboardLoading = false;
          this.auditDashboardLoader.serviceTypeDashboardError = true;
        });
  }

  getServiceTypeExport() {
    this.auditDashboardLoader.serviceTypeExportLoading = true;

    this.auditDashboardService.getServiceTypeChartExport(this.model)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Service_Type_Data.xlsx");
        this.auditDashboardLoader.serviceTypeExportLoading = false;
      },
        error => {
          this.auditDashboardLoader.serviceTypeExportLoading = false;
        });
  }

  getAuditTypeExport() {
    this.auditDashboardLoader.auditTypeExportLoading = true;

    this.auditDashboardService.getAuditTypeExport(this.model)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Audit_Type_Data.xlsx");
        this.auditDashboardLoader.auditTypeExportLoading = false;
      },
        error => {
          this.auditDashboardLoader.auditTypeExportLoading = false;
        });
  }

  getAuditTypeAuditDashboard() {
    this.auditDashboardLoader.auditTypeDashboardLoading = true;
    this.auditDashboardService.getAuditTypeAuditDashboard(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            let length = response.data.length;
            for (var i = 0; i < length; i++) {
              var _sublength = CusDashboardProductLength < i ? 18 : 9;
              response.data[i].name =

                response.data[i].statusName &&
                  response.data[i].statusName.length > 11 ?
                  response.data[i].statusName.substring(0, _sublength) + ".." :
                  response.data[i].statusName;
            }
            this.auditDashboardMasterData.auditTypeDashboard = response.data;
            setTimeout(() => {
              this.renderPieChart('chartdiv-circle2', this.auditDashboardMasterData.auditTypeDashboard);
            }, 1000);
            this.dataFound.auditTypeDashboardDataFound = true;
            this.auditDashboardLoader.auditTypeDashboardLoading = false;
            this.auditDashboardLoader.auditTypeDashboardExportLoading = false;
          }
          else {
            this.dataFound.auditTypeDashboardDataFound = false;
            this.auditDashboardLoader.auditTypeDashboardLoading = false;
          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.auditDashboardLoader.auditTypeDashboardLoading = false;
          this.auditDashboardLoader.auditTypeDashboardError = true;
        });
  }

  getOverviewDashboardSummary() {
    this.auditDashboardService.getOverviewAuditDashboardSummary(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.auditDashboardMasterData.overviewChart = response.data;
            this.dataFound.overviewDataFound = true;
            this.auditDashboardLoader.overviewChartLoading = false;
          }
          else {
            this.dataFound.overviewDataFound = false;
            this.auditDashboardLoader.overviewChartLoading = false;
          }
        },
        error => {
          this.showError('CUSTOMER_DASHBOARD.ERROR_RESULT', 'CUSTOMER_DASHBOARD.MSG_UNKNOWN_OCCURED');
          this.auditDashboardLoader.overviewChartLoading = false;
          this.auditDashboardLoader.overviewChartError = true;
        });
  }

  //search data
  getAuditList() {
    this.searchloading = true;
    const request = new auditcusreportrequest();
    request.searchtypeid = this.model.searchtypeid;
    request.searchtypetext = this.model.searchtypetext;
    request.customerid = this.model.customerid;
    request.supplierid = this.model.supplierid;
    request.factoryidlst = this.model.factoryidlst;
    request.statusidlst.push(AuditStatus.Audited);
    request.datetypeid = this.model.datetypeid;
    request.fromdate = this.model.fromdate;
    request.todate = this.model.todate;
    request.officeidlst = this.model.officeidlst;
    request.factoryCountryIdList = this.model.factoryCountryIdList;
    request.serviceTypelst = this.model.serviceTypeIdList;
    this.auditService.SearchAuditcusReport(request)
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.model.noFound = false;
            this.model.items = response.data.map((x) => {
              var item: AuditCusReportItem = {
                auditId: x.auditId,
                customer: x.customer,
                supplier: x.supplier,
                factory: x.factory,
                serviceDate: x.serviceDate,
                serviceType: x.serviceType,
                reportNo: x.reportNo,
                officeName: x.officeName,
                statusId: x.statusId,
                reportid: x.reportid,
                mimeType: x.mimeType,
                pathextension: x.pathextension,
                customerBookingNo: x.customerBookingNo,
                reportUrl: x.reportUrl,
                reportFileUniqueId: x.reportFileUniqueId,
                reportFileName: x.reportFileName,
                factoryCountry: x.factoryCountry,
                fbReportUrl: x.fbReportUrl,
                fbreportid: x.fbreportid
              }
              return item;
            });
          }
          else if (response && response.result == ResponseResult.NotFound) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
          }
          this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.searchloading = false;
        });
  }

  //download the report
  downloadreport(audititem: AuditCusReportItem) {
    if (audititem == null)
      return;
    if (audititem.reportUrl) {
      this.fileStoreService.downloadBlobFile(audititem.reportFileUniqueId, FileContainerList.Audit)
        .subscribe(res => {
          this.downloadReportFile(res, audititem.mimeType, audititem.pathextension, audititem.reportFileName);
          this.auditDashboardMasterData.downloadreport = false;
        },
          error => {
            this.auditDashboardMasterData.downloadreport = false;
            this.showError('AUDIT_REPORT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          });
      this.auditDashboardMasterData.downloadreport = false;
    }
    else {
      this.auditDashboardMasterData.downloadreport = true;
      this.auditService.getAuditReportFiles(audititem.reportid)
        .subscribe(res => {
          this.downloadReportFile(res, audititem.mimeType, audititem.pathextension, audititem.reportFileName);
          this.auditDashboardMasterData.downloadreport = false;
        },
          error => {
            this.auditDashboardMasterData.downloadreport = false;
            this.showError('AUDIT_REPORT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          });
    }
  }
  downloadReportFile(data, mimetype, pathextension, filename) {
    const blob = new Blob([data], { type: mimetype });
    filename = filename ? filename : "Audit_Report" + pathextension;
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, filename);
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = filename
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }

  getPathDetails() {
    throw new Error('Method not implemented.');
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

  getCustomerListBySearch() {
    this.requestCustomerModel.customerId = this.model.customerid;
    this.auditDashboardMasterData.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.auditDashboardMasterData.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.requestCustomerModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.auditDashboardMasterData.customerLoading = false))
      ))
      .subscribe(data => {
        this.auditDashboardMasterData.customerList = data;
        this.auditDashboardMasterData.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestCustomerModel.searchText = this.auditDashboardMasterData.customerInput.getValue();
      this.requestCustomerModel.skip = this.auditDashboardMasterData.customerList.length;
    }
    this.requestCustomerModel.customerId = this.model.customerid;
    this.auditDashboardMasterData.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.requestCustomerModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.auditDashboardMasterData.customerList = this.auditDashboardMasterData.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
        }
        this.auditDashboardMasterData.customerLoading = false;
      }),
      error => {
        this.auditDashboardMasterData.customerLoading = false;
        // this.setError(error);
      };
  }

  changeCustomerData(item) {
    if (item && item.id > 0) {
      var customerDetails = this.auditDashboardMasterData.customerList.find(x => x.id == item.id);
      if (customerDetails)
        this.auditDashboardMasterData.customerName =
          customerDetails.name.length > SupplierNameTrim ?
            customerDetails.name.substring(0, SupplierNameTrim) + "..." : customerDetails.name;
    }
    else {
      this.auditDashboardMasterData.customerName = "";
    }
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    this.requestSupModel.customerId = this.model.customerid;
    this.requestSupModel.supplierId = this.model.supplierid;
    this.requestSupModel.supplierType = SupplierType.Supplier;
    this.auditDashboardMasterData.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.auditDashboardMasterData.supLoading = true),
      switchMap(term => term
        ? this.supService.GetSupplierList(this.requestSupModel, term)
        : this.supService.GetSupplierList(this.requestSupModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.auditDashboardMasterData.supLoading = false))
      ))
      .subscribe(data => {
        this.auditDashboardMasterData.supplierList = data;
        this.auditDashboardMasterData.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestSupModel.searchText = this.auditDashboardMasterData.supInput.getValue();
      this.requestSupModel.skip = this.auditDashboardMasterData.supplierList.length;
    }
    this.requestSupModel.customerId = this.model.customerid;
    this.requestSupModel.supplierId = this.model.supplierid;
    this.requestSupModel.supplierType = SupplierType.Supplier;
    this.auditDashboardMasterData.supLoading = true;
    this.supService.GetSupplierList(this.requestSupModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.auditDashboardMasterData.supplierList = this.auditDashboardMasterData.supplierList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.requestSupModel.skip = 0;
          this.requestSupModel.take = ListSize;
        }
        this.auditDashboardMasterData.supLoading = false;
      }),
      error => {
        this.auditDashboardMasterData.supLoading = false;
        this.setError(error);
      };
  }

  //supplier change event
  supplierChange(supplierItem) {
    if (supplierItem && supplierItem.id > 0) {
      var supplierDetails = this.auditDashboardMasterData.supplierList.find(x => x.id == supplierItem.id);
      if (supplierDetails)
        this.auditDashboardMasterData.supplierName =
          supplierDetails.name.length > SupplierNameTrim ?
            supplierDetails.name.substring(0, SupplierNameTrim) + "..." : supplierDetails.name;
    }
    else {
      this.auditDashboardMasterData.supplierName = "";
    }
  }

  //fetch the first 10 suppliers for the customer on load
  getFactListBySearch() {
    this.requestFactModel.customerId = this.model.customerid;
    this.requestFactModel.supplierId = this.model.supplierid;
    this.requestFactModel.supplierType = SupplierType.Factory;
    this.auditDashboardMasterData.factInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.auditDashboardMasterData.factLoading = true),
      switchMap(term => term
        ? this.supService.GetFactoryList(this.requestFactModel, term)
        : this.supService.GetFactoryList(this.requestFactModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.auditDashboardMasterData.factLoading = false))
      ))
      .subscribe(data => {
        this.auditDashboardMasterData.factoryList = data;
        this.auditDashboardMasterData.factLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestFactModel.searchText = this.auditDashboardMasterData.factInput.getValue();
      this.requestFactModel.skip = this.auditDashboardMasterData.factoryList.length;
    }
    this.requestFactModel.customerId = this.model.customerid;
    this.requestFactModel.supplierType = SupplierType.Factory;
    this.requestFactModel.supplierId = this.model.supplierid;
    this.auditDashboardMasterData.factLoading = true;
    this.supService.GetFactoryList(this.requestFactModel).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.auditDashboardMasterData.factoryList = this.auditDashboardMasterData.factoryList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.requestModel = new CommonDataSourceRequest();
          this.requestFactModel.skip = 0;
          this.requestFactModel.take = ListSize;
        }
        this.auditDashboardMasterData.factLoading = false;
      }),
      error => {
        this.auditDashboardMasterData.factLoading = false;
        this.setError(error);
      };
  }

  //supplier change event
  factoryChange(item) {
    if (item) {

      if (this.model.factoryidlst && this.model.factoryidlst.length > 0) {

        var factoryLength = this.model.factoryidlst.length;

        var factoryDetails = [];
        for (var i = 0; i < factoryLength; i++) {

          factoryDetails.push(this.auditDashboardMasterData.factoryList.find(x => x.id == this.model.factoryidlst[i]).name);
        }
        this.auditDashboardMasterData.factoryNameList = factoryDetails;
      }
      else {
        this.auditDashboardMasterData.factoryNameList = [];
      }
    }
  }


  //fetch the country data with virtual scroll
  getCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.countryRequest.searchText = this.auditDashboardMasterData.countryInput.getValue();
      this.countryRequest.skip = this.auditDashboardMasterData.countryList.length;
    }

    this.auditDashboardMasterData.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.auditDashboardMasterData.countryList = this.auditDashboardMasterData.countryList.concat(customerData);
        }
        if (isDefaultLoad)
          this.countryRequest = new CountryDataSourceRequest();
        this.auditDashboardMasterData.countryLoading = false;
      }),
      error => {
        this.auditDashboardMasterData.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.auditDashboardMasterData.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.auditDashboardMasterData.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.auditDashboardMasterData.countryLoading = false))
      ))
      .subscribe(data => {
        this.auditDashboardMasterData.countryList = data;
        this.auditDashboardMasterData.countryLoading = false;
      });
  }

  //country change event
  countryChange(countryItem) {

    if (countryItem) {

      if (this.model.factoryCountryIdList && this.model.factoryCountryIdList.length > 0) {

        var countryLength = this.model.factoryCountryIdList.length;

        let countryDetails = [];
        for (var i = 0; i < countryLength; i++) {

          countryDetails.push(this.auditDashboardMasterData.countryList.find(x => x.id == this.model.factoryCountryIdList[i]).name);
        }
        this.auditDashboardMasterData.countryNameList = countryDetails;
      }
      else {
        this.auditDashboardMasterData.countryNameList = [];
      }
    }
  }

  //get office list
  getOfficeList() {
    this.auditDashboardMasterData.officeLoading = true;
    this.mandayDashboardService.getOfficeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.auditDashboardMasterData.officeList = response.dataSourceList;
          this.auditDashboardMasterData.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.auditDashboardMasterData.officeLoading = false;
        });
  }

  //office change event
  changeOffice(officeItem) {
    if (officeItem) {

      if (this.model.officeidlst && this.model.officeidlst.length > 0) {

        var officeLength = this.model.officeidlst.length;

        var officeDetails = [];
        for (var i = 0; i < officeLength; i++) {

          officeDetails.push(this.auditDashboardMasterData.officeList.find(x => x.id == this.model.officeidlst[i]).name);
        }
        this.auditDashboardMasterData.officeNameList = officeDetails;
      }
      else {
        this.auditDashboardMasterData.officeNameList = [];
      }
    }
  }
}

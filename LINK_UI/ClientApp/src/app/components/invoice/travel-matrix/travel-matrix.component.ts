import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, Data } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import {
  TravelMatrixMasterModel, TravelMatrixSummary, TravelMatrixModel, ProvinceList, CityList, CountyList, TravelMatixSaveResponse,
  TravelMatrixResponseResult, TravelMatixSearchResponse,
  AreaData, TravelMatrixExists, TariffType, AreaType
} from 'src/app/_Models/invoice/travelmatrix';
import { PageSizeCommon, TravelMatrixSearchEnum, travelMatrixSearchTypeList } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from "../../common/validator";
import { TravelMatrixService } from 'src/app/_Services/invoice/travelmatrix.service';
import { ResponseResult, DataSource, CityDataSourceRequest } from 'src/app/_Models/common/common.model';
import { first, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { CustomerResult } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { JsonHelper } from '../../common';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs/internal/Observable';
import { Subject, concat, of, observable } from 'rxjs';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';


@Component({
  selector: 'app-travel-matrix',
  templateUrl: './travel-matrix.component.html',
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
export class TravelMatrixComponent extends SummaryComponent<TravelMatrixSummary> {
  masterModel: TravelMatrixMasterModel;
  model: TravelMatrixSummary;
  itemModel: Array<TravelMatrixModel>;
  existsItemModel: Array<TravelMatrixExists>;
  provinceList: Array<ProvinceList>;
  cityList: Array<CityList>;
  countyList: Array<CountyList>;
  _tariffType = TariffType;
  travelMatrixValidators: Array<any>;
  isTravelDataExist: boolean;
  provinceDataList: Array<AreaData>;
  cityDataList: Array<AreaData>;
  countyDataList: Array<AreaData>;
  selectedPageSize;
  pagesizeitems = PageSizeCommon;
  toggleFormSection: boolean;
  isFilterOpen: boolean;
  private modelRef: NgbModalRef;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  searchClicked: boolean;
  searchTypeList: any = travelMatrixSearchTypeList;
  travelMatrixSearchEnum = TravelMatrixSearchEnum;

  @ViewChild('scrollableTable') scrollableTable: ElementRef;
  @ViewChild('existsitem') existsitempopup: ElementRef;

  constructor(router: Router, validator: Validator, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    public travelMatrixService: TravelMatrixService, public utility: UtilityService, public bookingService: BookingService, public locationService: LocationService,
    private jsonHelper: JsonHelper, private modalService: NgbModal) {

    super(router, validator, route, translate, toastr);

    this.provinceList = new Array<ProvinceList>();
    this.cityList = new Array<CityList>();
    this.countyList = new Array<CountyList>();

    this.provinceDataList = new Array<AreaData>();
    this.cityDataList = new Array<AreaData>();
    this.countyDataList = new Array<AreaData>();
    this._toastr = toastr;
    this._translate = translate;
    this.isFilterOpen = true;
    this.searchClicked = false;

  }

  onInit() {

    this.masterModel = new TravelMatrixMasterModel();
    this.model = new TravelMatrixSummary();
    this.itemModel = new Array<TravelMatrixModel>();
    this.existsItemModel = new Array<TravelMatrixExists>();
    this.travelMatrixValidators = [];
    this.isTravelDataExist = false;
    this.model.searchTypeId = TravelMatrixSearchEnum.County;

    this.selectedPageSize = PageSizeCommon[0];
    this.getCustomerList();
    this.getCountryList();
    this.getCurrencyList();

    this.validator.isSubmitted = false;
    this.validator.setJSON("invoice/travel-matrix-search.valid.json");
    this.validator.setModelAsync(() => this.model);
  }

  getPathDetails(): string {
    return "";
  }


  getData(): void {

    this.searchClicked = true;
    this.GetSearchData();
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    console.log(this.formValid())
    if (this.formValid()) {
      console.log(this.selectedPageSize)
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  formValid(): boolean {
    return this.validator.isValid('countryId');
  }

  GetSearchData() {
    this.masterModel.searchLoading = true;
    this.travelMatrixValidators = [];
    this.isTravelDataExist = false;
    this.itemModel = [];
    this.validator.isSubmitted = false;
    // let response: TravelMatixSearchResponse;
    this.model.isExport = false;
    this.travelMatrixService.search(this.model).
      pipe()
      .subscribe(
        response => {
          if (response) {
            this.mapPageProperties(response);

            switch (response.result) {

              case TravelMatrixResponseResult.Success:
                this.mapSearchData(response);
                break;
              case TravelMatrixResponseResult.DefaultData:
                this.mapSearchData(response);
                break;
              case TravelMatrixResponseResult.RequestNotCorrectFormat:
                this.showError('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_REQUEST_FORMAT');
                this.masterModel.searchLoading = false;
                this.model.noFound = true;
                break;
              case TravelMatrixResponseResult.Error:
                this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                this.masterModel.searchLoading = false;
                this.model.noFound = true;
                break;
              case TravelMatrixResponseResult.NotFound:
                this.masterModel.searchLoading = false;
                this.model.noFound = true;
                break;
            }

            setTimeout(() => {
              this.limitTableWidth();
            }, 3000);
          }


        },
        error => {
          this.setError(error);
          this.masterModel.searchLoading = false;

        }
      )

    // try {
    //   response =  this.travelMatrixService.search(this.model);
    // }
    // catch (e) {
    //   console.error(e);
    //   this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
    //   this.masterModel.searchLoading = false;
    // }


  }

  async mapSearchData(response: TravelMatixSearchResponse) {
    await this.getProviceLists(response.getData);
    await this.getCityLists(response.getData);
    await this.getCountyLists(response.getData)

    if (response.getData && response.getData.length > 0) {
      this.model.noFound = false;
      this.isTravelDataExist = response.isDataExist;
      response.getData.forEach(element => {
        var getProvince = this.provinceDataList.find(x => x.id == element.countryId);

        var getCity = this.cityDataList.find(x => x.id == element.provinceId);

        var getCounty = this.countyDataList.find(x => x.id == element.cityId);


        if (getProvince != null) {
          element.provinceList = getProvince.dataList;
        }

        if (getCity != null) {
          element.cityList = getCity.dataList;
        }

        if (getCounty != null) {
          element.countyList = getCounty.dataList;
        }

        // set selectd value for portName
        if (element.inspPortId && element["inspPortName"]) {
          element.selectedPortList = { id: element.inspPortId, name: element["inspPortName"] }
        }

        if (element.inspPortCityId && element["inspPortCityName"]) {
          element.selectedPortCityList = { id: element.inspPortCityId, name: element["inspPortCityName"] }
        }


        if (this.provinceList.findIndex(x => x.countryId == element.countryId) == -1) {
          this.pushAreaDataBytype(element.countryId, element.provinceList, AreaType.country);
        }

        if (this.cityList.findIndex(x => x.provinceId == element.provinceId) == -1) {
          this.pushAreaDataBytype(element.provinceId, element.cityList, AreaType.province);
        }

        if (this.countyList.findIndex(x => x.cityId == element.cityId) == -1) {
          this.pushAreaDataBytype(element.cityId, element.countyList, AreaType.city);
        }

        if (response.result == TravelMatrixResponseResult.DefaultData &&
          (element.id == null || element.id <= 0)) {
          element.tariffTypeId = TariffType.standardA;
        }

        this.loadInspPortList(element);

        this.loadInspPortCityList(element);
        this.travelMatrixValidators.push({
          travelMatrix: element,
          validator: Validator.getValidator(element, "invoice/travel-matrix-save.valid.json",
            this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)

        })
        this.itemModel.push(element);
      });
    }
    else {
      this.model.noFound = true;
    }
    this.masterModel.searchLoading = false;
  }

  //while edit get provice list 
  async getProviceLists(data: TravelMatrixModel[]) {
    let countryIds = data.map(x => x.countryId);

    //take distinct value
    let distinctCountryIdArray = countryIds.filter((n, i) => countryIds.indexOf(n) === i);

    try {

      let response = await this.travelMatrixService.getProvinceLists(distinctCountryIdArray);
      this.getProviceListsSuccessResponse(response, data);

    }
    catch (e) {
      console.error(e);
      this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
    }
  }

  getProviceListsSuccessResponse(response, data) {
    if (response) {
      switch (response.result) {
        case TravelMatrixResponseResult.Success:
          this.provinceDataList = response.areaDataList;
          break;
        case TravelMatrixResponseResult.NotFound:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.LBL_NOITEMS');
          break;
        case TravelMatrixResponseResult.RequestNotCorrectFormat:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_REQUEST_FORMAT');
          break;
        case TravelMatrixResponseResult.Error:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
      }
    }
  }

  // Auto complete ajax method for portList
  private loadInspPortList(item: TravelMatrixModel) {

    item.typeInspPortListInput = new Subject<string>();

    item.inspPortList = concat(
      of([]), // default items
      item.typeInspPortListInput.pipe(
        distinctUntilChanged(),
        tap(() => item.inspPortLoading = true),
        switchMap(term =>

          this.travelMatrixService.getCountyListByCountry(term, item.countryId).pipe(
            catchError(() => of([])), // empty list on error
            tap(() => item.inspPortLoading = false)
          ))
      )
    );

  }

  //while edit get city list 
  async getCityLists(data: TravelMatrixModel[]) {
    let provinceIds = data.map(x => x.provinceId);
    let distinctProvinceIdArray = provinceIds.filter((n, i) => provinceIds.indexOf(n) === i);
    try {
      let response = await this.travelMatrixService.getCityLists(distinctProvinceIdArray);
      this.getCityListsSuccessResponse(response);

    }
    catch (e) {
      console.error(e);
      this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
    }



  }

  getCityListsSuccessResponse(response) {
    if (response) {
      switch (response.result) {
        case TravelMatrixResponseResult.Success:
          this.cityDataList = response.areaDataList;
          break;
        case TravelMatrixResponseResult.NotFound:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.LBL_NOITEMS');
          break;
        case TravelMatrixResponseResult.RequestNotCorrectFormat:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_REQUEST_FORMAT');
          break;
        case TravelMatrixResponseResult.Error:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
      }
    }
  }

  //Auto complete ajax method for portList(city)
  //fetch city dropdown list
  loadInspPortCityList(item: TravelMatrixModel) {

    item.typeInspPortCityListInput = new Subject<string>();

    item.cityRequest = new CityDataSourceRequest();

    item.cityRequest.cityIds = [];

    item.inspPortCityList = concat(
      of([]), // default items
      item.typeInspPortCityListInput.pipe(
        distinctUntilChanged(),
        tap(() => item.inspPortCityLoading = true),
        switchMap(term =>

          this.locationService.getCityDataSourceList(item.cityRequest, term).pipe(
            catchError(() => of([])), // empty list on error
            tap(() => item.inspPortCityLoading = false)
          ))
      )
    );

  }

  //while edit get county list 
  async getCountyLists(data: TravelMatrixModel[]) {
    let cityIds = data.filter(x => x.cityId).map(x => x.cityId);
    if (cityIds) {
      let distinctCityIdArray = cityIds.filter((n, i) => cityIds.indexOf(n) === i);

      try {
        let response = await this.travelMatrixService.getCountyLists(distinctCityIdArray);
        this.getCountyListsSuccessResponse(response);
      }
      catch (e) {
        console.error(e);
        this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    }
  }

  //
  getCountyListsSuccessResponse(response) {
    if (response) {
      switch (response.result) {
        case TravelMatrixResponseResult.Success:
          this.countyDataList = response.areaDataList;
          break;
        case TravelMatrixResponseResult.NotFound:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.LBL_NOITEMS');
          break;
        case TravelMatrixResponseResult.RequestNotCorrectFormat:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_REQUEST_FORMAT');
          break;
        case TravelMatrixResponseResult.Error:
          this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
      }
    }
  }
  //add one row in the table
  rowAddDefault() {
    var model: TravelMatrixModel = new TravelMatrixModel();

    this.itemModel.push(model);

    this.loadInspPortList(model);

    this.loadInspPortCityList(model);
    this.validator.isSubmitted = false;

    this.travelMatrixValidators.unshift({
      travelMatrix: model,
      validator: Validator.getValidator(model, "invoice/travel-matrix-save.valid.json",
        this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
    });

    setTimeout(() => {
      this.limitTableWidth();
    }, 500);
    this.model.noFound = false;

  }

  //
  selectAllTravelMatrix() {

    this.itemModel.forEach(element => {
      element.isChecked = this.masterModel.selectedAllCheckBox;
    });

    this.masterModel.showButton = this.isSelected() != null ? true : false;
  }

  //
  travelMatrixItemSelect() {
    this.masterModel.selectedAllCheckBox = this.itemModel.every(function (item: any) {
      return item.isChecked == true;
    });

    this.masterModel.showButton = this.isSelected() != null ? true : false;
  }

  //table scroll set
  limitTableWidth() {
    let width = this.scrollableTable ?
      this.scrollableTable.nativeElement.offsetWidth : 0;
    if (width > 0 && width < this.scrollableTable.nativeElement.scrollWidth) {
      this.scrollableTable.nativeElement.classList.add('scroll-x');
    }
  }


  //valid the form to save
  isFormValid(): boolean {

    let isOk = false;

    if (!(this.travelMatrixValidators.find(x => x.travelMatrix.isChecked))) {
      this.showWarning('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_SELECT_CHECKBOX')

    }

    for (let item of this.travelMatrixValidators.filter(x => x.travelMatrix.isChecked)) {

      isOk = item.validator.isValid('travelCurrencyId') &&
        item.validator.isValid('sourceCurrencyId') &&
        item.validator.isValid('fixExchangeRate');

      if (this.model.searchTypeId == this.travelMatrixSearchEnum.City)
        isOk = item.validator.isValid('cityId');

      if (this.model.searchTypeId == this.travelMatrixSearchEnum.County)
        isOk = item.validator.isValid('countyId');

      if (isOk) {

        if ((item.travelMatrix.busCost == null || item.travelMatrix.busCost <= 0) &&
          (item.travelMatrix.trainCost == null || item.travelMatrix.trainCost <= 0) &&
          (item.travelMatrix.otherCost == null || item.travelMatrix.otherCost <= 0) &&
          (item.travelMatrix.hotelCost == null || item.travelMatrix.hotelCost <= 0) &&
          (item.travelMatrix.taxiCost == null || item.travelMatrix.taxiCost <= 0) &&
          (item.travelMatrix.airCost == null || item.travelMatrix.airCost <= 0)) {
          isOk = false;
          this.showWarning('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_COST_REQUIRED');
          break;
        }
      }

      if (isOk) {
        //drop down change will handle below validation
        if (item.travelMatrix.tariffTypeId == TariffType.standardA || item.travelMatrix.tariffTypeId == TariffType.standardB) {
          if (item.travelMatrix.customerId > 0) {
            isOk = false;
            this.showWarning('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_STANDARD_CUSTOMER_NOT_SELECTED')
            break;
          }
        }
      }
      if (isOk) {
        if (item.travelMatrix.tariffTypeId == TariffType.customized) {
          if (!item.validator.isValid('customerId')) {
            isOk = false;
            break;
          }
        }
      }

      if (isOk) {
        if (!item.travelMatrix.inspPortCityId && !item.travelMatrix.inspPortId) {

          isOk = false;
          this.showWarning('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_EMPTY_SELECT_INSP_PORT')
          break;

        }
      }

    }

    return isOk;
  }
  async save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    this.travelMatrixValidators.forEach(item => {
      if (item.travelMatrix.isChecked) {
        item.validator.isSubmitted = true;
      }
    });

    if (this.isFormValid()) {
      this.validator.isSubmitted = false;

      this.masterModel.saveLoading = true;
      this.itemModel = [];
      this.travelMatrixValidators.forEach(element => {
        if (element.travelMatrix.isChecked) {
          element.travelMatrix['inspPortList'] = null;
          element.travelMatrix['inspPortCityList'] = null;
          element.travelMatrix['typeInspPortListInput'] = null;
          element.travelMatrix['typeInspPortCityListInput'] = null;
          element.travelMatrix['inspPortId'] = element.travelMatrix["selectedPortList"] ? element.travelMatrix["selectedPortList"].id : null;
          element.travelMatrix['inspPortCityId'] = element.travelMatrix["selectedPortCityList"] ? element.travelMatrix["selectedPortCityList"].id : null;
          this.itemModel.push(element.travelMatrix);
        }

      });
      let response: TravelMatixSaveResponse;
      try {
        response = await this.travelMatrixService.save(this.itemModel);
      }
      catch (e) {
        console.error(e);
        this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterModel.saveLoading = false;
      }

      if (response) {

        switch (response.result) {
          case TravelMatrixResponseResult.Success:
            this.existsPopupShow(response.existsData);
            this.resetData();
            this.showSuccess('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_SAVED');
            break;
          case TravelMatrixResponseResult.Error:
            this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
            break;
          case TravelMatrixResponseResult.NotFound:
            this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.LBL_NOITEMS');
            break;
          case TravelMatrixResponseResult.RequestNotCorrectFormat:
            this.showError('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_REQUEST_FORMAT');
            break;
        }
      }
      this.masterModel.saveLoading = false;
    }
  }

  //exists check
  existsPopupShow(data: Array<TravelMatrixExists>) {

    this.existsItemModel = data;
    if (this.existsItemModel && this.existsItemModel.length > 0) {
      this.modelRef = this.modalService.open(this.existsitempopup, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }
    else {
      this.GetSearchData();
    }
  }

  add() {

    this.rowAddDefault();
  }
  delete() {

    this.masterModel.deleteLoading = true;

    this.travelMatrixService.delete(this.itemModel.filter(x => x.isChecked).map(x => x.id))
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.showSuccess('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
              this.resetData();
            }
            else {
              this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
            }
            this.masterModel.deleteLoading = false;
          }
          this.modelRef.close();
          this.GetSearchData();
        },
        error => {
          this.setError(error);
          this.masterModel.deleteLoading = false;
        });
  }

  resetData() {
    this.masterModel.selectedAllCheckBox = false;
    this.masterModel.showButton = false;
  }

  isSelected() {
    return this.itemModel.find(x => x.isChecked);
  }

  toggleExpandRow(event, index, rowItem) {

    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="cost' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
    }
    else {
      event.target.innerHTML = '+';
    }
  }

  //get tariff types list
  getTarifftypes() {
    this.masterModel.tariffTypeLoading = true;

    this.travelMatrixService.getTarifftypes()
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.masterModel.tariffTypeList = response.dataSourceList;
              this.model.tariffTypeId = TariffType.standardA;
            }
            this.masterModel.tariffTypeLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.tariffTypeLoading = false;
        });
  }


  //get customer list
  getCustomerList() {
    this.masterModel.customerLoading = true;
    this.bookingService.GetCustomerByUserType().subscribe(
      response => {
        this.getCustomerListResponse(response);
      },
      error => {
        this.showError('TRAVEL_MATRIX.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterModel.customerLoading = false;
      }
    );
  }

  //customer list success response
  getCustomerListResponse(response) {
    if (response) {
      if (response.result == CustomerResult.Success) {

        this.masterModel.customerList = response.customerList.map((x) => {
          var item: DataSource = {
            id: x.id,
            name: x.name
          }
          return item;
        });

      }
      else if (response.result == CustomerResult.CannotGetCustomerList) {
        this.showError('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_CUSTOMER_LIST_NOT_FOUND');
      }
    }
    this.masterModel.customerLoading = false;
  }

  //get currency list
  getCurrencyList() {
    this.masterModel.travelCurrencyLoading = true;
    this.masterModel.sourceCurrencyLoading = true;
    this.bookingService.GetCurrency()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {

            this.masterModel.travelCurrencyList = response.currencyList.map((x) => {
              var item: DataSource = {
                id: x.id,
                name: x.currencyName
              }
              return item;
            });
            this.masterModel.sourceCurrencyList = response.currencyList.map((x) => {
              var item: DataSource = {
                id: x.id,
                name: x.currencyName
              }
              return item;
            });
            // this.model.sourceCurrencyId = 156;
          }
          this.masterModel.travelCurrencyLoading = false;
          this.masterModel.sourceCurrencyLoading = false;
          this.getTarifftypes();

        },
        error => {
          this.setError(error);
          this.masterModel.travelCurrencyLoading = false;
          this.masterModel.sourceCurrencyLoading = false;
        });
  }

  //get country list
  getCountryList() {
    this.masterModel.countryLoading = true;
    this.locationService.getCountrySummary()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)

            this.masterModel.countryList = response.countryList.map((x) => {
              var item: DataSource = {
                id: x.id,
                name: x.countryName
              }
              return item;
            });
          this.masterModel.countryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.countryLoading = false;
        });
  }

  getProvinceList(id: number) {

    this.masterModel.provinceLoading = true;

    this.locationService.getprovincebycountryid(id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {

            this.masterModel.provinceList = response.data.map((x) => {
              var item: DataSource = {
                id: x.id,
                name: x.name
              }
              return item;
            });
            this.pushAreaDataBytype(id, this.masterModel.provinceList, AreaType.country);
          }
          else if (response.result == ResponseResult.NoDataFound) {
            this.pushAreaDataBytype(id, null, AreaType.country);
          }
          this.masterModel.provinceLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.provinceLoading = false;
        });
  }

  //item province list get
  getProvinceItemList(id: number, item: TravelMatrixModel) {

    item.provinceLoading = true;

    this.locationService.getprovincebycountryid(id)
      .pipe()
      .subscribe(
        response => {
          if (response) {

            if (response.result == ResponseResult.Success) {

              item.provinceList = response.data.map((x) => {
                var item: DataSource = {
                  id: x.id,
                  name: x.name
                }
                return item;
              });

              this.pushAreaDataBytype(id, item.provinceList, AreaType.country);
            }
            else if (response.result == ResponseResult.NoDataFound) {
              this.pushAreaDataBytype(id, null, AreaType.country);
            }
          }

          item.provinceLoading = false;
        },
        error => {
          this.setError(error);
          item.provinceLoading = false;
        });
  }

  //get province list on country change
  setProvinceCountryChange(countryEvent) {

    this.masterModel.provinceList = null;
    this.model.provinceId = null;

    this.masterModel.cityList = null;
    this.model.cityId = null;

    this.masterModel.countyList = null;
    this.model.countyId = null;

    if (countryEvent && countryEvent.id > 0) {

      var tempProvinceList = this.getTempProvinceList(countryEvent.id);

      if (tempProvinceList) {

        if (tempProvinceList.countryId > 0) //  this.masterModel.provinceList == null|| !(this.masterModel.provinceList.length > 0)
          this.masterModel.provinceList = tempProvinceList.provinceList;
        else
          this.getProvinceList(countryEvent.id);
      }
      else {
        this.getProvinceList(countryEvent.id);
      }
    }

  }

  //country id exists in temp list we are taking from client side 
  getTempProvinceList(id: number) {

    //
    let tempProvinceList = this.provinceList && this.provinceList.length > 0 ?
      this.provinceList.find(x => x.countryId == id) : null;

    return tempProvinceList;

  }

  //get province list on country change
  setProvinceCountryItemChange(countryEvent, item: TravelMatrixModel) {

    item.provinceList = null;
    item.provinceId = null;

    item.cityList = null;
    item.cityId = null;

    item.countyList = null;
    item.countyId = null;

    if (countryEvent && countryEvent.id > 0) {

      var tempProvinceList = this.getTempProvinceList(countryEvent.id);

      if (tempProvinceList) {

        if (tempProvinceList.countryId > 0)
          item.provinceList = tempProvinceList.provinceList;
        else
          this.getProvinceItemList(countryEvent.id, item);

      }
      else {
        this.getProvinceItemList(countryEvent.id, item);
      }
    }
  }

  getCityList(id: number) {

    this.masterModel.cityLoading = true;

    this.locationService.getcitybyprovinceid(id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {

            this.masterModel.cityList = response.data.map((x) => {
              var item: DataSource = {
                id: x.id,
                name: x.name
              }
              return item;
            });

            this.pushAreaDataBytype(id, this.masterModel.cityList, AreaType.province);
          }
          else if (response.result == ResponseResult.NoDataFound) {
            this.pushAreaDataBytype(id, null, AreaType.province);
          }
          this.masterModel.cityLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.cityLoading = false;
        });
  }

  //item province list 
  getCityItemList(id: number, item: TravelMatrixModel) {

    item.cityLoading = true;

    this.locationService.getcitybyprovinceid(id)
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              item.cityList = response.data.map((x) => {
                var item: DataSource = {
                  id: x.id,
                  name: x.name
                }
                return item;
              });

              this.pushAreaDataBytype(id, item.cityList, AreaType.province);
            }
            else if (response.result == ResponseResult.NoDataFound) {
              this.pushAreaDataBytype(id, null, AreaType.province);
            }
          }

          item.cityLoading = false;
        },
        error => {
          this.setError(error);
          item.cityLoading = false;
        });
  }

  //get city list on province change
  setCityProvinceChange(provinceEvent) {

    this.masterModel.cityList = null;
    this.model.cityId = null;

    this.masterModel.countyList = null;
    this.model.countyId = null;

    if (provinceEvent && provinceEvent.id > 0) {

      var tempCityList = this.getCityTempList(provinceEvent.id);

      if (tempCityList) {

        if (tempCityList.provinceId > 0)
          this.masterModel.cityList = tempCityList.cityList;
        else
          this.getCityList(provinceEvent.id);
      }
      else {
        this.getCityList(provinceEvent.id);
      }
    }

  }

  //
  getCityTempList(id: number) {

    //
    let tempList = this.cityList && this.cityList.length > 0 ?
      this.cityList.find(x => x.provinceId == id) : null;
    return tempList;

  }

  //get city list on province change
  setCityProvinceItemChange(Event, item: TravelMatrixModel) {

    item.cityList = null;
    item.cityId = null;

    item.countyList = null;
    item.countyId = null;

    if (Event && Event.id > 0) {

      var tempCityList = this.getCityTempList(Event.id);

      if (tempCityList) {

        if (tempCityList.provinceId > 0)
          item.cityList = tempCityList.cityList;
        else
          this.getCityItemList(Event.id, item);

      }
      else {
        this.getCityItemList(Event.id, item);
      }
    }
  }



  getCountyList(id: number) {

    this.masterModel.countyLoading = true;

    this.locationService.getcountybycity(id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {

            this.masterModel.countyList = response.dataList.map((x) => {
              var item: DataSource = {
                id: x.id,
                name: x.countyName
              }
              return item;
            });
            this.pushAreaDataBytype(id, this.masterModel.countyList, AreaType.city);
          }
          else if (response.result == ResponseResult.NoDataFound) {
            this.pushAreaDataBytype(id, null, AreaType.city);
          }
          this.masterModel.countyLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.countyLoading = false;
        });
  }

  //item province list 
  getCountyItemList(id: number, item: TravelMatrixModel) {

    item.countyLoading = true;
    item.inspPortLoading = true;

    this.locationService.getcountybycity(id)
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {

              item.countyList = response.dataList.map((x) => {
                var item: DataSource = {
                  id: x.id,
                  name: x.countyName
                }
                return item;
              });
              this.pushAreaDataBytype(id, item.countyList, AreaType.city);
            }
            else if (response.result == ResponseResult.NoDataFound) {
              this.pushAreaDataBytype(id, null, AreaType.city);
            }
          }

          item.countyLoading = false;
          item.inspPortLoading = false;
        },
        error => {
          this.setError(error);
          item.countyLoading = false;
          item.inspPortLoading = false;
        });
  }

  onPortChange(event, currentModel: TravelMatrixModel) {
    currentModel.inspPortCityId = null;
    currentModel.selectedPortCityList = null;
    if (event && event.id > 0) {
      currentModel.inspPortId = event.id;
    }
  }

  onPortCityChange(event, currentModel: TravelMatrixModel) {
    currentModel.inspPortId = null;
    currentModel.selectedPortList = null;
    if (event && event.id > 0) {
      currentModel.inspPortCityId = event.id;
    }
  }

  //get county list on city change
  setCountyCityChange(Event) {

    this.masterModel.countyList = null;
    this.model.countyId = null;

    if (Event && Event.id > 0) {

      var tempCountyList = this.getCountyTempList(Event.id);

      if (tempCountyList) {

        if (tempCountyList.cityId > 0) {
          this.masterModel.countyList = tempCountyList.countyList;
        }
        else
          this.getCountyList(Event.id);
      }
      else {
        this.getCountyList(Event.id);
      }
    }

  }

  //
  getCountyTempList(id: number) {

    //
    let tempCountyList = this.countyList && this.countyList.length > 0 ?
      this.countyList.find(x => x.cityId == id) : null;

    return tempCountyList;

  }

  //get count list on city change
  setCountyCityItemChange(countyEvent, item: TravelMatrixModel) {

    item.countyList = null;
    item.countyId = null;


    if (countyEvent && countyEvent.id > 0) {

      var tempCountyList = this.getCountyTempList(countyEvent.id);

      if (tempCountyList) {

        if (tempCountyList.cityId > 0) {
          item.countyList = tempCountyList.countyList;
        }
        else
          this.getCountyItemList(countyEvent.id, item);

      }
      else {
        this.getCountyItemList(countyEvent.id, item);
      }
    }
  }

  //get data and assign with appropriate list(temp list in client side) based on type
  pushAreaDataBytype(id: number, list: Array<DataSource>, type: number) {

    if (type == AreaType.country) {
      let data: ProvinceList = { countryId: id, provinceList: list };
      this.provinceList.push(data);
    }
    else if (type == AreaType.province) {
      let data: CityList = { provinceId: id, cityList: list };
      this.cityList.push(data);
    }
    else if (type == AreaType.city) {
      let data: CountyList = { cityId: id, countyList: list };
      this.countyList.push(data);
    }

  }

  customerChange() {
    this.model.tariffTypeId = TariffType.customized;
  }

  customerItemChange(item: TravelMatrixModel) {
    item.tariffTypeId = TariffType.customized;
  }

  tariffTypeChange(data) {
    if (data && data.id != TariffType.customized) {
      this.model.customerId = null;
    }
  }

  tariffTypeItemChange(item: TravelMatrixModel, event) {
    // item.
    if (event && event.id != TariffType.customized) {
      item.customerId = null;
    }
  }

  //if any loading true, disbale the button
  buttonDisable(): boolean {
    return (this.masterModel.saveLoading ||
      this.masterModel.deleteLoading ||
      this.masterModel.searchLoading ||
      this.masterModel.tariffTypeLoading ||
      this.masterModel.countryLoading ||
      this.masterModel.provinceLoading ||
      this.masterModel.cityLoading ||
      this.masterModel.countyLoading ||
      this.masterModel.customerLoading ||
      this.masterModel.travelCurrencyLoading ||
      this.masterModel.sourceCurrencyLoading ||
      this.masterModel.exportDataLoading);
  }
  export() {
    this.masterModel.exportDataLoading = true;
    this.model.isExport = true;
    this.travelMatrixService.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.masterModel.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "travelmatrix_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "travelmatrix_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.masterModel.exportDataLoading = false;
  }

  //export button visiable
  isExportVisible(): boolean {
    return (this.travelMatrixValidators && this.travelMatrixValidators.length > 0 && this.isTravelDataExist)
  }

  reset() {
    this.onInit();
    this.resetData();
  }

  openPopup(content) {
    if (!(this.itemModel.find(x => x.isChecked && x.id > 0))) {
      this.showWarning('TRAVEL_MATRIX.LBL_TITLE', 'TRAVEL_MATRIX.MSG_SELECT_CHECKBOX_SAVED_DATA');
    }
    else {
      this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
    }
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  SetSearchtype(item) {
    this.model.searchTypeId = item.id;
    this.travelMatrixValidators = [];
  }
  // copy() {
  // }
}

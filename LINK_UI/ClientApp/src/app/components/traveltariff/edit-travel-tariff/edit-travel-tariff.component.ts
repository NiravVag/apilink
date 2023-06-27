import { Component } from '@angular/core';
import { DetailComponent } from '../../common/detail.component';
import { JsonHelper, Validator } from '../../common';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { TravelTariffService } from 'src/app/_Services/traveltariff/traveltariff.service';
import { TravelTariffGetResponse, TravelTariffGetResult, TravelTariffResponseResult, TravelTariffSaveRequest, TravelTariffSaveResponse } from 'src/app/_Models/traveltariff/traveltariff';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ResponseResult, TownDataSourceRequest } from 'src/app/_Models/common/common.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { BehaviorSubject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject } from 'rxjs';

@Component({
  selector: 'app-edit-travel-tariff',
  templateUrl: './edit-travel-tariff.component.html',
  styleUrls: ['./edit-travel-tariff.component.scss']
})
export class EditTravelTariffComponent extends DetailComponent {

  private _translate: TranslateService;
  private _toastr: ToastrService;
  public model: TravelTariffSaveRequest;
  public traveltariffValidators: Array<any> = [];
  public startSpotList: Array<any> = [];
  public currencyList: Array<any> = [];
  townList: any[];
  townInput: BehaviorSubject<string>;
  townLoading: boolean;
  townRequest: TownDataSourceRequest;
  public jsonHelper: JsonHelper;
  public pageLoading: boolean = false;
  public saveloading: boolean = false;
  componentDestroyed$: Subject<boolean> = new Subject();
  countryLoading: boolean;
  countryList: any[];
  cityList: any[];
  countyList: any[];
  provinceLoading: boolean;
  provinceList: any[];
  cityLoading: boolean;
  countyLoading: boolean;

  constructor
    (
    public validator: Validator, translate: TranslateService, toastr: ToastrService, router: Router,
    route: ActivatedRoute, private activatedRoute: ActivatedRoute, public utility: UtilityService,
    public service: TravelTariffService,
    public refService: ReferenceService, public locationService: LocationService
  ) {
    super(router, route, translate, toastr);

    this._toastr = toastr;
    this._translate = translate;
    this.townInput = new BehaviorSubject<string>("");
    this.townRequest = new TownDataSourceRequest();
    this.model = new TravelTariffSaveRequest();
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "startDate": {
        this.model.startDate = null;
        break;
      }
      case "endDate": {
        this.model.endDate = null;
        break;
      }
    }
  }

  onInit(id?: any) {
    this.pageLoading = true;
    this.getStartportList();
    this.initValidator();
    this.getCurrencyList();
    this.getCountry();
    if (id > 0) {
      this.GetTravelTariffById(id);
      this.pageLoading = false;
    }
    else {
      this.pageLoading = false;
    }
  }

  async GetTravelTariffById(id) {
    let response: TravelTariffGetResponse;
    try {
      response = await this.service.getTravelTariff(id);
      switch (response.result) {
        case TravelTariffGetResult.Success:
          if (response.travelTariff.countryId > 0) {
            this.getProvince(response.travelTariff.countryId);
          }
          if (response.travelTariff.provinceId > 0) {
            this.refreshcity(response.travelTariff.provinceId)
          }
          if (response.travelTariff.cityId > 0) {
            this.refreshcounty(response.travelTariff.cityId)
          }
          if (response.travelTariff.countyId > 0) {
            this.refreshTown(response.travelTariff.countyId)
          }
          this.model = this.mapModel(response.travelTariff);

          if (this.model.townId > 0) {
            this.townRequest.townIds = [this.model.townId];
          }
          break;

        case TravelTariffGetResult.NotFound:
          this.model = new TravelTariffSaveRequest();
          break;

        default:
          break;
      }
      this.pageLoading = false;
    }
    catch (e) {
      this.pageLoading = false;
      this.setError(e);
    }
  }

  mapModel(travelTariff: any): TravelTariffSaveRequest {
    var model: TravelTariffSaveRequest = {

      id: travelTariff.id,
      townId: travelTariff.townId,
      startDate: travelTariff.startDate,
      endDate: travelTariff.endDate,
      startPort: travelTariff.startPortId,
      travelCurrency: travelTariff.travelCurrency,
      travelTariff: travelTariff.travelTariff,
      countryId: travelTariff.countryId,
      provinceId: travelTariff.provinceId,
      cityId: travelTariff.cityId,
      countyId: travelTariff.countyId,
    };
    return model;
  }

  getAddPath() {
    return "";
  }

  getEditPath() {
    return "";
  }
  getViewPath() {
    return "";
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  //  resetTravelTariff(){

  //   this.traveltariffValidators=[];
  //   this.validator.isSubmitted=false;
  // }

  initValidator() {
    this.validator.isSubmitted = false;
    this.validator.setJSON("traveltariff/edit-travel-tariff.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = this.validator.jsonHelper;
  }

  //get currency list
  getCurrencyList() {
    this.refService.getCurrencyList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.currencyList = response.dataSourceList;
          }

        },
        error => {
          this.setError(error);

        });
  }

  //get startport list
  async getStartportList() {
    var response = await this.service.getStartPortList();
    if (response && response.result == ResponseResult.Success) {
      this.startSpotList = response.dataSourceList;
    }

  }

  clearTown() {
    this.model.townId = null;
  }
  async save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.isFormValid()) {

      let response: TravelTariffSaveResponse;
      this.saveloading = true;
      try {
        if (this.model.id > 0) {
          response = await this.service.updateTravelTariff(this.model);
        }
        else {
          response = await this.service.saveTravelTariff(this.model);
        }
        this.saveloading = false;
        switch (response.result) {
          case TravelTariffResponseResult.Success:
            this.showSuccess('EDIT_TRAVEL_TARIFF.TITLE', 'EDIT_TRAVEL_TARIFF.SAVE_OK');
            this.return('traveltariffsummary/travel-tariff');
            break;
          case TravelTariffResponseResult.AllreadyExist:
            this.showError('EDIT_TRAVEL_TARIFF.TITLE', 'EDIT_TRAVEL_TARIFF.LBL_TRAVEL_TARIFF_EXIST');
            break;
          default:
            break;
        }
      }
      catch (e) {
        this.saveloading = false;
        this.setError(e);
      }

    }

  }

  isFormValid() {

    return this.validator.isValid('startPort')
      && this.validator.isValid('townId')
      && this.validator.isValid('startDate')
      && this.validator.isValid('endDate')
      && this.validator.isValid('travelTariff')
      && this.validator.isValid('travelCurrency')
      && this.validator.isValid('countryId')
      && this.validator.isValid('provinceId')
      && this.validator.isValid('cityId')
      && this.validator.isValid('countyId')
  }

  getCountry() {
    this.countryLoading = true;
    this.locationService.getCountrySummary()
      .pipe()
      .subscribe(
        result => {
          this.countryList = result.countryList;
          this.countryLoading = false;
        }
      )
  }

  refreshprovince(countryid) {
    this.resetDetails();
    this.getProvince(countryid);
  }

  resetDetails() {
    this.model.provinceId = null;
    this.model.cityId = null;
    this.model.countryId = null;
    this.model.townId = null;
    this.cityList = null;
    this.countyList = null;
  }

  getProvince(countryid: number) {
    if (countryid) {
      this.provinceLoading = true;
      this.locationService.getprovincebycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.provinceList = result.data;
            }
            else {
              this.provinceList = [];
            }
            this.provinceLoading = false;
          },
          error => {
            this.provinceList = [];
            this.provinceLoading = false;
          });
    }
    else {
      this.provinceList = [];
    }
  }

  refreshcity(provinceId) {
    this.resetProvince();
    if (provinceId) {
      this.cityLoading = true;
      this.locationService.getcitybyprovinceid(provinceId)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.cityList = result.data;
            }
            else {
              this.cityList = [];
            }
            this.cityLoading = false;
          },
          error => {
            this.cityList = [];
            this.cityLoading = false;
          });
    }
    else {
      this.cityList = [];
    }
  }

  resetProvince() {
    this.model.cityId = null;
    this.model.countyId = null;
    this.model.townId = null;
    this.countyList = null;
    this.townList = null;
  }

  resetCity() {
    this.model.countyId = null;
    this.model.townId = null;
    this.townList = null;
  }

  refreshcounty(cityId) {
    this.model.countyId = null;
    this.model.townId = null;
    if (cityId) {
      this.countyLoading = true;
      this.locationService.getcountybycity(cityId)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.countyList = result.dataList;
            }
            else {
              this.countyList = [];
            }
            this.countyLoading = false;
          },
          error => {
            this.countyList = [];
            this.countyLoading = false;
          });
    }
    else {
      this.countyList = [];
    }
  }

  refreshTown(countyId) {
    this.model.townId = null;
    if (countyId) {
      this.townLoading = true;
      this.locationService.getTownsByCountyId(countyId)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.townList = result.dataSourceList;
            }
            else {
              this.townList = [];
            }
            this.townLoading = false;
          },
          error => {
            this.townList = [];
            this.townLoading = false;
          });
    }
    else {
      this.townList = [];
    }
  }

}

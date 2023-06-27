import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { EditCountymodel } from '../../../../_Models/location/edit-countymodel'
import { debounceTime, distinctUntilChanged, first, switchMap, tap,catchError } from 'rxjs/operators';
import { LocationService } from '../../../../_Services/location/location.service'
import { Validator } from '../../../common/validator'
import { ToastrService } from "ngx-toastr";
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../../common/detail.component';
import { CommonZoneSourceRequest, DataSource, ResponseResult } from 'src/app/_Models/common/common.model';
import { BehaviorSubject } from 'rxjs';
import { of } from 'rxjs';
import { UtilityService } from 'src/app/_Services/common/utility.service';


@Component({
  selector: 'app-edit-county',
  templateUrl: './edit-county.component.html',
  styleUrls: ['./edit-county.component.scss']
})
export class EditCountyComponent extends DetailComponent {
  onInit(id?: any): void {
    this.validator.setJSON('location/edit-county.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.initialize(id);
  }
  getViewPath(): string {
    return "county/view-county"
  }
  getEditPath(): string {
    return "county/edit-county"
  }
  public data: any = {};
  zoneList: any;
  zoneInput: BehaviorSubject<string>;
  public error: string = "";
  public model: EditCountymodel;
  loading = false;
  searchloading = false;
  provinceLoading: boolean = false;
  zoneLoading: boolean = false;
  cityLoading: boolean = false;
  requestZoneModel: CommonZoneSourceRequest;
  constructor(
    private service: LocationService,
    route: ActivatedRoute,
    public validator: Validator,
    translate: TranslateService,
    private notification: ToastrService,
    public utility: UtilityService,
    router: Router) {
    super(router, route, translate, notification);
  }
  initialize(id?) {
    this.model = new EditCountymodel();
    this.requestZoneModel=new CommonZoneSourceRequest();
    this.zoneInput = new BehaviorSubject<string>("");
    this.validator.isSubmitted = false;
    this.loading = true;
   
    this.getZoneListBySearch();
    setTimeout(() => {
      this.getCounty(id);
    }, 200);
   
  }

  getCounty(id?) {
    this.service.geteditCounty(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (id != null) {
              this.model = {
                id: res.countydetails.id,
                cityName: res.countydetails.cityName,
                countyName: res.countydetails.countyName,
                countyCode: res.countydetails.countyCode,
                cityId: res.countydetails.cityId,
                countryId: res.countydetails.countryId,
                provinceId: res.countydetails.provinceId,
                provinceName: res.countydetails.provinceName,
                zoneId: res.countydetails.zoneId,
                zoneName:""
              }
            }
            else {
              this.data.cityvalues = null;
            }
            this.loading = false;
          }
          else {
            this.error = res.result;
            this.loading = false;
          }
        },
        error => {
          this.error = error;
        }
      );
  }
  Formvalid(): boolean {
    return this.validator.isValid('cityId')
      && this.validator.isValid('countyName');
  }
  refreshprovince(countryid) {
    this.resetCountryDetails();
    if (countryid) {
      this.provinceLoading = true;
      this.service.getprovincebycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.provincevalues = result.data;
            }
            else {
              this.data.provincevalues = [];
            }
            this.provinceLoading = false;
          },
          error => {
            this.data.provincevalues = [];
            this.provinceLoading = false;
          });
    }
    else {
      this.data.provincevalues = [];
    }
  }
  refreshcity(provinceid) {
    this.model.cityId = null;
    if (provinceid) {
      this.cityLoading = true;
      this.service.getcitybyprovinceid(provinceid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.cityvalues = result.data;
            }
            else {
              this.data.cityvalues = [];
            }
            this.cityLoading = false;
          },
          error => {
            this.data.cityvalues = [];
            this.cityLoading = false;
          });
    }
    else {
      this.data.cityvalues = [];
    }
  }
  resetCountryDetails() {
    this.model.cityName = '';
    this.model.cityId = null;
    this.data.cityvalues = [];
    this.model.provinceId = null;
    this.data.provincevalues = [];
  }
  getZoneListBySearch() {
    this.requestZoneModel.zoneId = 0;
    this.zoneInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.zoneLoading = true),
      switchMap(term => term
        ? this.service.getZoneDataSourceList(this.requestZoneModel, term)
        : this.service.getZoneDataSourceList(this.requestZoneModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.zoneLoading = false))
      ))
      .subscribe(data => {
        this.zoneList = data;
        this.zoneLoading = false;
      });
  }
  getZoneData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
        this.requestZoneModel.searchText = this.zoneInput.getValue();
        this.requestZoneModel.skip = this.zoneList.length;
    }
    this.zoneLoading = true;
    this.service.getZoneDataSourceList(this.requestZoneModel).
        subscribe(data => {
            if (data && data.length > 0) {
                this.zoneList = this.zoneList.concat(data);
            }
            if (isDefaultLoad)
                this.requestZoneModel = new CommonZoneSourceRequest();
                this.zoneLoading = false;
        }),
        error => {
          this.zoneLoading = false;
            this.setError(error);
        };
}
clearZoneSelection()
{
  this.zoneList = null;
  this.getZoneListBySearch();
}


  save() {
    
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if(this.model.cityId)
    var name = this.data.cityvalues.find(x => x.id == this.model.cityId).name;
    if (this.Formvalid()) {
      this.loading = true;
      this.service.saveCounty(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.showSuccess("EDIT_COUNTY.TITLE", "EDIT_COUNTY.MSG_SAVE_SUCCESS");
              if (this.fromSummary)
                this.return("county/county-summary");
              else
                this.initialize();
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError("EDIT_COUNTY.TITLE", 'EDIT_COUNTY.MSG_CANNOT_SAVE_COUNTY');
                  break;
                case 3:
                  this.showError("EDIT_COUNTY.TITLE", 'EDIT_COUNTY.MSG_CURRENT_COUNTY_NOTFOUND');
                  break;
                case 4:
                  this.showError("EDIT_COUNTY.TITLE", 'EDIT_COUNTY.MSG_CANNOTMAPREQUEST');
                  break;
                case 5:
                  this.showError("EDIT_COUNTY.TITLE", this.model.countyName + " already exists for the City " + name);
                  break;
                case 6:
                  this.showError("EDIT_COUNTY.TITLE", `EDIT_COUNTY.MSG_LENGTH`);
                  break;
              }
            }
            this.loading = false;
          },
          error => {
            this.showError("EDIT_COUNTY.TITLE", "EDIT_COUNTY.MSG_UNKNONW_ERROR");
            this.loading = false;
          });
    }
  }
  Reset() {
    this.model.cityName = '';
    this.model.countyCode = null;
    this.model.countyName = null;
    this.model.provinceName = '';
    this.model.provinceId = null;
    this.model.cityId = null;
    this.model.countryId = null;
    this.data.cityvalues = null;
    this.data.provincevalues = null;
  }
}

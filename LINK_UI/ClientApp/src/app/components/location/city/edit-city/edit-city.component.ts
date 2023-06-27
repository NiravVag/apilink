import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { EditCitymodel } from '../../../../_Models/location/edit-citymodel'
import { first } from 'rxjs/operators';
import { LocationService } from '../../../../_Services/location/location.service'
import { NewOption } from '../../../common/static-data-common'
import { Validator } from '../../../common/validator';
import { ToastrService } from "ngx-toastr";
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-edit-city',
  templateUrl: './edit-city.component.html',
  styleUrls: ['./edit-city.component.css']
})
export class EditCityComponent extends DetailComponent {
  onInit(id?: any): void {
    this.newoption = NewOption;
    this.validator.setJSON('location/edit-city.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.Initialize(id);
  }
  getViewPath(): string {
    return ('city/view-city');
  }
  getEditPath(): string {
    return ('city/edit-city');
  }
  public model: EditCitymodel;
  public error: string = '';
  public data: any;
  public newoption: number;
  loading = false;
  constructor(
    private service: LocationService,
    route: ActivatedRoute,
    public validator: Validator,
    translate: TranslateService,
    public utility: UtilityService,
    notification: ToastrService, router: Router, ) {
    super(router, route, translate, notification);
  }
  Initialize(id?) {
    this.data = {};
    this.model = new EditCitymodel();
    this.validator.isSubmitted = false;
    this.service.geteditCity(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (id) {
              this.model = {
                id: res.cityDetails.id,
                name: res.cityDetails.name,
                countryid: res.cityDetails.countryId == null ? 0 : res.cityDetails.countryId,
                provinceid: res.cityDetails.provinceId == null ? 0 : res.cityDetails.provinceId,
                travelTimeHH: res.cityDetails.travelTimeHH,
                officeid: res.cityDetails.officeId == null ? 0 : res.cityDetails.officeId,
                phonecode: res.cityDetails.phoneCode,
                zoneid: res.cityDetails.zoneId == null ? 0 : res.cityDetails.zoneId
              }
            }
          }
          else {
            this.error = res.result;
          }
        },
        error => {
          this.error = error
        }
      );
  }
  refreshvalues(countryid) {
    this.refreshprovince(countryid);
  }
  refreshprovince(countryid) {
    if (countryid != null && countryid > 0) {
      this.service.getprovincebycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.provinceValues = result.data;
            }
            else {
              this.data.provinceValues = [];
              this.data.inspectionPortValues = [];
            }
          },
          error => {
            this.data.provinceValues = [];
          });
    }
    else {
      this.data.provinceValues = [];
      this.data.inspectionPortValues = [];
      this.model.provinceid = 0;
    }
  }
  IsFormValid(): boolean {
    return this.validator.isValid('name')
      && this.validator.isValid('provinceid')
      && this.validator.isValid('officeid')
      && this.validator.isValid('zoneid')
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.IsFormValid()) {
      this.loading = true;
      this.service.saveCity(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.showSuccess('EDIT_CITY.TITLE', 'EDIT_CITY.MSG_SAVE_SUCCESS');
              if (this.fromSummary)
                this.return("city/city-summary");
              else
                this.Initialize();
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CITY.TITLE', 'EDIT_CITY.MSG_CANNOT_SAVE_CITY');
                  break;
                case 3:
                  this.showError('EDIT_CITY.TITLE', 'EDIT_CITY.MSG_CURRENT_CITY_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_CITY.TITLE', 'EDIT_CITY.MSG_CANNOTMAPREQUEST');
                  break;
                case 5:
                  this.showError('EDIT_CITY.TITLE', 'EDIT_CITY.MSG_CITY_EXISTS');
                  break;
              }
            }
            this.loading = false;
          },
          error => {
            this.showError('EDIT_CITY.TITLE', 'EDIT_CITY.MSG_UNKNONW_ERROR');
            this.loading = false;
          });
    }
  }
  Reset() {
    this.model.countryid = 0;
    this.refreshvalues(0);
    this.model.name = '';
    this.model.travelTimeHH = null;
    this.model.officeid = 0;
    this.model.phonecode = '';
    this.model.zoneid = 0;
  }
}

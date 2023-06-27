import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { EditTownModel } from '../../../../_Models/location/edit-townmodel'
import { first } from 'rxjs/operators';
import { LocationService } from '../../../../_Services/location/location.service'
import { Validator } from '../../../common/validator'
import { ToastrService } from "ngx-toastr";
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-edit-town',
  templateUrl: './edit-town.component.html',
  styleUrls: ['./edit-town.component.scss']
})
export class EditTownComponent extends DetailComponent {
  onInit(id?: any): void {
    this.validator.setJSON('location/edit-town.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.initialize(id);
  }

  getEditPath(): string {
    return "town/edit-town"
  }

  getViewPath(): string {
    return "town/view-town"
  }

  public data: any = {};
  public error: string = "";
  public model: EditTownModel;
  loading = false;
  countryLoading : boolean = false;
  provinceLoading : boolean = false;
  cityLoading : boolean = false;
  countyLoading : boolean = false;
  countyName : string;

  constructor(
    private service: LocationService,
    route: ActivatedRoute,
    public validator: Validator,
    public utility: UtilityService,
    translate: TranslateService,
    private notification: ToastrService,
    router: Router) {
    super(router, route, translate, notification);
  }

  initialize(id?) {
    this.model = new EditTownModel();
    this.validator.isSubmitted = false;
    this.loading = true;
    this.getTown(id);
  }

  getTown(id?)
  {
    this.service.getEditTown(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (id != null) {
              this.model = {
                id: res.townDetails.id,
                countryId : res.townDetails.countryId,
                provinceId : res.townDetails.provinceId,
                cityId : res.townDetails.cityId,
                countyId : res.townDetails.countyId,
                townCode : res.townDetails.townCode,
                townName : res.townDetails.townName
              }
          }
          else {
            this.error = res.result;
          }
          this.loading = false;
          }
        },
        error => {
          this.error = error;
        }
      );
  }
  refreshprovince(countryid) {
    this.resetProvinceDetails();
    this.model.provinceId = null;
    if (countryid) {
      this.provinceLoading = true;
      this.service.getprovincebycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.provinceValues = result.data;
            }
            else {
              this.data.provinceValues = [];
            }
            this.provinceLoading = false;
          },
          error => {
            this.data.provinceValues = [];
            this.provinceLoading = false;
          });
    }
    else {
      this.data.provinceValues = [];
    }
  }
  refreshcity(provinceid) {
    if (provinceid) {
      this.cityLoading = true;
      this.resetProvinceDetails();
      //this.resetCountryDetails();
      this.service.getcitybyprovinceid(provinceid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.cityValues = result.data;
            }
            else {
              this.data.cityValues = [];
            }
            this.cityLoading = false;
          },
          error => {
            this.data.cityValues = [];
            this.cityLoading = false;
          });
    }
    else {
      this.data.cityValues = [];
    }
  }
  refreshcounty(id) {
    this.model.countyId = null;
    if (id) {
      this.data.countyValues = [];
      this.countyLoading = true;
      //this.resetCountryDetails();
      this.service.getcountybycity(id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.countyValues = result.dataList;
            }
            else {
              this.data.countyValues = [];
            }
            this.countyLoading = false;
          },
          error => {
            this.data.countyValues = [];
            this.countyLoading = false;
          });
    }
    else {
      this.data.countyValues = [];
    }
  }
  resetProvinceDetails() {
    this.model.cityId = null;
    this.data.cityValues = [];
    this.model.countyId = null;
    this.data.countyValues = [];
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.Formvalid()) {
      this.loading = true;
      if(this.model.countyId)
      var name = this.data.countyValues.find(x => x.id == this.model.countyId).countyName;
      this.service.saveTown(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.showSuccess("EDIT_TOWN.TITLE", "EDIT_TOWN.MSG_SAVE_SUCCESS");
              if (this.fromSummary)
                this.return("town/town-summary");
              else
                this.initialize();
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError("EDIT_TOWN.TITLE", 'EDIT_TOWN.MSG_CANNOT_SAVE_TOWN');
                  break;
                case 3:
                  this.showError("EDIT_TOWN.TITLE", 'EDIT_TOWN.MSG_CURRENT_TOWN_NOTFOUND');
                  break;
                case 4:
                  this.showError("EDIT_TOWN.TITLE", 'EDIT_TOWN.MSG_CANNOTMAPREQUEST');
                  break;
                case 5:
                  this.showError("EDIT_TOWN.TITLE", this.model.townName + ' exists for county ' + name);
                  break;
              }
            }
            this.loading = false;
          },
          error => {
            this.showError("EDIT_TOWN.TITLE", "EDIT_TOWN.MSG_UNKNONW_ERROR");
            this.loading = false;
          });
    }
  }
  Formvalid(): boolean {
    return this.validator.isValid('countyId')
      && this.validator.isValid('townName');
  }

  
  Reset(){
    this.data.provinceValues = [];
    this.data.cityValues = [];
    this.data.countyValues = [];
    this.model.countryId = null;
    this.model.provinceId = null;
    this.model.cityId = null;
    this.model.countyId = null;
    this.model.townName = null;
    this.model.townCode = null;
  }
}

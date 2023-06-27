import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LocationService } from '../../../../_Services/location/location.service'
import { EditCountrymodel } from '../../../../_Models/location/edit-countrymodel'
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { Validator } from "../../../common";
import { ToastrService } from "ngx-toastr";
import { DetailComponent } from '../../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-edit-country',
  templateUrl: './edit-country.component.html',
  styleUrls: ['./edit-country.component.css']
})
export class EditCountryComponent extends DetailComponent {
  onInit(id?: any): void {
    this.validator.setJSON('location/edit-country.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.initialize(id);
  }
  getViewPath(): string {
    return "country/view-country"
  }
  getEditPath(): string {
    return "country/edit-country"
  }
  public model: EditCountrymodel;
  public data: any;
  public error: '';
  private isDetails: boolean = true;
  loading = false;
  constructor(
    notification: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    public utility: UtilityService,
    private service: LocationService, router: Router, translate: TranslateService) {
    super(router, route, translate, notification);
  }
  initialize(id?) {
    this.model = new EditCountrymodel();
    this.data = {};
    this.validator.isSubmitted = false;
    this.service.geteditcountry(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (id) {
              this.model = {
                id: res.countryDetails.id,
                countryname: res.countryDetails.countryName,
                areaId: res.countryDetails.areaId == null ? 0 : res.countryDetails.areaId,
                countrycode: res.countryDetails.countrycode,
                alphacode: res.countryDetails.alphacode
              };
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
  Formvalid(): boolean {
    return this.validator.isValid('countryname')
      && this.validator.isValid('areaId');
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.Formvalid()) {
      this.loading = true;
      this.service.saveCountry(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.showSuccess("EDIT_COUNTRY.TITLE", "EDIT_COUNTRY.MSG_SAVE_SUCCESS");

              if (this.fromSummary)
                this.return("country/country-summary");
              else
                this.initialize();

            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_COUNTRY.TITLE', 'EDIT_COUNTRY.MSG_CANNOT_SAVE_COUNTRY');
                  break;
                case 3:
                  this.showError("EDIT_COUNTRY.TITLE", 'EDIT_COUNTRY.MSG_CURRENT_COUNTRY_NOTFOUND');
                  break;
                case 4:
                  this.showError("EDIT_COUNTRY.TITLE", 'EDIT_COUNTRY.MSG_CANNOTMAPREQUEST');
                  break;
              }

            }
            this.loading = false;
          }, error => {
            this.showError("EDIT_COUNTRY.TITLE", 'EDIT_COUNTRY.MSG_UNKNONW_ERROR');
            this.loading = false;
          });
    }
  }
  Reset() {
    this.model.countryname = '';
    this.model.areaId = 0;
    this.model.alphacode = '';
    this.model.countrycode = 0;
  }
}

import { Component } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { OfficeService } from '../../../_Services/office/office.service'
import { EditOfficeModel } from '../../../_Models/office/edit-officemodel';
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { Validator } from "../../common";
import { ToastrService } from "ngx-toastr";
import { DetailComponent } from '../../common/detail.component';
import { LocationService } from '../../../_Services/location/location.service'
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-edit-office',
  templateUrl: './edit-office.component.html',
  styleUrls: ['./edit-office.component.css']
})
export class EditOfficeComponent extends DetailComponent {
  public model: EditOfficeModel;
  public data: any;
  public error: '';
  public cityList: Array<any> = [];
  public operationcountryList: Array<any> = [];
  public loading = false;
  public intialiseloading=false;
  constructor(
    private locationService: LocationService,
    private service: OfficeService,
    route: ActivatedRoute,
    public validator: Validator,
    translate: TranslateService,
    notification: ToastrService,
    public utility: UtilityService,
    router: Router
  ) {
    super(router, route, translate, notification);
  }
  onInit(id?: any): void {
    this.validator.setJSON('office/edit-office.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.initialize(id);
  }
  getViewPath(): string {
    return "office/view-office"
  }
  getEditPath(): string {
    return "office/edit-office"
  }
  onChange(countryId) {
    this.model.cityId = 0;
    this.refresCitybyCountryId(countryId);
  }
  refresCitybyCountryId(countryid) {
    if (countryid != null && countryid > 0) {
      this.locationService.getcitybycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.cityList = result.data;
            }
            else {
              this.cityList = [];
            }
          },
          error => {
            this.cityList = [];

          });
    }
    else {
      this.cityList = [];
    }
  }

  initialize(id?) {
    this.intialiseloading = true;
    this.model = new EditOfficeModel();
    this.data = {};
    this.validator.isSubmitted = false;
    this.service.getEditOffice(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (id) {
              this.model = {
                id: res.officeDetails.id,
                name: res.officeDetails.name,
                officecode: res.officeDetails.officeCode,
                fax: res.officeDetails.fax,
                tel: res.officeDetails.tel,
                zipcode: res.officeDetails.zipCode,
                address: res.officeDetails.address,
                address2: res.officeDetails.address2,
                email: res.officeDetails.email,
                headoffice: res.officeDetails.headOffice == null ? 0 : res.officeDetails.headOffice,
                locationTypeId: res.officeDetails.type.id == null ? 0 : res.officeDetails.type.id,
                countryId: res.officeDetails.country.id == null ? 0 : res.officeDetails.country.id,
                cityId: res.officeDetails.city.id == null ? 0 : res.officeDetails.city.id,
                comment: res.officeDetails.comment
              };
              this.operationcountryList = res.officeDetails.operationCountries;
              if (res.officeDetails.operationCountries == 0) {
                this.model.operationcountries = [];
              } else {
                this.model.operationcountries = [];
                res.officeDetails.operationCountries.forEach(item => {
                  this.model.operationcountries.push(item);
                });
              }
              this.cityList = this.data.cityList.filter(item => item.countryId === res.officeDetails.country.id);
            } else {
              this.model = {
                name: null,
                officecode: null,
                fax: null,
                tel: null,
                zipcode: null,
                address: null,
                address2: null,
                email: null,
                headoffice: 0,
                locationTypeId: 0,
                countryId: 0,
                cityId: 0,
                //operationcountries: null,
                comment: null
              };
            }
          }
          else {
            this.error = res.result;
          }
          this.intialiseloading = false;
        },
        error => {
          this.error = error;
          this.intialiseloading = false;
        }
      );
  }
  Formvalid(): boolean {
    return this.validator.isValid('name')
      && this.validator.isValid('address')
      && this.validator.isValid('address2')
      && this.validator.isValid('email')
      && this.validator.isValid('locationTypeId')
      && this.validator.isValid('countryId')
      && this.validator.isValid('cityId')
      && this.validator.isValid('operationcountries');
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.Formvalid()) {
      this.loading = true;
      this.model.locationType = {
        id: this.model.locationTypeId
      };
      this.model.country = {
        id: this.model.countryId
      };
      this.model.city = {
        id: this.model.cityId
      }
      if (this.model.headoffice === 0) {
        this.model.headoffice = null;
      }
      // this.operationcountryList.forEach(x => {
      //   this.model.operationcountries = this.model.operationcountries.filter(item => item !== x);
      // });
      this.model.operationcountries.forEach(x => {
        this.operationcountryList.forEach
      });
      this.service.saveOffice(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.showSuccess("EDIT_OFFICE.TITLE", "EDIT_OFFICE.MSG_SAVE_SUCCESS");
              this.initialize();
              if (this.fromSummary)
                this.return("office/office-summary");
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError("EDIT_COUNTRY.TITLE", "EDIT_OFFICE.MSG_CANNOT_SAVE_OFFICE");
                  break;
                case 3:
                  this.showError("EDIT_COUNTRY.TITLE", "EDIT_OFFICE.MSG_CURRENT_OFFICE_NOTFOUND");
                  break;
                case 4:
                  this.showError("EDIT_COUNTRY.TITLE", "EDIT_OFFICE.MSG_CANNOTMAPREQUEST");
                  break;
              }
            }
            this.loading = false;
          }, error => {
            this.showError("EDIT_OFFICE.TITLE", 'EDIT_OFFICE.MSG_UNKNONW_ERROR');
            this.loading = false;
          });
    }
  }
  Reset() {
    this.model.locationTypeId = 0;
    this.model.headoffice = 0;
    this.model.countryId = 0;
    this.model.cityId = 0;
    this.model.address = '';
    this.model.officecode = '';
    this.model.name = '';
    this.model.fax = '';
    this.model.tel = '';
    this.model.zipcode = null;
    this.model.email = '';
    this.model.operationcountries = null;
    this.model.comment = '';
    this.model.address2 = '';
  }
}

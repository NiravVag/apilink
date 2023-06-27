import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { EditProvincemodel } from '../../../../_Models/location/edit-provincemodel'
import { first } from 'rxjs/operators';
import { LocationService } from '../../../../_Services/location/location.service'
import { Validator } from '../../../common/validator'
import { ToastrService } from "ngx-toastr";
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-edit-province',
  templateUrl: './edit-province.component.html',
  styleUrls: ['./edit-province.component.css']
})
export class EditProvinceComponent extends DetailComponent {
  onInit(id?: any): void {
    this.validator.setJSON('location/edit-provinse.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.initialize(id);
  }
  getViewPath(): string {
   return "province/view-province"
  }
  getEditPath(): string {
    return "province/edit-province"
  }
  public data: any = {};
  public error: string = "";
  public model: EditProvincemodel;
  loading = false;
  constructor(
    private service: LocationService,
    route: ActivatedRoute,
    public validator: Validator,
    translate: TranslateService,
    public utility: UtilityService,
    private notification: ToastrService,
    router: Router) {
    super(router, route, translate, notification);
  }
  initialize(id?) {
    this.model = new EditProvincemodel();
    this.validator.isSubmitted = false;
    this.service.geteditProvince(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            console.log(res);
            if (id) {
              this.model = {
                id: res.provinceDetails.id,
                countryid: res.provinceDetails.countryId == null ? 0 : res.provinceDetails.countryId,
                code: res.provinceDetails.code,
                name: res.provinceDetails.name,
              }
            }
          }
          else {
            this.error = res.result;
          }
        },
        error => {
          this.error = error;
        }
      );
  }
  Formvalid(): boolean {
    return this.validator.isValid('name') && this.validator.isValid('countryid');
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.Formvalid()) {
      this.loading = true;
      this.service.saveProvince(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.showSuccess("EDIT_PROVINCE.TITLE", "EDIT_PROVINCE.MSG_SAVE_SUCCESS");
              if (this.fromSummary)
                this.return("province/province-summary");
              else
                this.initialize();
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError("EDIT_PROVINCE.TITLE", 'EDIT_PROVINCE.MSG_CANNOT_SAVE_PROVINCE');
                  break;
                case 3:
                  this.showError("EDIT_PROVINCE.TITLE", 'EDIT_PROVINCE.MSG_CURRENT_PROVINCE_NOTFOUND');
                  break;
                case 4:
                  this.showError("EDIT_PROVINCE.TITLE", 'EDIT_PROVINCE.MSG_CANNOTMAPREQUEST');
                  break;
                case 5:
                  this.showError("EDIT_PROVINCE.TITLE", 'EDIT_PROVINCE.MSG_PROVINCE_EXISTS');
                  break;
              }
            }
            this.loading = false;
          },
          error => {
            this.showError("EDIT_PROVINCE.TITLE", "EDIT_PROVINCE.MSG_UNKNONW_ERROR");
            this.loading = false;
          });
    }
  }
  Reset() {
    this.model.countryid = 0;
    this.model.code = '';
    this.model.name = '';
  }
}

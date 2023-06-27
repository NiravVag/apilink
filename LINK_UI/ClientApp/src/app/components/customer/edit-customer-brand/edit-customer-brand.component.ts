import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { CustomerBrandService } from '../../../_Services/customer/customerbrand.service'
import { CustomerSummaryItemModel } from '../../../_Models/customer/cutomersummary.model';
import { EditCustomerBrandModel, Brand } from '../../../_Models/customer/edit-customer-brand.model';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { SaveCustomerResult } from '../../common/static-data-common';
@Component({
  selector: 'app-edit-brand-Customer',
  templateUrl: './edit-customer-brand.component.html',
  styleUrls: ['./edit-customer-brand.component.css']
})
export class EditCustomerBrandComponent extends DetailComponent {
  public model: EditCustomerBrandModel;
  error: '';
  public isDetails: boolean = true;
  public jsonHelper: JsonHelper;
  public brandValidators: Array<any> = [];
  customerList: Array<any> = [];
  private _translate: TranslateService;
  private _toastr: ToastrService;
  Initialloading: boolean = false;
  Saveloading: boolean = false;

  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public utility: UtilityService,
    public service: CustomerBrandService) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("customer/edit-customerbrand.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;

    this._translate = translate;
    this._toastr = toastr;

  }

  onInit(id?: any) {
    this.init(id);
  }

  selectBrand() {
    if(this.model.id!=null){
      this.init(this.model.id);
    }
  }

  getViewPath(): string {
    return "cusbrand/view-customer-brand";
  }

  getEditPath(): string {
    return "cusbrand/edit-customer-brand";
  }

  ngOnChanges(changes: SimpleChanges) {
    const id: SimpleChange = changes.currentId;
    this.init(id.currentValue);
  }

  isFactory(): boolean {
    return this.model != null && this.model.id == 1;
  }


  init(id?) {
    this.Initialloading =true;
    this.model = new EditCustomerBrandModel();
    this.model.id = id;
    this.validator.isSubmitted = false;
    this.service.getCustomerSummary()
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.customerList = data.customerList;
            this.editCustomer(this.model.id);

          }
          else {
            this.error = data.result;
            this.Initialloading =false;
          }
        },
        error => {
          this.error = error;
          this.Initialloading =false;
        });

  }

  editCustomer(id){
    this.service.getEditCustomerBrand(id)
      .pipe()
      .subscribe(
        res => {
          if (res) {

            this.brandValidators = [];
            if (id) {
              this.model = this.mapModel(res);
              this.validator.setModel(this.model);
            }

            if (this.model.customerBrands == null || this.model.customerBrands.length == 0)
              this.addBrand();
            this.Initialloading =false;
          }
          else {
            this.error = res.result;
            this.Initialloading =false;
          }

        },
        error => {
          this.error = error;
          this.Initialloading =false;
        });
  }



  mapModel(customerBrandDetails: any): EditCustomerBrandModel {
    var model: EditCustomerBrandModel = {
      id: customerBrandDetails.id,
      customerBrands: customerBrandDetails.customerBrands.map((x) => {
        var brand: Brand = {
          id: x.id,
          name:x.name,
          brandCode:x.brandCode
        };
        this.brandValidators.push({ brand: brand, validator: Validator.getValidator(brand, "customer/edit-customerbrand.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
        return brand;
      }),
    };
    return model;
  }
  save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.brandValidators)
      item.validator.isSubmitted = true;

    const duplicates: string[] = [];
    this.model.customerBrands.forEach((c) => {
      if (this.model.customerBrands.filter(x => x.name.toUpperCase() == c.name.toUpperCase()).length > 1) {
        if (duplicates.filter(x => x.toUpperCase() == c.name.toUpperCase()).length == 0) {
          duplicates.push(c.name)
        }
      }
    });

    if (duplicates != null && duplicates.length > 0) {
      let tradMessage: string;
      this._translate.get('CUSTOMER_BRAND.MSG_CUSTOMER_BRAND_EXISTS').subscribe((text: string) => { tradMessage = text });
      tradMessage = tradMessage.replace("{0}", duplicates.toString());
      this.showError('CUSTOMER_BRAND.SAVE_RESULT', tradMessage);
    }

    if (this.isFormValid() && duplicates.length == 0) {
      this.Saveloading=true;
      // this.waitingService.open();

      this.service.saveCustomerBrand(this.model)
        .subscribe(
          res => {

            if (res && res.result == SaveCustomerResult.Success) {
              this.showSuccess('CUSTOMER_BRAND.SAVE_RESULT', 'CUSTOMER_BRAND.SAVE_OK');
              this.Saveloading=false;
              this.init(this.model.id);
            }
            else {
              this.Saveloading=false;
              switch (res.result) {
                case SaveCustomerResult.CustomerIsNotSaved:
                  this.showError('CUSTOMER_BRAND.SAVE_RESULT', 'CUSTOMER_BRAND.MSG_CANNOT_ADDCUSTOMERBRAND');
                  break;
                case SaveCustomerResult.CustomerIsNotFound:
                  this.showError('CUSTOMER_BRAND.SAVE_RESULT', 'CUSTOMER_BRAND.MSG_CURRENTBRAND_NOTFOUND');
                  break;
                case SaveCustomerResult.CustomerExists:
                  let tradMessage: string = "";
                  this._translate.get('CUSTOMER_BRAND.MSG_CUSTOMER_BRAND_EXISTS').subscribe((text: string) => { tradMessage = text });
                  tradMessage = tradMessage.replace("{0}", res.errorList[0].errorText);
                  this.showError('CUSTOMER_BRAND.SAVE_RESULT', tradMessage);
                  break;
                case SaveCustomerResult.CustomerOneBrandRequired:
                  this.showError('CUSTOMER_BRAND.SAVE_RESULT', 'CUSTOMER_BRAND.MSG_CUSTOMERBRAND_REQUIRED');
                  break;
              }

              //this.waitingService.close();
            }
          },
          error => {
            this.showError('CUSTOMER_BRAND.', 'CUSTOMER_BRAND.MSG_UNKNONW_ERROR');
            this.Saveloading=false;
            // this.waitingService.close();
          });
    }
  }


  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }

  removeBrand(index) {
    this.model.customerBrands.splice(index, 1);
    this.brandValidators.splice(index, 1);
  }

  addBrand() {
    var brand: Brand = {
      id: 0,
      name: '',
      brandCode: null
    };

    this.model.customerBrands.push(brand);
    this.brandValidators.push({ brand: brand, validator: Validator.getValidator(brand, "customer/edit-customerbrand.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

  }

  isFormValid() {
    return this.validator.isValid('id')
      && this.brandValidators.every((x) => x.validator.isValid('name'))
    // && this.brandValidators.length>0
  }

}




import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { CustomerBuyermodel, buyermodel, customerBuyerToRemove } from '../../../_Models/customer/customerbuyer.model';
import { CustomerbuyerService } from '../../../_Services/customer/customerbuyer.service';
import { ReferenceService } from '../../../_Services/reference/reference.service';
import { APIService, SaveCustomerBuyerResult } from "src/app/components/common/static-data-common";
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-customer-buyer',
  templateUrl: './customer-buyer.component.html',
  styleUrls: ['./customer-buyer.component.css']
})
export class CustomerBuyerComponent extends DetailComponent {

  Initialloading: boolean = false;
  customerList: Array<any> = [];
  public modelRemove: customerBuyerToRemove;

  public buyerValidators: Array<any> = [];
  public model: CustomerBuyermodel;
  public data: any;
  error = '';
  public isReadyForSubmit: boolean = false;
  public jsonHelper: JsonHelper;
  private _toastr: ToastrService;
  private _translate: TranslateService;
  public isNoReturn: boolean = false;

  Saveloading: boolean = false;
  public modelRef: NgbModalRef;
  public isNoRows: boolean = false;
  apiServiceList:any;
  apiService=APIService;

  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public service: CustomerbuyerService,
    public customerService: CustomerService,
    public utility: UtilityService,
    public modalService: NgbModal,
    public referenceService:ReferenceService)
  {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("customer/customer-buyer.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this._toastr = toastr;
    this._translate = translate;
    this.validator.isSubmitted = false;
    this.model = new CustomerBuyermodel();
  }

  getViewPath(): string {
    return "cusbuyer/customer-buyer";
  }
  getEditPath(): string {
    return "cusbuyer/customer-buyer";
  }

  onInit(id?: any) {
    this.init(id);
  }
  init(id?) {
    this.Initialloading = true;
    this.getAPIServices();
    if (id) {
      this.service.getCustomerBuyer(id)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.buyerValidators = [];
              this.data = response;
              this.customerList = response.customerList;
              this.isReadyForSubmit = true;
              this.model = {
                customerValue: parseInt(id),
                buyerList: response.customerBuyerList.map((x) => {

                  var buyer: buyermodel = {
                    id: x.id,
                    name: x.name,
                    code: x.code,
                    apiServiceIds:x.apiServiceIds
                  };
                  this.buyerValidators.push({ buyer: buyer, validator: Validator.getValidator(buyer, "customer/customer-buyer.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
                  return buyer;
                }),
              }
              if (this.model.buyerList == null || this.model.buyerList.length == 0)
                this.addBuyerRow();
              this.Initialloading = false;
            }
            else {
              this.error = response.result;
              this.Initialloading = false;
            }


          },
          error => {
            this.setError(error);
            this.Initialloading = false;
          });
    }
    else {
      this.customerService.getCustomerSummary()
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.data = response;
              this.isReadyForSubmit = false;
              this.isNoReturn = true;
              this.customerList = response.customerList;
              this.Initialloading = false;
            }
            else {
              this.error = response.result;
              this.Initialloading = false;
            }

          },
          error => {
            this.setError(error);
            this.Initialloading = false;
          });
    }
  }

  addBuyerRow() {
    this.isNoRows = false;
    let buyer: buyermodel = { id: 0, name: "", code: "",apiServiceIds:[] };
    if(this.apiServiceList.length>0)
    {
      buyer.apiServiceIds.push(this.apiServiceList[0].id);
    }
    this.model.buyerList.push(buyer);
    this.buyerValidators.push(
      {
        buyer: buyer,
        validator: Validator.getValidator(buyer, "customer/customer-buyer.valid.json",
          this.jsonHelper,
          this.validator.isSubmitted,
          this._toastr,
          this._translate)
      });
  }

  selectBuyer() {
    if(this.model.customerValue!=null){
      this.isReadyForSubmit=true;
      this.init(this.model.customerValue);
    }

  }

  isFormValid() {
    return this.validator.isValid('customerValue')
      && this.buyerValidators.every((x) => x.validator.isValid('name') && x.validator.isValid('apiServiceIds'))
      && this.buyerValidators.length > 0
  }

  save() {

    this.validator.initTost();

    for (let item of this.buyerValidators)
      item.validator.isSubmitted = true;

    const duplicates: string[] = [];
    this.model.buyerList.forEach((c) => {
      if (this.model.buyerList.filter(x => x.name.toUpperCase() == c.name.toUpperCase()).length > 1) {
        if (duplicates.filter(x => x.toUpperCase() == c.name.toUpperCase()).length == 0) {
          duplicates.push(c.name)
        }
      }
    });

    if (duplicates != null && duplicates.length > 0) {
      let tradMessage: string;
      this._translate.get('CUSTOMER_BUYER.MSG_CUSTOMERBUYER_EXISTS').subscribe((text: string) => { tradMessage = text });
      tradMessage = tradMessage.replace("{0}", duplicates.toString());
      this.showError('CUSTOMER_BUYER.SAVE_RESULT', tradMessage);
    }

    if (this.isFormValid() && duplicates.length == 0) {
      this.Saveloading = true;
      this.service.saveBuyer(this.model)
        .subscribe(res => {
          if (res && res.result == SaveCustomerBuyerResult.Success) {
            this.showSuccess('CUSTOMER_BUYER.TITLE', 'CUSTOMER_BUYER.MSG_SAVE_SUCCESS');
            this.Saveloading = false;
            this.init(this.model.customerValue);
          }
          else {
            this.Saveloading = false;
            switch (res.result) {
              case SaveCustomerBuyerResult.CustomerBuyerExists:
                let tradMessage: string = "";
                this._translate.get('CUSTOMER_BUYER.MSG_CUSTOMERBUYER_EXISTS').subscribe((text: string) => { tradMessage = text });
                tradMessage = tradMessage.replace("{0}", res.errorData.errorText);
                this.showError('CUSTOMER_BUYER.SAVE_RESULT', tradMessage);
                break;
            }
          }
        })


    }
  }

  openConfirm(id, name, content, index) {
    var buyerID = this.model.buyerList[index].id;
    var buyerName = this.model.buyerList[index].name;
    if (buyerID != 0) {

      this.modelRemove = {
        id: buyerID,
        name: buyerName
      };

      this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });

      this.modelRef.result.then((result) => {
        // this.closeResult = `Closed with: ${result}`;
      }, (reason) => {
        //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });

    }
    else {
      this.model.buyerList.splice(index, 1);
      this.buyerValidators.splice(index, 1);
      if( this.buyerValidators.length == 0) {
        this.isNoRows = true;
      }
    }

  }

  deleteCustomer(item: customerBuyerToRemove) {
    this.service.deleteCustomerBuyer(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            var buyerID = this.model.customerValue;
            this.init(buyerID);
          }
          else {
            this.error = response.result;
            this.loading = false;
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();

  }

  getAPIServices() {

    this.referenceService.getAPIServices()
      .pipe()
      .subscribe(
        response => {
          if (response.result == 1)
            this.apiServiceList = response.dataSourceList;
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

}

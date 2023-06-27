import { Component, SimpleChanges, SimpleChange } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, JsonHelper } from '../../common'
import { CustomerCollectionService } from '../../../_Services/customer/customercollection.service'
import { CustomerCollectionModel, Collection } from '../../../_Models/customer/customercollection.model';
import { TranslateService } from '@ngx-translate/core';
import { SummaryComponent } from '../../common/summary.component';
import { PageSizeCommon, SaveCustomerResult } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-customer-collection',
  templateUrl: './customer-collection.component.html'
})
export class CustomerCollectionComponent extends SummaryComponent<CustomerCollectionModel> {
  model: CustomerCollectionModel;
  error: '';
  isDetails: boolean = true;
  jsonHelper: JsonHelper;
  collectionValidators: Array<any> = [];
  customerList: Array<any> = [];
  private _translate: TranslateService;
  private _toastr: ToastrService;
  Initialloading: boolean = false;
  Saveloading: boolean = false;
  selectedPageSize;
  pagesizeitems = PageSizeCommon;
  isPagingShow: boolean;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute, public activateRoute: ActivatedRoute,
    router: Router, private routers: Router,
    public utility: UtilityService,
    public service: CustomerCollectionService) {
    super(router, validator, route, translate, toastr);

    this.validator.isSubmitted = false;
    this.validator.setJSON("customer/customer-collection.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;

    this._translate = translate;
    this._toastr = toastr;
  }

  onInit() {
    let modelId =  this.activateRoute.snapshot.paramMap.get("id");
    let id = modelId ? parseInt(modelId) : 0;
    this.init(id);
  }

  selectCollection() {
    if (this.model.id != null) {
      this.init(this.model.id);
    }
    this.isPagingShow = false;
  }
  getData() {
    this.editCustomer(this.model.id);
  }

  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }

  init(id?) {
    this.selectedPageSize = PageSizeCommon[0];
    this.Initialloading = true;
    this.model = new CustomerCollectionModel();
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
            this.Initialloading = false;
          }
        },
        error => {
          this.error = error;
          this.Initialloading = false;
        });

  }

  editCustomer(id) {
    this.model.id = id;
    this.model.customerCollectionList = [];
    this.collectionValidators = [];
    this.service.getEditCustomerCollection(this.model)
      .pipe()
      .subscribe(
        res => {
          if (res) {
            if (id && res.result == 1) {
              this.mapModel(res);
              this.mapPageProperties(res);
              this.isPagingShow = true;
            }

            if (this.model.customerCollectionList == null || this.model.customerCollectionList.length == 0)
              this.addCollection();

            this.Initialloading = false;
          }
          else {
            this.error = res.result;
            this.Initialloading = false;
          }

        },
        error => {
          this.error = error;
          this.Initialloading = false;
        });
  }

  mapModel(customerCollectionDetails: any) {
    if (customerCollectionDetails.id > 0) {
      this.model.id = customerCollectionDetails.id;
    }
    if (customerCollectionDetails.customerCollectionList && customerCollectionDetails.customerCollectionList.length > 0) {
      this.model.customerCollectionList = customerCollectionDetails.customerCollectionList.map((x) => {
        var collection: Collection = {
          id: x.id,
          name: x.name,
        };
        this.collectionValidators.push({
          collection: collection,
          validator: Validator.getValidator(collection, "customer/customer-collection.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
        });
        return collection;
      });
    }
  }

  save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.collectionValidators)
      item.validator.isSubmitted = true;

    const duplicates: string[] = [];
    this.model.customerCollectionList.forEach((c) => {
      if (this.model.customerCollectionList.filter(x => x.name.toUpperCase() == c.name.toUpperCase()).length > 1) {
        if (duplicates.filter(x => x.toUpperCase() == c.name.toUpperCase()).length == 0) {
          duplicates.push(c.name)
        }
      }
    });

    if (duplicates != null && duplicates.length > 0) {
      let tradMessage: string;
      this._translate.get('CUSTOMER_COLLECTION.MSG_CUSTOMERCOLLECTION_EXISTS').subscribe((text: string) => { tradMessage = text });
      tradMessage = tradMessage.replace("{0}", duplicates.toString());
      this.showError('CUSTOMER_COLLECTION.SAVE_RESULT', tradMessage);
    }

    if (this.isFormValid() && duplicates.length == 0) {
      this.Saveloading = true;
      this.service.saveCustomerCollection(this.model)
        .subscribe(
          res => {

            if (res && res.result == SaveCustomerResult.Success) {
              this.showSuccess('CUSTOMER_COLLECTION.SAVE_RESULT', 'CUSTOMER_COLLECTION.SAVE_OK');
              this.Saveloading = false;
              this.model.removeIds = [];
              this.init(this.model.id);
            }
            else {
              this.Saveloading = false;
              switch (res.result) {
                case SaveCustomerResult.CustomerIsNotSaved:
                  this.showError('CUSTOMER_COLLECTION.SAVE_RESULT', 'CUSTOMER_COLLECTION.MSG_CANNOT_ADDCUSTOMER_COLLECTION');
                  break;
                case SaveCustomerResult.CustomerIsNotFound:
                  this.showError('CUSTOMER_COLLECTION.SAVE_RESULT', 'CUSTOMER_COLLECTION.MSG_CURRENT_COLLECTION_NOTFOUND');
                  break;
                case SaveCustomerResult.CustomerExists:
                  let tradMessage: string = "";
                  this._translate.get('CUSTOMER_COLLECTION.MSG_CUSTOMERCOLLECTION_EXISTS').subscribe((text: string) => { tradMessage = text });
                  tradMessage = tradMessage.replace("{0}", res.errorList[0].errorText);
                  this.showError('CUSTOMER_COLLECTION.SAVE_RESULT', tradMessage);
                  break;
              }
            }
          },
          error => {
            this.showError('COMMON.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
            this.Saveloading = false;
          });
    }
  }

  removeCollection(index) {

    var getData = this.model.customerCollectionList.splice(index, 1).map(x => x.id);

    getData.forEach(element => {
      this.model.removeIds.push(element);
    });
    this.collectionValidators.splice(index, 1);
  }

  addCollection() {
    var collection: Collection = {
      id: 0,
      name: '',
    };

    this.model.customerCollectionList.push(collection);
    this.collectionValidators.push({ collection: collection, validator: Validator.getValidator(collection, "customer/customer-collection.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

  }

  isFormValid() {
    return this.validator.isValid('id')
      && this.collectionValidators.every((x) => x.validator.isValid('name'))
      && this.collectionValidators.length > 0
  }
  getPathDetails(): string {
    return "";
  }

}


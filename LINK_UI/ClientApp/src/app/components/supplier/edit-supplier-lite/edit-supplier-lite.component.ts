import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { SupplierSummaryItemModel } from '../../../_Models/supplier/suppliersummary.model';
import { EditSuplierModel, Address, CustomerContact, Customer } from '../../../_Models/supplier/edit-supplier.model'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Country, SupplierType } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';


@Component({
  selector: 'app-edit-supplier-lite',
  templateUrl: './edit-supplier-lite.component.html',
  styleUrls: ['./edit-supplier-lite.component.scss']
})
export class EditSupplierLiteComponent extends DetailComponent {
  public model: EditSuplierModel;
  public data: any;

  public suppliersbyCountry: Array<SupplierSummaryItemModel>;
  public suppliersSelected: Array<SupplierSummaryItemModel>;
  public selectCounryId: number = 0;
  private jsonHelper: any;
  public validator: any;
  public addressValidators: Array<any> = [];
  public contactValidators: Array<any> = [];
  public selectedcustomerList: Array<any>;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public _saveloader: boolean = false;
  public regionloader: boolean = false;
  public cityloader: boolean = false;  
  public isLocLanguageValidationRequired: boolean = true;
  supplierType = SupplierType;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    route: ActivatedRoute,
    jsonHelper: JsonHelper,
    router: Router,
    public utility: UtilityService,
    public modalService: NgbModal,
    private service: SupplierService) {
    super(router, route, translate, toastr);
    this._translate = translate;
    this._toastr = toastr;
    this.jsonHelper = jsonHelper;
  }
  @Input() modelData: any;
  onInit(id?: any) {
    this.init(id);
  }

  getViewPath(): string {
    return "supplierlite/view-supplier";
  }

  getEditPath(): string {
    return "supplierlite/edit-supplier";
  }

  public getAddPath(): string {
    return "supplierlite/new-supplier";
  }

  isFactory(): boolean {
    return this.model != null && this.model.typeId == 1;
  }

  init(id?) {
    this.loading = true;
    this.model = new EditSuplierModel();
    this.data = {};

    this.model.typeId = this.modelData.typeId
    this.validator = Validator.getValidator(this.model, "supplier/edit-supplier.valid.json", this.jsonHelper, false, this._toastr, this._translate);
    this.validator.isSubmitted = false;
    if (this.model.typeId == 1) {
      this.model.isNewSupplier = false;
      this.getSelectSuppliers();
    }
    else {
      this.model.isNewSupplier = false;
    }

    //this.waitingService.open();
    this.service.getEditSupplier(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;

            this.addressValidators = [];
            this.contactValidators = [];

            if (this.model.addressList == null || this.model.addressList.length == 0)
              this.addAddress();

            if (this.model.supplierContactList == null || this.model.supplierContactList.length == 0)
              this.addContact();

          }
          else {
            this.error = res.result;
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  cancelSupplierORFactory() {
    return this.modalService.dismissAll();
  }

  onChangeCountry(countryId, item: Address) {
    this.regionloader = true;    
    item.regionId = null;
    item.cityId = null;
    item.cityList = [];    
    this.getStates(countryId).subscribe(
      res => {
        if (res && res.result == 1) {
          item.regionList = res.data;
          //item.regionId = 0; 
        }
        else {
          item.regionList = [];
        }
        this.regionloader = false;
      },
      error => {
        item.regionList = [];
        this.regionloader = false;
      });

  }

  onChangeRegion(stateId, item: Address) {
    this.cityloader = true;
    item.cityId = null;
    this.getCities(stateId)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.cityList = res.data;
            //item.cityId = 0;
          }
          else {
            item.cityList = [];
          }
          this.cityloader = false;
        },
        error => {
          item.cityList = [];
          this.cityloader = false;
        });
  }

  getStates(countryId) {
    return this.service.getStates(countryId)
      .pipe()
  }  

  getCities(stateId) {
    return this.service.getCities(stateId)
      .pipe();
  }

  //assign service and entity data based on the selection
  assignEntityAndService() {
    //set service and entity data
    var service = JSON.parse(localStorage.getItem('service'));
    var entityData = this.utility.getEntityId();

    if (service && entityData) {
      //push the apiservice(which is supplier service)
      this.model.apiServiceIds.push(service);
      //map the entity data
      var entityId = parseInt(entityData);
      //assign the service,entity,primary entity to the contacts
      this.model.supplierContactList.forEach(supplierContact => {
        supplierContact.contactAPIServiceIds.push(service);
        supplierContact.primaryEntity = entityId;
        supplierContact.apiEntityIds.push(entityId);
        supplierContact.entityServiceIds = null;
      });
    }

  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.addressValidators) {
      // item.validator.initTost();
      item.validator.isSubmitted = true;
    }

    for (let item of this.contactValidators) {
      //item.validator.initTost();
      item.validator.isSubmitted = true;
    }

    if (this.isFormValid()) {
      this._saveloader = true;

      this.assignEntityAndService();
      //  this.waitingService.open();
      this.model.isFromBookingPage = true;
      this.service.saveSupplier(this.model)
        .subscribe(
          res => {

            if (res && res.result == 1) {

              this.showSuccess('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.SAVE_OK');
              this.cancelSupplierORFactory();
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_CANNOT_ADDSUPPLIER');
                  break;
                case 3:
                  this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_CURRENTSUPP_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_SUPPLIER_EXISTS');
                  break;
              }


              this.cancelSupplierORFactory();
              // this.waitingService.close();
            }
            this._saveloader = false;
          },
          error => {
            this._saveloader = false;
            if (error == "Unauthorized")
              this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'ERROR.MSG_MESSAGE_401');
            else
              this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_UNKNONW_ERROR');
            //this.waitingService.close();
          });
    }
  }


  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }

  clearCountry(address) {
    address.regionId = null;
    address.regionList = null;

    address.cityId = null;
    address.cityList = null;

  }

  clearRegion(address) {
    address.cityId = null;
    address.cityList = null;

  }

  removeAddress(index) {
    this.model.addressList.splice(index, 1);
    this.addressValidators.splice(index, 1);
  }

  addAddress() {

    var address: Address = {
      id: 0,
      countryId: null,
      cityId: null,
      cityList: [],
      regionId: null,
      regionList: [],
      way: '',
      zipCode: '',
      addressTypeId: 0,
      longitude: null,
      latitude: null,
      localLanguage: '',
      countyId: null,
      townId: null,
      countyList: [],
      townList: []
    };
    var customerContact: CustomerContact = {
      comment: '',
      contactEmail: '',
      contactId: 0,
      contactName: '',
      fax: '',
      jobTitle: '',
      mobile: '',
      phone: '',
      customerList: [],
      contactAPIServiceIds: [],
      apiEntityIds: [],
      entityServiceList: [],
      entityServiceIds: [],
      primaryEntityList: [],
      contactEntityList: [],
      primaryEntity: null,
      showPopupLoading: false
    };

    this.model.addressList.push(address);
    this.addressValidators.push({ address: address, validator: Validator.getValidator(address, "supplier/edit-address.valid.json", this.jsonHelper, false, this._toastr, this._translate) });
  }

  addContact() {

    var customerContact: CustomerContact = {
      comment: '',
      contactEmail: '',
      contactId: 0,
      contactName: '',
      fax: '',
      jobTitle: '',
      mobile: '',
      phone: '',
      customerList: [],
      contactAPIServiceIds: [],
      apiEntityIds: [],
      entityServiceIds: [],
      entityServiceList: [],
      primaryEntityList: [],
      contactEntityList: [],
      primaryEntity: null,
      showPopupLoading: false
    };

    this.model.supplierContactList.push(customerContact);
    this.contactValidators.push({ customerContact: customerContact, validator: Validator.getValidator(customerContact, "supplier/edit-contact.valid.json", this.jsonHelper, false, this._toastr, this._translate) });
    this.addCustomer();
  }

  removeContact(index) {
    this.model.supplierContactList.splice(index, 1);
    this.contactValidators.splice(index, 1);
  }

  getSelectSuppliers(): Array<SupplierSummaryItemModel> {

    this.model.supplierParentList = [];

    if (this.modelData.supplierId != null) {

      if (!this.model.supplierParentList.some(x => x.id == this.modelData.supplierId)) {
        var data: SupplierSummaryItemModel =
        {
          id: this.modelData.supplierId,
          name: "",
          countryName: "",
          regionName: "",
          cityName: "",
          typeId: this.modelData.typeId,
          typeName: "",
          list: [],
          isExpand: false,
          canBeDeleted: 0
        }
        this.model.supplierParentList.push(data);
      }

      // this.model.supplierParentList.forEach(element => 
      // {
      //   element.list=this.model.supplierParentList;
      // });

    }

    return this.model.supplierParentList;
  }

  removeSupplier(index) {
    this.suppliersSelected = [];
    this.model.supplierParentList.splice(index, 1);
  }

  isFormValid() {
    var isValid = this.validator.isValid('name')
      && this.validator.isValid('email')
      && this.validator.isValid('typeId')
      && this.validator.isValid('contactPersonName')
      && this.addressValidators.every((x) => x.validator.isValid('countryId')
        && x.validator.isValid('regionId')
        && x.validator.isValid('cityId')
        && x.validator.isValid('zipCode')        
      )
      && this.contactValidators.every((x) =>
        x.validator.isValid('contactName')
        && x.validator.isValid('contactEmail')
        && x.validator.isValid('phone')
      );

    return isValid;
  }

  checkHeadOffice(): boolean {
    if (this.model.typeId != 1 && this.validator.isSubmitted) {
      if (this.model.addressList == null || this.model.addressList.length == 0)
        return false;

      var data = this.model.addressList.filter(x => x.addressTypeId == 1);

      return data.length == 1;
    }
    return true;
  }

  addCustomer() {
    this.selectedcustomerList = [{ 'id': this.modelData.customerId }];
    if (this.selectedcustomerList != null) {
      for (let item of this.selectedcustomerList) {
        if (!this.model.customerList.some(x => x.id == item.id))
          //this.model.customerList.push(item);
          this.model.customerList = [...this.model.customerList, item];
      }
      this.model.supplierContactList.forEach(item => {
        item.customerList = this.model.customerList;
      });
    }
  }

  removeCustomer(index) {

    this.model.customerList.splice(index, 1);
  }

  checkCustomer(): boolean {

    if (!this.validator.isSubmitted)
      return true;

    return this.model.customerList != null && this.model.customerList.length > 0;
  }

  checkSupplierParent(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    if (this.model.typeId != 1)
      return true;

    if (this.model.isNewSupplier)
      return true;

    if (this.model.supplierParentList == null || this.model.supplierParentList.length == 0)
      return false;

    return true;
  }

  checkCustomerContact(): boolean {

    if (!this.validator.isSubmitted)
      return true;

    if (this.model.supplierContactList == null || this.model.customerList.length == 0)
      return true;

    if (this.model.customerList == null || this.model.customerList.length == 0)
      return false;

    let ids = this.model.customerList.map(x => x.id);

    for (let item of this.model.supplierContactList) {
      if (item.customerList.some(x => ids.indexOf(x.id) < 0))
        return false;
    }


    return true;
  }

  /**
   * function to toggle tabs on click
   * @param {event} event     [current event]
   * @param {string} tabTarget [targeted tab id]
   */
  toggleTab(event, tabTarget) {
    let tabs = event.target.parentNode.children;
    for (let tab of tabs) {
      tab.classList.remove('active');
    }
    event.target.classList.add('active');

    let tabContainers = document.querySelector('#' + tabTarget).parentNode.childNodes;
    for (let container of <any>tabContainers) {
      container.classList.remove('active');
    }
    document.getElementById(tabTarget).classList.add('active');
  }
}




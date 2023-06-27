import { Component } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, JsonHelper } from '../../common';
import { LabService } from '../../../_Services/lab/lab.service';
import { EditLabModel, LabAddress, CustomerContact, LabCustomer } from '../../../_Models/lab/edit-lab.model';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-edit-lab',
  templateUrl: './edit-lab.component.html',
  styleUrls: ['./edit-lab.component.css']
})
export class EditLabComponent extends DetailComponent {
  private jsonHelper: JsonHelper;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  model: EditLabModel;
  addressValidators: Array<any> = [];
  contactValidators: Array<any> = [];
  selectedCustomerList: Array<LabCustomer>;
  _saveloader: boolean = false;
  mainTypeList: Array<any> = [];
  mainTypeLoading: boolean = false;
  addressTypeList: Array<any> = [];
  addressTypeLoading: boolean = false;
  customerMasterList: Array<any> = [];
  customerLoading: boolean = false;
  countryList: Array<any> = [];
  countryLoading: boolean = false;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    public utility: UtilityService,
    router: Router,
    private service: LabService) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("lab/edit-lab.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this._translate = translate;
    this._toastr = toastr;
  }
  onInit(id?: any) {
    this.customerLoading = true;
    this.countryLoading = true;
    this.mainTypeLoading = true;
    this.addressTypeLoading = true;
    this.getMainType();
    this.getCustomer();
    this.getAddressType();
    this.getCountry();
    this.init(id);
  }
  getMainType() {
    this.service.getMainType()
      .pipe().subscribe(
        response => {
          if (response && response.result == 1)
            this.mainTypeList = response.typeList;
          else
            this.error = response.result;
          this.mainTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.mainTypeLoading = false;
        });
  }
  getAddressType() {
    this.service.getAddressType()
      .pipe().subscribe(
        response => {
          if (response && response.result == 1)
            this.addressTypeList = response.addressTypeList;
          else
            this.error = response.result;
          this.addressTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.addressTypeLoading = false;
        });
  }
  getCustomer() {
    this.service.getCustomer()
      .pipe().subscribe(
        response => {
          if (response && response.result == 1)
            this.customerMasterList = response.customerList;
          else
            this.error = response.result;
          this.customerLoading = false;
        },
        error => {
          this.setError(error);
          this.customerLoading = false;
        });
  }
  getCountry() {
    this.service.getCountry()
      .pipe().subscribe(
        response => {
          if (response && response.result == 1)
            this.countryList = response.countryList;
          else
            this.error = response.result;
          this.countryLoading = false;
        },
        error => {
          this.setError(error);
          this.countryLoading = false;
        });
  }
  init(id?) {
    this.loading = true;
    this.model = new EditLabModel();
    this.selectedCustomerList = [];
    this.validator.isSubmitted = false;
    this.addressValidators = [];
    this.contactValidators = [];
    if (id > 0) {
      this.service.getEditLab(id)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.model = this.mapModel(res.labDetails);
            }
            else {
              this.error = res.result;
            }
          },
          error => {
            this.setError(error);
            this.loading = false;
          });
    }
    if (!(id > 0)) {
      if (this.model.addressList == null || this.model.addressList.length == 0)
        this.addAddress();
      if (this.model.customerContactList == null || this.model.customerContactList.length == 0)
        this.addContact();
    }
    this.loading = false;
  }
  mapModel(labDetails: any): EditLabModel {
    var model: EditLabModel = {
      id: labDetails.id,
      comment: labDetails.comment,
      contactPersonName: labDetails.contactPersonName,
      fax: labDetails.fax,
      glCode: labDetails.glCode,
      legalName: labDetails.legalName,
      mobile: labDetails.mobile,
      locLanguageName: labDetails.locLanguageName,
      name: labDetails.name,
      typeId: labDetails.typeId == null ? 0 : labDetails.typeId,
      webSite: labDetails.webSite,
      email: labDetails.email,
      phone: labDetails.phone,
      addressList: labDetails.addressList.map((x) => {
        var address: LabAddress = {
          id: x.id,
          countryId: x.countryId,
          regionId: x.regionId,
          cityId: x.cityId,
          zipCode: x.zipCode,
          way: x.way,
          addressTypeId: x.addressTypeId == null ? 0 : x.addressTypeId,
          cityList: [],
          regionList: [],
          localLanguage: x.localLanguage
        };
        this.addressValidators.push({ address: address, validator: Validator.getValidator(address, "lab/edit-address.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
        this.refreshRegions(x.countryId, address);
        this.refreshCities(x.regionId, address);
        return address;
      }),
      customerList: labDetails.customerList.map((y) => {
        var customer: LabCustomer = {
          id: y.id,
          name: y.name,
          code: y.code
        };
        return customer;
      }),
      customerContactList: labDetails.customerContactList.map((x) => {
        var customerContact: CustomerContact = {
          comment: x.comment,
          contactEmail: x.contactEmail,
          contactId: x.contactId,
          contactName: x.contactName,
          customerList: x.customerList.map((y) => {
            var customer: LabCustomer = {
              id: y.id,
              name: y.name,
              code: y.code
            };
            return customer;
          }),
          fax: x.fax,
          jobTitle: x.jobTitle,
          mobile: x.mobile,
          phone: x.phone
        };
        this.contactValidators.push({ customerContact: customerContact, validator: Validator.getValidator(customerContact, "lab/edit-contact.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
        return customerContact;
      }),
    };
    return model;
  }
  refreshRegions(countryId, item: LabAddress) {
    this.service.getStates(countryId)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.regionList = res.data;
          }
          else {
            item.regionList = [];
          }
        },
        error => {
          item.regionList = [];
          this.setError(error);
        });
  }
  refreshCities(stateId, item: LabAddress) {
    this.service.getCities(stateId)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.cityList = res.data;
          }
          else {
            item.cityList = [];
          }
        },
        error => {
          item.cityList = [];
        });
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    for (let item of this.addressValidators) {
      item.validator.isSubmitted = true;
    }
    for (let item of this.contactValidators) {
      item.validator.isSubmitted = true;
    }
    if (this.isFormValid()) 
    {
      this._saveloader = true;
      var _serviceCall;
      if (this.model.id > 0) {
        _serviceCall = this.service.updateLab(this.model);
      }
      else
        _serviceCall = this.service.saveLab(this.model);
      _serviceCall.subscribe(
        res => {
          if (res && res.result == 1) {
            this.showSuccess('EDIT_LAB.SAVE_RESULT', 'EDIT_LAB.SAVE_OK');
            if (this.fromSummary)
              this.return('labsearch/lab-summary');
            else
              this.init();
          }
          else {
            switch (res.result) {
              case 2:
                this.showError('EDIT_LAB.SAVE_RESULT', 'EDIT_LAB.MSG_CANNOT_ADDLAB');
                break;
              case 3:
                this.showError('EDIT_LAB.SAVE_RESULT', 'EDIT_LAB.MSG_CURRENTLAB_NOTFOUND');
                break;
              case 4:
                this.showError('EDIT_LAB.SAVE_RESULT', 'EDIT_LAB.MSG_LAB_EXISTS');
                break;
            }
          }
          this._saveloader = false;
        },
        error => {
          this._saveloader = false;
          if (error == "Unauthorized")
            this.showError('EDIT_LAB.SAVE_RESULT', 'ERROR.MSG_MESSAGE_401');
          else
            this.showError('EDIT_LAB.SAVE_RESULT', 'EDIT_LAB.MSG_UNKNONW_ERROR');
        });
    }
  }
  removeAddress(index) {
    this.model.addressList.splice(index, 1);
    this.addressValidators.splice(index, 1);
  }
  addAddress() {
    var address: LabAddress = {
      id: 0,
      countryId: null,
      cityId: null,
      cityList: [],
      regionId: null,
      regionList: [],
      way: '',
      zipCode: '',
      addressTypeId: null,
      localLanguage: ''
    };
    this.model.addressList.push(address);
    this.addressValidators.push({ address: address, validator: Validator.getValidator(address, "lab/edit-address.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
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
      customerList: []
    };
    this.model.customerContactList.push(customerContact);
    this.contactValidators.push({ customerContact: customerContact, validator: Validator.getValidator(customerContact, "lab/edit-contact.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
  }
  removeContact(index) {
    this.model.customerContactList.splice(index, 1);
    this.contactValidators.splice(index, 1);
  }
  isFormValid() {
    return this.validator.isValid('name')
      //&& this.validator.isValid('email')
      && this.validator.isValid('typeId')
      && this.validator.isValid('contactPersonName')
      && this.addressValidators.every((x) => x.validator.isValid('countryId')
        && x.validator.isValid('regionId')
        && x.validator.isValid('cityId')
        && x.validator.isValid('zipCode')
        && x.validator.isValid('way')
        && x.validator.isValid('addressTypeId')
        && x.validator.isValid('localLanguage'))
      && this.contactValidators.every((x) => x.validator.isValid('contactName')
        && x.validator.isValid('contactEmail')
        && x.validator.isValid('customerList'))
      && this.checkCustomerContact()
      && this.checkCustomerMsgShow()
      && this.checkCustomer()
  }

  addCustomer() {
    if (this.selectedCustomerList != null) {
      for (let item of this.selectedCustomerList) {
        if (!this.model.customerList.some(x => x.id == item.id)) {
          var customer: LabCustomer = {
            id: item.id,
            name: item.name,
            code: item.code
          };
          this.model.customerList = [...this.model.customerList, customer];
        }
      }
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
  checkCustomerMsgShow(): boolean {
    if (!(this.model.customerList != null && this.model.customerList.length > 0)) {
      this.showWarning('COMMON.MSG_ERRORVALIDATION', 'EDIT_LAB.MSG_CUSTOMER_NOT_SELECTED');
      return false;
    }
    else
    return true;
  }
  checkCustomerContact(): boolean {
    if (!this.validator.isSubmitted)
      return true;
    if (this.model.customerContactList == null || this.model.customerList.length == 0)
      return true;
    if (this.model.customerList == null || this.model.customerList.length == 0)
      return false;
    let ids = this.model.customerList.map(x => x.id);
    for (let item of this.model.customerContactList) {
      if (item.customerList.some(x => ids.indexOf(x.id) < 0)) {
        this.showWarning('COMMON.MSG_ERRORVALIDATION', 'EDIT_LAB.MSG_CUSTOMERCONTACT_NOTIN_CUSTOMER');
        return false;
      }
    }
    return true;
  }
  getViewPath(): string {
    return "labedit/view-lab";
  }
  getEditPath(): string {
    return "labedit/edit-lab";
  }
  public getAddPath(): string {
    return "labedit/new-lab";
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

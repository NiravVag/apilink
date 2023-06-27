import {
  Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges,
  ViewChildren, AfterViewInit, QueryList
} from '@angular/core';
import { EditDfCustomerConfiguration, DfControlAttributes, EditDfCustomerConfigurationMaster, StringKeyValuePair } from '../../../_Models/dynamicfields/editdfcustomerconfiguration.model'
import { DfControlTypes } from '../../../_Models/dynamicfields/dfcustomerconfiguration.model'
import { CustomerService } from '../../../_Services/customer/customer.service'
import { DynamicFieldService } from '../../../_Services/dynamicfields/dynamicfield.service'
import { first, retry } from 'rxjs/operators';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from "@angular/router";
import { DetailComponent } from '../../common/detail.component';
import { ControlType, ControlAttribute, DynamicControlModuleType } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-editdfcustomerconfiguration',
  templateUrl: './editdfcustomerconfiguration.component.html',
  styleUrls: ['./editdfcustomerconfiguration.component.scss']
})

export class EditDfCustomerConfigurationComponent extends DetailComponent {
  public dfCustomerConfigurationModel: EditDfCustomerConfiguration;
  public dfCustomerConfigurationMaster: EditDfCustomerConfigurationMaster;
  public stringKeyValuePair: Array<StringKeyValuePair>;
  _dfControlType = ControlType;
  public error;
  public jsonHelper: JsonHelper;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  saveloading: boolean;
  initialloading: boolean;
  selectedFBSync: boolean;
  showFBReference: boolean = false;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    public utility: UtilityService,
    router: Router,
    public dynamicFieldService: DynamicFieldService,
    public customerService: CustomerService
  ) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("dynamicfields/edit-dfcontrolattibutes.valid.json");
    this.validator.setModelAsync(() => this.dfCustomerConfigurationModel);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    this.dfCustomerConfigurationMaster = new EditDfCustomerConfigurationMaster();
    this.dfCustomerConfigurationModel = new EditDfCustomerConfiguration();
    this.dfCustomerConfigurationModel.controlAttributeList = [];
    /* this.validator.isSubmitted = false;
    this.validator.setJSON("dynamicfields/edit-dfcontrolattibutes.valid.json");
    this.validator.setModelAsync(() => this.dfCustomerConfigurationModel);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false; */
    this._translate = translate;
    this._toastr = toastr;
  }


  onInit(id?: any) {
    this.getCustomerList();
    this.getModuleList();
    this.getControlTypesList();
    this.stringKeyValuePair = [];
    this.dfCustomerConfigurationMaster.dataTypeList = [
      { "id": "1", "name": "Text" },
      { "id": "2", "name": "Numeric" }
    ];
    this.dfCustomerConfigurationMaster.requiredTypeList = [
      { "id": 1, "name": "Yes" },
      { "id": 0, "name": "No" }
    ];
    this.dfCustomerConfigurationMaster.dropdownTypeList = [
      { "id": "1", "name": "Single" },
      { "id": "2", "name": "Multiple" }
    ];
    if (id) {
      this.getDFCustomerConfiguration(id);
    }
  }

  getViewPath(): string {
    return "cusedit/view-customer";
  }

  getEditPath(): string {
    return "cusedit/edit-customer";
  }

  mapModel(dfCustomerConfiguration) {
    this.dfCustomerConfigurationModel.id = dfCustomerConfiguration.id;
    this.dfCustomerConfigurationModel.customerId = dfCustomerConfiguration.customerId;
    this.dfCustomerConfigurationModel.moduleId = dfCustomerConfiguration.moduleId;
    this.dfCustomerConfigurationModel.controlTypeId = dfCustomerConfiguration.controlTypeId;
    this.dfCustomerConfigurationModel.label = dfCustomerConfiguration.label;
    this.dfCustomerConfigurationModel.displayOrder = dfCustomerConfiguration.displayOrder;
    this.dfCustomerConfigurationModel.dataSourceType = dfCustomerConfiguration.dataSourceType;
    this.dfCustomerConfigurationModel.controlAttributeList = dfCustomerConfiguration.controlAttributeList;

  }

  getDFCustomerConfiguration(id) {

    this.dynamicFieldService.getDFCustomerConfiguration(id)
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.dfCustomerConfigurationModel = data.dfCustomerConfiguration;
            this.getDDLSourceTypeList(this.dfCustomerConfigurationModel.customerId);
            this.getParentDDLSourceList(this.dfCustomerConfigurationModel.customerId);
            if (this.dfCustomerConfigurationModel.fbReference) {
              this.selectedFBSync=true;
              this.showFBReference=true;
            }
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  getCustomerList() {

    this.customerService.getCustomerSummary()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.dfCustomerConfigurationMaster.customerList = data.customerList;
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  getModuleList() {

    this.dynamicFieldService.getModules()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.dfCustomerConfigurationMaster.moduleList = data.moduleList;
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  getControlTypesList() {

    this.dynamicFieldService.getControlTypes()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.dfCustomerConfigurationMaster.controlTypeList = data.controlTypeList;
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  getDDLSourceTypeList(customerId) {

    this.dynamicFieldService.getddlsourcetypes(customerId)
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.dfCustomerConfigurationMaster.dataSourceTypeList = data.ddlSourceTypeList;
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  getDfControlTypeAttributes(controlTypeId) {
    this.dynamicFieldService.getdfcontrolattributes(controlTypeId)
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.dfCustomerConfigurationMaster.dfControlAttributes = data.dfControlTypeAttributes;
            this.addControlAttributes(this.dfCustomerConfigurationMaster.dfControlAttributes);
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  getDDLSourceList(typeId) {

    this.dynamicFieldService.getddlsourcelist(typeId)
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.dfCustomerConfigurationMaster.dataSourceList = data.ddlSourceList;
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  changeCustomer() {
    if (this.dfCustomerConfigurationModel.customerId) {
      this.getDDLSourceTypeList(this.dfCustomerConfigurationModel.customerId);
    }

  }

  changeDataSourceType() {
    if (this.dfCustomerConfigurationModel.dataSourceType) {
      this.getDDLSourceList(this.dfCustomerConfigurationModel.dataSourceType);
    }
  }

  changeControlTypes() {
    if (this.dfCustomerConfigurationModel.controlTypeId) {
      this.getDfControlTypeAttributes(this.dfCustomerConfigurationModel.controlTypeId);
    }

  }

  clearControlTypes() {
    this.dfCustomerConfigurationModel.controlAttributeList = [];
  }

  addControlAttributes(dfControlTypeAttributes) {
    this.dfCustomerConfigurationModel.controlAttributeList = [];
    dfControlTypeAttributes.forEach(element => {
      var dfControlAttribute: DfControlAttributes = {
        id: 0,
        name: element.name,
        dataType: element.dataType,
        value: element.defaultValue,
        controlTypeId: element.controlTypeId,
        controlAttributeId: element.id,
        attributeId: element.attributeId,
        active: true
      };
      this.dfCustomerConfigurationModel.controlAttributeList.push(dfControlAttribute);
      /* this.dfCustomerConfigurationMaster.controlAttributes.push({
        dfControlAttribute: dfControlAttribute,
        validator: Validator.getValidator(dfControlAttribute, "dynamicfields/edit-dfcontrolattibutes.valid.json",
          this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
      }); */
    });



  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.isFormValid()) {
      this.dynamicFieldService.saveDFCustomerConfiguration(this.dfCustomerConfigurationModel)
        .subscribe(
          res => {

            if (res && res.result == 1) {
              //this.waitingService.close();
              this.dfCustomerConfigurationModel = new EditDfCustomerConfiguration();
              this.dfCustomerConfigurationModel.controlAttributeList = [];
              this.showSuccess('EDIT_CUSTOMER.SAVE_RESULT', 'Customer Configuration saved successfully');
              this.return('dfcustomerconfigsummary/dfcustomerconfig-summary');
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_CANNOT_ADDCUSTOMER');
                  break;
                case 3:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_CURRENTSUPP_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_CUSTOMER_EXISTS');
                  break;
                case 5:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'Parent-DropDown cannot be empty for cascading dropdown');
                  break;
              }

              //this.waitingService.close();
            }
          },
          error => {
            this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_UNKNONW_ERROR');
            //this.waitingService.close();
          });
    }
  }

  isFormValid() {
    var isOk = this.validator.isValid('moduleId')
      && this.validator.isValid('customerId')
      && this.validator.isValid('controlTypeId')
      && this.validator.isValid('label')
      && this.validator.isValid('displayOrder');
    if (isOk) {
      if (this.dfCustomerConfigurationModel.controlTypeId == this._dfControlType.DropDown) {
        isOk = this.validator.isValid('dataSourceType');
      }
    }
    if (isOk) {
      if (this.selectedFBSync) {
        isOk = this.validator.isValid('fbReference');
      }
    }
    return isOk;
  }

  changeIscascading(item) {
    item.value = !item.value;
    if (item && item.controlTypeId == 3 && item.controlAttributeId == 20) {
      if (item.value) {
        this.getParentDDLSourceList(this.dfCustomerConfigurationModel.customerId);
      }
    }
  }

  getParentDDLSourceList(customerId) {

    this.dynamicFieldService.getparentddlsourcelist(customerId)
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            if (data.dfParentDDLList) {
              this.stringKeyValuePair = [];
              data.dfParentDDLList.forEach(element => {
                var keyValuePair = new StringKeyValuePair();
                keyValuePair.id = String(element.id);
                keyValuePair.name = element.name;
                this.stringKeyValuePair.push(keyValuePair);
              });

            }
            this.dfCustomerConfigurationMaster.parentDropdownTypeList = this.stringKeyValuePair;
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          //this.setError(error);

        });
  }

  changeCheckBox(item) {
    if (item.value == "true") {
      item.value = "false"
    }
    else {
      item.value = "true"
    }

    //item.value = !item.value;
    if (item && item.controlTypeId == 3 && item.controlAttributeId == 20) {
      this.dfCustomerConfigurationMaster.parentDropdownTypeList = [];
      if (item.value == "true") {
        this.getParentDDLSourceList(this.dfCustomerConfigurationModel.customerId);
      }
    }
  }
  //Change the fb sync checkbox
  selectFBSync() {
    if (this.selectedFBSync) {
      this.showFBReference = true;
    }
    else {
      this.dfCustomerConfigurationModel.fbReference="";
      this.showFBReference = false;
    }
  }



}

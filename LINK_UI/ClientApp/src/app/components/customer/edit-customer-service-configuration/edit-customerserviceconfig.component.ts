import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from "@angular/router";
import { first, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { EditCustomerServiceConfigModel } from '../../../_Models/customer/edit-customerserviceconfig.model';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { CustomerServiceConfig } from 'src/app/_Services/customer/customerserviceconfig.service';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-customerserviceconfiguration',
  templateUrl: './edit-customerserviceconfig.component.html',
  styleUrls: ['./edit-customerserviceconfig.component.css']
})

export class EditCustomerServiceConfigComponent extends DetailComponent {

  public model: EditCustomerServiceConfigModel;
  public jsonHelper: JsonHelper;
  serviceList: Array<any> = [];
  serviceTypeList: Array<any> = [];
  productCategoryList: Array<any> = [];
  pickTypeList: Array<any> = [];
  levelPickFirstList: Array<any> = [];
  levelPickSecondList: Array<any> = [];
  pickFirstList: Array<any> = [];
  pickSecondList: Array<any> = [];
  pickThirdList: Array<any> = [];
  defectClassificationList: Array<any> = [];
  reportUnitList: Array<any> = [];
  dpPointList: Array<any> = [];
  initialLoading = false;
  savedataloading = false;
  serviceTypeLoading = false;

  public id: number;
  serviceType: string;
  pickType: number;
  levelPick1: number;
  levelPick2: number;
  criticalPick1: number;
  criticalPick2: number;
  majorTolerancePick1: number;
  majorTolerancePick2: number;
  minorTolerancePick1: number;
  minorTolerancePick2: number;
  allowAQLModification: boolean;
  defectClassification: boolean;
  checkMeasurementPoints: boolean;
  reportUnit: number;
  productCategory: number;
  serviceValue: number;
  isProductCategory: boolean = true;
  public isDoubleData: boolean = true;
  public customerID: number;
  public serviceConfigID: number;
  public configServiceData: any;
  public customerList: Array<any> = [];

  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public utility: UtilityService,
    public service: CustomerServiceConfig,
    public customerService: CustomerService, private referenceService: ReferenceService) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("customer/edit-customerserviceconfig.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.model = new EditCustomerServiceConfigModel();
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
  }

  getViewPath(): string {
    return "cusserviceedit/view-customer-serviceconfig";
  }

  getEditPath(): string {
    return "cusserviceedit/edit-customer-serviceconfig";
  }



  onInit(id?: any, inputparam?: ParamMap): void {
    if (inputparam && inputparam.has("customerid")) {
      this.customerID = Number(inputparam.get("customerid"))
    }
    this.serviceConfigID = id;
    if (id)
      this.Intialize(id);
  }

  getCustomerList() {
    this.customerService.getCustomerSummary()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.customerList = response.customerList;
          }
          else {
            this.error = response.result;
          }
          this.loading = false;

        },
        error => {
          //this.setError(error);
          this.loading = false;
        });
  }



  Intialize(id?) {

    this.initialLoading = true;
    this.getCustomerList();
    this.service.getCustomerServiceConfigMaster()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {

            this.serviceList = res.serviceList;
            //this.serviceTypeList=res.serviceTypeList;
            this.productCategoryList = res.productCategoryList;
            this.pickTypeList = res.pickTypeList;
            this.levelPickFirstList = res.levelPick1List;
            this.levelPickSecondList = res.levelPick2List;
            this.pickFirstList = res.pick1List;
            this.pickSecondList = res.pick2List;
            this.defectClassificationList = res.defectClassificationList;
            this.reportUnitList = res.reportUnitList;
            this.dpPointList = res.dpPointList;
            if (id == 0) {
              var model: EditCustomerServiceConfigModel = {
                id: 0,
                serviceType: null,
                pickType: null,
                levelPick1: null,
                levelPick2: null,
                criticalPick1: null,
                criticalPick2: null,
                majorTolerancePick1: null,
                majorTolerancePick2: null,
                minorTolerancePick1: null,
                minorTolerancePick2: null,
                allowAQLModification: false,
                ignoreAcceptanceLevel:null,
                defectClassification: null,
                checkMeasurementPoints: null,
                reportUnit: null,
                productCategory: null,
                service: null,
                active: true,
                customerID: this.customerID,
                customerName: null,
                customServicetypeName: null,
                customerRequirementIndex: 1,
                dpPoint: null

              }
              this.model = model;
              this.configServiceData = this.model;
              this.isProductCategory = false;
            }


            if (id != 0) {
              let serviceObj = this.serviceList.find(i => i.id === this.model.service);
              if (serviceObj.name != "Audit") {
                this.isProductCategory = false;
              }

              let pickType = this.pickTypeList.find(i => i.id === this.model.pickType);
              if (pickType.value == "Single") {
                this.isDoubleData = true;
              }
              else if (pickType.value == "Double") {
                this.isDoubleData = false;
              }
            }
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

    if (id != 0) {
      this.service.getEditCustomerServiceConfigDetail(id)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 1) {

              if (id) {
                this.model = this.mapModel(res.customerServiceConfigData, this.customerID);

                this.getServiceTypesList();
                this.configServiceData = this.model;
              }
              this.initialLoading = false;

            }
            else {
              this.error = res.result;
              this.initialLoading = false;
            }
            this.initialLoading = false;
          },
          error => {
            this.setError(error);
            this.initialLoading = false;
          });
    }
    else {
      this.initialLoading = false;
    }





    /*  this.model = new EditCustomerContactModel();
     this.data = {}; */
    this.validator.isSubmitted = false;
  }

  mapModel(customerServiceConfig: any, customerID: number): EditCustomerServiceConfigModel {
    var model: EditCustomerServiceConfigModel = {
      id: customerServiceConfig.id,
      serviceType: customerServiceConfig.serviceType,
      pickType: customerServiceConfig.pickType,
      levelPick1: customerServiceConfig.levelPick1,
      levelPick2: customerServiceConfig.levelPick2,
      criticalPick1: customerServiceConfig.criticalPick1,
      criticalPick2: customerServiceConfig.criticalPick2,
      majorTolerancePick1: customerServiceConfig.majorTolerancePick1,
      majorTolerancePick2: customerServiceConfig.majorTolerancePick2,
      minorTolerancePick1: customerServiceConfig.minorTolerancePick1,
      minorTolerancePick2: customerServiceConfig.minorTolerancePick2,
      allowAQLModification: customerServiceConfig.allowAQLModification,
      ignoreAcceptanceLevel: customerServiceConfig.ignoreAcceptanceLevel,
      defectClassification: customerServiceConfig.defectClassification,
      checkMeasurementPoints: customerServiceConfig.checkMeasurementPoints,
      reportUnit: customerServiceConfig.reportUnit,
      productCategory: customerServiceConfig.productCategory,
      service: customerServiceConfig.service,
      active: customerServiceConfig.active,
      customerID: customerID,
      customerName: customerServiceConfig.customerName,
      customServicetypeName: customerServiceConfig.customServiceTypeName,
      customerRequirementIndex: customerServiceConfig.customerRequirementIndex,
      dpPoint: customerServiceConfig.dpPoint
    };
    return model;
  }

  selectDepartment(event: any) {
    //update service Type list ddl
    this.model.serviceType = null;
    if (event) {
      this.getServiceTypesList();
    }
    else {
      this.serviceTypeList = [];
    }

    let serviceObj = this.serviceList.find(i => i.id === event);
    if (serviceObj.name == "Audit") {
      this.isProductCategory = true;
      this.model.productCategory = null;
    }
    else if (serviceObj.name != "Audit") {
      this.isProductCategory = false;
    }


  }

  doCheck(event: any) {
    this.model.allowAQLModification = event.target.checked;
  }

  changePickType() {
    let pickType = this.pickTypeList.find(i => i.id === this.model.pickType);
    if (pickType.value == "Single") {
      this.isDoubleData = true;
      this.model.criticalPick2 = null;
      this.model.majorTolerancePick2 = null;
      this.model.minorTolerancePick2 = null;
    }
    else if (pickType.value == "Double") {
      this.isDoubleData = false;

    }
  }

  reset() {
    /* if (this.serviceConfigID)
    this.Intialize(this.serviceConfigID); */
    this.model = new EditCustomerServiceConfigModel();
  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.isFormValid()) {
      this.savedataloading = true;
      //this.waitingService.open();

      this.service.saveCustomerServiceConfig(this.model)
        .subscribe(
          res => {

            if (res && res.result == 1) {
              this.savedataloading = false;
              this.showSuccess('EDIT_CUSTOMER_SERVICECONFIG.SAVE_RESULT', 'EDIT_CUSTOMER_SERVICECONFIG.SAVE_OK');
              this.return('cusservicesearch/customer-serviceconfig');
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CUSTOMER_SERVICECONFIG.SAVE_RESULT', 'EDIT_CUSTOMER_SERVICECONFIG.MSG_CANNOT_ADDCUSTOMERSERVICECONFIG');
                  break;
                case 3:
                  this.showError('EDIT_CUSTOMER_SERVICECONFIG.SAVE_RESULT', 'EDIT_CUSTOMER_SERVICECONFIG.MSG_CURRENTBRAND_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_CUSTOMER_SERVICECONFIG.SAVE_RESULT', 'EDIT_CUSTOMER_SERVICECONFIG.MSG_CUSTOMERBRAND_EXISTS');
                  break;
              }
              this.savedataloading = false;

              //this.waitingService.close();
            }
          },
          error => {
            this.savedataloading = false;
            this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_UNKNONW_ERROR');
            //this.waitingService.close();
          });
    }
  }

  isFormValid() {

    let service = this.serviceList.find(i => i.id === this.model.service);
    if (service) {
      if (service.name == "Inspection") {
        return (this.validator.isValid('service')
          && this.validator.isValid('serviceType'));
      }
      else if (service.name == "Audit") {
        return (this.validator.isValid('service')
          && this.validator.isValid('serviceType'));
      }
    }
    else {
      return (this.validator.isValid('service'));
    }


  }
  getServiceTypesList() {

    this.serviceTypeLoading = true;
    let request = this.generateServiceTypeRequest();
    this.referenceService.getServiceTypes(request)
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.serviceTypeList = response.serviceTypeList;
            }
            else {
              this.serviceTypeList = [];
            }
            this.serviceTypeLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.serviceTypeList = [];
          this.serviceTypeLoading = false;
        });
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.serviceId = this.model.service ?? 0;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  customerReqIndexValidation(event, length) {
    this.utility.decimalValidation(event, length);
  }

  validateCustomerRequirementIndex(event) {
    if (event.target.value < 0)
      event.target.value = null;

    if (!event.target.value)
      event.target.value = null;
  }

}

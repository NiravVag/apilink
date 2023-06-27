import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { CustomerContactService } from '../../../_Services/customer/customercontact.service'
import { CustomerSummaryItemModel } from '../../../_Models/customer/cutomersummary.model';
import { EditCustomerContactModel } from '../../../_Models/customer/edit-customercontact.model';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { ReferenceService } from '../../../_Services/reference/reference.service';
import { APIService, ListSize, RoleEnum } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, DataSource, ResponseResult } from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { BehaviorSubject, of } from 'rxjs';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';

@Component({
  selector: 'app-editCustomerContact',
  templateUrl: './edit-customercontact.component.html',
  styleUrls: ['./edit-customercontact.component.css']
})
export class EditCustomerContactComponent extends DetailComponent {
  public model: EditCustomerContactModel;
  public data: any;
  loading = false;
  error: '';
  public selectCounryId: number = 0;
  public isDetails: boolean = true;
  public jsonHelper: JsonHelper;
  public addressValidators: Array<any> = [];
  public contactValidators: Array<any> = [];
  public action: string;
  public promotionalEmailList: Array<any>;
  public apiServiceList: any;
  public apiEntityList: any;
  public apiEntityLoading: boolean = false;
  public entityServiceList: any;
  public entityServiceLoading: boolean = false;
  public primaryEntityList: any;
  public primaryEntityLoading: boolean = false;

  customerList: Array<DataSource>;
  customerLoading: boolean;

  public id: number;
  name: string;
  lastName: string;
  jobTitle: string;
  email: string;
  mobile: string;
  phone: string;
  fax: string;
  others: string;
  office: number;
  comments: string;
  contactType: number;
  promotionalEmail: boolean;
  customerId: number;
  active: boolean;
  customerAddressList: any;
  contactTypeList: any;
  contactBrandList: any;
  contactDepartmentList: any;
  contactServiceList: any;
  initialloading: boolean = false;
  saveloading: boolean = false;
  apiServiceEnum = APIService;
  reportToList: any;
  isItTeamRole = false;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    private authService: AuthenticationService,
    public routeCurrent: ActivatedRoute,
    public service: CustomerContactService,
    public utility: UtilityService,
    public customerService: CustomerService,
    public referenceService: ReferenceService) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("customer/edit-customercontactsummary.valid.json");
    this.validator.setModelAsync(() => this.model);

    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    const user = authService.getCurrentUser();
    this.isItTeamRole = user.roles.some(x => x.id === RoleEnum.IT_Team);
  }

  onInit(id?: any, inputparam?: ParamMap) {
    this.promotionalEmailList =
      [{ name: 'Yes', id: 1 },
      { name: 'No', id: 0 }
      ];
    this.model = new EditCustomerContactModel();
    if (id == 0) {
      this.action = "add";
      this.routeCurrent.queryParams.subscribe(
        params => {
          if (params != null && params['paramParent'] != null) {
            this.paramParent = params['paramParent'];
            var paramParent = decodeURI(this.paramParent);
            var parentObj = JSON.parse(paramParent);
            id = parentObj.customerValue;
          }
        }
      );

    }
    else if (id != 0) {
      this.action = "edit";
    }
    this.init(id);
  }

  getViewPath(): string {
    return "cuscontactedit/view-customer-contact";
  }

  getEditPath(): string {
    return "cuscontactedit/edit-customer-contact";
  }

  isFactory(): boolean {
    return this.model != null && this.model.id == 1;
  }

  init(id?) {
    this.initialloading = true;
    this.getAPIServices();

    if (this.action == "add") {
      this.service.getAddCustomerContactSummary(id)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.data = res;
              if (id) {
                this.customerAddressList = res.customerAddressList;
                this.contactTypeList = res.customerContactDetails.contactTypeList;
                this.contactBrandList = res.contactBrandList;
                this.contactDepartmentList = res.contactDepartmentList;
                this.contactServiceList = res.contactServiceList;
                this.reportToList = res.reportToList;
                this.model = this.mapModel(res.customerContactDetails, this.action);
                this.model.customerId = id;
                this.model.contactServiceList = [];
                this.model.contactServiceList.push(this.apiServiceEnum.Inspection);
                this.model.apiEntityIds = [];
                this.initialloading = false;
                this.getCustomerEntityList();
                this.getCustomerSisterCompany();
              }

            }
            else {
              this.error = res.result;
              this.initialloading = false;
            }

          },
          error => {
            this.setError(error);
            this.initialloading = false;
          });
    }
    else if (this.action == "edit") {
      if (id != null) {

        this.service.getEditCustomer(id)
          .pipe()
          .subscribe(
            res => {
              if (res && res.result == 1) {
                this.data = res;

                this.addressValidators = [];
                this.contactValidators = [];
                if (id) {
                  this.customerAddressList = res.customerContactDetails.customerAddressList;
                  this.contactTypeList = res.customerContactDetails.contactTypeList;
                  this.contactBrandList = res.contactBrandList;
                  this.contactDepartmentList = res.contactDepartmentList;
                  this.contactServiceList = res.contactServiceList;
                  this.reportToList = res.reportToList;
                  this.model = this.mapModel(res.customerContactDetails, this.action);
                  this.getCustomerEntityList();
                  this.onChnageEntityServiceList();
                  this.getCustomerSisterCompany();
                }

              }
              else {
                this.error = res.result;
              }
              this.initialloading = false;
            },
            error => {
              this.setError(error);
              this.initialloading = false;
            });
      }
    }

    this.validator.isSubmitted = false;
  }



  mapModel(customerContactDetails: any, action: string): EditCustomerContactModel {
    var model: EditCustomerContactModel = {
      id: customerContactDetails.id,
      name: customerContactDetails.name,
      lastName: customerContactDetails.lastName,
      jobTitle: customerContactDetails.jobTitle,
      email: customerContactDetails.email,
      mobile: customerContactDetails.mobile,
      phone: customerContactDetails.phone,
      fax: customerContactDetails.fax,
      others: customerContactDetails.others,
      office: (action == "add") ? null : customerContactDetails.office,
      comments: customerContactDetails.comments,
      contactTypes: customerContactDetails.contactTypes,
      contactTypeItems: this.getcontactTypeValues(customerContactDetails.contactTypes),
      promotionalEmail: (action == "add") ? null : (customerContactDetails.promotionalEmail == true) ? 1 : 0,
      customerId: customerContactDetails.customerID,
      active: customerContactDetails.active,
      customerAddressList: customerContactDetails.customerAddressList,
      contactBrandList: customerContactDetails.contactBrandList,
      contactDepartmentList: customerContactDetails.contactDepartmentList,
      contactServiceList: customerContactDetails.contactServiceList,
      apiEntityIds: customerContactDetails.apiEntityIds,
      entityServiceIds: customerContactDetails.entityServiceIds,
      primaryEntity: customerContactDetails.primaryEntity ? customerContactDetails.primaryEntity : null,
      reportTo: customerContactDetails.reportTo ? customerContactDetails.reportTo : null,
      contactSisterCompanyIds: customerContactDetails.contactSisterCompanyIds
    };

    return model;
  }


  getcontactTypeValues(values) {
    if (this.contactTypeList == null || values == null)
      return [];
    return this.contactTypeList.filter(x => values.indexOf(x.id) >= 0).map(x => { return { id: x.id, type: x.type } });
  }

  save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.addressValidators)
      item.validator.isSubmitted = true;

    if (this.isFormValid() && this.validateEntityService()) {
      this.saveloading = true;
      //this.waitingService.open();
      this.model.contactTypes = this.getValues(this.model.contactTypeItems);
      this.service.saveCustomerContact(this.model)
        .subscribe(
          res => {

            if (res && res.result == 1) {
              this.saveloading = false;
              this.showSuccess('EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_RESULT', 'EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_OK');
              this.return('cuscontactsearch/customer-contact-summary');
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_RESULT', 'EDIT_CUSTOMER_CONTACT_SUMMARY.MSG_CANNOT_ADDCONTACT');
                  break;
                case 3:
                  this.showError('EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_RESULT', 'EDIT_CUSTOMER_CONTACT_SUMMARY.MSG_CONTACT_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_RESULT', 'EDIT_CUSTOMER_CONTACT_SUMMARY.MSG_CUSTOMER_EXISTS');
                  break;
                case 6:
                  this.showError('EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_RESULT', 'EDIT_CUSTOMER_CONTACT_SUMMARY.MSG_EMAIL_ID_EXISTS');
                  break;
              }
              this.saveloading = false;

            }
          },
          error => {
            this.showError('EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_RESULT', 'EDIT_CUSTOMER_CONTACT_SUMMARY.MSG_UNKNONW_ERROR');
            this.saveloading = false;
          });
    }
  }


  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }

  isFormValid() {

    return (this.validator.isValid('name')
      && this.validator.isValid('email')
      && this.validator.isValid('office')
      && this.validator.isValid('contactTypeItems')
      && this.validator.isValid('contactServiceList')
      && this.validator.isValid('apiEntityIds')
      && this.validator.isValid('entityServiceIds')
      && this.validator.isValid('primaryEntity'));

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

  //get customer entity list
  getCustomerEntityList() {
    this.apiEntityLoading = true;
    this.customerService.getCustomerEntityList(this.model.customerId)
      .pipe(first())
      .subscribe(
        response => {
          this.apiEntityLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.apiEntityList = response.dataSourceList;
            this.onChnageEntityServiceList();
          }
        },
        error => {
          this.apiEntityLoading = false;
          this.setError(error);
        });
  }

  onClearEntityList() {
    this.model.entityServiceIds = null;
    this.model.primaryEntity = null;
  }

  onClearServiceList() {
    this.model.entityServiceIds = null;
    this.model.primaryEntity = null;
  }

  onChnageEntityServiceList() {
    this.entityServiceList = [];
    this.primaryEntityList = [];
    if (this.model.contactServiceList && this.model.apiEntityIds && this.apiEntityList && this.apiServiceList) {
      let index = 1;

      this.primaryEntityList = [];

      this.primaryEntityList = this.apiEntityList.filter(x => this.model.apiEntityIds.includes(x.id));
      if (this.primaryEntityList.find(x => x.id == this.model.primaryEntity) == undefined) {
        this.model.primaryEntity = null;
      }

      this.model.contactServiceList.forEach(service => {
        this.model.apiEntityIds.forEach(entity => {
          var entityName = this.apiEntityList.find(x => x.id == entity).name;
          var serviceName = this.apiServiceList.find(x => x.id == service).name;
          var entityService = entityName + "(" + serviceName + ")";

          this.entityServiceList.push({ id: index, entityId: entity, serviceId: service, name: entityService });
          index = index + 1;
        })
      })

      this.model.entityServiceIds = this.model.entityServiceIds.filter(y => this.model.apiEntityIds.includes(y.entityId) && this.model.contactServiceList.includes(y.serviceId))
    }
  }

  validateEntityService() {
    let result = this.model.apiEntityIds.every(y => this.model.entityServiceIds.map(z => z.entityId).includes(y))

    if (!result) {
      this.showWarning('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_ENTITY_SERVICE_REQUIRED');
    }
    return result;
  }

  getCustomerSisterCompany() {
    this.customerService
      .getCustomerSisterCompany(this.model.customerId)
      .subscribe((data) => {
        this.customerList = data.dataSourceList;
        this.customerLoading = false;
      });
  }
}




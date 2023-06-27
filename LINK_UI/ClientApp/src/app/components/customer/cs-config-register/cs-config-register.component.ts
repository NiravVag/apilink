import { Component } from '@angular/core';
import { CSConfigService } from '../../../_Services/customer/csconfig.service';
import { first } from 'rxjs/operators';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common';
import { Router, ActivatedRoute } from '@angular/router';
import { editCSConfigModel } from '../../../_Models/customer/csconfig-register.model';
import { DetailComponent } from '../../common/detail.component';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-cs-config-register',
  templateUrl: './cs-config-register.component.html',
  styleUrls: ['./cs-config-register.component.css']
})

export class CSConfigRegisterComponent extends DetailComponent {
  public model: editCSConfigModel;
  public editModel: editCSConfigModel;
  public modelRef: NgbModalRef;
  initialLoading: boolean = false;
  searchLoading: boolean = false;
  serviceLoading: boolean = false;
  customerServiceLoading: boolean = false;
  productCategoryLoading: boolean = false;
  officeLoading: boolean = false;
  customerServiceList: Array<any> = [];
  customerList: Array<any> = [];
  serviceList: Array<any> = [];
  officeList: Array<any> = [];
  productCategoryList: Array<any> = [];
  public currentUser: UserModel;
  private _toastr: ToastrService;
  saveloading: boolean = false;
  public _productcategorystyle:boolean=false;
  constructor(public csConfigService: CSConfigService, public modalService: NgbModal,
    public routeCurrent: ActivatedRoute, public routerCurrent: Router,
    toastr: ToastrService,
    private authService: AuthenticationService,
    public utility: UtilityService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService) {
    super(router, route, translate, toastr);
    this.validator.setJSON("customer/csconfig-register.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model = new editCSConfigModel();
    this.currentUser = authService.getCurrentUser();
  }

  onInit(id?: number) {
    this.initialLoading = true;
    this.serviceLoading = true;
    this.customerServiceLoading = true;
    this.productCategoryLoading = true;
    this.getCustomerList();
    this.getProductCategoryList();
    this.getCustomerSerivce();
    this.getService();
    this.init(id);
  }

  init(id?) {
    this.editModel = new editCSConfigModel();
    if (id != null && id > 0) {
      this.csConfigService.editCSConfigRegister(id)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.editModel = this.mapModel(res.csConfigDetails);
              this.model.userId = this.editModel.userId;
              this.model.productCategoryId = [];
              this.model.officeLocationId = [];
              this.model.serviceId = [];
              this.model.customerId=[];
              if (this.editModel.productCategoryId != null)
                this.model.productCategoryId.push(this.editModel.productCategoryId);
              if (this.editModel.serviceId != null)
                this.model.serviceId.push(this.editModel.serviceId);
              this.model.officeLocationId.push(this.editModel.officeLocationId);
              if (this.editModel.customerId != null)
              this.model.customerId.push(this.editModel.customerId);
              this.model.id = this.editModel.id;
              this.ToogleProductCategory(this.model.serviceId);
              this.getOfficeList(this.model.userId)
            }
          },
          error => {
            this.setError(error);
          });
    }
    else this.fromSummary = false;
  }

  mapModel(customerDetails: any): editCSConfigModel {
    var modelEdit: editCSConfigModel = {
      id: customerDetails.id,
      userId: customerDetails.userId,
      customerId: customerDetails.customerId,
      productCategoryId: customerDetails.productCategoryId,
      serviceId: customerDetails.serviceId,
      officeLocationId: customerDetails.officeLocationId
    };
    return modelEdit;
  }

  getService() {
    this.csConfigService.getService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.serviceList = response.serviceList;
          }
          else {
            this.error = response.result;
          }
          this.serviceLoading = false;
        },
        error => {
          this.setError(error);
          this.serviceLoading = false;
        });
  }

  getCustomerList() {
    this.csConfigService.getCustomerSummary()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.customerList = response.customerList;
          }
          else {
            this.error = response.result;
          }
          this.initialLoading = false;
        },
        error => {
          this.setError(error);
          this.initialLoading = false;
        });
  }

  getCustomerSerivce() {
    this.csConfigService.getCustomerService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1)
            this.customerServiceList = response.csList;
          else
            this.error = response.result;
          this.customerServiceLoading = false;
        },
        error => {
          this.setError(error);
          this.customerServiceLoading = false;
        });
  }
  getProductCategoryList() {
    this.csConfigService.getProductCategoryList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.productCategoryList = response.productCategoryList;
          }
          else {
            this.error = response.result;
          }
          this.productCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.productCategoryLoading = false;
        });
  }

  getOfficeList(id) {
    this.csConfigService.getOffices(id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {
            this.officeList = response.data;
          }
          else {
            this.officeList = null;
            this.error = response.result;
          }
          this.officeLoading = false;
        },
        error => {
          this.error = error;
          this.officeLoading = false;
        });
  }
  onChangeCusService(event) {
    if (event != null && event != undefined && event.id) {
      this.officeLoading = false;
      this.getOfficeList(event.id);
    }
    else {
      this.officeList = [];
    }
    this.model.officeLocationId = null;
  }
  ToogleProductCategory(lstserviceid: any[]) {
    if (lstserviceid && lstserviceid.length > 0 && lstserviceid.includes(1))//inspection service included
    this._productcategorystyle= true;
    else
    {
      this.model.productCategoryId=[];
      this._productcategorystyle= false;
    }
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.saveloading = true;
      this.csConfigService.saveCSConfigRegister(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.validator.isSubmitted = false;
              this.showSuccess('CS_CONFIG_REGISTER.MSG_SAVE_RESULT', 'CS_CONFIG_REGISTER.MSG_SAVE_OK');
              this.model = new editCSConfigModel();
              this.saveloading = false;
              if (this.fromSummary)
                this.return('csconfigsearch/csconfig-summary');
              else
                this.init(0);
            }
            else {
            }
            this.saveloading = false;
          },
          error => {
            this.saveloading = false;
          });
    }
  }

  formValid(): boolean {
    return this.validator.isValid('userId')
      && this.validator.isValid('officeLocationId');      
  }
  getViewPath(): string {
    return "csconfigsearch/csconfig-summary";
  }
  getEditPath(): string {
    return "csconfigedit/csconfig-register";
  }
  unselectAllCustomer(){
    this.model.customerId =[];
  }
  selectAllCustomer(){
    this.model.customerId = this.customerList.map(item => item.id);
  }
  unselectAllOffice(){
    this.model.officeLocationId=[];
  }
  selectAllOffice(){
    this.model.officeLocationId= this.officeList.map(item => item.id);
  }
}

import { Component } from '@angular/core';
import { first } from 'rxjs/operators';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { csConfigSearchModel, csConfigItem, csDeleteItem } from 'src/app/_Models/customer/csconfig-summary.model';
import { CSConfigService } from '../../../_Services/customer/csconfig.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-cs-config-summary',
  templateUrl: './cs-config-summary.component.html',
  styleUrls: ['./cs-config-summary.component.css']
})
export class CSConfigSummaryComponent extends SummaryComponent<csConfigSearchModel> {
  public model: csConfigSearchModel;
  public modelRemove: csDeleteItem;
  public modelRef: NgbModalRef;
  public selectedAllCSSummary: boolean = false;
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
  deleteBtnShow: boolean = false;
  deleteId: any[] = [];
  public currentUser: UserModel;
  public currentRoute: Router;
  public editPage: string;
  public viewPage: string;
  constructor(public csConfigService: CSConfigService, public modalService: NgbModal,
    authService: AuthenticationService,public utility: UtilityService, router: Router, route: ActivatedRoute, public validator: Validator,
    public routeCurrent: ActivatedRoute, public routerCurrent: Router) {
    super(router, validator, route);
    this.validator.setJSON("customer/csconfig-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model = new csConfigSearchModel();
    this.currentUser = authService.getCurrentUser();
    this.currentRoute = router;
  }
  onInit() {
    this.initialLoading = true;
    this.serviceLoading = true;
    this.customerServiceLoading = true;
    this.productCategoryLoading = true;
    this.getCustomerList();
    this.getProductCategoryList();
    this.getCustomerSerivce();
    this.getService();
    this.getOfficeList(this.currentUser.staffId);
  }
  getService() {
    this.csConfigService.getService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.data = response;
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
            this.data = response;
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
            this.data = response;
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
          if (response && response.result == 1 && response.data && response.data.length > 0)
            this.officeList = response.data;
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
  getData() {
    this.searchLoading = true;
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.csConfigService.SearchCustomerServiceConfigSummary(this.model)
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.mapPageProperties(response);
              this.model.items = response.data.map((x) => {
                var item: csConfigItem = {
                  csConfigId: x.cusServiceConfigId,
                  customerName: x.customerName,
                  office: x.office,
                  customerService: x.customerService,
                  productCatgory: x.productCatgory,
                  service: x.service,
                  selected: false
                }
                return item;
              });
            }
            else if (response && response.result == 2) {
              this.model.noFound = true;
              this.model.items = [];
            }
            else {
              this.error = response.result;
            }
            this.searchLoading = false;
            this.deleteRefresh();
          },
          error => {
            this.setError(error);
            this.searchLoading = false;
          });
    }
    else{
      this.searchLoading = false;
    }
  }
  getPathDetails(): string {
    return "csconfigedit/csconfig-register";
  }

  getEditDetails(id) {
    this.editPage = "csconfigedit/csconfig-register";
    this.viewPage = "csconfigsearch/csconfig-summary";
    this.getDetails(id);
  }

  deleteRefresh() {
    this.modelRemove = new csDeleteItem();
    this.deleteId = [];
    this.deleteBtnShow = false;
    this.selectedAllCSSummary = false;
    for (var i = 0; i < this.model.items.length; i++) {
      this.model.items[i].selected = false;
    }
  }

  selectAllCS() {
    for (var i = 0; i < this.model.items.length; i++) {
      this.model.items[i].selected = this.selectedAllCSSummary;
    }
    this.deleteBtnShow = this.selectedAllCSSummary ? true : false;
    this.deleteId = [];
    if (this.selectedAllCSSummary) {
      for (var i = 0; i < this.model.items.length; i++) {
        if (this.model.items[i].selected == true)
          this.deleteId.push(this.model.items[i].csConfigId);
      }
    }
    else
      this.deleteId = [];
  }
  checkIfAllCSSelected() {
    this.selectedAllCSSummary = this.model.items.every(function (item: any) {
      return item.selected == true;
    });
    for (var i = 0; i < this.model.items.length; i++) {
      if (this.model.items[i].selected == true) {
        this.deleteBtnShow = true;
        break;
      }
      else { this.deleteBtnShow = false; }
    }
    this.deleteId = [];
    for (var i = 0; i < this.model.items.length; i++) {
      if (this.model.items[i].selected == true) {
        this.deleteId.push(this.model.items[i].csConfigId);
      }
    }
  }

  openConfirm(content) {
    this.modelRemove = {
      id: this.deleteId,
    };
    this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  deleteCustomerService(removeModel) {
    this.csConfigService.deleteCSConfig(removeModel)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.getData();
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
  IsCustomerValidationRequired(): boolean {
    if (this.model.userId > 0) 
      return false;
    else
    return true;
  }
  IsUserValidationRequired(): boolean {
    if (this.model.customerId > 0) 
      return false;
    else
    return true;
  }
  formValid(): boolean {
    return this.validator.isValidIf('userId', this.IsUserValidationRequired()) ||
      this.validator.isValidIf('customerId', this.IsCustomerValidationRequired())
  }
}

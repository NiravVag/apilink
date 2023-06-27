import { Component, NgModule, Input, Output, EventEmitter } from '@angular/core';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { TranslateService } from "@ngx-translate/core";
import { CustomerContactService } from '../../../_Services/customer/customercontact.service'
import { CustomerContactSummaryModel, CustomerContactSummaryItemModel, customerContactToRemove, UserDetailResult } from '../../../_Models/customer/customercontactsummary.model';
import { first } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserDetail } from 'src/app/_Models/useraccount/useraccount.model';
import { RoleEnum, UserType } from '../../common/static-data-common';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { CustomerContactUserRequest } from 'src/app/_Models/customer/customer-contact-user-request';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-customerContactSummary',
  templateUrl: './customercontactsummary.component.html',
  styleUrls: ['./customercontactsummary.component.css'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '24px',
        'padding-bottom': '24px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})

export class CustomerContactSummaryComponent extends SummaryComponent<CustomerContactSummaryModel> {

  public fromSummary: boolean = false;
  public paramParent: string;
  public model: CustomerContactSummaryModel;
  public modelRemove: customerContactToRemove;
  public modelRef: NgbModalRef;
  public searchIsCustomer = false;
  public isCustomerDetails: boolean = false;
  idCurrentCustomer: number;
  customerList: Array<any> = [];
  public currentUser: UserModel;
  public parentID: any;
  public customerValues: any;
  public isEditDetail: boolean;
  public customerID?: string
  initialloading: boolean = false;
  searchloading: boolean = false;
  contactBrandList: any = [];
  contactDepartmentList: any = [];
  contactServiceList: any = [];
  isFilterOpen: boolean;
  savePopupLoading: boolean;
  showPopupLoading: boolean;
  userdetail: UserDetail;
  selectedCustomerContact: any;
  public _roleEnum = RoleEnum;
  isShowCustomerContactCredentials: boolean;
  constructor(public service: CustomerContactService, public customerService: CustomerService, public modalService: NgbModal,
    authService: AuthenticationService, router: Router, route: ActivatedRoute, public validator: Validator,
    public routeCurrent: ActivatedRoute, public routerCurrent: Router, translate: TranslateService, public utility: UtilityService,
    private userAccountService: UserAccountService, public referenceService: ReferenceService, toastr: ToastrService) {
    super(router, validator, route, translate, toastr);
    this.validator.setJSON("customer/customercontactsummary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model = new CustomerContactSummaryModel();
    this.currentUser = authService.getCurrentUser();
    this.userdetail = new UserDetail();
    this.isFilterOpen = true;
  }

  onInit() {
    this.isShowCustomerContactCredentials = this.currentUser.roles.filter(x => x.id == this._roleEnum.KAM || x.id == this._roleEnum.SalesTeam).length > 0;
    this.customerID = this.routeCurrent.snapshot.paramMap.get("id");

    this.routeCurrent.queryParams.subscribe(
      params => {
        if (params != null && params['paramParent'] != null) {
          this.paramParent = params['paramParent'];
          this.fromSummary = true;
        }

      }
    );

    this.initialloading = true;

    this.customerService.getCustomerSummary()
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
        },
        error => {
          this.setError(error);
        });

    if (this.customerID) {


      this.model.customerValue = Number(this.customerID);

      this.service.getEditCustomerContactSummary({ index: 1, pageSize: 10, customerID: this.customerID })
        .pipe()
        .subscribe(
          response => {

            if (response && response.result == 1) {
              this.isEditDetail = true;
              this.model.index = response.index;
              this.model.pageSize = response.pageSize;
              this.model.totalCount = response.totalCount;
              this.model.pageCount = response.pageCount;
              this.model.items = response.customerContacts.map((x) => {

                var tabItem: CustomerContactSummaryItemModel = {
                  id: x.id,
                  name: x.contactName,
                  lastName: x.lastName,
                  job: x.jobTitle,
                  email: x.email,
                  phone: x.phone,
                  isExpand: false,
                  brand: x.brand,
                  department: x.department,
                  service: x.service,
                  reportToName: x.reportToName,
                  list: x.list == null ? [] : x.list.map(y => {
                    return {
                      id: y.id,
                      name: y.contactName,
                      job: y.jobTitle,
                      email: y.email,
                      phone: y.phone,
                    };
                  })
                }

                if (x.typeId != 1)
                  this.searchIsCustomer = true;

                return tabItem;
              });


            }
            else if (response && response.result == 2) {
              this.model.noFound = true;
            }
            else {
              this.error = response.result;
              // TODO check error from result
            }
            this.initialloading = false;

          },
          error => {
            this.error = error;
            this.initialloading = false;
          });
    }

    this.getContactBrandList(this.model.customerValue);
  }

  getPathDetails(): string {
    return this.isEditDetail ? "cuscontactedit/edit-customer-contact" : "cuscontactedit/view-customer-contact";
  }

  returnToParent() {
    let path = "cussearch/customer-summary";
    let entity: string = this.utility.getEntityName();
    this.routerCurrent.navigate([`/${entity}/${path}`], { queryParams: { param: this.paramParent } });
  }

  getData() {
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.searchloading = true;
      this.service.getCustomerContactSummary({
        index: this.model.index, pageSize: this.model.pageSize, customerValue: this.model.customerValue, contactName: this.model.contactName,
        cuBrandList: this.model.contactBrandList, cudepartmentList: this.model.contactDepartmentList, cuServiceList: this.model.contactServiceList
      })
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1 && response.data && response.data.length > 0) {

              this.validator.isSubmitted = true;
              this.model.index = response.index;
              this.model.pageSize = response.pageSize;
              this.model.totalCount = response.totalCount;
              this.model.pageCount = response.pageCount;
              this.model.contactBrandList = response.contactBrandList;
              this.model.contactDepartmentList = response.contactDepartmentList;
              this.model.contactServiceList = response.contactServiceList;

              this.model.items = response.data.map((x) => {

                var tabItem: CustomerContactSummaryItemModel = {
                  id: x.id,
                  name: x.contactName,
                  lastName: x.lastName,
                  job: x.jobTitle,
                  email: x.email,
                  phone: x.phone,
                  isExpand: false,
                  brand: x.brand,
                  department: x.department,
                  service: x.service,
                  reportToName: x.reportToName,
                  list: x.list == null ? [] : x.list.map(y => {
                    return {
                      id: y.id,
                      name: y.contactName,
                      job: y.jobTitle,
                      email: y.email,
                      phone: y.phone,
                    };
                  })
                }

                if (x.typeId != 1)
                  this.searchIsCustomer = true;

                return tabItem;
              });


            }
            else if (response && response.result == 2) {
              this.model.noFound = true;
            }
            else {
              this.error = response.result;
              // TODO check error from result
            }

            this.searchloading = false;

          },
          error => {
            this.error = error;
            this.searchloading = false;
          });

      this.getContactBrandList(this.model.customerValue);
    }

    this.initialloading = false;
  }

  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  openConfirm(id, name, content) {

    this.modelRemove = {
      id: id,
      name: name
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  deleteCustomer(item: customerContactToRemove) {

    this.service.deleteCustomerContact(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            // refresh
            this.refresh();
          }
          else {
            this.error = response.result;

            this.loading = false;
            // TODO check error from result
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();

  }

  addSupp() {
    this.isCustomerDetails = true;
    this.idCurrentCustomer = null;
  }

  expand(id) {
    let item = this.model.items.filter(x => x.id == id)[0];

    if (item != null)
      item.isExpand = true;
  }

  collapse(id) {
    let item = this.model.items.filter(x => x.id == id)[0];

    if (item != null)
      item.isExpand = false;
  }

  formValid(): boolean {
    return this.validator.isValid('customerValue');
  }

  getDetail() {

    this.validator.isSubmitted = true;

    if (this.formValid()) {
      this.getDetails(0);
    }
  }

  changeCustomer(cusitem) {
    if (cusitem != null && cusitem.id != null) {
      this.getContactBrandList(cusitem.id);
      this.model.contactBrandList = [];
      this.model.contactDepartmentList = [];
      this.model.contactServiceList = [];
    }
  }

  getContactBrandList(id) {
    if (id) {
      this.initialloading = true;
      this.service.getContactBrandByCusId(id)
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.contactBrandList = response.contactBrandList;
              this.contactDepartmentList = response.contactDepartmentList;
              this.contactServiceList = response.contactServiceList;
            }
            this.initialloading = false;
          },
          error => {
            this.initialloading = false;
            this.contactBrandList = [];
            this.contactDepartmentList = [];
            this.contactServiceList = [];
          }
        );
    }
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  createUserCredentials(showUserPopup: any) {
    this.savePopupLoading = true;
    const customerContactUserRequest = new CustomerContactUserRequest();

    customerContactUserRequest.userName = this.selectedCustomerContact.email;
    customerContactUserRequest.fullname = this.selectedCustomerContact.name + ' (' + this.customerList.find(x => x.id == this.model.customerValue)?.name + ')' ;
    customerContactUserRequest.contactId = this.selectedCustomerContact.id;
    customerContactUserRequest.customerId = this.model.customerValue;
    customerContactUserRequest.userTypeId = UserType.Customer;

    this.createCustomerContactUserCredential(customerContactUserRequest, showUserPopup);
  }

  showUserDetailsPopup(createUserPopup, showUserPopup, item) {
    this.selectedCustomerContact = item;
    this.showPopupLoading = true;

    const userTypeId = UserType.Customer;
    const contactId = this.selectedCustomerContact.id;

    //get login user details
    this.getUserDetails(createUserPopup, showUserPopup, userTypeId, contactId);
  }

  createCustomerContactUserCredential(customerContactUserRequest: CustomerContactUserRequest, showUserPopup: any) {
    this.service.saveContactUserCredential(customerContactUserRequest)
      .subscribe(
        response => {
          this.modelRef.close();
          if (response && response.result == UserDetailResult.Success) {
            this.userdetail.userName = response.userName;
            this.userdetail.password = response.password;
            this.userdetail.isChangePassword = response.changePassword;
            this.modelRef = this.modalService.open(showUserPopup, { windowClass: "smModelWidth", centered: true });
          }
          else {
            switch (response.result) {
              case UserDetailResult.CannotSaveUserAccount:
                this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CANNOT_SAVE");
                break;
              case UserDetailResult.CurrentUserAccountNotFound:
                this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CURRENT_USER_NOTFOUND");
                break;
              case UserDetailResult.CannotMapRequestToEntites:
                this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CANNOTMAPREQUEST");
                break;
              case UserDetailResult.Failure:
                this.showError("EDIT_USER_ACCOUNT.TITLE", response.errors[0]);
                break;
            }
          }
          this.savePopupLoading = false;
        }, error => {
          console.log(error);
          this.savePopupLoading = false;
          this.showError("EDIT_USER_ACCOUNT.TITLE", 'COMMON.MSG_UNKNONW_ERROR');
          this.modelRef.close();
        });
  }

  getUserDetails(createUserPopup, showUserPopup, usertypeId: number, contactId: number) {
    this.userAccountService.getUserDetails(contactId, usertypeId)
      .subscribe(
        response => {
          if (response && response.result == UserDetailResult.Success) {
            this.userdetail.userName = response.userName;
            this.userdetail.password = response.password;
            this.userdetail.isChangePassword = response.changePassword;
            this.modelRef = this.modalService.open(showUserPopup, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
          }
          else if (response.result == UserDetailResult.CurrentUserAccountNotFound) {
            this.modelRef = this.modalService.open(createUserPopup, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
          }
          createUserPopup.showPopupLoading = false;
          this.showPopupLoading = false;
        }, error => {
          console.log(error);
          createUserPopup.showPopupLoading = false;
          this.showPopupLoading = false;
        });
  }

}

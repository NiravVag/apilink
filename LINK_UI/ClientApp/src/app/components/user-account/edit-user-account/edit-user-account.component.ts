import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { JsonHelper, Validator } from "../../common";
import { ToastrService } from "ngx-toastr";
import { UserAccountService } from '../../../_Services/UserAccount/useraccount.service'
import { UserAccountMasterModel, UserAccountModel, UserAccountSummaryModel, UserRoleEntity } from '../../../_Models/useraccount/useraccount.model'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { DetailComponent } from '../../common/detail.component';
import { PageSizeCommon, UserType } from '../../common/static-data-common';
import { ReferenceService } from '../../../_Services/reference/reference.service';
import { APIService } from "src/app/components/common/static-data-common";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-edit-user-account',
  templateUrl: './edit-user-account.component.html',
  styleUrls: ['./edit-user-account.component.scss'],
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
export class EditUserAccountComponent extends DetailComponent {
  private modelRef: NgbModalRef;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  private jsonHelper: JsonHelper;
  public modelAdd: UserAccountModel;
  public userRoleValidators: Array<any> = [];
  public roleList: Array<any> = [];
  public contactList: Array<any> = [];
  public newContactList: Array<any> = [];
  public isNewItem: boolean = true;
  public data: any;
  public model: UserAccountSummaryModel;
  saveloading: boolean;
  public type: number;
  public id: number;
  public name: string;
  public roleName: string;
  public isInternalUser: boolean;

  public isInternalOrOutsourceUser: boolean;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  apiServiceEnum = APIService;
  isFilterOpen: boolean;
  userEntityLoading: boolean = false;
  userEntityList: boolean = false;
  currentUser: any;

  userTypeEnum = UserType;
  masterModel: UserAccountMasterModel;
  constructor(
    private service: UserAccountService,
    public validator: Validator,
    translate: TranslateService,
    toastr: ToastrService,
    router: Router,
    private modalService: NgbModal,
    route: ActivatedRoute,
    public subRoute: Router,
    private referenceService: ReferenceService,
    private authService: AuthenticationService,
    public utility: UtilityService
  ) {
    super(router, route, translate, toastr);
    this.model = new UserAccountSummaryModel();
    this.modelAdd = new UserAccountModel();
    this.validator.setJSON('useraccount/edit-user-account.valid.json');
    this.validator.setModelAsync(() => this.modelAdd);
    this.validator.isSubmitted = false;
    this.jsonHelper = validator.jsonHelper;
    this._translate = translate;
    this._toastr = toastr;
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
    this.currentUser = this.authService.getCurrentUser();
    this.masterModel = new UserAccountMasterModel();
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.userRoleValidators = [];
    this.modelAdd.userRoleEntityList = [];
    if (inputparam && inputparam.has("type")) {
      this.type = Number(inputparam.get("type"))
    }
    if (id) {
      this.id = id;
    }
    if (!this.modelAdd.id) {
      this.modelAdd.apiServiceIds = [];
      this.modelAdd.userRoleEntityList = [];
      this.modelAdd.apiServiceIds.push(this.apiServiceEnum.Inspection);
    }
    if (this.type == UserType.InternalUser || this.type==UserType.OutSource) {
      this.getUserEntityList(this.id);
    }


    this.model.index = 1;
    this.Intitialize(id, this.type);
  }

  getViewPath(): string {
    throw new Error("Method not implemented.");
  }
  getEditPath(): string {
    return "useredit/edit-user-account"
  }

  roleChange(event) {

    // clear if not exist   

    if (event && event.length > 0) {
      this.userRoleValidators = this.userRoleValidators.filter(o1 => event.some(o2 => o1.userRole.roleId == o2.id));

      this.modelAdd.userRoleEntityList = this.modelAdd.userRoleEntityList.filter(o1 => event.some(o2 => o1.roleId == o2.id));

    }
    else {
      this.userRoleValidators.forEach(element => {
        this.userRoleValidators.splice(element);
      });
    }

    this.modelAdd.roles.forEach(roleId => {

      var roleData = this.roleList.find(x => x.id == roleId);
      var alreadyAdded = this.userRoleValidators.find(x => x.userRole.roleId == roleId)

      if (roleData && this.modelAdd.userRoleEntityList && alreadyAdded == undefined) {
        var userRole: UserRoleEntity = { roleId: roleData.id, roleName: roleData.roleName, roleEntity: [] };
        this.userRoleValidators.push({ userRole: userRole, validator: Validator.getValidator(userRole, "useraccount/edit-user-role.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
        this.modelAdd.userRoleEntityList.push(userRole);
      }

    });


  }



  Intitialize(id?, userTypeId?) {
    this.loading = true;
    this.isNewItem = true;

    this.isInternalOrOutsourceUser = false;

    if (userTypeId == UserType.InternalUser || userTypeId == UserType.OutSource)

      this.isInternalOrOutsourceUser = true;


    this.getData(id, userTypeId);
  }

  getData(id?, userTypeId?) {
    this.model.id = id;
    this.model.userTypeId = userTypeId;
    this.service.getUserAccountDetail(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this.data = response;
            this.roleList = response.roleList;
            this.contactList = response.contactList;
            this.model.items = response.userAccountList;
            var arr = new Array();
            if (this.contactList != null) {
              this.contactList.forEach(x => {
                var itemsActive = this.model.items.filter(x => x.status == true);
                if ((itemsActive.findIndex(y => y.contact == x.contactId)) == -1)
                  arr.push(x)
              });

              this.newContactList = arr;
            }
            if (userTypeId == 1) {
              this.modelAdd.fullname = response.name;
            }
            this.name = response.name;
            this.roleName = response.roleName;
          }
          else if (response && response.result == 2) {
            this.roleList = response.roleList;
            this.contactList = response.contactList;
            this.model.items = response.userAccountList;

            this.newContactList = this.contactList;
            this.model.noFound = true;
            if (response.name) {
              this.modelAdd.fullname = response.name;
            }
            this.name = response.name;
            this.roleName = response.roleName;
          }
          else {
            this.error = response.result;
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  refresh() {
    this.modelAdd = new UserAccountModel;
    this.userRoleValidators = [];
    this.modelAdd.userRoleEntityList = [];
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.Intitialize(this.id, this.type);
  }

  Formvalid(): boolean {
    return this.validator.isValid('userName')
      && this.validator.isValid('roles')
      && this.validator.isValid('fullname')
      && this.validator.isValid('password')
      && this.validator.isValid('apiServiceIds')
      // && this.validator.isValid('primaryEntity')
      && (!this.isInternalOrOutsourceUser ? this.validator.isValid('contact') : true)
      && this.userRoleValidators.every((x) => x.validator.isValid('roleEntity')
      );
  }

  changePageSize() {
    this.model.pageSize = this.selectedPageSize;
    this.Intitialize(this.id, this.type);
  }

  openConfirm(userItem, content) {
    this.masterModel.deleteId = userItem.id;
    this.masterModel.removedName = userItem.userName;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
  }

  openEdit(item) {
    if (item.contactList != null)
      this.newContactList = item.contactList.filter(x => x.contactId == item.contact);
    //load the user assigned entity list on edit the user account details
    if (this.type == UserType.InternalUser || this.type == UserType.OutSource)
      this.getUserEntityList(this.id);
    else if (this.type == UserType.Customer || this.type == UserType.Supplier || this.type == UserType.Factory)
      this.getUserEntityList(item.contact);

    this.modelAdd = {
      id: item.id,
      userName: item.userName,
      password: item.password,
      status: item.status,
      roles: item.roles,
      fullname: item.fullname,
      contact: item.contact,
      // primaryEntity: item.primaryEntity,
      userRoleEntityList: item.userRoleEntityList,
      apiServiceIds: null
    }
    this.isNewItem = false;

    this.modelAdd.userRoleEntityList.map(x => {

      this.userRoleValidators.push({ userRole: x, validator: Validator.getValidator(x, "useraccount/edit-user-role.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });


    });

  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.userRoleValidators) {
      item.validator.isSubmitted = true;
    }

    if (this.Formvalid()) {
      this.saveloading = true;
      this.modelAdd.userId = this.id;
      this.modelAdd.userTypeId = this.type;
      this.service.saveUserAccount(this.modelAdd)
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.showSuccess("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_SAVE_SUCCESS");
              this.validator.isSubmitted = false;
              this.refresh();
            }
            else {
              switch (response.result) {
                case 2:
                  this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CANNOT_SAVE");
                  break;
                case 3:
                  this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CURRENT_USER_NOTFOUND");
                  break;
                case 4:
                  this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CANNOTMAPREQUEST");
                  break;
                case 5:
                  this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_DUPLICATE_NAME");
                  break;
              }
            }
            this.saveloading = false;
          }, error => {
            this.showError("EDIT_USER_ACCOUNT.TITLE", 'COMMON.MSG_UNKNONW_ERROR');
            this.saveloading = false;
          });
    }
  }
  
  delete() {
    if (this.masterModel.deleteId > 0) {
      this.service.deleteUserAccountById(this.masterModel.deleteId)
        .pipe()
        .subscribe(
          response => {
            if (response && (response.result == 1)) {
              // refresh
              this.refresh();
            }
            else {
              switch (response.result) {
                case 2:
                  this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CURRENT_USER_NOTFOUND");
                  break;
                case 3:
                  this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CANNOT_DELETE");
                  break;
              }
            }
            this.loading = false;
          },
          error => {
            this.error = error;
            this.loading = false;
          });
    }
    else {
      this.showWarning("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.LBL_DELETE_ID_NOT_CORRECT");
    }
    this.modelRef.close();
  }

  public mapPageProperties(response: any) {

    this.model.index = response.index;
    this.model.pageSize = response.pageSize;
    this.model.totalCount = response.totalCount;
    this.model.pageCount = response.pageCount;

  }

  onPager(event: any) {
    this.model.index = event;
    this.refresh();
  }


  UpdateFullName(contact: any) {
    if (contact != null || typeof (contact) !== 'undefined') {
      var fullName = this.contactList.find(x => x.contactId == contact).contactName;
      this.modelAdd.fullname = fullName + ' (' + this.name + ') ';
    }
  }

  changeContactName(contact) {
    this.UpdateFullName(contact);
    this.getUserEntityList(contact);
  }

  getPathDetails(): string {
    return "userconfigcustomer/user-config";
  }
  getDetailsUser(id) {
    let entity: string = this.utility.getEntityName();

    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.subRoute.navigate([`/${entity}/${this.getPathDetails()}/${id}/${this.id}/${this.type}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  //get entity list
  getUserEntityList(contactId) {
    this.userEntityLoading = true;
    this.referenceService.getUserEntityList(this.type, contactId)
      .pipe(first())
      .subscribe(
        response => {
          this.userEntityLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.userEntityList = response.dataSourceList;
          }
        },
        error => {
          this.userEntityLoading = false;
          this.setError(error);
        });
  }

}

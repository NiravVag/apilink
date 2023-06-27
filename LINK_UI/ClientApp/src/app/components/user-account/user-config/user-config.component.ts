import { Component } from '@angular/core';
import { Validator, JsonHelper } from "../../common";
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { DetailComponent } from '../../common/detail.component';
import { UserDataAccess, MasterUserDataAccess, ResponseResult, UserNameResponse,
   UserAccessMasterData, SaveUserConfigResponse, EditUserConfigResponse } from 'src/app/_Models/useraccount/userconfig.model';
import { HrService } from 'src/app/_Services/hr/hr.service';
import { HRProfileResponse } from 'src/app/_Models/hr/edit-staff.model';
import { DropdownResult, CustomerResult } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { first } from 'rxjs/operators';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { RoleRightService } from 'src/app/_Services/RoleRight/roleright.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { UserConfigService } from 'src/app/_Services/UserAccount/userconfig.service';
import { HRProfileUserConfig, HRProfileEnum, APIService } from '../../common/static-data-common';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { DataSource } from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-user-config',
  templateUrl: './user-config.component.html',
  styleUrls: ['./user-config.component.scss']
})
export class UserConfigComponent extends DetailComponent {

  private _translate: TranslateService;
  private _toastr: ToastrService;

  userDataAccess: UserDataAccess;
  masterUserDataAccess: MasterUserDataAccess;
  userAccessValidators: Array<any>;

  hrProfileUserConfig = HRProfileUserConfig;
  hrProfileEnum = HRProfileEnum;

  constructor(private jsonHelper: JsonHelper,
    public validator: Validator,
    translate: TranslateService,
    toastr: ToastrService,
    router: Router,
    route: ActivatedRoute, public hrService: HrService, public userAccountService: UserAccountService,
    public customerCheckPointService: CustomerCheckPointService,
    public productManagementService: ProductManagementService,
    public roleRightService: RoleRightService,
    public officeService: OfficeService,
    public bookingService: BookingService,
    public userConfigService: UserConfigService,
    public customerService: CustomerService,
    public utility: UtilityService,
    public subRoute: Router) {
    super(router, route, translate, toastr);

    this.userDataAccess = new UserDataAccess();
    this.masterUserDataAccess = new MasterUserDataAccess();

    this.validator.setJSON('userconfig/userconfig-save.valid.json');

    this.validator.setModelAsync(() => this.userDataAccess);
    this.validator.isSubmitted = false;
  }

  onInit(id: number, inputparam?: ParamMap): void {

    //get below details for return to previous page
    if (inputparam) {
      if (inputparam.has("type"))
        this.masterUserDataAccess.redirectType = Number(inputparam.get("type"))
      if (inputparam.has("userId"))
        this.masterUserDataAccess.redirectId = Number(inputparam.get("userId"))
    }
    this.Intitialize(id);
  }
  Intitialize(id: number) {

    this.userAccessValidators = [];

    this.getCustomerListByUserType();
    this.getServiceList();
    this.getProductCategoryList();
    this.getOfficeList();
    this.getProfileList();

    if (id && id > 0) { //userid

      this.userDataAccess.userId = Number(id);

      this.editData(this.userDataAccess.userId);
      this.getRoleList(this.userDataAccess.userId);
      this.getUserName(this.userDataAccess.userId);
    }
  }

  //get data from DB
  async editData(userId: number) {
    let response: EditUserConfigResponse;
    try {
      response = await this.userConfigService.edit(userId);
      this.editDataResponse(response);

    }
    catch (e) {
      console.error(e);
      this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
    }
  }

  //edit  response
  editDataResponse(response: EditUserConfigResponse) {
    if (response) {
      switch (response.result) {
        case ResponseResult.Success:
          this.userDataAccess = response.data;

          this.userDataAccess.userAccessList.forEach(element => {

            this.disableProductCategory(element);
            this.userAccessValidators.push({
              userAccess: element, validator:
                Validator.getValidator(element, "userconfig/userconfig-table-save.valid.json",
                  this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)

            });
          });
          break;
        case ResponseResult.Faliure:
          this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
      }
      if (response.result != ResponseResult.Success) {
        this.userDataAccess.emailAccess = true;
        this.frameUserAccessData();
      }
    }
  }

  //get user name
  async getUserName(id: number) {
    let response: UserNameResponse;

    try {
      response = await this.userAccountService.getUserName(id);
      this.getUserNameSuccess(response);
    }
    catch (e) {
      console.error(e);
      this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
    }
  }

  //user name get success response
  getUserNameSuccess(response: UserNameResponse) {
    if (response) {
      switch (response.result) {
        case ResponseResult.Success:
          this.masterUserDataAccess.userName = response.name;
          break;
        case ResponseResult.Faliure:
          this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
        case ResponseResult.NotFound:
          this.showError('USER_CONFIG.TITLE', 'USER_CONFIG.MSG_USER_NAME_NOT_FOUND');
          break;

      }
    }
  }

  //get user profile list
  async getProfileList() {
    this.masterUserDataAccess.profileLoading = true;
    let response: HRProfileResponse;

    try {
      response = await this.hrService.getProfileList();
      this.getProfileListSuccess(response);
    }
    catch (e) {
      console.error(e);
      this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      this.masterUserDataAccess.profileLoading = false;
    }
  }

  //user profile response
  getProfileListSuccess(response: HRProfileResponse) {
    if (response) {
      switch (response.result) {
        case DropdownResult.Success:

          this.masterUserDataAccess.profileList = response.profileList.filter
            (x => this.hrProfileUserConfig.some(y => y.id == x.id));

          break;
        case DropdownResult.NodataFound:
          this.showError('USER_CONFIG.TITLE', 'USER_CONFIG.MSG_PROFILE_LIST_NOT_FOUND');
      }
    }
    this.masterUserDataAccess.profileLoading = false;
  }

  //get customer list
  getCustomerListByUserType() {
    this.masterUserDataAccess.masterCustomerLoading = true;
    this.bookingService.GetCustomerByUserType().subscribe(
      response => {
        this.getCustomerListResponse(response);
      },
      error => {
        this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterUserDataAccess.masterCustomerLoading = false;
      }
    );
  }

  //customer list success response
  getCustomerListResponse(response) {
    if (response) {
      if (response.result == CustomerResult.Success) {
        this.masterUserDataAccess.masterCustomerList = response.customerList;
        this.masterUserDataAccess.masterOriginalCustomerList = response.customerList;
      }
      else if (response.result == CustomerResult.CannotGetCustomerList) {
        this.showError('USER_CONFIG.TITLE', 'USER_CONFIG.MSG_CUSTOMER_LIST_NOT_FOUND');
      }
    }
    this.masterUserDataAccess.masterCustomerLoading = false;
  }

  //frame the table
  frameUserAccessData() {
    var userAccessTable: UserAccessMasterData = new UserAccessMasterData();

    this.userAccessValidators.push({
      userAccess: userAccessTable, validator:
        Validator.getValidator(userAccessTable, "userconfig/userconfig-table-save.valid.json",
          this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
    });
  }

  //get service list
  getServiceList() {
    this.masterUserDataAccess.masterServiceLoading = true;
    this.customerCheckPointService.getService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterUserDataAccess.masterServiceList = response.serviceList;
          this.masterUserDataAccess.masterServiceLoading = false;
        },
        error => {
          this.setError(error);
          this.masterUserDataAccess.masterServiceLoading = false;
        });
  }

  //get product category list
  getProductCategoryList() {
    this.masterUserDataAccess.masterProductCategoryLoading = true;
    this.productManagementService.getProductCategorySummary()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterUserDataAccess.masterProductCategoryList = response.productCategoryList;
          this.masterUserDataAccess.masterProductCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterUserDataAccess.masterProductCategoryLoading = false;
        });
  }

  //get role list
  getRoleList(userId: number) {
    this.masterUserDataAccess.masterRoleLoading = true;
    this.roleRightService.getRoleList(userId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterUserDataAccess.masterRoleList = response.dataSourceList;
          this.masterUserDataAccess.masterRoleLoading = false;
        },
        error => {
          this.setError(error);
          this.masterUserDataAccess.masterRoleLoading = false;
        });
  }

  //get office list
  getOfficeList() {
    this.masterUserDataAccess.masterOfficeLoading = true;

    this.officeService.getOfficeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterUserDataAccess.masterOfficeList = response.dataSourceList;
          this.masterUserDataAccess.masterOfficeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterUserDataAccess.masterOfficeLoading = false;
        });
  }

  //add the table row
  addCustomerAccess() {
    var isCustomerNull;

    //lopp the user access 
    this.userAccessValidators.forEach(element => {
      if (element.userAccess.customerId != null) {
        isCustomerNull = false;
      }
      else {
        isCustomerNull = true;
      }
    });

    // If any row have null customer Id
    if (isCustomerNull)
      this.showError('USER_CONFIG.TITLE', 'USER_CONFIG.MSG_EMPTY_CUSTOMER');
    else {
      this.frameUserAccessData();
    }
  }

  //remove the row
  removeCustomerAccess(index) {

    if (this.userAccessValidators && this.userAccessValidators.length > 1) {

      if (this.userDataAccess.userAccessList)
        this.userDataAccess.userAccessList.splice(index, 1);

      this.userAccessValidators.splice(index, 1);
    }
  }
  //validation before submit the form
  isFormValid() {
    let isOk = this.validator.isValid('profileId');
    if (isOk) {
      this.userAccessValidators.every((x) =>
        isOk = x.validator.isValid('roleIdAccessList'));
    }

    if (isOk) {
      if (this.userDataAccess.profileId == HRProfileEnum.CS &&
        this.userAccessValidators && this.userAccessValidators.length > 0 &&
        (this.userAccessValidators.findIndex(x => x.userAccess.customerId > 0) == -1)) {
        this.showWarning('USER_CONFIG.TITLE', 'USER_CONFIG.MSG_ATLEAST_ONE_CUSTOMER_SHOULD_SELECT');
        isOk = false;
      }
    }
    return isOk;
  }

  //save
  async save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;

    this.userAccessValidators.forEach(item => {
      item.validator.isSubmitted = true;
    });

    if (this.isFormValid()) {
      this.masterUserDataAccess.saveLoading = true;

      let response: SaveUserConfigResponse;
      this.userDataAccess.userAccessList = [];

      this.userAccessValidators.forEach(element => {
        this.userDataAccess.userAccessList.push(element.userAccess);
      });
      try {
        response = await this.userConfigService.save(this.userDataAccess);
      }
      catch (e) {
        console.error(e);
        this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterUserDataAccess.saveLoading = false;
      }
      if (response) {
        switch (response.result) {
          case ResponseResult.Success:
            this.onInit(response.id);
            this.showSuccess('USER_CONFIG.TITLE', 'USER_CONFIG.MSG_SAVE_SUCCESS');
            break;
          case ResponseResult.Faliure:
            this.showError('USER_CONFIG.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
            break;
        }
      }
      this.masterUserDataAccess.saveLoading = false;

    }

  }

  getPathDetails(): string {
    return "useredit/edit-user-account";
  }

  //redirect to edit user account page
  redirectBack() {
    let entity: string = this.utility.getEntityName();
    this.subRoute.navigate([`/${entity}/${this.getPathDetails()}/${this.masterUserDataAccess.redirectId}/${this.masterUserDataAccess.redirectType}`], { queryParams: { paramParent: this.paramParent } });
  }

  //if any loading true, disable the save button
  saveDisable(): boolean {

    return (this.masterUserDataAccess.masterCustomerLoading ||
      this.masterUserDataAccess.masterOfficeLoading ||
      this.masterUserDataAccess.masterServiceLoading ||
      this.masterUserDataAccess.masterProductCategoryLoading ||
      this.masterUserDataAccess.masterRoleLoading ||
      this.masterUserDataAccess.profileLoading ||
      this.masterUserDataAccess.saveLoading);
  }

  //disable the product category if service is auidt
  disableProductCategory(userAccess): void {

    //If service - audit - disable product category dropdown
    if (userAccess.serviceIdAccessList && userAccess.serviceIdAccessList.length == 1
      && userAccess.serviceIdAccessList.find(x => APIService.Audit == x)) {

      //clear the selected product category
      userAccess.productCategoryIdAccessList = [];
      userAccess.disableProductCategory = true;
    }
    else {
      userAccess.disableProductCategory = false;

    }
  }

  getEditPath(): string {
    return "";
  }
  getViewPath(): string {
    return "";
  }

  //customer dropdown change event
  openCustomer() {
    //the below code is to remove the customer if it is available with the above record.
    this.masterUserDataAccess.masterCustomerList = Object.assign([],
      this.masterUserDataAccess.masterOriginalCustomerList);

    this.userAccessValidators.forEach(element => {

      if (element.userAccess.customerId != null) {
        this.masterUserDataAccess.masterCustomerList = this.masterUserDataAccess.masterCustomerList
          .filter(x => x.id != element.userAccess.customerId);
      }

    });
  }

  // customer change event
  changeCustomer(customerData: DataSource, userAccess: UserAccessMasterData) {

    userAccess.cusDepartmentIdAccessList = [];
    userAccess.cusBrandIdAccessList = [];
    userAccess.customerBrandList = [];
    userAccess.customerDepartmentList = [];

    this.getCustomerDepartments(customerData.id, userAccess);
    this.getCustomerBrands(customerData.id, userAccess);
  }

  //load the department based on customer
  getCustomerDepartments(customerId: number, userAccess: UserAccessMasterData) {
    if (customerId && customerId > 0) {
      userAccess.customerDepartmentLoading = true;
      this.customerService.getCustomerDepartments(customerId)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success)
              userAccess.customerDepartmentList = response.dataSourceList;
            userAccess.customerDepartmentLoading = false;
          },
          error => {
            this.setError(error);
            userAccess.customerDepartmentLoading = false;
          });
    }
  }

  getCustomerBrands(customerId: number, userAccess: UserAccessMasterData) {
    if (customerId && customerId > 0) {
      userAccess.customerBrandLoading = true;
      this.customerService.getCustomerBrands(customerId)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success)
              userAccess.customerBrandList = response.dataSourceList;
            userAccess.customerBrandLoading = false;
          },
          error => {
            this.setError(error);
            userAccess.customerBrandLoading = false;
          });
    }
  }
  /**
   *  getCustomerBuyers(customerId: number, userAccess: UserAccessMasterData) {
  if (customerId && customerId > 0) {
    userAccess.customerBuyerLoading = true;
    this.customerService.getCustomerBuyers(customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            userAccess.customerBuyerList = response.dataSourceList;
          userAccess.customerBuyerLoading = false;
        },
        error => {
          this.setError(error);
          userAccess.customerBuyerLoading = false;
        });
  }
}
load the department based on customer
getCustomerDepartments(customerId: number, userAccess: UserAccessMasterData) {
  if (customerId && customerId > 0) {

    //get only selected customer id dept list
    userAccess.customerDepartmentList = this.masterUserDataAccess.customerDepartmentsList && this.masterUserDataAccess.customerDepartmentsList.length > 0 ?
      this.masterUserDataAccess.customerDepartmentsList.filter(x => x.parentId == customerId) : [];

  }
}

//load the brand based on customer
getCustomerBrands(customerId: number, userAccess: UserAccessMasterData) {
  if (customerId && customerId > 0) {

    //get only selected customer id brand list
    userAccess.customerBrandList = this.masterUserDataAccess.customerBrandsList && this.masterUserDataAccess.customerBrandsList.length > 0
      ? this.masterUserDataAccess.customerBrandsList.filter(x => x.parentId == customerId) : [];
  }
}
   */
}

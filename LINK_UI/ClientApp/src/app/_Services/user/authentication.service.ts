import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { UserModel } from '../../_Models/user/user.model';
import { RoleModel } from '../../_Models/user/role.model';
import { MenuItemModel } from '../../_Models/user/menuItem.model';
import { ChangePassword } from 'src/app/_Models/user/changepassword.model';
import { APIService, AreaManagement, Customer, DataManagement, EntityAccess, ExpenseClaim, Holiday, HRProfileEnum, HumanResource, Leave, LinkAppServiceList, RightTypeEnum, RightTypeImageList, RoleEnum, Service, ServiceImageList, theme, UserManagement, UserType, Vendor } from 'src/app/components/common/static-data-common';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from '../common/utility.service';
import { UserRightTypeAccess, UserServiceAccess } from 'src/app/_Models/common/common.model';
import { RightType, RightTypeResponse } from 'src/app/_Models/user/right-type-response.model';

@Injectable({ providedIn: 'root' })

export class AuthenticationService {
  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute, @Inject('BASE_URL') baseUrl: string, private route: Router, public utility: UtilityService) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);

  }

  public url: string;
  public returnUrl: string;
  public appService: APIService;
  public EntityCount: number = 0;
  apiServiceEnum = APIService;
  selectedEntity: number;

  login(username: string, password: string) {
    const body = {
      UserName: username,
      Password: password,
      isEncrypt: true
    };

    return this.http.post<any>(`${this.url}/api/User/SignIn`, body)
      .pipe(map(response => {
        // login successful if there's a jwt token in the response
        if (response && response.result == 1 && response.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          var user: UserModel = {
            id: response.user.id,
            fullName: response.user.fullName,
            userName: response.user.userName,
            entityName: response.user.entityName,
            countryId: response.user.countryId,
            usertype: response.user.userType,
            customerid: response.user.customerId,
            supplierid: response.user.supplierId,
            factoryid: response.user.factoryId,
            staffId: response.user.staffId,
            profiles: response.user.userProfileList,
            showHeader: true,
            serviceAccess: response.user.serviceAccess,
            fbUserId: response.user.fbUserId,
            roles: response.user.roles.map((x) => {
              var role: RoleModel = {
                id: x.id,
                roleName: x.roleName
              }
              return role;
            }),
            rights: [],
            rightsDB: response.user.rights
          }

          localStorage.setItem('currentUser', JSON.stringify(user));
          localStorage.setItem('_APItoken', JSON.stringify(response.token));
          localStorage.setItem('_entity', response.user.entityName);
          localStorage.setItem('_entityId', response.user.entityId);
          return response;
        }
      }));
  }



  getCurrentUser(): UserModel {
    var user = JSON.parse(localStorage.getItem('currentUser'));
    if (user && user.rightsDB && localStorage.getItem('currentUser')) {
      user.rights = this.getMenuList(user.rightsDB);
      localStorage.setItem('currentUser', JSON.stringify(user));

    }
    return <UserModel>user;
  }

  changePassword(model: ChangePassword) {
    return this.http.post<any>(`${this.url}/api/User/ChangePassword`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  resetPassword(model: ChangePassword) {
    return this.http.post<any>(`${this.url}/api/User/ResetPassword`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getUserEntityAccess(userId: Number) {
    return this.http.get<any>(`${this.url}/api/User/EntityAccess/${userId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getUserRoleEntityAccess(userId: Number, entityId: string) {

    var entityAccessReq = { id: userId, entityId: entityId };

    return this.http.post<any>(`${this.url}/api/User/EntityRoleAccess`, entityAccessReq)
      .pipe(map(response => {
        return response;
      }));
  }


  redirectToLanding() {
    var EntityName = this.utility.getEntityName();
    var user = this.getCurrentUser();
    if (user && (user.usertype == UserType.InternalUser || user.usertype == UserType.OutSource)) {

      var accessServiceCount = this.getAccessCount(user);
      this.getUserEntityAccess(user.id)
        .pipe()
        .subscribe(
          data => {
            this.EntityCount = data ? data.length : 0;
            if (accessServiceCount == 1 && this.EntityCount == 1) {
              // set one service access, so menu will be filter by service
              if (user.serviceAccess[0]) {
                localStorage.setItem('service', JSON.stringify(user.serviceAccess[0]));
              }

              this.redirectToDashboard();
            }
            else {
              this.router.navigate(["/landing"]);
            }
          },
          error => {

          });

    }
    /* else if (user.usertype == UserType.OutSource) {
      this.RedirectWithServiceAccess(Service.Inspection);
    } */
    else if (user.usertype == UserType.Customer || user.usertype == UserType.Supplier || user.usertype == UserType.Factory) {
      this.router.navigate(["/landing"]);
    }
    else {
      var redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/inspsummary/booking-summary";
      this.router.navigate([redirectPath]);
    }
  }

  getAccessCount(user): Number {
    return user.serviceAccess.length;
  }

  redirectToDashboard() {
    var EntityName = this.utility.getEntityName();
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
    var user = this.getCurrentUser();
    var redirectPath = '';
    if (user && (user.usertype == UserType.InternalUser || user.usertype == UserType.OutSource)) {


      var isInspector = user.profiles.find(x => x == HRProfileEnum.Inspector);

      if (isInspector) {
        redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/qcdashboard";
        this.router.navigate([redirectPath]);
      }
      else {
        var management = user.roles.find(x => x.id == RoleEnum.TechnicalTeam || x.id == RoleEnum.OperationTeamManagement || x.id == RoleEnum.HRManagement
          || x.id == RoleEnum.KAM || x.id == RoleEnum.CEO || x.id == RoleEnum.SalesTeam);


        if (user.serviceAccess.includes(APIService.Inspection))//inspection
        {
          if (management) {
            redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/managementdashboard/dashboard";
            this.router.navigate([redirectPath]);
          }
          else {
            this.router.navigate([this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/cs/dashboard"]);
          }
        }
        //if only audit service and one entity then redirect to respective dashbaord after login.
        else if (user.serviceAccess.includes(APIService.Audit) && this.EntityCount == 1 && this.getAccessCount(user) == 1)//audit + only audit access. 
        {
          var redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/auditdashboard/dashboard";
          this.router.navigate([redirectPath]);
        }
        //if only Tcf service and one entity then redirect to respective dashbaord after login.
        else if (user.serviceAccess.includes(APIService.Tcf) && this.EntityCount == 1 && this.getAccessCount(user) == 1)//tcf + only tcf access
        {
          var redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/tcfdashboard/tcf-dashboard";
          this.router.navigate([redirectPath]);
        }

      }

    }
    else if (user.usertype == UserType.Customer) {
      redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/cusdashboard";
      this.router.navigate([redirectPath]);
    }
    else if (user.usertype == UserType.Supplier || user.usertype == UserType.Factory) {
      var redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + EntityName + "/supdashboard/dashboard";
      this.router.navigate([redirectPath]);
    }
  }

  logout() {
    // remove user from local storage to log user out
    var user = JSON.parse(localStorage.getItem('currentUser'));
    if (user) {
      this.logoutLog(user.id)
        .subscribe(res => {

        });
    }
    localStorage.removeItem('currentUser');
    localStorage.removeItem('_APItoken');
    localStorage.removeItem('serviceType');
    localStorage.removeItem('_activemenuid');
    localStorage.removeItem('_entity');
    localStorage.removeItem('mapbox.eventData.uuid:');
    localStorage.removeItem('recentUploads');

  }

  public logoutLog(id: number) {
    return this.http.get<any>(`${this.url}/api/User/SignOut/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getMenuItem(item, data, rightType) {

    var children = data.filter(x => x.parentId == item.id && x.showMenu);
    var childrenItems: Array<MenuItemModel> = [];

    if (children) {
      for (let item of children) {
        var menuItem = this.getMenuItem(item, data, rightType);
        if (rightType && rightType == RightTypeEnum.Inspection) {
          if (menuItem.rightType == RightTypeEnum.Inspection || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.Audit) {
          if (menuItem.rightType == RightTypeEnum.Audit || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.TCF) {
          if (menuItem.rightType == RightTypeEnum.TCF || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.LabTesting) {
          if (menuItem.rightType == RightTypeEnum.LabTesting || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.Customer) {
          if (menuItem.rightType == RightTypeEnum.Customer || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.Vendor) {
          if (menuItem.rightType == RightTypeEnum.Vendor || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.HumanResource) {
          if (menuItem.rightType == RightTypeEnum.HumanResource || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.Document) {
          if (menuItem.rightType == RightTypeEnum.Document || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else if (rightType && rightType == RightTypeEnum.Admin) {
          if (menuItem.rightType == RightTypeEnum.Admin || menuItem.rightType == null)
            childrenItems.push(menuItem);
        }
        else
          childrenItems.push(menuItem);
      }

    }

    var right: MenuItemModel = {
      id: item.id,
      active: item.active,
      glyphicons: item.glyphicons,
      isExpand: false,
      isHeading: item.isHeading,
      menuName: item.menuName,
      parentId: item.parentId,
      path: item.path && item.path != "" ? item.path : "#",
      ranking: item.ranking,
      titleName: item.titleName,
      children: childrenItems,
      showMenu: item.showMenu,
      rightType: item.rightType
    };

    return right;
  }

  getMenuList(data) {
    let items: Array<MenuItemModel> = [];
    if (data) {
      let headers = data.filter(x => x.parentId == null && x.isHeading);
      let rightType = Number(localStorage.getItem('rightType'));
      switch (rightType) {
        case RightTypeEnum.Customer:
          headers = headers.filter(x => x.titleName == Customer && x.rightType == rightType);
          break;
        case RightTypeEnum.Vendor:
          headers = headers.filter(x => x.titleName == Vendor && x.rightType == rightType);
          break;
        case RightTypeEnum.HumanResource:
          headers = headers.filter(x => (x.titleName == HumanResource || x.titleName == Leave || x.titleName == Holiday || x.titleName == ExpenseClaim) && x.rightType == rightType);
          break;
        case RightTypeEnum.Document:
          headers = headers.filter(x => x.titleName == DataManagement && x.rightType == rightType);
          break;
        case RightTypeEnum.Admin:
          headers = headers.filter(x => (x.titleName == AreaManagement || x.titleName == UserManagement) && x.rightType == rightType);
          break;
        default:
          headers = headers.filter(x => x.rightType == rightType);
          break;
      }

      if (headers) {
        for (let item of headers) {
          let menuItem = this.getMenuItem(item, data, rightType);
          items.push(menuItem);
        }
      }
    }


    return items
  }

  checkUserRightTypeAccess(rightTypeAccess): boolean {
    const user = JSON.parse(localStorage.getItem('currentUser'));
    return user.rightsDB.filter(x => x.rightType == rightTypeAccess).length > 0
  }

  checkUserHasAccess(serivceAccess): boolean {
    var user = JSON.parse(localStorage.getItem('currentUser'));
    return user.serviceAccess.filter(x => x == serivceAccess).length > 0
  }

  //Redirect to home page with defaut page configured for the service
  RedirectWithServiceAccess(service) {
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
    switch (service) {
      case this.apiServiceEnum.Inspection: {
        if (this.checkUserHasAccess(APIService.Inspection) && this.checkUserRightTypeAccess(RightTypeEnum.Inspection)) {
          this.redirectToDashboard();
        }
        else {
          this.RedirectToLearMore();
        }
        break;
      }
      case this.apiServiceEnum.Audit: {
        if (this.checkUserHasAccess(APIService.Audit) && this.checkUserRightTypeAccess(RightTypeEnum.Audit)) {
          var redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/auditdashboard/dashboard";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore();
        }

        break;
      }
      case this.apiServiceEnum.Tcf: {
        if (this.checkUserHasAccess(APIService.Tcf) && this.checkUserRightTypeAccess(RightTypeEnum.TCF)) {
          var redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/tcfdashboard/tcf-dashboard";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore()
        }

        break;
      }

      default: {
        this.RedirectToLearMore()
        break;
      }
    }

  }

  //Redirect to home page with defaut page configured for the rightType
  RedirectWithRightTypeAccess(rightType: any) {
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
    switch (rightType) {
      case RightTypeEnum.Inspection: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.Inspection)) {
          this.redirectToDashboard();
        }
        else {
          this.RedirectToLearMore();
        }
        break;
      }
      case RightTypeEnum.Audit: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.Audit)) {
          const redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/auditdashboard/dashboard";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore();
        }

        break;
      }
      case RightTypeEnum.TCF: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.TCF)) {
          const redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/tcfdashboard/tcf-dashboard";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore()
        }

        break;
      }
      case RightTypeEnum.Customer: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.Customer)) {
          const redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/cussearch/customer-summary";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore()
        }

        break;
      }
      case RightTypeEnum.Vendor: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.Vendor)) {
          const redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/suppliersearch/supplier-summary";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore()
        }

        break;
      }
      case RightTypeEnum.HumanResource: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.HumanResource)) {
          const redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/staffsearch/staff-summary";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore()
        }

        break;
      }
      case RightTypeEnum.Document: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.Document)) {
          const redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/datamanagement/dmsummary";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore()
        }

        break;
      }
      case RightTypeEnum.Admin: {
        if (this.checkUserRightTypeAccess(RightTypeEnum.Admin)) {
          const redirectPath = this.returnUrl && this.returnUrl != "/" ? this.returnUrl : "/" + this.utility.getEntityName() + "/usersearch/user-account-summary";
          this.router.navigate([redirectPath]);
        }
        else {
          this.RedirectToLearMore()
        }

        break;
      }

      default: {
        this.RedirectToLearMore()
        break;
      }
    }
  }

  // get the service list accessed to the user
  getUserServiceAccessList() {

    var serviceAccessList = new Array<UserServiceAccess>();
    var serviceAccess = new UserServiceAccess();
    var user = JSON.parse(localStorage.getItem('currentUser'));
    this.selectedEntity = Number(this.utility.getEntityId());
    //loop through the service list
    LinkAppServiceList.forEach(serviceData => {
      serviceAccess = new UserServiceAccess();
      serviceAccess.id = serviceData.id;
      serviceAccess.name = serviceData.name;
      //check the user has access with the service      
      if (user && user.serviceAccess.find(x => x == serviceData.id))
        serviceAccess.userHasAccess = true;
      //get the image url by selected service and selected entity  
      var image = ServiceImageList.find(x => x.serviceId == serviceData.id && x.entityId == this.selectedEntity);

      var imagePath = this.utility.getImagePathbyEntityAndTheme(theme.light);

      if (imagePath && image && image.serviceEnabledFileName)
        serviceAccess.serviceEnabledFileName = imagePath + image.serviceEnabledFileName;
      if (image && image.serviceDisabledFileName)
        serviceAccess.serviceDisabledFileName = imagePath + image.serviceDisabledFileName;
      serviceAccessList.push(serviceAccess);
    });

    return serviceAccessList;

  }

  getUserRightTypeAccessList(rightTypeList: RightType[]) {
    const rightTypeAccessList = new Array<UserRightTypeAccess>();
    let rightTypeAccess = new UserRightTypeAccess();
    const user = JSON.parse(localStorage.getItem('currentUser'));
    this.selectedEntity = Number(this.utility.getEntityId());
    rightTypeList.forEach(rightTypeData => {
      rightTypeAccess = new UserRightTypeAccess();
      rightTypeAccess.id = rightTypeData.id;
      rightTypeAccess.name = rightTypeData.name;
      if (rightTypeData.service) {
        //check the user has access with the service      
        if (user && user.serviceAccess.find(x => x == rightTypeData.service) && user.rightsDB.find(x => x.rightType == rightTypeData.id))
          rightTypeAccess.userHasAccess = true;
        else
          rightTypeAccess.userHasAccess = false;
      }
      //check the user has access with the right type 
      else if (user && user.rightsDB.find(x => x.rightType == rightTypeData.id))
        rightTypeAccess.userHasAccess = true;
      //get the image url by selected right type and selected entity  
      const image = RightTypeImageList.find(x => x.rightTypeId == rightTypeData.id && x.entityId == this.selectedEntity);
      const imagePath = this.utility.getImagePathbyEntityAndTheme(theme.light);

      if (imagePath && image && image.rightTypeEnabledFileName && rightTypeAccess.userHasAccess)
        rightTypeAccess.rightTypeFileName = imagePath + image.rightTypeEnabledFileName;
      if (image && image.rightTypeDisabledFileName && !rightTypeAccess.userHasAccess)
        rightTypeAccess.rightTypeFileName = imagePath + image.rightTypeDisabledFileName;
      rightTypeAccessList.push(rightTypeAccess);
    });
    return rightTypeAccessList.sort((a, b) => (a.userHasAccess > b.userHasAccess ? -1 : 1));
  }

  RedirectToLearMore() {
    this.selectedEntity = Number(this.utility.getEntityId());
    if (this.selectedEntity == EntityAccess.API) {
      window.open('https://www.api-hk.com/');
    }
    else if (this.selectedEntity == EntityAccess.SGT) {
      window.open('https://www.sgtgroup.net/en/');
    }
    else if (this.selectedEntity == EntityAccess.AQF) {
      window.open('https://www.asiaqualityfocus.com/');
    }
  }

  async getRightTypelst(): Promise<RightTypeResponse> {
    return await this.http.get<any>(`${this.url}/api/User/righttypelst`).toPromise();
  }
}

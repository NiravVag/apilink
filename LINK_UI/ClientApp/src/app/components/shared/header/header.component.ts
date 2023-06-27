import { Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Router, ActivatedRoute } from '@angular/router';

import { UserModel } from '../../../_Models/user/user.model';
import { MenuItemModel } from '../../../_Models/user/menuItem.model';
import { HrService } from '../../../_Services/hr/hr.service';
import { UserType, HeaderCount, BookingSearchRedirectPage, HRProfileEnum, Url, RoleEnum, WormsServicePortal, ITSupporEmail, APIService, EntityAccess, LinkAppServiceList, StandardOnlyDateFormat, RightTypeEnum, OutSourceUserType } from '../../common/static-data-common';
import { DataServiceService } from "../../../_Services/common/data-service-service.service";
import { HeaderService } from '../../../_Services/header/header.service';
import { TaskResult, TaskMessage, TaskType, TaskSearchRequest, TaskDateType, TaskFilterType } from '../../../_Models/header/task.model';
import { WorkerMiddlewareService } from '../../../_Services/common/worker.service';
import { ToastrService } from 'ngx-toastr';
import { NotificationDateType, NotificationFilterType, NotificationMessage, NotificationResult, NotificationSearchRequest, NotificationType } from '../../../_Models/header/notification.model';
import { CustomerKAMDetail } from 'src/app/_Models/user/customerkamdetails.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserRightTypeAccess } from 'src/app/_Models/common/common.model';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { HeaderMasterModel } from 'src/app/_Models/header/header-master.model';
import { NgSelectComponent } from '@ng-select/ng-select';
import { UserProfileService } from 'src/app/_Services/UserAccount/userprofile.service';
import { ResponseResult } from 'src/app/_Models/useraccount/userprofile.model';
import { RightType, RightTypeResult } from 'src/app/_Models/user/right-type-response.model';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'width': '*',
        'opacity': 1,
        'margin-right': '20px',
        'margin-left': '8px'
      })),
      state('close', style({
        'width': '0px',
        'opacity': '0',
        'margin-right': '0px',
        'margin-left': '0px'
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class HeaderComponent {
  public User: UserModel;
  public masterModel: HeaderMasterModel;
  public header: Array<any>;
  returnUrl: string;
  userEntity: string;
  standardOnlyDateFormat: string = StandardOnlyDateFormat;
  drawerIsOpen: boolean;
  photourl: string
  _userTypeId;
  usergender: string;
  _internalusertype = UserType.InternalUser;
  _customerusertype = UserType.Customer;
  _headercount = HeaderCount.count;
  @ViewChild('main-wrapper') mainWrapper: ElementRef;
  @ViewChild('drawer-menu-container') drawerContainer: ElementRef;
  @ViewChild('ngNotificationSelectComponent') ngNotificationSelectComponent: NgSelectComponent;
  @ViewChild('ngTaskSelectComponent') ngTaskSelectComponent: NgSelectComponent;
  _userType = UserType;
  customerKAMDetail: CustomerKAMDetail;
  itSupporEmail: string;
  expandNavigation: boolean;
  selectedEntity: number;
  selectedRightType: number;
  entityAccessEnum = EntityAccess;
  userRightTypeAccessList: Array<UserRightTypeAccess>;
  rightTypeList: Array<RightType>;
  @ViewChild('headerSecondColumnElement') headerSecondColumnElement: ElementRef;
  @ViewChild('headerSecondColumnLastChild') headerSecondColumnLastChild: ElementRef;

  isMobile: boolean;
  userName: string;
  isCustomer: boolean;
  @HostListener('document:click', ['$event']) public onClick(event) {
    let dropdownClick = false;
    let notificationTrigger = document.querySelector('#notificationTrigger');
    let notificationContainer = document.querySelector('#notificationContainer');
    let notificationDropdown = document.querySelector('#notificationDropdown');
    let taskDropdown = document.querySelector('#taskDropdown');

    ['ng-option', 'ng-option-label'].forEach((className) => {
      if(event.target.classList.contains(className)) { dropdownClick = true; }
    });

    if (!notificationContainer.contains(event.target) && !notificationTrigger.contains(event.target) && !notificationDropdown.contains(event.target) && !dropdownClick) {
      this.masterModel.showNotificationPanel = false;
    }

    let taskTrigger = document.querySelector('#taskTrigger');
    let taskContainer = document.querySelector('#taskContainer');

    if (!taskTrigger.contains(event.target) && !taskContainer.contains(event.target) && !taskDropdown.contains(event.target) && !dropdownClick) {
      this.masterModel.showTaskPanel = false;
    }

    let helpTrigger = document.querySelector('#helpTrigger');
    let helpContainer = document.querySelector('#helpContainer');

    if (!helpTrigger.contains(event.target) && !helpContainer.contains(event.target)) {
      this.masterModel.showHelpPanel = false;
    }

    let userTrigger = document.querySelector('#userTrigger');
    let userContainer = document.querySelector('#userContainer');

    if (!userTrigger.contains(event.target) && !userContainer.contains(event.target)) {
      this.masterModel.showUserPanel = false;
    }
  }

  constructor(private authenticationService: AuthenticationService, private router: Router, private customerService: CustomerService,
    private service: HrService, private ds: DataServiceService, public headerService: HeaderService, public utility: UtilityService,
    private workerService: WorkerMiddlewareService, private toastr: ToastrService, private activeRoute: ActivatedRoute, private userProfileService: UserProfileService) {
    this.drawerIsOpen = false;
    // Load user
    if (localStorage.getItem('currentUser')) {
      this.User = JSON.parse(localStorage.getItem('currentUser'));
      this.userName = this.User.fullName.slice(0, 15);
    }
    this._userTypeId = this.User.usertype;
    this.isCustomer = this.User.usertype == UserType.Customer;
    this.photourl = './assets/images/default-profile.svg';
    this.GetUserDetails();
    this.ds.currentDrawerState.subscribe((data) => {
      this.drawerIsOpen = data;
    });

    //get the customer KAM Details
    this.customerKAMDetail = new CustomerKAMDetail();
    this.masterModel = new HeaderMasterModel();
    this.getCustomerKAMDetails();
    this.itSupporEmail = ITSupporEmail;

    //console.log("getTasks()");
    this.getNotDoneTotalTaskCount();

    this.getUnReadTotalNotificationCount();

    this.selectedEntity = Number(this.utility.getEntityId());

    workerService.subscribeOnGetTask((data) => {
      // show  toast for new task
      this.toastr.info(data.Message, data.Title);

      // refresh tasks
      this.getTasks();
    });

    workerService.subscribeOnGetNotification((data) => {
      // show  toast for new notification
      this.toastr.info(data.Message, data.Title);

      // refresh notification
      this.getNotifications();
    });
    this.returnUrl = this.activeRoute.snapshot.queryParams['returnUrl'] || '/';

    this.userEntity = this.utility.getEntityName();
    this.expandNavigation = false;


    //get user right type access list
    this.getUserRightTypeAccessList();

    if (localStorage.getItem('rightType'))
      this.selectedRightType = Number(localStorage.getItem('rightType'));

    this.getIsMobile();

  }


  async getUserRightTypeAccessList() {
    const rightTypeResponse = await this.authenticationService.getRightTypelst();
    if (rightTypeResponse.result == RightTypeResult.Success) {
      if (this._userTypeId == this._internalusertype)
        this.rightTypeList = rightTypeResponse.rightTypeList;
      else if (this._userTypeId == UserType.OutSource)
        this.rightTypeList = rightTypeResponse.rightTypeList.filter(x => OutSourceUserType.includes(x.id));
      else
        this.rightTypeList = rightTypeResponse.rightTypeList.filter(x => x.id == RightTypeEnum.Inspection || x.id == RightTypeEnum.Audit || x.id == RightTypeEnum.TCF || x.id == RightTypeEnum.LabTesting);
      const rightTypeAccessList = this.authenticationService.getUserRightTypeAccessList(this.rightTypeList);
      if (rightTypeAccessList.length > 0)
        this.userRightTypeAccessList = rightTypeAccessList;
    }
  }

  getUnReadNotificationCount() {
    const request = new NotificationSearchRequest();
    request.skip = this.masterModel.notifiactionSkip;
    request.notificationType = this.masterModel.notificationType;

    this.headerService.unReadNotificationCount(request).subscribe(data => {
      this.masterModel.notificationCount = data;
    }, error => {
      console.log(error)
    });
  }

  getUnReadTotalNotificationCount() {
    const request = new NotificationSearchRequest();
    request.skip = 0;
    request.notificationType = null;

    this.headerService.unReadNotificationCount(request).subscribe(data => {
      this.masterModel.notifiactionTotalCount = data.totalCount;
    }, error => {
      console.log(error)
    });
  }

  getNotDoneTaskCount() {
    const request = new TaskSearchRequest();
    request.skip = this.masterModel.taskSkip;
    request.taskType = this.masterModel.taskType;

    this.headerService.notDoneTaskCount(request).subscribe(data => {
      this.masterModel.taskCount = data;
    }, error => {
      console.log(error)
    });
  }

  getNotDoneTotalTaskCount() {
    const request = new TaskSearchRequest();
    request.skip = 0;
    request.taskType = null;

    this.headerService.notDoneTaskCount(request).subscribe(data => {
      this.masterModel.taskTotalCount = data.totalCount;
    }, error => {
      console.log(error)
    });
  }

  ngAfterViewInit() {

    this.getIsMobile();
    this.getHeaderSize();

    this.header = new Array();
    if (this.User.rights.length >= this._headercount) {
      for (let i = this._headercount; i < this.User.rights.length; i++) {
        this.header.push(this.User.rights[i])
      }
    }

  }

  getHeaderSize() {
    let headerSecondColumnLastChildWidth = this.headerSecondColumnLastChild.nativeElement.clientWidth + 20 + 36;
    let availableWidth = this.headerSecondColumnElement.nativeElement.clientWidth - headerSecondColumnLastChildWidth;
    this.calculateHeaderCount(availableWidth);
  }

  calculateHeaderCount(availableWidth) {
    let totalWidth = 0;
    this._headercount = 0;
    this.User.rights.forEach(menu => {
      let width = 20 + (menu.titleName.length * 10);
      totalWidth += width
      if (availableWidth > totalWidth) {
        this._headercount = this._headercount + 1;
      }
    })
  }

  redirect() {
    this.authenticationService.redirectToLanding();
  }

  GetMenuParentId(menuid) {
    localStorage.setItem("_activemenuid", menuid)
  }

  geturl(item: MenuItemModel) {
    return "/" + this.utility.getEntityName() + "/" + item.path;
  }

  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
    console.log(this.isMobile);
  }

  // function to toggle hamburger
  openHamburger() {
    (<HTMLElement>document.querySelector('.submenu')).classList.remove('active');
    if (this.drawerIsOpen) {
      this.drawerIsOpen = !this.drawerIsOpen;
      (<HTMLElement>document.querySelector('.main-wrapper')).classList.remove('drawer-active');
      (<HTMLElement>document.querySelector('.drawer-menu-container')).classList.remove('active');
    }
    else {
      this.drawerIsOpen = !this.drawerIsOpen;
      (<HTMLElement>document.querySelector('.main-wrapper')).classList.add('drawer-active');
      (<HTMLElement>document.querySelector('.drawer-menu-container')).classList.add('active');
    }
  }
  public signOut() {
    this.authenticationService.logout();
    this.workerService.unregister();
    this.router.navigate(['/login']);
    setTimeout(function () {
      window.location.reload();
    }, 10);
  }
  setBlobFile(blob) {
    if (blob) {
      var reader = new FileReader();
      reader.readAsDataURL(blob); // read file as data url
      reader.onload = (event) => { // called once readAsDataURL is completed
        this.photourl = (<FileReaderEventTarget>event.target).result;
      }
    }
  }
  async GetUserDetails() {
    if (this._userTypeId == this._internalusertype) {
      this.service.GetGender()
        .subscribe((res) => {
          if (res && res.result == 1) {
            this.usergender = res.gender;
            if (res.isPhotovailable)
              this.GetPhoto();
          }
          else {
            this.photourl = './assets/images/default-profile.svg';
          }

          this.getHeaderSize();
        },
          error => console.log(error)
        );

    }
    else {
      var response = await this.userProfileService.getUserProfileSummary(this.User.id);
      if (response.result == ResponseResult.Success && response.data.profileImageUrl != null && response.data.profileImageUrl != '')
        this.photourl = response.data.profileImageUrl;
    }
  }
  GetPhoto() {
    if (this._userTypeId == this._internalusertype) {
      this.service.getPicture(this.User.staffId).subscribe(data => {
        if (data && data.result == 1) {
          this.photourl = data.fileUrl;
        }
        else {
          this.SetPhoto();
        }
      }, error => {
        this.SetPhoto();
      });
    }
  }
  SetPhoto() {
    if (this._userTypeId == this._internalusertype) {
      if (this.photourl && this.usergender == 'M')
        this.photourl = 'assets/images/male.jpg';
      else if (this.photourl && this.usergender == 'F')
        this.photourl = 'assets/images/female.jpg';
      return this.photourl;
    }

  }

  toggleNotification(event, navTarget) {
    this.masterModel.notificationType = null;
    this.resetNotificationList();
    this.resetTaskList();
    this.masterModel.taskType = null;
    if (navTarget == 'notificationContainer') {
      this.masterModel.showNotificationPanel = !this.masterModel.showNotificationPanel;
      if (this.masterModel.showNotificationPanel) {
        if(this.ngNotificationSelectComponent)
          this.ngNotificationSelectComponent.clearModel();
        this.getNotifications();
        this.getUnReadNotificationCount();
        this.getNotificationType();
      }

    }
    else if (navTarget == 'taskContainer') {
      this.masterModel.showTaskPanel = !this.masterModel.showTaskPanel;
      if (this.masterModel.showTaskPanel) {
        if(this.ngTaskSelectComponent)
          this.ngTaskSelectComponent.clearModel();
        this.getTasks();
        this.getNotDoneTaskCount();
        this.getTaskType();
      }
    }
    else if (navTarget == 'helpContainer') {
      this.masterModel.showHelpPanel = !this.masterModel.showHelpPanel;
    }
    else if (navTarget == 'userContainer') {
      this.masterModel.showUserPanel = !this.masterModel.showUserPanel;
    }
  }

  public getNotificationDetail(url: string, id: string) {
    this.headerService.readNotification(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.getUnReadNotificationCount();
            this.getUnReadTotalNotificationCount();
            const todayNotification = this.masterModel.todayNotificationList.find(x => x.notification.id == id);
            if (todayNotification != null) {
              todayNotification.notification.isRead = true;
            }
            const yesterdayNotification = this.masterModel.yesterdayNotificationList.find(x => x.notification.id == id);
            if (yesterdayNotification != null) {
              yesterdayNotification.notification.isRead = true;
            }
            const olderNotification = this.masterModel.olderNotificationList.find(x => x.notification.id == id);
            if (olderNotification != null) {
              olderNotification.notification.isRead = true;
            }
          }
        },
        error => {
          console.error(error);
        });

    //if (url === undefined || url === null || url === '')
    //  return;
    //this.router.navigate([url]);
  }

  navigateTaskPage(url: string) {
    if (url === undefined || url === null || url === '')
      return;
    this.router.navigateByUrl(`/${this.userEntity}/shared/blank-page`, { skipLocationChange: true }).then(() => this.router.navigate([url]));
  }

  navigateNotificationPage(url: string,id: string) {
    this.getNotificationDetail(url, id)
    if (url === undefined || url === null || url === '')
      return;
    this.router.navigateByUrl(`/${this.userEntity}/shared/blank-page`, { skipLocationChange: true }).then(() => this.router.navigate([url]));
  }

  public allDoneTask() {
    this.headerService.allDoneTask()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.getNotDoneTaskCount();
            this.getNotDoneTotalTaskCount();
            const todayTasks = this.masterModel.todayTaskList.filter(x => !x.task.isDone);
            if (todayTasks != null && todayTasks.length > 0) {
              todayTasks.forEach(x => x.task.isDone = true);
            }
            const yesterdayTasks = this.masterModel.yesterdayTaskList.filter(x => !x.task.isDone);
            if (yesterdayTasks != null && yesterdayTasks.length > 0) {
              yesterdayTasks.forEach(x => x.task.isDone = true);
            }
            const olderTasks = this.masterModel.olderTaskList.filter(x => !x.task.isDone);
            if (olderTasks != null && olderTasks.length > 0) {
              olderTasks.forEach(x => x.task.isDone = true);
            }
          }
        },
        error => {
          console.error(error);
        });
  }

  public doneTask(url: string, id: string) {
    this.headerService.doneTask(id)
      .pipe()
      .subscribe(
        res => {
          this.getNotDoneTaskCount();
          this.getNotDoneTotalTaskCount();
          if (res && res.result == 1) {
            const todayTask = this.masterModel.todayTaskList.find(x => x.task.id == id);
            if (todayTask != null) {
              todayTask.task.isDone = true;
            }
            const yesterdayTask = this.masterModel.yesterdayTaskList.find(x => x.task.id == id);
            if (yesterdayTask != null) {
              yesterdayTask.task.isDone = true;
            }
            const olderTask = this.masterModel.olderTaskList.find(x => x.task.id == id);
            if (olderTask != null) {
              olderTask.task.isDone = true;
            }
          }
        },
        error => {
          console.error(error);
        });

    //if (url === undefined || url === null || url === '')
    //  return;
    //this.router.navigate([url]);
  }

  public readAllNotification() {
    this.headerService.readAllNotification()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.getUnReadNotificationCount();
            this.getUnReadTotalNotificationCount();
            const todayNotification = this.masterModel.todayNotificationList.filter(x => x.notification.isRead == false);
            if (todayNotification != null && todayNotification.length > 0) {
              todayNotification.forEach(x => x.notification.isRead = true);
            }
            const yesterdayNotification = this.masterModel.yesterdayNotificationList.filter(x => x.notification.isRead == false);
            if (yesterdayNotification != null && yesterdayNotification.length > 0) {
              yesterdayNotification.forEach(x => x.notification.isRead = true);
            }
            const olderNotification = this.masterModel.olderNotificationList.filter(x => x.notification.isRead == false);
            if (olderNotification != null && olderNotification.length > 0) {
              olderNotification.forEach(x => x.notification.isRead = true);
            }
          }
        },
        error => {
          console.error(error);
        });
  }

  onScroll(event, navTarget) {
    if (navTarget == 'taskContainer' && event.target.offsetHeight + Math.round(event.target.scrollTop) >= event.target.scrollHeight) {
      this.masterModel.taskSkip = this.masterModel.taskList.length;
      this.getTasks();
    }
    else if (navTarget == 'notificationContainer' && event.target.offsetHeight + Math.round(event.target.scrollTop) >= event.target.scrollHeight) {
      this.masterModel.notifiactionSkip = this.masterModel.notificationList.length;
      this.getNotifications();
    }
  }

  getNotificationType() {
    this.masterModel.notificationTypeList = Object.keys(NotificationFilterType).filter((v) => isNaN(Number(v))).map((name) => {
      return {
        id: NotificationFilterType[name as keyof typeof NotificationFilterType],
        name,
      };
    });
    if (this._userTypeId != this._internalusertype)
      this.masterModel.notificationTypeList = this.masterModel.notificationTypeList.filter(x => x.id == NotificationFilterType.Inspection || x.id == NotificationFilterType.Quotation)
  }

  getTaskType() {
    this.masterModel.taskTypeList = Object.keys(TaskFilterType).filter((v) => isNaN(Number(v))).map((name) => {
      return {
        id: TaskFilterType[name as keyof typeof TaskFilterType],
        name,
      };
    });
    if (this._userTypeId != this._internalusertype)
      this.masterModel.taskTypeList = this.masterModel.taskTypeList.filter(x => x.id == TaskFilterType.Inspection || x.id == TaskFilterType.Quotation)
  }

  changeTaskType(type) {
    this.masterModel.showTaskPanel = true;
    this.resetTaskList();
    if (type) {
      this.masterModel.taskType = type.id;
      this.getTasks();
      this.getNotDoneTaskCount();
    }
    else {
      this.masterModel.taskType = null;
      this.getTasks();
      this.getNotDoneTaskCount();
    }
    this.masterModel.showTaskPanel = true;
  }
  changeNotificationType(notificationType) {
    this.resetNotificationList();
    if (notificationType) {
      this.masterModel.notificationType = notificationType.id;
      this.getNotifications();
      this.getUnReadNotificationCount();
    }
    else {
      this.masterModel.notificationType = null;
      this.getNotifications();
      this.getUnReadNotificationCount();
    }
  }

  public getTasks() {
    this.masterModel.taskLoader = true;
    var EntityName = this.utility.getEntityName();
    const request = new TaskSearchRequest();
    request.skip = this.masterModel.taskSkip;
    request.taskType = this.masterModel.taskType;
    request.isUnread = this.masterModel.taskIsUnread;
    this.headerService.getTasklist(request)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == TaskResult.Success) {
            let result = res.data.filter(x => !this.masterModel.taskList.some(y => x.id === y.task.id));
            result.map(x => {
              switch (x.type) {
                case TaskType.LeaveToApprove:
                  this.masterModel.taskList.push(new TaskMessage(x, `Leave request pending - ${x.staffName}, take action.`, `/${EntityName}/leaverequest/leave-request/${x.linkId}`, "leave-approve.svg"));
                  break;
                case TaskType.ExpenseToApprove:
                  this.masterModel.taskList.push(new TaskMessage(x, `Expense claim checked - ${x.staffName}, take action.`, `/${EntityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-checked.svg"));
                  break;
                case TaskType.ExpenseToCheck:
                  this.masterModel.taskList.push(new TaskMessage(x, `Expense claim pending - ${x.staffName}, take action.`, `/${EntityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-checked.svg"));
                  break;
                case TaskType.ExpenseToPay:
                  this.masterModel.taskList.push(new TaskMessage(x, `Expense claim approved - ${x.staffName}, take action.`, `/${EntityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-approved.svg"));
                  break;
                case TaskType.InspectionVerified:
                  this.masterModel.taskList.push(new TaskMessage(x, `Inspection booking to Verify - INS ${x.linkId}.`, `/${EntityName}/inspedit/edit-booking/${x.linkId}`, "inspection-verified.svg"));
                  break;
                case TaskType.InspectionConfirmed:
                  this.masterModel.taskList.push(new TaskMessage(x, `Inspection booking to Confirm - INS ${x.linkId}.`, `/${EntityName}/inspedit/edit-booking/${x.linkId}`, "inspection-confirmed.svg"));
                  break;
                case TaskType.SplitInspectionBooking:
                  this.masterModel.taskList.push(new TaskMessage(x, `Inspection booking to Verify (Split from INS - ${x.parentId}) - INS ${x.linkId}.`, `/${EntityName}/inspedit/edit-booking/${x.linkId}`, "inspection-verified.svg"));
                  break;
                case TaskType.ScheduleInspection:
                  this.masterModel.taskList.push(new TaskMessage(x, `Inspection booking to Schedule - INS ${x.linkId}.`, `/${EntityName}/schedule/schedule-summary/${x.linkId}`, "inspection-rescheduled.svg"));
                  break;
                case TaskType.QuotationPending:
                  this.masterModel.taskList.push(new TaskMessage(x, `Quotation to be Create   ${x.linkId}.`, `/${EntityName}/inspsummary/quotation-pending/3/${x.linkId}`, "quoatation-created.svg"));
                  break;
                case TaskType.QuotationModify:
                  this.masterModel.taskList.push(new TaskMessage(x, `Quotation to be Modify  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-modified.svg"));
                  break;
                case TaskType.QuotationSent:
                  this.masterModel.taskList.push(new TaskMessage(x, `Quotation to be Sent to Client  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-sent.svg"));
                  break;
                case TaskType.QuotationCustomerConfirmed:
                  if(this.isCustomer)
                    this.masterModel.taskList.push(new TaskMessage(x, `Quotation to be Validate  ${x.linkId}.`, `/${EntityName}/quotations/quotation-summary/${x.linkId}`, "quoatation-confirmed.svg"));
                  else
                    this.masterModel.taskList.push(new TaskMessage(x, `Quotation to be Validate  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-confirmed.svg"));
                  break;
                case TaskType.QuotationCustomerReject:
                  this.masterModel.taskList.push(new TaskMessage(x, `Quotation to be Re-Send (customer rejected)  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-rejected.svg"));
                  break;
                case TaskType.QuotationToApprove:
                  this.masterModel.taskList.push(new TaskMessage(x, `Quotation to be Approve  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-approved.svg"));
                  break;
                case TaskType.UpdateCustomerToFB:
                  this.masterModel.taskList.push(new TaskMessage(x, `Customer update failed in FB.To recreate  ${x.linkId}.`, `/${EntityName}/cusedit/edit-customer/${x.linkId}`, "error_notification.svg"));
                  break;
                case TaskType.UpdateSupplierToFB:
                  this.masterModel.taskList.push(new TaskMessage(x, `Supplier update failed in FB.To recreate ${x.linkId}.`, `/${EntityName}/supplieredit/edit-supplier/${x.linkId}`, "error_notification.svg"));
                  break;
                case TaskType.UpdateFactoryToFB:
                  this.masterModel.taskList.push(new TaskMessage(x, `Factory update failed in FB.To recreate ${x.linkId}.`, `/${EntityName}/supplieredit/edit-supplier/${x.linkId}`, "error_notification.svg"));
                  break;
                case TaskType.UpdateProductToFB:
                  this.masterModel.taskList.push(new TaskMessage(x, `Product update failed in FB.To recreate  ${x.linkId}.`, `/${EntityName}/cusproductedit/edit-customer-product/${x.linkId}`, "error_notification.svg"));
                  break;
              }
            });
            this.masterModel.todayTaskList = this.masterModel.taskList.filter(x => x.task.dateType == TaskDateType.Today);
            this.masterModel.yesterdayTaskList = this.masterModel.taskList.filter(x => x.task.dateType == TaskDateType.Yesterday);
            this.masterModel.olderTaskList = this.masterModel.taskList.filter(x => x.task.dateType == TaskDateType.Older);
          }
          this.masterModel.taskLoader = false;
        },
        error => {
          this.masterModel.taskLoader = false;
          console.error(error);
        });
  }

  public getNotifications() {
    this.masterModel.notifiactionLoader = true;
    var entityName = this.utility.getEntityName();
    const request = new NotificationSearchRequest();
    request.skip = this.masterModel.notifiactionSkip;
    request.notificationType = this.masterModel.notificationType;
    request.isUnread = this.masterModel.notificationIsUnread;
    this.headerService.getNotifications(request)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == NotificationResult.Success) {
            let result = res.data.filter(x => !this.masterModel.notificationList.some(y => x.id === y.notification.id));
            result.map(x => {
              switch (x.type) {
                case NotificationType.LeaveApproved:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Your Leave request has been approved.`, `/${entityName}/leaverequest/leave-request/${x.linkId}`, "leave-approve.svg"));
                  break;
                case NotificationType.LeaveRejected:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Your leave request  has been rejected.`, `/${entityName}/leaverequest/leave-request/${x.linkId}`, "leave-rejected.svg"));
                  break;
                case NotificationType.LeaveApprovedHrLeave:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Leave request has been approved.`, `/${entityName}/leaverequest/leave-request/${x.linkId}`, "leave-approve.svg"));
                  break;
                case NotificationType.LeaveCanceled:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Leave request has been canceled.`, `/${entityName}/leaverequest/leave-request/${x.linkId}`, "leave-cancelled.svg"));
                  break;
                case NotificationType.ExpenseApproved:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Your Expense claim  has been approved.`, `/${entityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-approved.svg"));
                  break;
                case NotificationType.ExpenseChecked:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `your Expense claim   has been checked.`, `/${entityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-checked.svg"));
                  break;
                case NotificationType.ExpensePaid:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Your Expense claim  has been paid.`, `/${entityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-paied.svg"));
                  break;
                case NotificationType.ExpenseRejected:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Your Expense claim  has been rejected.`, `/${entityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-rejected.svg"));
                case NotificationType.InspectionRequested:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking requested (INS - ${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-requested.svg"));
                  break;
                case NotificationType.InspectionConfirmed:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking confirmed (INS - ${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-confirmed.svg"));
                  break;
                case NotificationType.InspectionModified:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking modified (INS - ${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-modified.svg"));
                  break;
                case NotificationType.InspectionCancelled:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking cancelled (INS - ${x.linkId}).`, `/${entityName}/inspcancel/cancel-booking/${x.linkId}/${BookingSearchRedirectPage.Cancel}`, "inspection-cancelled.svg"));
                  break;
                case NotificationType.InspectionVerified:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking verified (INS - ${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-verified.svg"));
                  break;
                case NotificationType.InspectionRescheduled:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking rescheduled (INS - ${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-rescheduled.svg"));
                  break;
                case NotificationType.InspectionSplit:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking split (INS - ${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-modified.svg"));
                  break;
                case NotificationType.QuotationAdd:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` New quotation created (${x.linkId}).`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-created.svg"));
                  break;
                case NotificationType.QuotationToApprove:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Quotation (${x.linkId}) Manager approved .`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-approved.svg"));
                  break;
                case NotificationType.QuotationSent:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Quotation (${x.linkId}) sent to client .`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-sent.svg"));
                  break;
                case NotificationType.QuotationCustomerConfirmed:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Quotation (${x.linkId}) validated by customer .`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-confirmed.svg"));
                  break;
                case NotificationType.QuotationCustomerReject:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Quotation (${x.linkId}) rejected by customer .`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-rejected.svg"));
                  break;
                case NotificationType.QuotationCancelled:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Quotation (${x.linkId}) cancelled .`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-cancelled.svg"));
                  break;
                case NotificationType.QuotationRejected:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Quotation (${x.linkId}) Rejected .`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-rejected.svg"));
                  break;
                case NotificationType.QuotationModified:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Quotation (${x.linkId}) Modified .`, `/${entityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-modified.svg"));
                  break;
                case NotificationType.BookingQuantityChange:
                  this.masterModel.notificationList.push(new NotificationMessage(x, ` Inspection booking quantity updated (${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-modified.svg"));
                  break;
                case NotificationType.InspectionHold:
                  this.masterModel.notificationList.push(new NotificationMessage(x, `Inspection booking Hold (INS - ${x.linkId}).`, `/${entityName}/inspedit/edit-booking/${x.linkId}`, "inspection-modified.svg"));
                  break;
                case NotificationType.FastReportGenerationFailed:
                  this.masterModel.notificationList.push(new NotificationMessage(x, x.message, '', "error_notification.svg"));
                  break;
              }
            });
            this.masterModel.todayNotificationList = this.masterModel.notificationList.filter(x => x.notification.dateType == NotificationDateType.Today);
            this.masterModel.yesterdayNotificationList = this.masterModel.notificationList.filter(x => x.notification.dateType == NotificationDateType.Yesterday);
            this.masterModel.olderNotificationList = this.masterModel.notificationList.filter(x => x.notification.dateType == NotificationDateType.Older);
          }
          this.masterModel.notifiactionLoader = false;
        },
        error => {
          this.masterModel.notifiactionLoader = false;
          console.error(error);
        });
  }

  toggleAccordion(event) {
    let current = event.currentTarget.parentElement.querySelector('.accordion').style.display;
    event.currentTarget.parentElement.querySelector('.accordion').style.display = current == 'block' ? 'none' : 'block';
  }

  redirectToProfilePage() {
    this.router.navigate(["/" + this.utility.getEntityName() + Url.UserProfile + this.User.id]);
  }

  //open the email window with to address
  openEmailWindow() {
    window.location.href = "mailto:" + ITSupporEmail;
  }

  //redirect to user manual page
  redirectUserManual() {
    let entity: string = this.utility.getEntityName();
    let path = "/userguide/user-guide";
    this.router.navigate([`/${entity}/${path}`]);
  }

  //get the customer KAM Details
  async getCustomerKAMDetails() {
    if (this.User.usertype == UserType.Customer) {
      var customerKAMDetail = await this.customerService.getCustomerKAMDetails(this.User.customerid);
      if (customerKAMDetail) {
        this.customerKAMDetail = customerKAMDetail;
      }
    }
  }

  //navigateto worms service portal
  navigateToWormsServicePortal() {
    window.open(
      WormsServicePortal,
      '_blank' // <- This is what makes it open in a new window.
    );
  }

  toggleSidebar() {
    this.expandNavigation = !this.expandNavigation;
  }

  RedirectWithRightTypeAccess(rightType) {
    if(!rightType.userHasAccess)
      return false;

    if (this.expandNavigation)
      this.expandNavigation = !this.expandNavigation;
    this.selectedRightType = rightType.id;
    //set the selected service
    localStorage.setItem('rightType', JSON.stringify(this.selectedRightType));
    //update the current user and assign back to user object
    this.User = this.authenticationService.getCurrentUser();
    this.getHeaderSize();
    //redirect to page specific to the selected right type
    this.authenticationService.RedirectWithRightTypeAccess(this.selectedRightType);
  }

  resetTaskList() {
    this.masterModel.taskSkip = 0;
    this.masterModel.taskList = [];
    this.masterModel.todayTaskList = [];
    this.masterModel.yesterdayTaskList = [];
    this.masterModel.olderTaskList = [];
  }

  resetNotificationList() {
    this.masterModel.notifiactionSkip = 0;
    this.masterModel.notificationList = [];
    this.masterModel.todayNotificationList = [];
    this.masterModel.yesterdayNotificationList = [];
    this.masterModel.olderNotificationList = [];
  }

  getUnReadNotification(){
    this.masterModel.notificationIsUnread = false;
    this.resetNotificationList();
    this.getNotifications();
    this.getUnReadNotificationCount();

  }
  getNotDoneTask(){
    this.masterModel.taskIsUnread = false;
    this.resetTaskList();
    this.getTasks();
    this.getNotDoneTaskCount();
  }
  clearAllNotification(){
    this.masterModel.notificationIsUnread = null;
    this.masterModel.notificationType =null;
    this.ngNotificationSelectComponent.clearModel();
    this.resetNotificationList();
    this.getNotifications();
    this.getUnReadNotificationCount();
  }
  clearAllTask(){
    this.masterModel.taskIsUnread = null;
    this.masterModel.taskType =null;
    this.ngTaskSelectComponent.clearModel();
    this.resetTaskList();
    this.getTasks();
    this.getNotDoneTaskCount();
  }
}
interface FileReaderEventTarget extends EventTarget {
  result: string
}
interface FileReaderEvent extends Event {
  target: FileReaderEventTarget;
  getMessage(): string;
}

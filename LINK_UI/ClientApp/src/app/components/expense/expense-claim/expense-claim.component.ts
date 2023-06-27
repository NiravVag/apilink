import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from "@angular/router";
import { first, debounceTime, switchMap, distinctUntilChanged, tap, catchError, takeUntil } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import {
  ExpenseClaimModel, ExpenseClaimResult, ExpenseClaimDetails, ExpenseClaimReceipt, ExpenseType, ExpenseClaimStatusEnum,
  ExpenseClaimTypeEnum, ExpenseClaimTypeData, ExpenseBookingDetailAccess, BookingDetail, PendingExpenseRequest, ExpenseFoodClaimRequest, ExpenseClaimMasterData
} from '../../../_Models/expense/expenseclaim.model';

import { CityType, ExpenseTypeEnum, FileContainerList, HRProfileEnum, RoleEnum, TripType, Url, UserType } from '../../common/static-data-common'
import { ExpenseService } from '../../../_Services/expense/expense.service';
import { HeaderComponent } from '../../shared/header/header.component';
import { BehaviorSubject, Observable, Subject, of } from 'rxjs';
import { NgbModalRef, NgbModal, NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { HeaderService } from '../../../_Services/header/header.service';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { AttachmentFile, FileInfo } from 'src/app/_Models/fileupload/fileupload';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { validateLocaleAndSetLanguage } from 'typescript';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { CityDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { InspectionBaseAndProductDetails, InspectionProductBaseDetailResult } from 'src/app/_Models/booking/inspectionbookingbase.model';
import { LocationService } from 'src/app/_Services/location/location.service';

@Component({
  selector: 'app-expenseClaim',
  templateUrl: './expense-claim.component.html',
  styleUrls: ['./expense-claim.component.css'],
  providers: [HeaderComponent]
})
export class ExpenseClaimComponent extends DetailComponent {
  public model: ExpenseClaimModel;
  public data: any;
  currentUser: UserModel;
  public expenseClaimTypeData: ExpenseClaimTypeData
  public expenseBookingDetailAccess: ExpenseBookingDetailAccess;
  public amountCurrencyVisible: boolean = true;
  public items: Array<any> = [];
  tripTypeList: any;
  tripTypeLoading: boolean;
  public typeahead = new EventEmitter<string>();
  public _hrProfileEnum = HRProfileEnum;
  public _roleEnum = RoleEnum;
  public _expenseClaimStatusEnum = ExpenseClaimStatusEnum;
  public _expenseClaimTypeEnum = ExpenseClaimTypeEnum;
  public selectCounryId: number = 0;
  private jsonHelper: JsonHelper;
  public expenseValidators: Array<any> = [];

  public styleOverFlow: string = "auto";
  public canEdit: boolean = false;
  public canApprove: boolean = false;
  public canCheck: boolean = false;
  public pendingExpenseRequest: PendingExpenseRequest;



  private _translate: TranslateService;
  private _toastr: ToastrService;
  public _saveloader: boolean = false;
  public _initialloader: boolean = false;
  public fileExtension: string = ""
  public fileSize: number;
  public _rejectedloader: boolean = false;
  public _otherstatusloader: boolean = false;
  public _cancelloader: boolean = false;
  public ClaimTypeToastrHide: boolean = false;

  public expenseBookingList: Array<any> = [];
  public expenseSelectedBookings: Array<any> = [];

  public isMandayCost: boolean = false;

  userTypeEnum = UserType;

  expenseClaimResponse: any;
  @ViewChild('outsourceexpenseclaimData') outSourceQCClaimData: ElementRef;

  isOutSourceAccountingRole: boolean = false;
  isAccountingRole: boolean = false;

  inspectionBaseAndProductDetails: InspectionBaseAndProductDetails;
  expenseClaimMasterData: ExpenseClaimMasterData;
  expenseTypeEnum = ExpenseTypeEnum;
  public componentDestroyed$: Subject<boolean> = new Subject;
  public cityRequest: CityDataSourceRequest;
  public startCityInput: BehaviorSubject<string>;
  public destCityInput: BehaviorSubject<string>;
  public startCityList: any;
  public destCityList: any;
  public _cityType = CityType;

  destNotEmpty(model: ExpenseClaimDetails, typeList: Array<ExpenseType>) {
    if (model == null)
      return false;

    if (model.destCity != null)
      return true;

    let isTravelling = () => {
      if (model.expenseTypeId && typeList) {
        let item = typeList.filter(x => x.id == model.expenseTypeId);
        return item != null && item.length > 0 && item[0].isTravel;
      }
    }


    return isTravelling();
  }
  startNotEmpty(model: ExpenseClaimDetails, typeList: Array<ExpenseType>) {

    if (model == null)
      return false;

    if (model.startCity != null)
      return true;


    let isTravelling = () => {
      if (model.expenseTypeId && typeList) {
        let item = typeList.filter(x => x.id == model.expenseTypeId);
        return item != null && item.length > 0 && item[0].isTravel;
      }

    }

    return isTravelling();
  }

  //validate manday only if it is outsource inspection fees
  manDayRequiredForMandayCost(model) {
    return (model.expense.expenseTypeId == ExpenseTypeEnum.MandayCost && (!model.expense.manDay || model.expense.manDay == 0)) ? true : false;
  }


  private modelRef: NgbModalRef;
  public nextStatusId: number;
  public nextStatusLabel: string;
  public isRejectValid: boolean;
  public rejectComment: string;
  constructor(
    public dateparser: NgbDateParserFormatter,
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    private service: ExpenseService,
    private bookingService: BookingService,
    private cd: ChangeDetectorRef,
    authservice: AuthenticationService,
    public utility: UtilityService, private locationService: LocationService,
    private modalService: NgbModal, private activeRoute: ActivatedRoute,
    private header: HeaderComponent, private headerService: HeaderService,
    public refService: ReferenceService) {
    super(router, route, translate, toastr);
    this.currentUser = authservice.getCurrentUser();
    this.validator.setJSON("expense/expense-claim.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    this.fileExtension = 'png,jpg,jpeg,xlsx,pdf,doc,docx,xls';
    this.fileSize = 2097152;//2 mb
    this._translate = translate;
    this._toastr = toastr;
    this.expenseClaimTypeData = new ExpenseClaimTypeData();
    this.expenseClaimTypeData.bookingDetails = [];
    this.expenseClaimTypeData.selectedBookingDetails = [];
    this.expenseBookingDetailAccess = new ExpenseBookingDetailAccess();
    this.expenseClaimResponse = [];
    this.expenseClaimMasterData = new ExpenseClaimMasterData();
    this.cityRequest = new CityDataSourceRequest();
    this.startCityInput = new BehaviorSubject<string>("");
    this.destCityInput = new BehaviorSubject<string>("");
    this.startCityList = [];
    this.destCityList = [];
    //set isOutSourceAccountingRole if current user is of of type 'outsource accounting role'
    if (this.currentUser.roles.filter(x => x.id == RoleEnum.OutSourceAccountingRole).length > 0)
      this.isOutSourceAccountingRole = true;

    //set accounting Role if current user is of of type 'accounting role'
    if (this.currentUser.roles.filter(x => x.id == RoleEnum.ExpenseRoleAccess).length > 0)
      this.isAccountingRole = true;

    this.getCityListBySearch(this._cityType.StartCity, null);
    this.getCityListBySearch(this._cityType.DestCity, null);
    // router.events.subscribe(data => {
    //   if (data instanceof NavigationStart) {
    //     let id = route.snapshot.paramMap.get("id");
    //     this.onInit(id);
    //   }
    // });
  }


  onInit(id?: any) {
    this.activeRoute.params.subscribe(routeParams => {
      this.init(routeParams.id);
    });
  }

  init(id?) {
    this.model = new ExpenseClaimModel();
    this.data = {};
    this.validator.isSubmitted = false;
    this._initialloader = true;
    this.getClaimTypeList();
    this.getExpenseClaim(id);
    this.getTripTypeList();
  }

  // Get the claim type data list
  getClaimTypeList() {
    this.service.getClaimTypeList()
      .pipe()
      .subscribe(
        res => {
          this.processSucccessClaimTypeList(res);
        },
        error => {
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_CANNOT_GET');
        }
      );
  }

  //get entity list
  getTripTypeList() {
    this.tripTypeLoading = true;
    this.refService.getTripTypeList()
      .pipe(first())
      .subscribe(
        response => {
          this.tripTypeLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.tripTypeList = response.dataSourceList;
          }
        },
        error => {
          this.tripTypeLoading = false;
          this.setError(error);
        });
  }

  //process claimtype success block
  processSucccessClaimTypeList(res) {
    if (res && res.expenseClaimTypeList) {
      this.expenseClaimTypeData.expenseClaimTypeList = res.expenseClaimTypeList;
    }
    else {
      this.expenseClaimTypeData.expenseClaimTypeList = [];
    }
  }

  //Get the expense claim by id if id is null get the master data else expense data
  getExpenseClaim(id) {
    this.service.getExpenseClaim(id)
      .pipe()
      .subscribe(
        res => {
          this.processSuccessExpenseClaim(res);
        },
        error => {
          this.setError(error);
          this._initialloader = false;
          this.loading = false;
        });
  }

  //process success expense claim data
  processSuccessExpenseClaim(res) {
    if (res && res.result == ExpenseClaimResult.Success) {
      this.assignExpenseClaimData(res);
    }
    else {
      this.canEdit = false;
      this.canApprove = false;
      this.canCheck = false;

      switch (res.result) {
        case ExpenseClaimResult.CannotFindCountries:
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_COUNTRIES_NOUTFOUND');
          break;
        case ExpenseClaimResult.CannotFindCurrentStaff:
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_STAFF_NOUTFOUND');
          break;
        case ExpenseClaimResult.CannotFindLocation:
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_LOCATION_NOUTFOUND');
          break;
        case ExpenseClaimResult.CannotFindCurrencies:
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_CURRENCY_NOUTFOUND');
          break;
        case ExpenseClaimResult.CannotFindExpenseTypes:
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_TYPES_NOUTFOUND');
          break;
        case ExpenseClaimResult.CannotFindCurrentExpenseClaim:
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPENSE_NOUTFOUND');
          break;
      }
    }
    this._initialloader = false;
    this.loading = false;
  }

  //assign expense claim data
  assignExpenseClaimData(res) {
    this.expenseBookingDetailAccess = res.expenseBookingDetailAccess;
    this.data = res;
    this.model = res.expenseClaim;

    if (this.model.id > 0) {
      this.model.claimTypeId = res.expenseClaim.claimTypeId;
      this.isMandayCost = this.model.expenseList.some(x => x.expenseTypeId === ExpenseTypeEnum.MandayCost);
    }
    else
      this.assignClaimTypeData();

    if (this.model.id <= 0 && this.model.claimTypeId != this._expenseClaimTypeEnum.NonInspection)
      this.changeExpenseClaimType();

    if (this.model.id > 0 && this.model.claimTypeId > 0 &&
      this.model.claimTypeId != this._expenseClaimTypeEnum.NonInspection)
      this.getEditBookingDetails();

    this.populateExpenseList();

    this.canEdit = res.canEdit;
    this.canApprove = res.canApprove;
    this.canCheck = res.canCheck;
    if (this.model.expenseList == null || this.model.expenseList.length == 0) {
      this.addExpense(null);
    }
    this.model.managerApproveRequired = true;
  }
  assignClaimTypeData() {
    if (this.model.userTypeId == this.userTypeEnum.OutSource) {
      this.model.claimTypeId = this._expenseClaimTypeEnum.Inspection;
    }
    else if (this.currentUser.profiles.filter(x => x == this._hrProfileEnum.Inspector).length > 0) {
      this.model.claimTypeId = this._expenseClaimTypeEnum.Inspection;
    }
    else if (this.currentUser.profiles.filter(x => x == this._hrProfileEnum.Auditor).length > 0) {
      this.model.claimTypeId = this._expenseClaimTypeEnum.Audit;
    }
    else
      this.model.claimTypeId = this._expenseClaimTypeEnum.NonInspection;
  }

  //Get the booking details in edit scenario
  getEditBookingDetails() {
    var isEdit = this.model.id ? true : false;
    if (this.model.id)
      this.getBookingDetails(this.model.claimTypeId, this.model.id, isEdit);
  }

  //add the expense claim details in the expenseValidators list along the valdition file
  populateExpenseList() {
    this.expenseValidators = [];
    if (this.model.expenseList != null) {
      for (let item of this.model.expenseList) {
        let expenseType = this.data.expenseTypeList.find(x => x.id == item.expenseTypeId);
        if (expenseType)
          item.isTravelExpense = expenseType.isTravel;
        this.expenseValidators.push({ expense: item, validator: Validator.getValidator(item, "expense/expense-details.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
        this.calculateAmount(item);
      }
    }
  }


  getViewPath(): string {
    return "expenseclaim/expense-claim";
  }

  getEditPath(): string {
    return "expenseclaim/expense-claim";
  }

  public getAddPath(): string {
    return "expenseclaim/expense-claim";
  }

  //expense claim type dropdown change event
  changeExpenseClaimType() {
    this.amountCurrencyVisible = true;
    this.getBookingDetails(this.model.claimTypeId, this.model.id, false);
  }

  //Make all booking items to select when select main checkbox item
  selectAllBookings() {
    this.expenseClaimTypeData.selectedBookingDetails = [];
    for (var i = 0; i < this.expenseClaimTypeData.bookingDetails.length; i++) {

      var bookingDetail = this.expenseClaimTypeData.bookingDetails[i];

      //map the bookingname with servicedate by default
      var serviceDate = (bookingDetail.serviceDateFrom == bookingDetail.serviceDateTo) ? bookingDetail.serviceDateFrom : bookingDetail.serviceDateFrom + "-" + bookingDetail.serviceDateTo;
      let bookingName = bookingDetail.bookingId + " (" + serviceDate + ")";

      bookingDetail.selected = this.expenseClaimTypeData.selectedAllBooking;
      if (this.expenseClaimTypeData.selectedAllBooking) {

        //if user has role 'OutSource Accounting' Role
        if (this.isOutSourceAccountingRole) {

          var qcList = this.expenseClaimTypeData.bookingDetails.map(x => x.qcId);

          var distinctqcList = qcList.filter((n, i) => qcList.indexOf(n) === i);

          //if selected bookings belongs to more than one qc then add qc name also
          if (distinctqcList.length > 1)
            bookingName = bookingDetail.bookingId + " (" + serviceDate + ")-" + "(" + bookingDetail.qcName + ")";

        }

        this.expenseClaimTypeData.selectedBookingDetails.push({ "id": i + 1, "bookingNo": bookingDetail.bookingId, "name": bookingName, "qcId": bookingDetail.qcId, "qcName": bookingDetail.qcName });

      }
    }
  }


  //push the booking into selectedBookingDetails from the selected bookings from the dropdown
  assignSelectedBookings() {

    this.expenseClaimTypeData.selectedBookingDetails = [];

    var idIndex = 1;

    this.expenseClaimTypeData.bookingDetails.forEach(item => {
      if (item.selected) {
        var serviceDate = (item.serviceDateFrom == item.serviceDateTo) ? item.serviceDateFrom : item.serviceDateFrom + "-" + item.serviceDateTo;

        let bookingName = item.bookingId + " (" + serviceDate + ")";

        //if user is outsource accounting role and user select bookings belongs to more than qc then append the qcname along with the bookingno
        if (this.isOutSourceAccountingRole) {
          var qcList = this.expenseClaimTypeData.bookingDetails.filter(x => x.selected).map(x => x.qcId);
          var distinctqcList = qcList.filter((n, i) => qcList.indexOf(n) === i);

          if (distinctqcList.length > 1)
            bookingName = item.bookingId + " (" + serviceDate + ")-" + "(" + item.qcName + ")";

        }
        this.expenseClaimTypeData.selectedBookingDetails.push({ "id": idIndex, "bookingNo": item.bookingId, "name": bookingName, "qcId": item.qcId, "qcName": item.qcName });
      }
      if (!item.selected) {
        var expenseData = this.expenseValidators.filter(x => x.expense.bookingNo == item.bookingId);
        if (expenseData) {
          expenseData.forEach(element => {
            element.expense.bookingNo = null;
            element.expense.qcId = null;
            element.expense.qcName = null;
          });
        }
      }

      idIndex = idIndex + 1;

    });
    this.expenseClaimTypeData.selectedAllBooking = this.expenseClaimTypeData.bookingDetails.every(function (item: BookingDetail) {
      return item.selected == true;
    });
  }

  //set the booking data to be selected from the expense list data
  setBookingData() {

    this.model.expenseList.forEach(item => {
      this.expenseClaimTypeData.bookingDetails.filter(x => x.bookingId == item.bookingNo).forEach(y => y.selected = true);
    });

    this.assignSelectedBookings();

    this.model.expenseList.forEach(item => {
      var selectedBookingDetail = this.expenseClaimTypeData.selectedBookingDetails.find(x => x.bookingNo == item.bookingNo);
      if (selectedBookingDetail)
        item.selectedBookingId = selectedBookingDetail.id;
    });


  }


  processSuccessBookingDetails(res) {
    //set booking detail access from the bookingdetail ajax method if the role is auditor or inspector and expense
    //claim status is new,pending,rejected
    if (this.model.statusId == this._expenseClaimStatusEnum.Pending
      || this.model.statusId == this._expenseClaimStatusEnum.Rejected || this.model.statusId == this._expenseClaimStatusEnum.New) {
      if (res.expenseBookingDetailAccess)
        this.expenseBookingDetailAccess = res.expenseBookingDetailAccess;
    }
    if (res.expenseBookingDetailList && res.expenseBookingDetailList.length > 0) {
      this.expenseClaimTypeData.selectedAllBooking = false;
      this.expenseClaimTypeData.bookingDetails = res.expenseBookingDetailList;
      this.expenseBookingList = [...res.expenseBookingDetailList];

      if (!(this.model.id > 0))
        this.ClaimTypeChanged();

      if ((this.currentUser.profiles.filter(x => x == this._hrProfileEnum.Auditor).length > 0
        || this.currentUser.profiles.filter(x => x == this._hrProfileEnum.Inspector).length > 0)
        && this.expenseBookingDetailAccess.bookingDetailVisible) {
        if (!this.model.id) {// new 
          this.toggleTabContainer('tab1');
          this.amountCurrencyVisible = false;
        }
        else // edit
        {
          this.amountCurrencyVisible = true;
        }

      }
      if (this.model.id) {
        this.setBookingData();
      }
    }
    else {
      this.expenseClaimTypeData.bookingDetails = [];
      this.toggleTabContainer('tab2');
      if (!this.model.id) {
        if ((!this.ClaimTypeToastrHide) && this.model.claimTypeId == this._expenseClaimTypeEnum.Audit)
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_NO_AUDITBOOKING');
        else if ((!this.ClaimTypeToastrHide) && this.model.claimTypeId == this._expenseClaimTypeEnum.Inspection)
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_NO_INSPECTIONBOOKING');
      }
    }
    //show booking no dropdown on edit case
    if (this.model.id && this.model.claimTypeId != this._expenseClaimTypeEnum.NonInspection) {
      this.expenseBookingDetailAccess.bookingNoVisible = true;
    }
    this.ClaimTypeToastrHide = false;
  }
  ClaimTypeChanged() {
    if (this.currentUser.profiles.filter(x => x == this._hrProfileEnum.Auditor).length > 0
      && this.currentUser.profiles.filter(x => x == this._hrProfileEnum.Inspector).length > 0) {
      if (this.expenseClaimTypeData.selectedBookingDetails != null &&
        this.expenseClaimTypeData.selectedBookingDetails.length > 0)
        this.expenseClaimTypeData.selectedBookingDetails = [];

      this.expenseValidators.forEach(function (item: any) {
        item.expense.bookingNo = null;
      });
    }
  }

  changeBookingNo(event) {
    if (event) {
      let bookingIds = event.map(a => a.bookingId);
      var dataBookingList = [...this.expenseBookingList];
      if (bookingIds.length > 0) {
        this.expenseClaimTypeData.bookingDetails = dataBookingList.filter(x => bookingIds?.includes(x.bookingId));
      }
      else {
        this.expenseClaimTypeData.bookingDetails = dataBookingList;
      }
    }

  }

  getBookingDetails(claimTypeId, expenseId, isEdit) {
    this.service.getBookingDetail(claimTypeId, expenseId, isEdit)
      .pipe()
      .subscribe(
        res => {
          this.processSuccessBookingDetails(res);
        },
        error => {
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_CANNOT_GET');
        }
      );
  }

  save() {
    this.validator.isSubmitted = true;

    for (let item of this.expenseValidators)
      item.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this._saveloader = true;
      this.saveExpenseClaim();
    }
  }

  saveExpenseClaim() {
    this.service.saveExpenseClaim(this.model)
      .subscribe(
        res => {
          if (res && res.result == 1) {

            this.ClaimTypeToastrHide = true;

            if (this.isOutSourceAccountingRole && !this.model.id) {
              this.expenseClaimResponse = res.expenseClaimList;

              this.modelRef = this.modalService.open(this.outSourceQCClaimData, { windowClass: "md1ModelWidth", centered: true });

              this.init();
            }
            else {
              this.showSuccess('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_SAVE_SUCCESS');

              if (this.fromSummary)
                this.return("expensesearch/expenseclaim-list");
              else
                this.init();
            }

          }
          else {
            switch (res.result) {
              case 2:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_REQUESTNOTCORRECT');
                break;
              case 3:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_CURRENTCLAIM_NOTFOUND');
                break;
              case 5:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_CANNOT_SAVE_EXPENSE_OTHER_COMPANY');
                break;
              case 6:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_CANNOT_SAVE_EXPENSE_CLAIMNO_EXIST');
                break;
              case 7: {
                var bookingIds = res.createdExpenseBookingIds.join(',')
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', this.utility.textTranslate('EXPENSE.MSG_CLAIM_ALREADY_DONE_FOR_INSPECTION') + bookingIds);
                break;
              }

            }
          }
          this._saveloader = false;
        },
        error => {
          if (error == "Unauthorized")
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'ERROR.MSG_MESSAGE_401');
          else
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EDIT_SUPPLIER.MSG_UNKNONW_ERROR');
          this._saveloader = false;
        });
  }

  //navigate the page to edit expense claim page
  navigateToEditExpenseClaim(expenseId) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditExpenseClaim + expenseId;
    window.open(editPage);
  }

  isFormValid() {
    var validationResult = this.validator.isValid('countryId')
      && this.validator.isValid('claimTypeId')
      && this.expenseValidators.every((x) => x.validator.isValid('expenseDate'))
      && this.expenseValidators.every((x) => x.validator.isValid('expenseTypeId'))
      && (!this.model.isAutoExpense ? this.expenseValidators.every((x) => x.validator.isValid('actualAmount')) : true)
      && this.expenseValidators.every((x) => x.validator.isValidIfAsync('startCity', this.destNotEmpty, this.data.expenseTypeList))
      && this.expenseValidators.every((x) => x.validator.isValidIfAsync('destCity', this.startNotEmpty, this.data.expenseTypeList))
      && this.expenseValidators.every((x) => x.validator.isValidIfAsync('tripMode', this.startNotEmpty, this.data.expenseTypeList))
      && this.expenseValidators.every((x) => x.validator.isValidIf('manDay', this.manDayRequiredForMandayCost(x)));


    if (validationResult) {
      if (this.expenseValidators.length == 0) {
        validationResult = false;
        this.showError("EXPENSE.EXPENSECLAIM_RESULT", 'EXPENSE.MSG_ATLEAST_ONE_EXPENSE_CLAIM')
        return validationResult;
      }
      else if (this.model.claimTypeId != this._expenseClaimTypeEnum.NonInspection && this.expenseClaimTypeData.bookingDetails.length > 0)
        validationResult = this.expenseValidators.every((x) => x.validator.isValid('bookingNo'));
      if (this.model.claimTypeId != this._expenseClaimTypeEnum.NonInspection && this.expenseClaimTypeData.bookingDetails.length == 0) {
        validationResult = false;
        this.showWarning("EXPENSE.EXPENSECLAIM_RESULT", 'EXPENSE.MSG_NO_BOOKING')
        return validationResult;
      }
      else if (this.model.claimTypeId == this._expenseClaimTypeEnum.NonInspection) {
        validationResult = this.validator.isValid('expensePuropose');
      }
    }
    return validationResult;
  }


  addExpense(itemInfo: ExpenseClaimDetails) {

    let item: ExpenseClaimDetails;

    if (itemInfo == null)
      item = {
        id: 0,
        bookingNo: null,
        actualAmount: 0,
        amount: 0,
        currencyId: this.model.currencyId,
        description: "",
        destCity: null,
        exchangeRate: 1,
        expenseDate: {},
        expenseTypeId: null,
        receipt: false,
        startCity: null,
        tripMode: null,
        files: [],
        qcId: null,
        qcName: null,
        selectedBookingId: null,
        tax: null,
        taxAmount: 0,
        manDay: null,
        isTravelExpense: false,
        startCityLoading: false,
        destCityLoading: false
      };
    else
      item = {
        id: 0,
        bookingNo: itemInfo.bookingNo,
        actualAmount: itemInfo.actualAmount,
        amount: itemInfo.amount,
        currencyId: itemInfo.currencyId,
        description: itemInfo.description,
        destCity: itemInfo.destCity,
        exchangeRate: itemInfo.exchangeRate,
        expenseDate: itemInfo.expenseDate,
        expenseTypeId: itemInfo.expenseTypeId,
        receipt: itemInfo.receipt,
        startCity: itemInfo.startCity,
        tripMode: itemInfo.tripMode,
        files: [],
        qcId: itemInfo.qcId,
        qcName: itemInfo.qcName,
        selectedBookingId: null,
        tax: null,
        taxAmount: null,
        manDay: null,
        isTravelExpense: itemInfo.isTravelExpense,
        startCityLoading: false,
        destCityLoading: false
      };

    if (this.model.expenseList == null)
      this.model.expenseList = [];

    this.model.expenseList.push(item);
    this.expenseValidators.push({ expense: item, validator: Validator.getValidator(item, "expense/expense-details.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

    //this.setAmount(item);
    this.calculateAmount(item);
  }

  //calculate the amount based on tax ,manday,exchange rate
  calculateAmount(item) {
    if (item.actualAmount < 0 || item.manday < 0)
      return;
    var actualAmount = item.actualAmount;

    //do if the expense type is outsource inspecion fees
    if (item.expenseTypeId == ExpenseTypeEnum.MandayCost && item.manDay > 0)
      actualAmount = item.manDay * actualAmount;

    //calculate by tax and add tax amt
    if (item.tax > 0) {
      item.taxAmount = Math.round((item.tax / 100) * actualAmount);
      actualAmount = actualAmount + item.taxAmount;
    }
    else {
      item.taxAmount = Math.round((item.tax / 100) * actualAmount);
      actualAmount = actualAmount + item.taxAmount;
    }

    //add the exchange rate
    item.amount = Math.round(item.exchangeRate * actualAmount * 100) / 100;

    this.setTotalAmount();
  }

  setTotalAmount() {
    let total: number = 0;

    if (this.model.expenseList != null) {
      for (let item of this.model.expenseList) {
        total = total + item.amount;
      }
    }

    this.model.totalAmount = Math.round(total * 100) / 100;


  }

  setRate($event: number, item: ExpenseClaimDetails) {
    this.getFoodAllowanceExpense(item, item.expenseTypeId, $event, null);

    this.service.getCurrecyRate(this.model.currencyId, $event, `${this.model.claimDate.day}-${this.model.claimDate.month}-${this.model.claimDate.year}`)
      .pipe()
      .subscribe(
        res => {
          item.exchangeRate = res;
          this.calculateAmount(item);
        },
        error => {
          item.exchangeRate = 1;
          this.calculateAmount(item);
        });


  }

  setAmount(item: ExpenseClaimDetails) {
    item.amount = Math.round(item.exchangeRate * item.actualAmount * 100) / 100;

    this.setTotalAmount();
  }

  disableOverFlow() {
    this.styleOverFlow = "unset";
  }

  enableOverflow() {
    this.styleOverFlow = "auto";
  }

  closeFix(event, datePicker) {

    if (event.target.attributes.ngbdatepickerdayview == null
      && (event.target.attributes.class == null || (event.target.attributes.class.value != "ng-input" && event.target.attributes.class.value != "ng-arrow-wrapper" && event.target.attributes.class.value != "ng-select-container"))
      && (event.target.attributes.role == null || event.target.attributes.role.value != "combobox")) {
      this.enableOverflow();
    }
    //else if (event.target.offsetParent.nodeName != "NGB-DATEPICKER") {
    //  datePicker.close();
    //  console.log("theeeeeeeeeeeere 2");
    //}
  }
  getShortFileName(filename) {
    if (filename && filename.length > 25)
      return filename.substring(0, 25) + "...";
    else
      return filename;
  }
  selectFiles(event, item: ExpenseClaimDetails) {
    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: this.fileSize,
      uploadFileExtensions: this.fileExtension,
      uploadLimit: this.fileSize,
      containerId: FileContainerList.Expense,
      token: "",
      fileDescription: null
    }

    modalRef.componentInstance.fromParent = fileInfo;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {
          if (item.files) {
            item.files.push(element);
          }
        });
      }

    }, (reason) => {

    });
    // if (event.target.files) {

    //   if (item.files == null)
    //     item.files = [];
    //   if (event.target.files.length > 0 && this.isValidFileExtension(event.target.files) && this.isValidFileSize(event.target.files)) {
    //     for (let file of event.target.files) {
    //       let fileItem: ExpenseClaimReceipt = {
    //         fileName: file.name,
    //         file: file,
    //         isNew: true,
    //         id: 0,
    //         mimeType: "",
    //         guidId: this.newGuid()
    //       };
    //       item.files.push(fileItem);
    //     }
    //   }
    // }
  }
  isValidFileSize(files) {
    var valid = true;
    for (let f of files) {
      if (f.size > this.fileSize) {
        valid = false;
        this.showError('EXPENSE.LBL_UPLOAD', 'EXPENSE.MSG_FILESIZE_ERROR');
        break;
      }
    }
    return valid;
  }
  isValidFileExtension(files) {
    var valid = true;
    var extensions = (this.fileExtension.split(',')).map(function (x) { return x.toLocaleUpperCase().trim() });
    for (var i = 0; i < files.length; i++) {
      var ext = files[i].name.toUpperCase().split('.').pop() || files[i].name;
      var exists = extensions.includes(ext);
      if (!exists) {
        valid = false;
        this.showError('EXPENSE.LBL_UPLOAD', 'EXPENSE.MSG_FILEEXT_ERROR');
        break;
      }
    }
    return valid;
  }
  getFile(file: AttachmentFile) {

    //this.downloadloading = true;
    if (file.fileUrl) {
      window.open(file.fileUrl);
      //this.downloadloading = false;

    }

    if (!file.isNew) {
      this.service.getFile(file.id)
        .subscribe(res => {
          this.downloadFile(res, file.mimeType);
        },
          error => {

          });
    }
    else {
      const url = window.URL.createObjectURL(file.file);
      window.open(url);
    }
  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }

  getUploadCellWidth(files: Array<ExpenseClaimReceipt>): string {
    // console.log("ti emchi 3ad");
    return files != null && files.length > 0 ? "300px" : "5px";
  }


  removeExpense(index) {
    this.model.expenseList.splice(index, 1);
    this.expenseValidators.splice(index, 1);
  }

  removeFile(index, item: ExpenseClaimDetails) {
    item.files.splice(index, 1);
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  getAllNewFiles(): Array<ExpenseClaimReceipt> {
    var data: Array<ExpenseClaimReceipt> = [];

    for (let item of this.model.expenseList) {
      for (let file of item.files.filter(x => x.isNew)) {
        data.push(file);
      }
    }

    return data;
  }

  openConfirm(content) {
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  openConfirmStatus(content, idStatus: number) {


    if (this.canCheck && this.model.statusId == this._expenseClaimStatusEnum.Pending
      && this.model.isAutoExpense != null && this.model.isAutoExpense) {


      if (this.model.isTraveAllowanceExist && this.model.isFoodAllowanceExist) {
        this.showCheckOrApproveConfirm(content, idStatus);
      }
      else {
        if (!this.model.isTraveAllowanceExist) {
          this.showWarning("PendingExpense", "Travel allowance not available, please check");
        }
        else if (!this.model.isFoodAllowanceExist) {
          this.showWarning("PendingExpense", "Food allowance not available, please check");
        }
      }

      // this.pendingExpenseRequest = new PendingExpenseRequest();
      // this.pendingExpenseRequest.qcId = this.model.staffId;
      // this.pendingExpenseRequest.bookingList = this.model.expenseList.map(x => x.bookingNo);

      // this.service.checkPendingExpenseExist(this.pendingExpenseRequest)
      //   .subscribe(res => {
      //     if (res) {
      //       this.showWarning("PendingExpense", "Pending expense available, please check");
      //     }
      //     else {


      //     }
      //   },
      //     error => {
      //       this.setError(error);
      //     });

    }
    else {
      this.showCheckOrApproveConfirm(content, idStatus);
    }
  }

  showCheckOrApproveConfirm(content, idStatus) {

    if (this.canCheck && this.model.statusId == this._expenseClaimStatusEnum.Pending
      && this.model.isAutoExpense != null && this.model.isAutoExpense && this.model.managerApproveRequired) {
      this.nextStatusId = 2;
      this.nextStatusLabel = "Approved";
    }
    else {

      this.nextStatusId = idStatus;
      switch (idStatus) {
        case 2:
          this.nextStatusLabel = "Approved";
          break;
        case 4:
          this.nextStatusLabel = "Paid";
          break;
        case 5:
          this.nextStatusLabel = "Checked";
          break;
      }

    }

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  reject() {
    if (!this.rejectComment || this.rejectComment == "") {
      this.isRejectValid = false;
      return;
    }
    this._rejectedloader = true;
    this.service.reject(this.model.id, this.rejectComment)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.init(this.model.id)
          }
          else {
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
          this._rejectedloader = false;
        },
        error => {
          this.setError(error);
          this._rejectedloader = false;
        });

    this.modelRef.close();
    this.rejectComment = "";

  }

  setStatus(idstatus: number) {
    this._otherstatusloader = true;
    this.service.setStatus(this.model.id, this.nextStatusId, false)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.init(this.model.id);

          }
          else {
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
          this._otherstatusloader = false;
        },
        error => {
          this._otherstatusloader = false;
          this.setError(error);
        });

    this.modelRef.close();
    this.nextStatusId = 0;
    this.nextStatusLabel = '';
  }

  cancel() {
    this._cancelloader = true;
    this.service.setStatus(this.model.id, 6, false)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.init(this.model.id);

          }
          else {
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
          this._cancelloader = false;
        },
        error => {
          this._cancelloader = false;
          this.setError(error);
        });

    this.modelRef.close();
  }

  openCancelStatus(content) {

    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });

    this.modelRef.result.then((result) => {

    }, (reason) => {

    });

  }


  /**
   * function to toggle tabs on click
   * @param {event} event     [current event]
   * @param {string} tabTarget [targeted tab id]
   */
  toggleTab(event, tabTarget) {
    let tabs = event.target.parentNode.children;
    for (let tab of tabs) {
      tab.classList.remove('active');
    }
    event.target.classList.add('active');

    let tabContainers = document.querySelector('#' + tabTarget).parentNode.childNodes;
    for (let container of <any>tabContainers) {
      container.classList.remove('active');
    }
    document.getElementById(tabTarget).classList.add('active');
    this.amountCurrencyVisible = (tabTarget == "tab1") ? false : true;
  }

  toggleTabContainer(tabTarget) {
    let tabContainers = document.querySelector('#' + tabTarget).parentNode.childNodes;
    for (let container of <any>tabContainers) {
      container.classList.remove('active');
    }
    document.getElementById(tabTarget).classList.add('active');

    let tabHeading = document.querySelector('#' + tabTarget).parentNode.parentNode.firstChild.childNodes;

    for (let container of <any>tabHeading) {
      if (container.classList)
        container.classList.remove('active');
    }

    if (tabTarget == 'tab1') {
      if (tabHeading && tabHeading.length == 3) {
        var addLiClass = <any>tabHeading[1];
        if (addLiClass.classList)
          addLiClass.classList.add('active');
      }
    }
    else {
      if (tabHeading && tabHeading.length >= 2) {
        var addLiClass = <any>tabHeading[1];
        if (addLiClass.classList)
          addLiClass.classList.add('active');
      }
    }
  }
  bookingSelect(expenseData) {

    //apply the booking info
    var selectedBookingDetail = this.expenseClaimTypeData.selectedBookingDetails.find(x => x.id == expenseData.expense.selectedBookingId);

    if (selectedBookingDetail) {

      expenseData.expense.bookingNo = selectedBookingDetail.bookingNo;

      var bookingDetail = this.expenseClaimTypeData.bookingDetails.find(x => x.bookingId == expenseData.expense.bookingNo);

      if (bookingDetail) {
        if (Object.keys(expenseData.expense.expenseDate).length == 0) {

          var date = this.dateparser.parse(bookingDetail.serviceDateFrom);

          expenseData.expense.expenseDate = new NgbDate(date.year, date.month, date.day);

        }
      }
      //object empty check 

      expenseData.expense.qcId = selectedBookingDetail.qcId;
      expenseData.expense.qcName = selectedBookingDetail.qcName;

    }
  }

  numericValidation(event, length) {
    this.utility.numericValidation(event, length);
  }

  //fetch the booking details  to open in the popup
  viewBookingDetails(content, bookingId) {

    if (bookingId) {
      this.bookingService.getInspectionProductBaseDetail(bookingId)
        .pipe()
        .subscribe(
          response => {
            if (response.result == InspectionProductBaseDetailResult.Success) {
              this.inspectionBaseAndProductDetails = response;

              this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true });
            }
            else {
              this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
            }
            this._cancelloader = false;
          },
          error => {
            this._cancelloader = false;
            this.setError(error);
          });
    }
    else
      this.showWarning('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_PLS_SELECT_BOOKING');

  }
  changeExpenseDate(expensedate, expenseDetail) {
    this.getFoodAllowanceExpense(expenseDetail, expenseDetail.expenseTypeId, null, expensedate);

  }

  ///expense type change event to handle the travel and outsource inspection fees
  changeExpenseType(expenseDetail, expenseTypeId) {

    this.getFoodAllowanceExpense(expenseDetail, expenseTypeId.id, null, null);

    this.isMandayCost = false;
    expenseDetail.isTravelExpense = false;

    expenseDetail.startCity = null;
    expenseDetail.destCity = null;
    expenseDetail.tripMode = null;

    this.isMandayCost = this.model.expenseList.some(x => x.expenseTypeId === ExpenseTypeEnum.MandayCost);
    if (expenseTypeId != ExpenseTypeEnum.MandayCost) {
      expenseDetail.manDay = null;
    }

    let expenseType = this.data.expenseTypeList.find(x => x.id == expenseTypeId.id);

    if (expenseType)
      expenseDetail.isTravelExpense = expenseType.isTravel;

    this.calculateAmount(expenseDetail);
  }

  mapRequest(expenseDetail, currencyId: number, expenseDate) {
    var request = new ExpenseFoodClaimRequest();
    request.countryId = this.model.countryId;

    //expense date get from current change
    if (expenseDate != null)
      request.expenseDate = expenseDate;
    else
      request.expenseDate = expenseDetail.expenseDate;

    //currency get from current change
    if (currencyId && currencyId > 0)
      request.currencyId = currencyId;
    else
      request.currencyId = expenseDetail.currencyId;
    return request;
  }

  //get food allowance amount
  getFoodAllowanceExpense(expenseDetail, expenseTypeId, currencyId: number, expenseDate) {
    //if internal user and expense type is food allowance
    if (this.currentUser.usertype == UserType.InternalUser && expenseTypeId == ExpenseTypeEnum.FoodAllowance) {

      if (expenseDetail.expenseDate && expenseDetail.expenseDate.day > 0 &&
        expenseDetail.expenseDate.month > 0 && expenseDetail.expenseDate.year > 0) {
        var request = this.mapRequest(expenseDetail, currencyId, expenseDate);

        this.getExpenseFoodAmountByCountryAndDate(request, expenseDetail);
      }
      else {
        setTimeout(() => {
          expenseDetail.expenseTypeId = null;
        }, 100);
        this.calculateAmount(expenseDetail);
        this.showWarning('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.LBL_SELECT_EXPENSE_DATE');
      }
    }
  }

  async getExpenseFoodAmountByCountryAndDate(request: ExpenseFoodClaimRequest, expenseDetail) {
    try {
      this.expenseClaimMasterData.pageLoading = true;
      let response = await this.service.getExpenseFoodAmountByCountryAndDate(request);
      this.processSucccessFoodAllowance(response, expenseDetail);
    }
    catch (e) {
      this.expenseClaimMasterData.pageLoading = false;
      console.error(e);
      this.showError('QUOTATION.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
    }
  }

  //process food allowance success block
  processSucccessFoodAllowance(res, expenseDetail) {
    if (res && res.result == ResponseResult.Success) {
      expenseDetail.actualAmount = res.actualAmount;
    }
    else {
      expenseDetail.actualAmount = 0;
    }
    this.calculateAmount(expenseDetail);
    this.expenseClaimMasterData.pageLoading = false;
  }

  validateDecimal(item, type): void {
    let numArr: Array<string>;
    if (item[type]) {
      var decimalValue = item[type];
      if (!this.validateNegativeValue(decimalValue)) {
        numArr = decimalValue.toString().split('.');
        var manday = this.getDecimalValuewithTwoDigits(numArr);
        setTimeout(() => {
          if (manday)
            item[type] = manday;
        }, 10);
      }
      else {
        setTimeout(() => {
          item[type] = 0;
        }, 10);
      }
    }
  }

  validateNegativeValue(value) {
    if (parseInt(value) < 0)
      return true;
    return false;
  }

  getDecimalValuewithTwoDigits(numArr) {
    if (numArr.length > 1) {
      var afterDot = numArr[1];
      if (afterDot.length > 2) {
        return Number(numArr[0] + "." + afterDot.substring(0, 2)).toFixed(2);
      }
    }
  }

  changeTripType(event, expense) {
    const previousTripType = expense.tripMode;
    const newTripType = event;
    if (previousTripType == TripType.DoubleTrip) {
      expense.actualAmount = expense.actualAmount / 2
    }
    else if (newTripType == TripType.DoubleTrip) {
      expense.actualAmount = expense.actualAmount * 2
    }
    expense.tripMode = newTripType;
    this.calculateAmount(expense);
  }

  //fetch city dropdown list
  getCityListBySearch(cityType, expenseClaims: ExpenseClaimDetails) {
    if (cityType == this._cityType.StartCity) {
      this.startCityInput.pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(),
        switchMap(term => term
          ? this.locationService.getCityListBySearch(this.cityRequest, term)
          : this.locationService.getCityListBySearch(this.cityRequest)
            .pipe(
              catchError(() => of([])), // empty list on error
              tap())
        ))
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(response => {
          if (response && response.result == ResponseResult.Success && response.data.length > 0) {
            this.startCityList = response.data;
          }
        });
    }
    else if (cityType == this._cityType.DestCity) {
      this.destCityInput.pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(),
        switchMap(term => term
          ? this.locationService.getCityListBySearch(this.cityRequest, term)
          : this.locationService.getCityListBySearch(this.cityRequest)
            .pipe(
              catchError(() => of([])), // empty list on error
              tap())
        ))
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(response => {
          if (response && response.result == ResponseResult.Success && response.data.length > 0) {
            this.destCityList = response.data;
          }
        });
    }
  }

  //fetch the city data with virtual scroll
  getCityData(cityType, expenseClaims: ExpenseClaimDetails) {
    if (cityType == this._cityType.StartCity) {
      this.cityRequest.searchText = this.startCityInput.getValue();
      this.cityRequest.skip = this.startCityList.length;
      expenseClaims.startCityLoading = true;
      this.locationService.getCityListBySearch(this.cityRequest)
        .pipe(takeUntil(this.componentDestroyed$)).
        subscribe(response => {
          if (response && response.result == ResponseResult.Success && response.data.length > 0) {
            this.startCityList = this.startCityList.concat(response.data);
          }
          this.cityRequest = new CityDataSourceRequest();
          expenseClaims.startCityLoading = false;
        }),
        error => {
          expenseClaims.startCityLoading = false;
          this.setError(error);
        };
    }
    else if (cityType == this._cityType.DestCity) {
      this.cityRequest.searchText = this.destCityInput.getValue();
      this.cityRequest.skip = this.destCityList.length;
      expenseClaims.destCityLoading = true;
      this.locationService.getCityListBySearch(this.cityRequest)
        .pipe(takeUntil(this.componentDestroyed$)).
        subscribe(response => {
          if (response && response.result == ResponseResult.Success && response.data.length > 0) {
            this.destCityList = this.destCityList.concat(response.data);
          }
          this.cityRequest = new CityDataSourceRequest();
          expenseClaims.destCityLoading = false;
        }),
        error => {
          expenseClaims.destCityLoading = false;
          this.setError(error);
        };
    }
  }

  clearCity(cityType, expenseClaims: ExpenseClaimDetails) {
    this.getCityListBySearch(cityType, expenseClaims);
  }
}

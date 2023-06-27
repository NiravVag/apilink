import { Component } from '@angular/core';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { ActivatedRoute, Router, ParamMap, NavigationStart } from "@angular/router";
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { BookingService } from '../../../_Services/booking/booking.service';
import { NgbDate, NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserType, BookingStatus, BookingSearchRedirectPage, cancelrescheduletime, RoleEnum, RescheduleReason } from '../../common/static-data-common';
import { BookingCancelModel, BookingDetail } from '../../../_Models/booking/bookingcancelmodel';
import { UserModel } from '../../../_Models/user/user.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-cancel-booking',
  templateUrl: './cancel-booking.component.html',
  styleUrls: ['./cancel-booking.component.scss']
})
export class CancelBookingComponent extends DetailComponent {

  public model: any;
  public bookingItem: BookingDetail;
  public currencyLst: Array<any> = [];
  public reasonLst: Array<any> = [];
  public data: any;
  Initialloading = false;
  private jsonHelper: JsonHelper;
  public IsCancel: boolean = false;
  public IsReSchedule: boolean;
  _pagetype: any;
  public _cancelRescheduleTimeType = cancelrescheduletime;
  leaddays: NgbDate;
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  public _saveloading: boolean = false;
  public _IscurrencyRequired: boolean = false;
  _bookingStatus = BookingStatus;
  _status: any;
  _HolidayDates = [];
  _leadtime: number;
  BookingRuleDesc: string;
  customerId: number;
  factoryId: number;
  _roleEnum = RoleEnum;
  isQCCheckboxShow: boolean;
  msgTitle: string;
  modelRef: NgbModalRef;
  popUpMsg: string;
  isModalOpen: boolean = false;
  firstServiceDateDisabled = false;
  isMultiBookingQuotation = false;
  multiBookingQuotErrorMsg = " included in multi-booking quotation. Please cancel it.";
  isCancelKeepQC: boolean = false;
  isRescheduleBooking: boolean = false;

  onInit(id?: any, inputparam?: ParamMap): void {
    if (inputparam && inputparam.has("type")) {
      if (Number(inputparam.get("type")) == BookingSearchRedirectPage.Cancel) {
        this.IsCancel = true;
        this._pagetype = BookingSearchRedirectPage.Cancel;
        this._status = BookingStatus.Cancel;
        this.msgTitle = 'INSPECTION_CANCEL.TITLE_CANCEL';
      }
      else if (Number(inputparam.get("type")) == BookingSearchRedirectPage.Reschedule) {
        this.IsReSchedule = true;
        this.isQCCheckboxShow = this.currentUser.roles.
          filter(x => x.id == this._roleEnum.InspectionScheduled).length > 0;
        this._pagetype = BookingSearchRedirectPage.Reschedule;
        this._status = BookingStatus.Rescheduled;
        this.msgTitle = 'INSPECTION_CANCEL.TITLE_RESCHEDULE'
      }
    }
    if (id)
      this.Intialize(id);
  }
  getViewPath(): string {
    throw new Error("Method not implemented.");
  }
  getEditPath(): string {
    return "inspcancel/cancel-booking";
  }

  constructor(
    toastr: ToastrService,
    private authService: AuthenticationService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService,
    public service: BookingService,
    public calendar: NgbCalendar,
    public dateparser: NgbDateParserFormatter,  
    public modalService: NgbModal,
    public utility: UtilityService) {
    super(router, route, translate, toastr);
    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("booking/cancel-booking.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.currentUser = this.authService.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    // router.events.subscribe(data => {
    //   if (data instanceof NavigationStart) {
    //     let id = route.snapshot.paramMap.get("id");
    //     this.onInit(id);
    //   }
    // });
  }

  Intialize(id) {
    this.Initialloading = true;
    this.validator.isSubmitted = false;
    this.firstServiceDateDisabled = false;
    this.data = [];

    this.model = new BookingCancelModel();

    this.model.bookingId = id;

    this.GetCancelBookingDetails(id);

    this.GetReasonType(id);

    this.GetGetCurrencyList();

  }

  GetCancelBookingDetails(id: any) {
    this.bookingItem = new BookingDetail();
    this.service.GetCancelBookingDetails(id, this._status)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            if (response.itemDetails) {
              var x = response.itemDetails;
              this.bookingItem = {
                bookingId: id,
                customerName: x.customerName,
                supplierName: x.supplierName,
                factoryName: x.factoryName,
                serviceType: x.serviceType,
                serviceDateFrom: x.serviceDateFrom,
                serviceDateTo: x.serviceDateTo,
                productCategory: x.productCategory,
                statusId: x.statusId
              }

              var isKeepQC = true;
              this.isCancelKeepQC = this.bookingItem.statusId == BookingStatus.AllocateQC ? isKeepQC : !isKeepQC;
              this.isRescheduleBooking = response.itemDetails.isRescheduleBooking;
              this.isMultiBookingQuotation = response.itemDetails.isMultiBookingQuotation;
              this.model.firstServiceDateFrom = response.itemDetails.firstServiceDateFrom,
                this.model.firstServiceDateTo = response.itemDetails.firstServiceDateTo
              this.customerId = x.customerId;
              this.factoryId = x.factoryId;
              if (response.cancelItem) {
                this.model = this.mapDetail(response.cancelItem);
              }
              else if (response.rescheduleItem) {
                this.model = this.mapDetail(response.rescheduleItem);
                if (this.model.reasonTypeId == RescheduleReason.RequestedByCustomer || this.model.reasonTypeId == RescheduleReason.RequestedBySupplier || this.model.reasonTypeId == RescheduleReason.RequestedByFactory)
                  this.firstServiceDateDisabled = true;
              }
              if (this.IsReSchedule) {
                this.GetInspBookingRuleDetails(this.customerId, this.factoryId);
              }
            }
          }
          this.Initialloading = false;
        },
        error => {
          this.setError(error);
          this.Initialloading = false;
        }
      )
  }

  GetGetCurrencyList() {
    this.service.GetCurrency()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.currencyLst = response.currencyList;
          }
          this.Initialloading = false;
        },
        error => {
          this.setError(error);
          this.Initialloading = false;
        }
      )
  }

  GetReasonType(bookingId: any) {
    this.service.GetReason(bookingId, this._pagetype)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.reasonLst = response.responseList;
          }
          this.Initialloading = false;
        },
        error => {
          this.setError(error);
          this.Initialloading = false;
        }
      )
  }

  GetInspBookingRuleDetails(customerId, factoryId) {
    this.service.GetInspBookingRules(customerId, factoryId)
      .subscribe(response => {
        if (response && response.result == 1) {
          this._HolidayDates = response.bookingRuleList.holidays;
          this._leadtime = response.bookingRuleList.leadDays;
          this.BookingRuleDesc = response.bookingRuleList.bookingRule;
          this.GetLeadDays();
        }
        else {
          this.showError('INSPECTION_CANCEL.TITLE_CANCEL', 'EDIT_BOOKING.MSG_CANNOT_GET_BOOKINGRULE');
        }
      }, error => {
        this.setError(error);
      }
      );
  }

  GetLeadDays() {
    if (this._HolidayDates != null && this._HolidayDates.length > 0) {
      var count = 0;
      var leadtime: NgbDate = this.calendar.getNext(this.calendar.getToday(), 'd', this._leadtime);
      for (var i = this.calendar.getToday(); !i.after(leadtime); i = this.calendar.getNext(i, 'd', 1)) {
        if (this._HolidayDates.filter(x => i.equals(x)).length > 0)
          count++;
      }
      return this.leaddays = this.calendar.getNext(this.calendar.getToday(), 'd', this._leadtime + count);
    }
    else {
      return this.leaddays = this.calendar.getNext(this.calendar.getToday(), 'd', this._leadtime);
    }
  }

  isDisabled = (date: NgbDate, current: { month: number }) => {
    if (!this._IsInternalUser) {
      return date.before(this.calendar.getToday()) || (this._HolidayDates != null && this._HolidayDates.filter(x => date.equals(x)).length > 0)
        || this.leaddays && (date.before(this.leaddays) || date.equals(this.leaddays))
    };
  }

  onDateSelection(date: NgbDate) {
    if (this.bookingItem.serviceDateTo == null && this.bookingItem.serviceDateFrom != null) {
      this.bookingItem.serviceDateTo = this.bookingItem.serviceDateFrom;
    }

    var toDate = this.bookingItem.serviceDateTo ? this.dateparser.parse(this.bookingItem.serviceDateTo) : null;
    var toDateNgb = this.bookingItem.serviceDateTo != null ? new NgbDate(toDate.year, toDate.month, toDate.day) : null;

    var fromDate = this.bookingItem.serviceDateFrom ? this.dateparser.parse(this.bookingItem.serviceDateFrom) : null;
    var fromDateNgb = this.bookingItem.serviceDateFrom != null ? new NgbDate(fromDate.year, fromDate.month, fromDate.day) : null;

    if (toDateNgb.before(fromDateNgb)) {
      this.bookingItem.serviceDateTo = null;
    }

    this.reasonChangeEvent();

  }

  Reset() {
    this.Intialize(this.bookingItem.bookingId);
  }

  IsFormValid() {
    if (this.IsCancel)
      return this.validator.isValid('reasonTypeId') &&
        this.validator.isValidIf('currencyId', this.IscurrencyRequired())
    else if (this.IsReSchedule) {
      var isOk = this.validator.isValid('reasonTypeId') &&
        this.validator.isValid('serviceFromDate') &&
        this.validator.isValid('serviceToDate') &&
        this.validator.isValid('firstServiceDateFrom') &&
        this.validator.isValid('firstServiceDateTo') &&
        this.validator.isValidIf('currencyId', this.IscurrencyRequired());

      //if (isOk && this.model.isKeepAllocatedQC) {
      //  return this.toDateValidate();
      //}
      //else {
      //  return isOk;
      //}
      return isOk;
    }
  }

  //validate the todate should extends the booking to date
  //toDateValidate(): boolean {
  //  var isok: boolean = true;

  // var toDate = this.dateparser.parse(this.bookingItem.serviceDateTo);
  // var toDateNgb = new NgbDate(toDate.year, toDate.month, toDate.day);
  // var requestTodateNgb = new NgbDate(this.model.serviceToDate.year, this.model.serviceToDate.month, this.model.serviceToDate.day);

  // if (requestTodateNgb.before(toDateNgb)||requestTodateNgb.equals(toDateNgb)) {
  //   this.showWarning(this.msgTitle, 'INSPECTION_CANCEL.LBL_TO_DATE_GREATER_THAN_BOOKING_DATE');
  //   isok = false;
  // }
  // else {
  //   isok = true;
  // }
  //  return isok;
  //}

  IscurrencyRequired() {
    if (this.model.travelExpense != null && this.model.travelExpense != 0 && !isNaN(Number(this.model.travelExpense)))
      this._IscurrencyRequired = true;
    else
      this._IscurrencyRequired = false;
    return this._IscurrencyRequired;
  }

  Save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.IsFormValid()) {
      if (this.isMultiBookingQuotation && !this.isRescheduleBooking) {
        if (this._IsInternalUser) {
          this.showError(this.msgTitle, this.bookingItem.bookingId + this.multiBookingQuotErrorMsg);
        }

        else {
          this.showError(this.msgTitle, 'BOOKING_SUMMARY.MSG_CONTACT_API')
        }
      }
      else {
        //close the pop up if open
        if (this.isModalOpen) {
          this.modelRef.close();
        }

        //if (this.IsFormValid()) {
        this._saveloading = true;
        let msg = this.IsCancel ? 'INSPECTION_CANCEL.MSG_SAVE_SUCCESS' : 'INSPECTION_CANCEL.MSG_SAVE_SUCCESS_RESCHEDULE';
        this.service.SaveCancelBooking(this.model, this._pagetype)
          .subscribe(
            response => {
              if (response && response.result == 1) {
                this.showSuccess(this.msgTitle, msg);
                if (this.fromSummary)
                  this.return("inspsummary/booking-summary");
              }
              else {
                switch (response.result) {
                  case 2:
                    this.showError(this.msgTitle, 'INSPECTION_CANCEL.MSG_NOT_SAVED');
                    break;
                  case 3:
                    this.showError(this.msgTitle, 'INSPECTION_CANCEL.MSG_REQUEST_NOT_CORRECT_FORMAT');
                    break;
                  case 4:
                    this.showError(this.msgTitle, 'INSPECTION_CANCEL.MSG_NO_ITEM_FOUND');
                    break;
                  case 6:
                    this.showWarning(this.msgTitle, 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
                    break;
                  case 7:
                    this.ShowWarningMsgQuotationExists(this.msgTitle);
                    break;
                  case 8:
                    this.showWarning(this.msgTitle, 'EDIT_BOOKING.MSG_QUOTATION_CANCEL');
                    break;
                  case 9:
                    this.showWarning(this.msgTitle, 'INSPECTION_CANCEL.MSG_REMOVE_MULTI_BOOKING_QUOTATION');
                    break;
                }
              }
              this._saveloading = false;
            },
            error => {
              this.showError(this.msgTitle, 'INSPECTION_CANCEL.MSG_UNKNOWN_ERROR');
              this._saveloading = false;
            }
          )
        //}
      }
    }
  }

  ShowWarningMsgQuotationExists(title) {
    if (this._IsInternalUser)
      this.showError(title, 'INSPECTION_CANCEL.MSG_INTERNAL_USER_QUOTATIONCANCEL');
    else
      this.showError(title, 'INSPECTION_CANCEL.MSG_EXTERNAL_USER_QUOTATIONCANCEL');
  }

  IsCancelVisible() {
    if (this._IsInternalUser)
      return true;
    else if (this.data.statusId == this._bookingStatus.Cancel)
      return false;
    else
      return true;
  }

  mapDetail(detail: any): BookingCancelModel {
    var model: BookingCancelModel = {
      bookingId: detail.bookingId,
      comment: detail.comment,
      currencyId: detail.currencyId,
      isEmailToAccounting: detail.isEmailToAccounting,
      reasonTypeId: detail.reasonTypeId,
      timeTypeId: detail.timeTypeId,
      travelExpense: detail.travelExpense,
      internalComment: detail.internalComment,
      isKeepAllocatedQC: false,
      isDisableServiceFromDate: false,
      isCancelKeepAllocatedQC: false,
      firstServiceDateFrom: detail.firstServiceDateFrom,
      firstServiceDateTo: detail.firstServiceDateTo
    };

    if (this._pagetype == BookingSearchRedirectPage.Reschedule) {
      model.serviceFromDate = detail.serviceFromDate;
      model.serviceToDate = detail.serviceToDate;
      model.firstServiceDateFrom = detail.firstServiceDateFrom;
      model.firstServiceDateTo = detail.firstServiceDateTo;
    }
    return model;
  }

  //check box change event
  isKeepAllocatedQCCheckboxChange(isChecked) {

    if (isChecked) {
      var date = this.dateparser.parse(this.bookingItem.serviceDateFrom);
      var fromDate = new NgbDate(date.year, date.month, date.day);
      this.model.isDisableServiceFromDate = true;

      //if service date not assign or booking date and reschedule date not same
      if (!this.model.serviceFromDate || (!fromDate.equals(this.model.serviceFromDate))) {
        this.model.serviceFromDate = fromDate;
        this.showWarning(this.msgTitle, 'INSPECTION_CANCEL.LBL_FROM_DATE_BE_SAME_RESCHEDULE_FROM_DATE');
      }
    }
    else
      this.model.isDisableServiceFromDate = false;
  }

  //confirm pop up to show that the QC will be deleted  (only if selected serviceDateTo is less than the actual serviceDateTo)
  openPopupLesserServiceDateToSelect(content) {

    //convert the Todate to Ngb format
    var toDate = this.bookingItem.serviceDateTo ? this.dateparser.parse(this.bookingItem.serviceDateTo) : null;
    var toDateNgb = this.bookingItem.serviceDateTo != null ? new NgbDate(toDate.year, toDate.month, toDate.day) : null;

    //convert the Fromdate to Ngb format
    var fromDate = this.bookingItem.serviceDateFrom ? this.dateparser.parse(this.bookingItem.serviceDateFrom) : null;
    var fromDateNgb = this.bookingItem.serviceDateFrom != null ? new NgbDate(fromDate.year, fromDate.month, fromDate.day) : null;

    if (this.model.serviceToDate) {
      //convert the reschedule to date to Ngb format
      var requestTodateNgb = new NgbDate(this.model.serviceToDate.year, this.model.serviceToDate.month, this.model.serviceToDate.day);

      //check if the selected serviceDateTo is less than the actual serviceDateTo
      if (requestTodateNgb.before(toDateNgb) && this.model.isKeepAllocatedQC) {

        //get the dates in the date range - service dates
        var getOldDateArray = this.utility.getDatesBetweenDateRange(fromDateNgb, toDateNgb);

        //get the dates in the date range - rescheduled dates
        var getNewDateArray = this.utility.getDatesBetweenDateRange(this.model.serviceFromDate, this.model.serviceToDate);

        //get the dates that exists in the old range but does not exist in the new range 
        var difference = getOldDateArray.filter(x => getNewDateArray.indexOf(x) === -1);

        //format the date from "Sat 21 Mar 2020 0000" to "21-3-2020"
        var formattedDate = [];
        difference.forEach(element => {
          var date = new Date(element);
          var dateString = date.getDate() + "-" + date.getMonth() + "-" + date.getFullYear();
          formattedDate.push(dateString);
        });

        //set the dynamic message for the confirm pop up
        this.popUpMsg = this.utility.textTranslate('INSPECTION_CANCEL.MSG_QC_REMOVE') + formattedDate.join(', ');
        this.isModalOpen = true;
        this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
        this.modelRef.result.then((result) => {
        }, (reason) => {
        });
      }
      else this.Save();
    }
    else this.Save();
  }

  reasonChangeEvent() {
    if ((this.model.reasonTypeId == RescheduleReason.RequestedByCustomer || this.model.reasonTypeId == RescheduleReason.RequestedBySupplier || this.model.reasonTypeId == RescheduleReason.RequestedByFactory) ||
      (!this._IsInternalUser)) {
      this.firstServiceDateDisabled = true;
      this.model.firstServiceDateFrom = this.model.serviceFromDate;
      this.model.firstServiceDateTo = this.model.serviceToDate;
    }

    else {
      this.firstServiceDateDisabled = false;
    }
  }

}

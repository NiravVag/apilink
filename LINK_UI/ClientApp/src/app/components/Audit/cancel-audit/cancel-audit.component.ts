import { Component, OnInit } from '@angular/core';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { ActivatedRoute, Router, Params, ParamMap } from "@angular/router";
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { AuditService } from '../../../_Services/audit/audit.service'
import { first, retry, auditTime } from 'rxjs/operators';
import { NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { UserType, AuditStatus, AuditSearchRedirectPage, cancelrescheduletime } from '../../common/static-data-common';
import { Auditcancelmodel } from 'src/app/_Models/Audit/auditcancelmodel';
import { UserModel } from 'src/app/_Models/user/user.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-cancel-audit',
  templateUrl: './cancel-audit.component.html',
  styleUrls: ['./cancel-audit.component.css']
})
export class CancelAuditComponent extends DetailComponent {
  public model: Auditcancelmodel;
  public data: any;
  Initialloading = false;
  private jsonHelper: JsonHelper;
  public IsCancel: boolean = false;
  public IsReSchedule: boolean;
  _pagetype: any;
  public _cancelrescheduletimetype = cancelrescheduletime;
  leaddays: NgbDate;
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  public _saveloading: boolean = false;
  public _IscurrencyRequired: boolean = false;
  _auditstatus = AuditStatus;
  onInit(id?: any, inputparam?: ParamMap): void {
    if (inputparam && inputparam.has("type")) {
      if (Number(inputparam.get("type")) == AuditSearchRedirectPage.Cancel) {
        this.IsCancel = true;
        this._pagetype = AuditSearchRedirectPage.Cancel;
      }
      else if (Number(inputparam.get("type")) == AuditSearchRedirectPage.Reschedule) {
        this.IsReSchedule = true;
        this._pagetype = AuditSearchRedirectPage.Reschedule;
      }
    }
    if (id)
      this.Intialize(id);
  }
  getViewPath(): string {
    throw new Error("Method not implemented.");
  }
  getEditPath(): string {
    return "auditcancel/cancel-audit";
  }
  constructor(toastr: ToastrService,
    private authService: AuthenticationService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService,
    public utility: UtilityService,
    public service: AuditService,
    public calendar: NgbCalendar) {
    super(router, route, translate, toastr);
    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("audit/cancel-audit.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.currentUser = this.authService.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
  }

  Intialize(id) {
    this.Initialloading = true;
    this.validator.isSubmitted = false;
    this.data = [];
    this.model = new Auditcancelmodel();
    this.service.GetCancelauditDetails(id, this._pagetype)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.data = response.data;
            if (!this._IsInternalUser)
              this.GetLeadDays();
            if (response.itemDetails) {
              var x = response.itemDetails;
              this.model = {
                auditid: id,
                isemailtoaccounting: false,
                optypeid: x.optypeid,
                servicedatefrom: null,
                servicedateto: null,
                cancelrescheduletimetype: x.cancelrescheduletimetype,
                comment: x.comment,
                currencyId: x.currencyId,
                internalcomment: x.internalcomment,
                reasontypeid: x.reasontypeid,
                travelexpense: x.travelexpense == 0 ? "" : x.travelexpense
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
  GetLeadDays() {
    if (this.data.holidayDates != null && this.data.holidayDates.length > 0) {
      var count = 0;
      var leadtime: NgbDate = this.calendar.getNext(this.calendar.getToday(), 'd', this.data.leadTime);
      for (var i = this.calendar.getToday(); !i.after(leadtime); i = this.calendar.getNext(i, 'd', 1)) {
        if (this.data.holidayDates.filter(x => i.equals(x)).length > 0)
          count++;
      }
      return this.leaddays = this.calendar.getNext(this.calendar.getToday(), 'd', this.data.leadTime + count);
    }
    else {
      return this.leaddays = this.calendar.getNext(this.calendar.getToday(), 'd', this.data.leadTime);
    }
  }
  isDisabled = (date: NgbDate, current: { month: number }) => {
    if (!this._IsInternalUser) {
      return date.before(this.calendar.getToday()) || (this.data.holidayDates != null && this.data.holidayDates.filter(x => date.equals(x)).length > 0)
        || this.leaddays && (date.before(this.leaddays) || date.equals(this.leaddays))
    };
  }
  onDateSelection(date: NgbDate) {
    if (this.model.servicedateto == null && this.model.servicedatefrom != null) {
      this.model.servicedateto = this.model.servicedatefrom;
    }
    if (this.model.servicedateto < this.model.servicedatefrom) {
      this.model.servicedateto = null;
    }

  }
  Reset() {
    this.Intialize(this.model.auditid);
  }
  IsFormValid() {
    if (this.IsCancel)
      return this.validator.isValid('reasontypeid') &&
        this.validator.isValidIf('currencyId', this.IscurrencyRequired())
    else if (this.IsReSchedule)
      return this.validator.isValid('reasontypeid') &&
        this.validator.isValid('servicedatefrom') &&
        this.validator.isValid('servicedateto') &&
        this.validator.isValidIf('currencyId', this.IscurrencyRequired())
  }
  IscurrencyRequired() {
    // if (!this.validator.isSubmitted)
    //   return this._IscurrencyRequired= true;
    if (this.model.travelexpense != null && this.model.travelexpense != 0 && !isNaN(Number(this.model.travelexpense)))
      this._IscurrencyRequired = true;
    else
      this._IscurrencyRequired = false;
    return this._IscurrencyRequired;
  }
  Save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.IsFormValid()) {
      this._saveloading = true;
      let title = this.IsCancel ? 'AUDIT_CANCEL.TITLE_CANCEL' : 'AUDIT_CANCEL.TITLE_RESCHEDULE';
      let msg = this.IsCancel ? 'AUDIT_CANCEL.MSG_SAVE_SUCCESS' : 'AUDIT_CANCEL.MSG_SAVE_SUCCESS_RESCHEDULE';
      this.model.optypeid = this._pagetype;
      this.service.SaveCancelAudit(this.model)
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.showSuccess(title, msg);
              if (this.fromSummary)
                this.return("auditsummary/audit-summary");
            }
            else {
              switch (response.result) {
                case 2:
                  this.showError(title, 'AUDIT_CANCEL.MSG_NOT_SAVED');
                  break;
                case 3:
                  this.showError(title, 'AUDIT_CANCEL.MSG_REQUEST_NOT_CORRECT_FORMAT');
                  break;
                case 4:
                  this.showError(title, 'AUDIT_CANCEL.MSG_NO_ITEM_FOUND');
                  break;
              }
            }
            this._saveloading = false;
          },
          error => {
            this.showError(title, 'AUDIT_CANCEL.MSG_UNKNOWN_ERROR');
            this._saveloading = false;
          }
        )
    }
  }
  IsCancelVisible() {
    if (this._IsInternalUser)
      return true;
    else if (this.data.statusId == this._auditstatus.Cancel)
      return false;
    else
      return true;
  }
}

import { Component, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from "@angular/router";
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { LeaveRequestModel, LeaveRequestResult, SaveLeaveRequestResult } from '../../../_Models/hr/leave-request.model';
import { HrService } from '../../../_Services/hr/hr.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { HeaderService } from '../../../_Services/header/header.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-leaveRequest',
  templateUrl: './leave-request.component.html',
  styleUrls: ['./leave-request.component.css']
})
export class LeaveRequestComponent extends DetailComponent {
  public model: LeaveRequestModel;
  public data: any;
  public canEdit: boolean = false;
  public canApprove: boolean = false;
  public canCancel: boolean = false;

  public items: Array<any> = [];
  private id: any;
  private modelRef: NgbModalRef;
  private rejectComment: string;
  public isRejectValid: boolean;
  public _saveloader: boolean = false;
  public _rejectloader: boolean = false;
  public _approveloader: boolean = false;
  public _cancelloader: boolean = false;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public utility: UtilityService,
    private service: HrService,
    private cd: ChangeDetectorRef, private modalService: NgbModal, private activeRoute: ActivatedRoute, private headerService: HeaderService) {
    super(router, route, translate, toastr);

    this.validator.setJSON("hr/leave-request.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model = new LeaveRequestModel();
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

  getViewPath(): string {
    return "leaverequest/leave-request";
  }

  getEditPath(): string {
    return "leaverequest/leave-request";
  }

  public getAddPath(): string {
    return "leaverequest/leave-request";
  }


  init(id?) {
    this.loading = true;
    this.data = {};
    this.validator.isSubmitted = false;
    this.id = id;

    //this.waitingService.open();
    this.service.getLeaveRequest(id)
      .pipe()
      .subscribe(
        res => {
          let result: LeaveRequestResult = res.result;

          if (res && res.result == LeaveRequestResult.Success) {
            this.canEdit = res.canEdit;
            this.canApprove = res.canApprove;
            this.canCancel=res.canCancel;
            this.data = res;
            if (id == null)
              this.model = new LeaveRequestModel();
            else
              this.model = res.leaveRequest;
          }
          else {
            this.canEdit = false;
            this.canApprove = false;

            switch (result) {
              case LeaveRequestResult.CannotFindLeaveRequest:
                this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_LEAVEREQUEST_NOUTFOUND');
                break;
              case LeaveRequestResult.CannotFindTypes:
                this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_LEAVETYPES_NOUTFOUND');
                break;
              case LeaveRequestResult.CannotFinddayTypeList:
                this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_LEAVEDAYTYPES_NOUTFOUND');
                break;

            }
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getDays(value: any, sender: any): number {

    let startDate: any = this.model.startDate;
    let endDate: any = this.model.endDate;
    let startDayType: number = this.model.startDayType;
    let endDayType: number = this.model.endDayType;

    switch (sender) {
      case "startDate":
        startDate = value;
        break;
      case "endDate":
        endDate = value;
        break;
      case "startDayType":
        startDayType = value;
        break;
      case "endDayType":
        endDayType = value;
        break;
    }


    if (startDate !== null && endDate != null) {

      this.service.getNumberDays(startDate, endDate, startDayType, endDayType)
        .pipe()
        .subscribe(
          res => {
            this.loading = false;
            if (res && res.result == 1) {
              this.model.leaveDays = res.days;
              return res.days;
            }
            else {
              const fromDate: Date = this.createDateFromNgbDate(this.model.startDate);
              const toDate: Date = this.createDateFromNgbDate(this.model.endDate);
              const daysDiff = Math.floor(Math.abs(<any>fromDate - <any>toDate) / (1000 * 60 * 60 * 24)) + 1;
              this.model.leaveDays = daysDiff;
              return daysDiff;
            }
          },
          error => {
            const fromDate: Date = this.createDateFromNgbDate(this.model.startDate);
            const toDate: Date = this.createDateFromNgbDate(this.model.endDate);
            const daysDiff = Math.floor(Math.abs(<any>fromDate - <any>toDate) / (1000 * 60 * 60 * 24)) + 1;
            this.model.leaveDays = daysDiff;
            return daysDiff;
            //this.loading = false;
          });


    }

    return null;
  }

  private createDateFromNgbDate(ngbDate: any): Date {
    const date: Date = new Date(Date.UTC(ngbDate.year, ngbDate.month - 1, ngbDate.day));
    return date;
  }

  save() {

    this.validator.isSubmitted = true;

    if (this.validator.isFormValid()) {
      this._saveloader = true;
      this.service.saveLeaveRequest(this.model)
        .subscribe(
          res => {
            let result: SaveLeaveRequestResult = res.result;

            if (res && result == SaveLeaveRequestResult.Success) {

              this.showSuccess('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_SAVE_SUCCESS');

              if (this.fromSummary)
                this.return("leavesearch/leave-summary");
              else
                this.init();
            }
            else {
              switch (result) {
                case SaveLeaveRequestResult.StartDateIsRequired:
                  this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_STARTDATE_REQ');
                  break;
                case SaveLeaveRequestResult.EndDateIsRequired:
                  this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_ENDDATE_REQ');
                  break;
                case SaveLeaveRequestResult.LeaveTypeIsRequired:
                  this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_LEAVETYPEID_REQ');
                  break;
                case SaveLeaveRequestResult.CannotSave:
                  this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_CANNOTSAVE_REQ');
                  break;
                case SaveLeaveRequestResult.UnAuthorized:
                  this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'LEAVE_REQUEST.MSG_UNAUTHORIZED');
                  break;
                case SaveLeaveRequestResult.StaffEntityNotMatched:
                  this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'LEAVE_REQUEST.MSG_LEAVE_REQUEST_CANNOT_MADE_OTHER_COMPANY');
                  break;
              }
            }
            this._saveloader = false;
          },
          error => {
            this._saveloader = false;
            if (error == "Unauthorized")
              this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'ERROR.MSG_MESSAGE_401');
            else
              this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'ERROR.MSG_UNKNONW_ERROR');
          });
    }
  }

  approve() {
    this._approveloader = true;
    this.service.setStatus(this.id, 3)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.init(this.id);
          }
          else {
            this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
          this._approveloader = false;
        },
        error => {
          this._approveloader = false;
          this.setError(error);
        });
  }

  cancel() {
    this._cancelloader = true;
    this.service.setStatus(this.id, 5)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.init(this.id);
          }
          else {
            this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
          this._cancelloader = false;
        },
        error => {
          this._cancelloader = false;
          this.setError(error);
        });
  }

  openConfirm(content) {
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true });

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
    this._rejectloader = true;
    this.service.reject(this.id, this.rejectComment)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.init(this.id);
          }
          else {
            this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
          this._rejectloader = false;
        },
        error => {
          this._rejectloader = false;
          this.setError(error);
        });

    this.modelRef.close();
    this.rejectComment = "";

  }
}




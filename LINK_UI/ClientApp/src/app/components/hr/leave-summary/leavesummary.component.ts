import { Component, NgModule, OnChanges, SimpleChanges, OnInit } from '@angular/core';
import { HrService } from '../../../_Services/hr/hr.service'
import { first } from 'rxjs/operators';
import { NgbModal, NgbModalRef, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { Validator } from '../../common/validator'
import { Router, ActivatedRoute } from '@angular/router';
import { SummaryComponent } from '../../common/summary.component';
import { LeaveSummaryModel, LeaveSummaryItemModel, LeaveStatus, StatusColor, leavestatusColors } from '../../../_Models/hr/leave-summary.model';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { HeaderService } from '../../../_Services/header/header.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-leaveSummary',
  templateUrl: './leavesummary.component.html',
  styleUrls: ['./leavesummary.component.css'],
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

export class LeaveSummaryComponent extends SummaryComponent<LeaveSummaryModel> {

  public data: any;
  error = '';
  public exportDataLoading = false;
  public model: LeaveSummaryModel;
  private modelRef: NgbModalRef;
  public isStaffDetails: boolean = false;
  idCurrentStaff: number;
  public items: Array<LeaveSummaryItemModel>;
  public statusList: Array<LeaveStatus>;
  public statusColors: Array<StatusColor> = leavestatusColors;
  public canCheck: boolean = false;
  private rejectId: number;
  private rejectComment: string;
  private isRejectValid: boolean;
  private _router: Router;
  public _searchloader: boolean = false;
  isFilterOpen: boolean;
  constructor(private service: HrService, private modalService: NgbModal,
    public validator: Validator, router: Router, route: ActivatedRoute,
    public utility: UtilityService,
    private calendar: NgbCalendar, private headerService: HeaderService,translate: TranslateService,
    toastr: ToastrService) {
    super(router, validator, route,translate,toastr);
    this._router = router;
    this.model = new LeaveSummaryModel();
    this.validator.setJSON("hr/leave-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.isFilterOpen = true;
    this.data = {};
  }

  onInit() {
    this.model.isApproveSummary = this._router.url.indexOf("leave-approve") >= 0;
    this.loading = true;

    this.service.getLeaveSummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.loading = false;

            if (this.model.isApproveSummary) {
              this.model.statusValues = this.data.leaveStatusList.filter(x => x.id == 1);
              this.model.startDate = this.calendar.getPrev(this.calendar.getToday(), 'm', 6);
              this.model.endDate = this.calendar.getNext(this.calendar.getToday(), 'm', 6);

              this.search();
            }
          }
          else {
            this.error = data.result;
            this.loading = false;
          }
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getPathDetails() { return "leaverequest/leave-request" };

  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  getData() {
    this.canCheck = false;
    this.model.noFound = false;
    this.items = [];

    this._searchloader = true;
    this.service.getLeaveDataSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {
            this.canCheck = response.canCheck;
            this.mapPageProperties(response);
            this.statusList =this.model.isApproveSummary && response.leaveStatusList?response.leaveStatusList.filter(x => x.id == 1): response.leaveStatusList;
            this.model.totalCount = response.totalCount;
            this.model.index = response.index;
            this.model.pageSize = response.pageSize;

            this.items = response.data;

          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
          this._searchloader = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
          this._searchloader = false;
        });
  }


  return(isDetails) {
    this.isStaffDetails = isDetails;
  }



  export() {
    this.exportDataLoading = true;
    this.service.exportLeaveSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
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
    this.exportDataLoading = false;
  }

  setStatus(id: number, idstatus: number) {
    this.service.setStatus(id, idstatus)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.refresh();
          }
          else {
            this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
        },
        error => {
          this.setError(error);
        });
  }

  openConfirm(content, id: number) {
    this.rejectId = id;
    this.modelRef = this.modalService.open(content, { windowClass : "mdModelWidth", centered: true });

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

    this.service.reject(this.rejectId, this.rejectComment)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.refresh();
          }
          else {
            this.showError('LEAVE_REQUEST.LEAVEREQUEST_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
        },
        error => {
          this.setError(error);
        });

    this.modelRef.close();
    this.rejectComment = "";

  }

  searchByStatus(status: LeaveStatus) {
    this.model.statusValues = [status];
    this.search();
  }
  searchcolor(id) {
    var item = this.statusColors.filter(x => x.id == id);

    if (item && item.length > 0)
      return item[0].color;

    return "";
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "StartDate": { 
        this.model.startDate=null;
        break; 
     } 
     case "EndDate": { 
      this.model.endDate=null;
        break; 
     } 
    }
  }
}

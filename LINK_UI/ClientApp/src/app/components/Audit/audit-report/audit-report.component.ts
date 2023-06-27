import { Component, OnInit } from '@angular/core';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { ActivatedRoute, Router, Params, ParamMap } from "@angular/router";
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { AuditService } from '../../../_Services/audit/audit.service'
import { first, retry, auditTime } from 'rxjs/operators';
import { NgbDate, NgbCalendar, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserType, AuditStatus, AuditSearchRedirectPage, cancelrescheduletime, FileContainerList } from '../../common/static-data-common';
import { Auditreportmodel, AttachmentFile } from 'src/app/_Models/Audit/auditreportmodel';
import { UserModel } from 'src/app/_Models/user/user.model';
import { FileInfo } from 'src/app/_Models/fileupload/fileupload';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-audit-report',
  templateUrl: './audit-report.component.html',
  styleUrls: ['./audit-report.component.css']
})
export class AuditReportComponent extends DetailComponent {

  public model: Auditreportmodel;
  public data: any;
  Initialloading = false;
  Auditorloading=false;
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  fileSize: number;
  uploadLimit: number;
  uploadFileExtensions: string;
  _auditors: any[];
  savedataloading: boolean = false;
  downloadloading: boolean = false;
  onInit(id?: any, inputparam?: ParamMap): void {
    this.data = {};
    this.GetAuditDetails(id);
  }
  getViewPath(): string {
    throw new Error("Method not implemented.");
  }
  getEditPath(): string {
    return "auditreport/audit-report";
  }

  constructor(toastr: ToastrService,
    private authService: AuthenticationService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService,
    public service: AuditService,
    public calendar: NgbCalendar,
    public utility: UtilityService,
    public modalService: NgbModal, public fileStoreService: FileStoreService) {
    super(router, route, translate, toastr, modalService);
    this.validator.setJSON("audit/audit-report.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.currentUser = this.authService.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this.uploadLimit = 1;
    this.fileSize = 50457280;
    this.uploadFileExtensions = 'xlsx,pdf,doc,docx,xls,zip,rar,jpeg,png';
    this.validator.isSubmitted = false;
  }
  GetAuditDetails(id?) {
    this.Initialloading=true;
    this.GetAuditors(id);
    this.model = new Auditreportmodel();
    this.model.auidtid = id;
    this.model.attachments = [];
    this.GetAuditReportDetails(id);
    this.service.GetAuditDetailsForReport(id).
      subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res.data;
          }
          this.Initialloading=false;
        },
        error=>{
          this.Initialloading=false;
          this.setError(error);
        }
      );
  }
  GetAuditReportDetails(id?) {
    this.Auditorloading=true;
    this.service.GetAuditReportDetails(id).
      subscribe(
        res => {
          if (res && res.result == 1) {
            this.model = {
              attachments: res.data!=null ? res.data.attachments:[],
              auditors: res.data!=null ? res.data.auditors:null,
              auidtid: res.data!=null ? res.data.auidtid:id!=null?id: null,
              comment: res.data!=null ? res.data.comment:null,
              servicedatefrom: res.data!=null ? res.data.servicedatefrom:null,
              servicedateto: res.data!=null ? res.data.servicedateto:null
            };
          }
          this.Auditorloading=false;
        },
        error=>{
          this.Auditorloading=false;
          this.setError(error);
        }
      );
  }
  selectFiles(event) {
    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: this.fileSize,
      uploadFileExtensions: this.uploadFileExtensions,
      uploadLimit: 10,
      containerId: FileContainerList.Audit,
      token: "",
      fileDescription:null
    }

    modalRef.componentInstance.fromParent = fileInfo;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {
          if (this.model.attachments) {
            this.model.attachments.push(element);
          }
        });
      }

    }, (reason) => {

    });

    // if (event && !event.error && event.files) {

    //   // if (event.files.length == null)
    //   this.model.attachments = [];

    //   for (let file of event.files) {
    //     let fileItem: AttachmentFile = {
    //       fileName: file.name,
    //       file: file,
    //       isNew: true,
    //       id: 0,
    //       mimeType: "",
    //       uniqueld: this.newGuid()
    //     };
    //     this.model.attachments.push(fileItem);
    //   }
    //   //event.srcElement.value = null;
    // }
    // else if (event && event.error && event.errorMessage) {
    //   this.showError('AUDIT_REPORT.TITLE', event.errorMessage);
    // }
  }
  removeAttachment(index) {
    this.model.attachments.splice(index, 1);
  }
  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
  IsFormValid() {
    return this.validator.isValid('servicedatefrom') &&
      this.validator.isValid('servicedateto') &&
      this.validator.isValid('auditors');
  }
  Save() {
    try {
      this.validator.initTost();
      this.validator.isSubmitted = true;

      if (this.IsFormValid()) {
        this.savedataloading = true;
        var attachedList = this.model.attachments!=null? this.model.attachments.filter(x => x.isNew):null;
        this.service.saveAuditReport(this.model)
          .subscribe(
            res => {
              if (res && res.result == 1) {
                // if (attachedList && attachedList.length > 0) {
                //   this.service.uploadReportFiles(this.model.auidtid, attachedList).subscribe(Isupload => {
                //     if (Isupload) {
                //       this.showSuccess("AUDIT_REPORT.TITLE", "AUDIT_REPORT.MSG_SAVE_SUCCESS");
                //       if (this.fromSummary)
                //         this.return("auditsummary/audit-summary");
                //     }
                //     else {
                //       this.showWarning("AUDIT_REPORT.TITLE", "AUDIT_REPORT.MSG_SAVE_SUCCESS_FILE_NOT_UPLOAD");
                //     }
                //     this.savedataloading = false;
                //   },
                //     error => { this.savedataloading = false; this.showWarning("AUDIT_REPORT.TITLE", "AUDIT_REPORT.MSG_SAVE_SUCCESS_FILE_NOT_UPLOAD"); });
                // }
                // else {
                //   this.showSuccess("AUDIT_REPORT.TITLE", "AUDIT_REPORT.MSG_SAVE_SUCCESS");
                //   if (this.fromSummary)
                //     this.return("auditsummary/audit-summary");
                // }
                 this.savedataloading = false;
                 this.showSuccess("AUDIT_REPORT.TITLE", "AUDIT_REPORT.MSG_SAVE_SUCCESS");
                   if (this.fromSummary)
                     this.return("auditsummary/audit-summary");
              }
              else {
                switch (res.result) {
                  case 2:
                    this.showError("AUDIT_REPORT.TITLE", 'AUDIT_REPORT.MSG_NOT_SAVED');
                    break;
                  case 3:
                    this.showError("AUDIT_REPORT.TITLE", 'AUDIT_REPORT.MSG_REQUEST_NOT_CORRECT_FORMAT');
                    break;
                  case 4:
                    this.showError("AUDIT_REPORT.TITLE", 'AUDIT_REPORT.MSG_NO_ITEM_FOUND');
                    break;
                  case 5:
                    this.showError("AUDIT_REPORT.TITLE", 'AUDIT_REPORT.MSG_NO_AUDITOR');
                    break;
                }
              }
              this.savedataloading = false;
            },
            error => {
              this.showError("AUDIT_REPORT.TITLE", "EDIT_AUDIT.MSG_UNKNOWN_ERROR");
              this.savedataloading = false;
            }
          );
      }
    }
    catch (e) {
      this.showError("AUDIT_REPORT.TITLE", "EDIT_AUDIT.MSG_UNKNOWN_ERROR");
    }
  }
  GetAuditors(auditid) {
    this.service.GetScheduledAuditor(auditid).subscribe(
      res => {
        if (res && res.result == 1) {
          this._auditors = res.auditors;
        }
      }
    );
  }
  Reset() {
    this.model.servicedatefrom = null;
    this.model.servicedateto = null;
    this.model.auditors = [];
    this.model.attachments = [];
    this.model.comment = null;
  }
  getFile(file: AttachmentFile) {

    this.downloadloading = true;
    if(file.fileUrl){
      this.fileStoreService.downloadBlobFile(file.uniqueld, FileContainerList.Audit)
      .subscribe(res => {
        this.downloadFile(res, file.mimeType, file.fileName);
        this.downloadloading = false;
      },
        error => {
          this.downloadloading = false;
          this.showError('AUDIT_REPORT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
        });
        this.downloadloading = false;
      }
    else if (file.guid) {
      this.service.getAuditReportFiles(file.id)
        .subscribe(res => {
          this.downloadFile(res, file.mimeType, file.fileName);
        },
          error => {
            this.showError("AUDIT_REPORT.TITLE", "EDIT_AUDIT.MSG_UNKNOWN_ERROR");
          });
          this.downloadloading = false;
    }
  }

  downloadFile(data, mimeType, fileName) {
    let windowNavigator = window.navigator;
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = fileName ? fileName : "export";//url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }


  GetFileExtensionIcon(file) {
    var ext = file.fileName.toUpperCase().split('.').pop();
    if (ext == "XLS" || ext == "XLX" || ext == "XLSX")
      return "assets/images/uploaded-files.svg";
    else if (ext == "DOC" || ext == "DOCX")
      return "assets/images/uploaded-files-1.svg"
    else
      return "assets/images/uploaded-files-2.svg";
  }
}

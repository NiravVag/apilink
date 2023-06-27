import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { first, takeUntil } from 'rxjs/operators';
import { DataSource, ResponseResult } from 'src/app/_Models/common/common.model';
import { BookingReportModel, EditEmailSend, EmailAddressType, EmailPreviewData, EmailPreviewRequest, EmailPreviewResponseResult, EmailPreviewValidator, EmailSendFileDetails, EmailSendFileUpload, EmailSendHistoryResponse, EmailSendHistoryResult, EmailSendingType, EmailSendResult, EmailTempModelData, EmailValidOption, InvoiceModel, InvoiceSendFileUpload } from 'src/app/_Models/email-send/edit-email-send.model';
import { EmailSendType } from 'src/app/_Models/email-send/email-config.model';

import { AttachmentFile } from 'src/app/_Models/fileupload/fileupload';
import { UserModel } from 'src/app/_Models/user/user.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { EditEmailSendService } from 'src/app/_Services/email-send/edit-email-send.service';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { Validator } from '../../common';
import { DetailComponent } from '../../common/detail.component';
import { APIService, EmailTypeInvoiceSend, EmailTypeReportSend, FileContainerList, InvoiceType, Service, StandardDateFormat, Url } from '../../common/static-data-common';

@Component({
  selector: 'app-edit-invoice-send',
  templateUrl: './edit-invoice-send.component.html'
})
export class EditInvoiceSendComponent extends DetailComponent {
  componentDestroyed$: Subject<boolean> = new Subject();
  masterModel: EditEmailSend;
  model: InvoiceSendFileUpload;
  emailPreviewModel: EmailPreviewRequest;
  emailModelItem: Array<EmailPreviewData>;
  emailPreviewTitle: string;
  reportStatusValidated: string;
  private modelRef: NgbModalRef;
  toggleFormSection: boolean;
  standardDateFormat: string = StandardDateFormat;
  @ViewChild('emailruleinfo') emailruleinfo;
  @ViewChild('emailPreviewContent') emailPreviewContent;
  @ViewChild('emailSendHistory') emailSendHistory;
  emailSendHistoryResponse: EmailSendHistoryResponse;
  private currentRoute: Router;
  currentUser: UserModel;

  constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    public modalService: NgbModal, public pathroute: ActivatedRoute, public emailSendService: EditEmailSendService,
    private fileService: FileStoreService, public validator: Validator, public utility: UtilityService,
    private authserve: AuthenticationService, public referenceService: ReferenceService) {

    super(router, route, translate, toastr, modalService);

    this.validator.setJSON("emailsend/editemailsend.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.currentUser = authserve.getCurrentUser();
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.masterModel = new EditEmailSend();
    this.model = new InvoiceSendFileUpload();
    this.masterModel.bookingDataLoading = true;
    //get booking details from booking summary page
    this.pathroute.queryParams.subscribe(
      params => {
        if (params != null && params['paramParent'] != null) {
          var invoice = JSON.parse(decodeURI(params['paramParent']));
          this.masterModel.emailRuleRequestbyInvoice.invoiceList = invoice.items.filter(x => x.selectedToSendEmail).map(x => x.invoiceNo);
          this.masterModel.emailRuleRequestbyInvoice.serviceId = Service.Inspection;
          this.masterModel.emailRuleRequestbyInvoice.emailSendingtype = EmailSendingType.InvoiceStatus;
          if (invoice.items.length > 0 && invoice.items[0].invoiceTypeId && invoice.items[0].invoiceTypeId != null)
            this.masterModel.emailRuleRequestbyInvoice.invoiceType = invoice.items[0].invoiceTypeId;
        }
      });

    this.init();
  }

  init() {
    this.getEmailRuleData();
    this.getFileTypeList();
  }

  //each item select check box
  checkIfAllBookingSelected() {

    this.masterModel.selectedBookingReport = this.masterModel.invoiceList.every(function (item: InvoiceModel) {
      return item.isSelected;
    });

    this.masterModel.selectedReport = this.masterModel.invoiceList.filter(x => x.isSelected).map(x => x.invoiceId).length;

    this.selectUploadFile();

  }

  //all select check box
  selectAllBooking() {
    for (var i = 0; i < this.masterModel.invoiceList.length; i++) {
      let reportitem = this.masterModel.invoiceList[i];
      if (reportitem)
        this.masterModel.invoiceList[i].isSelected = this.masterModel.selectedBookingReport;
    }
    var invoiceIds = this.masterModel.invoiceList.filter(x => x.isSelected).map(x => x.invoiceId);
    var distinctinvoiceIds = invoiceIds.filter((n, i) => invoiceIds.indexOf(n) === i);
    this.masterModel.selectedReport = distinctinvoiceIds.length;


    this.selectUploadFile();
  }
  //select upload file to email send
  selectUploadFile() {


    this.masterModel.invoiceList.forEach(item => {
      this.masterModel.emailSendFileDetails.map((x: EmailSendFileDetails) => {
        if (item.isSelected) {
          if ((x.invoiceNo == item.invoiceNo)) {
            x.isSelected = true;
            x.isShow = true;
          }
        }
        else {
          if (x.invoiceNo == item.invoiceNo) {
            x.isSelected = false;
            x.isShow = false;
          }
          else if (x.invoiceNo == item.invoiceNo && this.masterModel.invoiceList.filter(y => y.isSelected && y.invoiceNo == item.invoiceNo).length == 0) {
            x.isSelected = false;
            x.isShow = false;
          }
        }
      });
    });

    // this.selectAllUpload();
    this.checkIfAllUpload();
  }

  //get invoice details
  getBookingInvoiceDetails() {
    this.emailSendService.getInvoiceDetails(this.masterModel.emailRuleRequestbyInvoice)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processSuccessInvoiceDetails(response);
        },
        error => {
          this.masterModel.bookingDataLoading = false;
        });
  }

  //success response
  processSuccessInvoiceDetails(response) {
    if (response) {
      if (response.result == EmailSendResult.Success) {
        this.masterModel.invoiceList = response.emailSendList;
        var tempDataList = []
        // create invoice number list
        response.emailSendList.forEach(function (d) {
          tempDataList.push({
            invoiceId: d.invoiceId,
            invoiceNo: d.invoiceNo
          });
        });
        this.masterModel.invoiceNumberList = tempDataList;
        this.getFileSendData();
      }
      else if (response.result == EmailSendResult.RequestNotCorrectFormat) {
        this.showWarning('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.MSG_REQUEST_NOT_CORRECT_FORMAT');
      }
      else if (response.result == EmailSendResult.NotFound) {
        this.showWarning('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.LBL_No_ITEM');
      }
      else {
        this.showWarning('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    }
    this.masterModel.bookingDataLoading = false;
  }

  //redirect to report link

  RedirectToReport(itemInfo) {
    if (itemInfo && itemInfo.finalManualReportLink && itemInfo.finalManualReportLink != '') {
      window.open(itemInfo.finalManualReportLink);
    }
    else if (itemInfo && itemInfo.reportLink && itemInfo.reportLink != '') {
      window.open(itemInfo.reportLink);
    }
  }

  //open pop up
  openConfirm(id, content) {
    this.masterModel.deleteId = id;
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
  }

  //remove the id assign
  getId() {
    this.masterModel.deleteId = null;
  }

  //delete the email send file
  deleteEmailSendFile() {
    this.masterModel.deleteLoading = true;
    if (this.masterModel.deleteId > 0) {
      this.emailSendService.deleteInvoiceSendFile(this.masterModel.deleteId)
        .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {
            if (response) {
              if (response.result == EmailSendResult.Success) {
                this.showSuccess('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
              }
              else {
                this.showError('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
              }
              this.masterModel.deleteLoading = false;
              this.modelRef.close();
              this.getFileSendData();
            }
            this.selectUploadFile();
          },
          error => {
            this.setError(error);
            this.masterModel.deleteLoading = false;
          });
    }
  }

  //get file send details
  getFileSendData() {
    this.masterModel.searchLoading = true;
    this.masterModel.invoiceFileRequest.InvoiceNoList = this.masterModel.invoiceNumberList.map(x => x.invoiceNo);
    this.masterModel.invoiceFileRequest.serviceId = Service.Inspection;
    this.masterModel.invoiceFileRequest.emailSendingtype = EmailSendingType.InvoiceStatus;

    this.emailSendService.getInvoiceFileList(this.masterModel.invoiceFileRequest)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == EmailSendResult.Success) {

            //... for file name if length
            for (var i = 0; i < response.emailSendFileList.length; i++) {
              response.emailSendFileList[i].fileName =
                response.emailSendFileList[i].fileName &&
                  response.emailSendFileList[i].fileName.length > 15 ?
                  response.emailSendFileList[i].fileName.substring(0, 15) + "..." :
                  response.emailSendFileList[i].fileName;
            }

            this.masterModel.emailSendFileDetails = response.emailSendFileList;
            this.masterModel.searchLoading = false;
            this.selectUploadFile();
          }
          else if (response && response.result == EmailSendResult.NotFound) {
            this.masterModel.emailSendFileDetails = [];
            this.masterModel.searchLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.searchLoading = false;
        });
  }

  //get file type list
  getFileTypeList() {
    this.masterModel.fileTypeLoading = true;
    this.emailSendService.getInvoiceFileTypeList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.fileTypeList = data.dataSourceList;
          }
          this.masterModel.fileTypeLoading = false;
        },
        error => {
          this.masterModel.fileTypeLoading = false;
        });
  }

  //onchange file select
  onSelectFile(event) {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.isformValid()) {
      this.cloudUpload(event);
    }
  }

  //upload the file to cloud
  cloudUpload(event) {
    // var event = this.masterModel.files;
    var uploadFiles = [];
    if (event && !event.error && event.files) {
      if (event.files.length > this.masterModel.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {
        this.masterModel.pageLoader = true;
        for (let file of event.files) {
          var guid = this.newUniqueId();
          let fileItem: AttachmentFile = {
            fileName: file.name,
            file: file,
            fileSize: file.fileSize,
            isNew: true,
            id: 0,
            status: 0,
            mimeType: file.type,
            fileUrl: "",
            uniqueld: guid,
            isSelected: false,
            fileDescription: null
          };
          uploadFiles.unshift(fileItem);

          // upload to cloud - selected files and the status is new
          if (uploadFiles) {
            this.fileService.uploadFiles(FileContainerList.InvoiceSend, [fileItem]).subscribe(response => {
              if (response && response.fileUploadDataList) {

                var invoiceNo = this.masterModel.invoiceNumberList.find(x => x.invoiceId == this.model.invoiceId).invoiceNo

                this.model.fileLink = response.fileUploadDataList[0].fileCloudUri;
                this.model.uniqueId = response.fileUploadDataList[0].fileName;
                this.model.fileName = file.name;
                if (invoiceNo) {
                  this.model.invoiceNo = invoiceNo;
                }
                this.save();
              }
            },
              error => {
                this.masterModel.pageLoader = false;
                this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
              });
          }
        }
      }
    }
    else {
      this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', event.errorMessage);
    }
  }

  //frame unique id
  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  //is form valid
  isformValid(): boolean {
    var isOk = this.validator.isValid('invoiceId') &&
      this.validator.isValid('fileTypeId');

    return isOk;
  }

  //file upload parts control reset
  fileUploadReset() {
    this.model = new InvoiceSendFileUpload();
    this.validator.isSubmitted = false;
  }

  //save the details
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isformValid()) {
      this.model.serviceId = APIService.Inspection;
      this.emailSendService.saveInvoiceSendAttachments(this.model)
        .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {
            if (response) {
              if (response.result == EmailSendResult.Success) {
                this.showSuccess('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.MSG_SAVED_SUCCESS');
              }
              else {
                this.showWarning('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
              }
            }
            this.getFileSendData();
            this.fileUploadReset();
            this.masterModel.pageLoader = false;
          },
          error => {
            if (error && error.error && error.error.errors && error.error.statusCode == 400) {
              let validationErrors: [];
              validationErrors = error.error.errors;
              this.openValidationPopup(validationErrors);
            }
            else {
              this.showError("EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE", "COMMON.MSG_UNKNONW_ERROR");
            }
            this.masterModel.pageLoader = false;
          });
    }
  }

  //is preview button
  isPreviewShown() {
    var isOk: boolean = false;
    if (this.masterModel.invoiceList && this.masterModel.invoiceList.length > 0) {
      isOk = this.masterModel.invoiceList.filter(x => x.isSelected).length > 0;
    }
    return isOk;
  }

  preview() {
    var selectedBookingIds;
    var selectedUploadIds;

    if (this.masterModel.invoiceList && this.masterModel.invoiceList.length > 0) {
      selectedBookingIds = this.masterModel.invoiceList.filter(x => x.isSelected).map(x => x.invoiceId);
    }
    if (this.masterModel.emailSendFileDetails && this.masterModel.emailSendFileDetails.length > 0) {
      //upload id primary key
      selectedUploadIds = this.masterModel.emailSendFileDetails.filter(x => x.isSelected).map(x => x.emailSendFileId);
    }
    var id = this.masterModel.ruleId;

    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}`], { queryParams: { paramParent: encodeURI(JSON.stringify(this.model)) } });
  }

  getPathDetails() {
    return '';
  }

  //redirect to file reports link
  RedirectToFileReport(fileLink: string) {
    if (fileLink && fileLink != '') {
      window.open(fileLink);
    }
  }

  redirectToEditInvoice(invoiceNo) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditInvoice + invoiceNo + "/" + Service.Inspection;
    window.open(editPage);
  }

  //advance filter
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  getUniqueColorList() {
    var resultColorSets = ['FFC1C1', '87CEEB', 'D3D3D3', 'FAF0E6', 'FFE4C4', 'FFDEAD', 'FFFACD', 'F0FFF0',
      'E6E6FA', 'FFE4E1', 'ADD8E6', 'AFEEEE', '48D1CC', 'E0FFFF', '66CDAA', '7FFFD4',
      '8FBC8F', '98FB98', 'BDB76B', 'F0E68C', 'FAFAD2', 'EEDD82', 'BC8F8F', 'D2B48C', 'FFB6C1',
      'DDA0DD', '8EE5EE', '9AFF9A', 'EEEE00', 'FFC1C1', '9381FF', 'FFD8BE', 'FFEEDD', '9986DF', 'CCAFCF',
      'F2CEC7', 'F686BD', 'F4BBD3', 'D6D2D2', 'FE5D9F', 'F5A1C8', 'F3D0E3', 'EDF5FC', 'B8C5D6', 'A39BA8',
      '5ADA8F', 'D3DDE9', '00B295', 'C9DAEA', 'C05A74'];

    return resultColorSets;
  }
  getUniqueCombineOrder() {
    let groupIds = this.masterModel.bookingReportData.filter(x => x.combineProductId != null && x.combineProductId != 0)
      .map(x => x.combineProductId);
    let distinctGroupIds = groupIds.filter((n, i) => groupIds.indexOf(n) === i);
    return distinctGroupIds;
  }

  applyGroupColor() {
    var count = 0;

    var uniqueCombineProductList = this.getUniqueCombineOrder();
    var uniqueColorList = this.getUniqueColorList();
    uniqueCombineProductList.forEach(item => {
      var productList = this.masterModel.bookingReportData.filter(x => x.combineProductId == item);
      productList.forEach(row => {

        if (row.combineProductId == item) {
          row.colorCode = "#" + uniqueColorList[count];
        }

      });
      count = count + 1;
    });
  }

  getViewPath(): string {
    return "invoicesummary";
  }

  getEditPath(): string {
    return "";
  }

  //get email rule list from email config
  getEmailRuleData() {
    this.masterModel.pageLoader = true;
    this.emailSendService.getEmailRuleDataByInvoiceList(this.masterModel.emailRuleRequestbyInvoice)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processSuccessemailRuleData(response);
        },
        error => {
          this.masterModel.pageLoader = false;
          this.masterModel.bookingDataLoading = false;
        });
  }

  processSuccessemailRuleData(response) {
    if (response) {

      if (response.result == EmailSendResult.OneRuleFound) {
        if (response.bookingIdsWithoutRule && response.bookingIdsWithoutRule.length > 0)//show warning if booking doesn't have a rule
        {
          this.masterModel.bookingParam.bookingIdList = response.bookingIdsWithRule;
          this.showWarning("EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE') + response.bookingIdsWithoutRule.join(", "));
        }
        this.masterModel.ruleId = response.ruleId;
        this.getBookingInvoiceDetails();
      }
      else if (response.result == EmailSendResult.MoreThanOneRuleFound) {

        if (response.bookingIdsWithoutRule && response.bookingIdsWithoutRule.length > 0)//show warning if booking doesn't have a rule
        {
          this.masterModel.bookingParam.bookingIdList = response.bookingIdsWithRule;
          this.showWarning("EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE') + response.bookingIdsWithoutRule.join(", "));
        }
        this.masterModel.emailRuleList = response.emailRuleList;

        this.modelRef = this.modalService.open(this.emailruleinfo, { windowClass: "lgModelWidth", centered: true, keyboard: false, backdrop: 'static' });
        this.getBookingInvoiceDetails();
      }
      else if (response.result == EmailSendResult.NoRuleFound) { //no rule found 
        this.masterModel.isNoRuleFound = true;
      }
      else if (response.result == EmailSendResult.EachBookingHasDifferentRule) {
        this.masterModel.isEachBookingHasDifferentRule = true;
      }
      else {
        this.masterModel.isNoRuleFound = true;
      }
    }
    this.masterModel.pageLoader = false;
  }

  getRuleId(event: number) {
    if (event && event > 0)
      this.masterModel.ruleId = event;
  }

  //each item select upload check box
  checkIfAllUpload() {

    this.masterModel.selectedReportUpload = this.masterModel.emailSendFileDetails.filter(x => x.isShow).every(function (item: any) {
      return item.isSelected;
    });

    this.masterModel.isheaderReportShow = this.masterModel.emailSendFileDetails.filter(x => x.isShow).length > 0;
  }

  //all select upload check box
  selectAllUpload() {
    for (var i = 0; i < this.masterModel.emailSendFileDetails.length; i++) {
      this.masterModel.emailSendFileDetails[i].isSelected = this.masterModel.selectedReportUpload;
    }
  }

  popupOk(): boolean {
    return !(this.masterModel.emailRuleList.filter(x => x.isSelected).length > 0);
  }

  closeEmailPreview($event) {
    this.modelRef.close();
  }

  emailPreview() {
    this.emailPreviewModel = new EmailPreviewRequest();
    this.emailPreviewModel.emailReportAttachment = [];
    this.emailPreviewModel.emailReportPreviewData = [];
    this.emailPreviewModel.emailRuleId = this.masterModel.ruleId;
    this.emailPreviewModel.esTypeId = EmailTypeInvoiceSend;

    this.masterModel.invoiceList.forEach(item => {
      if (item.isSelected) {
        this.emailPreviewModel.emailReportPreviewData.
          push({ reportId: 0, bookingId: 0, extraFileName: '', invoiceNo: item.invoiceNo, reportRevision: 0, reportVersion: 0 });
      }


    });

    this.masterModel.emailSendFileDetails.forEach(item => {
      if (item.isSelected) {
        this.emailPreviewModel.emailReportAttachment.
          push({ reportId: 0, bookingId: 0, invoiceNo: item.invoiceNo, fileLink: item.fileLink, fileName: item.fileName, fileType: item.fileTypeName });
      }
    });


    var invoiceNoList = this.emailPreviewModel.emailReportPreviewData.map(x => x.invoiceNo);
    var distinctInvoiceNoList = invoiceNoList.filter((n, i) => invoiceNoList.indexOf(n) === i);
    this.masterModel.bookingNoList = distinctInvoiceNoList;

    this.masterModel.pageLoader = true;
    this.emailSendService.getEmailDetails(this.emailPreviewModel)
      .subscribe(res => {
        if (res.result == EmailPreviewResponseResult.success) {
          if (res.data)
            this.processEmailPreviewService(res.data);
        }
        else if (res.result == EmailPreviewResponseResult.failure) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterModel.pageLoader = false;
        }
        else if (res.result == EmailPreviewResponseResult.emailrulenotvalid) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'EDIT_EMAIL_SEND.MSG_EMAIL_RULE_NOT_VALID');
          this.masterModel.pageLoader = false;
        }
        else if (res.result == EmailPreviewResponseResult.inspectionsummarylinknotavailable) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'EDIT_EMAIL_SEND.MSG_INSP_SUMMARY_LINK_NOT_AVAILABLE');
          this.masterModel.pageLoader = false;
        }
      },
        error => {
          this.masterModel.pageLoader = false;
        });
  }

  //process the email service response
  processEmailPreviewService(res) {
    if (res && res.length > 0) {
      this.emailPreviewTitle = "Invoice Send";
      this.emailModelItem = res.map(x => {
        var item: EmailPreviewData = {
          emailSubject: x.emailSubject,
          emailSubjectDisplay: x.emailSubject ? x.emailSubject.length >= 75 ? x.emailSubject.substring(0, 75) + '...' : x.emailSubject : "",
          emailBody: x.emailBody,
          emailCCList: x.emailCCList,
          emailBCCList: x.emailBCCList,
          emailValidOption: x.emailValidOption,
          ruleId: x.ruleId,
          emailToList: x.emailToList,
          attachmentList: x.attachmentList,
          reportBookingList: x.reportBookingList,
          emailId: x.emailId,
          isEmailSelected: false,
          isEmailValid: true,
          active: false,
          isenabled: true,
          ccMailText: "",
          toMailText: "",
          bccMailText: "",
          customerId: x.customerId
        }
        return item;
      });

      this.masterModel.isEmailReady = false;
      this.modelRef = this.modalService.open(this.emailPreviewContent, { windowClass: "mailer-popup", centered: true, backdrop: 'static' });
      this.masterModel.pageLoader = false;
    }
    else if (res.length == 0) {
      this.showError('EMAIL_SEND_PREVIEW.LBL_INVOICE_SEND_TITLE', 'EMAIL_SEND_PREVIEW.MSG_CONTACT_IT_TEAM');
      this.masterModel.pageLoader = false;
    }
    else {
      this.showError('EMAIL_SEND_PREVIEW.LBL_INVOICE_SEND_TITLE', 'EMAIL_SEND_PREVIEW.MSG_CONTACT_IT_TEAM');
      this.masterModel.pageLoader = false;
    }
  }

  //sending the mail
  sendEmail($event) {
    debugger;
    this.masterModel.emailSendLoading = true;
    var emailItems = $event.filter(x => x.isEmailSelected == true);
    this.emailSendService.sendEmail(emailItems)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {

          if (response) {
            this.showSuccess('EMAIL_SEND_PREVIEW.LBL_INVOICE_SEND_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_SEND_SHORTLY');
            this.masterModel.emailSendLoading = false;
            this.modelRef.close();
            if (this.masterModel.selectedBookingReport) {
              this.return('invoicesummary');
            }
            else {
              for (var i = 0; i < this.masterModel.bookingReportData.length; i++) {
                this.masterModel.bookingReportData[i].isSelected = !this.masterModel.selectedBookingReport;
              }

              this.masterModel.bookingReportData.forEach(item => {
                item.isSelected = false;
              });

              this.masterModel.selectedBookingReport = false;

              this.masterModel.selectedReportUpload = false;

              this.masterModel.emailSendFileDetails.forEach(item => {
                item.isSelected = false;
              });
            }
          }
          else {
            this.showError('EMAIL_SEND_PREVIEW.LBL_INVOICE_SEND_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_NOT_SEND');
          }
          this.masterModel.emailSendLoading = false;
        },
        error => {
          this.masterModel.emailSendLoading = false;
          this.showError('EMAIL_SEND_PREVIEW.LBL_INVOICE_SEND_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_NOT_SEND');
        });
  }

  //get the email send history data
  getEmailSendHistory(inspectionId, reportId) {

    this.masterModel.pageLoader = true;

    this.emailSendService.getEmailSendHistory(inspectionId, reportId, EmailSendType.ReportSend)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(response => {
        //process the email send history data
        this.processEmailSendHistory(response);
      },
        error => {
          this.setError(error);
          this.masterModel.pageLoader = false;
          this.showError('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'EDIT_EMAIL_SEND.MSG_NO_EMAIL_HISTORY_FOUND');
        });
  }

  //process the email send history
  processEmailSendHistory(response) {
    if (response.result == EmailSendHistoryResult.Success) {
      this.emailSendHistoryResponse = response;
      this.modelRef = this.modalService.open(this.emailSendHistory, { windowClass: "md1ModelWidth", centered: true, backdrop: 'static' });
      this.masterModel.pageLoader = false;
    }
    else if (response.result == EmailSendHistoryResult.NotFound) {
      this.showError('EDIT_EMAIL_SEND.LBL_INVOICE_SEND_TITLE', 'EDIT_EMAIL_SEND.MSG_NO_EMAIL_HISTORY_FOUND');
      this.masterModel.pageLoader = false;
    }
  }


  showEmailHistory(iteminfo) {
    //show the email history popup only if reportsend count greater than 0
    if (iteminfo.reportSend > 0)
      this.getEmailSendHistory(iteminfo.bookingId, iteminfo.reportId);
  }

  getBrowserTimeZonefromUTC(dateString) {
    if (dateString) {
      return new Date(dateString + ' UTC').toString();
    }
    return "";
  }

}

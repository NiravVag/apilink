/*  */import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { first, takeUntil } from 'rxjs/operators';
import { CustomerDecisionListSaveRequest } from 'src/app/_Models/booking/customerdecision.model';
import { DataSource, ResponseResult } from 'src/app/_Models/common/common.model';
import { AutoCustomerDecisionRequest, AutoCustomerDecisionResult, BookingReportModel, CustomerResult, EditEmailSend, EmailAddressType, EmailPreviewData, EmailPreviewRequest, EmailPreviewResponseResult, EmailPreviewValidator, EmailSendFileDetails, EmailSendFileUpload, EmailSendHistoryResponse, EmailSendHistoryResult, EmailSendingType, EmailSendResult, EmailTempModelData, EmailValidOption } from 'src/app/_Models/email-send/edit-email-send.model';
import { EmailSendType } from 'src/app/_Models/email-send/email-config.model';

import { AttachmentFile } from 'src/app/_Models/fileupload/fileupload';
import { UserModel } from 'src/app/_Models/user/user.model';
import { InspectionCustomerDecisionService } from 'src/app/_Services/booking/inspectioncustomerdecision.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { EditEmailSendService } from 'src/app/_Services/email-send/edit-email-send.service';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { Validator } from '../../common';
import { DetailComponent } from '../../common/detail.component';
import { APIService, CustomerDecisionSaveResponseResult, EmailTypeReportSend, FileContainerList, ReportSendInEmailAttachment, StandardDateFormat } from '../../common/static-data-common';

@Component({
  selector: 'app-edit-email-send',
  templateUrl: './edit-email-send.component.html'
})
export class EditEmailSendComponent extends DetailComponent {
  componentDestroyed$: Subject<boolean> = new Subject();
  masterModel: EditEmailSend;
  model: EmailSendFileUpload;
  emailPreviewModel: EmailPreviewRequest;
  emailModelItem: Array<EmailPreviewData>;
  emailPreviewTitle:string;
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
  selectedReportIndex:number;

  constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    public modalService: NgbModal, public pathroute: ActivatedRoute, public emailSendService: EditEmailSendService,
    private fileService: FileStoreService, public validator: Validator, public utility: UtilityService,
    private authserve: AuthenticationService, public referenceService: ReferenceService,private cdService: InspectionCustomerDecisionService) {

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
    this.model = new EmailSendFileUpload();

    this.masterModel.bookingDataLoading = true;

    //get booking details from booking summary page
    this.pathroute.queryParams.subscribe(
      params => {
        if (params != null && params['paramParent'] != null) {
          var getVal = JSON.parse(decodeURI(params['paramParent']));
          this.masterModel.bookingParam.bookingIdList = getVal.bookingIdList;
          this.masterModel.bookingParam.serviceId = getVal.serviceId;
          this.masterModel.bookingParam.emailSendingtype=EmailSendingType.ReportSend;
        }
      });

    this.init();
  }

  init() {
    this.getEmailRuleData();
    this.getFileSendData();
    this.getFileTypeList();
  }

  //each item select check box
  checkIfAllBookingSelected() {

    this.masterModel.selectedBookingReport = this.masterModel.bookingReportData.filter(x => x.isOkToSend &&
      x.reportRevision==x.requestedReportRevision && x.isParentProduct).every(function (item: BookingReportModel) {
      return item.isSelected;
    });

    this.masterModel.selectedReport = this.masterModel.bookingReportData.filter(x => x.isSelected).map(x => x.reportId).length;

    this.selectUploadFile();

  }

  //all select check box
  selectAllBooking() {
    for (var i = 0; i < this.masterModel.bookingReportData.length; i++) {
      let reportitem = this.masterModel.bookingReportData[i];
      if (reportitem.isOkToSend && reportitem.reportRevision==reportitem.requestedReportRevision)
        this.masterModel.bookingReportData[i].isSelected = this.masterModel.selectedBookingReport;
    }
    var reportIds = this.masterModel.bookingReportData.filter(x => x.isSelected).map(x => x.reportId);
    var distinctReportIds = reportIds.filter((n, i) => reportIds.indexOf(n) === i);
    this.masterModel.selectedReport = distinctReportIds.length;
    this.selectUploadFile();
  }
  //select upload file to email send
  selectUploadFile() {

    this.masterModel.bookingReportData.filter(x => x.isParentProduct && x.isOkToSend && x.reportRevision==x.requestedReportRevision).forEach(item => {

      this.masterModel.emailSendFileDetails.map((x: EmailSendFileDetails) => {
        if (item.isSelected) {
          if ((x.bookingId == item.bookingId && x.reportId == item.reportId) || (x.bookingId == item.bookingId && !x.reportId)) {
            x.isSelected = true;
            x.isShow = true;
          }
        }
        else {
          if (x.bookingId == item.bookingId && x.reportId == item.reportId) {
            x.isSelected = false;
            x.isShow = false;
          }
          else if (x.bookingId == item.bookingId && !x.reportId && this.masterModel.bookingReportData.filter(y => y.isSelected && y.bookingId == item.bookingId).length == 0) {
            x.isSelected = false;
            x.isShow = false;
          }
        }
      });
      
    });

    // this.selectAllUpload();
    this.checkIfAllUpload();
  }

  //get booking report details
  getBookingReportDetails() {
    this.emailSendService.getBookingReportDetails(this.masterModel.bookingParam)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processSuccessReportDetails(response);
        },
        error => {
          this.masterModel.bookingDataLoading = false;
        });
  }

  showNewReportRevision(rowItem,modalContent,index){
       this.masterModel.requestRevisionNo='';
       this.masterModel.requestNewRevisionNo=null;
       this.masterModel.fbReportId=null;
       if(rowItem)
       {
        this.masterModel.reportStatusLoading=true;
        this.masterModel.fbReportId=rowItem.fbReportId;
        this.emailSendService.checkFbReportIsInvalidated(this.masterModel.fbReportId)
        .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {       
                  this.masterModel.reportStatusLoading=false;
                  if (response) {
                    this.showError('EDIT_EMAIL_SEND.LBL_TITLE', 'Report is Invalidated');
                  }
                  else
                  {
                    this.selectedReportIndex=index;
                    var revNo = rowItem.reportRevision > 0 ? rowItem.reportRevision + 1 : 1;
                    this.masterModel.requestNewRevisionNo=revNo;                
                    this.masterModel.requestRevisionNo = "R"+ revNo;
                    this.masterModel.apiReportId=rowItem.reportId;
                    this.modelRef = this.modalService.open(modalContent, { centered: true, backdrop: 'static' });  
                  }      
              
          },
          error => {
            this.setError(error);
            this.masterModel.reportStatusLoading=false;
          }); 
       }
  }

  //success response
  processSuccessReportDetails(response) {
    if (response) {
      if (response.result == EmailSendResult.Success) {
        debugger;

        this.masterModel.bookingReportData = response.emailSendList;
        this.masterModel.bookingReportList = response.emailSendList;

        this.masterModel.bookingList = new Array<DataSource>();
        this.masterModel.productList = new Array<DataSource>();
        this.masterModel.poList = new Array<DataSource>();
        this.masterModel.reportList = new Array<DataSource>();

        if (this.masterModel.bookingReportData && this.masterModel.bookingReportData.length > 0) {

          //add color for combine product
          this.applyGroupColor();

          this.masterModel.isheaderCheckBoxShow =
            this.masterModel.bookingReportData.filter(x => x.isOkToSend && x.reportRevision==x.requestedReportRevision).length > 0;

          this.masterModel.bookingReportData.forEach(element => {

            var _bookingList = {
              id: element.bookingId,
              name: element.bookingId.toString()
            };
            var _productList = {
              id: element.productId,
              name: element.productName
            };

            var _reportList = {
              id: element.reportId,
              name: element.reportName
            };

            // set report Revision as File
            if(element.reportRevision>0 && element.reportRevision==element.requestedReportRevision)
            {
              element.extraFileName= "R"+element.requestedReportRevision;
            }
           

            //make unique booking list 
            if (this.masterModel.bookingList && this.masterModel.bookingList.length > 0 &&
              !(this.masterModel.bookingList.find(x => x.id == element.bookingId))) {

              this.masterModel.bookingList.push(_bookingList);
            }
            else if (this.masterModel.bookingList.length <= 0) {

              this.masterModel.bookingList.push(_bookingList);

            }

            //make unique  product list
            if (this.masterModel.productList && this.masterModel.productList.length > 0 && !(this.masterModel.productList.find(x => x.id == element.productId))) {
              this.masterModel.productList.push(_productList);
            }
            else if (this.masterModel.productList.length <= 0) {
              this.masterModel.productList.push(_productList);
            }

            //make unique  report list
            if (this.masterModel.reportList && this.masterModel.reportList.length > 0 && !(this.masterModel.reportList.find(x => x.id == element.reportId))) {
              this.masterModel.reportList.push(_reportList);
            }
            else if (this.masterModel.reportList.length <= 0) {
              this.masterModel.reportList.push(_reportList);
            }

            if (element.poList && element.poList.length > 0) {
              //make unique po list 
              element.poList.forEach(poData => {
                if (this.masterModel.poList && this.masterModel.poList.length > 0 && !(this.masterModel.poList.find(x => x.id == poData.id))) {
                  this.masterModel.poList.push(poData);
                }
                else if (this.masterModel.poList.length <= 0) {
                  this.masterModel.poList.push(poData);
                }
              });
            }

            if (element.reportSend == 0)
              element.reportSendData = "Not Send";
            else if (element.reportSend > 0)
              element.reportSendData = "Report Sent (" + element.reportSend + ")";
          });
        }
      }
      else if (response.result == EmailSendResult.RequestNotCorrectFormat) {
        this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_REQUEST_NOT_CORRECT_FORMAT');
      }
      else if (response.result == EmailSendResult.NotFound) {
        this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.LBL_No_ITEM');
      }
      else {
        this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    }
    this.masterModel.bookingDataLoading = false;
  }

  //reset 
  resetFilter() {
    this.masterModel.bookingReportData = this.masterModel.bookingReportList;

    this.masterModel.bookingIds = [];
    this.masterModel.productIds = [];
    this.masterModel.poIds = [];

  }

  //apply top filters to filter booking table data
  applyFilter() {

    this.masterModel.bookingReportData = this.masterModel.bookingReportList;

    if (this.masterModel.bookingIds && this.masterModel.bookingIds.length > 0) {
      this.masterModel.bookingReportData = this.masterModel.bookingReportData.
        filter(x => this.masterModel.bookingIds.includes(x.bookingId));
    }
    if (this.masterModel.poIds && this.masterModel.poIds.length > 0) {

      var _bookingReportList: Array<BookingReportModel> = new Array<BookingReportModel>();

      // this.masterModel.bookingReportData = this.masterModel.bookingReportData.
      // filter(x=> this.masterModel.poIds.includes(x.poList.map(x=>x.id).));

      this.masterModel.bookingReportData.forEach(elem => {

        var poIdList = elem.poList.map(x => x.id);

        this.masterModel.poIds.forEach(ele => {

          var reportList = this.masterModel.bookingReportData.
            filter(x => x.poList.map(x => x.id).includes(ele));
          var _bookingList: Array<BookingReportModel> = new Array<BookingReportModel>();

          if (reportList && reportList.length > 0) {

            reportList.forEach(element => {
              if (_bookingReportList.filter(x => x.bookingId == element.bookingId &&
                x.productId == element.productId).length <= 0) {
                _bookingList.push(element);
              }

            });
          }

          if (_bookingList && _bookingList.length > 0) {
            _bookingReportList.push.apply(_bookingReportList, _bookingList);
          }
        });

      });

      if (_bookingReportList && _bookingReportList.length > 0) {
        this.masterModel.bookingReportData = _bookingReportList;
      }
    }

    if (this.masterModel.productIds && this.masterModel.productIds.length > 0) {

      this.masterModel.bookingReportData = this.masterModel.bookingReportData.
        filter(x => this.masterModel.productIds.includes(x.productId));
    }
    else if ((!this.masterModel.bookingIds || this.masterModel.bookingIds.length <= 0) &&
      (!this.masterModel.poIds || this.masterModel.poIds.length <= 0)) {
      this.masterModel.bookingReportData = this.masterModel.bookingReportList;
    }
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
      this.emailSendService.deleteEmailSendFile(this.masterModel.deleteId)
        .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {
            if (response) {
              if (response.result == EmailSendResult.Success) {
                this.showSuccess('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
                // this.reset();
              }
              else {
                this.showError('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
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

    requestNewVersionFromFB() {
      this.masterModel.requestVersionLoading = true;
      if (this.masterModel.requestNewRevisionNo > 0) {
        this.emailSendService.getReportRevision(this.masterModel.apiReportId,this.masterModel.fbReportId,this.masterModel.requestNewRevisionNo)
          .pipe(takeUntil(this.componentDestroyed$), first())
          .subscribe(
            response => {
              if (response) {
                if (response.result == EmailSendResult.Success) {
                   this.showSuccess('EDIT_EMAIL_SEND.LBL_TITLE', 'please wait to generate the report with new revision# '+this.masterModel.requestRevisionNo+'. it may take 2-3 min');
                   this.masterModel.bookingReportData[this.selectedReportIndex].extraFileName="";                 
                   this.masterModel.bookingReportData[this.selectedReportIndex].requestedReportRevision=this.masterModel.requestNewRevisionNo;
                   this.masterModel.bookingReportData=[...this.masterModel.bookingReportData];
                   this.checkIfAllBookingSelected();
                }
                else 
                {
                  this.showError('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                }
                this.masterModel.requestVersionLoading = false;
                this.modelRef.close();
              }
            },
            error => {
              this.setError(error);
              this.masterModel.requestVersionLoading = false;
            });
      }
    }

  //get file send details
  getFileSendData() {
    this.masterModel.searchLoading = true;
    this.emailSendService.getFileSendData(this.masterModel.bookingParam)
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
    this.emailSendService.getFileTypeList()
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
      // this.masterModel.files = event;
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
            this.fileService.uploadFiles(FileContainerList.EmailSend, [fileItem]).subscribe(response => {
              if (response && response.fileUploadDataList) {
                this.model.fileLink = response.fileUploadDataList[0].fileCloudUri;
                this.model.uniqueId = response.fileUploadDataList[0].fileName;
                this.model.fileName = file.name;
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
    var isOk = this.validator.isValid('bookingIds') &&
      this.validator.isValid('fileTypeId');

    return isOk;
  }

  //file upload parts control reset
  fileUploadReset() {
    this.model = new EmailSendFileUpload();
    this.validator.isSubmitted = false;
  }

  //save the details
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isformValid()) {
      this.model.serviceId = APIService.Inspection;
      this.emailSendService.save(this.model)
        .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {
            if (response) {
              if (response.result == EmailSendResult.Success) {
                this.showSuccess('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_SAVED_SUCCESS');
              }
              else {
                this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
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
              this.showError("EDIT_EMAIL_SEND.LBL_TITLE", "COMMON.MSG_UNKNONW_ERROR");
            }
            this.masterModel.pageLoader = false;
          });
    }
  }

  //is preview button
  isPreviewShown() {
    var isOk: boolean = false;
    if (this.masterModel.bookingReportData && this.masterModel.bookingReportData.length > 0) {
      isOk = this.masterModel.bookingReportData.filter(x => x.isSelected).length > 0;
    }
    return isOk;

  }

  preview() {
    var selectedBookingIds;
    var selectedUploadIds;

    if (this.masterModel.bookingReportData && this.masterModel.bookingReportData.length > 0) {
      selectedBookingIds = this.masterModel.bookingReportData.filter(x => x.isSelected).map(x => x.bookingId);
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
    return "esend/summary";
  }

  getEditPath(): string {
    return "";
  }

  //get email rule list from email config
  getEmailRuleData() {
    this.masterModel.pageLoader = true;
    this.emailSendService.getEmailRuleData(this.masterModel.bookingParam)
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
          this.showWarning("EDIT_EMAIL_SEND.LBL_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE') + response.bookingIdsWithoutRule.join(", "));
        }
        this.masterModel.ruleId = response.ruleId;
        this.getBookingReportDetails();
      }
      else if (response.result == EmailSendResult.MoreThanOneRuleFound) {

        if (response.bookingIdsWithoutRule && response.bookingIdsWithoutRule.length > 0)//show warning if booking doesn't have a rule
        {
          this.masterModel.bookingParam.bookingIdList = response.bookingIdsWithRule;
          this.showWarning("EDIT_EMAIL_SEND.LBL_TITLE", this.utility.textTranslate('EDIT_EMAIL_SEND.LBL_EMAIL_RULE_NOT_CONFIGURE') + response.bookingIdsWithoutRule.join(", "));
        }
        this.masterModel.emailRuleList = response.emailRuleList;

        this.modelRef = this.modalService.open(this.emailruleinfo, { windowClass: "lgModelWidth", centered: true, keyboard: false, backdrop: 'static' });
        this.getBookingReportDetails();
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
    this.emailPreviewModel.esTypeId = EmailTypeReportSend;

    this.masterModel.bookingReportData.forEach(item => {
      if (item.isSelected) {
        this.emailPreviewModel.emailReportPreviewData.
            push({ reportId: item.reportId, bookingId: item.bookingId, 
              reportVersion:item.reportVersion,reportRevision:item.reportRevision,
              extraFileName: item.extraFileName ,invoiceNo:""});
      }


    });

    this.masterModel.emailSendFileDetails.forEach(item => {
      if (item.isSelected) {
        this.emailPreviewModel.emailReportAttachment.
          push({ reportId: item.reportId,invoiceNo:"", bookingId: item.bookingId, fileLink: item.fileLink, fileName: item.fileName, fileType: item.fileTypeName });
      }
    });


    var bookingNoList = this.emailPreviewModel.emailReportPreviewData.map(x => x.bookingId);
    var distinctBookingNoList = bookingNoList.filter((n, i) => bookingNoList.indexOf(n) === i);
    this.masterModel.bookingNoList = distinctBookingNoList;

    this.masterModel.pageLoader = true;
    this.emailSendService.getEmailDetails(this.emailPreviewModel)
      .subscribe(res => {
        if (res.result == EmailPreviewResponseResult.success) {
          if (res.data)
            this.processEmailPreviewService(res.data);
        }
        else if (res.result == EmailPreviewResponseResult.failure) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          this.masterModel.pageLoader = false;
        }
        else if (res.result == EmailPreviewResponseResult.emailrulenotvalid) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'EDIT_EMAIL_SEND.MSG_EMAIL_RULE_NOT_VALID');
          this.masterModel.pageLoader = false;
        }
        else if (res.result == EmailPreviewResponseResult.inspectionsummarylinknotavailable) {
          this.showWarning('EDIT_EMAIL_SEND.LBL_TITLE', 'EDIT_EMAIL_SEND.MSG_INSP_SUMMARY_LINK_NOT_AVAILABLE');
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
      this.emailPreviewTitle="Report Send";
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
      this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_CONTACT_IT_TEAM');
      this.masterModel.pageLoader = false;
    }
    else {
      this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_CONTACT_IT_TEAM');
      this.masterModel.pageLoader = false;
    }
  }

  //sending the mail
  sendEmail($event) {
    this.masterModel.emailSendLoading = true;
    var emailItems = $event.filter(x => x.isEmailSelected == true);
    this.emailSendService.sendEmail(emailItems)
      .subscribe(
        response => {
          if (response) {
            // call parallel method
            this.autoCustomerDecisionList(emailItems);
            this.showSuccess('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_SEND_SHORTLY');
            this.masterModel.emailSendLoading = false;
            this.modelRef.close();
            if (this.masterModel.selectedBookingReport) {
              this.return('esend/summary');
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
            this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_NOT_SEND');
          }
          this.masterModel.emailSendLoading = false;
        },
        error => {
          this.masterModel.emailSendLoading = false;
          this.showError('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_EMAIL_NOT_SEND');
        });
  }

  autoCustomerDecisionList(emailItems: any) {
    const request = new AutoCustomerDecisionRequest();
    if(emailItems){
      for(const item of  emailItems){
        for(const booking of item.reportBookingList){
          request.bookingIdList.push(booking.inspectionId);
          request.reportIdList.push(booking.reportId);
        }
      }
      request.customerId = emailItems[0].customerId
      // call parallel api with out destroy
      this.emailSendService.autoCustomerDecisionList(request)
        .subscribe(
          response => {
            if (response.result == AutoCustomerDecisionResult.Success) {
              this.saveCustomerDecisionList(response.autoCustomerDecisionList);
            }
          },
          error => {
            this.setError(error);
          });
    }
  }

  saveCustomerDecisionList(autoCustomerDecisionList: any) {
    for(const item of  autoCustomerDecisionList){
      const request = new CustomerDecisionListSaveRequest();
      request.bookingId = item.bookingId;
      request.reportIdList = item.reportIdList;
      request.comments = item.comments;
      request.customerResultId = CustomerResult.Pass;
      request.isAutoCustomerDecision = true;
      // call parallel api with out destroy
      this.cdService.SaveInspectionCustomerDecisionList(request)
      .subscribe(res => {
      },
      error => {
        this.setError(error);
      });
    }
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
          this.showError('EDIT_EMAIL_SEND.LBL_TITLE', 'EDIT_EMAIL_SEND.MSG_NO_EMAIL_HISTORY_FOUND');
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
      this.showError('EDIT_EMAIL_SEND.LBL_TITLE', 'EDIT_EMAIL_SEND.MSG_NO_EMAIL_HISTORY_FOUND');
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

import { Component, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { ToastrService } from 'ngx-toastr';
import { JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { uploadPOErrorMessages } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { AttachmentFile, PoProductUploadRequest, POProductUploadResult, PurchaseOrderSampleFile } from 'src/app/_Models/purchaseorder/upload-purchaseorder.model';
import { PurchaseOrderService } from 'src/app/_Services/purchaseorder/purchaseorder.service';


@Component({
  selector: 'app-upload-purchaseorder-lite',
  templateUrl: './upload-purchaseorder-lite.component.html',
  styleUrls: ['./upload-purchaseorder-lite.component.scss']
})
export class UploadPurchaseOrderLiteComponent {

  public uploadLimit = 1;
  public fileSize = 5000000;
  public uploadFileExtensions = 'csv,xlsx';
  public pageLoader: boolean = false;
  public saveloading: boolean = false;
  public uploadfileimage: string = "assets/images/uploaded-files.svg";
  public SmallUploadTitle = "Upload PO File";
  public SmallUploadSubTitle = "";
  public smallUploadButtonText = "Add File";
  public smallSignatureTitle = "Upload Purchase order File";
  processFileLoading: boolean;
  customerId: number = 17;
  supplierId: number = 3674;

  poUploadErrorList: any;
  poUploadErrorMessageList: any;

  purchaseOrderAttachments: any;

  @Input() modelData: any;

  @Output("closeUploadPoProductFromBooking") closeUploadPoProductFromBooking: EventEmitter<any> = new EventEmitter();

  _purchaseOrderSampleFile=PurchaseOrderSampleFile;

  constructor(
    public translate: TranslateService,
    public toastr: ToastrService,
    route: ActivatedRoute,
    jsonHelper: JsonHelper,
    router: Router,
    public utility: UtilityService,
    public modalService: NgbModal,
    private service: PurchaseOrderService) {

    this.poUploadErrorList = [];
    this.poUploadErrorMessageList = [];

  }


  removeAttachment(index) {
    this.purchaseOrderAttachments.splice(index, 1);
    this.poUploadErrorMessageList = [];
  }

  selectFiles(event) {
    if (event && !event.error && event.files) {
      this.purchaseOrderAttachments = [];

      if (this.purchaseOrderAttachments.length + 1 > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else if (event.files.length > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {
        this.addFilesToAttachments(event);
      }
    }
    else if (event && event.error && event.errorMessage) {
      this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', event.errorMessage);
    }
  }

  addFilesToAttachments(event) {
    for (let file of event.files) {
      let fileItem: AttachmentFile = {
        fileName: file.name,
        file: file,
        isNew: true,
        id: 0,
        mimeType: "",
        uniqueld: this.newGuid()
      };
      this.purchaseOrderAttachments.push(fileItem);
    }
    this.poUploadErrorMessageList = [];
    this.processFile();
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  //generate the po product upload request
  generatePoProductUploadRequest() {
    var poProductUploadRequest = new PoProductUploadRequest();

    poProductUploadRequest.customerId = this.modelData.customerId;
    poProductUploadRequest.supplierId = this.modelData.supplierId;
    poProductUploadRequest.businessLineId = this.modelData.businessLineId;
    poProductUploadRequest.existingBookingPoProductList = this.modelData.existingBookingPoProductList;
    return poProductUploadRequest;
  }

  processFile() {
    this.processFileLoading = true;
    var attachedList = [];
    if (this.purchaseOrderAttachments)
      attachedList = this.purchaseOrderAttachments.filter(x => x.isNew);
    if (attachedList.length > 0) {
      //generate the po product upload request
      var poProductUploadRequest = this.generatePoProductUploadRequest();
      this.service.uploadPurchaseOrderLite(poProductUploadRequest, attachedList).subscribe(response => {
        this.processFileSuccessResponse(response);
      },
        error => {
          this.processFileLoading = false;
          this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CONNECT_IT_TEAM');
        });
    }
    else {
      this.processFileLoading = false;
      this.showError('PURCHASEORDER_UPLOAD.TITLE', 'PURCHASEORDER_UPLOAD.MSG_FILEUPLOAD_REQ');
    }
  }

  //download po sample file
  downloadPOSampleFile(typeId) {
    this.service.downloadFile(typeId)
      .subscribe(res => {
        if (typeId == PurchaseOrderSampleFile.ImportPOSampleFile)
          this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Import-Purchase-Order");
        else if (typeId == PurchaseOrderSampleFile.ImportPODateFormat)
          this.downloadFile(res, "application/pdf", "Purchase-Order-DateFormat-UserGuide");
      },
        error => {
          this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
        });
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



  processFileSuccessResponse(response) {
    this.processFileLoading = false;
    if (response.result == POProductUploadResult.Success) {
      this.closeUploadPoProductFromBooking.emit(response.purchaseOrderSuccessList);
      this.showSuccess("UPLOAD_PURCHASE_ORDER_LITE.LBL_TITLE", "UPLOAD_PURCHASE_ORDER_LITE.LBL_SAVE_PO_SUCCESS");
    }
    else if (response.result == POProductUploadResult.EmptyRows) {
      this.showError("UPLOAD_PURCHASE_ORDER_LITE.LBL_TITLE", "UPLOAD_PURCHASE_ORDER_LITE.MSG_NO_DATA_FOUND");
    }
    else if (response.result == POProductUploadResult.ValidationError) {
      this.processValidationError(response);
    }
  }

  processValidationError(response) {

    this.poUploadErrorList = [];
    this.poUploadErrorList = response.poProductUploadErrorList;
    var errorDataList = this.poUploadErrorList.map(x => x.errorData);
    var distinctErrorDataList = errorDataList.filter((n, i) => errorDataList.indexOf(n) === i);


    this.poUploadErrorMessageList = [];
    distinctErrorDataList.forEach(element => {
      var errorMessage = uploadPOErrorMessages.find(x => x.id == element);
      if (errorMessage)
        this.poUploadErrorMessageList.push(errorMessage.name);
    });
  }

  public showError(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle);
  }

  public showWarning(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.warning(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }


  public showSuccess(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.success(tradMessage, tradTitle);
  }

}




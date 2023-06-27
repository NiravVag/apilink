import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Validator } from '../../common/validator';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceBankSaveRequest, InvoiceBankGetResponse, InvoiceBankGetResult, InvoiceBankTax, InvoiceBankSaveResponse, InvoiceBankSaveResult } from 'src/app/_Models/invoice/invoicebank';
import { InvoiceBankService } from 'src/app/_Services/invoice/invoicebank.service';
import { DetailComponent } from '../../common/detail.component';
import { JsonHelper } from '../../common/jsonHelper';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { FileContainerList } from '../../common/static-data-common';
import { AttachmentFile } from 'src/app/_Models/fileupload/fileupload';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { UtilityService } from 'src/app/_Services/common/utility.service';


@Component({
  selector: 'app-edit-invoice-bank',
  templateUrl: './edit-invoice-bank.component.html',
  styleUrls: ['./edit-invoice-bank.component.scss']
})
export class EditInvoiceBankComponent extends DetailComponent {

  private _translate: TranslateService;
  private _toastr: ToastrService;
  public model: InvoiceBankSaveRequest;
  public bankTaxValidators: Array<any> = [];
  public accountCurrencyList: Array<any> = [];
  public billingEntityList: Array<any> = [];
  public jsonHelper: JsonHelper;
  public uploadfileimage: string = "assets/images/uploaded-files.svg";
  public smallSignatureTitle = "Upload Signature File";
  public smallChopeTitle = "Upload Chop File";
  public smallUploadButtonText = "Add File";
  public uploadLimit = 1;
  public fileSize = 5000000;
  public uploadFileExtensions = 'png,jpg,jpeg';
  public pageLoader: boolean = false;
  public saveloading: boolean = false;

  constructor(router: Router, public validator: Validator, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService, public bankService: InvoiceBankService, public refService: ReferenceService, public utility: UtilityService,
    public fileStoreService: FileStoreService, public calendar: NgbCalendar) {
    super(router, route, translate, toastr);
    this._toastr = toastr;
    this._translate = translate;
  }
  onInit(id?: any) {
    this.pageLoader = true;
    this.resetBank();
    this.initValidator();
    this.getCurrencyList();
    this.getBillingEntityList();
    if (id) {
      this.GetBankDetailsById(id);
    }
    else {
      this.addBankTax();
      this.pageLoader = false;
    }
  }

  async GetBankDetailsById(bankId) {
    let response: InvoiceBankGetResponse;
    try {
      response = await this.bankService.getBankDetails(bankId);
      switch (response.result) {
        case InvoiceBankGetResult.success:
          this.model = this.mapModel(response.bankDetails, response.bankTaxDetails);
          break;

        case InvoiceBankGetResult.invoiceBankNotFound:
          this.model = new InvoiceBankSaveRequest();
          this.bankTaxValidators = [];
          this.addBankTax();
          break;

        default:
          break;
      }
      this.pageLoader = false;
    }
    catch (e) {
      this.pageLoader = false;
      this.setError(e);
    }
  }

  initValidator() {
    this.validator.isSubmitted = false;
    this.validator.setJSON("invoice/invoice-bank-save.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = this.validator.jsonHelper;
  }

  getFile(file: string, fileName: string) {
    this.pageLoader = true;

    this.fileStoreService.downloadBlobFile(file, FileContainerList.Invoice)
      .subscribe(res => {

        this.downloadFile(res, res.type);
      },
        error => {
          this.pageLoader = false;
          this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
        });

  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
    this.pageLoader = false;
  }

  selectSignatureFile(event) {
    var uploadFiles = [];
    if (event && !event.error && event.files) {

      if (event.files.length > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {
        this.pageLoader = true;
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
          this.model.signatureFileName = file.name;
          this.model.signatureFileType = file.type;
          uploadFiles.unshift(fileItem);

          // upload to cloud - selected files and the status is new
          if (uploadFiles) {

            this.fileStoreService.uploadFiles(FileContainerList.Invoice, [fileItem]).subscribe(response => {
              if (response && response.fileUploadDataList) {
                this.model.signatureFileUrl = response.fileUploadDataList[0].fileCloudUri;
                this.model.signatureFileUniqueId = response.fileUploadDataList[0].fileName;

              }
              this.pageLoader = false;
            },
              error => {
                this.pageLoader = false;
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



  removeSignatureFile() {
    this.model.signatureFileUrl = "";
    this.model.signatureFileName = "";
    this.model.signatureFileUniqueId = "";
    this.model.signatureFileType = "";
  }

  removeChopFile() {
    this.model.chopFileName = "";
    this.model.chopFileUniqueId = "";
    this.model.chopFileUrl = "";
    this.model.chopFileType = "";
  }

  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  selectChopFile(event) {
    var uploadFiles = [];
    if (event && !event.error && event.files) {

      if (event.files.length > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {
        this.pageLoader = true;
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
          this.model.chopFileName = file.name;
          this.model.chopFileType = file.type;
          uploadFiles.unshift(fileItem);

          // upload to cloud - selected files and the status is new
          if (uploadFiles) {

            this.fileStoreService.uploadFiles(FileContainerList.Invoice, [fileItem]).subscribe(response => {
              if (response && response.fileUploadDataList) {
                this.model.chopFileUrl = response.fileUploadDataList[0].fileCloudUri;
                this.model.chopFileUniqueId = response.fileUploadDataList[0].fileName;

              }
              this.pageLoader = false;
            },
              error => {
                this.pageLoader = false;
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



  mapModel(bankDetails: any, bankTaxDetails: any): InvoiceBankSaveRequest {
    var model: InvoiceBankSaveRequest = {
      id: bankDetails.id,
      accountName: bankDetails.accountName,
      accountNumber: bankDetails.accountNumber,
      swiftCode: bankDetails.swiftCode,
      bankName: bankDetails.bankName,
      bankAddress: bankDetails.bankAddress,
      remarks: bankDetails.remarks,
      accountCurrency: bankDetails.accountCurrency,
      signatureFileUniqueId: bankDetails.signatureFileUniqueId,
      signatureFileUrl: bankDetails.signatureFileUrl,
      signatureFileName: bankDetails.signatureFilename,
      chopFileUniqueId: bankDetails.chopFileUniqueId,
      chopFileUrl: bankDetails.chopFileUrl,
      chopFileName: bankDetails.chopFilename,
      billingEntity: bankDetails.billingEntity,
      signatureFileType: '',
      chopFileType: '',
      invoiceBankTaxList: bankTaxDetails.map((x) => {
        var bankTax: InvoiceBankTax = {
          id: x.id,
          taxName: x.taxName,
          taxValue: x.taxValue,
          fromDate: x.fromDate,
          toDate: x.toDate,
          isDisable: this.calendar.getToday().after(new NgbDate(x.toDate.year, x.toDate.month, x.toDate.day))
        };

        this.bankTaxValidators.push({
          bankTax: bankTax, validator:
            Validator.getValidator(bankTax, "invoice/invoice-bank-tax-save.valid.json", this.jsonHelper,
              this.validator.isSubmitted, this._toastr, this._translate)
        });

        return bankTax;
      })
    };
    return model;
  }


  addBankTax() {
    var bankTax: InvoiceBankTax = {
      id: 0,
      taxName: null,
      taxValue: null,
      fromDate: null,
      toDate: null,
      isDisable: false
    };

    this.model.invoiceBankTaxList.push(bankTax);
    this.bankTaxValidators.push({
      bankTax: bankTax, validator: Validator.getValidator(bankTax, "invoice/invoice-bank-tax-save.valid.json",
        this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
    });
  }


  removeBankTax(index) {
    this.model.invoiceBankTaxList.splice(index, 1);
    this.bankTaxValidators.splice(index, 1);
  }

  //get currency list
  getCurrencyList() {
    this.refService.getCurrencyList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.accountCurrencyList = response.dataSourceList;
          }

        },
        error => {
          this.setError(error);

        });
  }

  //get billing entity list
  getBillingEntityList() {
    this.refService.getBillingEntityList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.billingEntityList = response.dataSourceList;
          }
        },
        error => {
          this.setError(error);
        });
  }


  async save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.bankTaxValidators)
      item.validator.isSubmitted = true;

    if (this.isFormValid()) {
      if (this.checkTaxDateIsInOrder()) {
        let response: InvoiceBankSaveResponse;
        this.saveloading = true;
        try {
          if (this.model.id > 0) {
            response = await this.bankService.updateBankDetails(this.model);
          }
          else {
            response = await this.bankService.saveBankDetails(this.model);
          }
          this.saveloading = false;
          switch (response.result) {
            case InvoiceBankSaveResult.success:
              this.showSuccess('EDIT_INV_BANK.TITLE', 'EDIT_INV_BANK.MSG_SAVE_SUCCESS');
              this.return('invoicebanksummary/invoice-bank-summary');
              break;
            case InvoiceBankSaveResult.invoiceBankAccountIsAlreadyExist:
              this.showError('EDIT_INV_BANK.TITLE', 'EDIT_INV_BANK.MSG_BANK_ACC_EXIST');
              break;

            default:
              break;
          }
        }
        catch (e) {
          this.saveloading = false;
          this.setError(e);
        }

      }
      else {
        this.showError('EDIT_INV_BANK.TITLE', 'EDIT_INV_BANK.MSG_TAX_DATE_RANGE');
      }
    }

  }

  isFormValid() {
    return this.validator.isValid('accountName')
      && this.validator.isValid('accountNumber')
      && this.validator.isValid('bankName')
      && this.validator.isValid('swiftCode')
      && this.validator.isValid('accountCurrency')
      && this.validator.isValid('billingEntity')
      && this.bankTaxValidators.every((x) =>
        x.validator.isValid('taxName')
        && x.validator.isValid('taxValue')
        && x.validator.isValid('fromDate')
        && x.validator.isValid('toDate'))


  }

  checkTaxDateIsInOrder() {
    let isok = true;
    if (this.model.invoiceBankTaxList.length > 1) {

      const toFindDuplicates = arry => arry.filter((item, index) => this.model.invoiceBankTaxList.filter(x => x.taxName.trim() == item.taxName.trim()).length >= 2)
      const duplicateElements = toFindDuplicates(this.model.invoiceBankTaxList);

      for (let index = duplicateElements.length; index > 0; index--) {

        var currentfromDate = duplicateElements[index - 1].fromDate;
        var preViousToDate = (duplicateElements[index - 2]) ? duplicateElements[index - 2].toDate : null;

        if (preViousToDate != null) {
          var fromDate = new NgbDate(currentfromDate.year, currentfromDate.month, currentfromDate.day);
          var prevDate = new NgbDate(preViousToDate.year, preViousToDate.month, preViousToDate.day);

          if (prevDate.before(fromDate)) {
            isok = true;
          }
          else {
            isok = false;
            break;
          }
        }
      }
    }
    return isok;
  }

  resetBank() {
    this.model = new InvoiceBankSaveRequest();
    this.bankTaxValidators = [];
    this.model.invoiceBankTaxList = [];
    this.validator.isSubmitted = false;
    // this.addBankTax();
  }

  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }
}

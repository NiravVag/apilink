import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import * as CryptoJS from 'crypto-js';
import { DataSource, InvoicePreviewFrom, InvoicePreviewRequest, InvoiceReportTemplate, InvoiceReportTemplateRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { InvoiceSummaryService } from 'src/app/_Services/invoice/invoicesummary.service';
import { Validator } from '../../common';
import { Url } from '../../common/static-data-common';

@Component({
  selector: 'app-invoice-preview',
  templateUrl: './invoice-preview.component.html',
  styleUrls: ['./invoice-preview.component.scss']
})
export class InvoicePreviewComponent {


  @Input() invoicePreviewModel: InvoicePreviewRequest;
  @Output() closeInvoicePreview = new EventEmitter<any>();
  model: InvoiceReportTemplate;

  invoiceReportTemplateListLoading: boolean;
  invoiceReportTemplateList: Array<DataSource> = [];
  templateBaseUrl: string;
  templateEntityId: string;
  invoiceSelected: boolean = true;
  invoicePreviewFrom = InvoicePreviewFrom;
  isInvoiceTableVisible: boolean = false;


  constructor(public activeModal: NgbActiveModal, public validator: Validator, private translate: TranslateService,
    private invoiceSummaryService: InvoiceSummaryService, public utility: UtilityService,) { }


  ngOnInit(): void {
    this.model = new InvoiceReportTemplate();  
    
    if (this.invoicePreviewModel.invoicePreviewFrom == InvoicePreviewFrom.InvoiceSummary ||
      this.invoicePreviewModel.invoicePreviewFrom == InvoicePreviewFrom.CreditNoteSummary ||
      this.invoicePreviewModel.invoicePreviewFrom == InvoicePreviewFrom.ManualInvoice ||
      this.invoicePreviewModel.invoicePreviewFrom == InvoicePreviewFrom.ExtarFees) {
      this.isInvoiceTableVisible = true;
    }

    this.getInvoiceTemplates();
    this.getInvoiceUrl();
  }

  redirectToEditInvoice(invoiceNo) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditInvoice + invoiceNo + "/" + this.invoicePreviewModel.service;
    window.open(editPage);
  }

  closePreview() {
    this.closeInvoicePreview.next(null);
  }

  getInvoiceTemplates() {
    this.invoiceReportTemplateListLoading = true;
    let invoiceTemplateRequest = new InvoiceReportTemplateRequest();
    invoiceTemplateRequest.customerId = this.invoicePreviewModel.customerId;
    invoiceTemplateRequest.invoicePreviewTypes = this.invoicePreviewModel.invoicePreviewTypes;
    this.invoiceSummaryService.getInvoiceReportTemplates(invoiceTemplateRequest)
      .pipe()
      .subscribe(
        res => {
          if (res.result == ResponseResult.Success) {
            this.invoiceReportTemplateList = res.resultList;
          }

          this.invoiceReportTemplateListLoading = false;
        },
        error => {

        });
  }

  getInvoiceUrl() {
    this.invoiceSummaryService.getInvoiceReportTemplateUrl()
      .pipe()
      .subscribe(
        res => {
          if (res.result == ResponseResult.Success) {
            this.templateBaseUrl = res.url;
            this.templateEntityId = res.entityId;
          }
        },
        error => {

        });
  }

  //open another tab to show a report from different project
  generateInvoiceReport() {

    if (this.validator.isValid('templateId')) {


      var editPage = this.templateBaseUrl + this.invoicePreviewModel.invoiceNo +
        "/" + this.model.templateId + "/" + this.templateEntityId;

      if (this.invoicePreviewModel.isAccountingRole && this.invoicePreviewModel.createdBy)
        editPage = editPage + "/" + encodeURIComponent(this.encryptString(this.invoicePreviewModel.createdBy)) + "/" + encodeURIComponent(this.encryptString(this.invoicePreviewModel.isAccountingRole));

      window.open(editPage);
      this.validator.isSubmitted = false;
      this.model.templateId = null;
      this.closePreview();

    }
  }

  encryptString(data): string {
    var key = CryptoJS.enc.Utf8.parse('1234567891012345');
    var iv = CryptoJS.enc.Utf8.parse('1234567891012345');
    var encryptedData = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(data), key,
      {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
    return encryptedData.toString();
  }
}

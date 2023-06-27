import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CreditNoteSummaryMasterModel, CreditNoteSummaryModel } from 'src/app/_Models/claim/credit-note-summary.model';
import { InvoicePreviewFrom, InvoicePreviewRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { ClaimService } from 'src/app/_Services/claim/claim.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { Validator } from '../../common';
import { APIService, InvoicePreviewType, MobileViewFilterCount, PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { InvoicePreviewComponent } from '../../shared/invoice-preview/invoice-preview.component';

@Component({
  selector: 'app-credit-note-summary',
  templateUrl: './credit-note-summary.component.html',
  styleUrls: ['./credit-note-summary.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '0',
        'padding-bottom': '16px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ]),
    trigger('expandCollapseMobileAd', [
      state('open', style({
        'height': '*',
        'opacity': 1
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class CreditNoteSummaryComponent extends SummaryComponent<CreditNoteSummaryModel> {

  masterModel: CreditNoteSummaryMasterModel;
  searchLoading: boolean;
  isFilterOpen = true;
  pagesizeitems = PageSizeCommon;
  selectedPageSize = PageSizeCommon[0];
  modelRemove: any;
  modelRef: NgbModalRef;
  filterDataShown: boolean;
  filterCount: number;
  invoicePreviewRequest= new InvoicePreviewRequest();    
  _customvalidationforCreditid: boolean = false;
  @ViewChild('invoicePreviewTemplate') invoicePreviewTemplate: TemplateRef<any>; 
  constructor(
    private claimService: ClaimService,
    private modalService: NgbModal,
    router: Router, public validator: Validator, route: ActivatedRoute, translate?: TranslateService,
    toastr?: ToastrService, public utility?: UtilityService) {
    super(router, validator, route, translate, toastr, utility);
  }

  onInit(): void {
    this.masterModel = new CreditNoteSummaryMasterModel();
    this.model = new CreditNoteSummaryModel();
    this.getCreditTypeList();
    this.filterDataShown = false;
  }
  getData(): void {

    this.searchLoading = true;
    this.model.noFound = false;
    if (this.isFilterOpen)
      this.isFilterOpen = false;

    this.filterDataShown = this.filterTextShown();
    this.claimService.creditNoteSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this.model.items = response.data;
            this.masterModel.isAccountingCreditNoteRole = response.isAccountingCreditNoteRole
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this.model.items = [];
          }
          else {
            this.error = response.result;
          }
          this.searchLoading = false;
        },
        error => {
          this.setError(error);
          this.searchLoading = false;
        });
  }
  getPathDetails(): string {
    return "editClaim/edit-credit-note";
  }

  getCreditTypeList() {
    this.masterModel.creditTypeLoading = true;
    this.claimService.getCreditTypeList().subscribe(res => {
      if (res && res.result === ResponseResult.Success) {
        this.masterModel.creditTypeList = res.dataSourceList;
      }
      else {
        this.masterModel.creditTypeList = [];
      }
      this.masterModel.creditTypeLoading = false;;
    }, error => {
      this.masterModel.creditTypeList = [];
      this.masterModel.creditTypeLoading = false;;
    })
  }


  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  IsDateValidationRequired(): boolean {
    return (this.model.fromDate !== undefined && this.model.fromDate !== null) || (this.model.toDate !== undefined && this.model.toDate !== null);
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "fromdate": {
        this.model.fromDate = null;
        break;
      }
      case "todate": {
        this.model.toDate = null;
        break;
      }
    }
  }

  deleteCreditNote(item) {
    this.claimService.deleteCreditNote(item.id)
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.showSuccess('CREDI_NOTE_SUMMARY.LBL_TITLE', item.creditNo + ' Deleted Successfully')
            this.refresh();
          }
          else {
            this.error = response.result;
            this.loading = false;
          }

        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();
  }

  openConfirm(id, cerditNo, content) {

    this.modelRemove = {
      id: id,
      creditNo: cerditNo
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  export() {
    this.masterModel.exportLoading = true;


    this.claimService.exportCreditNoteSummary(this.model)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Credit_Note.xlsx");
        this.masterModel.exportLoading = false;
      },
        error => {
          this.masterModel.exportLoading = false;
        });
  }
  async downloadFile(data: BlobPart, mimeType: string, fileName: string) {
    const blob = new Blob([data], { type: mimeType });
    let windowNavigator: any = window.Navigator;
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, fileName);
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fileName;
      a.href = url;
      a.click();
    }
  }
  reset() {
    this.isFilterOpen = true;
    this.onInit();
  }

  SearchDetails() {
    this.validator.isSubmitted = true;
    if (this.isSummaryFormValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.model.index = 1;
      this.refresh();
    }
  }
  isSummaryFormValid() {
    return this.validator.isValidIf('fromDate', this.IsDateValidationRequired()) && this.validator.isValidIf('toDate', this.IsDateValidationRequired())
  }

  getSearchDetails() {
    if (this.model.creditNo && this.model.creditNo != '') {
      this.SearchDetails();
    }
  }

  filterTextShown() {
    var isFilterDataSelected = false;

    if ((this.model.fromDate && this.model.fromDate != '') || (this.model.toDate && this.model.toDate != '') || (this.model.creditNo && this.model.creditNo != '') || this.model.creditType > 0) {

      //desktop version
      if (!this.isMobile) {
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;

        //from date and to date selected
        if (this.model.fromDate && this.model.fromDate != '' && this.model.toDate && this.model.toDate != '')
          count = MobileViewFilterCount + count;

        if (this.model.creditNo != '')
          count = MobileViewFilterCount + count;

        //credit type selected
        if (this.model.creditType > 0) {
          count = MobileViewFilterCount + count;
        }

        this.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.filterCount = 0;
        this.masterModel.creditTypeList = [];
      }
    }

    return isFilterDataSelected;
  }
  //open invoice template the popup
  openInvoiceTemplatePopUp(creditNo, customerId) {
    var invoiceNo = creditNo;

    if (invoiceNo && invoiceNo != "") {

      this.invoicePreviewRequest= new InvoicePreviewRequest();   
  
      this.invoicePreviewRequest.customerId=customerId;
      this.invoicePreviewRequest.invoiceNo=invoiceNo;
      this.invoicePreviewRequest.invoiceData=[invoiceNo];
      this.invoicePreviewRequest.invoicePreviewTypes=[InvoicePreviewType.CreditNote];
      this.invoicePreviewRequest.invoicePreviewFrom=InvoicePreviewFrom.CreditNoteSummary;
      this.invoicePreviewRequest.previewTitle="Invoice Preview";
      this.invoicePreviewRequest.service= APIService.Inspection;
      this.modelRef = this.modalService.open(this.invoicePreviewTemplate,
        {
            windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title',
            centered: true, backdrop: 'static'
        });       
    }
  }
  closeInvoicePreview() 
  {
    this.modelRef.close();
  }
}

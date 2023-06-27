import {
  trigger,
  state,
  style,
  transition,
  animate,
} from "@angular/animations";
import { Component, TemplateRef, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { of, Subject } from "rxjs";
import {
  catchError,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  takeUntil,
  tap,
} from "rxjs/operators";
import { Validator } from "src/app/components/common";
import {
  APIService,
  EntityAccess,
  InvoicePreviewType,
  ListSize,
  PageSizeCommon,
} from "src/app/components/common/static-data-common";
import { SummaryComponent } from "src/app/components/common/summary.component";
import { InvoicePreviewFrom, InvoicePreviewRequest, ResponseResult } from "src/app/_Models/common/common.model";
import {
  ManualInvoiceSummaryMasterModel,
  ManualInvoiceSummaryModel,
} from "src/app/_Models/invoice/manual-invoice-summary.model";
import { UtilityService } from "src/app/_Services/common/utility.service";
import { CustomerService } from "src/app/_Services/customer/customer.service";
import { CustomerPriceCardService } from "src/app/_Services/customer/customerpricecard.service";
import { ManualInvoiceService } from "src/app/_Services/invoice/manual-invoice.service";
import { InvoicePreviewComponent } from "../../shared/invoice-preview/invoice-preview.component";

@Component({
  selector: "app-manual-invoice-summary",
  templateUrl: "./manual-invoice-summary.component.html",
  styleUrls: ["./manual-invoice-summary.component.scss"],
  animations: [
    trigger("expandCollapse", [
      state(
        "open",
        style({
          height: "*",
          opacity: 1,
          "padding-top": "24px",
          "padding-bottom": "24px",
        })
      ),
      state(
        "close",
        style({
          height: "0px",
          opacity: 0,
          "padding-top": 0,
          "padding-bottom": 0,
        })
      ),
      transition("open <=> close", animate(300)),
    ]),
  ],
})
export class ManualInvoiceSummaryComponent extends SummaryComponent<ManualInvoiceSummaryModel> {
  masterModel: ManualInvoiceSummaryMasterModel;
  isFilterOpen: boolean = true;
  selectedPageSize;
  pagesizeitems = PageSizeCommon;
  componentDestroyed$: Subject<boolean> = new Subject();
  private modelRef: NgbModalRef;
  invoicePreviewRequest = new InvoicePreviewRequest();
  @ViewChild('invoicePreviewTemplate') invoicePreviewTemplate: TemplateRef<any>;
  modelRemove: any;
  constructor(
    private manualInvoiceService: ManualInvoiceService,
    private cusService: CustomerService,
    private customerPriceCardService: CustomerPriceCardService,
    private modalService: NgbModal,
    router: Router,
    validator: Validator,
    route: ActivatedRoute,
    translateService: TranslateService,
    toastr: ToastrService,
    utility: UtilityService
  ) {
    super(router, validator, route, translateService, toastr, utility);
    this.validator.setJSON("invoice/manual-invoice-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;

  }

  SearchByStatus(id) {
    this.model.invoiceStatusId = [];
    this.model.invoiceStatusId.push(id);
    this.search();
  }
  onInit(): void {
    this.selectedPageSize = PageSizeCommon[0];
    this.masterModel = new ManualInvoiceSummaryMasterModel();
    this.masterModel.entityId = parseInt(this.utility.getEntityId());
    this.model = new ManualInvoiceSummaryModel();
    this.getBillingToList();
    this.getCustomerListBySearch();
  }
  getData(): void {
    this.masterModel.searchloading = true;
    this.manualInvoiceService
      .getManualInvoiceSummary(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        (res) => {
          if (res && res.result === ResponseResult.Success) {
            this.model.items = res.data;
            this.masterModel.statuslist = res.statusCountList;
          } else {
            this.model.items = [];
            this.model.noFound = true;
          }
          this.mapPageProperties(res);
          this.masterModel.searchloading = false;
        },
        (error) => {
          this.masterModel.searchloading = false;
        }
      );
  }
  getPathDetails(): string {
    return "manualinvoiceedit/manual-invoice-edit";
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  clearDateInput(name) {
    this.model[name] = null;
  }

  getBillingToList() {
    this.masterModel.invoiceToLoading = true;
    this.customerPriceCardService
      .getBillingToList()
      .pipe()
      .subscribe(
        (response) => {
          if (response.result == ResponseResult.Success) {
            this.masterModel.invoiceToList = response.dataSourceList;
          }
          this.masterModel.invoiceToLoading = false;
        },
        (error) => {
          this.setError(error);
          this.showError(
            "INVOICE_MODIFY.LBL_TITLE",
            "COMMON.MSG_UNKNONW_ERROR"
          );
          this.masterModel.invoiceToLoading = false;
        }
      );
  }

  //customer list
  getCustomerListBySearch() {
    if (this.model.customerId)
      this.masterModel.requestCustomerModel.id = this.model.customerId;
    this.masterModel.customerInput
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => (this.masterModel.customerLoading = true)),
        switchMap((term) =>
          this.cusService
            .getCustomerDataSourceList(
              this.masterModel.requestCustomerModel,
              term
            )
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => (this.masterModel.customerLoading = false))
            )
        )
      )
      .subscribe((data) => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.masterModel.requestCustomerModel.searchText =
        this.masterModel.customerInput.getValue();
      this.masterModel.requestCustomerModel.skip =
        this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.cusService
      .getCustomerDataSourceList(this.masterModel.requestCustomerModel)
      .subscribe((customerData) => {
        if (IsVirtual) {
          this.masterModel.requestCustomerModel.skip = 0;
          this.masterModel.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
            this.masterModel.customerList =
              this.masterModel.customerList.concat(customerData);
          }
        }
        this.masterModel.customerLoading = false;
      }),
      (error) => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  deleteManualInvoice(item) {
    this.manualInvoiceService.deleteManualInvoice(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.showSuccess('MANUAL_INVOICE_SUMMARY.LBL_TITLE', item.invoiceNo + ' Deleted Successfully')
            this.refresh();
          }
          else {
            this.error = response.result;

            this.loading = false;
            // TODO check error from result
          }

        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();
  }
  openConfirm(id, invoiceNo, content) {

    this.modelRemove = {
      id: id,
      invoiceNo: invoiceNo
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


    this.manualInvoiceService.exportManulInvoiceSummary(this.model)
      .subscribe(async res => {
        await this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Manual_invoice.xlsx");
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
  Reset() {
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
  IsDateValidationRequired(): boolean {
    return (this.model.fromDate !== undefined && this.model.fromDate !== null) || (this.model.toDate !== undefined && this.model.toDate !== null);
  }

  openTemplatePopUp(manualInvoice) {
    const entityId = this.utility.getEntityId();
    if (Number(entityId) == EntityAccess.AQF) {
      if (manualInvoice.invoicePdfUrl == null) {
        this.showError("MANUAL_INVOICE_SUMMARY.LBL_TITLE", "MANUAL_INVOICE_SUMMARY.LBL_INVOICE_NOT_UPLOADED");
        return;
      }
      window.open(manualInvoice.invoicePdfUrl, '_blank');
      return;
    }
    else {
      if (manualInvoice.invoiceNo && manualInvoice.invoiceNo != "" && manualInvoice.customerId && manualInvoice.customerId > 0) {

        this.invoicePreviewRequest = new InvoicePreviewRequest();
        this.invoicePreviewRequest.customerId = manualInvoice.customerId.toString();
        this.invoicePreviewRequest.invoiceNo = manualInvoice.invoiceNo;
        this.invoicePreviewRequest.invoicePreviewTypes = [InvoicePreviewType.ManualInvoice];
        this.invoicePreviewRequest.invoicePreviewFrom = InvoicePreviewFrom.ManualInvoice;
        this.invoicePreviewRequest.previewTitle = "Invoice Preview";
        this.invoicePreviewRequest.service = APIService.Inspection;

        this.modelRef = this.modalService.open(this.invoicePreviewTemplate,
          {
            windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title',
            centered: true, backdrop: 'static'
          });

      }
    }
  }

  closeInvoicePreview() {
    this.modelRef.close();
  }
}

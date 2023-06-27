import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'src/app/_Services/location/location.service';
import { InvoiceDiscountService } from './../../../_Services/invoice/invoice-discount.service';
import { BehaviorSubject, of } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceDiscountModel } from 'src/app/_Models/invoice/invoice-discount-summary.model';
import { Validator } from '../../common';
import { SummaryComponent } from '../../common/summary.component';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { ListSize, PageSizeCommon } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-invoice-discount-summary',
  templateUrl: './invoice-discount-summary.component.html',
  styleUrls: ['./invoice-discount-summary.component.scss']
})
export class InvoiceDiscountSummaryComponent extends SummaryComponent<InvoiceDiscountModel> {
  model: InvoiceDiscountModel;
  modelRef: NgbModalRef;
  customerList: Array<any>;
  customerLoading: boolean;
  customerInput: BehaviorSubject<string> = new BehaviorSubject<string>("");
  requestCustomerModel: CommonDataSourceRequest;
  searchLoading: boolean;

  invDisTypeLoading: boolean;
  invDisTypeList: Array<any>;

  countryList: Array<any> = [];
  countryLoading: boolean;

  items: Array<any>;
  pagesizeitems = PageSizeCommon;
  selectedPageSize = PageSizeCommon[0];
  deleteId: number;
  constructor(
    private locationService: LocationService,
    private modalService: NgbModal,
    private invoiceDiscountService: InvoiceDiscountService,
    private cusService: CustomerService,
    router: Router, route: ActivatedRoute, public validator: Validator, translate: TranslateService, toastr: ToastrService, utility?: UtilityService) {
    super(router, validator, route, translate, toastr, utility);
  }

  onInit(): void {
    this.model = new InvoiceDiscountModel();
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.validator.setJSON("invoice/invoice-discount-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted=false;    
    this.getInvoiceDiscountTypes();
  }
  ngAfterViewInit() {
    //get the customer details
    this.getCustomerListBySearch();        
  }
  SearchDetails() {
    this.validator.isSubmitted=true;
    if (this.isSummaryFormValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.model.index = 1;
      this.refresh();
    }
  }

  getData(): void {
    this.searchLoading = true;
    this.invoiceDiscountService.getInvoiceDiscountSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result === 1) {
            this.model.items = response.data;
            this.model.index = response.index;
            this.model.pageSize = response.pageSize;
            this.model.totalCount = response.totalCount;
            this.model.pageCount = response.pageCount;
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
          this.error = error;
          this.searchLoading = false;
        });
  }
  getPathDetails(): string {
    return "invoicediscountedit/invoice-discount-edit";
  }


  getCustomerListBySearch() {
    if (this.model.customerId) {
      this.requestCustomerModel.idList.push(this.model.customerId);
    }
    else {
      this.requestCustomerModel.idList = null;
    }
    this.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.customerLoading = true),
      switchMap(term =>
        this.cusService.getCustomerDataSourceList(this.requestCustomerModel, term)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.customerLoading = false))
      ))
      .subscribe(data => {
        if (data && data.length > 0) {
          this.customerList = data;
        }
        this.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.requestCustomerModel.searchText = this.customerInput.getValue();
      this.requestCustomerModel.skip = this.customerList.length;
    }

    this.customerLoading = true;
    this.cusService.getCustomerDataSourceList(this.requestCustomerModel).
      subscribe(customerData => {
        if (IsVirtual) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
            this.customerList = this.customerList.concat(customerData);
          }
        }
        this.customerLoading = false;
      }),
      error => {
        this.customerLoading = false;
        this.setError(error);
      };
  }

  changeCustomerData(event) {
    this.model.countryId = undefined;
    this.countryList = [];
    if (event.id > 0) {
      this.getCountryList();
    }
  }
  getCountryList() {
    this.countryLoading = true;
    let request = new CommonDataSourceRequest();
    request.customerId = this.model.customerId;
    this.invoiceDiscountService.getCustomerBussinessCountries(request)
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.countryList = data.dataSourceList;
          }
          else {
            this.error = data.result;
          }
          this.countryLoading = false;
        },
        error => {
          this.setError(error);
          this.countryLoading = false;
        });

  }
  getInvoiceDiscountTypes() {
    this.invDisTypeLoading = true;
    this.invoiceDiscountService.getInvDisTypes()
      .pipe()
      .subscribe(response => {
        if (response && response.result === 1) {
          this.invDisTypeList = response.dataSourceList;
        }
        this.invDisTypeLoading = false;
      }, error => {
        this.setError(error);
        this.invDisTypeLoading = false;
      });
  }

  openConfirm(id, content) {
    this.deleteId = id;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
  }
  delete() {
    this.invoiceDiscountService.delete(this.deleteId).subscribe(response => {
      if (response && response.result === 1) {
        this.search();
        this.modelRef.dismiss();
      }
    })
  }

  IsDateValidationRequired(): boolean {
    return (this.model.periodFrom !== undefined && this.model.periodFrom !== null) || (this.model.periodTo !== undefined && this.model.periodTo!==null);
  }

  isSummaryFormValid() {
    return this.validator.isValidIf('periodFrom', this.IsDateValidationRequired()) && this.validator.isValidIf('periodTo', this.IsDateValidationRequired())
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "periodFrom": {
        this.model.periodFrom = null;
        break;
      }
      case "periodTo": {
        this.model.periodTo = null;
        break;
      }
    }
  }

  clearCustomer() {
    this.model.customerId = null;    
    this.getCustomerListBySearch();    
  }
}

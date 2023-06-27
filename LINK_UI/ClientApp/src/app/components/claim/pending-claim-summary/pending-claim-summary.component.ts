import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbCalendar, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { PendingClaimSummaryMasterModel, PendingClaimSummaryModel } from 'src/app/_Models/claim/pending-claim-summary.model';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { ClaimService } from 'src/app/_Services/claim/claim.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { Validator } from '../../common';
import { ListSize, PageSizeCommon, SearchType } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';

@Component({
  selector: 'app-pending-claim-summary',
  templateUrl: './pending-claim-summary.component.html',
  styleUrls: ['./pending-claim-summary.component.scss'],
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
export class PendingClaimSummaryComponent extends SummaryComponent<PendingClaimSummaryModel>{

  componentDestroyed$: Subject<boolean> = new Subject();
  masterModel: PendingClaimSummaryMasterModel;
  pagesizeitems = PageSizeCommon;
  selectedPageSize = PageSizeCommon[0];
  searchLoading: boolean = false;
  isFilterOpen = true;
  public _customValidationForInvoiceNo: boolean = false;
  selectedClaimIds = [];
  currenctRouter: any;
  filterDataShown: boolean;
  constructor(
    private customerService: CustomerService, private officeService: OfficeService, private claimService: ClaimService,
    public calendar: NgbCalendar, public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService, public utility: UtilityService, public modalService: NgbModal) {
    super(router, validator, route, translate, toastr, utility);
    this.currenctRouter = router;
  }

  onInit(): void {
    this.model = new PendingClaimSummaryModel();
    this.model.fromDate = this.calendar.getPrev(this.calendar.getToday(), 'm', 6);
    this.model.toDate = this.calendar.getToday();
    this.masterModel = new PendingClaimSummaryMasterModel();
    this.validator.setJSON("claim/pending-claim-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.getOfficeList();
  }
  getData(): void {
    this.searchLoading = true;
    this.model.noFound = false;

    this.claimService.getPendingClaimSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this.model.items = response.data;
            this.masterModel.isAccountingCreditNoteRole = response.isAccountingCreditNoteRole;
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

  ngAfterViewInit() {
    this.getCustomerListBySearch();
  }
  getCustomerListBySearch() {

    //push the customerid to  customer id list 
    if (this.model.customerId) {
      this.masterModel.requestCustomerModel.idList.push(this.model.customerId);
    }
    else {
      this.masterModel.requestCustomerModel.idList = null;
    }

    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.masterModel.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.masterModel.requestCustomerModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.customerLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  getOfficeList() {
    this.masterModel.officeLoading = true;
    this.officeService.getOfficeDetails()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.officeList = response.dataSourceList;
          this.masterModel.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.officeLoading = false;
        });
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.requestCustomerModel.searchText = this.masterModel.customerInput.getValue();
      this.masterModel.requestCustomerModel.skip = this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.masterModel.requestCustomerModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.masterModel.customerList = this.masterModel.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.masterModel.requestCustomerModel.skip = 0;
          this.masterModel.requestCustomerModel.take = ListSize;
        }
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  //validate & set booking no
  BookingNoValidation() {
    return this._customValidationForInvoiceNo = this.model.searchTypeId == SearchType.BookingNo
      && this.model.searchTypeText != null && this.model.searchTypeText.trim() != "" && isNaN(Number(this.model.searchTypeText));
  }

  checkIfSameCustomer() {
    var items = this.model.items.filter(x => x.selected);
    if (items.length > 0) {
      this.model.items.filter(x => x.customerId !== items[0].customerId).forEach(x => {
        x.disabled = true;
      });
    }
    else {
      this.model.items.forEach(x => x.disabled = false);
    }
    this.selectedClaimIds = [];
    for (var i = 0; i < this.model.items.length; i++) {
      if (this.model.items[i].selected == true) {
        this.selectedClaimIds.push(this.model.items[i].claimId);
      }
    }
  }
  createCreditNote() {
    this.currenctRouter.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}`], { queryParams: { claimIds: encodeURI(JSON.stringify(this.selectedClaimIds)) } })

  }

  isSummaryFormValid() {
    return this.validator.isValid('fromDate') && this.validator.isValid('toDate')
  }

  SearchDetails() {
    this.validator.isSubmitted = true;
    if (this.isSummaryFormValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.model.index = 1;
      this.refresh();
    }
  }
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "fromDate": {
        this.model.fromDate = null;
        break;
      }
      case "toDate": {
        this.model.toDate = null;
        break;
      }
    }
  }

  reset() {
    this.onInit();
    this.getCustomerListBySearch();
  }

  SetSearchTypemodel(item) {
    this.model.searchTypeId = item;
    this.masterModel.filterName = item.name;
  }
}

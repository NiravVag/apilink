import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { ClaimStatuses, ClaimSummaryMasterData, ClaimSummaryModel, ClaimToCancel, DataSource } from 'src/app/_Models/claim/claim-summary.model';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { EditOfficeModel } from 'src/app/_Models/office/edit-officemodel';
import { of, Subject } from 'rxjs';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ClaimService } from 'src/app/_Services/claim/claim.service';
import { claimDatetypelst, DefaultDateType, ListSize, PageSizeCommon, SearchType, SupplierType, Url } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ClaimStatus } from 'src/app/_Models/claim/edit-claim.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { ResponseResult } from 'src/app/_Models/common/common.model';

@Component({
  selector: 'app-claim-summary',
  templateUrl: './claim-summary.component.html',
  styleUrls: ['./claim-summary.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '24px',
        'padding-bottom': '24px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class ClaimSummaryComponent extends SummaryComponent<ClaimSummaryModel> {
  componentDestroyed$: Subject<boolean> = new Subject();
  public model: ClaimSummaryModel;
  // public customerList: Array<DataSource>;
  // public supplierList: Array<DataSource>;
  // public factoryList: Array<DataSource>;
  // public officeList: Array<EditOfficeModel>;
  // public statusList: Array<ClaimStatuses>;
  claimMasterData: ClaimSummaryMasterData;
  public searchloading: boolean = false;
  public error: string = "";
  public modelRef: NgbModalRef;
  public modelRemove: ClaimToCancel;
  public _statuslist: any[] = [];
  filterDataShown: boolean;
  isFilterOpen: boolean;
  selectedPageSize;
  public exportDataLoading = false;
  public _customvalidationforbookid: boolean = false;
  Initialloading: boolean = false;
  suploading: boolean = false;
  factloading: boolean = false;
  cancelLoading: boolean;
  _booksearttypeid = SearchType.BookingNo;;
  datetypelst: any = claimDatetypelst;
  public _IsInternalUser: boolean = false;
  isListResultView: boolean;
  toggleFormSection: boolean;
  private currentRoute: Router;
  pagesizeitems = PageSizeCommon;
  public data: any;
  claimStatus = ClaimStatus;
  @ViewChild('scrollableTable') scrollableTable: ElementRef;
  getData(): void {
    this.GetSearchData();
  }

  getPathDetails(): string {
    return 'editClaim/edit-claim';
  }

  constructor(private service: ClaimService, public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService, private serviceLocation: LocationService,
    toastr: ToastrService, public utility: UtilityService, public modalService: NgbModal,
    public customerService: CustomerService, public officeService: OfficeService, private supService: SupplierService) {
    super(router, validator, route, translate, toastr);
    this.currentRoute = router;
    this.isFilterOpen = true;
    this.getIsMobile();
  }

  onInit(): void {
    this.initialize();
    this.validator.setJSON("claim/claim-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
  }


  async initialize() {
    //initialize the objects
    this.model = new ClaimSummaryModel();
    this.data = [];
    this._statuslist = [];
    this.claimMasterData = new ClaimSummaryMasterData();
    this.validator.isSubmitted = false;
    this.Initialloading = false;
    this.model.searchtypeid = 1;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.service.getClaimSummary()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.claimMasterData.statusList = res.statusList;
            this.GetOfficeList();
            this.refreshCountry();
          }
          else {
          }
          this.Initialloading = false;
        },
        error => {
          this.Initialloading = false;
          this.setError(error);
        });
  }

  ngAfterViewInit() {
    //get the customer details
    this.getCustomerListBySearch();
    //get the customer based details
    this.getCustomerBasedDetails(this.model.customerid);
  }
  getCustomerBasedDetails(customerId) {
    if (customerId) {
      this.getSupplierListBySearch();
    }
    else {
    }
  }

  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
  }

  Reset() {
    this.initialize();
    this.getCustomerListBySearch();
  }

  refreshCountry() {
    this.serviceLocation.getCountrySummary()
      .pipe()
      .subscribe(
        result => {
          this.claimMasterData.countryList = result.countryList;
        }
      )
  }

  GetOfficeList() {
    this.claimMasterData.officeLoading = true;
    this.officeService.getOfficeDetails()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.claimMasterData.officeList = response.dataSourceList;
          this.claimMasterData.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.claimMasterData.officeLoading = false;
        });
  }

  getCustomerListBySearch() {

    //push the customerid to  customer id list 
    if (this.model.customerid) {
      this.claimMasterData.requestCustomerModel.idList.push(this.model.customerid);
    }
    else {
      this.claimMasterData.requestCustomerModel.idList = null;
    }

    this.claimMasterData.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.claimMasterData.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.claimMasterData.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.claimMasterData.requestCustomerModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.claimMasterData.customerLoading = false))
      ))
      .subscribe(data => {
        this.claimMasterData.customerList = data;
        this.claimMasterData.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.claimMasterData.requestCustomerModel.searchText = this.claimMasterData.customerInput.getValue();
      this.claimMasterData.requestCustomerModel.skip = this.claimMasterData.customerList.length;
    }

    this.claimMasterData.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.claimMasterData.requestCustomerModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.claimMasterData.customerList = this.claimMasterData.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.claimMasterData.requestCustomerModel.skip = 0;
          this.claimMasterData.requestCustomerModel.take = ListSize;
        }
        this.claimMasterData.customerLoading = false;
      }),
      error => {
        this.claimMasterData.customerLoading = false;
        this.setError(error);
      };
  }

  getSupplierListBySearch() {
    this.claimMasterData.supsearchRequest.supplierIds = [];
    if (this.model.customerid)
      this.claimMasterData.supsearchRequest.customerIds.push(this.model.customerid);
    this.claimMasterData.supsearchRequest.supplierType = SupplierType.Supplier;
    if (this.model.supplierid)
      this.claimMasterData.supsearchRequest.supplierIds.push(this.model.supplierid);
    this.claimMasterData.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.claimMasterData.supLoading = true),
      switchMap(term => term
        ? this.supService.getSupplierDataSource(this.claimMasterData.supsearchRequest, null, term)
        : this.supService.getSupplierDataSource(this.claimMasterData.supsearchRequest, null)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.claimMasterData.supLoading = false))
      ))
      .subscribe(data => {
        this.claimMasterData.supplierList = data;
        this.claimMasterData.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.claimMasterData.supsearchRequest.searchText = this.claimMasterData.supInput.getValue();
    this.claimMasterData.supsearchRequest.skip = this.claimMasterData.supplierList.length;

    this.claimMasterData.supsearchRequest.customerIds.push(this.model.customerid);
    this.claimMasterData.supsearchRequest.supplierType = SupplierType.Supplier;
    this.claimMasterData.supLoading = true;
    this.supService.getSupplierDataSource(this.claimMasterData.supsearchRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.claimMasterData.supplierList = this.claimMasterData.supplierList.concat(customerData);
        }
        this.claimMasterData.supsearchRequest.skip = 0;
        this.claimMasterData.supsearchRequest.take = ListSize;
        this.claimMasterData.supLoading = false;
      }),
      (error: any) => {
        this.claimMasterData.supLoading = false;
      };
  }

  SetSearchTypemodel(searchtype) {
    this.model.searchtypeid = searchtype;
  }
  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype.id;
  }
  SetSearchTypeText(searchtypetext) {
    this.model.advancedSearchtypeid = searchtypetext;
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  GetSearchData() {
    this.searchloading = true;
    this.model.noFound = false;

    this.service.getClaimDataSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this._statuslist = response.claimStatuslst;
            this.model.items = response.data;
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this.model.items = [];
          }
          else {
            this.error = response.result;
          }
          this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.searchloading = false;
        });
  }

  export() {
    this.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "claim_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "claim_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }

  toggleResultContainer() {
    if (!this._IsInternalUser)
      this.isListResultView = !this.isListResultView;
  }
  ChangeCustomer(cusitem: DataSource) {
    // this.model.supplierid = null;
    // if (cusitem != null && cusitem.id != null) {
    //   this.GetSupplierlist(cusitem.id);
    // }
    if (cusitem != null && cusitem.id != null) {
      //clear the list
      this.claimMasterData.supplierList = [];
      this.model.supplierid = null;
      this.getCustomerBasedDetails(cusitem.id);
    }
  }

  getDetails(id) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  RedirectToEdit(claimId) {
    super.getDetails(claimId);
    // let entity: string = this.utility.getEntityName();
    // if (claimId && claimId > 0) {
    //   var editPage = entity + "/" + Url.EditClaim + claimId;
    //   window.open(editPage);
    // }
  }

  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;

    if (this.model.searchtypetext == null || this.model.searchtypetext == "") {
      if (!this.model.fromdate)
        this.validator.isValid('fromdate');

      else if (this.model.fromdate && !this.model.todate)
        this.validator.isValid('todate');
    }
    return isOk;
  }

  formValid(): boolean {
    return !this.BookingNoValidation() && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  clearCustomer() {
    this.model.customerid = null;
    this.model.supplierid = null;
    this.claimMasterData.supsearchRequest.customerIds = [];
    this.claimMasterData.supplierList = [];
  }

  openConfirm(id, claimNo, content) {
    this.modelRemove = {
      id: id,
      claimNo: claimNo
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  cancelClaim(item: ClaimToCancel) {
    this.service.cancelClaim(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            // refresh
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

  clickStatus(id) {
    if (id && id > 0) {

      //if it contains the value
      var isValueExists = this.model.statusidlst.includes(id);

      //open the filter
      if (!this.isFilterOpen)
        this.isFilterOpen = true;
      this.toggleFormSection = false;

      if (isValueExists) {
        this.model.statusidlst = this.model.statusidlst.filter(x => x != id);
        this.model.statusidlst = [...this.model.statusidlst];

      }
      else {
        this.model.statusidlst.push(id);
        this.model.statusidlst = [...this.model.statusidlst];
      }
    }
  }
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "Fromdate": {
        this.model.fromdate = null;
        break;
      }
      case "Todate": {
        this.model.todate = null;
        break;
      }
    }
  }

  changeStatus(id) {
    return this.model.statusidlst.includes(id);
  }
}

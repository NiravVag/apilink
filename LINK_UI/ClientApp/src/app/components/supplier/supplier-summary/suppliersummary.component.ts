import { Component, NgModule } from '@angular/core';
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { SupplierSummaryModel, SupplierSummaryItemModel, supplierToRemove, SupplierSummaryListModel, SupplierMasterData } from '../../../_Models/supplier/suppliersummary.model'
import { first, switchMap, distinctUntilChanged, tap, catchError, debounceTime, takeUntil } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from "@ngx-translate/core";

import { CityDataSourceRequest, CommonDataSourceRequest, CommonSupplierSourceRequest, CountryDataSourceRequest, ProvinceDataSourceRequest } from 'src/app/_Models/common/common.model';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { LocationService } from 'src/app/_Services/location/location.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ListSize } from '../../common/static-data-common';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-supplierSummary',
  templateUrl: './suppliersummary.component.html',
  styleUrls: ['./suppliersummary.component.css'],
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

export class SupplierSummaryComponent extends SummaryComponent<SupplierSummaryModel> {

  public data: any;
  loading = true;
  error = '';
  public model: SupplierSummaryModel;
  public modelRemove: supplierToRemove;
  private modelRef: NgbModalRef;
  public searchIsSupplier = false;
  public isSupplierDetails: boolean = false;
  idCurrentSupplier: number;
  nameList: Array<any> = [];
  private currentUser: UserModel;
  public _searchloader: boolean = false;

  countryList: any;
  countryLoading: boolean;
  countryInput: BehaviorSubject<string>;
  countryRequest: CountryDataSourceRequest;

  supplierList: any = [];
  supplierLoading: boolean;
  supplierInput: BehaviorSubject<string>;
  searchSupplierModel: CommonSupplierSourceRequest;

  summaryModel: SupplierSummaryListModel;
  componentDestroyed$: Subject<boolean> = new Subject();

  isFilterOpen: boolean;
  toggleFormSection: boolean;
  exportDataLoading = false;
  supplierMasterData:SupplierMasterData;
  constructor(private service: SupplierService, public locationService: LocationService, private modalService: NgbModal,
    authService: AuthenticationService, router: Router, route: ActivatedRoute, private customerService: CustomerService,
    public validator: Validator, translate: TranslateService,  public utility: UtilityService,
    toastr: ToastrService) {
    super(router, validator, route, translate, toastr);

    this.validator.setJSON("supplier/supplier-master.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.isFilterOpen = true;

    this.model = new SupplierSummaryModel();
    this.searchSupplierModel = new CommonSupplierSourceRequest();
    this.currentUser = authService.getCurrentUser();
    this.supplierInput = new BehaviorSubject<string>("");
    this.countryInput = new BehaviorSubject<string>("");
    this.countryRequest = new CountryDataSourceRequest();
    this.summaryModel = new SupplierSummaryListModel();
    this.toggleFormSection = false;
    this.supplierMasterData=new SupplierMasterData();
    this.supplierMasterData.entityId=parseInt(this.utility.getEntityId());
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit() {

    this.loading = true;
    this.data = this.service.getSupplierSummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;

            // load supplier list if we have supplier type
            // if (this.model.typeValues && this.model.typeValues.length > 0) {
            //   this.getSupListBySearch();
            // }
            // else {
            //   this.model.suppValues = null;
            // }
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });

    if (this.currentUser.countryId != null) {
      //select default country
      this.countryRequest.countryIds = [this.currentUser.countryId];
      this.model.countryValues = [this.currentUser.countryId];
    }
    //this.getCountryListBySearch();
  }

  ngAfterViewInit() {

    this.getSupListBySearch();
    this.getCountryListBySearch();
    this.getCustomerListBySearch();
    this.getProvinceListBySearch();
    this.getCityListBySearch();
  }

  getPathDetails(): string {
    return this.data.isEdit ? "supplieredit/edit-supplier" : "supplieredit/view-supplier";
  }

  getData() {
    this._searchloader = true;
    this.service.getDataSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {

            this.model.index = response.index;
            this.model.pageSize = response.pageSize;
            this.model.totalCount = response.totalCount;
            this.model.pageCount = response.pageCount;

            this.model.items = response.data.map((x) => {

              var tabItem: SupplierSummaryItemModel = {
                id: x.id,
                countryName: x.countryName,
                name: x.name,
                regionName: x.regionName,
                cityName: x.cityName,
                typeId: x.typeId,
                typeName: x.typeName,
                canBeDeleted: x.canBeDeleted,
                isExpand: false,
                list: []

              }

              if (x.typeId != 1)
                this.searchIsSupplier = true;

              return tabItem;
            });


          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
          this._searchloader = false;
        },
        error => {
          this.loading = false;
          this._searchloader = false;
        });
  }


  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  openConfirm(id, name, content) {

    this.modelRemove = {
      id: id,
      name: name
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  deleteSupplier(item: supplierToRemove) {

    this.service.deleteSupplier(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.showSuccess('SUPPLIER_SUMMARY.TITLE', item.name + ' Deleted Successfully')
            this.refresh();
          }
          else if (response.result == 3) {
            this.showWarning('SUPPLIER_SUMMARY.MSG_CANTDELETE', 'SUPPLIER_SUMMARY.MSG_CANNOTDELETE_SUP')
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


  getDetail(id) {
    this.isSupplierDetails = true;
    this.idCurrentSupplier = id;
  }

  addSupp() {
    this.isSupplierDetails = true;
    this.idCurrentSupplier = null;
  }

  expand(item) {
    item.isLoader = true;
    // let item = this.model.items.filter(x => x.id == id)[0];
    this.service.getChildDataSummary(item.id, item.typeId)
      .pipe()
      .subscribe(
        response => {
          item.isLoader = false;
          if (response && response.result == 1 && response.data && response.data.length > 0) {

            item.list = response.data.map((x) => {
              return {
                id: x.id,
                countryName: x.countryName,
                name: x.name,
                regionName: x.regionName,
                cityName: x.cityName,
                typeId: x.typeId,
                typeName: x.typeName,
                canBeDeleted: x.canBeDeleted

              };

            });


          }
          else if (response && response.result == 2) {
            item.list = [];
          }
          else {
            item.list = [];
          }

        },
        error => {
          item.list = [];
          item.isLoader = false;
        });

    if (item != null)
      item.isExpand = true;
  }

  collapse(id) {
    let item = this.model.items.filter(x => x.id == id)[0];

    if (item != null)
      item.isExpand = false;
  }

  selectSupp(event) {
    if (event && event.length > 0) {
      this.getSupListBySearch();
    }
    else {
      this.supplierList = [];
      this.model.suppValues = null;
    }
  }

  onChangeCountry(event) {
    if (event && event.length > 0) {

      this.summaryModel.provinceRequest = new ProvinceDataSourceRequest();

      this.summaryModel.cityRequest = new CityDataSourceRequest();

      this.getProvinceListBySearch();
      this.getCityListBySearch();

      if (this.model.typeValues && this.model.typeValues.length > 0)
        this.getSupListBySearch();
    }

    else {
      this.countryRequest.countryIds = [];
      this.searchSupplierModel.countryIds = [];
      this.countryList = [];

      this.model.countryValues = [];
      this.model.provinceId = null;
      this.model.cityValues = [];

      this.summaryModel.provinceRequest = new ProvinceDataSourceRequest();

      this.summaryModel.cityRequest = new CityDataSourceRequest();

      this.searchSupplierModel.provinceId = null
      this.searchSupplierModel.searchText = "";
      this.searchSupplierModel.cityIds = [];

      this.getCountryListBySearch();
      this.getProvinceListBySearch();
      this.getCityListBySearch();

      //this.getCountryData(true);
      if (this.model.typeValues && this.model.typeValues.length > 0) {
        this.clearSupplierDetails()
        this.getSupListBySearch();
      }
    }
  }

  clearSupplierDetails() {
    this.supplierList = [];
    this.model.suppValues = null;
    this.searchSupplierModel.id = null;
    this.searchSupplierModel.searchText = "";
    this.getSupListBySearch();
  }


  getSupListBySearch() {
    if (this.model.typeValues && this.model.typeValues.length > 0) {
      this.supplierList = [];

      if (this.model.suppValues && this.model.suppValues > 0)
        this.searchSupplierModel.id = this.model.suppValues;

      if (this.model.countryValues && this.model.countryValues.length > 0)
        this.searchSupplierModel.countryIds = this.model.countryValues;

      if (this.model.typeValues && this.model.typeValues.length > 0)
        this.searchSupplierModel.supplierTypes = this.model.typeValues;

      if (this.model.customerId && this.model.customerId > 0)
        this.searchSupplierModel.customerId = this.model.customerId;

      if (this.model.provinceId && this.model.provinceId > 0)
        this.searchSupplierModel.provinceId = this.model.provinceId;

      if (this.model.cityValues && this.model.cityValues.length > 0)
        this.searchSupplierModel.cityIds = this.model.cityValues;

      this.searchSupplierModel.isRegionalNameChecked = this.model.isRegionalNameChecked;
      this.supplierInput.pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => this.supplierLoading = true),
        switchMap(term => term
          ? this.service.getFactoryOrSupplierList(this.searchSupplierModel, term)
          : this.service.getFactoryOrSupplierList(this.searchSupplierModel)
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => this.supplierLoading = false))
        ))
        .subscribe(data => {
          this.supplierList = data;
          this.supplierLoading = false;
        });
    }
  }

  //fetch the supplier data with virtual scroll
  getSupplierData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.searchSupplierModel.searchText = this.supplierInput.getValue();
      this.searchSupplierModel.skip = this.supplierList.length;
    }

    this.searchSupplierModel.countryIds = this.model.countryValues;
    this.searchSupplierModel.supplierTypes = this.model.typeValues;

    this.searchSupplierModel.isRegionalNameChecked = this.model.isRegionalNameChecked;
    this.supplierLoading = true;
    this.service.getFactoryOrSupplierList(this.searchSupplierModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.supplierList = this.supplierList.concat(data);
        }
        if (isDefaultLoad) {
          // this.searchSupplierModel = new CommonSupplierSourceRequest();
          this.searchSupplierModel.skip = 0;
          this.searchSupplierModel.take = ListSize;
        }
        this.supplierLoading = false;
      }),
      error => {
        this.supplierLoading = false;
        this.setError(error);
      };
  }


  // clearCountry()
  // {
  //   this.countryRequest.countryIds=[]; 
  //   this.model.countryValues=[];
  //   this.countryList=[];
  //   this.getCountryData(true);
  // }


  //fetch the country data with virtual scroll
  getCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.countryRequest.searchText = this.countryInput.getValue();
      this.countryRequest.skip = this.countryList.length;
    }

    this.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.countryList = this.countryList.concat(customerData);
        }
        if (isDefaultLoad)
          this.countryRequest = new CountryDataSourceRequest();
        this.countryLoading = false;
      }),
      error => {
        this.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    if (this.model.countryValues && this.model.countryValues.length > 0)
      this.countryRequest.countryIds = this.model.countryValues;


    this.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.countryLoading = false))
      ))
      .subscribe(data => {
        this.countryList = data;
        this.countryLoading = false;
      });
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  //fetch customer dropdown list
  getCustomerListBySearch() {
    this.summaryModel.customerRequest = new CommonDataSourceRequest();
    if (this.model.customerId && this.model.customerId > 0) {
      this.summaryModel.customerRequest.id = this.model.customerId;
    }
    else {
      this.summaryModel.customerRequest.id = null;
    }
    this.summaryModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.customerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryModel.customerList = data;
        this.summaryModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {

    this.summaryModel.customerRequest.searchText = this.summaryModel.customerInput.getValue();
    this.summaryModel.customerRequest.skip = this.summaryModel.customerList.length;


    this.summaryModel.customerLoading = true;

    this.summaryModel.customerRequest.id = 0;
    this.customerService.getCustomerDataSourceList(this.summaryModel.customerRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.customerList = this.summaryModel.customerList.concat(customerData);
        }
        this.summaryModel.customerRequest = new CommonDataSourceRequest();
        this.summaryModel.customerLoading = false;
      }),
      error => {
        this.summaryModel.customerLoading = false;
        this.setError(error);
      };
  }

  //fetch province dropdown list
  getProvinceListBySearch() {
    if (this.model.countryValues && this.model.countryValues.length > 0) {
      this.summaryModel.provinceRequest.countryIds = this.model.countryValues;
    }
    else {
      this.summaryModel.provinceRequest.countryIds = [];
    }
    if (this.model.provinceId && this.model.provinceId > 0) {
      this.summaryModel.provinceRequest.provinceId = this.model.provinceId;
    }
    else {
      this.summaryModel.provinceRequest.provinceId = null;
    }
    this.summaryModel.provinceInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.provinceLoading = true),
      switchMap(term => term
        ? this.locationService.getProvinceDataSourceList(this.summaryModel.provinceRequest, term)
        : this.locationService.getProvinceDataSourceList(this.summaryModel.provinceRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.provinceLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryModel.provinceList = data;
        this.summaryModel.provinceLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getProvinceData() {

    this.summaryModel.provinceRequest.searchText = this.summaryModel.provinceInput.getValue();
    this.summaryModel.provinceRequest.skip = this.summaryModel.provinceList.length;

    if (this.model.countryValues && this.model.countryValues.length > 0) {
      this.summaryModel.provinceRequest.countryIds = this.model.countryValues;
    }
    else {
      this.summaryModel.provinceRequest.countryIds = [];
    }

    this.summaryModel.provinceLoading = true;

    //this.summaryModel.provinceRequest.provinceId = 0;
    this.locationService.getProvinceDataSourceList(this.summaryModel.provinceRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.summaryModel.provinceList = this.summaryModel.provinceList.concat(data);
        }
        this.summaryModel.provinceRequest = new ProvinceDataSourceRequest();
        this.summaryModel.provinceLoading = false;
      }),
      error => {
        this.summaryModel.provinceLoading = false;
        this.setError(error);
      };
  }

  //fetch city dropdown list
  getCityListBySearch() {
    if (this.model.provinceId && this.model.provinceId > 0) {
      this.summaryModel.cityRequest.provinceId = this.model.provinceId;
    }
    else {
      this.summaryModel.cityRequest.provinceId = null;
    }

    if(this.model.cityValues && this.model.cityValues.length > 0){
      this.summaryModel.cityRequest.cityIds = this.model.cityValues;
    }
    else{
      this.summaryModel.cityRequest.cityIds = [];
    }
    if(this.model.countryValues && this.model.countryValues.length > 0){
      this.summaryModel.cityRequest.countryIds = this.model.countryValues;
    }
    else{
      this.summaryModel.cityRequest.countryIds = [];
    }
    this.summaryModel.cityRequest.countryIds = this.model.countryValues ? this.model.countryValues : [];

    this.summaryModel.cityInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.cityLoading = true),
      switchMap(term => term
        ? this.locationService.getCityDataSourceList(this.summaryModel.cityRequest, term)
        : this.locationService.getCityDataSourceList(this.summaryModel.cityRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.cityLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryModel.cityList = data;
        this.summaryModel.cityLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCityData() {

    this.summaryModel.cityRequest.searchText = this.summaryModel.cityInput.getValue();
    this.summaryModel.cityRequest.skip = this.summaryModel.cityList.length;

    if (this.model.provinceId && this.model.provinceId > 0) {
      this.summaryModel.cityRequest.provinceId = this.model.provinceId;
    }
    else {
      this.summaryModel.cityRequest.provinceId = null;
    }
    if(this.model.countryValues && this.model.countryValues.length > 0){
      this.summaryModel.cityRequest.countryIds = this.model.countryValues;
    }
    else{
      this.summaryModel.cityRequest.countryIds = [];
    }
    this.summaryModel.cityLoading = true;

    this.locationService.getCityDataSourceList(this.summaryModel.cityRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.summaryModel.cityList = this.summaryModel.cityList.concat(data);
        }
        this.summaryModel.cityRequest = new CityDataSourceRequest();
        this.summaryModel.cityLoading = false;
      }),
      error => {
        this.summaryModel.cityLoading = false;
        this.setError(error);
      };
  }

  clearCustomer() {
    this.model.customerId = null;
    this.getSupListBySearch();
    this.getCustomerListBySearch();
  }

  customerChange(item) {
    if (item && item > 0) {
      this.searchSupplierModel.customerId = null;
      this.getSupListBySearch();
      this.getCustomerListBySearch();
    }
  }

  provinceChange(item) {
    if (item && item.id > 0) {
      this.model.cityValues = [];
      this.getSupListBySearch();
      this.getCityListBySearch();
    }
  }

  clearProvince() {
    this.model.cityValues = [];
    this.summaryModel.cityList = [];
    this.model.provinceId = null;

    this.searchSupplierModel.provinceId = null;
    this.searchSupplierModel.cityIds = [];

    this.getProvinceListBySearch();
    this.getCityListBySearch();
    this.getSupListBySearch();
  }

  cityChange(item) {
    if (item && item.length > 0)
    this.getSupListBySearch();
  }

  clearCity() {
    this.model.cityValues = [];
    this.summaryModel.cityList = [];
    this.searchSupplierModel.cityIds = [];
    this.getCityListBySearch();
    this.getSupListBySearch();
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  checkBoxChangeEvent(){
    if(this.searchSupplierModel.searchText != null){
      this.model.suppValues = null;
      this.searchSupplierModel.searchText = "";
    }
    this.getSupListBySearch();
  }

  export() {
    this.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
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
      window.navigator.msSaveOrOpenBlob(blob, "Supplier_factory.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Supplier_factory.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }
}

import { Component, NgModule, Input, Output, EventEmitter } from '@angular/core';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { CustomerContactService } from '../../../_Services/customer/customercontact.service'
import { CustomerProductSummaryModel, CustomerProductItemModel, CustomerProductToRemove } from '../../../_Models/customer/customerproductsummary.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { CommonDataSourceRequest, ProductSubCategory2SourceRequest } from 'src/app/_Models/common/common.model';
import { CustomerProductMaster } from 'src/app/_Models/customer/customerproductmaster.model';
import { ListSize } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-customerstylesummary',
  templateUrl: './customerstylesummary.component.html',
  styleUrls: ['./customerstylesummary.component.css'],
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

export class CustomerStyleSummaryComponent extends SummaryComponent<CustomerProductSummaryModel> {

  public model: CustomerProductSummaryModel;
  public modelRemove: CustomerProductToRemove;
  public modelRef: NgbModalRef;
  initialLoading: boolean = false;
  searchLoading: boolean = false;
  productSubLoading: boolean = false;
  productInternalLoading: boolean = false;
  customerList: Array<any> = [];
  productCategoryList: Array<any> = [];
  productSubCategoryList: Array<any> = [];
  productCategorySub2List: Array<any> = [];
  public currentUser: UserModel;
  public parentID: any;
  public customerValues: any;
  public isEditDetail: boolean;
  public customerID?: string;
  public currentRoute: Router;
  isFilterOpen: boolean;
  public exportDataLoading = false;
  customerProductMaster: CustomerProductMaster;
  requestCustomerModel: CommonDataSourceRequest;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;
  componentDestroyed$: Subject<boolean> = new Subject();

  constructor(public service: CustomerProduct, public customerService: CustomerService, public modalService: NgbModal,
    authService: AuthenticationService,public utility: UtilityService, router: Router, route: ActivatedRoute, public validator: Validator, translate: TranslateService,
    toastr: ToastrService, public routeCurrent: ActivatedRoute, public routerCurrent: Router) {
    super(router, validator, route, translate, toastr);

    this.validator.setJSON("customer/customer-productsummary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;

    this.model = new CustomerProductSummaryModel();
    this.model.isStyle=true;
    this.currentUser = authService.getCurrentUser();
    this.currentRoute = router;
    this.isFilterOpen = true;
    this.customerProductMaster = new CustomerProductMaster();
    this.customerInput = new BehaviorSubject<string>("");
    this.requestCustomerModel = new CommonDataSourceRequest();
  }

  onInit() {
    this.initialLoading = true;
    this.routeCurrent.queryParams.subscribe(
      params => {
        //// (+) before `params.get()` turns the string into a number
        //this.selectedId = +params.get('id');
        //return this.service.getHeroes();
        if (params && params != null && params['param'] != null) {
          this.model = JSON.parse(decodeURI(params['param']));
          // if (this.model != null)
          //   this.refresh();
        }
        if (params && params != null && params['paramParent'] != null) {
          this.paramParent = params['paramParent'];
      }
    }
    );
    this.getCustomerListBySearch();
    this.getProductCategoryList();
  }

  getProductCategoryList() {
    this.service.getProductCategoryList()
      .pipe()
      .subscribe(
        response => {

          if (response && response.result == 1) {
            this.data = response;
            this.productCategoryList = response.productCategoryList;
            if(this.model.productcategoryValue)
            {
              this.loadSubcategory(this.model.productcategoryValue);
            }
          }
          else {
            this.error = response.result;
          }

          this.initialLoading = false;

        },
        error => {
          this.setError(error);
          this.initialLoading = false;
        });
  }

  loadSubcategory(id){
    if (id) {
      this.productSubLoading = true;
      this.service.getProductSubCategoryList(id)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.data = response;
              this.productSubCategoryList = response.productSubCategoryList;
            }
            else {
              this.error = response.result;
            }
            this.productSubLoading = false;
          },
          error => {
            this.setError(error);
            this.productSubLoading = false;
          });
    }
  }

  changeProductCategory(id) {
    this.productSubCategoryList = null;
    this.productCategorySub2List = null;
    var customerID = this.model.customerValue;
    this.model.customerValue = customerID;
    this.model.productSubCategoryValue = null;
    this.model.productCategorySub2s = null;
    if (id) {
      this.productSubLoading = true;
      this.service.getProductSubCategoryList(id)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.data = response;
              this.productSubCategoryList = response.productSubCategoryList;
            }
            else {
              this.error = response.result;
            }
            this.productSubLoading = false;
          },
          error => {
            this.setError(error);
            this.productSubLoading = false;
          });
    }

  }

  changeProductSubCategory(id) {
    this.customerProductMaster.productCategorySub2List = null;
    this.model.productCategorySub2s = null;

    if (id) {
      this.getProductSubCategory2ListBySearch();
    }

  }

  getPathDetails(): string {
    return this.isEditDetail ? "cusstyleedit/edit-customer-style" : "cusstyleedit/edit-customer-style";
  }

  NavigateDetailPage(id) {
    this.validator.isSubmitted = true;

    if (this.formValid()) {
      var data = Object.keys(this.model);
      var currentItem: any = {};

      for (let item of data) {
        if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
          currentItem[item] = this.model[item];
      }
      this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}/${this.model.customerValue}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
    }
  }

  getData() {

    this.model.pageSize = 10;
    if (!this.model.index) {
      this.model.index = 0;
    }
    if (this.model.customerValue) {
      this.searchLoading = true;
      this.service.getCustomerProductSummary(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1 && response.data && response.data.length > 0) {

              this.validator.isSubmitted = true;
              this.model.index = response.index;
              this.model.pageSize = response.pageSize;
              this.model.totalCount = response.totalCount;
              this.model.pageCount = response.pageCount;

              this.model.items = response.data.map((x) => {

                var tabItem: CustomerProductItemModel = {
                  id: x.id,
                  customerName: x.customerName,
                  productID: x.productId,
                  productDescription: x.productDescription,
                  productCategory: x.productCategory,
                  productSubCategory: x.productSubCategory,
                  productCategorySub2: x.productCategorySub2,
                  factoryReference: x.factoryReference,
                  isBooked: x.isBooked,
                  productCategorySub3: null,
                  sampleSize8h: null,
                  timePreparation: null
                }

                return tabItem;
              });


            }
            else if (response && response.result == 2) {
              this.model.noFound = true;
            }
            else {
              this.error = response.result;
              this.searchLoading = false;
              // TODO check error from result
            }
            this.searchLoading = false;
          },
          error => {
            this.error = error;
            this.searchLoading = false;
          });
    }

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
    }, (reason) => {
    });

  }

  deleteCustomerProduct(item: CustomerProductToRemove) {

    this.service.deleteCustomerProduct(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.refresh();
          }
          else if (response && (response.result == 3)) {
            this.showError('CUSTOMER_PRODUCT_SUMMARY.TITLE', 'CUSTOMER_PRODUCT_SUMMARY.MSG_PRODUCT_MAP_INSP');
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

  editDetails(id) {
    var bookedItem = 0;
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }


  getDetail() {   

    this.validator.isSubmitted = true;

    if (this.formValid()) {
      let entity: string = this.utility.getEntityName();
      let Path = "cusstyleedit/edit-customer-style";
      let id = this.model.customerValue + '|add';
      this.routerCurrent.navigate([`/${entity}/${Path}/${id}`]);
    }
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
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Customer Products.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }

  getProductDetail() {
    this.getDetails(0);
  }

  formValid(): boolean {
    return this.validator.isValid('customerValue');
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  //#region ProductSubCategory2 Loading
  //fetch the first 10 productSubCategory2 List on load
  getProductSubCategory2ListBySearch() {
    this.customerProductMaster.productCategorySub2ModelRequest = new ProductSubCategory2SourceRequest();
    if (this.model.productSubCategoryValue)
      this.customerProductMaster.productCategorySub2ModelRequest.productSubCategoryIds.push(this.model.productSubCategoryValue);
    this.customerProductMaster.productCategorySub2Input.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.customerProductMaster.productCategorySub2Loading = true),
      switchMap(term => term
        ? this.service.getProductSubCategory2DataSource(this.customerProductMaster.productCategorySub2ModelRequest, term)
        : this.service.getProductSubCategory2DataSource(this.customerProductMaster.productCategorySub2ModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.customerProductMaster.productCategorySub2Loading = false))
      ))
      .subscribe(data => {
        this.customerProductMaster.productCategorySub2List = data;
        this.customerProductMaster.productCategorySub2Loading = false;
      });
  }

  //fetch the product sub category 2 data with virtual scroll
  getProductSubCategory2Data(isDefaultLoad: boolean) {

    if (this.model.productSubCategoryValue)
      this.customerProductMaster.productCategorySub2ModelRequest.productSubCategoryIds.push(this.model.productSubCategoryValue);

    if (isDefaultLoad) {
      this.customerProductMaster.productCategorySub2ModelRequest.searchText = this.customerProductMaster.productCategorySub2Input.getValue();
      this.customerProductMaster.productCategorySub2ModelRequest.skip = this.customerProductMaster.productCategorySub2List.length;
    }
    this.customerProductMaster.productCategorySub2Loading = true;
    this.service.getProductSubCategory2DataSource(this.customerProductMaster.productCategorySub2ModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.customerProductMaster.productCategorySub2List = this.customerProductMaster.productCategorySub2List.concat(data);
        }
        if (isDefaultLoad)
          this.customerProductMaster.productCategorySub2ModelRequest = new ProductSubCategory2SourceRequest();
        this.customerProductMaster.productCategorySub2Loading = false;
      }),
      error => {
        this.customerProductMaster.productCategorySub2Loading = false;
      };
  }

  getCustomerListBySearch() {
    //push the customerid to  customer id list 
    if (this.model.customerValue) 
    {
      this.requestCustomerModel.idList.push(this.model.customerValue);
    }
    else 
    {
      this.requestCustomerModel.idList = null;
    }

    this.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.requestCustomerModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.customerLoading = false))
      ))
      .subscribe(data => {
        this.customerList = data;
        this.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestCustomerModel.searchText = this.customerInput.getValue();
      this.requestCustomerModel.skip = this.customerList.length;
    }

    this.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.requestCustomerModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.customerList = this.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
        }
        this.customerLoading = false;
      }),
      error => {
        this.customerLoading = false;
        this.setError(error);
      };
  }


  clearCustomer() {
    this.model.customerValue = null;    
    this.getCustomerListBySearch();
  }

  //#endregion

}

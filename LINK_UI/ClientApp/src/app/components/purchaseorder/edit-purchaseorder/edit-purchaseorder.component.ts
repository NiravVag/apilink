import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first, switchMap, distinctUntilChanged, tap, catchError, debounceTime, takeUntil } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { ListSize, PageSizeCommon, SupplierType, UserType } from '../../common/static-data-common'
import { LocationService } from '../../../_Services/location/location.service';
import { CustomerProduct } from '../../../_Services/customer/customerproductsummary.service'
import { PurchaseOrderService } from '../../../_Services/purchaseorder/purchaseorder.service'
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { PurchaseOrderSummaryItemModel, PurchaseOrderListModel } from '../../../_Models/purchaseorder/purchaseordersummary.model';
import { CustomerService } from '../../../_Services/customer/customer.service';
import { CustomerDepartmentService } from '../../../_Services/customer/customerdepartment.service';
import { CustomerBrandService } from '../../../_Services/customer/customerbrand.service';
import { OfficeService } from '../../../_Services/office/office.service';
import { EditPurchaseOrderModel, purchaseOrderDetail, AttachmentFile, purchaseOrderDetailModel, RemovePurchaseOrderDetail, DataSourceResult, PurchaseOrderMaster } from '../../../_Models/purchaseorder/edit-purchaseorder.model';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponentPaging } from '../../common/detail.paging.component';
import { BehaviorSubject, of, Subject, from } from 'rxjs';
import { NgbModalRef, NgbModal, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { CustomerProductToRemove } from 'src/app/_Models/customer/customerproductsummary.model';
import { CommonDataSourceRequest, CountryDataSourceRequest, ProductCategorySourceRequest, SupplierDataSourceRequest, ProductDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ProductScreenCallType } from 'src/app/_Models/customer/editcustomerproduct.model';
import { commonDataSource } from 'src/app/_Models/booking/inspectionbooking.model';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';

@Component({
  selector: 'app-edit-purchaseorder',
  templateUrl: './edit-purchaseorder.component.html',
  styleUrls: ['./edit-purchaseorder.component.css']
})

export class EditPurchaseorderComponent extends DetailComponentPaging<purchaseOrderDetailModel> {
  public poModel: EditPurchaseOrderModel;
  public model: purchaseOrderDetailModel;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  public data: any;
  public modelRemove: CustomerProductToRemove;
  public productRemoveModel: RemovePurchaseOrderDetail;
  initialLoading = false;
  productLoading = false;
  brandLoading = false;
  deptLoading = false;
  supLoading = false;
  savedataloading = false;
  error: '';
  public jsonHelper: JsonHelper;
  public modelRef: NgbModalRef;
  public poDetailValidators: Array<any> = [];
  customerList: Array<any> = [];
  selectedProducts: any;
  departmentList: any;
  officeList: any;
  brandList: any;
  productList: any;
  destinationCountryList: any;
  supplierList: any;
  factoryList: any;
  modalInputData: any;
  fileSize: number;
  uploadLimit: number;
  uploadFileExtensions: string;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  listModel: PurchaseOrderListModel;
  componentDestroyed$: Subject<boolean> = new Subject();

  customerProductList: any;

  supplierDataList: any;
  countryDataList: any;

  productBaseList: any;
  supplierBaseList: any;
  countryBaseList: any;
  public currentUser: UserModel;
  purchaseOrderMaster: PurchaseOrderMaster;

  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public service: PurchaseOrderService,
    public customerService: CustomerService,
    public locationService: LocationService,
    public customerProductService: CustomerProduct,
    public supplierService: SupplierService,
    public customerBrandService: CustomerBrandService,
    public customerDepartmentService: CustomerDepartmentService,
    public officeService: OfficeService,
    public utility: UtilityService,
    public modalService: NgbModal, authService: AuthenticationService) {
    super(router, validator, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("purchaseorder/edit-purchaseorder.valid.json");
    this.validator.setModelAsync(() => this.poModel);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    this._translate = translate;
    this._toastr = toastr;
    this.uploadFileExtensions = 'png,jpg,jpeg,pdf';
    this.listModel = new PurchaseOrderListModel();
    this.currentUser = authService.getCurrentUser();
    this.listModel.isCustomer = this.currentUser.usertype == UserType.Customer;
    this.listModel.isInternalUser = this.currentUser.usertype == UserType.InternalUser;
    this.purchaseOrderMaster = new PurchaseOrderMaster();
  }

  onInit(id?: any) {
    this.selectedPageSize = PageSizeCommon[0];
    this.init(id);
  }

  getViewPath(): string {
    return "poedit/view-purchaseorder";
  }

  getEditPath(): string {
    return "poedit/edit-purchaseorder";
  }
  getData(): void {
    this.GetSearchData();
  }

  ngOnChanges(changes: SimpleChanges) {
    const id: SimpleChange = changes.currentId;
    this.init(id.currentValue);
  }

  isFactory(): boolean {
    return this.poModel != null && this.poModel.id == 1;
  }

  async onChangeCustomer() {
    if (this.poModel.customerId != null) {
      this.clearListParam();
      this.clearPurchaseOrderDetails();
      this.listModel.productRequest.customerIds = [];
      this.listModel.supplierRequest.customerIds = [];

      if (this.poModel.customerId) {
        this.listModel.productRequest.customerIds.push(this.poModel.customerId);
        this.listModel.supplierRequest.customerIds.push(this.poModel.customerId);
      }

      this.productListLoad();
      this.getBrandList(this.poModel.customerId);
      this.getDepartmentList(this.poModel.customerId);
      this.getSupplierListBySearch();
      this.getFactoryListBySearch();
    }
  }

  async productListLoad() {

    var customerProductResponse = await this.customerProductService.getBaseProductDataSource(this.listModel.productRequest);


    if (customerProductResponse.result == DataSourceResult.Success) {
      this.customerProductList = customerProductResponse.dataSourceList;
      this.poDetailValidators.forEach(poDetailValidator => {
        this.getProductListBySearch(poDetailValidator);
      });
    }

  }

  clearCustomer() {
    this.poModel.customerId = null;
    this.poModel.brandId = null;
    this.poModel.departmentId = null;

    this.supplierList = [];
    this.brandList = [];
    this.departmentList = [];
    this.productList = [];
    this.clearPurchaseOrderDetails();
    this.listModel.customerRequest.searchText = "";
    this.getCustomerListBySearch();
  }

  clearPurchaseOrderDetails() {
    for (let item of this.poDetailValidators) {
      item.poDetails.productId = null;
      item.poDetails.productDesc = "";
      item.poDetails.supplierId = null;
      item.poDetails.factoryId = null;
      item.poDetails.factoryList = [];

    }
    this.listModel.productList = null;
    this.listModel.supplierList = null;
    this.poModel.supplierId = null
  }

  onChangeSupplier(item) {
    item.factoryList = [];
    item.factoryId = null;
    // this.listModel.supplierRequest.supplierIds=[];
    // this.listModel.supplierRequest.searchText ="";
    this.getFactoryList(this.poModel.customerId, item);

  }

  getFactoryList(customerId, item) {
    if (customerId && item.supplierId) {
      if (item.supplierId) {
        this.supplierService.GetfactoryBycustomeridsupId(customerId, item.supplierId)
          .pipe()
          .subscribe(
            data => {

              if (data && data.result == 1) {
                item.factoryList = data.data;
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
      }
    }
  }

  //fetch customer dropdown list
  getCustomerListBySearch() {

    if (this.poModel.customerId && this.poModel.customerId > 0) {
      this.listModel.customerRequest.id = this.poModel.customerId;
    }
    else {
      this.listModel.customerRequest.id = null;
    }
    this.listModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.listModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.listModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.listModel.customerRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.listModel.customerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.listModel.customerList = data;
        this.listModel.customerLoading = false;
        this.initialLoading = false;
        this.isCustomerLogin();
      });
  }

  isCustomerLogin() {
    if (this.listModel.isCustomer) {
      this.poModel.customerId = this.currentUser.customerid;
      this.listModel.supplierRequest.customerIds = [];
      this.getSupplierListBySearch();
      this.getFactoryListBySearch();
      this.productListLoad();
    }
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {

    this.listModel.customerRequest.searchText = this.listModel.customerInput.getValue();
    this.listModel.customerRequest.skip = this.listModel.customerList.length;


    this.listModel.customerLoading = true;

    this.listModel.customerRequest.id = 0;
    this.customerService.getCustomerDataSourceList(this.listModel.customerRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.listModel.customerList = this.listModel.customerList.concat(customerData);
        }
        this.listModel.customerRequest = new CommonDataSourceRequest();
        this.listModel.customerLoading = false;
      }),
      error => {
        this.listModel.customerLoading = false;
        this.setError(error);
      };
  }

  getDepartmentList(id) {
    this.deptLoading = true;
    this.customerDepartmentService.getCustomerDepartment(id)
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.departmentList = data.customerDepartmentList;
          }
          else {
            this.error = data.result;
          }

          this.deptLoading = false;

        },
        error => {
          this.setError(error);
          this.deptLoading = false;
        });
  }

  getBrandList(id) {
    this.brandLoading = true;
    this.customerBrandService.getEditCustomerBrand(id)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.brandList = data.customerBrands;
          }
          else {
            this.error = data;
          }

          this.brandLoading = false;

        },
        error => {
          this.setError(error);
          this.brandLoading = false;
        });
  }

  getProductData() {
    this.listModel.productRequest.searchText = this.listModel.productInput.getValue();
    this.listModel.productRequest.skip = this.listModel.productList.length;
    this.listModel.productRequest.customerIds.push(this.poModel.customerId);

    this.listModel.productRequest.productIds = [];
    this.listModel.productLoading = true;
    this.customerProductService.getProductDataSource(this.listModel.productRequest).
      subscribe(productData => {
        if (productData && productData.length > 0) {
          this.listModel.productList = this.listModel.productList.concat(productData);
        }

        this.listModel.productRequest = new ProductDataSourceRequest();
        this.listModel.productRequest.customerIds.push(this.poModel.customerId);
        this.listModel.productLoading = false;
      }),
      error => {
        this.listModel.productLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getProductListBySearch(item) {

    item.poDetails.productRequest = new ProductDataSourceRequest();

    if (this.poModel.customerId)
      item.poDetails.productRequest.customerIds.push(this.poModel.customerId);

    item.poDetails.productLoading = true;

    item.poDetails.productInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => item.poDetails.productLoading = true),
      switchMap(term => term
        ? this.customerProductService.getPOProductListByName(item, term, this.customerProductList)
        : this.customerProductService.getPOProductListByName(item, "", this.customerProductList)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.listModel.productLoading = false))
      ))
      .subscribe(data => {
        item.poDetails.productList = data;
        item.poDetails.productLoading = false;

      });
  }

  //get the po product list with virtual scroll
  getProductListData(item, isDefaultLoad: boolean) {

    item.poDetails.productRequest = new ProductDataSourceRequest();

    item.poDetails.productRequest.productId = item.poDetails.productId;

    if (this.poModel.customerId)
      item.poDetails.productRequest.customerIds.push(this.poModel.customerId);

    if (this.poModel.supplierId)
      item.poDetails.productRequest.supplierIds.push(this.poModel.supplierId);

    if (isDefaultLoad) {
      item.poDetails.productRequest.searchText = item.poDetails.productInput.getValue();
      item.poDetails.productRequest.skip = item.poDetails.productList.length;
    }

    item.poDetails.productLoading = true;

    this.customerProductService.getPOProductListByName(item, "", null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(productData => {

        if (productData && productData.length > 0) {
          item.poDetails.productList = item.poDetails.productList.concat(productData);
          item.poDetails.productLoading = false;
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          item.poDetails.productRequest.skip = 0;
          item.poDetails.productRequest.take = ListSize;
        }
        item.poDetails.productLoading = false;

      }),
      error => {
        item.poDetails.productLoading = true;
        this.setError(error);
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getSupplierListBySearch() {
    this.purchaseOrderMaster.supplierRequest.customerId = this.poModel.customerId;

    //get the supplier data by id
    if (this.currentUser.usertype == UserType.Supplier)
      this.purchaseOrderMaster.supplierRequest.id = this.currentUser.supplierid;

    //get the supplier data by factory id
    if (this.currentUser.usertype == UserType.Factory)
      this.purchaseOrderMaster.supplierRequest.factoryId = this.currentUser.factoryid;

    this.purchaseOrderMaster.supplierRequest.supplierType = SupplierType.Supplier;
    this.purchaseOrderMaster.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.purchaseOrderMaster.supplierLoading = true),
      switchMap(term => term
        ? this.service.getSupplierDataSourceList(this.purchaseOrderMaster.editSupplierList, this.purchaseOrderMaster.supplierRequest, term)
        : this.service.getSupplierDataSourceList(this.purchaseOrderMaster.editSupplierList, this.purchaseOrderMaster.supplierRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.purchaseOrderMaster.supplierLoading = false))
      ))
      .subscribe(data => {

        this.purchaseOrderMaster.supplierList = data;
        this.purchaseOrderMaster.supplierLoading = false;


      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.purchaseOrderMaster.supplierRequest.searchText = this.purchaseOrderMaster.supplierInput.getValue();
    this.purchaseOrderMaster.supplierRequest.skip = this.purchaseOrderMaster.supplierList.length;

    this.purchaseOrderMaster.supplierRequest.customerId = this.poModel.customerId;
    this.purchaseOrderMaster.supplierRequest.supplierType = SupplierType.Supplier;
    //get the supplier by factory id
    if (this.currentUser.usertype == UserType.Factory)
      this.purchaseOrderMaster.supplierRequest.factoryId = this.currentUser.factoryid;

    this.purchaseOrderMaster.supplierLoading = true;
    this.service.getSupplierDataSourceList(null, this.purchaseOrderMaster.supplierRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(SupplierData => {
        if (SupplierData && SupplierData.length > 0) {
          this.purchaseOrderMaster.supplierList = this.purchaseOrderMaster.supplierList.concat(SupplierData);
        }
        this.purchaseOrderMaster.supplierRequest.skip = 0;
        this.purchaseOrderMaster.supplierRequest.take = ListSize;

        this.purchaseOrderMaster.supplierLoading = false;
      }),
      error => {
        this.purchaseOrderMaster.supplierLoading = false;
        this.setError(error);
      };
  }


  clearSupplier() {
    this.purchaseOrderMaster.editSupplierList = [];
    this.getSupplierListBySearch();
  }

  getFactoryListBySearch() {
    this.applyFactoryRelatedFilters();
    this.purchaseOrderMaster.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.purchaseOrderMaster.factoryLoading = true),
      switchMap(term => term
        ? this.service.getFactoryDataSource(this.purchaseOrderMaster.editFactoryList, this.purchaseOrderMaster.factoryRequest, null, term)
        : this.service.getFactoryDataSource(this.purchaseOrderMaster.editFactoryList, this.purchaseOrderMaster.factoryRequest, null)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.purchaseOrderMaster.factoryLoading = false))
      ))
      .subscribe(data => {
        this.purchaseOrderMaster.factoryList = data;
        this.purchaseOrderMaster.factoryLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData() {

    this.applyFactoryRelatedFilters();
    this.purchaseOrderMaster.factoryRequest.searchText = this.purchaseOrderMaster.factoryInput.getValue();
    this.purchaseOrderMaster.factoryRequest.skip = this.purchaseOrderMaster.factoryList.length;
    this.purchaseOrderMaster.factoryLoading = true;
    this.service.getFactoryDataSource(null, this.purchaseOrderMaster.factoryRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.purchaseOrderMaster.factoryList = this.purchaseOrderMaster.factoryList.concat(customerData);
        }
        this.purchaseOrderMaster.factoryRequest.skip = 0;
        this.purchaseOrderMaster.factoryRequest.take = ListSize;
        this.purchaseOrderMaster.factoryLoading = false;
      }),
      (error: any) => {
        this.purchaseOrderMaster.factoryLoading = false;
      };
  }

  clearFactory() {
    this.purchaseOrderMaster.editFactoryList = [];
    this.getFactoryListBySearch();
  }

  applyFactoryRelatedFilters() {
    this.purchaseOrderMaster.factoryRequest.customerIds = [];
    this.purchaseOrderMaster.factoryRequest.supplierIds = [];
    if (this.poModel.customerId)
      this.purchaseOrderMaster.factoryRequest.customerIds.push(this.poModel.customerId);
    this.purchaseOrderMaster.factoryRequest.supplierType = SupplierType.Factory;
  }

  getCountryListDataBySearch(item) {

    item.poDetails.countryRequest = new SupplierDataSourceRequest();

    item.poDetails.countryLoading = true;

    item.poDetails.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => item.poDetails.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryListByName(item, term, this.countryDataList)
        : this.locationService.getCountryListByName(item, "", this.countryDataList)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => item.poDetails.countryLoading = false))
      ))
      .subscribe(data => {

        item.poDetails.countryList = data;
        item.poDetails.countryLoading = false;

      });
  }

  //get the po product list with virtual scroll
  getCountryListData(item, isDefaultLoad: boolean) {


    item.poDetails.countryRequest = new CountryDataSourceRequest();


    if (isDefaultLoad) {
      item.poDetails.countryRequest.searchText = item.poDetails.countryInput.getValue();
      item.poDetails.countryRequest.skip = item.poDetails.countryList.length;
    }

    item.poDetails.countryLoading = true;

    this.locationService.getCountryListByName(item, "", null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(countryData => {

        if (countryData && countryData.length > 0) {
          item.poDetails.countryList = item.poDetails.countryList.concat(countryData);
          item.poDetails.countryLoading = false;
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          item.poDetails.countryRequest.skip = 0;
          item.poDetails.countryRequest.take = ListSize;
        }

      }),
      error => {
        item.poDetails.countryLoading = true;
        this.setError(error);
      };
  }




  getOfficeList() {

    this.officeService.getOfficeSummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.officeList = data.officeList;
          }
          else {
            this.error = data.result;
          }



        },
        error => {
          this.setError(error);

        });
  }

  getCountryData() {
    this.listModel.countryRequest.searchText = this.listModel.countryInput.getValue();
    this.listModel.countryRequest.skip = this.listModel.countryList.length;

    this.listModel.countryLoading = true;
    this.listModel.countryRequest.countryIds = [];
    this.locationService.getCountryDataSourceList(this.listModel.countryRequest).
      subscribe(countryData => {
        if (countryData && countryData.length > 0) {
          this.listModel.countryList = this.listModel.countryList.concat(countryData);
        }

        this.listModel.countryRequest = new CountryDataSourceRequest();
        this.listModel.countryLoading = false;
      }),
      error => {
        this.listModel.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {




    this.listModel.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.listModel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.listModel.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.listModel.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.listModel.countryLoading = false))
      ))
      .subscribe(data => {
        this.listModel.countryList = data;
        this.listModel.countryRequest = new CountryDataSourceRequest();
        this.listModel.countryLoading = false;
        this.initialLoading = false;
      });
  }

  init(id?) {
    this.model = new purchaseOrderDetailModel();
    this.initialLoading = true;
    this.getOfficeList();

    this.poModel = new EditPurchaseOrderModel();
    this.poDetailValidators = [];
    this.data = {};
    this.validator.isSubmitted = false;

    if (id <= 0) {
      this.initialLoading = false;
      this.poModel.purchaseOrderAttachments = [];
      this.getCustomerListBySearch();
      this.getCountryListBySearch();

      this.addPurchaseOrderDetail();
      return
    };
    this.service.getPurchaseOrderDataById(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.processSuccessPOResponse(res, id);
          }
          else if (res && res.result == 2) {
            this.processNotFoundPOResponse(res);
          }
          else {
            this.error = res.result;
          }
          this.initialLoading = false;
        },
        error => {
          this.setError(error);
          this.initialLoading = false;
        });
  }

  async getProductBaseList() {
    this.listModel.productRequest = new ProductDataSourceRequest();

    if (this.poModel.customerId)
      this.listModel.productRequest.customerIds.push(this.poModel.customerId);

    var productResponse = await this.customerProductService.getBaseProductDataSource(this.listModel.productRequest);

    if (productResponse.result == DataSourceResult.Success) {
      if (productResponse.dataSourceList && productResponse.dataSourceList.length > 0)
        this.productBaseList = productResponse.dataSourceList;
    }
  }

  async getSupplierBaseList() {
    this.listModel.supplierRequest = new SupplierDataSourceRequest();

    if (this.poModel.customerId)
      this.listModel.supplierRequest.customerIds.push(this.poModel.customerId);

    var supplierResponse = await this.supplierService.getBaseSupplierDataSource(this.listModel.supplierRequest);

    if (supplierResponse.result == ResponseResult.Success) {
      if (supplierResponse.dataSourceList && supplierResponse.dataSourceList.length > 0)
        this.supplierBaseList = supplierResponse.dataSourceList;
    }
  }

  async getCountryBaseList() {
    this.listModel.countryRequest = new CountryDataSourceRequest();

    var countryResponse = await this.locationService.getBaseCountryDataSource(this.listModel.countryRequest);

    if (countryResponse.result == ResponseResult.Success) {
      if (countryResponse.dataSourceList && countryResponse.dataSourceList.length > 0)
        this.countryBaseList = countryResponse.dataSourceList;
    }
  }

  clearListParam() {
    this.listModel.countryRequest.countryIds = [];
    this.listModel.supplierRequest.supplierIds = [];
    this.listModel.productRequest.productIds = [];
    this.listModel.supplierRequest.searchText = ""
    this.listModel.productRequest.searchText = ""
  }


  async GetSearchData() {
    this.initialLoading = true;
    this.poDetailValidators = [];
    this.clearListParam();
    this.service.SearchPurchaseOrderDetails(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this.model.items = response.data.map((x) => {
              var poDetail: purchaseOrderDetail = {
                id: x.id,
                poId: x.poId,
                productId: x.productId,
                productName: null,
                bookingStatus: x.bookingStatus,
                productDesc: x.productDesc,
                destinationCountryId: x.destinationCountryId,
                etd: x.etd,
                active: x.active,
                quantity: x.quantity,
                factoryId: x.factoryId,
                factoryReference: x.factoryReference,
                supplierId: x.supplierId,
                factoryList: null,
                isBooked: x.isBooked,

                productList: null,
                productInput: new BehaviorSubject<string>(""),
                productLoading: false,
                productRequest: new ProductDataSourceRequest(),

                /*  supplierList: null,
                 supplierInput: new BehaviorSubject<string>(""),
                 supplierLoading: false,
                 supplierRequest: null, */

                countryList: null,
                countryInput: new BehaviorSubject<string>(""),
                countryLoading: false,
                countryRequest: null

              };

              this.getFactoryList(this.poModel.customerId, poDetail);

              if (poDetail.destinationCountryId && poDetail.destinationCountryId > 0) {
                this.listModel.countryRequest.countryIds.push(poDetail.destinationCountryId);
              }
              if (poDetail.supplierId && poDetail.supplierId > 0) {
                this.listModel.supplierRequest.supplierIds.push(poDetail.supplierId);
              }
              if (poDetail.productId && poDetail.productId > 0) {
                this.listModel.productRequest.productIds.push(poDetail.productId);
              }



              var poDetailValidator = { poDetails: poDetail, validator: Validator.getValidator(poDetail, "purchaseorder/edit-purchaseorderdetails.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };


              this.mapAssingProductData(x, poDetailValidator);


              this.mapAssingCountryData(x.destinationCountryId, x.destinationCountryName, poDetailValidator);



              this.poDetailValidators.push(poDetailValidator);
              return poDetail;
            });
            this.poModel.purchaseOrderDetails = this.model.items;
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;

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

  async mapAssingProductData(data, poDetail) {

    this.customerProductList = this.productBaseList;

    if (this.customerProductList.filter(x => x.id == data.productId).length <= 0) {
      var product = { "id": data.productId, "productId": data.productName, 'productDescription': data.productDesc };
      this.customerProductList.push(product);
    }

    this.getProductListBySearch(poDetail);
  }


  appendSupplierData(id, name) {
    if (this.supplierDataList.filter(x => x.id == id).length <= 0) {
      var supplier = { "id": id, "name": name };
      this.supplierDataList.push(supplier);
      this.supplierDataList = [...this.supplierDataList];
    }
  }

  async mapAssingCountryData(id, name, poDetail) {
    this.countryDataList = this.countryBaseList;
    var country = { "id": id, "name": name };
    var countryData = this.countryDataList.find(x => x.id == id);
    if (!countryData)
      this.countryDataList.push(country);
    this.countryDataList = [...this.countryDataList];

    this.getCountryListDataBySearch(poDetail);
  }


  GetPOAttachments() {
    this.initialLoading = true;
    this.poDetailValidators = [];
    this.poModel.purchaseOrderAttachments = [];
    this.service.GetPOattachments(this.model.id)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.poModel.purchaseOrderAttachments = response.data;
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;

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

  //in edit purchase order success response
  async processSuccessPOResponse(res, id) {
    this.data = res;
    this.poDetailValidators = [];
    if (id) {
      //map the purchase order model data
      this.poModel = this.mapModel(res.purchaseOrderData);
      //assign the supplier ids and mapped supplier data
      if (res.purchaseOrderData.supplierData) {
        this.poModel.supplierIds = res.purchaseOrderData.supplierData.map(x => x.id);
        this.purchaseOrderMaster.editSupplierList = res.purchaseOrderData.supplierData;
      }
      //assign the factory ids and mapped factory data
      if (res.purchaseOrderData.factoryData) {
        this.poModel.factoryIds = res.purchaseOrderData.factoryData.map(x => x.id);
        this.purchaseOrderMaster.editFactoryList = res.purchaseOrderData.factoryData;
      }

      this.model.id = id;
      this.getCustomerListBySearch();
      this.getCountryListBySearch();
      this.getSupplierListBySearch();
      this.getFactoryListBySearch();

      await this.getProductBaseList();
      await this.getCountryBaseList();

      this.GetSearchData();
      this.GetPOAttachments();
    }
  }



  processNotFoundPOResponse(res) {
    this.poDetailValidators = [];
    if (res.purchaseOrderData == null || res.purchaseOrderData == 0)
      this.addPurchaseOrderDetail();
  }

  mapModel(purchaseOrder: any): EditPurchaseOrderModel {

    if (purchaseOrder != null && purchaseOrder.customerId != null) {
      this.poModel.customerId = purchaseOrder.customerId;

      //this.getProductListBySearch();

      this.getBrandList(purchaseOrder.customerId);
      this.getDepartmentList(purchaseOrder.customerId);

      //this.getSupplierListBySearch();
    }
    var model: EditPurchaseOrderModel = {
      id: purchaseOrder.id,
      pono: purchaseOrder.pono,
      officeId: purchaseOrder.officeId,
      customerId: purchaseOrder.customerId,
      supplierIds: null,
      factoryIds: null,
      departmentId: purchaseOrder.departmentId,
      brandId: purchaseOrder.brandId,
      internalRemarks: purchaseOrder.internalRemarks,
      customerRemarks: purchaseOrder.customerRemarks,
      active: purchaseOrder.active,
      createdBy: purchaseOrder.createdBy,
      createdTime: purchaseOrder.createdTime,
      purchaseOrderAttachments: [],
      customerReferencePo: purchaseOrder.customerreferencePO,
      accessType: 1,
      supplierId: null,
      destinationCountryId: null,
      etd: null,
      purchaseOrderDetails: []
    };
    return model;
  }


  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    for (let item of this.poDetailValidators)
      item.validator.isSubmitted = true;


    if (this.isFormValid()) {
      this.savedataloading = true;
      var attachedList = [];
      if (this.poModel.purchaseOrderAttachments)
        attachedList = this.poModel.purchaseOrderAttachments.filter(x => x.isNew);


      this.poDetailValidators.forEach(item => {
        item.poDetails.productInput = null;
        item.poDetails.countryInput = null;
        item.poDetails.supplierInput = null;

      });

      this.service.savePurchaseOrder(this.poModel)
        .subscribe(
          res => {

            if (res && res.result == 1) {
              if (attachedList.length > 0) {
                this.service.uploadAttachedFiles(res.id, attachedList).subscribe(Isupload => {
                  if (Isupload) {
                    this.showSuccess('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.SAVE_OK');
                    if (this.fromSummary && this.poModel.id != 0)
                      this.return('posearch/purchaseorder-summary');
                    else
                      this.init(0);
                  }
                },
                  error => { });
              }
              else {
                this.showSuccess('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.SAVE_OK');
                if (this.fromSummary && this.poModel.id != 0)
                  this.return('posearch/purchaseorder-summary');
                else
                  this.init(0);
              }
              this.savedataloading = false;
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CANNOT_ADDPURCHASEORDER');
                  break;
                case 3:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CURRENTPO_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_PO_EXISTS');
                  break;
                case 5:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_DUPLICATE_PRODUCTS');
                  break;
              }
              this.savedataloading = false;
              //this.waitingService.close();
            }
          },
          error => {
            this.savedataloading = false;
            this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_UNKNONW_ERROR');
            //this.waitingService.close();
          });
    }
  }


  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }


  selectFiles(event) {
    if (event && !event.error && event.files) {

      if (event.files.length == null)
        this.poModel.purchaseOrderAttachments = [];

      if (this.poModel.purchaseOrderAttachments.length + 1 > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else if (event.files.length > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {

        for (let file of event.files) {
          let fileItem: AttachmentFile = {
            fileName: file.name,
            file: file,
            isNew: true,
            id: 0,
            mimeType: "",
            uniqueld: this.newGuid()
          };
          this.poModel.purchaseOrderAttachments.push(fileItem);
        }
      }

    }
    else if (event && event.error && event.errorMessage) {
      this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', event.errorMessage);
    }
  }

  removeAttachment(index) {
    this.poModel.purchaseOrderAttachments.splice(index, 1);
  }
  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  getFile(file: AttachmentFile) {
    if (!file.isNew) {
      this.service.getFile(file.id)
        .subscribe(res => {
          this.downloadFile(res, file.mimeType);
        },
          error => {
            this.showError("EDIT_PURCHASEORDER.TITLE", "EDIT_PURCHASEORDER.MSG_UNKNOWN_ERROR");
          });
    }
    else {
      const url = window.URL.createObjectURL(file.file);
      window.open(url);
    }
  }
  downloadFile(data, mimeType) {
    let navigator: any = window.navigator;
    const blob = new Blob([data], { type: mimeType });
    if (navigator && navigator.msSaveOrOpenBlob) {
      navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }


  removePurchaseOrderDetail(index, id, content) {
    if (id <= 0) {
      this.poModel.purchaseOrderDetails.splice(index, 1);
      this.poDetailValidators.splice(index, 1);
      return;
    }
    this.productRemoveModel = {
      id: id,
      accessType: this.poModel.accessType
    }
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });

  }
  RemovePoDetailConfirm(itemModel: RemovePurchaseOrderDetail) {

    this.initialLoading = true;
    this.service.RemovePurchaseOrderDetail(itemModel)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.showSuccess('EDIT_PURCHASEORDER.TITLE', 'COMMON.MSG_DELETE_SUCCESS')

            this.model.items.forEach(element => {
              element.productList = [];
              element.productInput = null;
              element.countryList = [];
              element.countryInput = null;
            });
            this.GetSearchData();
          }
          else if (response && response.result == 2) {
            this.showWarning('SUPPLIER_SUMMARY.MSG_CANTDELETE', 'EDIT_PURCHASEORDER.MSG_CANNOT_DELETE')
          }
          else {
            this.error = response.result;
          }
          this.initialLoading = false;
          this.modelRef.close()
        },
        error => {
          this.setError(error);
          this.initialLoading = false;
        });


  }

  openAddProduct(id, name, content) {
    this.modelRemove = {
      id: id,
      name: name
    };

    this.modalInputData = {
      typeId: id,
      customerId: this.poModel.customerId,
      supplierId: this.poModel.customerId,
      productScreenCallType: ProductScreenCallType.PurchaseOrder
    };
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.modelRef.result.then((result) => {

    }, (reason) => {

      if (this.poModel.customerId) {
      }
    });

  }

  checkDuplicatesProducts() {
    this.selectedProducts = Object.assign([], this.productList);

    this.poDetailValidators.forEach(element => {
      if (element.poDetails.productId != null) {
        this.selectedProducts = this.selectedProducts.filter(x => x.id != element.poDetails.productId);
      }
    });

  }

  changeProduct(item, event) {

    item.productDesc = event.productDescription;

    this.mapProductToSupplierCheck(item);
    this.listModel.productRequest.productIds = [];
    this.listModel.productRequest.searchText = "";

  }
  clearProduct(item) {
    item.poDetails.productDesc = "";
    this.listModel.productRequest.productIds = [];
    this.listModel.productRequest.searchText = "";
    this.listModel.productRequest.customerIds = [];
    this.listModel.productRequest.customerIds.push(this.poModel.customerId);
    this.getProductListBySearch(item);
  }

  /* clearSupplierData(item) {
    item.supplierRequest.supplierIds = [];
    item.supplierRequest.searchText = "";
    item.supplierRequest.customerIds = [];
    item.supplierRequest.customerIds.push(this.poModel.customerId);
    this.getSupplierListDataBySearch(item);
  } */

  async addPurchaseOrderDetail() {
    var poDetail: purchaseOrderDetail = {
      id: 0,
      poId: 0,
      productId: null,
      productName: null,
      bookingStatus: 3,
      productDesc: "",
      destinationCountryId: null,
      etd: null,
      active: true,
      quantity: null,
      factoryId: null,
      factoryReference: "",
      supplierId: null,
      factoryList: null,
      isBooked: false,

      productList: null,
      productInput: new BehaviorSubject<string>(""),
      productLoading: false,
      productRequest: null,

      /* supplierList: null,
      supplierInput: new BehaviorSubject<string>(""),
      supplierLoading: false,
      supplierRequest: null, */

      countryList: null,
      countryInput: new BehaviorSubject<string>(""),
      countryLoading: false,
      countryRequest: null
    };


    if (this.poModel.destinationCountryId)
      poDetail.destinationCountryId = this.poModel.destinationCountryId;

    if (this.poModel.etd)
      poDetail.etd = this.poModel.etd;

    this.poModel.purchaseOrderDetails.push(poDetail);
    var poItem = { poDetails: poDetail, validator: Validator.getValidator(poDetail, "purchaseorder/edit-purchaseorderdetails.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };
    
    this.poDetailValidators.push(poItem);

    this.customerProductList=[];
    this.getProductListBySearch(poItem);

    this.listModel.countryRequest.countryIds = [];
    this.listModel.supplierRequest.supplierIds = [];
    this.listModel.productRequest.productIds = [];
    this.listModel.productRequest.searchText = "";





    if (this.poModel.destinationCountryId) {
      await this.getCountryBaseList();
      var commonCountryData = this.listModel.countryList.find(x => x.id == this.poModel.destinationCountryId);
      this.mapAssingCountryData(commonCountryData.id, commonCountryData.name, poItem);
      poItem.poDetails.destinationCountryId = commonCountryData.id;
    }
    else
      this.getCountryListDataBySearch(poItem);



    if (this.poModel.customerId) {
      this.getProductListBySearch(poItem);
    }


  }

  isFormValid() {
    var isOk = this.validator.isValid('customerId')
      && this.validator.isValid('pono')
      && this.validator.isValid('supplierIds')
      && this.poDetailValidators.every((x) => x.validator.isValid('productId')
        && x.validator.isValid('quantity'));
    return isOk;

  }

  mapProductToSupplierCheck(item) {
    if (item && item.poDetails && item.poDetails.productId && item.poDetails.supplierId) {
      var poDetails = this.poDetailValidators.filter(x => x.poDetails.productId == item.poDetails.productId &&
        x.poDetails.supplierId == item.poDetails.supplierId);
      if (poDetails && poDetails.length > 1) {
        this.showWarning('EDIT_PURCHASEORDER.SAVE_RESULT', "EDIT_PURCHASEORDER.MSG_DUPLICATE_PRODUCT_EXISTS");
      }
    }
  }

  applySupplierAndFactoryList(supplierData, poDetail) {
    this.supplierService.GetfactoryBycustomeridsupId(this.poModel.customerId, supplierData.id)
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.processSuccessFactoryList(supplierData, data.data, poDetail);

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
  }

  processSuccessFactoryList(supplierData, data, poDetail) {

    this.poDetailValidators.forEach(element => {
      if (element.poDetails.supplierList) {
        var supplierInfo = element.poDetails.supplierList.find(x => x.id == supplierData.id);
        if (!supplierInfo)
          element.poDetails.supplierList.push(supplierData);
        element.poDetails.supplierList = [...element.poDetails.supplierList];
      }
    });

    if (!poDetail) {

      this.poDetailValidators.forEach(element => {
        if (element.poDetails.supplierId == null) {
          element.poDetails.supplierId = supplierData.id;
          element.poDetails.factoryList = data;
        }
      });

    }
    else if (poDetail) {
      poDetail.factoryList = data;
    }


  }

  applySupplierToPODetail(item) {

    this.applySupplierAndFactoryList(item, null);

  }

  applyCountryToPODetail(item) {

    var country = { "id": item.id, "name": item.name };
    this.poDetailValidators.forEach(element => {

      if (element.poDetails.countryList) {
        var countryData = element.poDetails.countryList.find(x => x.id == item.id);
        if (!countryData)
          element.poDetails.countryList.push(country);
        element.poDetails.countryList = [...element.poDetails.countryList];
      }

      if (element.poDetails.destinationCountryId == null) {
        element.poDetails.destinationCountryId = item.id;
      }
    });

  }

  applyETDToPODetail(date) {

    this.poDetailValidators.forEach(element => {
      if (element.poDetails.etd == null) {
        element.poDetails.etd = new NgbDate(date.year, date.month, date.day);
      }
    });

  }
  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }

  changePoProduct() {

  }
}

import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { EditCustomerProductModel, AttachmentFile, ProductScreenCallType, BookingProductData } from '../../../_Models/customer/editcustomerproduct.model';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ReturnStatement } from '@angular/compiler';
import { UserType, FileUploadResponseResult, FileContainerList, RoleEnum, MSChartFileType, Service, RightsEnum, EntPageFieldAccessEnum, Size, Description, Code, RequiredValue, TolerancePlus, ToleranceMinus } from '../../common/static-data-common'
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgxGalleryOptions, NgxGalleryImage } from 'ngx-gallery-9';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { FileInfo } from 'src/app/_Models/fileupload/fileupload';
import { ReferenceService } from '../../../_Services/reference/reference.service';
import { APIService } from "src/app/components/common/static-data-common";
import { CustomerProductMaster } from 'src/app/_Models/customer/customerproductmaster.model';
import { of, Subject } from 'rxjs';
import { ProductSubCategory2SourceRequest, ProductSubCategory3SourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { WorkLoadMatrixService } from 'src/app/_Services/workloadmatrix/workloadmatrix.service';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { EntPageFieldAccessResult, EntPageRequest } from 'src/app/_Models/booking/inspectionbooking.model';
import { Ocr, OcrMasterModel, OcrTableItem, OcrTableRequest } from 'src/app/_Models/customer/ocr-master-model';
import { ProductMSChartMasterModel } from 'src/app/_Models/customer/product-mschart-master-model';
@Component({
  selector: 'app-editCustomerProduct',
  templateUrl: './editcustomerproduct.component.html',
  styleUrls: ['./editcustomerproduct.component.css']
})
export class EditCustomerProductComponent extends DetailComponent {
  public model: EditCustomerProductModel;
  public data: any;
  public modelRef: NgbModalRef;
  initialLoading = false;
  savedataloading = false;
  downloadloading = false;
  productSubLoading: boolean = false;
  productInternalLoading: boolean = false;
  loading = false;
  error: '';
  public selectCounryId: number = 0;
  public isDetails: boolean = true;
  private jsonHelper: any;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public addressValidators: Array<any> = [];
  public contactValidators: Array<any> = [];
  public action: string;
  public productID: number;
  public callFrom: number;
  public recentUploads: Array<AttachmentFile>;
  customerList: Array<any> = [];
  productCategoryList: Array<any> = [];
  productSubCategoryList: Array<any> = [];
  productCategorySub2List: Array<any> = [];
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  public productGalleryOptions: NgxGalleryOptions[];
  public productGalleryImages: NgxGalleryImage[];
  apiServiceList: any;
  apiServiceEnum = APIService;
  public _roleEnum = RoleEnum;
  customerProductMaster: CustomerProductMaster;
  productScreenCallType = ProductScreenCallType;
  productSubCat3Exists: boolean = false;
  tpUpdated: boolean = false;
  uploadFileExtensions: string;
  uploadFileLimit: string = "10";
  msChartFileType = MSChartFileType;
  fileuploadLoader: boolean = false;
  fileTypeId: number;
  fileTypeList: any;
  fileTypeLoading: boolean;
  componentDestroyed$: Subject<boolean> = new Subject();
  msChartFileList: Array<any> = [];
  msChartDDLFileList: Array<any> = [];
  msChartStyleCodeList: Array<any> = [];
  msStyleCode: string;
  msFileId: number;

  entPageFieldAccess: any;
  filterEntFieldAccess: any;

  _productSubCategory2Visible: boolean = false;

  entPageFieldAccessEnum = EntPageFieldAccessEnum;

  @Input() modelData: any;

  @Output("closeProductPopupFromBooking") closeProductPopupFromBooking: EventEmitter<any> = new EventEmitter();

  bookingProductList: Array<EditCustomerProductModel>;
  ocrMasterModel: OcrMasterModel;
  productMSChartMasterModel: ProductMSChartMasterModel;
  fileIndex: number;
  uploadedFileCountByType: number;
  errorFlag: boolean;
  errorMessage: string;
  fileLimit: number;
  fileCountErrorFlag: boolean;
  fileArray: Array<any>;
  responseObject: object;
  fileSizeLimit: number;
  fileSizeErrorFlag: boolean;
  @Output() fileUpload = new EventEmitter();
  @ViewChild('fileHandler', { static: true }) fileHandler: ElementRef;
  constructor(

    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    jsonHelper: JsonHelper,
    public service: CustomerProduct,
    public modalService: NgbModal,
    public customerService: CustomerService,
    public fileStoreService: FileStoreService,
    public utility: UtilityService,
    authserve: AuthenticationService,
    public referenceService: ReferenceService,
    private fileService: FileStoreService,
    public workLoadMatrixService: WorkLoadMatrixService, private bookingService: BookingService) {
    super(router, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this._translate = translate;
    this._toastr = toastr;
    this.jsonHelper = jsonHelper;
    this.productGalleryOptions = [
      {
        width: '800px',
        height: '500px',
        "preview": false
      },
      { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
      { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 }
    ];
    this.productGalleryImages = [];
    this.responseObject = {
      error: true,
      errorMessage: '',
      files: []
    }
    this.customerProductMaster = new CustomerProductMaster();
    this.ocrMasterModel = new OcrMasterModel();
    this.productMSChartMasterModel = new ProductMSChartMasterModel();
    this.bookingProductList = new Array<EditCustomerProductModel>();
    // router.events.subscribe(data => {
    //   if (data instanceof NavigationStart) {
    //     let id = route.snapshot.paramMap.get("id");
    //     this.onInit(id);
    //   }
    // });
  }

  onInit(id?: any, type?: any) {
    this.getAPIServices();
    this.model = new EditCustomerProductModel();

    //default set call from to product screen
    this.callFrom = this.productScreenCallType.Product;

    if (this.modelData) {
      this.model.customerID = this.modelData.customerId;

      if (this.modelData.productScreenCallType && this.modelData.productScreenCallType != this.productScreenCallType.Product)
        this.callFrom = this.modelData.productScreenCallType;

      if (this.callFrom != this.productScreenCallType.Product)
        id = 0;
    }


    this.getEntPageFieldAccessList();


    this.productID = id;
    this.validator = Validator.getValidator(this.model, "customer/edit-customerproduct.valid.json", this.jsonHelper, false, this._toastr, this._translate);
    this.validator.isSubmitted = false;
    this.init(id);
  }

  getViewPath(): string {
    return "cusproductedit/view-customer-product";
  }

  getEditPath(): string {
    return "cusproductedit/edit-customer-product";
  }

  async getEntPageFieldAccessList() {
    var entPageRequest = new EntPageRequest();

    entPageRequest.serviceId = Service.Inspection;
    entPageRequest.rightId = RightsEnum.CustomerProduct;

    var entPageFieldAccessResponse = await this.bookingService.getEntPageAccessList(entPageRequest);

    if (entPageFieldAccessResponse.result == EntPageFieldAccessResult.Success) {
      this.entPageFieldAccess = entPageFieldAccessResponse.entPageFieldAccess;
      this.filterEntFieldAccess = this.entPageFieldAccess;
    }


  }

  init(id?) {
    this.initialLoading = true;
    this.getCustomerList();
    this.getProductCategoryList();
    this.getUnitList();
    this.getFileTypeList();


    this.model.apiServiceIds = [];
    this.model.apiServiceIds.push(this.apiServiceEnum.Inspection);

    if (id) {
      if (id != 0) {
        this.service.getEditCustomerProduct(id)
          .pipe()
          .subscribe(
            res => {
              if (res && res.result == 1) {
                this.data = res;
                if (id) {
                  this.model = this.mapModel(res.customerProductDetails);
                  if (this.model.productSubCategory) {
                    this.loadProductSubCategory(this.model.productCategory);
                  }
                  if (this.model.productSubCategory) {

                    this.getProductSubCategory2ListBySearch();
                  }
                  if (this.model.productCategorySub2) {
                    this.getProductSubCategory3ListBySearch();
                  }
                  this.validator.setModel(this.model);
                  this.initialLoading = false;
                }

              }
              else {
                this.initialLoading = false;
                this.error = res.result;
              }
              this.initialLoading = false;
            },
            error => {
              this.setError(error);
              this.initialLoading = false;
              this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
            });
      }
      else {

        this.validator.isSubmitted = false;
        this.initialLoading = false;
      }
    }
    else {
      this.initialLoading = false;
    }

  }

  getCustomerList() {
    this.customerService.getCustomerSummary()
      .pipe()
      .subscribe(
        response => {

          if (response && response.result == 1) {
            this.data = response;
            this.customerList = response.customerList;
          }
          else {
            this.error = response.result;
          }

        },
        error => {
          this.setError(error);
          this.initialLoading = false;
          this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
        });
  }

  getProductCategoryList() {
    this.service.getProductCategoryList()
      .pipe()
      .subscribe(
        response => {

          if (response && response.result == 1) {
            this.data = response;
            this.productCategoryList = response.productCategoryList;
          }
          else {
            this.error = response.result;
          }

        },
        error => {
          this.setError(error);
          this.initialLoading = false;
          this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
        });

  }

  changeProductSubCategory(productCategory) {
    this.model.productSubCategory = null;
    this.model.productCategorySub2 = null;
    this.model.productCategorySub3 = null;
    this.model.productCategoryName = productCategory.name;
    this.loadProductSubCategory(productCategory.id);
  }

  loadProductSubCategory(id) {
    this.productSubCategoryList = null;
    this.productCategorySub2List = null;

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
            this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
          });
    }
  }

  changeProductSubCategory2(productSubCategory) {
    this.model.productCategorySub2 = null;
    this.customerProductMaster.productCategorySub2List = null;
    this.model.productCategorySub3 = null;
    this.customerProductMaster.productCategorySub3List = null;
    this.model.productSubCategoryName = productSubCategory.name;
    this.getProductSubCategory2ListBySearch();
  }

  loadProductSubCategory3(productSubCategory2) {
    this.model.productCategorySub2Name = productSubCategory2.name;
    this.model.productCategorySub3 = null;
    this.customerProductMaster.productCategorySub3List = null;

    this.getProductSubCategory3ListBySearch();
  }

  changeProductSubCategory3(productSubCategory3) {
    this.model.productCategorySub3Name = productSubCategory3.name;
    this.loadWorkLoadMatrixData();
  }

  loadProductCategorySub2(id) {
    this.productCategorySub2List = null;
    if (id) {
      this.productInternalLoading = true;
      this.service.getProductCategorySub2(id)
        .pipe()
        .subscribe(
          response => {

            if (response && response.result == 1) {
              this.data = response;
              this.productCategorySub2List = response.productCategorySub2List;
            }
            else {
              this.error = response.result;
            }

            this.productInternalLoading = false;

          },
          error => {
            this.setError(error);
            this.productInternalLoading = false;
            this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
          });
    }
  }




  mapModel(customerProductDetails: any): EditCustomerProductModel {
    var model: EditCustomerProductModel = {
      id: customerProductDetails.id,
      productID: customerProductDetails.productId,
      productDescription: customerProductDetails.productDescription,
      customerID: customerProductDetails.customerId,
      barcode: customerProductDetails.barcode,
      factoryReference: customerProductDetails.factoryReference,
      photo: customerProductDetails.photo,
      productCategory: customerProductDetails.productCategory,
      productSubCategory: customerProductDetails.productSubCategory,
      productCategorySub2: customerProductDetails.productCategorySub2,
      productCategorySub3: customerProductDetails.productCategorySub3,
      remarks: customerProductDetails.remarks,
      cuProductFileAttachments: customerProductDetails.cuProductFileAttachments,
      isProductBooked: customerProductDetails.isProductBooked ? this.isProductBookedwithUserRole(customerProductDetails.isProductBooked) : customerProductDetails.isProductBooked,
      isBooked: customerProductDetails.isBooked,
      apiServiceIds: customerProductDetails.apiServiceIds,
      isNewProduct: customerProductDetails.isNewProduct,
      isMsChart: false,
      isStyle: false,
      timePreparation: customerProductDetails.timePreparation,
      sampleSize8h: customerProductDetails.sampleSize8h,
      tpAdjustmentReason: customerProductDetails.tpAdjustmentReason,
      unit: customerProductDetails.unit,
      technicalComments: customerProductDetails.technicalComments,
      screenCallType: this.callFrom,
      productCategoryName: null,
      productSubCategoryName: null,
      productCategorySub2Name: null,
      productCategorySub3Name: null
    };
    const productFileAttachmentList = customerProductDetails.cuProductFileAttachments?.filter(x => x.productMsCharts && x.productMsCharts.length > 0);
    productFileAttachmentList.forEach(x => {
      this.mapOcrList(x.productMsCharts, x.uniqueld);
    });
    return model;
  }

  // users under roles(4,6,8) can update the category in any time.  
  isProductBookedwithUserRole(_isProductBooked: boolean): boolean {
    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.IT_Team
        || x.id == this._roleEnum.CSManagement
        || x.id == this._roleEnum.TechnicalTeam || x.id == this._roleEnum.WorkLoadMatrix).length > 0) {
      _isProductBooked = false;
    }
    return _isProductBooked;
  }


  save() {
    this.mapProductMsChartList(this.ocrMasterModel.ocrList);
    this.validator.initTost();
    this.validator.isSubmitted = true;
    for (let item of this.ocrMasterModel.ocrValidators)
      item.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.savedataloading = true;
      this.model.screenCallType = this.callFrom;
      this.service.saveCustomerProduct(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.showSuccess('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.SAVE_OK');
              if (this.callFrom == this.productScreenCallType.Booking) {
                this.model.id = res.id;

                if (res.productList && res.productList.length > 0)
                  this.addExistingProductDataForBooking(res.productList);

                this.bookingProductList.push(this.model);

                this.closeProductPopupFromBooking.emit(this.bookingProductList);
                this.return;
              }
              else if (this.callFrom == this.productScreenCallType.PurchaseOrder) {
                return this.modalService.dismissAll();
              }
              else if (this.fromSummary)
                this.return('cusproductsearch/customer-productsummary');
              else
                this.init();
            }

            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CANNOT_ADDCUSTOMERPRODUCT');
                  break;
                case 3:
                  this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CUSTOMER_PRODUCT_NOT_FOUND');
                  break;
                case 4:
                  this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CUSTOMER_PRODUCT_EXISTS');
                  break;
              }

              //this.waitingService.close();
            }
            this.savedataloading = false;
          },
          error => {
            this.savedataloading = false;
            this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
            //this.waitingService.close();
          });
    }
  }

  addExistingProductDataForBooking(productList) {
    var productData = productList[0];
    if (productData) {
      this.model.id = productData.id;
      this.model.productID = productData.productName;
      this.model.productDescription = productData.productDescription;
      this.model.productCategory = productData.productCategoryId;
      this.model.productSubCategory = productData.productSubCategoryId;
      this.model.productCategorySub2 = productData.productSubCategory2Id;
      this.model.productCategorySub3 = productData.PproductSubCategory3Id;

      this.model.productCategoryName = productData.productCategoryName;
      this.model.productSubCategoryName = productData.productSubCategoryName;
      this.model.productCategorySub2Name = productData.productSubCategory2Name;
      this.model.productCategorySub3Name = productData.productSubCategory3Name;

      this.model.barcode = productData.barcode;
      this.model.factoryReference = productData.factoryReference;
    }

  }

  reset() {
    //this.init(this.productID);
    this.model = new EditCustomerProductModel();
    this.productSubCategoryList = null;
    this.productCategorySub2List = null;
  }

  isFormValid() {
    var isOk = true;
    if (this._IsInternalUser) {
      isOk = this.validator.isValid('productID') &&
        this.validator.isValid('productDescription') &&
        this.validator.isValid('customerID') &&
        this.validator.isValid('productCategory') &&
        this.validator.isValid('apiServiceIds') &&
        this.validator.isValid('productCategorySub2') &&
        this.ocrMasterModel.ocrValidators.filter(x=>x.ocr.isSelected).every((x) =>
          x.validator.isValid('code') &&
          x.validator.isValid('description') &&
          x.validator.isValid('required') &&
          x.validator.isValid('tolerance1Up'));
      //        && this.validator.isValid('productCategorySub3');
      //productsubcategory and product subcategory2 mandatory for tcf service
      //temporarily comment this code since productsubcategory not confirmed from tcf side
      //if (isOk) {
      //  var isTCFProduct = this.model.apiServiceIds.find(x => x == this.apiServiceEnum.TCF);
      //  if (!isTCFProduct) {
      //    isOk = this.validator.isValid('productSubCategory') && this.validator.isValid('productCategorySub2');
      //  }
      //}
    }
    else {
      isOk = this.validator.isValid('productID') &&
        this.validator.isValid('productDescription') &&
        this.validator.isValid('customerID') &&
        this.validator.isValid('productSubCategory') &&
        this.validator.isValid('productCategory') &&
        this.validator.isValid('apiServiceIds') &&
        this.ocrMasterModel.ocrValidators.filter(x=>x.ocr.isSelected).every((x) =>
          x.validator.isValid('code') &&
          x.validator.isValid('mpcode') &&
          x.validator.isValid('description') &&
          x.validator.isValid('required') &&
          x.validator.isValid('tolerance1Up') &&
          x.validator.isValid('tolerance1Down'));


      //productsubcategory mandatory for tcf service
      //temporarily comment this code since productsubcategory not confirmed from tcf side
      //if (isOk) {
      //  var isTCFProduct = this.model.apiServiceIds.find(x => x == this.apiServiceEnum.TCF);
      //  if (!isTCFProduct) {
      //    isOk = this.validator.isValid('productSubCategory');
      //  }
      //}
    }
    if (isOk && this.tpUpdated && !this.model.tpAdjustmentReason) {
      this.showWarning('Validation', 'Tp Adjustment reason required');
      isOk = false;
    }

    return isOk;
  }

  removeAttachment(index) {
    if(this.model.cuProductFileAttachments[index].fileTypeId == this.msChartFileType.MSChartPdf)
      this.ocrMasterModel = new OcrMasterModel();
    if (this.modelRef)
      this.modelRef.close();
    this.model.cuProductFileAttachments.splice(index, 1);
  }

  getFile(file: AttachmentFile) {
    this.downloadloading = true;
    this.fileStoreService.downloadBlobFile(file.uniqueld, FileContainerList.Products)
      .subscribe(res => {

        this.downloadFile(res, file.mimeType);
      },
        error => {
          this.downloadloading = false;
          this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
        });

  }

  getPreviewProductImage(data, modalContent) {
    this.productGalleryImages = [];
    this.getProductFile(data)
    this.modelRef = this.modalService.open(modalContent, { windowClass: "mdModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => { });
  }


  uploadProductDocsModal() {

    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: 1000000,
      uploadFileExtensions: 'png,jpg,jpeg,pdf',
      uploadLimit: 10,
      containerId: FileContainerList.Products,
      token: "",
      fileDescription: null
    }

    modalRef.componentInstance.fromParent = fileInfo;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {

          this.model.cuProductFileAttachments.push(element);

        });
      }

    }, (reason) => {

    });

  }

  cancelFileUpload() {
    this.modelRef.close();
  }

  getProductFile(file: AttachmentFile) {
    this.productGalleryImages = [];
    this.productGalleryImages.push(
      {
        small: file.fileUrl,
        medium: file.fileUrl,
        big: file.fileUrl,
      });
  }

  downloadFile(data, mimeType) {
    let navigator: any = window.navigator;
    const blob = new Blob([data], { type: mimeType });
    if (navigator && navigator.msSaveOrOpenBlob) {
      navigator.msSaveOrOpenBlob(blob, "export.xlsx");
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
    this.downloadloading = false;
  }

  getAPIServices() {

    this.referenceService.getAPIServices()
      .pipe()
      .subscribe(
        response => {
          if (response.result == 1)
            this.apiServiceList = response.dataSourceList;
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  //#region ProductSubCategory2 Loading
  //fetch the first 10 productSubCategory2 List on load
  getProductSubCategory2ListBySearch() {
    this.customerProductMaster.productCategorySub2ModelRequest.productSubCategoryIds = [];
    this.customerProductMaster.productCategorySub2ModelRequest.productSubCategory2Ids = [];
    if (this.model.productCategorySub2)
      this.customerProductMaster.productCategorySub2ModelRequest.productSubCategory2Ids.push(this.model.productCategorySub2);
    if (this.model.productSubCategory)
      this.customerProductMaster.productCategorySub2ModelRequest.productSubCategoryIds.push(parseInt(this.model.productSubCategory));
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

    if (this.model.productSubCategory)
      this.customerProductMaster.productCategorySub2ModelRequest.productSubCategoryIds.push(parseInt(this.model.productSubCategory));

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

  //#endregion

  //#region ProductSubCategory3 Loading
  //fetch the first 10 productSubCategory3 List on load
  getProductSubCategory3ListBySearch() {
    this.customerProductMaster.productCategorySub3ModelRequest = new ProductSubCategory3SourceRequest();

    if (this.model.productCategory)
      this.customerProductMaster.productCategorySub3ModelRequest.productCategoryId = parseInt(this.model.productCategory);

    if (this.model.productCategorySub2)
      this.customerProductMaster.productCategorySub3ModelRequest.productSubCategory2Ids.push(this.model.productCategorySub2);

    if (this.model.productSubCategory)
      this.customerProductMaster.productCategorySub3ModelRequest.productSubCategoryId = parseInt(this.model.productSubCategory);
    this.customerProductMaster.productCategorySub3Input.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.customerProductMaster.productCategorySub3Loading = true),
      switchMap(term => term
        ? this.service.getProductSubCategory3DataSource(this.customerProductMaster.productCategorySub3ModelRequest, term)
        : this.service.getProductSubCategory3DataSource(this.customerProductMaster.productCategorySub3ModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.customerProductMaster.productCategorySub3Loading = false))
      ))
      .subscribe(data => {
        this.customerProductMaster.productCategorySub3List = data;
        this.productSubCat3Exists = this.customerProductMaster.productCategorySub3List && this.customerProductMaster.productCategorySub3List.length > 0;
        this.customerProductMaster.productCategorySub3Loading = false;
      });
  }

  //fetch the product sub category 3 data with virtual scroll
  getProductSubCategory3Data(isDefaultLoad: boolean) {
    if (this.model.productCategory)
      this.customerProductMaster.productCategorySub3ModelRequest.productCategoryId = parseInt(this.model.productCategory);

    if (this.model.productSubCategory)
      this.customerProductMaster.productCategorySub3ModelRequest.productSubCategoryId = parseInt(this.model.productSubCategory);

    if (this.model.productCategorySub2)
      this.customerProductMaster.productCategorySub3ModelRequest.productSubCategory2Ids.push(this.model.productCategorySub2);

    if (isDefaultLoad) {
      this.customerProductMaster.productCategorySub3ModelRequest.searchText = this.customerProductMaster.productCategorySub3Input.getValue();
      this.customerProductMaster.productCategorySub3ModelRequest.skip = this.customerProductMaster.productCategorySub3List.length;
    }
    this.customerProductMaster.productCategorySub3Loading = true;
    this.service.getProductSubCategory3DataSource(this.customerProductMaster.productCategorySub3ModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.customerProductMaster.productCategorySub3List = this.customerProductMaster.productCategorySub3List.concat(data);
        }
        if (isDefaultLoad)
          this.customerProductMaster.productCategorySub3ModelRequest = new ProductSubCategory3SourceRequest();
        this.customerProductMaster.productCategorySub3Loading = false;
      }),
      error => {
        this.customerProductMaster.productCategorySub3Loading = false;
      };
  }

  //#endregion

  async loadWorkLoadMatrixData() {
    let res = await this.workLoadMatrixService.getWorkLoadMatrixByProdCatSub3Id(this.model.productCategorySub3);

    if (res.result == ResponseResult.Success) {
      this.model.timePreparation = res.data.preparationTime;
      this.model.sampleSize8h = res.data.eightHourSampleSize;
    }
  }

  changeTp() {
    this.tpUpdated = true;
  }

  validateTpAdjustmentReason() {
    if (this.validator.isSubmitted && this.tpUpdated) {
      return this.model.tpAdjustmentReason ? false : true;
    }
    return false;
  }

  getUnitList() {
    this.customerProductMaster.unitLoading = true;
    this.bookingService.GetUnitList()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.customerProductMaster.unitList = res.unitList;
          }
          else {
            this.customerProductMaster.unitList = [];
          }

          this.customerProductMaster.unitLoading = false;
        },
        error => {
          this.customerProductMaster.unitLoading = false;
        }
      );
  }

  //upload the file to cloud
  cloudUpload(event) {
    var uploadFiles = [];
    if (this.modelRef)
      this.modelRef.close();
    if (event && !event.error && event.files) {
      if (event.files.length > 100000) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED');
      }
      else if (this.fileTypeId == this.msChartFileType.MSChartExcel && this.model.cuProductFileAttachments.filter(x => x.fileTypeId == this.msChartFileType.MSChartExcel).length >= 1) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED');
      }
      else if (this.fileTypeId == this.msChartFileType.MSChartPdf && this.model.cuProductFileAttachments.filter(x => x.fileTypeId == this.msChartFileType.MSChartPdf).length >= 1) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED');
      }
      else {
        this.fileuploadLoader = true;
        for (let file of event.files) {
          var guid = this.newUniqueId();
          let fileItem: AttachmentFile = {
            fileName: file.name,
            file: file,
            fileSize: file.size,
            isNew: true,
            id: 0,
            status: 0,
            mimeType: file.type,
            fileUrl: "",
            uniqueld: guid,
            isSelected: false,
            fileTypeId: this.fileTypeId,
            productMsCharts: [],
            fileDescription: null
          };
          uploadFiles.unshift(fileItem);

          // upload to cloud - selected files and the status is new
          if (uploadFiles) {
            this.fileService.uploadFiles(FileContainerList.Products, [fileItem]).subscribe(response => {
              if (response && response.fileUploadDataList) {
                fileItem.fileUrl = response.fileUploadDataList[0].fileCloudUri;
                fileItem.uniqueld = response.fileUploadDataList[0].fileName;
                this.model.cuProductFileAttachments.push(fileItem);
                if (this.fileTypeId == this.msChartFileType.MSChartPdf) {
                  this.getOcrTableData(fileItem);
                }
                else
                  this.fileuploadLoader = false;
              }
            },
              error => {
                this.fileuploadLoader = false;
                this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
              });
          }
          else {
            this.fileuploadLoader = false;
          }
        }
      }
    }
    else {
      this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', event.errorMessage);
    }
  }

  //frame unique id
  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  //get file type list
  getFileTypeList() {
    this.fileTypeLoading = true;
    this.service.getFileTypeList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.fileTypeList = data.dataSourceList;
          }
          this.fileTypeLoading = false;
        },
        error => {
          this.fileTypeLoading = false;
        });
  }

  //when change the file type 
  onChangeFileType(event) {
    //if selected value is MS chart
    if (event && event.id == this.msChartFileType.MSChartExcel) {
      this.uploadFileExtensions = "xlsx,xls";
      this.uploadFileLimit = "1";
    }
    //if selected value is Picture
    else if (event && event.id == this.msChartFileType.ProductRefPictures) {
      this.uploadFileLimit = "10";
      this.uploadFileExtensions = "jpg,png,gif,jpeg";
    }
    //if selected value is MS chart Pdf
    else if (event && event.id == this.msChartFileType.MSChartPdf) {
      this.getMsChartFileFormatList();
    }

  }

  //when clear the file type set upload file limit and upload file extension
  clearFileType() {
    this.fileTypeId = null;
    this.productMSChartMasterModel.showMsChartFileFormat = false;
    this.clearFileFormat();
  }

  //get file type name
  getFileType(fileTypeId) {
    return this.fileTypeList?.find(x => x.id === fileTypeId)?.name;
  }


  //validate the file type on click of the upload control
  validateFileType() {
    if (!this.uploadFileExtensions || this.uploadFileExtensions === '') {
      this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.MSG_FILE_TYPE_REQ');
    }
  }

  changeCustomer() {
    this.clearFileType();
    const fileIndex = this.model.cuProductFileAttachments.findIndex(x=>x.fileTypeId == this.msChartFileType.MSChartPdf);
    if(!(fileIndex < 0)){
      this.model.cuProductFileAttachments.splice(fileIndex, 1);
      this.ocrMasterModel = new OcrMasterModel();
    }
  }

  onChangeFileFormat() {
    if (this.productMSChartMasterModel.msChartFileFormatId) {
      this.uploadFileLimit = "1";
      this.uploadFileExtensions = "pdf";
    }
  }
  clearFileFormat() {
    this.uploadFileLimit = "0";
    this.uploadFileExtensions = undefined;
    this.productMSChartMasterModel.msChartFileFormatId = null;
  }

  getMsChartFileFormatList() {
    if (!this.model.customerID)
      return;
    this.productMSChartMasterModel.msChartFileFormatLoading = true;
    this.service.getMsChartFileFormat(this.model.customerID)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.productMSChartMasterModel.msChartFileFormatList = data.dataSourceList;
            const ocrFileFormat = this.productMSChartMasterModel.msChartFileFormatList.find(x => x.ocrFileFormat != null);
            if (ocrFileFormat)
              this.productMSChartMasterModel.showMsChartFileFormat = true;
            else {
              this.uploadFileLimit = "1";
              this.uploadFileExtensions = "pdf";
              this.productMSChartMasterModel.msChartFileFormatId = this.productMSChartMasterModel.msChartFileFormatList[0].id;
            }
          }
          this.productMSChartMasterModel.msChartFileFormatLoading = false;
        },
        error => {
          this.setError(error);
          this.productMSChartMasterModel.msChartFileFormatLoading = false;
        });
  }

  getOcrTableData(fileItem: AttachmentFile) {
    const msChartFile = this.productMSChartMasterModel.msChartFileFormatList.find(x => x.id == this.productMSChartMasterModel.msChartFileFormatId);
    if (msChartFile) {
      const ocrRequest = new OcrTableRequest();
      ocrRequest.file = fileItem.fileUrl;
      ocrRequest.brand_name = msChartFile.ocrCustomerName;
      ocrRequest.brand_format = msChartFile.ocrFileFormat;
      this.service.getOcrTableData(ocrRequest)
        .subscribe(
          res => {
            if (res != null) {
              this.ocrMasterModel.ocrTableNameList = Object.keys(res);
              this.ocrMasterModel.ocrTableNameList.forEach(element => {
                const ocrTableItem = new OcrTableItem();
                ocrTableItem.ocrTableName = element;
                ocrTableItem.ocrTableList = res[element].map((x) => {
                  const item: Ocr = {
                    id: 0,
                    productId: 0,
                    productFileId: null,
                    code: x[Size],
                    description: x[Description],
                    mpcode: x[Code],
                    required: x[RequiredValue] > 0 ? parseFloat(x[RequiredValue]) : null,
                    tolerance1Up: x[TolerancePlus] > 0 ? parseFloat(x[TolerancePlus]) : null,
                    tolerance1Down: x[ToleranceMinus] > 0 ? parseFloat(x[ToleranceMinus]) : null,
                    tolerance2Up: null,
                    tolerance2Down: null,
                    sort: null,
                    isSelected: false
                  }
                  return item;
                });
                this.ocrMasterModel.ocrTableData.push(ocrTableItem);
              });
              this.ocrMasterModel.ocrName = this.ocrMasterModel.ocrTableData[0].ocrTableName;
              this.mapOcrList(this.ocrMasterModel.ocrTableData[0].ocrTableList, fileItem.uniqueld);
              this.showSuccess('COMMON.LBL_SUCCESS','EDIT_CUSTOMER_PRODUCT.MSG_MSCHART_CONVERTED_SUCCESSFULLY');
            }
            else{
              const fileIndex = this.model.cuProductFileAttachments.findIndex(x => x.fileTypeId == this.msChartFileType.MSChartPdf);
              if (!(fileIndex < 0))
                this.removeAttachment(fileIndex);
              this.showError('COMMON.LBL_ERROR', 'EDIT_CUSTOMER_PRODUCT.MSG_MSCHART_CONVERSION_FAILED');
            }
            this.fileuploadLoader = false;
          },
          error => {
            this.setError(error);
            this.fileuploadLoader = false;
          });
    }
  }

  mapOcrList(items: any, uniqueld: string) {
    items.map((z) => {
      const item: Ocr = {
        id: z.id,
        productId: z.productId,
        productFileId: z.productFileId,
        code: z.code,
        description: z.description,
        mpcode: z.mpcode,
        required: z.required,
        tolerance1Up: z.tolerance1Up > 0 ? z.tolerance1Up : 0,
        tolerance1Down: z.tolerance1Down > 0 ? z.tolerance1Down : 0,
        tolerance2Up: z.tolerance2Up,
        tolerance2Down: z.tolerance2Down,
        sort: z.sort,
        isSelected: z.id > 0 ? true : false
      }
      this.ocrMasterModel.ocrList.push(item);
      this.ocrMasterModel.ocrValidators.push({ ocr: item, validator: Validator.getValidator(item, "customer/ocr.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
    })
    this.ocrMasterModel.ocrFileUniqueld = uniqueld;
    this.isAllSelected();
  }

  mapProductMsChartList(ocrList: Ocr[]) {
    const productFileAttachment = this.model.cuProductFileAttachments.find(x => x.fileTypeId == this.msChartFileType.MSChartPdf);
    if (productFileAttachment) {
      productFileAttachment.productMsCharts = [];
      const ocrSelectedList = ocrList.filter(x => x.isSelected);
      for (let i = 0; i < ocrSelectedList.length; i++) {
        productFileAttachment.productMsCharts.push(ocrSelectedList[i]);
      }
    }
    if (this.modelRef)
      this.modelRef.close();
  }

  checkUncheckAll() {
    for (let i = 0; i < this.ocrMasterModel.ocrList.length; i++) {
      this.ocrMasterModel.ocrList[i].isSelected = this.ocrMasterModel.isAllSelected;
    }
    this.ocrMasterModel.showOcrExport = this.ocrMasterModel.ocrList.find(x => x.isSelected) ? true : false;
  }
  isAllSelected() {
    this.ocrMasterModel.isAllSelected = this.ocrMasterModel.ocrList.every(function (item: any) {
      return item.isSelected == true;
    })
    this.ocrMasterModel.showOcrExport = this.ocrMasterModel.ocrList.find(x => x.isSelected) ? true : false;
  }

  changeOcrName() {
    this.ocrMasterModel.ocrList = [];
    this.ocrMasterModel.ocrValidators = [];
    const ocrList = this.ocrMasterModel.ocrTableData.find(x => x.ocrTableName == this.ocrMasterModel.ocrName)?.ocrTableList;
    if (ocrList && ocrList.length > 0)
      this.mapOcrList(ocrList, this.ocrMasterModel.ocrFileUniqueld);
  }

  exportOcr() {
    this.ocrMasterModel.exportOcrLoading = true;
    const exportocr = this.ocrMasterModel.ocrList.filter(y => y.isSelected);
    this.service.exportOcr(exportocr)
      .subscribe(res => {
        this.ocrMasterModel.exportOcrLoading = false;
        const filename = this.model.productID + "_" + this.ocrMasterModel.ocrFileUniqueld + ".xlsx"
        this.downloadOcrFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",filename);
      },
        error => {
          this.setError(error);
          this.ocrMasterModel.exportOcrLoading = false;
        });
  }
  downloadOcrFile(data, mimetype, filename) {
    const blob = new Blob([data], { type: mimetype });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, filename);
    }
    else {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = filename
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }

  openConfirm(index, content) {
    this.fileIndex = index;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => { }, (reason) => {});
  }

  saveProductConfirm(productSaveConfirmPopup){
    this.modelRef = this.modalService.open(productSaveConfirmPopup, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => { }, (reason) => {});
  }

  fnUplodclick(fileConfirmationPopsUp) {
    if ((this.fileTypeId == this.msChartFileType.MSChartExcel &&
      this.model.cuProductFileAttachments.filter(x => x.fileTypeId == this.msChartFileType.MSChartExcel).length >= 1)) {
      if (this.uploadedFileCountByType == 0) {
        this.uploadedFileCountByType = this.uploadedFileCountByType + 1;
        this.showWarning('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.MSG_ANOTHER_FILE');
      }
      else
        this.uploadedFileCountByType = 0;

      return false;
    }
    else if ((this.fileTypeId == this.msChartFileType.MSChartPdf && this.model.cuProductFileAttachments.filter(x => x.fileTypeId == this.msChartFileType.MSChartPdf).length >= 1)) {
      if (this.uploadedFileCountByType == 0) {
        this.uploadedFileCountByType = this.uploadedFileCountByType + 1;
        this.modelRef = this.modalService.open(fileConfirmationPopsUp, { windowClass: "smModelWidth", centered: true });
        this.modelRef.result.then((result) => { }, (reason) => { });
      }
      else
        this.uploadedFileCountByType = 0;

      return false;
    }
    else
      return true;
  }

  existingFile() {
    if (this.modelRef)
      this.modelRef.close();
    if (this.fileTypeId == this.msChartFileType.MSChartPdf) {
      this.fileuploadLoader = true;
      const fileItem = this.model.cuProductFileAttachments.find(x => x.fileTypeId == this.msChartFileType.MSChartPdf);
      if (fileItem) {
        this.ocrMasterModel.ocrList = [];
        this.ocrMasterModel.ocrValidators = [];
        this.getOcrTableData(fileItem);
      }
    }
  }

  // function to handle input click upload
  inputFileUpload(event) {
    this.fileArray = [];
    let files = event.target.files;
    if (files && files.length > 0)
      this.saveFiles(files);
    event.target.value = '';
  }

  // function to validate files on various checks
  saveFiles(files) {
    if (files.length <= 0) {
      this.errorFlag = true;
      this.errorMessage = 'No file found';
      this.sendResponse();
      return;
    }
    else if (files.length > this.fileLimit) {
      this.errorFlag = true;
      this.fileCountErrorFlag = true;
      this.errorMessage = 'File limit exceeded';
      this.sendResponse();
      return;
    }
    else {
      this.errorFlag = false;
    }

    this.isValidFileExtension(files);
    this.isValidFileSize(files);

    if (!this.errorFlag) {
      for (let j = 0; j < files.length; j++) {
        this.fileArray.push(files[j]);
      }
      this.sendResponse();
      return;
    }
  }

  // function to emit response according to errorFlag
  sendResponse() {
    // if errorFlag is true send error to parent
    if (this.errorFlag) {
      this.responseObject['error'] = true;
      this.responseObject['errorMessage'] = this.errorMessage;
      this.responseObject['files'] = [];
    }
    else {
      this.responseObject['error'] = false;
      this.responseObject['errorMessage'] = '';
      this.responseObject['files'] = this.fileArray;
      const fileIndex = this.model.cuProductFileAttachments.findIndex(x => x.fileTypeId == MSChartFileType.MSChartPdf);
      if (!(fileIndex < 0))
        this.removeAttachment(fileIndex);
    }
    this.cloudUpload(this.responseObject);
  }

  /**
   * function to validate each file size accourding to input
   * @param {Array} files Array of files
   */
  isValidFileSize(files) {
    for (let f of files) {
      if (f.size > this.fileSizeLimit) {
        this.errorFlag = true;
        this.fileSizeErrorFlag = true;
        this.errorMessage = 'file Size Exceeded';
        this.sendResponse();
        break;
      }
    }
  }

  /**
   * function to validate file extension
   * @param {Array} files [Array of files]
   */
  isValidFileExtension(files) {
    if (this.uploadFileExtensions) {
      // Make array of file extensions
      const extensions = (this.uploadFileExtensions.split(',')).map(function (x) { return x.toLocaleUpperCase().trim() });
      for (let i = 0; i < files.length; i++) {
        // Get file extension
        let ext = files[i].name.toUpperCase().split('.').pop() || files[i].name;
        // Check the extension exists
        let exists = extensions.includes(ext);
        if (!exists) {
          this.errorFlag = true;
          this.errorMessage = 'please upload correct file format';
          this.sendResponse();
          break;
        }
        else {
          this.errorFlag = false;
        }
      }
    }
  }

  //validate the extension is selected or not
  validateExtension() {
    //check the file extension is null then show the error
    if (!this.uploadFileExtensions) {
      this.errorFlag = true;
      this.errorMessage = 'please select file type';
      this.sendResponse();
    }
    else {
      this.errorFlag = false;
    }
  }
}





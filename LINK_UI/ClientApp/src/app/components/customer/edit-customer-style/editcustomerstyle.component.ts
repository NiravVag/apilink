import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, JsonHelper } from '../../common'
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { EditCustomerProductModel, AttachmentFile, ProductScreenCallType } from '../../../_Models/customer/editcustomerproduct.model';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserType, FileContainerList, RoleEnum, MSChartFileType, ListSize } from '../../common/static-data-common'
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgxGalleryOptions, NgxGalleryImage } from 'ngx-gallery-9';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { ReferenceService } from '../../../_Services/reference/reference.service';
import { APIService } from "src/app/components/common/static-data-common";
import { CustomerProductMaster } from 'src/app/_Models/customer/customerproductmaster.model';
import { of, Subject } from 'rxjs';
import { CommonDataSourceRequest, ProductSubCategory2SourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { BehaviorSubject } from 'rxjs';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-editCustomerStyle',
  templateUrl: './editcustomerstyle.component.html',
  styleUrls: ['./editcustomerstyle.component.css']
})
export class EditCustomerStyleComponent extends DetailComponent {
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
  requestCustomerModel: CommonDataSourceRequest;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;
  componentDestroyed$: Subject<boolean> = new Subject();
  public selectCounryId: number = 0;
  public isDetails: boolean = true;

  private jsonHelper: any;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public addressValidators: Array<any> = [];
  public contactValidators: Array<any> = [];
  public action: string;
  public productID: number;
  public callFrom: string;
  public recentUploads: Array<AttachmentFile>;
  customerList: Array<any> = [];
  productCategoryList: Array<any> = [];
  productSubCategoryList: Array<any> = [];
  productCategorySub2List: Array<any> = [];
  msChartFileList: Array<any> = [];
  msChartDDLFileList: Array<any> = [];
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  fileuploadLoader: boolean = false;
  public productGalleryOptions: NgxGalleryOptions[];
  public productGalleryImages: NgxGalleryImage[];
  apiServiceList: any;
  fileTypeList: any;
  fileTypeId: number;
  msFileId: number;
  msFileList: Array<any> = [];
  fileTypeLoading: boolean;
  apiServiceEnum = APIService;
  public _roleEnum = RoleEnum;
  uploadFileExtensions: string = "jpg,png,gif";
  uploadFileLimit: string = "10";
  customerProductMaster: CustomerProductMaster;
  msChartStyleCodeList: Array<any> = [];
  msStyleCode: string;
  msChartFileType = MSChartFileType;

  constructor(

    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    private fileService: FileStoreService,
    route: ActivatedRoute,
    router: Router,
    jsonHelper: JsonHelper,
    public service: CustomerProduct,
    public modalService: NgbModal,
    public customerService: CustomerService,
    public utility: UtilityService,
    public fileStoreService: FileStoreService,
    authserve: AuthenticationService,
    public referenceService: ReferenceService) {
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

    this.customerProductMaster = new CustomerProductMaster();
    this.customerInput = new BehaviorSubject<string>("");
    this.requestCustomerModel = new CommonDataSourceRequest();

  }
  @Input() modelData: any;

  onInit(id?: any, type?: any) {
    this.getAPIServices();

    this.callFrom = "EditCustomerStyleComponent";

    this.model = new EditCustomerProductModel();
    this.model.isStyle = true;
    if (this.callFrom != "EditCustomerStyleComponent") {
      id = 0;
      if (this.modelData) {
        this.model.customerID = this.modelData.customerId;
      }
    }

    this.productID = id;
    this.validator = Validator.getValidator(this.model, "customer/edit-customerstyle.valid.json", this.jsonHelper, false, this._toastr, this._translate);
    this.validator.isSubmitted = false;
    this.init(id);
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onFileChange(event) {
    this.msChartFileList = [];
    this.msChartStyleCodeList = [];
    var index = -1
    for (var i = 0; i < this.model.cuProductFileAttachments.length; i++) {
      if (this.model.cuProductFileAttachments[i].id === event.id) {
        index = i;
        break;
      }
    }
    if (index != -1) {
      this.msChartFileList = this.model.cuProductFileAttachments[index].productMsCharts;
      // this.msChartFileList = this.msChartFileList.sort((a, b) => (a.code < b.code ? -1 : 1));
      this.msChartStyleCodeList = this.getuniqueStyleList(index);
      this.msStyleCode = null;
    }

  }

  getuniqueStyleList(index) {
    const result = [];
    const map = new Map();
    for (const item of this.model.cuProductFileAttachments[index].productMsCharts) {
      if (!map.has(item.code)) {
        map.set(item.code, true);    // set any value to Map
        result.push({
          id: item.id,
          code: item.code
        });
      }
    }
    return result;
  }

  getViewPath(): string {
    return "cusstyleedit/view-customer-style";
  }

  getEditPath(): string {
    return "cusstyleedit/edit-customer-style";
  }



  init(id?) {
    this.initialLoading = true;
    this.getProductCategoryList();
    this.getFileTypeList();

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
                  this.getCustomerListBySearch();
                  // set default first item
                  this.msChartDDLFileList = [...this.model.cuProductFileAttachments.filter(x => x.fileTypeId == this.msChartFileType.MSChartExcel)];

                  if (this.msChartDDLFileList && this.msChartDDLFileList.length > 0) {
                    let msChartIndex = this.model.cuProductFileAttachments.findIndex(x => x.fileTypeId === this.msChartFileType.MSChartExcel);
                    this.msFileId = this.model.cuProductFileAttachments[msChartIndex].id;
                    this.msChartFileList = this.model.cuProductFileAttachments[msChartIndex].productMsCharts;
                    this.msChartStyleCodeList = this.getuniqueStyleList(msChartIndex);
                  }

                  if (this.model.productSubCategory) {
                    this.loadProductSubCategory(this.model.productCategory);
                  }

                  if (this.model.productSubCategory) {
                    this.getProductSubCategory2ListBySearch();
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
        this.getCustomerListBySearch();
        this.model.apiServiceIds = [];
        this.model.apiServiceIds.push(this.apiServiceEnum.Inspection);
        this.validator.isSubmitted = false;
        this.initialLoading = false;
      }
    }
    else {
      this.getCustomerListBySearch();
      this.initialLoading = false;
    }

  }

  getCustomerListBySearch() {

    //push the customerid to  customer id list 
    if (this.model.customerID) {
      this.requestCustomerModel.idList.push(this.model.customerID);
    }
    else {
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
    this.model.customerID = null;
    this.getCustomerListBySearch();
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

  changeProductSubCategory(id) {
    this.model.productSubCategory = null;
    this.model.productCategorySub2 = null;
    this.loadProductSubCategory(id);
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

  changeProductSubCategory2(id) {
    this.model.productCategorySub2 = null;
    this.getProductSubCategory2ListBySearch();
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
      remarks: customerProductDetails.remarks,
      cuProductFileAttachments: customerProductDetails.cuProductFileAttachments,
      isProductBooked: customerProductDetails.isProductBooked ? this.isProductBookedwithUserRole(customerProductDetails.isProductBooked) : customerProductDetails.isProductBooked,
      isBooked: customerProductDetails.isBooked,
      apiServiceIds: customerProductDetails.apiServiceIds,
      isNewProduct: customerProductDetails.isNewProduct,
      isMsChart: false,
      isStyle: false,
      productCategorySub3: null,
      timePreparation: null,
      sampleSize8h: null,
      tpAdjustmentReason: null,
      unit: null,
      technicalComments: null,
      screenCallType: ProductScreenCallType.Product,
      productCategoryName:null,
      productSubCategoryName:null,
      productCategorySub2Name:null,
      productCategorySub3Name:null
    };
    return model;
  }

  // users under roles(4,6,8) can update the category in any time.  
  isProductBookedwithUserRole(_isProductBooked: boolean): boolean {
    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.IT_Team
        || x.id == this._roleEnum.CSManagement
        || x.id == this._roleEnum.TechnicalTeam).length > 0) {
      _isProductBooked = false;
    }
    return _isProductBooked;
  }


  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.savedataloading = true;
      this.setMSChart();
      this.model.screenCallType = this.modelData.screenCallType;
      this.service.saveCustomerProduct(this.model)
        .subscribe(
          res => {

            if (res && res.result == 1) {
              this.showSuccess('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.STYLE_SAVE_OK');
              if (this.callFrom != "EditCustomerStyleComponent") {
                return this.modalService.dismissAll();
              }
              if (this.fromSummary)
                this.return('cusstylesearch/customer-stylesummary');
              else
                this.init();
            }

            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CANNOT_ADDCUSTOMERSTYLE');
                  break;
                case 3:
                  this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CUSTOMER_STYLE_NOT_FOUND');
                  break;
                case 4:
                  this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CUSTOMER_STYLE_EXISTS');
                  break;
              }

            }
            this.savedataloading = false;
          },
          error => {
            this.savedataloading = false;
            this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
          });
    }
  }

  setMSChart() {
    // set mschart flag
    if (this.model.cuProductFileAttachments && this.model.cuProductFileAttachments.length > 0) {
      for (let index = 0; index < this.model.cuProductFileAttachments.length; index++) {
        const element = this.model.cuProductFileAttachments[index];
        if (element.fileTypeId == this.msChartFileType.MSChartExcel) {
          this.model.isMsChart = true;
          break;
        }
      }
    }
  }

  reset() {
    this.model = new EditCustomerProductModel();
    this.model.isStyle = true;
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
        this.validator.isValid('productCategorySub2');
    }
    else {
      isOk = this.validator.isValid('productID') &&
        this.validator.isValid('productDescription') &&
        this.validator.isValid('customerID') &&
        this.validator.isValid('productSubCategory') &&
        this.validator.isValid('productCategory') &&
        this.validator.isValid('apiServiceIds')

    }
    return isOk;
  }

  removeAttachment(index) {
    this.model.cuProductFileAttachments.splice(index, 1);
    this.model.cuProductFileAttachments = [...this.model.cuProductFileAttachments];
    // set default first item
    this.msChartDDLFileList = [...this.model.cuProductFileAttachments.filter(x => x.fileTypeId == this.msChartFileType.MSChartExcel)];
    this.msChartFileList = [];
    this.msChartStyleCodeList = [];
    this.msFileId = null;
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





  //upload the file to cloud
  cloudUpload(event) {
    var uploadFiles = [];
    if (event && !event.error && event.files) {
      if (event.files.length > 100000) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED');
      }
      else if (this.model.cuProductFileAttachments.filter(x => x.fileTypeId == this.msChartFileType.MSChartExcel).length >= 1) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED');
      }
      else {
        this.fileuploadLoader = true;
        for (let file of event.files) {
          var guid = this.newUniqueId();
          let fileItem: AttachmentFile = {
            fileName: file.name,
            file: file,
            fileSize: file.fileSize,
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
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
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

  /**
* function to toggle tabs on click
* @param {event} event     [current event]
* @param {string} tabTarget [targeted tab id]
*/
  toggleTab(event, tabTarget) {
    let tabs = event.target.parentNode.children;
    for (let tab of tabs) {
      tab.classList.remove('active');
    }
    event.target.classList.add('active');

    let tabContainers = document.querySelector('#' + tabTarget).parentNode.childNodes;
    for (let container of <any>tabContainers) {
      container.classList.remove('active');
    }
    document.getElementById(tabTarget).classList.add('active');
  }

  onChangeMSFileList(event) {
    this.uploadFileLimit = "10";
    this.uploadFileExtensions = "jpg,png,gif";
    if (event && event.id == this.msChartFileType.MSChartExcel) {
      this.uploadFileExtensions = "xlsx";
      this.uploadFileLimit = "1";
    }

  }

  onStyleChange(event, fileId) {
    if (event && fileId) {
      var index = -1
      for (var i = 0; i < this.model.cuProductFileAttachments.length; i++) {
        if (this.model.cuProductFileAttachments[i].id === fileId) {
          index = i;
          break;
        }
      }
      this.msChartFileList = [...this.model.cuProductFileAttachments[index].productMsCharts.filter((item) => item.code == event.code)];
      // this.msChartFileList = this.msChartFileList.sort((a, b) => (a.code < b.code ? -1 : 1));
    }
    else if (fileId) {
      var index = -1
      for (var i = 0; i < this.model.cuProductFileAttachments.length; i++) {
        if (this.model.cuProductFileAttachments[i].id === fileId) {
          index = i;
          break;
        }
      }
      this.msChartFileList = [...this.model.cuProductFileAttachments[index].productMsCharts];
      // this.msChartFileList = this.msChartFileList.sort((a, b) => (a.code < b.code ? -1 : 1));
    }
  }

  //#endregion

}





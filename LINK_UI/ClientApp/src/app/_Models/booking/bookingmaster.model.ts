import { NgbDate } from "@ng-bootstrap/ng-bootstrap/datepicker/ngb-date";
import { BookingPoProduct, commonDataSource, InspectionPODetails, InspectionProductDetails } from "./inspectionbooking.model";
import { CustomSampleSize } from "../reference/custom-sample-size.model";
import { AqlType, DACorrelationEnum, DACorrelationList, DateObject, ListSize } from "src/app/components/common/static-data-common";
import { DataSource, ProductSubCategory2SourceRequest } from "../common/common.model";
import { BehaviorSubject } from "rxjs";

export class BookingMasterData {
   public Initialloading = false;
   public cusloading: boolean = false;
   public cuscontactloading: boolean = false;
   public productsCategoryloading: boolean = false;
   public suploading: boolean = false;
   public factloading: boolean = false;
   public factContactloading: boolean = false;
   public savedataloading = false;
   public IsCustomer: boolean = false;
   public Issupplier: boolean = false;
   public IsFactory: boolean = false;
   public IsInternalUser: boolean = false;
   public IsconfirmBtnShow = false;
   public IsverifyBtnShow = false;
   public IsholdBtnShow = false;
   public IsSaveBtnShow = false;
   public IsConfirmEmainBtnShow = false;
   public Customerlist = [];
   public Seasonyearlist = [];
   public SeasonList = [];
   public InspectionLocationList = [];
   public inspectionBookingTypes = [];
   public inspectionPaymentOptions = [];
   public inspectionShipmentTypes = [];
   public businessLines = [];
   public customerProductCategories = [];
   public customerSeasonConfigList = [];
   public Office = [];
   public officedetails: any;
   public totalattachedfile: number;
   public issuccess: boolean = false;
   public fileSize: number;
   public uploadLimit: number;
   public uploadFileExtensions: string;
   public userTypeId: any;
   public leaddays: NgbDate;
   public customerContactList: any;
   public customerBrandList: any;
   public customerDepartmentList: any;
   public customerServiceList: any;
   public seasonList: any;
   public customerBuyerList: any;
   public customerMerchandiserList: any;
   public supplierList: any;
   public supplierContactList: any;
   public factoryContactList: any;
   public factoryList: any;
   public poList: any;
   public poProductSourceList: Array<BookingPoProduct>;
   //public bookingProductList: Array<InspectionProductDetails>;

   //codes commented by ganesh since there is no use
   //public bookingPOValidators: Array<any>;
   //newBookingProductList: any;
   //codes commented by ganesh since there is no use

   public bookingPOProductValidators: Array<any>;
   public pickingValidators: Array<any>;
   public bookingProductCategoryList: any;
   public bookingSubProductCategoryList: any;
   public bookingSub2ProductCategoryList: any;
   public bookingSub3ProductCategoryList: any;
   public bookingParentSubProductCategoryList: any;
   public bookingParentSub2ProductCategoryList: any;
   public bookingProductAqlList: any;
   public bookingProductPickList: any;
   public bookingProductCriticalList: any;
   public bookingProductMajorList: any;
   public bookingProductMinorList: any;
   public bookingProductUnitList: any;
   public inspectionHoldReasonTypes: any;
   public poProductList: any;
   public ponumber: any;
   public poId: number;
   public selectedPOName: string;
   public selectedCommonPONo: number;
   public selectedCommonPOName: string;
   public searchOptionsList: any;
   public searchText: any;
   public searchType: any;
   public productCategoryID: number;
   public productCategoryName: string;
   public totalPickingQty: number;
   public totalManDays: number;
   public totalNoofContainers: number;
   public totalBookingQty: number;
   public totalProducts: number;
   public totalContainers: number;
   public totalReports: number;
   public totalSamplingSize: number;
   public totalPos: number;
   public PoList: string;
   public bookingServiceConfig: any;
   public reInspectionServiceTypes: any;
   public bookingProductFiles: any;
   public isPicking: boolean;
   public parentProductCategoryId: number;
   public parentContainerId: number;
   public parentEtd: DateObject;
   public parentFbTemplateId: number;
   public parentFbTemplateName: number;
   public parentProductSubCategoryId: number;
   public parentProductCategorySub2Id: number;
   public parentProductCategorySub3Id: number;

   public parentDestinationCountryId: number;
   public parentDestinationCountryName: number;

   public parentProductCategoryName: string;

   public parentProductSubCategoryName: string;
   public parentProductCategorySub2Name: string;
   public parentProductCategorySub3Name: string;

   public parentAql: number;
   public parenttCritical: number;
   public parentMajor: number;
   public parentMinor: number;

   public isCustomerBrandAvailable: boolean;
   public isCustomerDeptAvailable: boolean;
   public isCustomerBuyerAvailable: boolean;
   public isCustomerMerchandiserAvailable: boolean;
   public dfCustomerConfigPreviewDataList: Array<DFCustomerConfigPreviewData>;
   factoryloading: boolean = false;
   supplierloading: boolean = false;
   sampleSizeVisible: boolean = false;
   destinationCountryAvailable: boolean = false;
   factoryReferenceAvailable: boolean = false;
   isFromSplitBooking: boolean;

   searchPO: boolean;
   customerserviceloading: boolean = false;
   public customerCollection: any;
   public customerPriceCategory: any;
   public fbTemplateList: Array<any>;
   public isCustomerCollectionAvailable: boolean;
   public isCustomerPriceCategoryAvailable: boolean;
   public customSampleTypeList: any;
   public isCustomAqlSelected: boolean = false;
   public aqlType = AqlType;
   displayMasterProducts: any;
   selectedPOList: any;
   selectedDeletePoDetails: Array<selectedDeletePoDetail>;
   selectAllDeletePOChecked: boolean;
   isDeleteAllPOChecked: boolean;
   disableAllPOChecked: boolean;
   deletePopupLoading: boolean;
   public currentStatusId: number;

   selectedSearchPoDetails: Array<selectedSearchPoDetail>;
   selectAllSearchPOChecked: boolean;
   isSearchAllPOChecked: boolean;

   businessLineLoading: boolean;
   customerProductCategoryLoading: boolean;
   customerConfigLoading: boolean;
   inspectionLocationLoading: boolean;
   shipmentTypeLoading: boolean;
   showServiceDateTo: boolean;

   customerCheckPointLoading: boolean;

   inspectionBookingTypeVisible: boolean;
   inspectionPaymentOptionVisible: boolean;

   productCategorySub2ModelRequest: ProductSubCategory2SourceRequest;
   parentSubCategory2ModelRequest: ProductSubCategory2SourceRequest;
   productCategorySub2Input: BehaviorSubject<string>;
   productCategorySub2Loading: boolean;

   requestPoDataSourceModel: PODataSourceRequest;
   poListInput: BehaviorSubject<string>;
   poLoading: boolean;

   searchPoId: number;
   searchPoName: number;
   searchPoList: any;

   public poDataSourceList: any;
   public poProductDataSourceList: any;

   public editBookingPoProductLoading: boolean;

   public editBookingPoLoading: boolean;

   public customerProductLoading: any;

   public savePurchaseOrderLoading: boolean;

   public isPoProductPopupVisible: boolean;

   searchPoLoading: boolean;

   csList: any;

   productFileUrls: Array<string>;

   productFileLoading: boolean;

   pageLoader: boolean;

   saveDraftBookingLoader: boolean;

   bookingProductLoading: boolean;

   deletePoProductIndex: number;

   deletePoProductTransaction: any;

   deletePoProductName: string;

   selectedPoNotAvailableIndex: number;
   selectedPoNotAvailableName: string;

   selectedPoProductIndex: number;

   bookingZipFileAttachment: BookingFileZipAttachment;
   bookingZipFileLoading: boolean;

   showDownloadAllAttachment: boolean;

   inspectionFileAttachments: any;
   gapFileAttachments: any;

   showFactoryCreationLink: boolean;
   showSupplierCreationLink: boolean;
   poProductDependentFilter: boolean;

   applicantContactId: number;
   togglePickingSection: boolean;
   daCorrelationList: any = DACorrelationList;
   daCorrelationEnum = DACorrelationEnum;
   constructor() {
      this.selectedDeletePoDetails = new Array<selectedDeletePoDetail>();
      this.selectAllDeletePOChecked = true;
      this.isDeleteAllPOChecked = false;
      this.deletePopupLoading = false;
      this.disableAllPOChecked = false;
      this.productCategorySub2ModelRequest = new ProductSubCategory2SourceRequest();
      this.parentSubCategory2ModelRequest = new ProductSubCategory2SourceRequest();
      this.productCategorySub2Input = new BehaviorSubject<string>("");
      this.businessLineLoading = false;
      this.customerCheckPointLoading = false;
      this.customerProductCategoryLoading = false;
      this.showServiceDateTo = true;

      this.poListInput = new BehaviorSubject<string>("");
      this.poLoading = false;

      /*  this.editBookingPoProductLoading = false;
       this.editBookingPoLoading = false;
       */

      this.poDataSourceList = [];
      this.poProductDataSourceList = [];

      this.displayMasterProducts = [];
      this.isCustomAqlSelected = false;
      this.customerProductLoading = false;

      this.savePurchaseOrderLoading = false;
      this.isPoProductPopupVisible = false;

      this.selectedSearchPoDetails = new Array<selectedSearchPoDetail>();
      this.selectAllSearchPOChecked = false;
      this.searchPoLoading = false;
      this.productFileUrls = [];
      this.productFileLoading = false;
      this.pageLoader = false;
      this.saveDraftBookingLoader = false;
      this.deletePoProductIndex = null;
      this.deletePoProductTransaction = null;

      this.bookingZipFileAttachment = new BookingFileZipAttachment();
      this.showDownloadAllAttachment = false;

      this.inspectionFileAttachments = [];
      this.gapFileAttachments = [];

      this.showFactoryCreationLink = false;
      this.showSupplierCreationLink = false;
      this.poProductDependentFilter = false;
      this.togglePickingSection = false;

   }
}

export class DFCustomerConfigPreviewData {
   public label: string;
   public value: string;
}


export class ApplicantStaffResponse {
   applicantInfo: BookingStaffInfo;
   result: ApplicantStaffResponseResult;
}

export enum ApplicantStaffResponseResult {
   Success = 1,
   Fail = 2
}

export class BookingStaffInfo {
   id: number;
   staffName: string;
   companyEmail: string;
   CompanyPhone: string;
}

export class selectedDeletePoDetail {
   poTransactionId: number;
   poId: number;
   poName: string;
   productId: number;
   productName: string;
   productDesc: string;
   containerId: number;
   containerName: string;
   colorCode: string;
   colorName: string;
   poQty: number;
   etd: string;
   destinationCountry: string;
   isPoSelected: boolean;
   disablePO: boolean;
   remarks: string;
   index: string;
   status: ProductValidationStatus
   constructor() {
      this.isPoSelected = false;
      this.disablePO = false;
      this.status = ProductValidationStatus.Pending;
   }
}

export class selectedSearchPoDetail {

   productId: number;
   productName: string;
   productDesc: string;
   productCategoryId: number;
   productCategoryName: string;
   productSubCategoryId: number;
   productSubCategoryName: string;
   productSubCategory2Id: number;
   productSubCategory2Name: string;
   productSubCategory3Id: number;
   productSubCategory3Name: string;
   barCode: string;
   factoryReference: string;
   remarks: string;
   isNewProduct: boolean;
   productSubCategoryList: Array<commonDataSource>;
   productSubCategory2List: Array<commonDataSource>;
   productSubCategory3List: Array<commonDataSource>;


   poTransactionId: number;
   poId: number;
   poName: string;
   etd: any;
   destinationCountryId: number;
   destinationCountryName: string;
   poQty: number;
   isPoSelected: boolean;

   constructor() {
      this.isPoSelected = false;
   }
}

export enum ProductValidationStatus {
   Success = 1, Error = 2, Pending = 3
}

export class PanelNavigation {
   stepOne: number;
   stepTwo: number;
   stepThree: number;
   stepFour: number;
   constructor() {
      this.stepOne = PanelNavigationStatus.Active;
      this.stepTwo = PanelNavigationStatus.Default;
      this.stepThree = PanelNavigationStatus.Default;
      this.stepFour = PanelNavigationStatus.Default;
   }
}

export enum PanelNavigationStatus {
   Default = 1,
   Active = 2,
   Completed = 3
}

export enum PanelData {
   Step1 = 1,
   Step2 = 2,
   Step3 = 3,
   Step4 = 4

}

export class PODataSourceRequest {
   public searchText: string;
   public skip: number;
   public take: number;
   public customerId: number;
   public supplierId: number;
   constructor() {
      this.searchText = "";
      this.skip = 0;
      this.take = ListSize;
      this.customerId = null;
      this.supplierId = null;
   }
}

export enum CommonFieldType {
   FBTemplate = 1,
   ETD = 2,
   ProductCategory = 3,
   ProductSubCategory = 4,
   ProductSubCategory2 = 5,
   ProductSubCategory3 = 6,
   DestinationCountry = 7
}

export class POProductDataRequest {
   poId: number;
   supplierId: number;
}

export class BookingFileZipAttachment {
   inspectionId: number;
   zipFileUrl: string;
   zipFileName: string;
   constructor() {
      this.zipFileName = "";
      this.zipFileUrl = "";
   }
}

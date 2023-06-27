import { array } from "@amcharts/amcharts4/core";
import { BehaviorSubject, Observable } from "rxjs";
import { DateObject, LabType, ListSize, SupplierType } from "src/app/components/common/static-data-common";
import { ProductDataSourceRequest } from "../common/common.model";

export class EditInspectionBooking {

  constructor() {
    this.id = 0;
    this.guidId = "";
    this.isEmailRequired = false;
    this.isFlexibleInspectionDate = false;
    this.isPickingRequired = false;

    this.factoryId = null;
    this.supplierId = null;
    this.customerId = null;
    this.statusId = null;
    this.seasonId = null;
    this.seasonYearId = null;
    this.officeId = null;
    this.entityId = 1;

    this.serviceDateFrom = null;
    this.serviceDateTo = null;
    this.cusBookingComments = "";
    this.apiBookingComments = "";
    this.internalComments = "";
    this.applicantName = "";
    this.applicantEmail = "";
    this.customerBookingNo = "";
    this.qcBookingComments = "";
    this.csNames = "";


    this.inspectionProductList = [];
    this.inspectionCustomerContactList = [];
    this.inspectionFactoryContactList = [];
    this.inspectionFileAttachmentList = [];
    this.inspectionServiceTypeList = [];
    this.inspectionSupplierContactList = [];
    this.inspectionCustomerBrandList = [];
    this.inspectionCustomerBuyerList = [];
    this.inspectionCustomerDepartmentList = [];
    this.isTermsChecked = false;
    this.isSupplierOrFactoryEmailSend = true;
    this.isCustomerEmailSend = false;
    this.inspectionDFTransactions = [];
    this.inspectionCustomerMerchandiserList = [];
    this.CreatedUserType = null;
    this.isCombineRequired = false;
    this.isCombineDone = false;
    this.isPickingDone = false;
  }

  public id: number;
  public guidId: string;
  public isEmailRequired: boolean;
  public isFlexibleInspectionDate: boolean;
  public isPickingRequired: boolean;
  public isCombineRequired: boolean;
  public isCombineDone: boolean;
  public isPickingDone: boolean;
  public customerId: number;
  public supplierId: number;
  public factoryId: number;
  public statusId: number;
  public statusName: string;
  public seasonId: number;
  public seasonYearId: number;
  public officeId: number;

  public entityId: number;

  public serviceDateFrom: any;
  public serviceDateTo: any;
  public firstServiceDateFrom: any;
  public firstServiceDateTo: any;
  public serviceTypeId: number;
  public cusBookingComments: string;
  public apiBookingComments: string;
  public internalComments: string;
  public applicantName: string;
  public applicantEmail: string;
  public applicantPhoneNo: number;
  public qcBookingComments: string;
  public csNames: string;
  public csList: any;

  public previousBookingNo: number;
  public splitPreviousBookingNo: number;
  public reinspectionType: number;
  public createdBy: number;

  public isBookingRequestRole: boolean;
  public isBookingConfirmRole: boolean;
  public isBookingVerifyRole: boolean;
  public isBookingInvoiced: boolean;
  // public createdOn: any;

  //public inspectionPoList: Array<InspectionProductDetails>;
  public inspectionProductList: Array<InspectionPOProductDetail>;
  public inspectionCustomerContactList: Array<number>;
  public inspectionFactoryContactList: Array<number>;
  public inspectionFileAttachmentList: Array<InspectionFileAttachment>;
  public inspectionServiceTypeList: Array<number>;
  public inspectionSupplierContactList: Array<number>;

  public inspectionCustomerBrandList: Array<number>;
  public inspectionCustomerBuyerList: Array<number>;
  public inspectionCustomerDepartmentList: Array<number>;
  public customerBookingNo: string;
  public inspectionPageType: number;
  public isTermsChecked: boolean;

  public isSupplierOrFactoryEmailSend: boolean;
  public isCustomerEmailSend: boolean;
  public inspectionDFTransactions: Array<InspectionDFTransaction>;
  public CreatedUserType?: number
  public inspectionCustomerMerchandiserList: Array<number>;
  public containerLimit: number;

  public collectionId: number;
  public priceCategoryId: number;
  public bookingType: number;
  public daCorrelationId: number;
  public gapDACorrelation: boolean;
  public gapDAName: string;
  public gapDAEmail: string;
  public paymentOptions: number;
  public customerCollectionName: string;
  public customerPriceCategoryName: string;
  compassBookingNo: string;
  public previousBookingNoList: Array<number>;
  public holdReasonTypeId: number;
  public holdReasonType: string;
  public holdReason: string;
  public draftInspectionId: number;

  public businessLine: number;
  public inspectionLocation: number;
  public shipmentPort: string;
  public ean: string;
  public shipmentDate: string;
  public cuProductCategory: string;
  public shipmentTypeIds: any;
  public isUpdateBookingDetail: boolean = false;
  public isEaqf: boolean = false;
}

export class InspectionPODetails {
  public id: number;
  public poId: number;
  public poDetailId: number;
  public poName: string;
  public productId: number;
  public productName: string;
  public productDesc: string;
  public productCategoryId: number;
  public productSubCategoryId: number;
  public productCategorySub2Id: number;
  public productCategorySub2Name: string;
  public productSubCategoryName: string;
  public productCategoryName: string;
  public inspectionId: number;
  public unit: number;
  public unitName: string;
  public unitCount: number;
  public poQuantity: number;
  public poReminingQuantity: number;
  public bookingQuantity: number;
  public pickingQuantity: number;
  public remarks: string;
  public aql: number;
  public aqlName: string;
  public critical: number;
  public major: number;
  public minor: number;
  public previewSamplingSize: number;
  public samplingSize: number;
  public combineSamplingSize: number;
  public combineGroupId: number;
  public colorCode: string;
  public bookingCategorySubProductList: Array<any>;
  public bookingCategorySub2ProductList: Array<any>;
  public productFileAttachments: Array<ProductFileAttachment>;
  public productCategoryMapped: boolean;
  public productSubCategoryMapped: boolean;
  public productCategorySub2Mapped: boolean;
  public existingbookingQuantity: number;
  public productCategorySubLoading: boolean;
  public productCategorySub2Loading: boolean;
  public destinationCountryID: number;
  public destinationCountryName: string;
  public factoryReference: string;
  public etd: Date;
  public barcode: string;
}


export class InspectionProductDetails {
  public id: number;
  public poId: number;
  public poName: string;
  public productId: number;
  public productName: string;
  public productDesc: string;
  public productSubCategoryId: number;
  public productCategorySub2Id: number;
  public productCategorySub2Name: string;
  public productSubCategoryName: string;
  public productCategoryName: string;
  public inspectionId: number;
  public unit: number;
  public unitName: string;
  public unitCount: number;
  public totalBookingQuantity: number;
  public aql: number;
  public aqlName: string;
  public critical: number;
  public major: number;
  public minor: number;
  public previewSamplingSize: number;
  public aqlQuantity: number;
  public sampleType: number;
  public combineSamplingSize: number;
  public combineGroupId: number;
  public colorCode: string;
  public factoryReference: string;
  public bookingCategorySubProductList: Array<any>;
  public bookingCategorySub2ProductList: Observable<string>;
  public productFileAttachments: Array<ProductFileAttachment>;
  public productPODetails: any;
  public productPODetailList: any;
  public productSubCategoryMapped: boolean;
  public productCategorySub2Mapped: boolean;
  public productCategorySub2Loading: boolean;
  public productCategoryMapped: boolean;
  public productCategoryId: number;
  public remarks: string;
  public barcode: string;
  public isEcopack: boolean;
  public asReceivedDate: any;
  public tfReceivedDate: any;
  public isDisplayMaster: boolean;
  public parentProductId: number;
  public parentProductName: number;
  public fbTemplateId: number;
  public fbTemplateName: string;
  public isVisible: boolean;
  public isNewProduct: boolean;
}

export class InspectionProductPODetails {
  public id: number;
  public poId: number;
  public poName: string;
  public productId: number;
  public inspectionId: number;
  public poQuantity: number;
  public bookingQuantity: number;
  public pickingQuantity: number;
  public existingbookingQuantity: number;
  public destinationCountryID: number;
  public containerId: number;
  public destinationCountryName: string;
  public containerName: string;
  public customerReferencePo: string;
  public etd: Date;
  isVisible: boolean;
}


export class InspectionCustomerContact {
  public id: number;
  public inspectionId: number;
  public contactId: number;
  public active: boolean;
  public createdOn: any;
  public createdBy: number;
  public deletedOn: any;
  public deletedBy: number;
}

export class InspectionFactoryContact {
  public id: number;
  public inspectionId: number;
  public contactId: number;
  public active: boolean;
  public createdOn: any;
  public createdBy: number;
  public deletedOn: any;
  public deletedBy: number;
}

export class InspectionFileAttachment {
  public id: number;
  public uniqueld: string;
  public fileName: string;
  public fileUrl: string;
  public isNew: boolean;
  public mimeType: string;
  public isBookingEmailNotification: boolean;
  public IsReportSendToFB: boolean;
  public fileAttachmentCategoryId: number;
}

export class ProductFileAttachment {
  public id: number;
  public uniqueld: string;
  public productId: number;
  public fileName: string;
  public fileDescription: string;
  public isNew: boolean;
  public mimeType: string;
  public fileUrl: string;
}

export class InspectionServiceType {
  public id: number;
  public inspectionId: number;
  public serviceTypeId: number;
  public active: boolean;
  public createdBy: number;
  public createdOn: any;
  public deletedBy: number;
  public deletedOn: any;
}

export class InspectionSupplierContact {
  public id: number;
  public inspectionId: number;
  public contactId: number;
  public active: boolean;
  public createdOn: any;
  public createdBy: number;
  public deletedOn: any;
  public deletedBy: number;
}

export class BookingPoProduct {
  public poId: number;
  public pono: string;
  public productId: number;
  public productName: string;
  public productDesc: string;
  public productQuantity: number;
  public selected: boolean;
  public aql: number;
  public critical: number;
  public major: number;
  public minor: number;
}

export class BookingCustomerContactRequest {
  public customerId: number;
  public customerServiceId: number;
  public bookingId: number;
  public brandIdlst: any[] = [];
  public deptIdlst: any[];
}

export class InspectionDFTransaction {
  public id: number;
  public bookingId: number;
  public controlConfigurationId: number;
  public value: string;
}

export class PurchaseOrderData {
  constructor() {
  }
  id: number;
  name: string;
}

export class ProductValidationResponse {
  public result: ProductValidationResult;
}

export enum ProductValidationResult {
  success = 1,
  quotationExists = 2,
  pickingExists = 3,
  pickingAndQuotationExists = 4,
  fail = 5
}

export enum EditBookingCustomerResult {
  success = 1,
  customerContactNotFound = 2,
  customerBrandNotFound = 3,
  customerDeptNotFound = 4,
  customerBuyerNotFound = 5,
  merchandiserNotFound = 6,
  collectionNotFound = 7,
  priceCategoryNotFound = 8,
  customerServiceTypeNotFound = 9,
  cannotGetDetails = 10

}

export enum BusinessLine {
  HardLine = 1,
  SoftLine = 2
}

export class InspectionPOProductDetail {
  public id: number;
  public poId: number;
  public poName: string;
  public productId: number;
  public productName: string;
  public productDesc: string;
  public productSubCategoryId: number;
  public productCategorySub2Id: number;
  public productCategorySub3Id: number;
  public productCategorySub2Name: string;
  public productCategorySub3Name: string;
  public productSubCategoryName: string;
  public productCategoryName: string;
  public inspectionId: number;
  public unit: number;
  public unitName: string;
  public unitCount: number;
  unitNameCount: string;
  public totalBookingQuantity: number;
  public aql: number;
  public aqlName: string;
  public critical: number;
  public major: number;
  public minor: number;
  public criticalName: string;
  public majorName: string;
  public minorName: string;
  public previewSamplingSize: number;
  public aqlQuantity: number;
  public sampleType: number;
  public combineSamplingSize: number;
  public combineGroupId: number;
  public factoryReference: string;
  public reportId: number;
  public bookingCategorySubProductList: Array<any>;
  public bookingCategorySub2ProductList: Array<any>;
  public bookingCategorySub3ProductList: Array<any>;
  public productFileAttachments: Array<ProductFileAttachment>;
  public productSubCategoryMapped: boolean;
  public productCategorySub2Mapped: boolean;
  public productCategorySub3Mapped: boolean;
  public productCategorySub2Loading: boolean;
  public productCategoryMapped: boolean;
  public productCategoryId: number;
  public remarks: string;
  public barcode: string;
  public isEcopack: boolean;
  public isDisplayMaster: boolean;
  public parentProductId: number;
  public parentProductName: number;
  public fbTemplateId: number;
  public fbTemplateName: string;
  public isNewProduct: boolean;
  public poTransactionId: number;
  public poQuantity: number;
  public bookingQuantity: number;
  public pickingQuantity: number;
  public existingbookingQuantity: number;
  public destinationCountryID: number;
  public containerId: number;
  public destinationCountryName: string;
  public containerName: string;
  public customerReferencePo: string;
  public etd: any;
  public isPoProductVisible: boolean;
  public colorTransactionId: number;
  public colorCode: string;
  public colorName: string;
  public asReceivedDate: any;
  public tfReceivedDate: any;
  public isPrimaryProductInGroup: boolean;
  public customSampleTypeName: string;
  public previewAqlName: string;

  productList: any;
  productInput: BehaviorSubject<string>;
  productLoading: boolean = false;
  productRequest: ProductDataSourceRequest;

  poProductList: any;
  poProductLoading = false;
  poProductListInput: BehaviorSubject<string>;
  requestPoProductDataSourceModel: POProductDataSourceRequest;



  poList: any;
  poLoading = false;
  poListInput: BehaviorSubject<string>;
  requestPoDataSourceModel: POProductDataSourceRequest;

  public isVisible: boolean;

  productImageCount: number;

  saveInspectionPickingList: Array<InspectionPickingDetails>;
  inspectionPickingValidators: Array<any>;
  isPlaceHolderVisible: boolean;
  togglePicking: boolean;


  constructor() {
    this.poId = null;
    this.poName = null;
    this.productId = null;
    this.productName = null;
    this.productDesc = null;
    this.productSubCategoryId = null;
    this.productCategorySub2Id = null;
    this.productCategorySub3Id = null;
    this.productCategorySub2Name = null;
    this.productCategorySub3Name = null;
    this.productSubCategoryName = null;
    this.productCategoryName = null;
    this.poTransactionId = 0;
    this.inspectionId = null;
    this.unit = null;
    this.unitName = null;
    this.unitCount = null;
    this.totalBookingQuantity = null;
    this.aql = null;
    this.aqlName = null;
    this.critical = null;
    this.major = null;
    this.minor = null;
    this.criticalName = null;
    this.majorName = null;
    this.minorName = null;
    this.previewSamplingSize = null;
    this.aqlQuantity = null;
    this.sampleType = null;
    this.combineSamplingSize = null;
    this.combineGroupId = null;
    this.factoryReference = null;
    this.bookingCategorySubProductList = [];
    this.bookingCategorySub2ProductList = [];
    this.bookingCategorySub3ProductList = [];
    this.productFileAttachments = [];
    this.productSubCategoryMapped = false;
    this.productCategorySub2Mapped = false;
    this.productCategorySub3Mapped = false;
    this.productCategorySub2Loading = false;
    this.productCategoryMapped = false;
    this.productCategoryId = null;
    this.remarks = null;
    this.barcode = null;
    this.isEcopack = false;
    this.isDisplayMaster = false;
    this.parentProductId = null;
    this.parentProductName = null;
    this.fbTemplateId = null;
    this.fbTemplateName = null;
    this.isNewProduct = false;
    this.poQuantity = null;
    this.bookingQuantity = null;
    this.pickingQuantity = null;
    this.existingbookingQuantity = null;
    this.destinationCountryID = null;
    this.containerId = null;
    this.destinationCountryName = null;
    this.containerName = null;
    this.customerReferencePo = null;
    this.etd = null;
    this.asReceivedDate = null;
    this.tfReceivedDate = null;
    this.isPoProductVisible = true;
    this.colorCode = null;
    this.colorName = null;
    this.poProductListInput = new BehaviorSubject<string>("");
    this.poListInput = new BehaviorSubject<string>("");

    this.productInput = new BehaviorSubject<string>("");

    this.poList = [];
    this.poProductList = [];
    this.isVisible = true;
    this.isPrimaryProductInGroup = true;
    this.productImageCount = 0;
    this.isPlaceHolderVisible = false;
    this.saveInspectionPickingList = [];
    this.inspectionPickingValidators = [];
    this.togglePicking = false;
  }
}

export class InspectionPickingDetails {
  id: number;
  labOrCustomerId: number;
  labOrCustomerAddressList: any;
  labOrCustomerAddressLoading: boolean;
  labOrCustomerAddressId: number;

  labOrCustomerContactList: any;
  labOrCustomerContactLoading: boolean;
  labOrCustomerContactIds: any;


  pickingQuantity: number;
  remarks: number;
  labType: number;
  pickingContactList: Array<SavePickingContact>;

  labOrCustomerName: string;
  labOrCustomerAddressName: string;
  labOrCustomerContactName: string;

  constructor() {
    this.labOrCustomerAddressLoading = false;
    this.labOrCustomerContactLoading = false;

    this.labOrCustomerId = null;
    this.labOrCustomerAddressList = [];
    this.labOrCustomerAddressId = null;
    this.labOrCustomerContactList = [];
    this.labOrCustomerContactIds = [];
    this.pickingQuantity = null;
    this.remarks = null;
    this.labType = null;
    this.pickingContactList = new Array<SavePickingContact>();
  }
}

export class SavePickingContact {
  id: number;
  labOrCustomerContactId: number;
}

export class InspectionPickingMaster {
  labOrCustomerList: any;
  labLoading: boolean;

  labOrCustomerAddressList: any;
  labOrContactList: any;
  labTypeList: any;

  _labType = LabType;

  constructor() {

    this.labOrCustomerList = [];
    this.labLoading = false;

    this.labOrContactList = [{ "id": 1, "name": "Contact1" }, { "id": 2, "name": "Contact2" }];

    this.labOrCustomerAddressList = [{ "id": 1, "name": "Address1" }, { "id": 2, "name": "Address2" }];
    this.labTypeList = [{ "id": 1, "name": "Type1" }, { "id": 2, "name": "Type2" }];
  }
}

export enum LabDataResult {
  Success = 1,
  CannotGetTypeList = 2
}

export class POProductList {
  id: number;
  name: string;
  description: string;
  /*  productCategoryId:number;
   productSubCategoryId:number;
   productSubCategory2Id:number;
   barCode:string;
   factoryReference:string;
   productCategoryName:string;
   productSubCategoryName:string;
   productSubCategory2Name:string; */
}

export class POProductDataSourceRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public poId: number;
  public supplierId: number;
  public customerIds: any;

  public filterPoProduct: FilterPoProductEnum;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.poId = null;
    this.supplierId = null;
    this.filterPoProduct = FilterPoProductEnum.ProductByCustomer;
    this.customerIds = [];
  }
}

export enum FilterPoProductEnum {
  ProductByPo = 1,
  ProductByCustomer = 2
}

export class commonDataSource {
  id: number;
  name: string;
}

export enum CustomerProductDetailResult {
  success = 1,
  cannotSaveProducts = 2,
  failed = 3
}

export class InspectionPickingExistRequest {
  poTransactionIds: Array<number>;
}

export enum BookingUnit {
  Set = 1,
  Pairs = 2,
  Yards = 3,
  Pieces = 4
}

export enum BookingProductFieldType {
  Aql = 1,
  Critical = 2,
  Major = 3,
  Minor = 4,
  Unit = 5,
  UnitCount = 6,
  BarCode = 7,
  FactoryReference = 8,
  ProductCategory = 9,
  ProductSubCategory = 10,
  ProductSubCategory2 = 11,
  CustomSampleTypeName = 12,
  CustomSampleSize = 13,
  IsEcoPack = 14,
  FBTemplate = 15,
  DisplayChild = 16,
  Remarks = 17,
  ProductSubCategory3 = 18,
}

export class DraftInspectionBooking {

  id: number;
  customerId: number;
  supplierId: number;
  factoryId: number;
  serviceDateFrom: DateObject;
  serviceDateTo: DateObject;
  brandId: number;
  departmentId: number;

  bookingInfo: string;
  isReInspectionDraft: boolean;
  IsReBookingDraft: boolean;
  PreviousBookingNo: number;

}

export class EntPageRequest {
  rightId: number;
  serviceId: number;
  customerId: number;
  entityId: number;
}

export class EntPageFieldAccessResponse {
  entPageFieldAccess: Array<EntPageFieldAccess>;
  result: EntPageFieldAccessResult;
}

export class EntPageFieldAccess {
  id: number;
  name: string;
  customerId: number;
}

export enum EntPageFieldAccessResult {
  Success = 1,
  NotFound = 2
}

export enum DraftInspectionResult {
  Success = 1,
  NotFound = 2
}

export enum SaveDraftInspectionResult {
  Success = 1,
  Failure = 2,
  DuplicateEmailFound = 3
}

export enum DeleteDraftInspectionResult {
  DeleteSuccess = 1,
  Error = 2,
  NotFound = 3
}

export enum BookingViewType {
  DraftBooking = 1,
  NewBooking = 2
}

export class BookingProductTableMaster {
  isPoQtyVisible: boolean;
  isEcoPackVisible: boolean;
  isDisplayMasterVisible: boolean;
  isDisplayChildVisible: boolean;
  isCustomerRefPOVisible: boolean;
  isProductSubCategory2Visible: boolean;
  isPicking: boolean;
  isCombine: boolean;
  isBarcode: boolean;
  isFactoryReference: boolean;
  isFirstServiceDate: boolean;

}

export class PoProductDetailRequest {
  poId: number
  productId: number
  customerId: number
  supplierId: number;
  filterPoByProduct: boolean;
}

export class POProductDetailResponse {
  poProductDetail: POProductDetail;
  result: number;
}

export enum POProductDetailResult {
  Success = 1,
  NotFound = 2
}

export class POProductDetail {
  id: number;
  name: string;
  description: string;
  poQuantity: number;
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
  isNewProduct: boolean;
  remarks: string;
  productImageCount: number;
  destinationCountryId: number;
  etd: string;
}

export enum BookingFileZipResult {
  Success = 1,
  NotFound = 2
}


export class UploadPurchaseOrderModel {
  public purchaseOrderAttachments: Array<AttachmentFile>;
  public customerId: number;
  constructor() {
    this.purchaseOrderAttachments = [];
  }
}


export class AttachmentFile {
  public id: number;
  public uniqueld: string;
  public fileName: string;
  public isNew: boolean;
  public mimeType: string;
  public file: File;
}

export class ProductDetails {
  public product: string;
  public pono: string;
  public barCode: string;
  public productDescription: string;
  public factoryReference: string;
  public selected: boolean;
}


export enum ProductUploadErrorData {
  PoNoMandatory = 1,
  ProductRefMandatory = 2,
  QuantityMandatory = 3,
  EtdNotValidDateFormat = 4,
  CountryNotAvailable = 5,
  ExistingPoList = 6
}


export class PoProductUploadRequest {
  customerId: number;
  supplierId: number;
  businessLineId:number;
  existingBookingPoProductList: Array<ExistingBookingPoProductData>;
  constructor() {
    this.existingBookingPoProductList = [];
  }
}

export class ExistingBookingPoProductData {
  poId: string;
  productId: number;
}

export enum POProductUploadResult {
  Success = 1,
  NotAbleToProcess = 2,
  ValidationError = 3,
  EmptyRows=4
}

export enum PurchaseOrderSampleFile {
  ImportPOSampleFile = 1,
  ImportPODateFormat = 2
}
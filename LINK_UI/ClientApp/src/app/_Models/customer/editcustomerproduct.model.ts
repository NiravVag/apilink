export class EditCustomerProductModel {
  public id: number;
  public productID: string;
  public productDescription: string;
  public customerID: number;
  public barcode: string;
  public factoryReference: string;
  public photo: string;
  public productCategory: string;
  public productSubCategory: string;
  public productCategorySub2: number;
  public productCategorySub3: number;

  public productCategoryName: string;
  public productSubCategoryName: string;
  public productCategorySub2Name: string;
  public productCategorySub3Name: string;

  public remarks: string;
  public isProductBooked: boolean;
  public isBooked: boolean;
  public cuProductFileAttachments: Array<AttachmentFile>;
  public apiServiceIds: any;
  public isNewProduct: boolean;
  public isMsChart: boolean;
  public isStyle: boolean;
  public timePreparation: number;
  public sampleSize8h: number;
  public tpAdjustmentReason: string;
  public technicalComments: string;
  public unit: number;
  public screenCallType: number;
  constructor() {
    this.id = 0;
    this.cuProductFileAttachments = new Array<AttachmentFile>();
    this.isNewProduct = true;
  }
}

export class AttachmentFile {
  public id: number;
  public uniqueld: string;
  public fileName: string;
  public fileSize: number;
  public isNew: boolean;
  public mimeType: string;
  public fileUrl: string;
  public file: File;
  public isSelected: boolean;
  public status: number = 0;
  public fileTypeId?: number;
  public productMsCharts: any;
  public fileDescription: string;
}

export class CustomerProductDetailResponse {
  productList: Array<CustomerProductDetail>;
  result: CustomerProductDetailResult;
}

export enum CustomerProductDetailResult {
  Success = 1,
  CannotGetProducts = 2,
  NotFound = 3
}


export class CustomerProductDetail {
  id: number;
  productName: string;
  productDescription: string;
  customerId: number;
  barcode: string;
  productCategoryId: number;
  productCategoryName: string;
  productSubCategoryId: number;
  productSubCategoryName: string;
  productSubCategory2Id: number;
  productSubCategory2Name: string;
  productSubCategory3Id: number;
  productSubCategory3Name: string;
  factoryReference: string;
  remarks: string;
  active: boolean;
  productImageCount: number;
}

export class PoProductRequest {
  productIds: Array<number>;
  constructor() {
    this.productIds = [];
  }
}

export enum ProductScreenCallType {
  Product = 1,
  Booking = 2,
  PurchaseOrder = 3,
  PoUpload = 4
}

export class CustomerProductFileResponse {
  productFileUrls: Array<string>;
  result: CustomerProductFileResult;
}

export enum CustomerProductFileResult {
  Success = 1,
  FileNotFound = 2
}

export class BookingProductData {
  id: number;
  name: string;
}
import { DataList } from "../useraccount/userconfig.model";
import { summaryModel } from "../summary.model";

export class MasterCustomerPriceCard {
  customerList: Array<DataList>;
  serviceList: Array<DataList>;
  productCategoryList: Array<DataList>;
  currencyList: Array<DataList>;
  countryList: Array<DataList>;
  provinceList: Array<DataList>;
  billingMethodList: Array<DataList>;
  billingToList: Array<DataList>;
  supplierList: Array<DataList>;
  serviceTypeList: Array<DataList>;
  tariffTypeList: Array<DataList>;
  customerBrandList: Array<DataList>;
  customerBuyerList: Array<DataList>;
  customerCategoryList: Array<DataList>;
  customerProductSubCategoryList: Array<DataList>;
  customerProductSub2CategoryList: Array<DataList>;
  customerDepartmentList: Array<DataList>;
  customerPriceCategoryList: Array<DataList>;
  customerPriceHolidayList: Array<DataList>;
  invoiceRequestList: Array<DataList>;
  invoiceOfficeList: Array<DataList>;
  invoiceFeesTypeList: Array<DataList>;
  travelExpenseFeesList: Array<DataList>;
  hotelFeesList: Array<DataList>;
  discountFeesList: Array<DataList>;
  otherFeesList: Array<DataList>;
  invoicePaymentTypeList: Array<DataList>;
  invoiceBankList: Array<DataList>;
  billingEntityList: Array<DataList>;
  customerAddressList: Array<DataList>;
  customerContactList: Array<DataList>;
  inspectionLocationList: Array<DataList>;
  billFrequencyList: Array<DataList>;
  billQuantityTypeList: Array<DataList>;
  interventionTypeList: Array<DataList>;
  maxStyleTypeList: Array<DataList>;
  cityList: Array<DataList>;
  tariffTypeLoading: boolean;
  isProvinceShow: boolean;
  serviceTypeLoading: boolean;
  supplierLoading: boolean;
  billingToLoading: boolean;
  billingMethodLoading: boolean;
  provinceLoading: boolean;
  countryLoading: boolean;
  currencyLoading: boolean;
  customerLoading: boolean;
  serviceLoading: boolean;
  productCategoryLoading: boolean;
  productSubCategoryLoading: boolean;
  productSubCategory2Loading: boolean;
  saveLoading: boolean;
  cancelLoading: boolean;
  customerBrandLoading: boolean;
  customerBuyerLoading: boolean;
  customerCategoryLoading: boolean;
  customerDepartmentLoading: boolean;
  customerPriceCategoryLoading: boolean;
  customerPriceHolidayLoading: boolean;
  invoiceRequestLoading: boolean;
  invoiceOfficeLoading: boolean;
  invoiceFeesLoading: boolean;
  invoicePaymentLaoding: boolean;
  invoiceBankLoading: boolean;
  billingEntityLoading: boolean;
  customerAddressLoading: boolean;
  customerContactsLoading: boolean;
  toggleInvoiceSection: boolean;
  toggleCategorySection: boolean = true;
  isInvoiceConfigured: boolean;
  hotelfeeVisible: boolean;
  inspectionLocationLoading: boolean;
  billFrequancyLoading: boolean;
  billQuantityTypeLoading: boolean;
  interventionTypeLoading: boolean;
  hasMandayComplexAccess: boolean = false;
  hasSamplingComplexAccess: boolean = false;
  hasPieceRateComplexAccess: boolean = false;
  hasInterventionComplexAccess: boolean = false;
  cityLoading: boolean;
  showCityField: boolean;
}

export enum PriceComplexType {
  Simple = 1,
  Complex = 2
}
export class CustomerPriceCard {
  id: number;
  customerId: number;
  tariffTypeId: number;
  billingMethodId: number;
  billingToId: number;
  serviceId: number;
  priceComplexType: number;
  currencyId: number;
  unitPrice: number;
  holidayPrice: number;
  productPrice: number;
  taxIncluded: boolean;
  travelIncluded: boolean;
  freeTravelKM: number;
  remarks: string;
  periodFrom: any;
  periodTo: any;
  factoryCountryIdList: Array<number>;
  factoryProvinceIdList: Array<number>;
  productCategoryIdList: Array<number>;
  productSubCategoryIdList: Array<number>;
  serviceTypeIdList: Array<number>;
  supplierIdList: Array<number>;
  departmentIdList: Array<number>;
  brandIdList: Array<number>;
  buyerIdList: Array<number>;
  priceCategoryIdList: Array<number>;
  holidayTypeIdList: Array<number>;
  maxProductCount: number;
  sampleSizeBySet: boolean = false;
  priceToEachProduct: boolean = false;
  minBillingDay: number;
  maxSampleSize: number;
  additionalSampleSize: number;
  additionalSamplePrice: number;
  quantity8: number;
  quantity13: number;
  quantity20: number;
  quantity32: number;
  quantity50: number;
  quantity80: number;
  quantity125: number;
  quantity200: number;
  quantity315: number;
  quantity500: number;
  quantity800: number;
  quantity1250: number;

  invoiceRequestSelectAll: boolean;
  isInvoiceConfigured: boolean;

  invoiceRequestType: number;
  invoiceRequestBilledName: string;
  invoiceRequestAddress: string;

  customerAddressId: number;


  billingEntity: number;
  paymentDuration: number;
  paymentTypeId: number;
  paymentTerms: string;

  invoiceNoDigit: string;
  invoiceNoPrefix: string;

  bankAccount: number;

  invoiceInspFeeFrom: number;
  invoiceTravelExpense: number;

  invoiceHotelFeeFrom: number;
  invoiceOtherFeeFrom: number;
  invoiceDiscountFeeFrom: number;
  invoiceOffice: number;

  invoiceRequestList: Array<PriceInvoiceRequest>;
  subCategoryList: Array<PriceSubCategory>;
  ruleList: Array<PriceSpecialRule>;
  invoiceRequestContact: Array<number>;

  inspectionLocationList: Array<number>;
  billQuantityType: number;
  interventionType: number;
  maxFeeStyle: number;
  invoiceSubject: string;
  billFrequency: number;
  customerSegment: string;
  customerCountry: string;
  subCategorySelectAll: boolean;
  isSpecial: boolean;
  maxStyleType: number;

  mandayProductivity: number;
  mandayReports: number;
  mandayBuffer: number;
  isSelectAllSubCategory: boolean;
  factoryCityIdList: Array<number>;
  constructor() {
    this.id = 0;
    this.factoryCountryIdList = new Array<number>();
    this.factoryProvinceIdList = new Array<number>();
    this.productCategoryIdList = new Array<number>();
    this.serviceTypeIdList = new Array<number>();
    this.supplierIdList = new Array<number>();
    this.departmentIdList = new Array<number>();
    this.brandIdList = new Array<number>();
    this.buyerIdList = new Array<number>();
    this.priceCategoryIdList = new Array<number>();
    this.holidayTypeIdList = new Array<number>();
    this.invoiceRequestList = new Array<PriceInvoiceRequest>();
    this.invoiceRequestContact = new Array<number>();
    this.inspectionLocationList = new Array<number>();
    this.subCategoryList = new Array<PriceSubCategory>();
    this.ruleList = new Array<PriceSpecialRule>();
    this.unitPrice = 0;
    this.factoryCityIdList = new Array<number>();
  }
}


export class PriceSubCategory {
  id: number;
  cuPriceCardId: number;
  subCategory2Id: number;
  mandayProductivity: number;
  unitPrice: number;
  mandayReports: number;
  mandayBuffer: number;
  subCategory2Name: string;
  aqL_QTY_8: number;
  aqL_QTY_13: number;
  aqL_QTY_20: number;
  aqL_QTY_32: number;
  aqL_QTY_50: number;
  aqL_QTY_80: number;
  aqL_QTY_125: number;
  aqL_QTY_200: number;
  aqL_QTY_315: number;
  aqL_QTY_500: number;
  aqL_QTY_800: number;
  aqL_QTY_1250: number;
  isCommon: boolean = false;
}



export class PriceSpecialRule {
  id: number;
  cuPriceCardId: number;

  mandayProductivity: number;
  mandayReports: number;
  unitPrice: number;

  pieceRate_Billing_Q_Start: number;
  piecerate_Billing_Q_End: number;
  additionalFee: number;
  piecerate_MinBilling: number;
  perInterventionRange1: number;
  perInterventionRange2: number;
  max_Style_Per_Day: number;
  max_Style_Per_Week: number;
  max_Style_per_Month: number;
  interventionfee: number;
}


export class PriceInvoiceRequest {
  id: number;
  cuPriceCardId: number;
  billedName: string;
  billedAddressId: number;
  billedAddress: string;
  departmentId: number;
  brandId: number;
  productCategoryId: number;
  buyerId: number;
  invRequestName: string;
  isCommon: boolean = false;
  invoiceRequestContactList: Array<number>;
}

export class PriceInvoiceRequestContact {
  id: number;
  cuPriceCardId: number;
  invoiceRequestId: number;
  contactId: number;
  isCommon: boolean;
}

export class SaveCustomerPriceCardResponse {
  result: CustomerPriceCardResponseResult;
  id: number;
}
export class EditCustomerPriceCardResponse {
  getData: CustomerPriceCard;
  result: CustomerPriceCardResponseResult;
}

export enum CustomerPriceCardResponseResult {
  Success = 1,
  Faliure = 2,
  RequestNotCorrectFormat = 3,
  NotFound = 4,
  Error = 5,
  Exists = 6,
  MoreRuleExists = 7,
  NoQuotationCommonDataMatch = 8
}

export enum UnitPriceResponseResult {
  Success = 1,
  NotFound = 2,
  MoreRuleExists = 3,
  SingleRuleExists = 4,
}

export enum SamplingPriceResponseResult {
  Success = 1,
  NotFound = 2
}

export enum InvoiceRequestType {
  Brand = 1,
  Department = 2,
  Buyer = 3,
  NotApplicable = 4,
  ProductCategory = 5
}

export enum InvoiceFeesFrom {
  Invoice = 1,
  Quotation = 2,
  Carrefour = 3,
  NotApplicable = 4
}

export enum InvoiceBillingTo {
  Customer = 1,
  Supplier = 2,
  Factory = 3,
  SGTCustomer = 4,
  SGTSupplier = 5,
  SGTFactory = 6

}

export class CustomerPriceCardSummary extends summaryModel {
  id: number;
  customerId: number;
  billingMethodId: number;
  billingToId: number;
  serviceId: number;
  taxIncluded: boolean;
  travelIncluded: boolean;
  periodFrom: any;
  periodTo: any;
  productCategoryIdList: Array<number>;
  serviceTypeIdList: Array<number>;
  countryIdList: Array<number>;
  departmentIdList: Array<number>;
  priceCategoryIdList: Array<number>;
  searchLoading: boolean;

  constructor() {
    super();
    this.noFound = false;
  }

}

export class MasterCustomerPriceCardSummary {
  billingMethodLoading: boolean;
  billingToLoading: boolean;
  customerLoading: boolean;
  serviceTypeLoading: boolean;
  productCategoryLoading: boolean;
  productSubCategoryLoading: boolean;
  serviceLoading: boolean;
  countryLoading: boolean;
  departmentLoading: boolean;
  priceCategoryLoading: boolean;

  billingMethodList: Array<DataList>;
  billingToList: Array<DataList>;
  customerList: Array<DataList>;
  serviceTypeList: Array<DataList>;
  countryList: Array<DataList>;
  serviceList: Array<DataList>;
  productCategoryList: Array<DataList>;
  productSubCategoryList: Array<DataList>;
  priceCategoryList: Array<DataList>;
  departmentList: Array<DataList>;

  searchLoading: boolean;
  exportDataLoading: boolean;
  isShowColumn: boolean;
  isShowColumnImagePath: string;
  showColumnTooltip: string;
  constructor() {
    this.isShowColumn = true;
  }
}

export class CustomerPriceCardSummaryItem {
  id: number;
  customerName: string;
  billMethodName: string;
  billToName: string;
  serviceName: string;
  unitPrice: number;
  currencyName: string;
  periodFrom: string;
  periodTo: string;
  factoryCountryList: string;
  serviceTypeList: string;
  travelIncluded: string;
  departmentName: string;
  priceCategory: string;
  createdByName: string;
  createdOn: string;
  updatedByName: string;
  updatedOn: string;
}
export class CustomerPriceCardSummaryResponse {
  getData: Array<CustomerPriceCardSummaryItem>;
  result: CustomerPriceCardResponseResult;
}

export class QuotationCustomerPriceCard {
  public customerName: string;
  public billingMethodName: string;
  public serviceTypeNames: string;
  public productCategoryNames: string;
  public unitPrice: number;
  public freeTravelKM: number;
  public remarks: string;
  public taxIncluded: string;
  public travelIncluded: string;
  public factoryCountryNames: string;
  public periodFrom: string;
  public periodTo: string;
}

export class CustomerPriceCardRequest {
  bookingId: number;
  ruleId: number;
  quotationId: number;
  billMethodId: number;
  billPaidById: number;
  currencyId: number;
}

export class CustomerPriceCardUnitPriceRequest {
  bookingIds: Array<number>;
  billMethodId: number;
  billPaidById: number;
  currencyId: number;
  ruleId: number;
  quotationId: number;
  billQuantityType: number;
  invoiceType: number;
  paymentTermsValue: string;
  paymentTermsCount: number;
}

export class CustomerPriceCardUnitPrice {
  bookingId: number;
  unitPrice: number;
  billQuantityType: number;
  totalBillQuantity: number;
}

export class CustomerPriceCardUnitPriceResponse {
  unitPriceList: Array<CustomerPriceCardUnitPrice>;
  result: CustomerPriceCardResponseResult;
}

export class UnitPriceDetailModel {
  bookingId: number;
  oldUnitPrice: number;
  newUnitPrice: number;
  isSelect: boolean;
}
export class QuotationPriceCard {
  priceCardDetails: QuotationCustomerPriceCard;
  result: CustomerPriceCardResponseResult;
}

export class SamplingUnitPriceRequest {
  bookingId: number;
  priceCardId: number;
}

export class SamplingUnitPriceResponse {
  unitPriceList: Array<SamplingUnitPrice>;
  result: SamplingPriceResponseResult;
}

export class SamplingUnitPrice {
  public bookingId: number;
  unitPrice: number;
}

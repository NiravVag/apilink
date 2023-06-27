import { CountryModel, EditOfficeModel } from "../office/edit-officemodel";
import { NgbDate } from "@ng-bootstrap/ng-bootstrap";
import { Currency } from "../currency/echange-rate.model";
import { Validator } from "../../components/common";
import { APIService } from "src/app/components/common/static-data-common"
import { NumberSymbol } from "@angular/common";
import { DataList } from "../useraccount/userconfig.model";

export interface QuotationResponse {
  billQuantityTypeList: Array<DataList>;
  countryList: Array<CountryModel>;
  serviceList: Array<Service>;
  billingMethodList: Array<BillingMethod>;
  billPaidByList: Array<BillPaidBy>;
  billingEntities: Array<BillPaidBy>;
  officeList: Array<DataSource>;
  currencyList: Array<Currency>;
  result: QuotationResult
  model: QuotationModel;
  customerList: Array<DataSource>;
  supplierList: Array<DataSource>;
  factoryList: Array<DataSource>;
  abilities: Array<QuotationAbility>;
  paymentTermList: Array<BillPaidBy>;
  factoryBookingDetail: FactoryBookingDetail;
  paymentTermsValueList: Array<PaymentTypeSource>;
  isPreInvoiceContactMandatoryInQuotation: boolean;
}


export enum QuotationResult {
  Success = 1,
  CannotFindCountryList = 2,
  CannotFindServiceList = 3,
  CannotFindBillingMethodList = 4,
  CannotFindBillPaidByList = 5,
  CannotFindOfficeList = 6,
  CannotFindCurrencies = 7,
  CurrentQuotationNotFound = 8,
  CannotGetCustList = 9,
  CannotGetSuppList = 10,
  CannotGetFactoryList = 11,
  NoAccess = 12
}

//Quotation billing method enum
export enum QuotationBillMethod {
  Manday = 1,
  Sampling = 2,
  PieceRate = 3,
  PerIntervention = 4
}
export enum QuotationBillPaidBy {
  customer = 1
}

export interface Service {
  id: number;
  name: string;
}

export class QuotationFromBooking {
  public customerid: number;
  public supplierid: number;
  public factoryid: number;
  public factorycountryid: number;
  public bookingitems: number[];
  public service: APIService;
  public officeid: number;
}

export interface BillingMethod {
  id: number;
  label: string;
}

export interface BillPaidBy {
  id: number;
  name: string;
}

export class QuotationModel {
  public id: number;
  public country: CountryModel;
  public service: Service;
  public billingMethod: BillingMethod;
  public billingPaidBy: BillPaidBy;
  public customer: DataSource;
  public customerLegalName: string;
  public customerContactList: Array<QuotationEntityContact>;
  public supplier: DataSource;
  public supplierLegalName: string;
  public supplierContactList: Array<QuotationEntityContact>;
  public grade: string;
  public factory: DataSource;
  public legalFactoryName: string;
  public factoryContactList: Array<QuotationEntityContact>;
  public factoryAddress: string;
  public office: DataSource;
  public internalContactList: Array<QuotationEntityContact>;
  public inspectionFees: number;
  public estimatedManday: number;
  public currency: Currency;
  public travelCostsAir: number;
  public travelCostsLand: number;
  public travelCostsHotel: number;
  public otherCosts: number;
  public discount: number;
  public totalCost: number;
  public apiRemark: string;
  public customerRemark: string;
  public orderList: Array<Order>;
  public statusId: QuotationStatus;
  public statusLabel: string;
  public isToForward: boolean;
  public quotationPDFList: Array<QuotationPDFVersion>;
  public apiInternalRemark: string;
  public billingEntity: number;
  public confirmDate: any;
  public paymentTerm: number;
  public paymentTermId: number;
  public paymentTermsValue: string;
  public paymentTermsCount: number;
  public skipclientconfirmation: boolean;
  public isBookingInvoiced: boolean;
  public skipQuotationSentToClient: boolean;
  public ruleId: number;
  public factoryBookingInfoList: Array<FactoryBookingInfo>;
}
export class PreviewMainDetails {
  public customer: string;
  public supplier: string;
  public factory: string;
  public country: string;
  public service: string;
  public billingMethod: string;
  public billPaidBy: string
  public office: string;
  public factoryAddress: string;
  public customerContactList: Array<QuotationEntityContact>;
  public factoryContactList: Array<QuotationEntityContact>;
  public supplierContactList: Array<QuotationEntityContact>;
}
export class PreviewBookInfo {
  public totalBooking: number;
  public totalReports: number;
  public totalContainers: number;
  public totalCombine: number;
  public totalSampling: number;
  public totalProducts: number;
  public totalPicking: number;
  public totalProductsList: any = [];
}
export interface QuotationPDFVersion {
  id: string;
  fileName: string;
  sendDate: string;
}

export class QuotationEntityContact {
  public contactId: number;
  public contactName: string;
  public contactEmail: string;
  public contactPhone: string;
  public quotation: boolean;
  public email: boolean;
  public entityType: QuotationEntityType;
  public customerAE: boolean;
  public invoiceEmail: boolean;
}


export enum QuotationEntityType {
  Customer = 1,
  Supplier = 2,
  Factory = 3,
  Internal = 4
}


export interface DataSource {
  id: number;
  name: string;
  isForwardToManager: boolean;
  countyId: number;
  cityId: number;
  provinceId: number;
  invoiceType: number;
  statusName: string;
}


export class QuotationDataSourceResponse {
  public dataSource: Array<DataSource>;
  public result: QuotationDataSourceResult;
}

export enum QuotationDataSourceResult {
  Success = 1,
  CountryEmpty = 2,
  ServiceEmpty = 3,
  NotFound = 4
}


export class QuotationContactListResponse {
  public data: Array<QuotationEntityContact>;

  public result: QuotationContactListResult;
}

export enum QuotationContactListResult {
  Success = 1,
  NotFound = 2
}

export class FilterOrderRequest {
  public customerId: number;

  public supplierId: number;

  public factoryId: number;

  public serviceId: number;

  public startDate: NgbDate;

  public endDate: NgbDate;

  public bookingNo: number;

  public bookingIds: number[];

  public billMethodId: number;

  public billPaidById: number;

  public currencyId: number;
}

export interface OrderListResponse {
  data: Array<Order>;
  result: OrderListResult;
}

export class Order {
  id: number;
  productCategory: string;
  serviceTypeList: Array<ServiceType>;
  inspectionFrom: string;
  inspectionTo: string;
  serviceTypeStr: string;
  office: string;
  locationId: string;
  productList: Array<QuotProduct>;
  containerList: any;
  productValidatorList: Array<productValidation>;
  isChecked: boolean;
  internalBookingRemarks: string;
  qcBookingRemarks: string
  orderCost: OrderCost;
  ordervalidator: orderValidation;
  quotationMandayList: Array<QuotationManday>;
  orderMandayvalidatorList: Array<orderMandayValidation>;
  isPicking: boolean;
  isContainerServiceType: boolean;
  priceCategoryName: string;
  previousBookingNo: Array<number>;
  statusName: string;
  calculatedWorkingHours: number;
  calculatedWorkingManday: number;
  bookingZipFileUrl: string;
  paymentOption: string;
  dynamicFieldData: Array<DynamicFieldData>;
}

export class OrderCost {
  unitPrice: number;
  noOfManday: number;
  inspFees: number;
  travelAir: number;
  travelLand: number;
  travelHotel: number;
  customerPriceCardId: number;
  travelManday: number;
  travelDistance: number;
  travelTime: number;
  calculatedWorkingHours: number;
  calculatedManday: number;
  quantity: number;
  billedQtyType: number;
}

export class DynamicFieldData {
  dynamicFieldName: string;
  dynamicFieldValue: string;
}

export class BookingContainerData {
  containerId: number;
  bookingId: number;
  totalBookingQuantity: number;
  inspectedQuantity: number;
  inspectionDate: string
  reportId: number;
  reportStatus: string;
  containerName: string;
  reportResult: string;
  reportResultId: number;
  reportPath: string;
}
export interface productValidation {
  product: QuotProduct;
  validator: Validator;
}
export interface orderValidation {
  ordercost: OrderCost;
  validator: Validator;
}
export interface orderMandayValidation {
  ordermanday: QuotationManday;
  validator: Validator;
}
export class ServiceType {
  public id: number;
  public name: string;
}

export enum OrderListResult {
  Success = 1,
  NotFound = 2
}


export interface QuotProduct {
  id: number
  productId: string;
  productDescription: string;
  aqlLevel: string;
  aqlLevelAndSampleType: string;
  sampleQty: number;
  aqlLevelDescription: string;
  bookingQty: number;
  unit: string;
  unitAndUnitCount: string;
  factoryReference: string;
  combineProductList: Array<QuotProduct>;
  isExpand: boolean;
  pictList: Array<number>;
  poNo: string;
  destination: string;
  inspPoId: number;
  productSubCategory: string;
  productSubCategory2: string;
  pickingQty: number;
  productRemarks: string;
  combineProductCount: number;
  combineProductId: number;
  isParentProduct: boolean;
}
export class QuoBookingInfoPopUp {
  inspectionFrom: string;
  inspectionTo: string;
  serviceTypeStr: string;
  office: string;
  internalBookingRemarks: string;
  qcBookingRemarks: string;
  priceCategoryName: string;
  prevBookingNo: Array<number>;
  bookingId: number;
  bookingAttachmentUrl: string;
  paymentOption: string;
  dynamicFieldDatas: Array<DynamicFieldData>;
}
export class SaveQuotationResponse {
  public item: QuotationModel;
  public result: SaveQuotationResult;
  public serviceDateChangeInfo: BookingDateChangeInfo;
  public bookingOrAuditNos: Array<number>;
}

export enum SaveQuotationResult {
  Success = 1,
  CannotSave = 2,
  NotFound = 3,
  SuccessWithBrodcastError = 6,
  NoAccess = 4,
  CustomerNotFound = 5,
  ServiceDateChanged = 7,
  QuotationExists = 8,
  BookingIsCancelled = 9,
  BookingIsHold = 10
}

export interface AddressFactoryResponse {
  address: string;
  result: AddressFactoryResult;
}

export enum AddressFactoryResult {
  Success = 1,
  NotFound = 2
}


export enum QuotationAbility {
  CanSave = 1,
  CanApprove = 2,
  CanCancel = 3,
  CanSend = 5,
  CanCustConfirm = 6
}

export enum QuotationStatus {
  QuotationCreated = 1,
  ManagerApproved = 2,
  ManagerRejected = 3,
  QuotationVerified = 4,
  Canceled = 5,
  CustomerRejected = 6,
  CustomerValidated = 7,
  SentToClient = 8,
  AERejected = 9
}

export class BookingDateChangeInfo {
  bookingId: number;
  previousServiceDateFrom: string;
  previousServiceDateTo: string;
  serviceDateFrom: string;
  serviceDateTo: string;
  result: BookingDateChangeInfoResult
}
export enum BookingDateChangeInfoResult {
  Verified = 1,
  DateChanged = 2,
  ServiceNotFound = 3,
  NodateFound = 4,
  Error = 5
}


export class SetStatusRequest {
  id: number;
  statusId: number;
  cuscomment: string;
  apiRemark: string;
  apiInternalRemark: string;
  confirmDate: any;
}

export interface SetStatusQuotationResponse {
  item: QuotationModel;
  result: SetStatusQuotationResult;
  serviceDateChangeInfo: BookingDateChangeInfo;
}

export enum SetStatusQuotationResult {
  Success = 1,
  CannotUpdateStatus = 2,
  NoAccess = 3,
  StatusNotConfigued = 4,
  QuotationNotFound = 5,
  SuccessButErrorBrodcast = 6,
  ServiceDateChanged = 7,
  BookingNotConfirmed = 8,
  BookingIsCancelled = 9,
  BookingIsHold = 10
}

export class QuotationLoader {
  saveLoading: boolean = false;
  bookingSearchLoading: boolean = false;
  customerLoading: boolean = false;
  supplierLoading: boolean = false;
  factoryLoading: boolean = false;
  selectOrderLoading: boolean = false;
}
export class QuotationManday {
  bookingId: number;
  serviceDate: string;
  manDay: number
  remarks: string
}

export enum ManDayResult {
  Success = 1,
  NotFound = 2
}
export interface QuotationManDayResponse {
  quotationMandaysList: Array<QuotationManday>;
  mandayResult: ManDayResult;
}
export class QuoationMandayrequest {
  bookingId: Array<number>;
}

export enum BillingEntity {
  AsiaPacificInspectionLtdHONGKONG = 1,
  GuangzhouOuyataiCHINA = 2,
  AsiaPacificInspectionVietnamCompanyLtdVIETNAM = 3,
  APIAuditLimitedHONGKONGAudit = 4
}

export enum PaymentTerm {
  MonthlyInvoice = 1,
  PreInvoice = 2
}

export class QuotCheckpointRequest {
  bookingIdList: Array<number>;
  customerId: number;
}

export class CalculatedWorkingHoursReponse {
  calculatedWorkingHours: number;
  calculatedManday: number;
  result: CalculatedWorkingHoursResult;
  saveResult: CalculateManDaySaveResult;
}

export enum CalculatedWorkingHoursResult {
  success = 1,
  fail = 2,
  prodCatSub3NotMapped = 3,
  unitNotPcs = 4
}

export class PriceCardTravelResponse {
  isTravelInclude: boolean;
  result: PriceCardTravelResult;
}

export enum PriceCardTravelResult {
  Success = 1,
  NotFound = 2
}

export enum CalculateManDaySaveResult {
  Success = 1,
  Fail = 2
}

export class SupplierGradeRequest {
  customerId: number;
  supplierId: number;
  bookingIds: Array<number>;
}

export enum SupplierGradeResult {
  Success = 1,
  NotFound = 2,
  CustomerRequired = 3,
  SupplierRequired = 4,
  BookingIdsRequired = 5
}


export class FactoryBookingDetail {
  totalBooking: number;
  totalItem: number;
  totalReport: number;
  totalSampleSize: number;
  totalManday: number;
  totalTravelCost: number;
  totalInspectionFee: number;
  totalOtherCost: number;
  totalFee: number;
  totalAverageManday: number;
  totalSuggestedManday: number;
  constructor() {
    this.totalBooking = 0;
    this.totalItem = 0;
    this.totalReport = 0;
    this.totalSampleSize = 0;
    this.totalManday = 0;
    this.totalTravelCost = 0;
    this.totalInspectionFee = 0;
    this.totalOtherCost = 0;
    this.totalFee = 0;
    this.totalAverageManday = 0;
  }
}

export class FactoryBookingInfo {
  bookingId: number;
  customerName: string;
  serviceDate: string;
  productCount: number;
  reportCount: number;
  sampleSize: number;
  manday: number;
  inspectionFee: number;
  travelCost: number;
  otherCost: number;
  total: number;
  averageManday: number;
  suggestedManday: number;
  isQuotation: boolean;
  serviceTypeName: string;
}

export class FactoryBookingInfoRequest {
  factoryId: number;
  bookingIds: Array<number>;
}

export enum FactoryBookingInfoResult {
  Success = 1,
  NotFound = 2,
  RequestNotCorrectFormat = 3
}

export class PaymentTypeSource {
  id: number;
  name: string;
  duration: number;
}

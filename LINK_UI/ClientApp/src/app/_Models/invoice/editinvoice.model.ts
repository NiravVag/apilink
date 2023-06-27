import { DataSource } from '../../_Models/common/common.model';
import { DataList } from "../useraccount/userconfig.model";
import { DateObject } from "src/app/components/common/static-data-common";
export class InvoiceBaseDetail {
    invoiceNo: string;
    oldInvoiceNo: string;
    invoiceDate: DateObject;
    postDate: DateObject;
    subject: string;
    billTo: number;
    invoiceType: number;
    billedName: string;
    billedAddress: string;
    contactIds: Array<number>;
    paymentTerms: string;
    paymentDuration: string;
    invoiceStatus: string;
    office: number;
    bankDetails: InvoiceBankDetail;
    billMethod: number;
    currency: string;
    paymentStatus: number;
    taxValue: number;
    paymentDate: DateObject;
    customerId: number;
    supplierId: number;
    factoryId: number;
    totalInvoiceFees: number;
    totalTaxAmount: number;
    totalTravelFees: number;
    isTravelExpense :boolean;
    isInspectionFees :boolean;
    invoicingRequest:number;
    billedQuantityType:string;
    invoiceCurrencyName:string;
    invoiceCurrency:number;
    exchangeRate:number;
}

export class NewInvoiceBookingSearchRequest
{
    bookingNumber: number;
    customerId:number;
    supplierId:number;
    factoryId:number;
    serviceId:number;
    billedTo:number;
    invoiceType:number;
    bookingStartDate: DateObject;
    bookingEndDate: DateObject;
}

export class InvoiceBankDetail {
    bankId:number;
    accountName: string;
    accountNumber: string;
    bankName: string;
    bankAddress: string;
    accountCurrency: string;
}

export class InvoiceBaseDetailResponse {
    invoiceBaseDetail: InvoiceBaseDetail
    result: InvoiceBaseDetailResult
}

export enum InvoiceBaseDetailResult {
    Success = 1,
    NotFound = 2,
    InvoiceNoEmpty = 3
}

export class EditInvoiceMaster {
    billingMethodList: Array<DataList>;
    invoiceTypeList: any;
    billingToList: Array<DataList>;
    billingAddressList: Array<DataList>;
    invoicePaymentStatusList: Array<DataList>;
    invoiceOfficeList: Array<DataList>;
    invoicePaymentTermList: Array<DataList>;
    contacts: Array<DataList>;
    paymentTerms: Array<DataList>;
    office: Array<DataList>;
    bankDetails: string;
    bankId: number;
    billingMethodLoading: boolean;
    billingToLoading: boolean;
    invoiceBilledAddressLoading: boolean;
    invoiceContactsLoading: boolean;
    invoicePaymentStatusLoading: boolean;
    invoiceOfficeLoading: boolean;
    invoicePaymentTermsLoading: boolean;
    invoiceTypeLoading:boolean;

    inspectionFeesTotal: string;
    airCostTotal: string;
    landCostTotal: string;
    otherTravelCostTotal: string;
    hotelCostTotal: string;
    otherCostTotal: string;
    discountTotal: string;
    extraFeeTotal: string;
    extraFeeSubTotal: string;
    extraFeeTax: string;

    inspectionFeesTaxTotal: string;
    airCostTaxTotal: string;
    landCostTaxTotal: string;
    otherTravelCostTaxTotal: string;
    hotelCostTaxTotal: string;
    otherCostTaxTotal: string;
    discountTaxTotal: string;
    totalInvoiceFees: string;
    totalTravelFees: number;
    totalTaxAmount: number;
    totalExtarFeeswithTax: number;
    saveDataLoading: boolean;
}

export class InvoiceBilledAddressResponse {
    billedAddress: Array<DataSource>;
    result: InvoiceBilledAddressResult;
}

export enum InvoiceBilledAddressResult {
    Success = 1,
    AddressNotFound = 2,
    BillToIdCannotBeEmpty = 3,
    SearchIdCannotBeEmpty = 4
}

export class InvoiceContactsResponse {
    contacts: Array<DataSource>;
    result: InvoiceBilledContactsResult;
}

export enum InvoiceBilledContactsResult {
    Success = 1,
    ContactsNotFound = 2,
    BillToIdCannotBeEmpty = 3,
    SearchIdCannotBeEmpty = 4
}

export class UpdateInvoiceDetailRequest {
    invoiceBaseDetail: UpdateInvoiceBaseDetail;
    invoiceDetails: Array<UpdateInvoiceDetail>;
}

export class UpdateInvoiceBaseDetail {
    invoiceNo: string;
    invoiceDate: DateObject;
    postDate: DateObject;
    subject: string;
    billTo: number;
    serviceId: number;
    billedName: string;
    billedAddressId: number;
    billedAddress: string;
    contactIds: Array<number>;
    paymentTermsId: number;
    paymentTerms: string;
    paymentDuration: string;
    office: number;
    billMethod: number;
    currency: number;
    invoicePaymentStatus: number;
    invoicePaymentDate: DateObject;
    totalInvoiceFees: number;
    totalTaxAmount: number;
    totalTravelFees: number;
}

export class UpdateInvoiceDetail {
    id: number;
    bookingNo: number;
    manDays: number;
    unitPrice: number;
    inspectionFees: number;
    travelAirFees: number;
    travelLandFees: number;
    hotelFees: number;
    otherFees: number;
    discount: number;
    travelOtherFees: number;
    remarks: string;
    totalInspectionFees:number;
    totalTravelFees:number;
    totalTaxAmount:number;

}

export class UpdateInvoiceDetailsResponse {
    public result: UpdateInvoiceDetailResult
}

export enum UpdateInvoiceDetailResult {
    Success = 1,
    Failure = 2
}

export class InvoiceTransactionDetailsResponse {
    transactionDetails: Array<InvoiceTransactionDetails>;
    result: InvoiceTransactionDetailsResult;
}

export enum InvoiceTransactionDetailsResult {
    Success = 1,
    DataNotFound = 2,
    InvoiceNoCannotBeEmptyOrZero = 3
}

export class InvoiceMoExistsResult {
    isInvoiceNoExists: boolean;
}

export class InvoiceTransactionDetails {
    id: number;
    bookingNo: number;
    customerBookingNo: number;
    quotationNo: number;
    serviceDateFrom: DateObject;
    serviceDateTo: DateObject;
    serviceType: string;
    customer: string;
    supplier: string;
    factory: string;
    factoryCountry: string;
    factoryProvince: string;
    factoryCounty: string;
    factoryCity: string;
    factoryTown: string;
    priceCategory: string;
    totalBookingQty: number;
    totalInspectedQty: number;
    manDay: number;
    unitPrice: string;
    inspectionFees: string;
    airCost: string;
    landCost: string;
    hotelCost: string;
    travelOtherFees: string;
    otherCost: string;
    extraFees: string;
    extraFeeSubTotal : string;
    extraFeeTax : string;
    discount: string;
    remarks: string;
    productList: any;
    isPlaceHolderVisible: boolean;
    isTravelExpense: boolean;
    isInspectionFees: boolean;
    totalInspectionFees:number;
    totalTravelFees:number;
    totalTaxAmount:number;

}

export class InvoiceBookingMoreInfoResponse {
    invoiceBookingMoreInfo: InvoiceBookingMoreInfo;
    result: InvoiceBookingMoreInfoResult;
}

export enum InvoiceBookingMoreInfoResult {
    success = 1,
    notFound = 2,
    bookingNoCannotBeEmptyOrZero = 3
}

export class InvoiceBookingMoreInfo {
    bookingNo: number;
    brands: string;
    departments: string;
    priceCategory: string;
    qcNames: string;
    collection: string;
    presentedQuantity: number;
}


export class InvoiceBookingProductsResponse {
    invoiceBookingProducts: Array<InvoiceBookingProducts>;
    result: InvoiceBookingProductResult;
}

export class InvoiceBookingProducts {
    productId: number;
    productName: string;
    productDescription: string;
    poNumber: string;
    bookingQuantity: number;
    presentedQuantity: number;
    inspectionQuantity: number;
    productCategory: string;
    productSubCategory: string;
    productSubCategory2: string;
}

export enum InvoiceBookingProductResult {
    success = 1,
    notFound = 2,
    bookingNoCannotBeEmptyOrZero = 3
}

export class DeleteInvoiceDetailResponse {
    result; DeleteInvoiceDetailResult;
}

export enum DeleteInvoiceDetailResult {
    DeleteSuccess = 1,
    DeleteFailed = 2
}

export enum FeeType {
    InspectionFees = 1,
    TravelAirFees = 2,
    TravelLandFees = 3,
    OtherTravelFees = 4,
    HotelFees = 5,
    OtherFees = 6,
    UnitPrice = 7,
    Discount = 8,
    Manday =9
}

export class  InvoiceNewBookingDetail
{
     bookingId:number;
     bookingQuantity:number;
     serviceDate :string;
     customerId :number;
     supplierId :number;
     factoryId :number;
     priceCategoryId:number;
     customerName :string;
     supplierName :string;
     factoryName :string;
     priceCategoryName :string;
     serviceTypeName :string;
     isChecked:boolean=false;
}

export class InvoiceNewBookingResponse
{
     bookingList :Array<InvoiceNewBookingDetail>;
     result:InvoiceNewBookingResult
}

export enum InvoiceNewBookingResult
{
    success = 1,
    failure = 2,
    nodata = 3
}

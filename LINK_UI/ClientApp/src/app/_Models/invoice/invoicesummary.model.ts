import { array } from "@amcharts/amcharts4/core";
import { BootModuleInfo } from "aspnet-prerendering";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { CountryDataSourceRequest, DataSource } from "../common/common.model";
import { summaryModel } from "../summary.model";
import { DataList } from "../useraccount/userconfig.model";

export class InvoiceSummaryRequestModel extends summaryModel
{
    invoiceFromDate: any;
    invoiceToDate: any;
    customerId: number;
    supplierId: number;
    factoryId: number;
    serviceId: number;
    invoiceTo: number;
    invoiceType: number;
    searchTypeId: number;
    datetypeid: number;
    searchTypeText: string = "";
    invoiceReportTemplateId: number;
    invoiceStatusId: Array<number>;
    invoicekpiTemplateId: number;
    factoryCountryIds: Array<number>;
    officeIdList: Array<number>;
    billingMethodIdList: Array<number>;
    paymentStatusIdList: Array<number>;
}

export class InvoicePdfAvailableRequest
{
     invoiceNumbers: Array<string>;
}
export class InvoiceSummaryModel{

    customerList: any;
    customerLoading: boolean;
    customerInput: BehaviorSubject<string>;

    invoiceTypeList:any;
    invoiceTypeLoading:boolean;

    factoryList: any;
    factoryLoading: boolean;
    factoryInput: BehaviorSubject<string>;

    supplierList: any;
    supplierLoading: boolean;
    supplierInput: BehaviorSubject<string>;

    serviceLoading: boolean;
    serviceList: any;

    billPaidByList: any;
    billPaidByListLoading: boolean;

    invoiceReportTemplateList: any;
    invoiceReportTemplateListLoading: boolean;

    searchloading: boolean;
    viewLoading: boolean;
    exportDataLoading: boolean;
    checkPdfInvoiceLoading:boolean;

    templateBaseUrl: string;
    templateEntityId: string;
    _statuslist: Array<any> = [];

    invoiceStatusList: any;
    statusLoading: boolean;

    kpiTemplateList: any;
    kpiTemplateListLoading: boolean;
    kpiExportLoading: boolean;

    cancelInvoiceLoading: boolean;
    isInvoiceEmailSendButtonVisible:boolean;
    invoicePdfAvailableResponse:InvoicePdfCreatedResponse;

    factoryCountryList: Array<any>;
    factoryCountryLoading: boolean;
    factoryCountryInput: BehaviorSubject<string>;
    factoryCountryRequest: CountryDataSourceRequest;
    billingMethodLoading: boolean;
    invoicePaymentStatusLoading: boolean;
    invoiceOfficeLoading: boolean;
    invoicePaymentStatusList: Array<DataList>;
    invoiceOfficeList: Array<DataList>;
    billingMethodList: Array<DataList>;
    isShowColumn: boolean;
    isShowColumnImagePath: string;
    showColumnTooltip: string;

    constructor() {
        this.factoryInput = new BehaviorSubject<string>("");
        this.customerInput = new BehaviorSubject<string>("");
        this.supplierInput = new BehaviorSubject<string>("");

        this.factoryCountryRequest = new CountryDataSourceRequest();
        this.factoryCountryInput = new BehaviorSubject<string>("");
        this.isShowColumn = true;
    }
}


export class InvoicePdfCreatedResponse
{
      invoiceNumbers:Array<string>;
      result :InvoicePdfCreatedResponseResult;
}

export enum InvoicePdfCreatedResponseResult
{
    PdfCreatedToAllInvoice = 1,
    PdfCreatedToFewInvoice = 2,
    PdfNotCreatedToAnyInvoice = 3,
    RequestIsNotValid = 4
}

export class InvoiceItem {
    id : number;
    invoiceNo: string;
    invoiceDate: any;
    invoiceTo: string;
    invoiceToName: string;
    invoiceTypeId: string;
    invoiceTypeName: string;
    service: string;
    serviceId: string;
    serviceType:string;
    invoiceCurrency: string;
    travelFee: number;
    otherExpense: number;
    hotelFee: number;
    discount: number;
    inspFees: number;
    totalFee: number;
    extraFees: number;
    isInspection: boolean;
    isTravelExpense: boolean;
    invoiceBookingDataList: Array<any>;
    invoiceStatusId: number;
    customerIdList: Array<number>;
    selectedToSendEmail:boolean;
    billTo:number;
    customerName: string;
    factoryCountry: string;
    paymentStatusName: string;
    billingMethodName: string;
    invoiceOfficeName: string;
    bankId: number;
}

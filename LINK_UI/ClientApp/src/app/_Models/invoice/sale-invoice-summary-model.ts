import { BehaviorSubject } from "rxjs";
import { SearchType } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, CustomerCommonDataSourceRequest, DataSource, SupplierDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class SaleInvoiceSummaryModel {
    requestCustomerModel: CustomerCommonDataSourceRequest;
    customerInput: BehaviorSubject<string>;
    customerLoading: boolean;
    public customerList: any;

    supsearchRequest: CommonDataSourceRequest;
    supInput: BehaviorSubject<string>;
    supLoading: boolean;
    public supplierList: any;

    facsearchRequest: SupplierDataSourceRequest;
    facInput: BehaviorSubject<string>;
    facLoading: boolean;
    public factoryList: any;

    isShowColumn: boolean;

    pageLoader: boolean;
    supplierName: string;
    customerName: string;
    DateName: string;

    exportDataLoading: boolean;
    searchLoading: boolean;
    invoicePaymentStatusLoading: boolean;
    invoicePaymentStatusList: Array<DataSource>;
    billPaidByListLoading: boolean;
    billPaidByList: any;
    serviceLoading: boolean;
    serviceList: any;
    _paymentStatusList: any[] = [];
    isShowColumnImagePath: string;
    showColumnTooltip: string;
    isAuditInvoice: boolean;
    selectedNumber: string;
    selectedNumberPlaceHolder: string;
    isShowSearchLens: boolean;
    constructor() {
        this.pageLoader = false;
        this.isShowColumn = true;

        this.customerInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.facInput = new BehaviorSubject<string>("");

        this.requestCustomerModel = new CustomerCommonDataSourceRequest();
        this.supsearchRequest = new CommonDataSourceRequest();

        this.facsearchRequest = new SupplierDataSourceRequest();
    }
}

export class SaleInvoiceSummaryRequestModel extends summaryModel {
    dateTypeId: number;
    fromDate: any;
    toDate: any;
    searchTypeId: number;
    searchTypeText: string = "";
    customerId: number;
    supplierId: number;
    factoryIdList: Array<number>;
    invoiceTo: number;
    paymentStatusIdList: any[] = [];
    serviceId: number;
    supplierTypeId: SearchType;
    isExport: boolean;
}

export class SaleInvoiceItem {
    id: number;
    invoiceNo: string;
    invoicedName: string;
    invoiceDate: string;
    invoiceCurrency: string;
    totalFee: number;
    paymentStatusName: string;
    paymentStatusId: number;
    paymentDate: string;
    uniqueId: string;
}
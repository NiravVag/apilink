
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { DateObject } from "src/app/components/common/static-data-common";
import { DataList } from "../useraccount/userconfig.model";

export class InvoiceGenerateMasterData {
    customerList: any;
    customerLoading: boolean;
    customerInput: BehaviorSubject<string>;
    requestCustomerModel: CommonDataSourceRequest;

    supplierfactoryList: any;
    supplierfactoryLoading: boolean;
    supplierfactoryInput: BehaviorSubject<string>;
    requestSupModel: CommonDataSourceRequest;

    supplierList: any;
    supplierLoading: boolean;
    supplierInput: BehaviorSubject<string>;
    requestSupFactoryModel: CommonDataSourceRequest;

    invoiceRequestLoading: boolean;
    invoiceRequestList: any;

    billingToLoading: boolean;
    billingToList: any;

    invoiceBilledAddressLoading: boolean;
    billingAddressList: any;
    billingAddressData: number;
    invoiceContactsLoading: boolean;
    contactsList: any;

    serviceTypeList: any;
    serviceTypeLoading: boolean;

    serviceLoading: boolean;
    serviceList: any;

    productCategoryLoading: boolean;
    productCategoryList: any;

    productSubCategoryLoading: boolean;
    productSubCategoryList: any;

    saveDataLoading: boolean;
    countryLoading: boolean;
    countryList: any;
    customerBrandLoading: boolean;
    customerBrandList: any;
    customerDepartmentLoading: boolean;
    customerDepartmentList: any;
    customerBuyerLoading: boolean;
    customerBuyerList: any;
    customerContactsLoading: boolean;
    customerContactList: any;

    splitInvoiceList: any;
    splitInvoiceLoading: boolean;

    currencyLoading: boolean;
    currencyList: any;

    invoiceReportTemplateList: any;
    invoiceReportTemplateListLoading: boolean;
    invoiceReportTemplateId: number;
    templateBaseUrl: string;
    templateEntityId: string;
    selectedInvoiceNo: string;
    selectedCustomerId: number;
    templateVisible: boolean;

    supplierInfoValidator: any;
    supplierHeader: string;
    invoiceBankList: Array<DataList>;
    billingEntityList: Array<DataList>;
    invoiceBankLoading: boolean;
    billingEntityLoading: boolean;

    hasInvoiceAccess:boolean;
    hasInvoiceAccessLoading:boolean;

    constructor() {
        this.customerInput = new BehaviorSubject<string>("");
        this.supplierInput = new BehaviorSubject<string>("");
        this.supplierfactoryInput = new BehaviorSubject<string>("");
        this.supplierHeader = "Supplier";

    }
}

export class InvoiceGenerateModel {
    customerId: number;
    invoicingRequest: number;
    realInspectionFromDate: DateObject;
    realInspectionToDate: DateObject;
    invoiceTo: number;
    isTravelExpense: boolean;
    isInspection: boolean;
    isNewBookingInvoice: boolean;
    supplierInfo: InvoiceSupplierInfo;
    bookingNo: number;
    invoiceType: number;
    bookingNoList: Array<number>;
    invoiceNumber: string;
    service: number
    serviceTypes: Array<number>
    factoryCountryList: Array<number>
    brandIdList: Array<number>
    departmentIdList: Array<number>
    buyerIdList: Array<number>
    supplierList: Array<number>
    customerContacts: Array<number>
    splitInvoice: Array<number>;
    currencyId: number;
    exchangeRate: number;
    billedName: string;
    invoiceSelected: boolean;
    billingEntity: number;
    bankAccount: number;
    additionalTax: number;
    productCategoryIdList: Array<number>;
    productSubCategoryIdList: Array<number>;


    constructor() {
        this.supplierInfo = new InvoiceSupplierInfo();
        this.bookingNoList = [];
        this.isNewBookingInvoice = false;
    }
}

export class InvoiceSupplierInfo {
    supplierId: number;
    billedName: string;
    billingAddress: string;
    contactPersonIdList: Array<number>;
}

export class InvoiceGenerateResponse {
    invoiceData: Array<string>;
    result: InvoiceGenerateResult
}

export enum InvoiceGenerateResult {
    Success = 1,
    Failure = 2,
    RequestIsNotValid = 3,
    NoPricecardRuleFound = 4,
    NoInspectionFound = 5,
    NoRuleMapped = 6,
    FutureDateNotAllowed = 7,
    FromDateAfterToDate = 8,
    NoSupplierSelected = 9,
    SupplierIsRequired = 10,
    TravelOrInspectionRequired = 11,
    NoInspectionSelected=12,
    NoInvoiceConfigured=13,
    BankIsRequired=14,
    NoInvoiceDataAccess = 15
}

export enum InvoiceGenerationGroupBy {
    Supplier = 1,
    Service = 2,
    ServiceType = 3,
    Country = 4,
    Brand = 5,
    Department = 6,
    Buyer = 7,
    CustomerContact = 8,
    BookingNo = 9,
    ProductCategory = 10
}

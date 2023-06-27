import { BehaviorSubject } from "rxjs";
import { EmailTypeEnum, InvoiceType } from "src/app/components/common/static-data-common";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, DataSource } from "../common/common.model";
import { SupFactDashboardDataSourceRequest } from "../dashboard/supfactdashboard.model";

export class EmailConfig {
    id: number;
    customerContactIdList: Array<number>;
    apiContactIdList: Array<number>;
    isSupplierContact: boolean;
    isFactoryContact: boolean;
    isCustomerContact: boolean;
    customerId: number;
    serviceId: number;
    officeIdList: Array<number>;
    serviceTypeIdList: Array<number>;
    productCategoryIdList: Array<number>;
    apiResultIdList: Array<number>;
    supplierIdList: Array<number>;
    factoryIdList: Array<number>;
    factoryCountryIdList: Array<number>;
    departmentIdList: Array<number>;
    brandIdList: Array<number>;
    buyerIdList: Array<number>;
    collectionIdList: Array<number>;
    specialRuleIdList: Array<number>;
    recipientName: string;
    numberOfReports: number;
    reportInEmailId: number;
    emailSizeId: number;
    reportSendTypeId: number;
    emailSubjectId: number;
    cusDecisionIdList: Array<number>;
    typeId: number;
    fileNameId: number;
    toIdList: Array<number>;
    ccIdList: Array<number>;
    isMerchandiserContact: boolean;
    isPictureFileInEmail: boolean;
    additionalEmailRecipients: AdditionalEmailRecipient[];
    invoiceTypeId: number;
    constructor() {
        this.toIdList = [];
        this.ccIdList = [];
        this.customerContactIdList = [];
        this.additionalEmailRecipients = [];

    }
}
export class AdditionalEmailRecipient {
    id: number;
    email: string;
    recipientId: number;
    recipientType: string;
}
export class EmailConfigMaster {
    reportInEmailList: Array<DataSource>;
    emailSizeList: Array<DataSource>;
    reportSendTypeList: Array<DataSource>;
    emailSubjectList: Array<DataSource>;
    specialRuleList: Array<DataSource>;
    departmentList: Array<DataSource>;
    brandList: Array<DataSource>;
    buyerList: Array<DataSource>;
    collectionList: Array<DataSource>;
    factoryCountryList: Array<DataSource>;
    factoryList: Array<DataSource>;
    supplierList: Array<DataSource>;
    apiResultList: Array<DataSource>;
    productCategoryList: Array<DataSource>;
    serviceTypeList: Array<DataSource>;
    officeList: Array<DataSource>;
    serviceList: Array<DataSource>;
    customerList: Array<DataSource>;
    customerContactList: Array<DataSource>;
    apiContactList: Array<DataSource>;
    esTypeList: Array<DataSource>;
    cusDecistionList: Array<DataSource>;

    customerContactLoading: boolean;
    apiContactLoading: boolean;
    serviceLoading: boolean;
    officeLoading: boolean;
    serviceTypeLoading: boolean;
    productCategoryLoading: boolean;
    apiResultLoading: boolean;
    specialRuleLoading: boolean;
    reportInEmailLoading: boolean;
    emailSizeLoading: boolean;
    reportSendTypeLoading: boolean;
    emailSubjectLoading: boolean;
    esTypeLoading: boolean;
    cusDecisionLoading: boolean;
    subjectFileNameLoading: boolean;

    subjectFileNameList: Array<DataSource>;
    supplierLoading: boolean;
    supplierInput: BehaviorSubject<string>;
    factoryLoading: boolean;
    factoryInput: BehaviorSubject<string>;
    factoryCountryLoading: boolean;
    factoryCountryInput: BehaviorSubject<string>;
    departmentLoading: boolean;
    departmentInput: BehaviorSubject<string>;
    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    collectionLoading: boolean;
    collectionInput: BehaviorSubject<string>;
    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;

    factoryCountryRequest: CountryDataSourceRequest;
    brandSearchRequest: CommonCustomerSourceRequest;
    deptSearchRequest: CommonCustomerSourceRequest;
    collectionSearchRequest: CommonCustomerSourceRequest;
    buyerSearchRequest: CommonCustomerSourceRequest;

    customerLoading: boolean;
    customerInput: BehaviorSubject<string>;
    customerModelRequest: CommonDataSourceRequest;

    supplierModelRequest: SupFactDashboardDataSourceRequest;
    factoryModelRequest: SupFactDashboardDataSourceRequest;
    officeInput: BehaviorSubject<string>;
    officeModelRequest: CommonDataSourceRequest;

    saveLoading: boolean;
    reportInEmaiBaseOnlLoad: boolean;
    EmailTypeBaseOnLoad: boolean;

    toList: Array<DataSource>;
    ccList: Array<DataSource>;
    recipientList: Array<DataSource>;
    ccLoading: boolean;
    toLoading: boolean;
    fulllRecipientIdList: Array<number>;

    disableNoOfReport: boolean;
    disableReportInEmail: boolean;

    recipientTypeList: Array<DataSource>;
    recipientTypeLoading: boolean;

    additionalEmailRecipient: AdditionalEmailRecipient;

    invoiceTypeLoading: boolean;
    invoiceTypeList: Array<any>;
    emailType = EmailTypeEnum;
    invoiceType = InvoiceType;

    constructor() {
        this.customerList = new Array<DataSource>();
        this.brandList = new Array<DataSource>();
        this.departmentList = new Array<DataSource>();
        this.customerContactList = new Array<DataSource>();
        this.buyerList = new Array<DataSource>();
        this.collectionList = new Array<DataSource>();
        this.officeList = new Array<DataSource>();
        this.factoryList = new Array<DataSource>();
        this.serviceList = new Array<DataSource>();
        this.specialRuleList = new Array<DataSource>();
        this.supplierList = new Array<DataSource>();
        this.apiResultList = new Array<DataSource>();
        this.emailSizeList = new Array<DataSource>();
        this.apiContactList = new Array<DataSource>();
        this.serviceTypeList = new Array<DataSource>();
        this.productCategoryList = new Array<DataSource>();
        this.factoryCountryList = new Array<DataSource>();

        this.buyerInput = new BehaviorSubject<string>("");
        this.collectionInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.departmentInput = new BehaviorSubject<string>("");

        this.customerInput = new BehaviorSubject<string>("");
        this.supplierInput = new BehaviorSubject<string>("");
        this.factoryInput = new BehaviorSubject<string>("");
        this.factoryCountryInput = new BehaviorSubject<string>("");

        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest = new CommonCustomerSourceRequest();

        this.customerModelRequest = new CommonDataSourceRequest();
        this.supplierModelRequest = new SupFactDashboardDataSourceRequest();
        this.factoryModelRequest = new SupFactDashboardDataSourceRequest();
        this.officeModelRequest = new CommonDataSourceRequest();
        this.officeInput = new BehaviorSubject<string>("");
        this.factoryCountryRequest = new CountryDataSourceRequest();

        this.additionalEmailRecipient = new AdditionalEmailRecipient();

        this.invoiceTypeList = new Array<any>();
    }
}

export class EmailSubRequest {
    customerId: number;
    emailTypeId: number;
}

export enum EmailConfigResponseResult {
    Success = 1,
    Failure = 2,
    RequestNotCorrectFormat = 3,
    NotFound = 4,
    DataExists = 5
}

export enum EmailSendType {
    CustomerDecision = 1,
    ReportSend = 2,
    InvoiceSend = 3
}

export enum ReportInEmail {
    Link = 1,
    Attachment = 2
}

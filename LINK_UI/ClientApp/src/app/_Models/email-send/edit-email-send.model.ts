import { number, string } from "@amcharts/amcharts4/core";
import { DataSource } from "../common/common.model";

export class BookingReportModel {
    isSelected: boolean;
    bookingId: number;
    reportId: number;
    reportLink: string;
    productId: number;
    productName: string;
    poIdList: Array<number>;
    poList: Array<DataSource>;
    poIds: string;
    containerNumber: number;
    containerId: number;
    totalBookingQuantity: number;
    reportResult: string;
    reportStatus: string;
    extraFileName: string;
    reportSend: number;
    reportSendData: string;
    reportStatusColor: string;
    combineProductId: number;
    combineProductCount: number;
    isParentProduct: boolean;
    colorCode: string;
    isOkToSend: boolean;
    reportName: string;
    reportRevision: number;
    reportSendRevision: number;
    requestedReportRevision: number;
    fbReportId: number;
    reportVersion: number;
}

export class InvoiceNumberData {
    invoiceId: number;
    invoiceNo: string;
}

export class InvoiceModel {
    isSelected: boolean;
    invoiceId: number;
    invoiceNo: string;
    invoiceDate: string;
    billTo: string;
    billedName: string;
    invoiceType: string;
    invoiceTotal: number;
    currencyCode: string;
    invoiceFileUrl: string;
    isOkToSend: boolean;
}

export class EditEmailSend {
    bookingDataLoading: boolean;
    selectedReport: number;
    requestRevisionNo: string;
    requestNewRevisionNo: number;
    fbReportId: number;
    apiReportId: number;
    bookingReportData: Array<BookingReportModel>;
    bookingReportList: Array<BookingReportModel>;
    invoiceNumberList: Array<InvoiceNumberData>;
    invoiceList: Array<InvoiceModel>;
    bookingList: Array<DataSource>;
    productList: Array<DataSource>;
    reportList: Array<DataSource>;
    reportLoading: boolean;
    files: any;
    fileTypeList: Array<DataSource>;
    fileTypeLoading: boolean;
    loading: boolean;
    poList: Array<DataSource>;
    bookingIds: Array<number>;
    invoiceId: number;
    productIds: Array<number>;
    poIds: Array<number>;
    selectedBookingReport: boolean;
    emailSendFileDetails: Array<EmailSendFileDetails>;
    deleteId: number;
    deleteLoading: boolean;
    requestVersionLoading: boolean;
    reportStatusLoading: boolean = false;
    searchLoading: boolean;
    uploadLimit: number;
    fileSize: number;
    uploadFileExtensions: string;
    isheaderCheckBoxShow: boolean;
    bookingParam: BookingReportRequest;
    invoiceFileRequest: InvoiceFileRequest;
    emailRuleRequestbyInvoice: EmailRuleRequestByInvoice;
    emailRuleList: Array<EmailRuleData>;
    ruleId: number;
    pageLoader: boolean;
    isNoRuleFound: boolean;
    isEachBookingHasDifferentRule: boolean;
    selectedReportUpload: boolean;
    isheaderReportShow: boolean;
    emailSendLoading: boolean;
    bookingNoList: any;
    isEmailReady: boolean;
    apiResultLoading: boolean;
    apiResultList: any;
    selectedEmailPreview: boolean;
    selectedEmail: number;
    constructor() {
        this.bookingParam = new BookingReportRequest();
        this.invoiceFileRequest = new InvoiceFileRequest();
        this.emailRuleRequestbyInvoice = new EmailRuleRequestByInvoice();
        this.fileSize = 5000000;
        this.uploadLimit = 1;
        this.uploadFileExtensions = 'png,jpg,jpeg,xlsx ,xlsm,xltx, xls, pdf,doc,docx,tif,tiff,bmp, gif,zip,rar';
        this.emailSendFileDetails = new Array<EmailSendFileDetails>();
        this.bookingReportData = new Array<BookingReportModel>();
        this.emailRuleList = new Array<EmailRuleData>();
        this.invoiceList = new Array<InvoiceModel>();
        this.isEmailReady = false;
        this.apiResultLoading = false;
    }
}

export class BookingReportRequest {
    bookingIdList: Array<number>;
    serviceId: number;
    emailSendingtype: EmailSendingType;
}

export class InvoiceFileRequest {
    InvoiceNoList: Array<string>;
    serviceId: number;
    emailSendingtype: EmailSendingType;
}

export class EmailRuleRequestByInvoice {
    invoiceList: Array<string>;
    serviceId: number;
    emailSendingtype: EmailSendingType;
    invoiceType: number;
}

export enum EmailSendingType {
    CustomerDecision = 1,
    ReportSend = 2,
    InvoiceStatus = 3
}

export enum EmailSendResult {
    Success = 1,
    NotFound = 2,
    Failure = 3,
    RequestNotCorrectFormat = 4,
    OneRuleFound = 5,
    MoreThanOneRuleFound = 6,
    NoRuleFound = 7,
    SomeBookingDoesNotHaveRule = 8,
    EachBookingHasDifferentRule = 9
}

export class EmailSendFileDetails {
    bookingId: number;
    reportId: number;
    invoiceNo: string;
    fileTypeName: string;
    fileName: string;
    fileLink: string;
    emailSendFileId: number;
    isSelected: boolean;
    isShow: boolean;
}

export class EmailSendFileUpload {
    bookingIds: Array<number>;
    reportIds: Array<number>;
    fileTypeId: number;
    fileLink: string;
    fileName: string;
    serviceId: number;
    id: number;
    uniqueId: string;
}

export class InvoiceSendFileUpload {
    fileTypeId: number;
    invoiceId: number;
    invoiceNo: string;
    fileLink: string;
    fileName: string;
    serviceId: number;
    id: number;
    uniqueId: string;
}

export class EmailRuleData {
    isSelected: boolean;
    serviceTypeName: string;
    productCategoryName: string;
    apiResultName: string;
    factoryCountryName: string;
    departmentName: string;
    brandName: string;
    collectionName: string;
    buyerName: string;
    specialRuleName: string;
    supplierName: string;
    factoryName: string;
    ruleId: number;
    reportSendType: string;
    reportInEmail: string;
}


export class EmailPreviewRequest {
    emailRuleId: number;
    emailReportPreviewData: Array<EmailReportPreviewDetail>;
    emailReportAttachment: Array<EmailReportPreviewAttachment>;
    esTypeId: number;
}

export class EmailReportPreviewDetail {
    bookingId: number;
    reportId: number;
    reportVersion: number;
    reportRevision: number;
    extraFileName: string;
    invoiceNo: string;
}

export class EmailReportPreviewAttachment {
    bookingId: number;
    reportId: number;
    fileType: string;
    fileName: string;
    fileLink: string;
    invoiceNo: string;
}

export class EmailPreviewData {
    emailId: number;
    emailToList: Array<string>;
    emailCCList: Array<string>;
    emailBCCList: Array<string>;
    emailSubject: string;
    emailSubjectDisplay: string;
    ruleId: number;
    emailBody: string;
    emailValidOption: number;
    attachmentList: Array<EmailAttachments>;
    reportBookingList: Array<ReportBooking>;
    isEmailSelected: boolean;
    isEmailValid: boolean;
    active: boolean;
    toMailText: string;
    ccMailText: string;
    bccMailText: string;
    isenabled: boolean;
    customerId: number;
}

export class EmailAttachments {
    fileName: string;
    fileLink: string;
}


export class ReportBooking {
    reportId: number;
    inspectionId: number;
    auditId: number;
    esTypeId: number;
}

export class EmailTempModelData {
    toMailArray: Array<string>;
    ccMailArray: Array<string>;
    emailSubject: string;
    emailBody: string;
    emailId: number;
    toMailText: string;
    ccMailText: string;
    isenabled: boolean;
    constructor() {
        this.toMailArray = [];
        this.ccMailArray = [];
        this.emailSubject = "";
        this.emailBody = "";
        this.isenabled = true;
    }
}

export class EmailPreviewValidator {
    toListValid: boolean;
    ccListValid: boolean;
    bccListValid: boolean;
    subjectValid: boolean;
    emailBodyValid: boolean;
    toListErrorMessage: string;
    ccListErrorMessage: string;
    bccListErrorMessage: string;
    subjectErrorMessage: string;
    emailBodyErrorMessage: string;

    constructor() {
        this.toListValid = true;
        this.ccListValid = true;
        this.subjectValid = true;
        this.emailBodyValid = true;
        this.bccListValid = true;
        this.toListErrorMessage = "";
        this.ccListErrorMessage = "";
        this.subjectErrorMessage = "";
        this.emailBodyErrorMessage = "";
    }
}

export enum EmailAddressType {
    toList = 1,
    ccList = 2,
    bccList = 3
}

export enum EmailValidOption {
    EmailSizeExceed = 1,
    ReportLinkIsNotValid = 2,
    EmailSuccess = 3
}

export enum EmailPreviewResponseResult {
    success = 1,
    failure = 2,
    emailrulenotvalid = 3,
    inspectionsummarylinknotavailable = 4
}


export class EmailSendHistory {
    emailSentBy: string;
    emailSentOn: string;
    emailStatus: string;
}

export class EmailSendHistoryResponse {
    public emailSendHistoryList: Array<EmailSendHistory>;
    emailSendHistoryResult: EmailSendHistoryResult;
}

export enum EmailSendHistoryResult {
    Success = 1,
    NotFound = 2
}

export class AutoCustomerDecisionRequest {
    customerId: number;
    reportIdList: Array<number>;
    bookingIdList: Array<number>;
    constructor() {
        this.reportIdList = [];
        this.bookingIdList = [];
    }
}

export enum AutoCustomerDecisionResult {
    Success = 1,
    RequestShouldNotBeEmpty = 2,
    DataNotFound = 3,
    Failed = 4
}

export enum CustomerResult {
    Pass = 1,
    Fail = 2,
    Pending = 3
}

import { BehaviorSubject } from "rxjs";
import { BookingIdDataSourceRequest, DataSource } from "../common/common.model";


export class EditClaimMasterModel {
    bookingIdList: Array<DataSource>;
    bookingIdInput: BehaviorSubject<string>;
    bookingIdRequest: BookingIdDataSourceRequest;
    bookingIdLoading: boolean;

    reportList: Array<DataSource>;
    reportListLoading: boolean;

    claimFromList: Array<DataSource>;
    claimFromListLoading: boolean;

    receivedFromList: Array<DataSource>;
    receivedFromListLoading: boolean;

    claimSourceList: Array<DataSource>;
    claimSourceListLoading: boolean;

    defectFamilyList: Array<DataSource>;
    defectFamilyListLoading: boolean;

    claimDepartmentList: Array<DataSource>;
    claimDepartmentListLoading: boolean;

    claimCustomerRequestList: Array<DataSource>;
    claimCustomerRequestListLoading: boolean;

    priorityList: Array<DataSource>;
    priorityListLoading: boolean;

    customerRequestRefundList: Array<DataSource>;
    customerRequestRefundListLoading: boolean;

    currencyList: Array<DataSource>;
    currencyListLoading: boolean;

    defectDistributionList: Array<DataSource>;
    defectDistributionListLoading: boolean;

    claimResultList: Array<DataSource>;
    claimResultLoading: boolean;

    finalResultList: Array<DataSource>;
    finalResultLoading: boolean;

    fileTypeList: Array<DataSource>;
    fileTypeListLoading: boolean;

    showBookingDetails: boolean;
    constructor() {
        this.bookingIdRequest = new BookingIdDataSourceRequest();
        this.bookingIdInput = new BehaviorSubject<string>("");
        this.bookingIdList=new Array<DataSource>();
        this.reportList=new Array<DataSource>();
        this.claimFromList=new Array<DataSource>();
        this.defectFamilyList=new Array<DataSource>();
        this.claimDepartmentList=new Array<DataSource>();
        this.claimCustomerRequestList=new Array<DataSource>();
        this.priorityList=new Array<DataSource>();
        this.customerRequestRefundList=new Array<DataSource>();
        this.currencyList=new Array<DataSource>();
        this.defectDistributionList=new Array<DataSource>();
        this.claimResultList=new Array<DataSource>();
        this.finalResultList=new Array<DataSource>();
        this.fileTypeList=new Array<DataSource>();
    }
}

export class EditClaimModel {
    id: number;
    claimNo:string;
    bookingId: number;
    reportIdList: Array<number>;
    claimDate: any;
    requestContactName: string;
    claimFromId: number;
    receivedFromId: number;
    claimSourceId: number
    defectFamilyIdList: Array<number>;
    claimDepartmentIdList: Array<number>;
    claimDescription: string;
    claimCustomerRequestIdList: Array<number>;
    priorityId: number;
    customerRequestRefundIdList: Array<number>
    amount: number;
    customercomment: string;
    currencyId: number;
    qcControlId: number;
    defectPercentage: number;
    noOfPieces: number;
    compareToAQL: string;
    defectDistributionId: number;
    color: string;
    defectCartonInspected: string;
    fobPrice: number;
    fobCurrencyId: number;
    retailPrice: number;
    retailCurrencyId: number;
    analyzerFeedback: string;
    claimResultId: number;
    claimRemarks: string;
    claimRecommendation: string;
    finalDecisionIdList: Array<number>;
    finalRefundIdList: Array<number>;
    finalAmount: number;
    finalCurrencyId: number;
    statusId: number;
    statusName:string;
    claimFinalRefundRemarks: string;
    realInspectionFees: number;
    realInspectionCurrencyId: number;
    public fileTypeId: number;
    public fileDesc: string;
    attachments: Array<AttachmentFile>;
}

export class Attachment {
    public id: number;
    public fileTypeId: number;
    public fileTypeName: string;
    attachments: Array<AttachmentFile>;
    public fileDesc: string;
}

export class AttachmentFile {
    public id: number;
    public uniqueld: string;
    public fileName: string;
    public isNew: boolean;
    public mimeType: string;
    public file: File;
    public fileUrl: string;
    public fileTypeId: number;
    public fileTypeName: string;
    public fileDesc: string;
}

export class BookingRequestModel {
    bookingId: number;
    reportId: Array<any>;
}

export class InvoiceDetailModel {
    invoiceCurrencyName: string;
    invoiceDate: string;
    invoiceNo: string;
    totalInvoiceFees: number;
    invoiceCurrency: number;
}

export enum ClaimStatus {
    Registered = 1,
    Analyzed = 2,
    Validated = 3,
    Closed = 4,
    Cancelled = 5
}
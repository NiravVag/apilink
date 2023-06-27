import { any } from "@amcharts/amcharts4/.internal/core/utils/Array";

export class EditCreditNoteModel {
    id: number;
    creditNo: string;
    creditDate: any;
    creditTypeId: number;
    postDate: any;
    billedTo: string;
    billedAddress: string;
    contactPersons: number[];
    currencyId: number;
    paymentTerms: string;
    paymentDuration: number;
    officeId: number;
    invoiceAddress: string;
    bankId: number;
    billingEntity: any;
    subject: string;
    saveCreditNotes: GetCreditNoteItemModel[];
    constructor() {
        this.saveCreditNotes = [];
        this.contactPersons = [];
    }
}

export class EditCreditNoteItemModel {
    id: number;
    claimId: number;
    inspectionId: number;
    invoiceId: number;
    remarks: string;
    refundAmount: number;
    sortAmount: number;
}
export class GetCreditNoteItemModel extends EditCreditNoteItemModel {
    bookingNo: string;
    invoiceNo: string;
    claimNo: string;
    inspectionFee: number;
    inspectionDate: string;
    productCategory: string;
    productSubCategory: string;
    office: string;
}

export class EditCreditNoteMasterModel {
    oldCreditNo: string;
    customerList: any;
    customerLoading: boolean;

    customerAddressLoading: boolean;
    customerAddressList: any;

    customerId: number;

    customerContactList: Array<any>;
    customerContactsLoading: boolean;

    currencyList: Array<any>;
    currencyLoading: boolean;

    paymentTermList: Array<any>;
    paymentTermLoading: boolean;

    officeList: Array<any>;
    officeLoading: boolean;

    bankLoading: boolean;
    bankList: Array<any>;

    billingEntityList: Array<any>;
    billingLoading: boolean;

    isCreditNoLoading: boolean;
    isCreditNoLoadingMsg: string;

    creditTypeList: Array<any>;
    creditTypeLoading: boolean;
    billedAddress: string;

    isAccountingCreditNoteRole: boolean;
}
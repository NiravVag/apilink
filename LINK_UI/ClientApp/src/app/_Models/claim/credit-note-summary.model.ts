import { summaryModel } from "../summary.model";

export class CreditNoteSummaryMasterModel {
    creditTypeList: Array<any>;
    creditTypeLoading: boolean;
    exportLoading: boolean;
    isAccountingCreditNoteRole: boolean;
    constructor() {
        this.creditTypeList = [];
    }
}

export class CreditNoteSummaryModel extends summaryModel {
    fromDate: any;
    toDate: any;
    creditNo: string;
    creditType: number;
    searchtypetext: string;
}
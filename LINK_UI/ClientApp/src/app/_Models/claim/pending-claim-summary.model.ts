import { BehaviorSubject } from "rxjs";
import { pendingClaimSearchTypeLst } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class PendingClaimSummaryModel extends summaryModel {
    fromDate: any;
    toDate: any;
    customerId: number;
    searchTypeId: number;
    searchTypeText: string;
    officeId: number;

}

export class PendingClaimSummaryMasterModel {
    customerInput: BehaviorSubject<string>;
    customerList: any;
    customerLoading: boolean;
    requestCustomerModel: CommonDataSourceRequest;

    officeList: any;
    officeLoading: boolean;

    pendingClaimSearchTypeList = pendingClaimSearchTypeLst;
    isAccountingCreditNoteRole: boolean;
    filterName: string;
    customerName: string;
    officeName: string;
    constructor() {
        this.customerInput = new BehaviorSubject("");
        this.customerList = [];
        this.requestCustomerModel = new CommonDataSourceRequest();

        this.officeList = [];
    }
} 

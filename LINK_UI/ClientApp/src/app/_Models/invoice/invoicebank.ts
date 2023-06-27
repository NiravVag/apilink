import { DateObject } from "src/app/components/common/static-data-common";
import { summaryModel } from "../summary.model";

export class InvoiceBank {
    id: number;
    accountName: string;
    accountNumber: string;
    bankName: string;
    swiftCode: string;
    bankAddress: string;
    billingEntity: number;
    accountCurrency: string;
    accountCurrencyName: string;
    remarks: string;
    chopFileUniqueId: string;
    signatureFileUniqueId: string;
    chopFilename: string;
    signatureFilename: string;
    chopFileUrl: string;
    signatureFileUrl: string;
}

export class InvoiceBankTax {
    id: number;
    taxName: string
    taxValue: string;
    fromDate: any;
    toDate: any;
    isDisable: boolean;
}

export class InvoiceBankSaveRequest {

    constructor() {
        this.invoiceBankTaxList = [];
    }

    id: number;
    accountName: string;
    accountNumber: string;
    bankName: string;
    swiftCode: string;
    bankAddress: string;
    accountCurrency: number;
    billingEntity: number;
    remarks: string;
    chopFileUniqueId: string;
    signatureFileUniqueId: string;
    chopFileName: string;
    signatureFileName: string;
    signatureFileType: string;
    chopFileType: string;
    chopFileUrl: string;
    signatureFileUrl: string;
    invoiceBankTaxList: Array<InvoiceBankTax>;
}

export class InvoiceBankSaveResponse {
    id: number;
    result: InvoiceBankSaveResult
}


export class InvoiceBankDeleteResponse {
    id: number;
    result: InvoiceBankDeleteResult
}

export class InvoiceBankGetAllResponse {
    bankDetails: Array<InvoiceBank>;
    result: InvoiceBankGetAllResult
}

export class InvoiceBankGetResponse {
    bankDetails: InvoiceBank;
    bankTaxDetails: Array<InvoiceBankTax>;
    result: InvoiceBankGetResult;
}

export enum InvoiceBankSaveResult {
    success = 1,
    failure = 2,
    invoiceBankAccountIsAlreadyExist = 3,
    invoiceBankIsNotExist = 4
}

export enum InvoiceBankDeleteResult {
    success = 1,
    failure = 2
}

export enum InvoiceBankGetAllResult {
    success = 1,
    failure = 2,
    invoiceBankNotFound = 3
}

export enum InvoiceBankGetResult {
    success = 1,
    failure = 2,
    invoiceBankNotFound = 3
}

export class bankSummaryMasterData {
    bankDetails: Array<InvoiceBank>;
    loading: boolean = false;
}

export class InvoiceBankSummary extends summaryModel {

}

export class InvoiceBankTaxRequest {
    toDate: any;
}
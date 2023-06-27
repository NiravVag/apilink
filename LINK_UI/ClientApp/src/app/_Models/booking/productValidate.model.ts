export class ProductValidateMaster {
    totalProducts: number;
    deleteSuccessCount: number;
    validationFailedCount: number;
    poRemarks: string;
    poValid: boolean;
    constructor() {
        this.totalProducts = 0;
        this.deleteSuccessCount = 0;
        this.validationFailedCount = 0;
        this.poRemarks = '';
        this.poValid = true;
    }
}

export class ProductValidateData {
    bookingId: number;
    poTransactionId: number;
    quotationExists: boolean;
    pickingExists: boolean;
    reportExists: boolean;
    constructor() {
        this.quotationExists = false;
        this.pickingExists = false;
        this.reportExists = false;
    }
}

export enum ValidationType {
    quotationExists = 1,
    pickingExists = 2,
    reportExists = 3
}
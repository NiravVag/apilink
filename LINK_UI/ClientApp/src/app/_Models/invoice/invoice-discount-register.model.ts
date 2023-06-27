export class EditInvoiceDiscountModel {
    id: number;
    customerId: number;
    discountType: number;
    countryIds: number[];
    periodFrom : any;
    periodTo: any;
    applyToNewCountry: boolean;
    limits: EditInvoiceDiscountPeriodModel[];
    constructor() {
        this.limits = [];
        this.countryIds=[];
    }
}

export class EditInvoiceDiscountPeriodModel {
    id: number;
    limitFrom: number;
    limitTo: number;
    notification: boolean;
}
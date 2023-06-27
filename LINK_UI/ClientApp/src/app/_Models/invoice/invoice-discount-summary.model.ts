import { summaryModel } from "../summary.model";

export class InvoiceDiscountModel extends summaryModel {
    customerId: number;
    discountType: number;
    periodFrom: any;
    periodTo: any;
    countryId: any;
}

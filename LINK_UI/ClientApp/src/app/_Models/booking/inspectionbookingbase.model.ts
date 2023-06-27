export class InspectionBaseAndProductDetails {
    bookingNo: number;
    status: string;
    customer: string
    supplier: string
    factory: string
    inspectionDate: string
    totalBookingQuantity: number;
    unit: string
    country: string
    province: string
    city: string
    county: string
    town: string
    productBaseDetails: Array<InpectionProductBaseDetail>;
}


export class InpectionProductBaseDetail {
    productName: string
    productDesc: string
    unit: string
    quantity: number;
}

export class inspectionProductBaseDetailResponse {
    inspectionBaseDetail: InspectionBaseAndProductDetails
    result: InspectionProductBaseDetailResult
}

export enum InspectionProductBaseDetailResult {
    Success = 1,
    NotFound = 2
}
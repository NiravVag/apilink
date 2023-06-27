import { summaryModel } from "../summary.model";

export class ICSummaryMasterData {
    customerLoading: boolean;
    supplierLoading: boolean;
    isPreview: boolean;
    searchloading: boolean = false;
    statusLoading: boolean;
    bookingSearchLoading: boolean;
    customerList: Array<DataList>;
    supplierList: Array<DataList>;
    icStatusList: Array<DataList>;
}

export interface DataList {
    id: number;
    name: string;
}

export enum CustomerResult {
    Success = 1,
    CannotGetCustomerList = 2
}
export enum SupplierResult {
    Success = 1,
    NodataFound = 2
}

export enum ICStatusResult {
    Success = 1,
    NodataFound = 2
}

export class ICUserAccessData {
    public currentUser: any;
    public isInternalUser: boolean;
    public isCustomerUser: boolean;
}

export class ICSummaryModel extends summaryModel {
    public searchTypeId:number;

    public searchTypeText:string="";

    public customerId: number;

    public supplierId: number;

    public statusIdlst: Array<number>;

    public fromdate: any;

    public todate: any;

    public datetypeid:number;

}

export class ICItem {

    public bookingNumber:number;
    public icNumber:number;
    public icId:number;
    public statusId:number;
    public customerName:string;
    public supplierName:string;
    public serviceDate:string;
    public productList:any[];
    public isExpand: boolean = false;
    public buyerName: string;
}
import { BehaviorSubject } from "rxjs";
import { PageSizeCommon } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, CountryDataSourceRequest, QcDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class InvoiceDataAccessModel extends summaryModel {
    public staffId: number;
    public customerIdList: Array<number>;
    public invoiceTypeIdList: Array<number>;
    public officeIdList: Array<number>;
}

export class InvoiceDataAccessMasterModel{

    customerList: any;
    customerLoading: boolean
    customerInput: BehaviorSubject<string>;
    customerRequest: CommonDataSourceRequest;

    officeList: any;
    officeLoading: boolean


    invoiceTypeList: any;
    invoiceTypeLoading: boolean

    
    staffList: any;
    staffLoading: boolean
    staffInput: BehaviorSubject<string>;
    staffRequest: CommonDataSourceRequest;

    searchloading: boolean;
    selectedPageSize: any;

    constructor(){
    this.customerRequest = new CommonDataSourceRequest();    
    this.customerInput = new BehaviorSubject<string>("");

    this.selectedPageSize = PageSizeCommon[0];



    this.staffRequest = new CommonDataSourceRequest();    
    this.staffInput = new BehaviorSubject<string>("");

   
    }
}

export class InvoiceDataAccessEditModel {
    public id?: number;
    public staffId: number;
    public customerIdList: Array<number>;
    public invoiceTypeIdList: Array<number>;
    public officeIdList: Array<number>;

    constructor(){
        this.id = 0;
    }
}


export class InvoiceDataAccessEditMasterModel{
    customerList: any;
    customerLoading: boolean
    customerInput: BehaviorSubject<string>;
    customerRequest: CommonDataSourceRequest;

    officeList: any;
    officeLoading: boolean
    officeInput: BehaviorSubject<string>;
    officeRequest: CommonDataSourceRequest;

    invoiceTypeList: any;
    invoiceTypeLoading: boolean

    
    staffList: any;
    staffLoading: boolean
    staffInput: BehaviorSubject<string>;
    staffRequest: CommonDataSourceRequest;
   

    saveloading: boolean;
    deleteLoading: boolean;

    constructor()
    {
        this.customerRequest = new CommonDataSourceRequest();    
        this.customerInput = new BehaviorSubject<string>("");

        this.staffRequest = new CommonDataSourceRequest();    
        this.staffInput = new BehaviorSubject<string>("");   
 
    }
}


export enum InvoiceDataAccessResult{
    Success = 1,
    Failed = 2,
    RequestNotCorrectFormat = 3,
    NotFound = 4,
    Error = 5,
    Exists = 6,
}
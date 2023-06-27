import { BehaviorSubject } from "rxjs";
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, DataSource } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class EmailConfigSummaryModel extends summaryModel {
    customerId: number;
    serviceId: number;
    serviceTypeIdList: Array<number>;
    factoryCountryIdList: Array<number>;
    departmentIdList: Array<number>;
    officeIdList: Array<number>;
    brandIdList: Array<number>;
    productCategoryIdList: Array<number>;
    resultIdList: Array<number>;
    typeId : number;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;
}

export class EmailConfigSummaryMasterModel {
    brandList: Array<DataSource>;
    brandLoading: boolean;

    officeList: Array<DataSource>;
    officeLoading: boolean;

    productCategoryList: Array<DataSource>;
    productCategoryLoading: boolean;

    resultList: Array<DataSource>;
    resultLoading: boolean;

    customerList: Array<DataSource>;
    customerLoading: boolean;
    customerInput: BehaviorSubject<string>;

    serviceList: Array<DataSource>;
    serviceLoading: boolean;

    serviceTypeList: Array<DataSource>;
    serviceTypeLoading: boolean;

    factoryCountryList: Array<DataSource>;
    factoryCountryLoading: boolean;

    departmentList: Array<DataSource>;
    departmentLoading: boolean;


    customerModelRequest: CommonDataSourceRequest;

    searchLoading: boolean;
    deleteId: number;
    deleteLoading: boolean;
    factoryCountryRequest: CountryDataSourceRequest;
    brandSearchRequest: CommonCustomerSourceRequest;
    deptSearchRequest: CommonCustomerSourceRequest;
    officeInput: BehaviorSubject<string>;
    officeModelRequest: CommonDataSourceRequest;
    factoryLoading: boolean;
    factoryInput: BehaviorSubject<string>;
    factoryCountryInput: BehaviorSubject<string>;
    departmentInput: BehaviorSubject<string>;
    brandInput: BehaviorSubject<string>;
    apiResultLoading: boolean;
    apiResultList: Array<DataSource>;
    esTypeLoading: boolean;
esTypeList: Array<DataSource>;
    constructor() {
        this.factoryCountryInput = new BehaviorSubject<string>("");
        this.customerInput = new BehaviorSubject<string>("");
        this.customerModelRequest = new CommonDataSourceRequest();
        this.brandInput = new BehaviorSubject<string>("");
        this.departmentInput = new BehaviorSubject<string>("");
        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.officeModelRequest = new CommonDataSourceRequest();
        this.officeInput = new BehaviorSubject<string>("");
        this.factoryCountryRequest = new CountryDataSourceRequest();
    }
}

export class EmailConfigSummaryItem {
    customerName: string;
    serviceTypeName: string;
    officeName: string;
    factoryCountryName: string;
    departmentName: string;
    brandName: string;
    productcategoryName: string;
    resultName: string;
    emailConfigId: number;
    service:string;
    reportinemail:string;
    reportSendType:string;
    emailTypeName: string;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;
}
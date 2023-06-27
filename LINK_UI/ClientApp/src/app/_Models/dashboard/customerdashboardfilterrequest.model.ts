import { DataSource } from './../common/common.model';
import { NgbDate } from "@ng-bootstrap/ng-bootstrap";
import { BehaviorSubject } from "rxjs";
import { ProductDataSourceRequest } from "../common/common.model";

export class CustomerDashboardFilterModel {
    public customerId: number;
    public supplierId: number;
    public selectedCountryIdList: any;
    public serviceDateFrom: any;
    public serviceDateTo: any;
    public factoryId: number;
    selectedBrandIdList: Array<number>;
    selectedBuyerIdList: Array<number>;
    selectedCollectionIdList: Array<number>;
    selectedDeptIdList: Array<number>;
    prodCategoryList: Array<number>;
    productIdList: Array<number>;
    statusIdList: Array<number>;
    constructor() {
        this.selectedCountryIdList = [];
        this.selectedCollectionIdList = [];
        this.selectedBuyerIdList = [];
        this.selectedBrandIdList = [];
        this.selectedDeptIdList = [];
        this.prodCategoryList = [];
        this.productIdList = [];
        this.statusIdList = [];
    }
}

export class CustomerDashboardFilterMaster {
    public supplierList: any;
    public countryList: any;
    public customerList: any;
    public customerLists : Array<DataSource>;
    supplierName: string;
    customerName: string;
    countryListName: string;
    filterCount: number;
    filterDataShown: boolean;
    customerLoading : boolean;

    countryInput: BehaviorSubject<string>;
    supInput: BehaviorSubject<string>;

    countryLoading: boolean;
    supLoading: boolean;

    brandLoading: boolean;
    brandInput: BehaviorSubject<string>;
    brandList: any;

    deptLoading: boolean;
    deptInput: BehaviorSubject<string>;
    deptList: any;

    buyerLoading: boolean;
    buyerInput: BehaviorSubject<string>;
    buyerList: any;

    collectionLoading: boolean;
    collectionInput: BehaviorSubject<string>;
    collectionList: any;

    productCategoryList: any;
    productCategoryListLoading: boolean;

    productList: any;
    productInput: BehaviorSubject<string>;
    productLoading: boolean;
    productRequest: ProductDataSourceRequest;

    searchtypeid: number;

    constructor() {
        this.countryInput = new BehaviorSubject<string>("");
        this.supInput = new BehaviorSubject<string>("");
        this.brandInput = new BehaviorSubject<string>("");
        this.buyerInput = new BehaviorSubject<string>("");
        this.deptInput = new BehaviorSubject<string>("");
        this.collectionInput = new BehaviorSubject<string>("");
        this.productInput = new BehaviorSubject<string>("");
        this.countryLoading = false;
        this.supLoading = false;

        this.productRequest = new ProductDataSourceRequest();
    }
}

export class InspectionListRequest {
    inspectionIds: Array<number>;
    prodCategoryId: Array<number>;
    productRefList: Array<number>;
}

export class DashboardMapFilterRequest {
    public customerId: number;
    public supplierId: number;
    public serviceDateFrom: any;
    public serviceDateTo: any;
    public factoryIds: Array<number>;
    public statusIds: Array<number>;
    public countryIds: Array<number>;
    public supplierIds: Array<number>;
    public officeIds: Array<number>;
    public brandIds: Array<number>;
    public buyerIds: Array<number>;
    public collectionIds: Array<number>;
    public departmentIds: Array<number>;
    public productCategoryIds: Array<number>;
    public productIds: Array<number>;
    public dashboardType: number;
    constructor() {
        this.countryIds = [];
        this.customerId = null;
        this.supplierId = null;
        this.factoryIds = [];
        this.officeIds = [];
        this.supplierIds = [];
        this.brandIds = [];
        this.buyerIds = [];
        this.collectionIds = [];
        this.departmentIds = [];
        this.productCategoryIds = [];
        this.productIds = [];
    }
}

export enum DashboardType {
    CustomerDashboard = 1,
    ManagementDashboard = 2
}

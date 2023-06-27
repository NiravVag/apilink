import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest, DataSource, ProductSubCategory2SourceRequest, ProductSubCategory3SourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class WorkLoadMatrixSummaryModel extends summaryModel {
    productCategoryId: number;
    productCategorySub2IdList: any[];
    productSubCategoryId: number;
    productCategorySub3IdList: any[];
    workLoadMatrixNotConfigured: boolean = false;
}

export class EditWorkLoadMatrixModel {
    public id?:number;
    public productSubCategoryId:number;
    public productCategoryId: number;
    public productSubCategory2Id:number;
    public productSubCategory3Id:number;
    public preparationTime: number;
    public eightHourSampleSize: number;
}

export class WorkLoadMatrixMasterModel{
    productCategoryList: Array<DataSource>;
    productSubCategoryList: Array<DataSource>;
    productSubCategory2List: Array<DataSource>;
    productSubCategory3List: Array<DataSource>;

    productSubCategory2Input: BehaviorSubject<string>;
    productSubCategory3Input: BehaviorSubject<string>;

    productSubCategory2Request: ProductSubCategory2SourceRequest;
    productSubCategory3Request: ProductSubCategory3SourceRequest;

    productCategoryLoading: boolean;
    productSubCategoryLoading: boolean;
    productCategorySub2Loading: boolean;
    productCategorySub3Loading: boolean;
    searchloading: boolean;
    exportLoading: boolean;

    constructor(){
        this.productSubCategory2Request = new ProductSubCategory2SourceRequest();
        this.productSubCategory3Request = new ProductSubCategory3SourceRequest();

        this.productSubCategory2Input = new BehaviorSubject<string>("");
        this.productSubCategory3Input = new BehaviorSubject<string>("");
        this.searchloading = false;
    }
}

export class EditWorkLoadMatrixMasterModel{
    productSubCategory2EditList: Array<DataSource>;
    productCategoryList: Array<DataSource>;
    productSubCategoryList: Array<DataSource>;
    productSubCategory3EditList: Array<DataSource>;

    productSubCategory2Input: BehaviorSubject<string>;
    productSubCategory3Input: BehaviorSubject<string>;

    productSubCategory2Request: ProductSubCategory2SourceRequest;
    productSubCategory3Request: ProductSubCategory3SourceRequest;
    productCategoryLoading: boolean;
    productSubCategoryLoading: boolean;
    productCategorySub2Loading: boolean;
    productCategorySub3Loading: boolean;
    saveloading: boolean;
    loading: boolean;
    deleteLoading:boolean;

    constructor(){
        this.productSubCategory2Request = new ProductSubCategory2SourceRequest();
        this.productSubCategory3Request = new ProductSubCategory3SourceRequest();

        this.productSubCategory2Input = new BehaviorSubject<string>("");
        this.productSubCategory3Input = new BehaviorSubject<string>("");
        this.saveloading = false;
    }
}
import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest, DataSource, ProductSubCategory2SourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";
import { ProductSubCategory } from "./productcategorysub2.model";

export class ProductCategorySub3SummaryModel extends summaryModel {
    productCategoryId: number;
    productCategorySub2Values: any[];
    productSubCategoryId: number;
    productCategorySub3Name: string;
    productCategorySub3Values: any[];
}

export class EditProductCategorySub3Model {
    public id?:number;
    public name:string;
    public productSubCategoryId:number;
    public productCategoryId: number;
    public productSubCategory2Id:number;
    public workLoadMatrixChecked: boolean;
    public preparationTime: number;
    public eightHourSampleSize: number;
}

export class ProductCategorySub3MasterModel{
    productCategoryList: Array<DataSource>;
    productSubCategoryList: Array<DataSource>;
    productSubCategory2List: Array<DataSource>;

    productSubCategory2Input: BehaviorSubject<string>;

    productSubCategory2Request: ProductSubCategory2SourceRequest;

    productCategoryLoading: boolean;
    productSubCategoryLoading: boolean;
    productCategorySub2Loading: boolean;
    searchloading: boolean;
    exportLoading: boolean;

    constructor(){
        this.productSubCategory2Request = new ProductSubCategory2SourceRequest();

        this.productSubCategory2Input = new BehaviorSubject<string>("");
        this.searchloading = false;
    }
}

export class EditProductCategorySub3MasterModel{
    productSubCategory2EditList: Array<DataSource>;
    productCategoryList: Array<DataSource>;
    productSubCategoryList: Array<DataSource>;

    productSubCategory2Input: BehaviorSubject<string>;

    productSubCategory2Request: ProductSubCategory2SourceRequest;
    productCategoryLoading: boolean;
    productSubCategoryLoading: boolean;
    productCategorySub2Loading: boolean;
    saveloading: boolean;
    loading: boolean;
    deleteLoading:boolean;

    constructor(){
        this.productSubCategory2Request = new ProductSubCategory2SourceRequest();

        this.productSubCategory2Input = new BehaviorSubject<string>("");
        this.saveloading = false;
    }
}
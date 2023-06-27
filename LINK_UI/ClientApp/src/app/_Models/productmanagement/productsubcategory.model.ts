import { summaryModel } from "../summary.model";

export class ProductSubCategorySummaryModel extends summaryModel {

}

export class ProductSubCategoryModel {
    public id?:number;
    public name:string;
    public productCategoryId?:number;
    public productCategory?: ProductCategory;
    public active?: boolean;
}

export class ProductCategory {
    public id: number;
    public name?: string;
}

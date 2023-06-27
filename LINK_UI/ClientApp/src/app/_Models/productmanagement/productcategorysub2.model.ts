import { summaryModel } from "../summary.model";

export class ProductCategorySub2SummaryModel extends summaryModel {
    productCategoryId: number;
    productCategorySub2Values: any[];
    productSubCategoryId: number;
    productCategorySub2Name: string;
}

export class ProductCategorySub2Model {
    public id?:number;
    public name:string;
    public productSubCategoryId?:number;
    public productCategoryId?: number;
    public active?: boolean;
    public productSubCategory?: ProductSubCategory;
}

export class ProductSubCategory {
    public id: number;
    public name?: string;
}

export enum DeleteProductManagementResult {
    Success = 1,
    NotFound = 2,
    CannotDelete = 3,
    ProductMapped = 4
}

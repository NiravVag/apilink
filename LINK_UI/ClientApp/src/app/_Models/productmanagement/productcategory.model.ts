import { summaryModel } from "../summary.model";

export class ProductCategorySummaryModel extends summaryModel {

}

export class ProductCategoryModel {
    public id?:number;
    public name:string;
    public active?: boolean;
    public businessLineId? : number;
    public businessLine? : string;
}

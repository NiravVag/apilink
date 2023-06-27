import { BehaviorSubject } from "rxjs";
import { ProductSubCategory2SourceRequest, ProductSubCategory3SourceRequest } from "../common/common.model";

export class CustomerProductMaster{
    productCategorySub2ModelRequest: ProductSubCategory2SourceRequest;
    productCategorySub2Input: BehaviorSubject<string>;
    productCategorySub2Loading:boolean;
    productCategorySub2List:any;
    
    productCategorySub3ModelRequest: ProductSubCategory3SourceRequest;
    productCategorySub3Input: BehaviorSubject<string>;
    productCategorySub3Loading:boolean;
    productCategorySub3List:any;

    unitList: any;
    unitLoading: boolean;

    constructor(){
        this.productCategorySub2ModelRequest = new ProductSubCategory2SourceRequest();
        this.productCategorySub2Input = new BehaviorSubject<string>("");

        this.productCategorySub3ModelRequest = new ProductSubCategory3SourceRequest();
        this.productCategorySub3Input = new BehaviorSubject<string>("");
    }
}
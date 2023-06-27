import { summaryModel } from "../summary.model";

export class CustomerProductSummaryModel extends summaryModel {

  customerValue: number;
  productCategoryValue: number;
  productSubCategoryValue: number;
  productCategorySub2s: any[];
  productCategorySub3s: any[];
  productvalue: string;
  productDescription: string;
  productcategoryValue: any;
  factoryReference: string;
  isNewProduct: boolean;
  isStyle:boolean;
  categoryMapped: number;
  categorytypeid: number;

  items: Array<CustomerProductItemModel> = [];

  constructor() {
    super();
    this.noFound = false;
    this.isNewProduct = false;
    this.isStyle=false;
  }

}

export class CustomerProductItemModel {
  constructor() {
  }

  id: number;
  customerName: string;
  productID: string;
  productDescription: string;
  productCategory: string;
  productSubCategory: string;
  productCategorySub2: string;
  productCategorySub3: string;
  factoryReference: string;
  isBooked: boolean;
  sampleSize8h: string;
  timePreparation: string;
}



export class CustomerProductToRemove {
  constructor() {
  }
  id: number;
  name: string;
}

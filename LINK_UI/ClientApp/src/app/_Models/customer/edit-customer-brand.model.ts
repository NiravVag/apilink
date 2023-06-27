
import { CustomerSummaryItemModel } from "./cutomersummary.model";
export class EditCustomerBrandModel {
  public id: number;  
  public customerBrands: Array<Brand>; 
 
  constructor() {
  //  this.id = 0;    
    this.customerBrands = [];  
  }
}

export class Brand {
  public id : number;
  public name: string;  
  public brandCode: string;  
}

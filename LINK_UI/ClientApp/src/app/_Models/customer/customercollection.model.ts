import { summaryModel } from "../summary.model";

export class CustomerCollectionModel extends summaryModel {
  public id: number;  
  public customerCollectionList: Array<Collection>; 

  public removeIds: Array<number>;
  constructor() {
    super();
    this.customerCollectionList = [];  
    this.removeIds = [];
  }
}

export class Collection {
  public id : number;
  public name: string;  
}
// export class CustomerCollectionListSummary extends summaryModel
// {
//      CustomerId: number;
// }
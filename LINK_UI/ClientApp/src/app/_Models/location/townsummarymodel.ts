import { summaryModel } from "../../_Models/summary.model";
export class TownSummaryModel extends summaryModel {
  countryValues: number;
  provinceValues : number;
  cityValues: number;
  countyValues: number;
  townValues : string;
  items: any[];

  constructor(){
    super();
    this.townValues="";
    this.cityValues=null;
  }
}
export class TownSummaryItemModel {
  id: number;
  country: string;
  province : string;
  city : string;
  county : string;
  townName : string;
  townCode : string;
}

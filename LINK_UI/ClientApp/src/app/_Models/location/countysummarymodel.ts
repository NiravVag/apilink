import { summaryModel } from "../../_Models/summary.model";
export class CountySummaryModel extends summaryModel {
  CountryValues: number;
  CountyValues: string;
  CityValues: number;
  ProvinceValues : number;
  items: any[];
  constructor(){
    super();
    this.CountyValues="";
    this.CityValues=null;
  }
}
export class CountySummaryItemModel {
  id: number;
  countyName: string;
  country: string;
  countyCode: string;
  cityName: string;
  provinceName: string;
}

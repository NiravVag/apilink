import { summaryModel } from "../../_Models/summary.model";
export class Provincesummarymodel extends summaryModel {
  CountryValues: any;
  ProvinceValues: any[];
  items: any[];
}
export class ProvinceSummaryItemModel {
  id: number;
  provincename: string;
  country: string;
  prvincecode: string;
}

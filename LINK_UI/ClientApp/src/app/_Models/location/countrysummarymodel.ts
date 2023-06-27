import{summaryModel} from "../../_Models/summary.model";
export class Countrysummarymodel extends summaryModel {
CountryValues:any[];   
}

export class CountrySummaryItemModel{
    id:number;
    countryname:string;
    area:string;
    countrycode:number;
    alphacode:string;
}

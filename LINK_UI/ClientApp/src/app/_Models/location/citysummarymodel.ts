import{summaryModel} from "../../_Models/summary.model";
export class Citysummarymodel extends summaryModel {
Countryvalues:number=0;
provinceValues:any;
Cityvalues:any[]=[];
items:any[];
}
export class CitySummaryItemModel{
    id:number;
    cityname:string;
    countryname:string;
    provincename:string;
    officename:string;
    zone:string;
    traveltime:string;
    phcode:string
}

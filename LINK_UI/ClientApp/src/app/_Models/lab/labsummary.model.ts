import { summaryModel } from "../summary.model";

export class LabSummaryModel extends summaryModel {
  labTypeValues: any[];
  countryValues: any[];
  labNameValues: any[];  
  constructor() {
    super();
    this.labTypeValues = [];
    this.countryValues = [];
    this.labNameValues=[];
  }
}

export class LabSummaryItemModel {
  constructor() {
  }
  id: number;
  labName: string;
  countryName: string;
  provinceName: string;
  cityName: string;
  labType: string; 
}
export class LabToRemove {
  constructor() {
  }
  id: number;
  name: string;
}

export class LabSummaryMaster {
  mainTypeList: Array<any> = [];
  mainTypeLoading: boolean=false;
  labNameList: Array<any> = [];
  labNameLoading: boolean=false;
  countryList: Array<any> = [];
  countryLoading: boolean = false;
}

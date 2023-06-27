import { DataSource } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class StaffSummaryModel extends summaryModel {

  positionValues: any[];
  officeValues: any[];
  countryValues: any[];
  departmentValues: any[]; 
  employeeTypeValues: any[];
  employeeNumber: string;
  staffName: string;
  // isLeft: boolean;

  constructor() {
    super();
    this.countryValues = [];
    this.departmentValues = [];
    this.officeValues = [];
    this.positionValues = [];
    this.staffName = "";
    this.employeeNumber = "";
    this.noFound = false;
    // this.isLeft = false; 
  }
}

export class StaffSummaryItemModel {

  constructor() {
    this.isChecked = false;
  }

  id: number;
  staffName: string;
  countryName: string;
  positionName: string;
  departmentName: string;
  officeName: string;
  joinDate: string;
  employeeType: string; 
  isChecked: boolean;
  statusName: string;
  statusColor: string;
  statusId:number;
}


export class StaffToRemove {

  constructor() {
  }

  id: number;
  staffName: string;
  leaveDate: any;
  reason: string;
  statusId: number;
}


export class StaffSummaryMasterModel {
  statusLoading: boolean;
  statusList : Array<DataSource>;
}
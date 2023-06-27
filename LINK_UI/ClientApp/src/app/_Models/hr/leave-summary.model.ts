import { summaryModel } from "../summary.model";


export enum LeaveStatusEnum {
  Request = 1,
  Checked = 2,
  Approved = 3,
  Rejected = 4,
  Canceled = 5
}

export class StatusColor {
  id: LeaveStatusEnum;
  color: string; 

}

export const leavestatusColors: Array<StatusColor> = [
  { id: LeaveStatusEnum.Request, color: '#5bc0de' },
  { id: LeaveStatusEnum.Checked, color: '#428bca' },
  { id: LeaveStatusEnum.Approved, color: '#28a745' },
  { id: LeaveStatusEnum.Rejected, color: '#ef0505' },
  { id: LeaveStatusEnum.Canceled, color: '#dd4b39' }
]


export class LeaveSummaryModel extends summaryModel{
  public staffName: string;
  public statusValues: Array<LeaveStatus>;
  public typeValues: Array<LeaveType>;
  public startDate: any;
  public endDate: any;
  public canCheck: boolean;
  public isApproveSummary: boolean;
}

export class LeaveSummaryItemModel {
  public id: number;
  public staffName: string;
  public positionName: string;
  public officeName: string;
  public applicationDate: string;
  public leaveType: LeaveType;
  public startDate: string; 
  public endDate: string;
  public days: number;
  public status: LeaveStatus;
  public reason: string;
  public comment: string; 
  public canApprove: boolean;
}


export class LeaveType {
  public id: number;
  public label: string;
}

export class LeaveStatus  {


  public id: number;
  public label: string;
  public totalCount: number; 
}






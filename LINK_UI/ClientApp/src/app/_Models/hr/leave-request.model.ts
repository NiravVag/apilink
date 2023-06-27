export class LeaveRequestModel {
  public startDate: any;
  public endDate: any;
  public startDayType: number;
  public endDayType: number; 
  public leaveTypeId: number;
  public leaveDays: number;
  public reason: string;
  public statusName: string;
  public statusId: number; 

  constructor() {
    this.leaveTypeId = 0;
    this.startDayType = 0;
    this.endDayType = 0;
    this.statusName = "Request";
    this.statusId = 0; 
  }
}


export enum LeaveRequestResult {
  Success = 1,
  CannotFindTypes = 2,
  CannotFindLeaveRequest = 3,
  CannotFinddayTypeList = 4
}

export enum SaveLeaveRequestResult {
  Success = 1,
  StartDateIsRequired = 2,
  EndDateIsRequired = 3,
  LeaveTypeIsRequired = 4,
  NotFound = 5,
  UnAuthorized = 6,
  CannotSave = 7,
  StaffEntityNotMatched=8
}

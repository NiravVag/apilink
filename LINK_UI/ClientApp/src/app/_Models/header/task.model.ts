export class TaskModel {
  public id: string;
  public userId: number;
  public staffName: string;
  public type: TaskType;
  public linkId: number;
  public parentId: number;
  public isDone: boolean;
  public createdOn: string;
  public dateType: TaskDateType;
  public typeLable: string;
}


export enum TaskType {
  LeaveToApprove = 1,
  ExpenseToApprove = 2,
  ExpenseToCheck = 3,
  ExpenseToPay = 4,
  InspectionVerified = 5,
  InspectionConfirmed = 6,
  SplitInspectionBooking = 8,
  ScheduleInspection = 9,
  QuotationModify = 10,
  QuotationSent = 11,
  QuotationCustomerConfirmed = 12,
  QuotationCustomerReject = 13,
  QuotationToApprove = 7,
  QuotationPending = 14,
  UpdateCustomerToFB = 15,
  UpdateSupplierToFB = 16,
  UpdateFactoryToFB = 17,
  UpdateProductToFB = 18
}

export enum TaskFilterType {
  Leave = 1,
  Expense = 2,
  Inspection = 3,
  Quotation = 4
}
export class TaskResponse {
  public result: TaskResult;
  public data: Array<TaskModel>;
}

export class TaskSearchRequest {
  public skip: number;
  public isUnread?: boolean;
  public taskType?: TaskFilterType;
}

export enum TaskDateType {
  Today = 1,
  Yesterday = 2,
  Older = 3
}

export enum TaskResult {
  Success = 1,
  NotFound = 2
}

export class TaskMessage {

  constructor(t: TaskModel, m: string, l: string, i: string) {
    this.task = t;
    this.message = m;
    this.link = l;
    this.iconName = i;
  }
  public message: string;
  public task: TaskModel;
  public link: string;
  public iconName: string
}

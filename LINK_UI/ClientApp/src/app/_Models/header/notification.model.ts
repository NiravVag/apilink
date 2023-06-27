export class NotificationModel {
  public id: string;
  public userId: number;
  public message: string;
  public type: NotificationType;
  public linkId: number;
  public createdOn: string;
  public isRead: boolean;
  public typeLabel: string;
  public dateType: NotificationDateType;
}

export enum NotificationFilterType {
  Leave = 1,
  Expense = 2,
  Inspection = 3,
  Quotation = 4,
  Report = 6
}

export enum NotificationType {
  LeaveApproved = 1,
  LeaveRejected = 2,
  ExpenseApproved = 3,
  ExpenseChecked = 4,
  ExpensePaid = 5,
  ExpenseRejected = 6,
  LeaveApprovedHrLeave = 7,
  LeaveCanceled = 8,
  InspectionRequested = 10,
  InspectionConfirmed = 11,
  InspectionCancelled = 12,
  InspectionModified = 13,
  InspectionVerified = 14,
  InspectionRescheduled = 15,
  InspectionSplit = 16,
  QuotationAdd = 17,
  QuotationToApprove = 18,
  QuotationSent = 19,
  QuotationCustomerConfirmed = 20,
  QuotationCustomerReject = 21,
  QuotationCancelled = 22,
  QuotationRejected = 23,
  QuotationModified = 24,
  BookingQuantityChange = 25,
  InspectionHold = 27,
  FastReportGenerationFailed = 32
}

export class NotificationResponse {
  public result: NotificationResult;
  public data: Array<NotificationModel>;
}

export class NotificationSearchRequest {
  public skip: number;
  public isUnread?: boolean;
  public notificationType?: NotificationFilterType;
}

export enum NotificationDateType {
  Today = 1,
  Yesterday = 2,
  Older = 3
}

export enum NotificationResult {
  Success = 1,
  NotFound = 2
}

export class NotificationMessage {

  constructor(n: NotificationModel, m: string, l: string, i: string) {
    this.notification = n;
    this.message = m;
    this.link = l;
    this.iconName = i;
  }
  public message: string;
  public notification: NotificationModel;
  public link: string;
  public iconName: string
}

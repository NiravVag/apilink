import { expenseSummaryExportTypeList } from "src/app/components/common/static-data-common";
import { array } from "@amcharts/amcharts4/core";
import { DataSource } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class ExpenseClaimListModel extends summaryModel {
  public employeeValues: Array<any>;
  public statusValues: Array<any>;
  public locationValues: Array<any>;
  public startDate: any;
  public endDate: any;
  public isClaimDate: boolean;
  public canCheck: boolean;
  public items: Array<ExpenseClaimGroup> = [];
  public claimTypeIds: Array<any>;
  public claimIdList: Array<number>;
  public employeeTypeId: number;
  public companyIds: any;
  public payrollCompanyIds: Array<any>;
  isAutoExpense: boolean;
  exportType: number;
  constructor() {
    super();
    this.isClaimDate = true;
  }
}

export class ExpenseClaimItemModel {
  public id: number;
  public claimNo: string;
  public claimDate: string;
  public employee: string;
  public expenseAmount: number;
  public foodAllowance: string;
  public totalAmount: number;
  public payrollCurrencyId: number;
  public payrollCurrency: string;
  public statusUserName: string;
  public statusStatusDate: string;
  public inspType: string;
  public statusId: number;
  public status: string;
  public canApprove: boolean;
  public comment: string;
  public isChecked: boolean = false;
  isAutoExpense: boolean;
  staffId: number;
  bookingIdList: Array<number>;
}

export class ExpenseClaimGroup {
  public expenseAmount: number;
  public foodAllowance: string;
  public totalAmount: number;
  public items: Array<ExpenseClaimItemModel> = [];
}





export enum ExpenseClaimListResult {
  Success = 1,
  StartDateRequired = 2,
  EndDateRequired = 3,
  StatusRequired = 5,
  EmployeeRequired = 6,
  NotFound = 7,
  NoAffectedLocations = 8
}

export enum ExpenseClaimTypeEnum {
  Audit = 1,
  Inspection = 2,
  NonInspection = 3
}
export class ExpenseMasterData {
  payrollCompanyLoading:boolean;
  payrollCompanyList: Array<DataSource>;
  employeeTypeLoading: boolean;
  toggleFormSection: boolean;
  hasExpenseRole: boolean;
  updateStatusLoading: boolean;
  showUpdateStatusBtn: boolean;
  pendingExpenseExistLoading: boolean;
  updatetitleMsg: string;  
  expenseSummaryExportTypeList;
  constructor() {
    this.payrollCompanyList = new Array<DataSource>();
    this.expenseSummaryExportTypeList = expenseSummaryExportTypeList;
  }
}

export class ExpenseClaimUpdateStatus {
  id: number;
  expenseType: boolean
  currentStatusId: number;
  nextStatusId: number;
}

export class PendingBookingExpenseRequest {
  public qcId: number;
  public bookingIdList: Array<number>;

  constructor() {
    this.bookingIdList = [];
  }
}

export class PendingBookingExpenseResponse {
  result: PendingBookingExpenseResponseResult;
  bookingIdList: Array<number>;
}

export enum PendingBookingExpenseResponseResult {
  notConfigure = 1,
  configure = 2
}
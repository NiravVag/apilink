import { DateObject } from "src/app/components/common/static-data-common";
import { ResponseResult } from "../common/common.model";

export class ExpenseClaimModel {
  public id: number;
  public name: string;
  public locationName: string;
  public claimDate: any;
  public countryId: number;
  public locationId: number;
  public staffId: number;
  public claimNo: string;
  public statusId: number;
  public status: string;
  public expensePuropose: string;
  public currencyId: number;
  public currencyName: string;
  public totalAmount: number;
  public expenseList: Array<ExpenseClaimDetails>;
  public claimTypeId: number;
  public managerApproveRequired: boolean = true;
  public isAutoExpense: boolean = false;
  public isTraveAllowanceExist: boolean = false;
  public isFoodAllowanceExist: boolean = false;
  public userTypeId: number;
  public outSourceCompanyName: string;

  constructor() {
    this.claimDate = {};
    this.managerApproveRequired = true;
  }
}

export class PendingExpenseRequest {
  public qcId: number;
  public bookingList: Array<number>;

  constructor() {
    this.bookingList = [];
  }
}

export class ExpenseClaimDetails {
  public id: number;
  public bookingNo: number;
  public expenseDate: any;
  public expenseTypeId: number;
  public receipt: boolean;
  public startCity: any;
  public destCity: any;
  public currencyId: number;
  public actualAmount: number;
  public exchangeRate: number;
  public amount: number;
  public description: string;
  public files: Array<ExpenseClaimReceipt>;
  public tripMode: number;
  public qcId: number;
  public qcName: string;
  public selectedBookingId: number;
  public tax: number;
  public taxAmount: number;
  public manDay: number;
  public isTravelExpense: boolean;
  startCityLoading: boolean;
  destCityLoading: boolean;
  constructor() {
    this.startCityLoading = false;
    this.destCityLoading = false;
  }
}

export class ExpenseType {
  public id: number;
  public label: string;
  public isTravel: boolean;

}

export enum ExpenseClaimResult {
  Success = 1,
  CannotFindCountries = 2,
  CannotFindCurrentStaff = 3,
  CannotFindLocation = 4,
  CannotFindCurrencies = 5,
  CannotFindExpenseTypes = 6,
  CannotFindCurrentExpenseClaim = 7
}

export class ExpenseClaimResponse {
  public countryList: Array<any>;
  public currencyList: Array<any>;
  public expenseTypeList: Array<ExpenseType>;
  public expenseClaim: ExpenseClaimModel;
  public result: ExpenseClaimResult;
  public canEdit: boolean;
  public canApprove: boolean;
  public canCheck: boolean;
}

export class ExpenseClaimReceipt {
  public file: File;
  public fileName: string;
  public isNew: boolean;
  public id: number;
  public mimeType: string;
  public guidId: string;
  public uniqueld: string;
}
export class ExpenseClaimType {
  public id: number;
  public name: string;
}

export class BookingDetail {
  public bookingId: number;
  public serviceDateFrom: string;
  public serviceDateTo: string;
  public customer: string;
  public supplier: string;
  public factory: string;
  public serviceType: string;
  public selected: boolean;
  public qcId: number;
  public qcName: string;
}

export enum ExpenseClaimTypeEnum {
  Audit = 1,
  Inspection = 2,
  NonInspection = 3
}



export class ExpenseClaimTypeData {
  public expenseClaimTypeList: Array<ExpenseClaimType> = [];
  public bookingDetails: Array<BookingDetail>;
  public selectedBookingDetails: any;
  public selectedAllBooking: boolean = false;
}


export class ExpenseBookingDetailAccess {
  public bookingNoVisible: boolean;
  public bookingNoEnabled: boolean;
  public bookingDetailVisible: boolean;
  public claimTypeEnabled: boolean;
}

export enum ExpenseClaimStatusEnum {
  New = 0,
  Pending = 1,
  Approved = 2,
  Rejected = 3,
  Paid = 4,
  Checked = 5,
  Cancelled = 6
}

export class ExpenseClaimMasterData {
  pageLoading: boolean;
}

export class ExpenseFoodClaimRequest {
  countryId: number;
  expenseDate: DateObject;
  currencyId: number;
}

export class ExpenseFoodClaimResponse {
  result: ResponseResult;
  actualAmount: number;
}

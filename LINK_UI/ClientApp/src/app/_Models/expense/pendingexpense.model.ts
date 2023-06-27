import { summaryModel } from "../summary.model";

export class PendingExpenseModel extends summaryModel {    
    public startDate: any;
    public endDate: any;
    public officeIdList: any[] = [];
    public qcIdList: any[] = [];
    public searchTypeId: number;
    public searchTypeText: string = "";
    public datetypeid: number;
    public statusId: number;
    public expenseTypeId:number;
}

export const PendingExpenseStatusList = [
    { id: 1, name: "Configured" },
    { id: 2, name: "NotConfigured" }
  ];

  export const PendingExpenseTypeList = [
    { id: 1, name: "Travel Expense" },
    { id: 2, name: "Food Expense" }
  ];


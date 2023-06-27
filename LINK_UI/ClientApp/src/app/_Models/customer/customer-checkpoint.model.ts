import { BehaviorSubject } from "rxjs";
import { summaryModel } from "../summary.model";

export class EditCustomerCheckPointModel {
  id: number;
  customerId: number;
  serviceId: number;
  checkPointId: number;
  remarks?: string;
  brandId: Array<number>;
  deptId: Array<number>;
  serviceTypeId: Array<number>;
  countryIdList: Array<number>;
  constructor() {
  }
}

export class CustomerCheckPointSummaryModel extends summaryModel {
  public checkPointList: Array<any>;
  public customerList: Array<any>;
  public serviceList: Array<any>;
  public customerBrandList: Array<any>;
  public customerDeptList: Array<any>;
  public serviceTypeList: Array<any>;
  public serviceLoading: boolean;
  public customerCPLoading: boolean;
  public customerLoading: boolean;
  public isNewItem: boolean;
  public deleteId: number;
  public customerDisabled: boolean;
  public saveLoading: boolean;
  public customerIdParam: number;
  public customerBrandLoading: boolean;
  public customerDeptLoading: boolean;
  public serviceTypeLoading: boolean;
  countryList: Array<any>;
  countryLoading: boolean
  countryInput: BehaviorSubject<string>;

  constructor() {
    super();
    this.countryInput = new BehaviorSubject<string>("");
  }
}

export class CustomerCheckPointModel {
  id: number;
  customerName: number;
  serviceName: number;
  checkPointName: number;
  remarks: string;
  customerId: number;
  serviceId: number;
  checkPointId: number;
  brandIdList: Array<number>;
  deptIdList: Array<number>;
  serviceTypeIdList: Array<number>;
  brandNames: string;
  deptNames: string;
  serviceTypeNames: string;
  countryIdList: Array<number>;
  constructor() {
  }

}

export enum CheckPointTypeEnum {
  QuotationRequired = 1,
  QuotationApproveByManager = 2,
  POQuantityModification = 3,
  ICRequired = 4,
  CustomerDecisionRequired = 5,
  SkipQuotationSentToClient = 6,
    NewReportFormat = 7,
    AutoCustomerDecisionForPassReportResult = 9,
  SupplierCreationNotAllowedByCustomer=10,
  FactoryCreationNotAllowedByCustomer=11,
  FactoryCreationNotAllowedBySupplier=12,
  PoProductBySupplier=14,
  HideMultiSelectCustomerDecision = 15
}

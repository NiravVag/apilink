import { BehaviorSubject } from "rxjs";
import { UserRightsList } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class DMUserSummaryModel extends summaryModel {

  searchType: number;
  roleId: number;
  staffId: number;
  moduleName: string;
  moduleId: number;
  rightIds: Array<number>;

  constructor() {
    super();
  }
}

export class DMUserSummaryMasterModel {

  roles: Array<any> = [];
  roleLoading: boolean = false;

  staffs: Array<any> = [];
  staffLoading: boolean = false;
  staffInput: BehaviorSubject<string>;
  staffRequest: CommonDataSourceRequest;
  userRights = UserRightsList;

  searchType = DMUserManagementSearchType;

  searchloading: boolean;

  moduleLoading: boolean;

  dmSearchTypeList = dmSearchTypeList;

  deleteLoading: boolean;

  searchTypeName: string;
  searchTypeValue: string;

  filterRights: string;
  constructor() {
    this.staffRequest = new CommonDataSourceRequest();
    this.staffInput = new BehaviorSubject<string>("");
  }
}

export enum DMUserManagementSearchType {
  Role = 1,
  Staff = 2,
  Tree = 3
}


export const dmSearchTypeList = [
  {
    id: 1,
    name: "Role"
  },
  {
    id: 2,
    name: "Employee"
  },
  {
    id: 3,
    name: "Tree"
  }
]

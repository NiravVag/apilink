import { ParentDataSource } from "../common/common.model";

export class MasterUserDataAccess {
    redirectType: number;
    redirectId: number;
    profileList: Array<DataList>;
    userName: string;

    profileLoading: boolean;
    masterCustomerLoading: boolean;
    masterRoleLoading: boolean;
    masterProductCategoryLoading: boolean;
    masterOfficeLoading: boolean;
    masterServiceLoading: boolean;
    saveLoading: boolean;


    masterCustomerList: Array<DataList>;
    masterOriginalCustomerList: Array<DataList>;
    masterRoleList: Array<DataList>;
    masterProductCategoryList: Array<DataList>;
    masterOfficeList: Array<DataList>;
    masterServiceList: Array<DataList>;

    customerDepartmentsList: Array<ParentDataSource>;
    customerBrandsList: Array<ParentDataSource>;
}

export class UserDataAccess {
    id: number;
    userId: number;
    profileId: number
    emailAccess: boolean;
    customerId: number;
    userAccessList: UserAccessMasterData[];
    constructor() {
        this.userAccessList = [];
    }
}

export interface DataList {
    id: number;
    name: string;
}

export class UserAccessMasterData {
    customerId: number;
    roleIdAccessList: number[];
    disableProductCategory: boolean;
    productCategoryIdAccessList: number[];
    officeIdAccessList: number[];
    serviceIdAccessList: number[];

    cusDepartmentIdAccessList: number[];
    customerDepartmentList: Array<DataList>;
    customerDepartmentLoading: boolean;

    cusBrandIdAccessList: number[];
    customerBrandList: Array<DataList>;
    customerBrandLoading: boolean;

    cusBuyerIdAccessList: number[];
    customerBuyerList: Array<DataList>;
    customerBuyerLoading: boolean;
}

export class UserNameResponse
{
     name: string;
     result: ResponseResult;
}

export enum ResponseResult {
    Success = 1,
    Faliure = 2,
    RequestNotCorrectFormat = 3,
    NotFound = 4,
    Error = 5,
    Exists = 6,
    MoreRuleExists = 7
}

export class SaveUserConfigResponse{
    result: ResponseResult;
    id: number;
}

export class EditUserConfigResponse{
    result: ResponseResult;
    data: UserDataAccess;
}
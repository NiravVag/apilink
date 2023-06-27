import { summaryModel } from "../summary.model";

export class UserAccountSummaryModel extends summaryModel {
    userTypeId?: number;
    countryValues: any[];
    id?: number;
    name: string;
}

export class UserAccountSummaryItemModel {
    public id?:number;
    public name:string;
    public gender:string;
    public departmentName:string;
    public position:string;
    public office:string;
    public country:string;
    public hasAccount?: boolean;
    public userTypeId?: number;
}

export class UserAccountModel {
    public id?: number;
    public userName: string;
    public password: string;
    public fullname: string;
    public roles?:Array<number>=[];
    public status: boolean;
    public userId?: number;
    public userTypeId?: number;
    public contact?: number;
    public apiServiceIds:any;
    //public primaryEntity:any;
    public userRoleEntityList?:Array<UserRoleEntity>=[];
}

export class UserRoleEntity{
    public roleId: number;
    public roleName: number;
    public roleEntity?:Array<number>=[];
}

export class UserDetail {
    userName: string;
    password: string;
    isChangePassword: boolean;
}

export class UserAccountMasterModel {
    deleteId : number;
    removedName: string;
}

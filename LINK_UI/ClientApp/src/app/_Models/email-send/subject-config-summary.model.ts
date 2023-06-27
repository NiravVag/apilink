import { BehaviorSubject } from "rxjs";
import { summaryModel } from "../summary.model";
import { CommonDataSourceRequest, DataSource } from "../common/common.model";

export class SubConfigSummaryModel extends summaryModel {
    customerIds: Array<number>;
    emailTypeId: number;
    moduleId: number;
    templateName: string;
}

export class SubConfigSummaryMasterModel {
    customerList: Array<DataSource>;
    customerLoading: boolean;
    customerInput: BehaviorSubject<string>;
    templateName: string;
    customerModelRequest: CommonDataSourceRequest;
    searchLoading: boolean;
    deleteId: number;
    deleteLoading: boolean;  
    emailTypeLoading: boolean;
    emailTypeList: Array<DataSource>;
    moduleLoading: boolean;
    moduleList: Array<DataSource>;  

    constructor() {
        this.customerInput = new BehaviorSubject<string>("");
        this.customerModelRequest = new CommonDataSourceRequest();
    }
}

export class SubConfigItem {
    templateName: string;
    templateDisplayName: string;
    customerName: string;
    subConfigId: number;
    isDelete: boolean;
    emailType:string;
    moduleName:string;
}
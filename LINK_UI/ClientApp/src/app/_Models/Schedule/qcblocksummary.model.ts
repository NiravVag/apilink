import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest, DataSource } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class QCBlockRequestModel extends summaryModel
{
    officeIds: number[];
    qcIds: number[];
}

export class QCBlockSummaryModel {
    qcList: Array<DataSource>;
    qcLoading: boolean;
    qcInput: BehaviorSubject<string>;

    officeList:Array<DataSource>;
    officeLoading: boolean;
    officeInput: BehaviorSubject<string>;

    searchLoading: boolean;
    exportDataLoading: boolean;
    deleteLoading: boolean;
    isQCBlockSelected: boolean;
    qcModelRequest: CommonDataSourceRequest;
    officeModelRequest: CommonDataSourceRequest;
    selectAllCheckbox: boolean;
    
    constructor() {
        this.qcInput = new BehaviorSubject<string>("");
        this.officeInput = new BehaviorSubject<string>("");
        this.qcModelRequest = new CommonDataSourceRequest();
        this.officeModelRequest = new CommonDataSourceRequest();
    }
}

export class QCBlockSummaryItem {
    qcBlockId: number;
    qcName: string;
    customerNames: string;
    supplierNames: string;
    factoryNames: string;
    productCategoryNames: string;
    productCategorySubNames: string;
    productCategorySub2Names: string;
    isDeleteRow: boolean;
}
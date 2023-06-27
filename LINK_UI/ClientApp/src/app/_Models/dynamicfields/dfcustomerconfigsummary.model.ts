import{summaryModel} from "../../_Models/summary.model";
export class DFCustomerConfigsummarymodel extends summaryModel{

    public moduleId:number;

    public customerDataList:any;

    public controlTypeDataList:any;
}



export class DFCustomerConfigItem {

    public id:number;
    public customerName:string;
    public moduleName:string;
    public controlName:string;
    public label:string;
    public displayOrder:number;
    public isBooking:boolean;
}

export class DFCustomerConfigMaster {

    public customerList:any;
    public moduleList:any;
    public controlTypeList:any;
}

export class DFCustomerConfigToRemove {

    constructor() {
    }
    id: number;
    name: string;
}

export class DfCustomerConfigurationRequest {
    customerId: number;
    moduleId: number;
    dataSourceTypeIds: Array<number>;
    constructor(){
        this.dataSourceTypeIds = [];
    }
}
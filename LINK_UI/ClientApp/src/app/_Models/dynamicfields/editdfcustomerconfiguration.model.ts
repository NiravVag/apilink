export class EditDfCustomerConfiguration {
    public id: number;
    public customerId: number;
    public moduleId: number;
    public controlTypeId: number;
    public label: string;
    public type: string;
    public dataSourceType: number;
    public displayOrder: number;
    public fbReference: string;
    public active: boolean;
    public isBooking:boolean;
    public controlAttributeList: Array<DfControlAttributes>;
    constructor() {
        this.id = 0;
        this.controlAttributeList=[];
    }
}

export class DfControlAttributes {
    public id: number;
    public name:string;
    public dataType:string;
    public value: number;
    public attributeId:number;
    public controlAttributeId: number;
    public controlTypeId: number;
    public active: boolean
}

export class EditDfCustomerConfigurationMaster {
    public customerList: any[];
    public moduleList: any[];
    public controlTypeList: any[];
    public dataSourceTypeList: any[];
    public dataSourceList: any[];
    public dataTypeList: any[];
    public requiredTypeList: any[];
    public dropdownTypeList: any[];
    public parentDropdownTypeList: any[];
    public dfControlAttributes:any[];
    public controlAttributes: Array<any> = [];
    constructor() {
        this.customerList = [];
        this.moduleList = [];
        this.dataSourceTypeList = [];
        this.dataSourceList = [];
        this.dataTypeList = [];
        this.requiredTypeList = [];
        this.dropdownTypeList = [];
        this.parentDropdownTypeList=[];
        this.controlAttributes=[];
        this.dfControlAttributes=[];
    }

}

export class StringKeyValuePair{
 public id:string;
 public name:string;   
}
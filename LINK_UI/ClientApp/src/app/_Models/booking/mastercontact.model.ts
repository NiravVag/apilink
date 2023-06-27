export class SaveMasterContactRequest {
    contactList: Array<MasterContact>;
    masterContactTypeId: number;
    customerId: number;
    supplierId: number;
    factoryId: number;
    constructor() {
        this.contactList = [];
        this.customerId = null;
        this.supplierId = null;
        this.factoryId = null;
    }
}

export class MasterContact {
    name: string;
    emailId: string;
    phoneNo: string;
    constructor() {
        this.name = "";
        this.emailId = "";
        this.phoneNo = "";
    }
}

export enum MasterContactType {
    customer = 1,
    supplier = 2,
    factory = 3
}

export class SaveMasterContactResponse {
    saveMasterContactResult: SaveMasterContactResult
}

export enum SaveMasterContactResult {
    Success = 1,
    Failed = 2
}

export class MasterContactData {
    public contactValidators: Array<any>;
    contactList: Array<MasterContact>;
    saveMasterContactRequest: SaveMasterContactRequest;
    masterContactTypeId: number;
    masterContactTypeEnum = MasterContactType;
    constructor() {
        this.saveMasterContactRequest = new SaveMasterContactRequest();
        this.contactValidators = [];
        this.contactList = [];
    }
}


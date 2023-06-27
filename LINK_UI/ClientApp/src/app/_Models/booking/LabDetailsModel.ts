import { NullLogger } from "@microsoft/signalr";
import { commonDataSource } from "./inspectionbooking.model";

export class LabAddressRequest {
    labIdList: Array<number>;
    constructor() {
        this.labIdList = [];
    }
}

export class LabAddressListResponse {
    addressList: Array<commonDataSource>;
    result: LabAddressResult;
}

export enum LabAddressResult {
    Success = 1,
    CannotGetTypeList = 2
}

export class LabContactRequest {
    labIdList: Array<number>
    customerId: number;
    constructor() {
        this.labIdList = [];
        this.customerId = null;
    }
}

export class LabContactsListResponse {
    labContactList: Array<number>;
    result: LabDataContactsListResult;
}
export enum LabDataContactsListResult {
    Success = 1,
    CannotGetTypeList = 2
}

export class MasterLabAddressData {
    labAddressValidators: Array<any>;
    labAddressList: Array<SaveLabAddressData>;
    labId: number;
    pickingItem: any;
    saveLabAddressRequestData: SaveLabAddressRequestData;
    constructor() {
        this.labAddressValidators = [];
        this.labAddressList = [];
        this.pickingItem = null;
        this.saveLabAddressRequestData = new SaveLabAddressRequestData();
    }
}

export class SaveLabAddressRequestData {
    labId: number;
    labAddressList: Array<SaveLabAddressData>;
    constructor() {
        this.labAddressList = new Array<SaveLabAddressData>();
    }
}

export class SaveLabAddressData {
    countryId: number;
    regionId: number;
    cityId: number;
    zipCode: number;
    way: string;
    localLanguage: string;
    addressTypeId: number;
    regionList: any;
    cityList: any;
    constructor() {
        this.countryId = null;
        this.regionId = null;
        this.cityId = null;
        this.zipCode = null;
        this.way = "";
        this.localLanguage = "";

        this.regionList = [];
        this.cityList = [];
    }
}

export enum SaveLabAddressResult
{
    Success = 1,
    LabAddressIsNotSaved = 2,
    LabAddressIsNotFound = 3,
}
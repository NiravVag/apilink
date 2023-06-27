import { BookingStatus } from "src/app/components/common/static-data-common";
import { UserModel } from "../user/user.model";

export class CombineOrders {
    public id: number;
    public inspectionId: number;
    public productId: number;
    public combineProductId: number;
    public combineCount: number;
    public productName: string;
    public colorCode: string;
    public productDescription: string
    public totalBookingQuantity: number;
    public samplingQuantity: number;
    public combinedAqlQuantity: number;
    public selectedProducts: Array<Product>;
    public isProductListEnable: boolean;
    public isProductQTYEnable: boolean = true;
    public aqlLevel: number;
    public factoryReference: string;
    public poList: any[] = [];
    public isDisplayMaster: boolean;
    public parentProductId: number;
    public parentProductName: string;
}

export class CombineMasterData {
    currentUser: UserModel;
    internalUserCombineSaveStatus: any;
    externalUserCombineSaveStatus: any;
    constructor() {
        this.internalUserCombineSaveStatus = [BookingStatus.Requested, BookingStatus.Verified, BookingStatus.Confirmed, BookingStatus.AllocateQC, BookingStatus.Rescheduled];
        this.externalUserCombineSaveStatus = [BookingStatus.Requested];
    }
}

export class SaveCombineOrdersRequest {
    public id: number;
    public inspectionId: number;
    public aqlId: number;
    public productId: number;
    public combineProductId: number;
    public samplingQuantity: number;
    public aqlQuantity: number;
    public combinedAqlQuantity: number;

}

export class Product {
    public productId: number;
    public productName: string;
}

export class SamplingQuantityRequest {
    public aql: number;
    public orderQuantity: number;
}

export class CombineOrderProductFilter {
    public productList: any;
    public combineGroupList: any;
    public aqlList: any;
}

export class CombineOrderProductFilterRequest {
    public filterProductIds: any;
    public filterCombineProductId: any;
    public filterAqlId: any;
}

export enum SaveCombineOrdersResult {
    Success = 1,
    CombineOrdersIsNotSaved = 2,
    CombineOrdersIsNotFound = 3,
    CombineOrdersExists = 4,
    CombineProductIdCannotBeNull = 5,
    CombineAqlQuantityGreaterThanZero = 6,
    InternalUserRoleNotMatched = 7,
    InternalUserStatusNotMatched = 8,
    ExternalUserStatusNotMatched = 9
}
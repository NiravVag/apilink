import { DateObject } from "src/app/components/common/static-data-common";
import { summaryModel } from "../summary.model";

export class TravelTariffSaveRequest {
    id: number;
    startPort: number;
    townId: number;
    startDate: any;
    endDate: any;
    travelTariff: number;
    travelCurrency: number;
    countryId: number;
    provinceId: number;
    cityId: number;
    countyId: number;
}
export class TravelTariffSaveResponse {
    id: number;
    result: TravelTariffResponseResult
}

export enum TravelTariffResponseResult {
    Success = 1,
    Error = 2,
    RequestNotCorrectFormat = 3,
    NotFound = 4,
    AllreadyExist = 5
}


// Search Request and Response

export class TravelTariffSearchRequest extends summaryModel {
    startPort: number;
    countryId: number;
    provinceId: number;
    cityId: number;
    townId: number;
    countyId: number;
    startDate: DateObject;
    endDate: DateObject;
    status: boolean = true;
}

export class TravelTariffSummary {

    countryLoading: boolean = false;
    provinceLoading: boolean = false;
    cityLoading: boolean = false;
    countyLoading: boolean = false;
    exportDataLoading: boolean = false;
    townLoading: boolean = false;
    startPortLoading: boolean = false;
    countryList: any[];
    provinceList: any[];
    cityList: any[];
    countyList: any[];
    townList: any[];
    startPortList: any[];
    deleteLoading: boolean = false;
    constructor() {
        this.startPortList = [];
        this.countryList = [];
        this.provinceList = [];
        this.cityList = [];
        this.townList = [];
        this.countyList = [];
    }
}

export class TravelTariffDetail {
    id: number;
    startPortName: string;
    travelTariff: number;
    countryName: string;
    provinceName: string;
    cityName: string;
    townName: string;
    countyName: string;
    startDate: string;
    endDate: string;
}

export class TravelTariffGetAllResponse {
    travelTariffDetails: Array<TravelTariffDetail>
    result: TravelTariffGetAllResult
    totalCount: number
    index: number
    pageSize: number
    pageCount: number
}

export enum TravelTariffGetAllResult {
    Success = 1,
    Failure = 2,
    NotFound = 3
}

// edit Request 

export class TravelTariffData {
    id: number;
    startPortId: number;
    startPortName: string;
    travelTariff: number;
    travelCurrency: number;
    townId: number;
    startDate: string;
    endDate: string;
    countryId: number;
    provinceId: number;
    cityId: number;
    countyId: number;
}

export class TravelTariffGetResponse {
    travelTariff: TravelTariffData;
    result: TravelTariffGetResult;
}

export enum TravelTariffGetResult {
    Success = 1,
    Failure = 2,
    NotFound = 3
}

// delete request

export class TravelTariffDeleteResponse {
    id: number;
    result: TravelTariffDeleteResult;
}

export enum TravelTariffDeleteResult {
    Success = 1,
    NotFound = 2,
    Failure = 3,
    RequestNotCorrect = 4
}

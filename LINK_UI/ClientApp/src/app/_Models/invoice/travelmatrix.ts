import { CityDataSourceRequest, DataSource } from "../common/common.model";
import { summaryModel } from "../summary.model";
import { Observable } from "rxjs/internal/Observable";
import { Subject } from "rxjs";

export class TravelMatrixMasterModel {
    tariffTypeList: Array<DataSource>;
    customerList: Array<DataSource>;
    travelCurrencyList: Array<DataSource>;
    sourceCurrencyList: Array<DataSource>;
    countryList: Array<DataSource>;
    provinceList: Array<DataSource>;
    cityList: Array<DataSource>;
    countyList: Array<DataSource>;

    exportDataLoading: boolean;
    tariffTypeLoading: boolean;
    travelCurrencyLoading: boolean;
    customerLoading: boolean;
    sourceCurrencyLoading: boolean;
    countryLoading: boolean;
    provinceLoading: boolean;
    cityLoading: boolean;
    countyLoading: boolean;
    searchLoading: boolean;
    saveLoading: boolean;
    addLoading: boolean;

    copyLoading: boolean;
    deleteLoading: boolean;
    selectedAllCheckBox: boolean;
    showButton: boolean;
}

export class TravelMatrixSummary extends summaryModel {
    tariffTypeId: number;
    travelCurrencyId: number;
    customerId: number;
    sourceCurrencyId: number;
    countryId: number;
    provinceId: number;
    cityId: number;
    countyId: number;
    isExport: boolean;
    searchTypeId: number;
}

export class TravelMatrixModel {

    constructor() {
        this.typeInspPortListInput = new Subject<string>();
        this.typeInspPortCityListInput = new Subject<string>();
        this.cityRequest = new CityDataSourceRequest();
    }

    id: number;
    customerLoading: boolean;
    tariffTypeLoading: boolean;
    countryLoading: boolean;
    provinceLoading: boolean;
    cityLoading: boolean;
    countyLoading: boolean;
    inspPortLoading: boolean;
    travelCurrencyLoading: boolean;
    sourceCurrencyLoading: boolean;
    isChecked: boolean;

    customerId: number;
    tariffTypeId: number;
    countryId: number;
    provinceId: number;
    countyId: number;
    cityId: number;
    inspPortId: number;
    distanceKM: number;
    travelTime: number;
    busCost: number;
    trainCost: number;
    taxiCost: number;
    hotelCost: number;
    otherCost: number;
    markUpCost: number;
    airCost: number;
    markUpCostAir: number;
    fixExchangeRate: number;
    remarks: string;
    sourceCurrencyId: number;
    travelCurrencyId: number;
    typeInspPortListInput: Subject<string>;
    provinceList: Array<DataSource>;
    countyList: Array<DataSource>;
    cityList: Array<DataSource>;
    inspPortList: Observable<DataSource[]>;
    selectedPortList: DataSource;

    typeInspPortCityListInput: Subject<string>;
    inspPortCityList: Observable<DataSource[]>;
    inspPortCityLoading: boolean;
    cityRequest: CityDataSourceRequest;
    selectedPortCityList: DataSource;
    inspPortCityId: number;
}

export interface ProvinceList {
    countryId: Number;
    provinceList: Array<DataSource>;
}

export interface CityList {
    provinceId: number;
    cityList: Array<DataSource>;
}
export interface CountyList {
    cityId: number;
    countyList: Array<DataSource>;
}

export enum TravelMatrixResponseResult {
    Success = 1,
    Error = 2,
    RequestNotCorrectFormat = 3,
    NotFound = 4,
    DefaultData = 5,
    MoreMatrixExists = 6,
    PriceCardNotCorrect = 7
}

export class TravelMatixSaveResponse {
    result: TravelMatrixResponseResult;
    existsData: Array<TravelMatrixExists>;
}

export class TravelMatixSearchResponse {
    result: TravelMatrixResponseResult;
    getData: Array<TravelMatrixModel>;
    isDataExist: boolean;
}

export class AreaData {
    id: number;
    dataList: Array<DataSource>;
    constructor() {
        this.dataList = Array<DataSource>();
    }
}

export class TravelMatrixExists {
    customerName: string;
    tariffTypeName: string;
    countryName: string;
    countyName: string;
    provinceName: string;
    cityName: string;
    sourceCurrencyName: string;
    travelCurrencyName: string;
}

export enum TariffType {
    standardA = 1,
    standardB = 2,
    customized = 3
}

export enum AreaType {
    country = 1,
    province = 2,
    city = 3
}

export class QuotationTravelMatrix {
    id: number;

    customerId: number;
    tariffTypeId: number;
    countryId: number;
    provinceId: number;
    countyId: number;
    cityId: number;
    inspPortId: number;
    distanceKM: number;
    travelTime: number;
    busCost: number;
    trainCost: number;
    taxiCost: number;
    hotelCost: number;
    otherCost: number;
    markUpCost: number;
    airCost: number;
    markUpCostAir: number;
    fixExchangeRate: number;
    remarks: string;
    sourceCurrencyId: number;
    travelCurrencyId: number;

    busMarkUp: string;
    busTotal: string;
    trainMarkUp: string;
    trainTotal: string;
    taxiMarkUp: string;
    taxiTotal: string;

    landTravelCurrencyTotal: string;
    landSourceCurrencyTotal: string;

    airMarkUp: string;
    airTotal: string;

    hotelMarkUp: string;
    hotelTotal: string;

    otherMarkUp: string;
    otherTotal: string;

    grandTotal: string;
}

export class QuotationTravelMatrixResponse {
    travelMatrixDetails: QuotationTravelMatrix;
    result: TravelMatrixResponseResult;
}

export class TravelMatrixRequest {
    countyId: number;
    cityId: number;
    provinceId: number;
    billMethodId: number;
    billPaidById: number;
    currencyId: number;
    bookingId: number;
    customerId: number;
    ruleId: number;
    quotationId: number;
}

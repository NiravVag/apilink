import { BehaviorSubject } from "rxjs";
import { PageSizeCommon } from "src/app/components/common/static-data-common";
import { CountryDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class FoodAllowanceModel extends summaryModel {
    public countryId: number;
    public startDate: any;
    public endDate: any;
}

export class FoodAllowanceSummaryModel{
    countryList: any;
    countryLoading: boolean
    countryInput: BehaviorSubject<string>;
    countryRequest: CountryDataSourceRequest;

    searchloading: boolean;
    exportLoading: boolean;
    selectedPageSize: any;

    constructor(){
    this.countryRequest = new CountryDataSourceRequest();    
    this.countryInput = new BehaviorSubject<string>("");
    this.exportLoading = false;
    this.selectedPageSize = PageSizeCommon[0];
    }
}

export class FoodAllowanceEditSummaryModel{
    countryList: any;
    countryLoading: boolean
    countryInput: BehaviorSubject<string>;
    countryRequest: CountryDataSourceRequest;

    currencyList: any;
    currencyLoading: boolean

    saveloading: boolean;
    loading: boolean;
    deleteLoading: boolean;

    constructor(){
    this.countryRequest = new CountryDataSourceRequest();    
    this.countryInput = new BehaviorSubject<string>("");
    }
    
}

export class FoodAllowanceEditModel {
    public id: number;
    public countryId: number;
    public startDate: any;
    public endDate: any;
    public foodAllowanceValue: any;
    public currencyId: number;
}

export enum FoodAllowanceResult{
        Success = 1,
        Fail = 2,
        NoDataFound = 3,
        AlreadyExists = 4
}
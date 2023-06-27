import { BehaviorSubject } from "rxjs";
import { PageSizeCommon } from "src/app/components/common/static-data-common";
import { CityDataSourceRequest, CountryDataSourceRequest } from "../common/common.model";
import { summaryModel } from "../summary.model";

export class StartingPortModel extends summaryModel {
    public startingPortId: number;
    public cityId: number;
}

export class StartingPortSummaryModel{
    startingPortNameList: any;
    startingPortNameListLoading: boolean;

    searchloading: boolean;
    selectedPageSize: any;

    addLoading: boolean;

    constructor(){
    this.selectedPageSize = PageSizeCommon[0];
    }
}

export class StartingPortEditSummaryModel{
    cityList: any;
    cityLoading: boolean
    cityInput: BehaviorSubject<string>;
    cityRequest: CityDataSourceRequest;

    saveloading: boolean;
    loading: boolean;
    deleteLoading: boolean;
    showSaveButton: boolean;

    constructor(){
    this.cityRequest = new CityDataSourceRequest();    
    this.cityInput = new BehaviorSubject<string>("");
    this.showSaveButton = false;
    }
    
}

export class StartingPortEditModel {
    public id: number;
    public startPortName: string;
    public cityId: number;
}

export enum StartingPortResult{
        Success = 1,
        Fail = 2,
        NoDataFound = 3,
        AlreadyExists = 4,
        RequestNotCorrectFormat = 5
}

export class StartingPortSaveResponse {
    id: number;
    result: StartingPortResult;
}

export class StartingPortData
{
    startingPortId: number;
    startingPortName: string;
    cityId: number;
    cityName: string;
}

export class StartingPortGetResponse extends summaryModel{
    data: Array<StartingPortData>;
    result: StartingPortResult;
}
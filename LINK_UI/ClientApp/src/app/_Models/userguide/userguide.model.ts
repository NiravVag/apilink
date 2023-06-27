import { DataSource } from "../common/common.model";

export class UserGuideMaster {
    moduleList: Array<DataSource>;
    userGuideDetails: Array<UserGuideDetail>;
    moduleImageList: Array<string>;
    userGuideDetailLoading: boolean;
    constructor() {
        this.moduleList = new Array<DataSource>();
        this.userGuideDetails = new Array<UserGuideDetail>();
        this.moduleImageList = new Array<string>();
        this.userGuideDetailLoading = false;
    }
}

export class UserGuideDetail {
    id: number;
    name: string;
    fileUrl: string;
    videoUrl: string;
    totalPage: string;
    imageIcon: string;
}

export class UserGuideDetailResponse {
    userGuideDetails: Array<UserGuideDetail>;
    result: UserGuideDetailResult;
    constructor() {
        this.userGuideDetails = new Array<UserGuideDetail>();
    }
}

export enum UserGuideDetailResult {
    Success = 1,
    NotFound = 2
}
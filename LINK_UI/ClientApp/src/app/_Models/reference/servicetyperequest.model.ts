export class ServiceTypeRequest {
    customerId: number;
    serviceId: number;
    businessLineId: number;
    bookingId:number;
    isReInspectedService:boolean;
    constructor(){
       // this.isReInspectedService=false;
    }
}

export class ServiceTypeResponse {
    serviceTypeList: Array<ServiceTypeData>;

    result: ServiceTypeResult;
}

export class ServiceTypeData {
    id: number;
    name: string;
    showServiceDateTo: boolean;
}

export enum ServiceTypeResult {
    Success = 1,
    NotFound = 2
}
export class GenericAPIGETRequest {
    requestUrl: string;
    client: number;
    token: string;
    isGenericToken: boolean;
    constructor() {
        this.isGenericToken = true;
        this.client = ClientEnum.TCF;
    }
}

export class GenericAPIPOSTRequest {
    requestUrl: string;
    client: number;
    requestData: any;
    token: string;
    isGenericToken: boolean;
    constructor() {
        this.isGenericToken = true;
        this.client = ClientEnum.TCF;
    }
}

export class APIGatewayPUTRequest {
    requestUrl: string
    requestBase: string;
    isGenericToken: boolean;
    client: number;
    token: string;
    constructor() {
        this.isGenericToken = true;
        this.client = ClientEnum.TCF;
    }
}

export enum ClientEnum {
    TCF = 1,
    FullBridge = 2
}

export class GenericFileUploadRequest {
    id:number;
    requestUrl: string;
    client: number;
    token: string;
    files:any;
    constructor() {
        this.client = ClientEnum.TCF;
    }
}
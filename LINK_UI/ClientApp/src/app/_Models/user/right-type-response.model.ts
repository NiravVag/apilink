export class RightTypeResponse {
    rightTypeList: Array<RightType>;
    result: RightTypeResult;
}
export class RightType {
    id: number;
    name: string;
    service: number;
}
export enum RightTypeResult {
    Success = 1,
    NoDataFound = 2
}
export class CustomSampleSize
{
    public Id:number;
    public SampleType:string;
    public SampleSize:string;
}

export enum ResponseResult {
    Success = 1,
    NoDataFound = 2
  }
  
  export class CustomSampleSizeResponse {
    dataSourceList: Array<CustomSampleSize>;
    result: ResponseResult;
  }
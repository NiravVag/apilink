import { FieldType } from "./template.column.model";

export interface KpiFilterListResponse
    {
      data: Array<KpiFilterItem>;
      result: KpiFilterListResult;
    }

    export interface KpiFilterItem
    {
      id: number;
      fieldLabel: string;
      idSubModule: number | null;
      idModule: number | null; 
      required: boolean;
      isMultiple: boolean;
      type: FieldType;
      fieldName: string; 
    }


    export enum KpiFilterListResult
    {
        Success = 1, 
        NoFound = 2
    }

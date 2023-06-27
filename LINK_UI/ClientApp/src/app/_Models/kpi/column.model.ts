import { FieldType } from "./template.column.model";

    export interface KpiColumnListResponse
    {
      data: Array<KpiColumnItem>;
      result: KpiColumnListResult;
    }

    export  interface KpiColumnItem
    {
      id: number; 
      fieldLabel: string; 
      idSubModule: number | null;
      idModule: number | null;
      type: FieldType;
      fieldName: string; 
    }

    export enum KpiColumnListResult
    {
        Success = 1,
        NotFound = 2
    }

   export interface KpiTemplateColumnListResponse
    {
        data : Array<TemplateColumn>;
        result: KpiTemplateColumnListResult;
    }

    export interface TemplateColumn extends  Field
    {
      sumFooter: boolean;
      group: boolean;
      valuecolumn: string; 
    }

    export enum KpiTemplateColumnListResult
    {
        Success = 1,
        NotFound = 2
    } 

    export abstract class Field {
      id: number;
      idColumn: number;
      columnName: string;
      type: FieldType;
      originalLabel: string;
      idSubModule: number | null;
      idModule: number | null;
      fieldName: string; 
    }

    export enum FieldType {
      String = 1,
      Number = 2,
      Date = 3,
      DateTime = 3
    }

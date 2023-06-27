import { Field } from "./template.column.model";

    export interface KpiTemplateFilterListResponse
    {
      data : Array<TemplateFilter>;
      result: KpiTemplateFilterListResult;
    }

    export interface TemplateFilter extends Field
    {
      isMultiple: boolean; 
      required: boolean;
      selectMultiple: boolean;
      filterLazy: boolean; 
    }

    export enum KpiTemplateFilterListResult
    {
        Success  = 1, 
        NotFound = 2
    }

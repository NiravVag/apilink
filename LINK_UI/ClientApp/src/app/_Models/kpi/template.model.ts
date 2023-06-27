import { summaryModel } from "../summary.model";
import { ModuleItem } from "./module.model";
import { TemplateFilter } from "./template.filter.model";
import { TemplateColumn } from "./template.column.model";

    export interface KpiTemplateListResponse
    {
      data: Array<KpiTemplateItem>;
      result: KpiTemplateListResult;
      totalCount: number; 
      index: number;
      pageSize: number; 
      pageCount: number;
    }

    export interface KpiTemplateListRequest {
      idSubModule: number;
      idModule: number; 
      index: number; 
      pageSize: number; 
    }

    export interface KpiTemplateItem
    {
      id: number; 
      name: string;
      idSubModuleList: Array<number>;
      idModule: number; 
      subModuleName: string;
      userName: string;
      shared: boolean;
      useXlsFormulas: boolean;
      canSave: boolean;
    }

    export enum KpiTemplateListResult
    {
        Success = 1, 
        NotFound = 2 
    }

  export class TemplateSummaryModel extends summaryModel {
    public submoule: ModuleItem;
    public module: ModuleItem; 
  }

export class TemplateModel {
  public id: number;
  public name: string;
  public submoduleList: Array<ModuleItem>;
  public module: ModuleItem;
  public templateFilterList: Array<TemplateFilter>; 
  public templateColumnList: Array<TemplateColumn>;
  public user: string;
  public shared: boolean;
  public useXlsFormulas: boolean;
  public canSave: boolean;
}

export interface KpiTemplateItemResponse {
  item: KpiTemplateItem;
  result: KpiTemplateListResult
}

export interface KpiSavetemplateResponse {
  id: number;
  result: KpiSavetemplateResult;
}

export enum KpiSavetemplateResult {
  Success = 1,
  cannotSave = 2,
  UnAuthorized = 3,
  ModuleIsrequired = 4,
  SubModuleIsrequired = 5,
  TemplateNameIsRequired = 6,
  TemplateColumnsRequired = 7
}


export interface DeleteTemplateResponse {
  id: number;
  result: DeleteTemplateResult;
}

export enum DeleteTemplateResult {
  Success = 1,
  NotFound = 2
}

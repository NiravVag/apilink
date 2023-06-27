import { FieldType } from "./template.column.model";

export interface KpiDataSourceResponse {
  dataSource: Array<any>;
  dataSourceFieldName: string;
  dataSourceFieldValue: string;
  result: DataSourceResult;
}

export enum DataSourceResult {
  Success = 1,
  NotFound = 2,
  FilterNotFound = 3
}


export interface ViewDataResponse {
  rows: Array<HtmlRow>;
  result: ViewDataResult;
}

export interface HtmlRow {
  cells: Array<HtmlCell>;
  isSum: boolean;
}

export interface HtmlCell {
  rowSpan : number;
  value: string; 
  type: FieldType;
  colSpan: number;
}

export enum ViewDataResult {
  Success = 1,
  NotFound = 2,
  TemplateNotFound = 3
}


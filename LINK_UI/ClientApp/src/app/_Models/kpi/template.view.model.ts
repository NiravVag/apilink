import { TemplateColumn } from "./template.column.model";
import { TemplateFilter } from "./template.filter.model";
import { Observable, Subject } from "rxjs";

    export interface KpiTemplateViewResponse
    {
      result: KpiTemplateViewResult;
      item: KpiTemplateView;
    }

    export interface KpiTemplateView
    {
      idTemplate: number; 
      templateName: string; 
      module: string;
      filterList: Array<TemplateFilter>;
      columnList: Array<TemplateColumn>;
    }

    export enum KpiTemplateViewResult
    {
        Success = 1, 
        NotFound = 2 
    }

export interface KpiTemplateViewRequest {
  idTemplate: number;
  filterValues: Array<FilterRequest>;
}

export interface InternalBusinessModel {
  idCustomer: number;
  beginDate: any;
  endDate: any;
  loadDepartment: boolean;
  loadFactory: boolean;
  customer: any;
  brandList: Array<number>;
  deptList: Array<number>; 
  searchDateTypeId: number;
}

export interface FilterRequest {
  id: number;
  value: any;
}

export interface FilterWithDataSource {
  filter: TemplateFilter;
  dataSource: Array<any>;
  dataSourceFieldName: string;
  dataSourceFieldValue: string;
  loading: boolean;
  value: any;
  dataLazy$: Observable<any[]>;
  dataInput$: Subject<string>;
}

export interface EventProgress {
  percent: number; 
  message: string; 
}

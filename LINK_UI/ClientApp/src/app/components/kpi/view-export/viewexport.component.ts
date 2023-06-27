
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { first, retry, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { TemplateSummaryModel, KpiTemplateListResponse, KpiTemplateListResult, KpiTemplateListRequest } from '../../../_Models/kpi/template.model';
import { ModuleItem, ModuleListResponse, ModuleListResult } from '../../../_Models/kpi/module.model';
import { KpiService } from '../../../_Services/kpi/kpi.service';
import { DetailComponent } from '../../common/detail.component';
import { KpiTemplateViewResult, KpiTemplateViewResponse, KpiTemplateView, FilterWithDataSource, KpiTemplateViewRequest, FilterRequest } from '../../../_Models/kpi/template.view.model';
import { KpiDataSourceResponse, DataSourceResult, HtmlRow, ViewDataResponse, ViewDataResult } from '../../../_Models/kpi/datasource.model';
import { TemplateFilter } from '../../../_Models/kpi/template.filter.model';
import { FieldType, TemplateColumn } from '../../../_Models/kpi/template.column.model';
import { Subject, concat, of } from 'rxjs';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-ViewExport',
  templateUrl: './viewexport.component.html',
  styleUrls: ['./viewexport.component.css']
})

export class ViewExportComponent extends DetailComponent {

  public model: KpiTemplateView;
  public searchloading: boolean = false;
  public error: string = "";
  public modelRef: NgbModalRef;
  public templateLoading: boolean = false;
  public exportDataLoading: boolean = false;

  private currentRoute: Router;
  private _router: Router;
  public filters: Array<FilterWithDataSource> = [];
  public rows: Array<Array<FilterWithDataSource>> = [];
  public colFiltersNumbers: number;
  public items: Array<HtmlRow> = [];
  public noFound: boolean = false;
  private table: any = {};
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public utility: UtilityService,
    private service: KpiService) {
    super(router, route, translate, toastr);

    //this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
   // this.validator.isDebug = true; 
    //this._translate = translate;
    //this._toastr = toastr;
  }

  onInit(id?: any) {
    this.init(id);
  }

  getViewPath(): string {
    return "kpi/view-export";
  }

  getEditPath(): string {
    return "";
  }

  public getAddPath(): string {
    return "";
  }

  async init(id) {

    console.log("id:" + id);
    this.templateLoading = true;

    this.model = {
      columnList: [],
      filterList: [],
      idTemplate: id,
      module: "",
      templateName : ""
    };

    this.validator.isSubmitted = false;

    await this.getTemplateView(id);
  }

  initValidator() {
    this.validator.isSubmitted = false;

    if (this.filters && this.filters.length > 0) {

      

      let data: any = {
        model: "kpi-model.model",
        fields: {
        }
      }

      for (let item of this.filters) {

        let dataReq: Array<any> = [];

        if (item.filter.required) {
          dataReq.push({
            type: "required",
            fieldType: this.getValidatorFieldType(item.filter.type, item.filter.isMultiple),
            ressource: item.filter.columnName + " is required"
          });
        }

        if (dataReq.length > 0)
          data.fields[item.filter.columnName] = dataReq;
      }

      console.log(data);
      console.log(data);
      this.validator.setJSONObject(data);
      this.validator.setModelAsync(() => {
        let model: any = {};
        for (let item of this.filters)
          model[item.filter.columnName] = item.value;

        return model; 
      });
    }
  }

  getValidatorFieldType(type: FieldType, isMultiple: boolean): string {

    if (isMultiple)
      return "A";
    else {
      switch (type) {
        case FieldType.Date:
        case FieldType.DateTime:
          return "D";
        case FieldType.Number:
          return "N";
        case FieldType.String:
        default:
          return "T";
      }
    }
  }


  async getTemplateView(id : number) {

    this.templateLoading = true;
    let response: KpiTemplateViewResponse;

    try {
      response = await this.service.GetViewTemplate(id);
      console.log(response);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.templateLoading = false;

    if (response && response.result == KpiTemplateViewResult.Success) {
      this.model = response.item;

      for (var i = 0; i < this.model.filterList.length; i++) {
        let item: TemplateFilter = this.model.filterList[i];

        let filter: FilterWithDataSource = {
          filter: item,
          loading: false,
          dataSource: null,
          dataSourceFieldName: "",
          dataSourceFieldValue: "",
          value: null,
          dataInput$: new Subject<string>(),
          dataLazy$ : null
        }
        this.filters.push(filter);

        if (item.isMultiple)       
          this.getDataSource(filter);
        

        let index = Math.floor(i / 4);
        if (this.rows.length > index)
          this.rows[index].push(filter);
        else {
          let row: Array<FilterWithDataSource> = [filter];
          this.rows.push(row);
        }
      }

      this.initValidator();
    }    
  }


  async getDataSource(filter: FilterWithDataSource) {

    let response: KpiDataSourceResponse;
    filter.loading = true; 

    try {
      response = await this.service.GetDataSource(filter.filter.id);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    filter.loading = false;

    if (response && response.result == DataSourceResult.Success) {
        filter.dataSource = response.dataSource,
        filter.dataSourceFieldName = response.dataSourceFieldName,
        filter.dataSourceFieldValue = response.dataSourceFieldValue  
    };

    if (filter.filter.filterLazy)
      this.loadLazyData(filter);
  }
   

  async search() {
    this.validator.isSubmitted = true;

    if (this.validator.isFormValid()) {
      this.searchloading = true;
      this.noFound = false;
      this.items = [];
      let response: ViewDataResponse;
      try {
        let request: KpiTemplateViewRequest = {
          idTemplate: this.model.idTemplate,
          filterValues: this.filters.map<FilterRequest>(x => {

            let value: string;

            if (x.filter.isMultiple && x.filter.selectMultiple)
              value = x.value == null ? null : x.value.map(y => y[x.dataSourceFieldValue]);
            else if (x.filter.isMultiple)
              value = x.value == null ? null : x.value[x.dataSourceFieldValue];
            else
              value = x.value;

            return {
              id: x.filter.id,
              value: value
            }
          })
        };

        console.log(request);

        response = await this.service.ViewResult(request);
      }
      catch (e) {
        console.error(e);
        this.setError(e);
      }

      this.searchloading = false;

      console.log(response);
      if (response) {
        if (response.result == ViewDataResult.Success) {
          this.items = response.rows;
          //this.table = this.createTableData();
        }
        else if (response.result == ViewDataResult.NotFound) {
          this.noFound = true;
        }
        else {
          console.error(ViewDataResult[response.result]);
        }
      }
    }
  }

  export() {
    this.exportDataLoading = true;

    let request: KpiTemplateViewRequest = {
      idTemplate: this.model.idTemplate,
      filterValues: this.filters.map<FilterRequest>(x => {

        let value: string;

        if (x.filter.isMultiple && x.filter.selectMultiple)
          value = x.value == null ? null : x.value.map(y => y[x.dataSourceFieldValue]);
        else if (x.filter.isMultiple)
          value = x.value == null ? null : x.value[x.dataSourceFieldValue];
        else
          value = x.value;

        return {
          id: x.filter.id,
          value: value
        }
      })
    };

    this.service.export(request)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });

  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "kpi.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "kpi.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }

   createTableData(): any{
     console.log("creating table");

    let table: any = {    
     };

     let positions: Array<number> = []; 
     let counts: Array<number> = []; 
     let rows: Array<any> = this.createNewRows(this.items, positions, 0, counts);

     table.tr = rows;

    console.log(table);

    return table; 
  }

  //createRows(items: Array<any>, pos: number): Array<any> {

  //  let column: TemplateColumn = this.model.columnList[pos];

  //  let index: number = 0;
  //  let trList: Array<any> = [];

  //  for (let item of items) {

  //    if (column.group) {
  //      index = 0;
  //      trList = trList.concat(this.createRows(item[Object.keys(item)[0]], pos + 1));
  //    }
  //    else {
  //      let tr: any = { td: [] };

  //      console.log("creatin tr");

  //      if (index == 0) {
  //        for (let i = 0; i < pos; i++) {
  //          let rowspan: number = 2; //this.getRowSpan(i);
  //          let value: any = item[this.model.columnList[i].fieldName];
  //          tr.td.push({ rowspan: rowspan, value: value });
  //        }
  //      }



  //      for (let i = pos; i < this.model.columnList.length; i++) {
  //        let value: any = item[this.model.columnList[i].fieldName];
  //        tr.td.push({ value: value });
  //      }
  //      trList.push(tr);

  //      index++;
  //    }
      
     
  //  }

  //  return trList
  //}

  createNewRows(items: Array<any>, positions: Array<number>, pos: number, counts: Array<number>): Array<any> {

    let arr: Array<any> = [];

    for (let item of items) {

      if (Array.isArray(item[Object.keys(item)[0]])) {

        if (positions.length > pos)
        {
          for (let i: number = pos; i < positions.length; i++)
            positions[i] = 0;
        }
        else
          positions.push(0);

        let count: number = this.getCount(item[Object.keys(item)[0]]);

        if (counts.length > pos) {
          for (let i: number = pos; i < counts.length; i++)
            counts[i] = count;
        }
        else
          counts.push(count);

        console.log("item count : _________________");
        console.log("item");
        console.log(item);
        console.log("count:" + count);

        arr = arr.concat(this.createNewRows(item[Object.keys(item)[0]], positions, (pos+1), counts));
      }
      else {

        let tr: any =  { td: [] };

        for (let i: number = 0; i < this.model.columnList.length; i++) {
          let column: TemplateColumn = this.model.columnList[i];


          if (positions.length > i) {
            let posIndex = positions[i];


            if (posIndex == 0) {
              tr.td.push({ rowspan:  counts[i], value: item[column.fieldName] });
            }

            posIndex++;
            positions[i] = posIndex; 

          }
          else {
            tr.td.push({ value: item[column.fieldName] });
          }

        }

        arr.push(tr);
      }     
    }

    return arr;
  }

  getRowSpan(items : Array<any>, pos: number): number {

    //let items = this.items;

    for (let i = 0; i <= pos; i++) {

      let item = items[0];

      items = item[Object.keys(item)[0]];
    }

    return this.getCount(items);
  }

  getCount(items: Array<any>) : number {

    let count: number = 0;

    for (let item of items) {
      if (Array.isArray(item[Object.keys(item)[0]]))
        count  = count + this.getCount(item[Object.keys(item)[0]]);
      else
        count =  items.length ; 
    }

    return count; 

  }

  private loadLazyData(item: FilterWithDataSource) {

    item.dataLazy$ = concat(
      of([]), // default items
      item.dataInput$.pipe(
        distinctUntilChanged(),
        tap(() => item.loading = true),
        switchMap(term => this.service.getLazyDataSource(term, item.filter.id, item.dataSourceFieldName).pipe(
          catchError(() => of([])), // empty list on error
          tap(() => item.loading = false)
        ))
      )
    );
  }
}

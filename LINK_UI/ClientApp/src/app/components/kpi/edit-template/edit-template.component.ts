import { Component, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { TemplateModel, KpiTemplateListResult, KpiTemplateItemResponse, KpiSavetemplateResponse, KpiSavetemplateResult } from '../../../_Models/kpi/template.model';
import { ModuleItem, ModuleListResult, ModuleListResponse } from '../../../_Models/kpi/module.model';
import { KpiService } from '../../../_Services/kpi/kpi.service';
import { KpiColumnItem, KpiColumnListResponse, KpiColumnListResult } from '../../../_Models/kpi/column.model';
import { KpiFilterItem, KpiFilterListResponse, KpiFilterListResult } from '../../../_Models/kpi/filter.model';
import { KpiTemplateColumnListResponse, KpiTemplateColumnListResult, TemplateColumn, FieldType } from '../../../_Models/kpi/template.column.model';
import { KpiTemplateFilterListResponse, KpiTemplateFilterListResult, TemplateFilter } from '../../../_Models/kpi/template.filter.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { UserModel } from '../../../_Models/user/user.model';
import { DndDropEvent } from 'ngx-drag-drop';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-editTemplate',
  templateUrl: './edit-template.component.html',
  styleUrls: ['./edit-template.component.css']
})
export class EditTemplateComponent extends DetailComponent {
@ViewChild('myDiv') myDiv: ElementRef;
 @ViewChild('myDivFilter') myDivFilter: ElementRef;
  public model: TemplateModel;
  public data: any;

  private jsonHelper: JsonHelper;

  private _translate: TranslateService;
  private _toastr: ToastrService;
  public _saveloader: boolean = false;
  public moduleloading: boolean = false;
  public submoduleloading: boolean = false;
  public columnsloading: boolean = false;
  public filtersloading: boolean = false;
  public templateColumnsloading: boolean = false;
  public templateFiltersloading: boolean = false;

  public moduleList: Array<ModuleItem>;
  public subModuleList: Array<ModuleItem>;
  public filterList: Array<KpiFilterItem>;
  public columnList: Array<KpiColumnItem>;

  //private currentIdSubModule: number = 0;
  private currentUser: UserModel;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    authserve: AuthenticationService,
    public utility: UtilityService,
    private service: KpiService) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("kpi/edit-template.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    this._translate = translate;
    this._toastr = toastr;
    this.currentUser = authserve.getCurrentUser();
  }

  onInit(id?: any) {
    this.init(id);
  }

  getViewPath(): string {
    return "kpi/edit-template";
  }

  getEditPath(): string {
    return "kpi/edit-template";
  }

  public getAddPath(): string {
    return "kpi/register-template";
  }

  async getTemplate(id : number) {
    this.loading = true;

    let response: KpiTemplateItemResponse;

    try {
      response = await this.service.GetTemplate(id);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.loading = false;

    if (response && response.result == KpiTemplateListResult.Success) {
      this.model.id = response.item.id;
      this.model.name = response.item.name;
      this.model.user = response.item.userName;
      //this.currentIdSubModule = response.item.idSubModule;
      this.model.shared = response.item.shared;
      this.model.useXlsFormulas = response.item.useXlsFormulas
      this.model.canSave = response.item.canSave;

      await this.getSubModules(response.item.idModule);
      this.model.submoduleList = this.subModuleList.filter(x => response.item.idSubModuleList.indexOf(x.id) >= 0);

      await this.getTemplateColumns(response.item.id);
      await this.getTemplateFilters(response.item.id);

      if (this.moduleList && this.moduleList.some(x => x.id == response.item.idModule))
        this.model.module = this.moduleList.filter(x => x.id == response.item.idModule)[0];

      this.getColumns(response.item.idModule, false);
      this.getFilters(response.item.idModule, false);

      if(response.item.idSubModuleList)
        for (let idsubModule of response.item.idSubModuleList) {
          this.getColumns(idsubModule, true);
          this.getFilters(idsubModule, true);
        }

    }
  }

  async getTemplateColumns(id: number) {
    this.templateColumnsloading = true;

    let response: KpiTemplateColumnListResponse;

    try {
      response = await this.service.GetTemplateColumnList(id);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.templateColumnsloading = false;

    if (response && response.result == KpiTemplateColumnListResult.Success) {
      this.model.templateColumnList = response.data;
    }

  }

  async getTemplateFilters(id: number) {
    this.templateFiltersloading = true;

    let response: KpiTemplateFilterListResponse;

    try {
      response = await this.service.GetTemplateFilterList(id);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.templateFiltersloading = false;

    if (response && response.result == KpiTemplateFilterListResult.Success) {
      this.model.templateFilterList = response.data;
    }
  }

  async getModules() {
    this.moduleloading = true;
    let response: ModuleListResponse;

    try {
      response = await this.service.getModuleList();
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.moduleloading = false;

    if (response) {
      switch (response.result) {
        case ModuleListResult.Success:
          this.moduleList = response.data;
          break;
        case ModuleListResult.NotFound:
          break;
      }

    }
  }

  async getSubModules(idmodule : number) {
    this.submoduleloading = true;

    let response: ModuleListResponse;

    try {
      response = await this.service.GetSubModuleList(idmodule);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.submoduleloading = false;

    if (response && response.result == ModuleListResult.Success) {
      this.subModuleList = response.data;
    }
  }

  async getFilters(id: number, isSubModule : boolean) {
    this.filtersloading = true;

    if (!this.filterList)
      this.filterList = [];

    let response: KpiFilterListResponse;

    try {
      if (isSubModule)
        response = await this.service.GetFilterList(id);
      else
        response = await this.service.GetFilterListByModule(id);

    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.filtersloading = false;

    if (response && response.result == KpiFilterListResult.Success) {
      this.filterList = this.filterList.concat(response.data.filter(x => !this.model.templateFilterList.some(y => y.idColumn == x.id)));

      if (this.filterList.some(x => x.required)) {
        const tmp = [...this.filterList];

        for (let item of tmp.filter(x => x.required)) {
          const index: number = this.filterList.findIndex(x => x.id == item.id);

          if (index >= 0) {
            this.filterList.splice(index, 1);

            this.model.templateFilterList.push({
              id: 0,
              columnName: item.fieldLabel,
              isMultiple: item.isMultiple,
              required: item.required,
              idColumn: item.id,
              type: item.type,
              originalLabel: item.fieldLabel,
              idSubModule: item.idSubModule,
              fieldName: item.fieldName,
              selectMultiple: false,
              idModule: item.idModule,
              filterLazy : false
            });
          }
        }
      }

    }
  }

  async getColumns(id: number, isSubModule : boolean) {
    this.columnsloading = true;

    if (!this.columnList)
      this.columnList = [];

    let response: KpiColumnListResponse;

    try {
      if (isSubModule)
        response = await this.service.GetColumnList(id);
      else
        response = await this.service.GetColumnListByModule(id);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.columnsloading = false;

    if (response && response.result == KpiColumnListResult.Success) {
      this.columnList = this.columnList.concat(response.data.filter(x => !this.model.templateColumnList.some(y => y.idColumn == x.id)));
    }
  }

  async changeModule(module: ModuleItem) {

    this.model.templateColumnList = [];
    this.model.templateFilterList = [];
    this.columnList = [];
    this.filterList = [];
    this.model.submoduleList = [];

    if (module) {      
      await this.getSubModules(module.id);

      this.getColumns(module.id, false);
      this.getFilters(module.id, false);
    }

  }

  changeSubModule(submoduleList: Array<ModuleItem>) {

    //if (this.currentIdSubModule == 0) {

    //}
    console.log(this.model.templateColumnList );

    this.model.templateColumnList = this.model.templateColumnList.filter(x => x.idModule!= null ||  submoduleList.some(y => y.id == x.idSubModule));
    this.model.templateFilterList = this.model.templateFilterList.filter(x => x.idModule != null || submoduleList.some(y => y.id == x.idSubModule));
    this.columnList = this.columnList.filter(x => x.idModule != null || submoduleList.some(y => y.id == x.idSubModule));
    this.filterList = this.filterList.filter(x => x.idModule != null || submoduleList.some(y => y.id == x.idSubModule));
    ////this.currentIdSubModule = 0;

    if (submoduleList && submoduleList.length > 0) {
      for (let item of submoduleList.filter(x => !this.columnList.some(y => y.idSubModule == x.id))) {
        this.getColumns(item.id, true);
        this.getFilters(item.id, true);
      }
    }
  }

  async init(id?) {

    this.columnList = [];
    this.filterList = [];
    console.log("id:" + id);
    this.loading = true;
    this.model = new TemplateModel();
    this.data = {};
    this.validator.isSubmitted = false;

    this.getModules();

    if (id)
      this.getTemplate(id);
    else {
      this.model.user = this.currentUser.fullName;
      this.model.canSave = true; 
    }
  }

  async save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    
    if (this.validator.isFormValid()) {
      this._saveloader = true;

      let response: KpiSavetemplateResponse;

      try {
        response = await this.service.saveTemplate(this.model);
      }
      catch (e) {
        console.error(e);
        this.showError('EDIT_TEMPLATE.TITLE', 'EDIT_TEMPLATE.MSG_UNKNOWN');
        this._saveloader = false;
      }

      if (response) {
        switch (response.result) {
          case KpiSavetemplateResult.Success:
            this.showSuccess('EDIT_TEMPLATE.TITLE', 'EDIT_TEMPLATE.MSG_SAVED');
            this.init(response.id);
            break;
          default :
            this.showError('EDIT_TEMPLATE.TITLE', 'EDIT_TEMPLATE.MSG_SAVED_ERROR');
            break;
        }

        this._saveloader = false;
      }
    }
  }
  onDragStart(event: DragEvent, item: KpiColumnItem) {
    this.myDiv.nativeElement.innerHTML = item.fieldLabel
  }

  onDragEnd(event: DragEvent, item: KpiColumnItem) {
    this.myDiv.nativeElement.innerHTML = "select (+)";
  }

  onDragover(event: DragEvent) {

  }

  onDrop(event: DndDropEvent) {
    let index: number;

      for (var i = 0; i < this.columnList.length; i++)
        if (this.columnList[i].id == event.data.id) {
          index = i;
          break;
        }

      if (index >= 0)
        this.columnList.splice(index, 1);

      if (event.data) {
        let item: TemplateColumn = {
          id: 0,
          columnName: event.data.fieldLabel,
          group: false,
          sumFooter: false,
          idColumn: event.data.id,
          type: event.data.type,
          originalLabel: event.data.fieldLabel,
          idSubModule: event.data.idSubModule,
          fieldName: event.data.fieldName,
          idModule: event.data.idModule,
          valuecolumn : ''
        };

        if (this.model.templateColumnList == null)
          this.model.templateColumnList = [];

        this.model.templateColumnList.push(item);
    }

  }

  remove(index: number) {

    let item: TemplateColumn = this.model.templateColumnList[index];

    if (index >= 0)
      this.model.templateColumnList.splice(index, 1);

    let column: KpiColumnItem = {
      id: item.idColumn,
      fieldLabel: item.originalLabel,
      idSubModule: item.idSubModule,
      type: item.type,
      fieldName: item.fieldName,
      idModule : item.idModule
    };
    this.columnList.push(column);
  }

  sortUp(index : number) {
    let item: TemplateColumn = this.model.templateColumnList[index];
    let itemUp: TemplateColumn = this.model.templateColumnList[index-1];
    this.model.templateColumnList[index] = itemUp;
    this.model.templateColumnList[index - 1] = item;
  }

  sortDown(index : number) {
    let item: TemplateColumn = this.model.templateColumnList[index];
    let itemDown: TemplateColumn = this.model.templateColumnList[index + 1];
    this.model.templateColumnList[index] = itemDown;
    this.model.templateColumnList[index + 1] = item;
  }

  getLabel(type: FieldType) {
    return FieldType[type];
  }

  onDragStartFilter(event: DragEvent, item: KpiFilterItem) {
    this.myDivFilter.nativeElement.innerHTML = item.fieldLabel
  }

  onDragEndFilter(event: DragEvent, item: KpiFilterItem) {
    this.myDivFilter.nativeElement.innerHTML = "select (+)";
  }

  onDragoverFilter(event: DragEvent) {

  }

  onDropFilter(event: DndDropEvent) {
    let index: number;

    for (var i = 0; i < this.filterList.length; i++)
      if (this.filterList[i].id == event.data.id) {
        index = i;
        break;
      }

    if (index >= 0)
      this.filterList.splice(index, 1);


    if (event.data) {
      let item: TemplateFilter = {
        id: 0,
        columnName: event.data.fieldLabel,
        isMultiple: event.data.isMultiple,
        required: event.data.required,
        idColumn: event.data.id,
        type: event.data.type,
        originalLabel: event.data.fieldLabel,
        idSubModule: event.data.idSubModule,
        fieldName: event.data.fieldName,
        selectMultiple: false,
        idModule: event.data.idModule,
        filterLazy : false
      };

      if (this.model.templateFilterList == null)
        this.model.templateFilterList = [];

      this.model.templateFilterList.push(item);

      console.log(event.data);
    }

  }

  removeFilter(index: number) {

    let item: TemplateFilter= this.model.templateFilterList[index];

    if (index >= 0)
      this.model.templateFilterList.splice(index, 1);

    let filter: KpiFilterItem = {
      id: item.idColumn,
      fieldLabel: item.originalLabel,
      idSubModule: item.idSubModule,
      type: item.type,
      isMultiple: item.isMultiple,
      required: item.required,
      fieldName: item.fieldName,
      idModule : item.idModule
    };

    this.filterList.push(filter);
  }

  sortUpFilter(index: number) {
    let item: TemplateFilter = this.model.templateFilterList[index];
    let itemUp: TemplateFilter = this.model.templateFilterList[index - 1];
    this.model.templateFilterList[index] = itemUp;
    this.model.templateFilterList[index - 1] = item;
  }

  sortDownFilter(index: number) {
    let item: TemplateFilter = this.model.templateFilterList[index];
    let itemDown: TemplateFilter = this.model.templateFilterList[index + 1];
    this.model.templateFilterList[index] = itemDown;
    this.model.templateFilterList[index + 1] = item;
  }

  setSum(item: TemplateColumn) {

    if (item.type != FieldType.Number)
      return;

    item.sumFooter = !item.sumFooter;
  }

  setGroup(item: TemplateColumn) {

    if (!item.group && item.sumFooter)
      item.sumFooter = false; 

    if (item.group && this.model.templateColumnList.some(x => x.group && x.idColumn != item.idColumn))
      if (item.type == FieldType.Number)
        item.sumFooter = true;
      else
        this.model.templateColumnList.filter(x => x.idColumn != item.idColumn).map(x => {
          x.group = false;
        });

    if (!item.group) {
      this.model.templateColumnList.map(x => {
        console.log(x);
        if (item.idColumn != x.idColumn) {
          if (x.type == FieldType.Number && !x.group)
            x.sumFooter = true;
          else
            x.group = true;
        }
      })
    }

    item.group = !item.group;
    console.log(this.model.templateColumnList);
  }

  setSelectMultiple(item: TemplateFilter) {
    
  }

  setShared() {
    this.model.shared = !this.model.shared;
  }

  setXlsFormulas() {
    this.model.useXlsFormulas = !this.model.useXlsFormulas;
  }

  getsubModuleById(idsubmodule: number) {
    if (this.subModuleList == null)
      return "";

    if (!this.subModuleList.some(x => x.id == idsubmodule))
      return ""; 
    return this.subModuleList.filter(x => x.id == idsubmodule)[0].name;
  }

  view() {
    let entity: string = this.utility.getEntityName();
    this.router.navigate([`/${entity}/kpi/view-export/${this.model.id}`]);
  }

  addEmpty() {
    let item: TemplateColumn = {
      id: 0,
      columnName: "",
      group: false,
      sumFooter: false,
      idColumn: 0,
      type: FieldType.String,
      originalLabel: "",
      idSubModule: 0,
      fieldName: "",
      idModule: 0,
      valuecolumn  : ''
    };

    if (this.model.templateColumnList == null)
      this.model.templateColumnList = [];

    this.model.templateColumnList.push(item);
  }
}




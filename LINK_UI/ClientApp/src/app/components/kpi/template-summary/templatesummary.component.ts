
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { TemplateSummaryModel, KpiTemplateListResponse, KpiTemplateListResult, KpiTemplateListRequest, DeleteTemplateResponse, DeleteTemplateResult } from '../../../_Models/kpi/template.model';
import { ModuleItem, ModuleListResponse, ModuleListResult } from '../../../_Models/kpi/module.model';
import { KpiService } from '../../../_Services/kpi/kpi.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-template-summary',
  templateUrl: './templatesummary.component.html',
  styleUrls: ['./templatesummary.component.css']
})

export class TemplateSummaryComponent extends SummaryComponent<TemplateSummaryModel> {

  public model: TemplateSummaryModel;
  public moduleList: Array<ModuleItem>;
  public subModuleList: Array<ModuleItem>;
  public searchloading: boolean = false;
  public deleteloading: boolean = false; 
  public error: string = "";
  public modelRef: NgbModalRef;
  moduleloading: boolean = false;
  submoduleloading: boolean = false;
  currentUser: UserModel;
  private currentRoute: Router;
  private _router: Router;
  public modelRemove: any;


  getPathDetails(): string {
    return 'kpi/edit-template';
  }

  getData(): void {
    this.getSearchData();
  }

  constructor(private service: KpiService, public validator: Validator, router: Router,
    route: ActivatedRoute, authserve: AuthenticationService, public pathroute: ActivatedRoute,
    public utility: UtilityService,
    translate: TranslateService, toastr: ToastrService, private modalService: NgbModal) {
    super(router, validator, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this.currentRoute = router;
    this._router = router;
  }
  onInit(): void {
    this.Initialize();
    this.validator.setJSON("kpi/template-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
  }
  async Initialize() {
    this.model = new TemplateSummaryModel();
    this.validator.isSubmitted = false;
    this.model.pageSize = 10;
    this.model.index = 1;
    this.data = [];
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

  ChangeModule(module: ModuleItem) {
    this.model.submoule = null;
   
  
    if (module != null && module.id != null) {
      this.model.module = module;
      this.getSubModuleList(module.id);
    }
  }

  async getSubModuleList(id) {
    if (id) {
      this.submoduleloading = true;

      let response: ModuleListResponse;

      try {
        response = await this.service.GetSubModuleList(id);
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
  }
   
  Reset() {
    this.Initialize();
  }

  async getSearchData() {
    this.searchloading = true;

    let response: KpiTemplateListResponse;
    try {
      let request: KpiTemplateListRequest =  {
        idSubModule: this.model.submoule ? this.model.submoule.id : 0,
        index: this.model.index,
        pageSize: this.model.pageSize,
        idModule : this.model.module.id
      };

      response = await this.service.searchTemplates(request);

    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.searchloading = false;

    if (response) {
      if (response.result == KpiTemplateListResult.Success) {
        this.mapPageProperties(response);
        this.model.items = response.data;
      }
      else if (response.result == KpiTemplateListResult.NotFound) {
        this.model.noFound = true;
      }
      else {
        this.error = response.result;
      }
    }

  }

  getDetails(id) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}`, { paramParent: encodeURI(JSON.stringify(currentItem)) }]);
  }

  getView(id:number) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/kpi/view-export/${id}`, { paramParent: encodeURI(JSON.stringify(currentItem)) }]);
  }
  public showError(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }

  public showSuccess(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.success(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }

  openConfirm(id, name, content) {

    this.modelRemove = {
      id: id,
      name: name
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  async deleteTemplate(modelRemove: any) {
    this.deleteloading = true;

    let response: DeleteTemplateResponse;
    try {

      response = await this.service.delete(modelRemove.id);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.deleteloading = false;

    if (response) {
      if (response.result == DeleteTemplateResult.Success) {
        this.showSuccess("TEMPLATE_SUMMARY.TITLE", "TEMPLATE_SUMMARY.MSG_REMOVE_SUCCESS");
      }
      else if (response.result == DeleteTemplateResult.NotFound) {
        this.showError("TEMPLATE_SUMMARY.TITLE", "TEMPLATE_SUMMARY.MSG_NOTFOUND");
      }
      else {
        this.error = response.result;
      }
    }

    this.modelRef.close();
    this.refresh();
  }

}

import { Component } from '@angular/core';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from "ngx-toastr";
import { Validator } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { DataManagementService } from '../../../_Services/data-management/dm.service';
import { DmModule } from '../../../_Models/data-management/dm.module.model';
import { CustomerService } from '../../../_Services/customer/customer.service';
import { CommonDataSourceRequest, ResponseResult } from '../../../_Models/common/common.model';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { FileContainerList, ListSize, PageSizeCommon, Url } from '../../common/static-data-common';
import { DataManagementItem, DataManagementListRequest, DataManagementListResult, DataManagementRight, DataManagementRightRequest, DataManagementRightResult, HierarchyData, RightRequest, SaveDataManagementRightRequest } from '../../../_Models/data-management/dm.model';
import { UtilityService } from '../../../_Services/common/utility.service';
import { FileStoreService } from '../../../_Services/filestore/filestore.service';
import { SummaryComponent } from '../../common/summary.component';
import { dmFileToRemove, DMSummaryItemModel, DMSummaryModel } from 'src/app/_Models/data-management/dmsummary.model';
import { TreeviewConfig, TreeviewItem } from 'ngx-treeview';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';


@Component({
  selector: 'app-data-management-summary',
  templateUrl: './data-management-summary.component.html',
  styleUrls: ['./data-management-summary.component.scss']
})

export class DataManagementSummary extends SummaryComponent<DMSummaryModel> {
  public error = '';
  private modelRef: NgbModalRef;
  public loading: boolean = true;
  searchloading: boolean = false;
  public isNewItem: boolean = true;
  saveloading: boolean;
  public module: DmModule;
  public customerList: any;
  public customerInput: BehaviorSubject<string>;
  public customerLoading: boolean;
  public model: DMSummaryModel;
  //public dmSummaryModel: DMSummaryModel;
  public requestCustomerModel: CommonDataSourceRequest;
  public componentDestroyed$: Subject<boolean> = new Subject();
  public request: DataManagementListRequest;
  public data: Array<DataManagementItem>;
  public count: number;
  public modelRemove: dmFileToRemove;
  private currentRoute: Router;
  _editpath: string;
  showCustomer: boolean = false;
  public treeviewList: TreeviewItem[];
  public hierarchyList: HierarchyData[];
  config: any;
  buttonClass: any;
  selectedPath: any;
  selectedValue: number;
  disableTreeView: boolean = false;
  items: any;
  itemList: any;
  public rightRequest: RightRequest;
  moduleList: any;
  moduleLoading: boolean = false;
  moduleId: number;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  public paramParent: string;
  private _route: ActivatedRoute;


  brandLoading: boolean;
  brandList: any = [];

  deptLoading: boolean;
  deptList: any = [];

  constructor(private service: DataManagementService, private modalService: NgbModal, public deptService: CustomerDepartmentService,
    router: Router, route: ActivatedRoute, public validator: Validator, translate: TranslateService,
    toastr: ToastrService, private customerService: CustomerService, public utility: UtilityService, private fileStoreService: FileStoreService) {

    super(router, validator, route, translate, toastr);

    this.validator.isSubmitted = false;

    this.validator.setJSON("common/novalidation.valid.json");
    this.validator.setModelAsync(() => this.model);

    this.model = new DMSummaryModel();

    this.requestCustomerModel = new CommonDataSourceRequest();
    this.rightRequest = new RightRequest();
    this.customerInput = new BehaviorSubject<string>("");
    this.data = [];
    this.currentRoute = router;
    this._editpath = "data-management/dmedit";

    this.config = TreeviewConfig.create({
      hasAllCheckBox: false,
      hasCollapseExpand: true,
      hasFilter: false,
      maxHeight: 500
    });
    this.buttonClass = "treeview-trigger";
    this._route = route;
    this.model.noFound = false;
  }

  async onInit() {
    await this.Intitialize();
    this.getCustomerListBySearch();
    this.selectedPageSize = PageSizeCommon[0];
  }

  getPathDetails() {
    return 'data-management/dmedit';
  }

  ngAfterViewInit() {

    if (this.model.moduleName) {
      let trigger = document.querySelector('.treeview-trigger');
      trigger.innerHTML = this.model.moduleName;
    }

  }


  //get the rights data in the tree view structure
  async getRightModules() {
    this.loading = true;
    try {

      const response = await this.service.getRights(this.rightRequest);
      this.loading = false;
      this.treeviewList = [];

      switch (response.result) {
        case DataManagementRightResult.Success: {
          for (let item of response.modules)
            this.treeviewList.push(this.mapToTreeViewItem(item));

          this.items = this.treeviewList;
          this.itemList = this.treeviewList;
          break;
        }

        default:
          console.log(response.result);
      }
    }
    catch (e) {
      console.error(e);
      this.loading = false;
    }
  }

  //map to the tree view item
  mapToTreeViewItem(item: DataManagementRight): TreeviewItem {

    //var _this = this;
    return new TreeviewItem({
      checked: item.hasRight,
      collapsed: true,
      //checked : false,
      text: item.moduleName,
      value: item.idModule,
      children: item.children?.map(x => this.mapToTreeViewItem(x))
    });

  }

  //navigate to dm detail page
  getDetails(id) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditDataManagement + id;
    window.open(editPage);



  }

  //intialize the method
  private async Intitialize() {
    this.loading = true;

    this.getModuleList();
    this.getRightModules();

    this._route.queryParams.subscribe(
      params => {

        if (params && params != null && params['param'] != null) {

          this.model = JSON.parse(decodeURI(params['param']));
          this.selectedValue = this.model.idModule;


          if (this.model.isCustomerRequired)
            this.showCustomer = true;


        }
      }
    );
  }

  async getData() {

    this.loading = true;

    this.service.getDMDataSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {

            this.model.index = response.index;
            this.model.pageSize = response.pageSize;
            this.model.totalCount = response.totalCount;
            this.model.pageCount = response.pageCount;

            this.model.items = response.data.map((x) => {

              var tabItem: DMSummaryItemModel = {
                id: x.id,
                dmDetailId: x.dmDetailId,
                customer: x.customer,
                module: x.module,
                documentName: x.documentName,
                documentType: x.documentType,
                documentSize: x.documentSize,
                description: x.description,
                createdOn: x.createdOn,
                editRight: x.editRight,
                deleteRight: x.deleteRight,
                downloadRight: x.downloadRight,
                documentUrl: x.documentUrl,
                documentId: x.documentId,
                brands: x.brands,
                departments: x.departments
              }

              return tabItem;
            });


          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
          this.loading = false;
        },
        error => {
          this.loading = false;

        });

  }


  public showSuccess(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.success(tradMessage, tradTitle);
  }

  public showError(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle);
  }
  public showWarning(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.warning(tradMessage, tradTitle);
  }


  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.requestCustomerModel.searchText = this.customerInput.getValue();
      this.requestCustomerModel.skip = this.customerList.length;
    }

    this.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.requestCustomerModel).
      // pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.customerList = this.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
        }
        this.customerLoading = false;
      }),
      error => {
        console.log("**** error");
        this.customerLoading = false;
        //this.setError(error);
      };
  }

  getCustomerListBySearch() {
    if (this.model.idCustomer) {
      this.requestCustomerModel.idList.push(this.model.idCustomer);
    }
    else {
      this.requestCustomerModel.idList = null;
    }

    console.log("***** customerInput", this.customerInput)

    this.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.requestCustomerModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.customerLoading = false))
      ))
      .subscribe(data => {
        console.log("***** data", data)
        this.customerList = data;
        this.customerLoading = false;
      });
  }


  onPager(event: any) {
    this.model.index = event;
    this.getData();
  }


  download(item) {
    this.loading = true;
    this.fileStoreService.downloadBlobFile(item.documentId, FileContainerList.DataManagement)
      .subscribe(res => {
        this.downloadFile(res, item.documentType, item.documentName);
      },
        error => {
          this.showError('AUDIT_REPORT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          this.loading = false;
        });
  }

  downloadFile(data, mimetype, filename) {
    const blob = new Blob([data], { type: mimetype });

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, filename);
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = filename
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
    this.loading = false;
  }

  //open the delete dm data confirm
  openDeleteDMDataConfirm(iteminfo, content) {
    this.modelRemove = {
      id: iteminfo.id,
      name: iteminfo.documentName
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  //delete the dm data
  deleteDMData(item: dmFileToRemove) {
    this.service.deleteDMData(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.showSuccess("DataManagement", "File Deleted Successfully");
            // refresh
            this.refresh();
          }
          else {
            this.error = response.result;

            this.loading = false;
            // TODO check error from result
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();

  }

  //get the module list
  getModuleList() {
    this.moduleList = [];
    this.moduleLoading = true;
    this.service.getModuleList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success)
            this.moduleList = response.moduleList;
          else
            this.showError('DATA_MANAGEMENT.LBL_TITLE', 'DATA_MANAGEMENT.LBL_MODULE_NOT_FOUND');
          this.moduleLoading = false;
        },
        error => {
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
          this.moduleLoading = false;
        }
      );
  }

  //select the item in the treeview structure
  select(item: TreeviewItem): void {

    if (!item.children) {
      this.selectedValue = item.value;
      //get the module path in the hierarchy way
      var hierarchyList = this.getModulePath(item.value);
      hierarchyList = hierarchyList.sort((a, b) => a.order < b.order ? 1 : -1);
      this.selectedPath = hierarchyList.map(x => x.moduleName).join(' > ');
      this.model.idModule = this.selectedValue;

      var module = this.moduleList.find(x => x.id == this.model.idModule);
      if (module) {
        this.model.moduleName = module.name;
        this.model.isCustomerRequired = module.needCustomer;
      }

      //show the customer based on the module config
      this.isCustomerRequired();
      let trigger = document.querySelector('.treeview-trigger');
      trigger.innerHTML = String(item.text);
    }
  }

  //show the customer based on the module config
  isCustomerRequired() {
    this.showCustomer = false;
    if (this.model.idModule) {
      var module = this.moduleList.find(x => x.id == this.model.idModule);
      if (module && module.needCustomer)
        this.showCustomer = true;
    }
  }

  //get the module path in hierarchy way
  getModulePath(moduleId) {
    var module = this.moduleList.find(x => x.id == moduleId);
    var orderNo = 1;
    var hierarchyList = new Array<HierarchyData>();
    if (module) {
      var hierarchyData = new HierarchyData();
      hierarchyData.moduleName = module.name;
      hierarchyData.order = orderNo;
      hierarchyList.push(hierarchyData);
      if (module.parentId)
        this.getModuleChildPath(module.parentId, hierarchyList, orderNo);

    }

    return hierarchyList;
  }

  //get the module child path
  getModuleChildPath(parentId, hierarchyList, orderNo) {
    var module = this.moduleList.find(x => x.id == parentId);
    if (module) {
      orderNo = orderNo + 1;
      var hierarchyData = new HierarchyData();
      hierarchyData.moduleName = module.name;
      hierarchyData.order = orderNo;
      hierarchyList.push(hierarchyData);

      this.getModuleChildPath(module.parentId, hierarchyList, orderNo);
    }
  }

  //search the dm details
  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }

  changeCustomer() {
    this.brandList = [];
    this.deptList = [];
    if (this.model.idCustomer) {
      this.getBrandListBySearch();
      this.getDeptListBySearch();
    }
  }

  clearCustomer() {
    this.model.idCustomer = null;
    this.brandList = [];
    this.deptList = [];
    this.getCustomerListBySearch();
  }

  getDeptListBySearch() {

    this.deptService.getCustomerDepartment(this.model.idCustomer)
      .subscribe(data => {
        this.deptList = data.customerDepartmentList;
        this.deptLoading = false;
      },
        err => {
          this.deptLoading = false;
        });
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.brandLoading = true;
    this.customerService.getCustomerBrands(this.model.idCustomer)
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.brandList = res.dataSourceList;
        }
        this.brandLoading = false;
      },
        err => {
          this.brandLoading = false;
        });
  }
}



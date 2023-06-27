import { ResponseResult } from './../../../_Models/common/common.model';
import { MobileViewFilterCount, PageSizeCommon, SearchType, UserRightsList } from './../../common/static-data-common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { TreeviewConfig, TreeviewItem } from 'ngx-treeview';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { DataManagementRight, DataManagementRightResult, RightRequest, SaveDataManagementRightRequest, UserManagementMaster } from 'src/app/_Models/data-management/dm.model';
import { Role, RolesResult } from 'src/app/_Models/user/role.model';
import { DataManagementService } from 'src/app/_Services/data-management/dm.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { JsonHelper, Validator } from '../../common';
import { DetailComponent } from '../../common/detail.component';
import { ListSize } from '../../common/static-data-common';
import { OfficeService } from '../../../_Services/office/office.service';
import { SummaryComponent } from '../../common/summary.component';
import { DMUserManagementSearchType, DMUserSummaryMasterModel, DMUserSummaryModel } from 'src/app/_Models/data-management/dmusermanagementsummary.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { dmFileToRemove } from 'src/app/_Models/data-management/dmsummary.model';
import { ModuleListResult } from 'src/app/_Models/kpi/module.model';
import { animate, state, style, transition, trigger } from '@angular/animations';


@Component({
  selector: 'app-dm-user-management-summary',
  templateUrl: './dm-user-management-summary.component.html',
  styleUrls: ['./dm-user-management-summary.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '0px',
        'padding-bottom': '16px'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding-top': 0,
        'padding-bottom': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class DmUserManagementSummaryComponent extends SummaryComponent<DMUserSummaryModel> {

  masterModel: DMUserSummaryMasterModel = new DMUserSummaryMasterModel();
  public componentDestroyed$: Subject<boolean> = new Subject;
  public searchloading: boolean = false;
  buttonClass = "treeview-trigger";
  config: any;
  items: any;
  public rightRequest: RightRequest;
  private modelRef: NgbModalRef;
  public modelRemove: dmFileToRemove;
  treeviewList = [];
  selectedPageSize;
  pagesizeitems = PageSizeCommon;
  isFilterOpen = true;
  filterDataShown: boolean = false;
  filterCount: number;

  constructor(
    translate: TranslateService,
    private modalService: NgbModal,
    toastr: ToastrService,
    route: ActivatedRoute,
    validator: Validator,
    jsonHelper: JsonHelper,
    router: Router, private referenceService: ReferenceService,
    utility: UtilityService,
    private service: DataManagementService
  ) {
    super(router, validator, route, translate, toastr, utility);
    this.model = new DMUserSummaryModel();
    this.model.searchType = this.masterModel.searchType.Role;
    this.getIsMobile();
    this.config = TreeviewConfig.create({
      hasAllCheckBox: false,
      hasCollapseExpand: true,
      hasFilter: false,
      decoupleChildFromParent: false,
      maxHeight: 500
    });

  }

  onInit(): void {
    this.selectedPageSize = PageSizeCommon[0];
    this.getRoles();
    this.rightRequest = new RightRequest();
    this.model.searchType = DMUserManagementSearchType.Role;
    this.masterModel.searchTypeName = this.masterModel.dmSearchTypeList.find(x => x.id == DMUserManagementSearchType.Role)?.name;

  }
  getData(): void {

    this.masterModel.searchloading = true;

    if (this.isFilterOpen)
      this.isFilterOpen = !this.isFilterOpen;

    this.filterDataShown = this.filterTextShown();
    this.service.getUserRights(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res && res.result == ResponseResult.Success) {
          this.model.items = res.data;
          this.model.noFound = false;
        }
        else {
          this.model.noFound = true;
          this.model.items = [];
        }
        this.mapPageProperties(res)
        this.masterModel.searchloading = false;
      },
        err => {
          this.masterModel.searchloading = false;
        });

  }
  getPathDetails(): string {
    return "datamanagement/dmusermanager";
  }

  private async getRoles() {

    this.masterModel.roleLoading = true;
    try {
      const response = await this.service.getRoles();
      this.masterModel.roleLoading = false;


      switch (response.result) {
        case RolesResult.Success:
          this.masterModel.roles = response.list;
          break;
        case RolesResult.NotFound:
          this.masterModel.roles = [];
          break;
        default:
          console.log(response.result);
      }
    }
    catch (e) {
      console.error(e);
      this.loading = false;
    }
  }

  setModules(searchType) {
    this.model.searchType = searchType.id;
    this.model.items = [];
    if (this.model.searchType == this.masterModel.searchType.Role) {
      this.model.staffId = null;
      this.model.moduleId = null;
    }
    else if (this.model.searchType == this.masterModel.searchType.Staff) {
      if (!this.masterModel.staffs || this.masterModel.staffs.length == 0)
        this.getStaffListBySearch();
      this.model.roleId = null;
      this.model.moduleId = null;
    }
    else if (this.model.searchType == this.masterModel.searchType.Tree) {
      if (!this.items || this.items.length == 0)
        this.getRightModules();
      this.model.roleId = null;
      this.model.staffId = null;
    }
    this.isfilterData();
    this.masterModel.searchTypeName = searchType.name;
    this.masterModel.searchTypeValue = "";
  }

  getStaffListBySearch() {

    this.masterModel.staffRequest = new CommonDataSourceRequest();

    this.masterModel.staffRequest.id = null;

    this.masterModel.staffInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.staffLoading = true),
      switchMap(term => term
        ? this.referenceService.getStaffList(this.masterModel.staffRequest, term)
        : this.referenceService.getStaffList(this.masterModel.staffRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])),
            tap(() => this.masterModel.staffLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.staffs = data;
        this.masterModel.staffLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getStaffData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.masterModel.staffRequest.searchText = this.masterModel.staffInput.getValue();
      this.masterModel.staffRequest.skip = this.masterModel.staffs.length;
    }

    this.masterModel.staffLoading = true;
    this.referenceService.getStaffList(this.masterModel.staffRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(staffData => {

        if (staffData && staffData.length > 0) {
          this.masterModel.staffs = this.masterModel.staffs.concat(staffData);
        }
        if (isDefaultLoad) {
          this.masterModel.staffRequest.skip = 0;
          this.masterModel.staffRequest.take = ListSize;
        }
        this.masterModel.staffLoading = false;
      }),
      error => {
        this.masterModel.staffLoading = false;
        this.setError(error);
      };
  }

  async getRightModules() {
    this.masterModel.moduleLoading = true;
    try {

      const response = await this.service.getRights(this.rightRequest);
      this.masterModel.moduleLoading = false;

      let treeviewList = [];
      switch (response.result) {
        case DataManagementRightResult.Success: {
          for (let item of response.modules)
            treeviewList.push(this.mapToTreeViewItem(item));

          this.items = treeviewList;
          break;
        }
        default:
          console.log(response.result);
      }
    }
    catch (e) {
      console.error(e);
      this.masterModel.moduleLoading = false;
    }
  }

  mapToTreeViewItem(item: DataManagementRight, collapsed = true): TreeviewItem {

    //var _this = this;
    return new TreeviewItem({
      collapsed: collapsed,
      text: item.moduleName,
      value: item.idModule,
      children: item.children?.map(x => this.mapToTreeViewItem(x, collapsed))
    });

  }

  select(item: TreeviewItem): void {
    if (!item.children) {
      this.model.moduleId = item.value;
      let trigger = document.querySelector('.treeview-trigger');
      trigger.innerHTML = String(item.text);
      this.isfilterData();
      this.masterModel.searchTypeValue = item.text;
    }
  }

  formValid(): boolean {
    let isOk = true;
    if (this.model.searchType == DMUserManagementSearchType.Role && this.model.roleId == null) {
      this.showError('DATA_MANAGEMENT.LBL_DATA_MANAGEMENT', 'DATA_MANAGEMENT.MSG_ROLE_REQ');
      isOk = false;
    }
    else if (this.model.searchType == DMUserManagementSearchType.Staff && this.model.staffId == null) {
      this.showError('DATA_MANAGEMENT.LBL_DATA_MANAGEMENT', 'DATA_MANAGEMENT.MSG_STAFF_REQ');
      isOk = false;
    }
    else if (this.model.searchType == DMUserManagementSearchType.Tree && this.model.moduleId == null) {
      this.showError('DATA_MANAGEMENT.LBL_DATA_MANAGEMENT', 'DATA_MANAGEMENT.MSG_MODULE_REQ');
      isOk = false;
    }
    return isOk;
  }

  openDeleteDMDataConfirm(iteminfo, content) {
    this.modelRemove = {
      id: iteminfo.id,
      name: ''
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }
  deleteDMData(modelRemove) {
    this.masterModel.deleteLoading = true;
    this.service.deleteDmUserManagement(modelRemove.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.showSuccess("DataManagement", "Right Deleted Successfully");
            // refresh
            this.refresh();

            this.modelRef.close();
          }
          this.masterModel.deleteLoading = false;
        },
        error => {
          this.error = error;
          this.masterModel.deleteLoading = false;
        });
  }

  openTreeViewPopup(content, dmRoleId) {
    this.treeviewList = [];
    this.service.getDmModulesById(dmRoleId).subscribe(response => {
      switch (response.result) {
        case ModuleListResult.Success:
          for (let item of response.list)
            this.treeviewList.push(this.mapToTreeViewItem(item, false));
          break;
        case ModuleListResult.NotFound:
          this.treeviewList = [];
          break;
        default:
          console.log(response.result);
      }
    });
    this.modalService.open(content, { windowClass: "mdModelWidth", centered: true });
  }

  searchDetails() {
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.model.index = 1;
      this.refresh();
    }
  }


  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
  }

  filterTextShown() {
    var isFilterDataSelected = false;

    if (this.model.roleId || this.model.staffId || this.model.moduleId || (this.model.rightIds && this.model.rightIds.length > 0)) {
      if (!this.isMobile) {
        isFilterDataSelected = true;
      }
      else if (this.isMobile) {
        var count = 0;
        //date add
        count = MobileViewFilterCount + count;
        if (this.model.roleId || this.model.staffId || this.model.moduleId) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.rightIds && this.model.rightIds.length > 0) {
          count = MobileViewFilterCount + count;
        }

        this.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.filterCount = 0;
        this.masterModel.searchTypeName = "";
        this.masterModel.searchTypeValue = "";
        this.masterModel.filterRights = "";
      }

    }

    return isFilterDataSelected;
  }
  changeRole(event) {
    this.isfilterData();
    this.masterModel.searchTypeValue = event.name;
  }
  changeStaff(event) {
    this.isfilterData();
    this.masterModel.searchTypeValue = event.name;
  }

  isfilterData() {
    if (this.model.roleId || this.model.staffId || this.model.moduleId || (this.model.rightIds && this.model.rightIds.length > 0)) {
      this.filterDataShown = true;
    }
    else {
      this.filterDataShown = false;
    }
  }
  Reset() {
    this.masterModel.roles = [];
    this.masterModel.staffs = [];
    this.treeviewList = [];
    this.filterDataShown = false;
    this.model.rightIds = [];
    this.model.roleId = null;
    this.model.staffId = null;
    this.model.moduleId = null;
    this.model.searchType = DMUserManagementSearchType.Role;
    this.model.items = [];
    this.model.pageSize = 10;
    this.model.index = 1;
    this.isFilterOpen = true;
    this.model.noFound = false;
    this.onInit();
  }

  changeRight(event) {
    this.isfilterData();
    if (event.length > 0) {
      let rights = event[0].name;
      if (event.length > 1) {
        rights = rights + " " + (event.length - 1) + "+";
      }
      this.masterModel.filterRights = rights;
    } else {
      this.masterModel.filterRights = ""
    }


  }
}
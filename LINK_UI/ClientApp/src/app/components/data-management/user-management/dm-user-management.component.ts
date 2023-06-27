import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, Pipe } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { SupplierSummaryItemModel } from '../../../_Models/supplier/suppliersummary.model';
import { EditSuplierModel, Address, CustomerContact, Customer } from '../../../_Models/supplier/edit-supplier.model'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Country, FileContainerList, ListSize, SupplierType } from '../../common/static-data-common';
import { DataManagementItem, DataManagementItemResult, DataManagementRight, DataManagementRightResult, SaveDataManagementItemResult, SaveDataManagementRightRequest, UserManagementMaster } from '../../../_Models/data-management/dm.model';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { CommonDataSourceRequest } from '../../../_Models/common/common.model';
import { CustomerService } from '../../../_Services/customer/customer.service';
import { DataManagementService } from '../../../_Services/data-management/dm.service';
import { DmModule, ModuleListResult } from '../../../_Models/data-management/dm.module.model';
import { FileStoreService } from '../../../_Services/filestore/filestore.service';
import { AttachmentFile, FileInfo } from '../../../_Models/fileupload/fileupload';
import { OfficeService } from '../../../_Services/office/office.service';
import { ResponseResult } from '../../../_Models/useraccount/userprofile.model';
import { Role, RolesResult } from '../../../_Models/user/role.model';
import { TreeviewConfig, TreeviewItem } from 'ngx-treeview';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';


@Component({
  selector: 'app-dmUserManagement',
  templateUrl: './dm-user-management.component.html',
  styleUrls: ['./dm-user-management.component.scss']
})
export class DmUserManagement extends DetailComponent {
  public model: Array<DmModule>;
  private jsonHelper: any;
  public validator: Validator;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public _saveloader: boolean = false;
  public officeList: Array<any>;
  public officeListLoading: boolean;
  public roleLoading: boolean;
  public roles: Array<Role>;
  public staffs: Array<any>;
  public employeesLoading: boolean;
  public request: SaveDataManagementRightRequest;
  public treeviewList: TreeviewItem[];
  public config: TreeviewConfig;
  public noItem: boolean;
  public modelRef: NgbModalRef;
  selectedItem: any;
  userManagementMaster: UserManagementMaster;
  public componentDestroyed$: Subject<boolean> = new Subject;
  
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    route: ActivatedRoute,
    jsonHelper: JsonHelper,
    router: Router,
    public modalService: NgbModal, private referenceService: ReferenceService, utility: UtilityService,
    private service: DataManagementService, private customerService: CustomerService, private fileStoreService: FileStoreService, private officeService: OfficeService) {
    super(router, route, translate, toastr, null, utility);
    this._translate = translate;
    this._toastr = toastr;
    this.jsonHelper = jsonHelper;
    this.officeList = [];
    this.officeListLoading = false;
    this.roleLoading = false;
    this.employeesLoading = false;
    this.roles = [];
    this.staffs = [];
    this.request = {
      id: null,
      modules: [],
      rightRequest: {
        uploadRight: false,
        deleteRight: false,
        downloadRight: false,
        editRight: false,
        idOffice: null,
        idRole: null,
        idStaff: null,
      },
      byRole: true,
      byEmployee: false
    };

    this.config = TreeviewConfig.create({
      hasAllCheckBox: false,
      hasFilter: false,
      hasCollapseExpand: true,
      decoupleChildFromParent: false,
      maxHeight: 500
    });

    this.noItem = true;
    this.treeviewList = [];

    this.userManagementMaster = new UserManagementMaster();

  }
  @Input() modelData: any;
  onInit(id?: any) {
    this.init(id);
  }

  getViewPath(): string {
    return "";
  }

  getEditPath(): string {
    return "data-management/edit";
  }

  public getAddPath(): string {
    return "data-management/add";
  }

  async init(id?) {
    this.loading = true;

    this.model = [];

    if (id) {
      let response = await this.service.getEditDmUserManagement(id);
      this.request = response.dmRole;
      this.request.byRole = this.request.rightRequest.idRole != null;
      this.request.byEmployee = this.request.rightRequest.idStaff != null;
      this.request.id = id;    
    }
    await this.getRightModules();
    await this.getRoles();
    this.getStaffListBySearch();
  }

  getOfficeList() {
    this.officeListLoading = true;

    this.officeService.getOfficeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.officeList = response.dataSourceList;
          this.officeListLoading = false;
        },
        error => {
          this.setError(error);
          this.officeListLoading = false;
        });
  }

  getEmployees(idOffice: number) {
    this.employeesLoading = true;

    this.officeService.getStaffsByOffice(idOffice)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1)
            this.staffs = response.data;
          this.employeesLoading = false;
        },
        error => {
          this.setError(error);
          this.employeesLoading = false;
        });
  }


  private async getRoles() {

    this.roleLoading = true;
    try {
      const response = await this.service.getRoles();
      this.roleLoading = false;


      switch (response.result) {
        case RolesResult.Success:
          this.roles = response.list;
          break;
        case RolesResult.NotFound:
          this.roles = [];
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

  async getRightModules() {
    this.loading = true;

    try {
      const response = await this.service.getMainModuleList();
      this.loading = false;
      this.treeviewList = [];

      switch (response.result) {
        case ModuleListResult.Success:
          this.model = response.list;
          this.noItem = false;
          for (let item of this.model)
            this.treeviewList.push(this.mapToTreeViewItem(item));
          break;
        case ModuleListResult.NotFound:
          this.model = [];
          this.noItem = true;
          this.treeviewList = [];
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

  isDataValid() {
    var isValid = true;
    if (this.request.byRole && !this.request.rightRequest.idRole) {
      isValid = false;
      this.showWarning("DATA_MANAGEMENT.LBL_TITLE", "Please choose the role");
    }
    else if (this.request.byEmployee && !this.request.rightRequest.idStaff) {
      isValid = false;
      this.showWarning("DATA_MANAGEMENT.LBL_TITLE", "Please choose the employee");
    }
    else if (this.request.modules.length == 0) {
      isValid = false;
      this.showWarning("DATA_MANAGEMENT.LBL_TITLE", "DATA_MANAGEMENT.MSG_SELECT_MODULES");
    }
    else if (!this.request.rightRequest.editRight && !this.request.rightRequest.deleteRight && !this.request.rightRequest.downloadRight && !this.request.rightRequest.uploadRight) {
      isValid = false;
      this.showWarning("DATA_MANAGEMENT.LBL_TITLE", "DATA_MANAGEMENT.MSG_SELECT_RIGHTS");
    }
    return isValid;
  }

  save() {
    if (this.isDataValid()) {
      this._saveloader = true;
      this.service.saveRights(this.request).then(response => {
        this._saveloader = false;

        switch (response.result) {

          case DataManagementRightResult.Success:
            this.showSuccess('DATA_MANAGEMENT.SAVE_RESULT', 'DATA_MANAGEMENT.SAVE_OK');
            this.return("datamanagement/dmusermanagersummary");
            break;
          case DataManagementRightResult.RightsAlreadyConfigured:
            this.showError('DATA_MANAGEMENT.SAVE_RESULT', response.alreadyExistModules + " already exists");
            break;
          default:
            this.showError('DATA_MANAGEMENT.SAVE_RESULT', response.result.toString());
            break;
        }
      }).catch(err => {

      });
    }

  }


  cancel() {

  }

  setModules(code: string) {
    this.request.rightRequest.idOffice = null;
    this.request.rightRequest.idRole = null;
    this.request.rightRequest.idStaff = null;
    this.staffs = [];
    if (code == 'role') {
      this.request.byRole = true;
      this.request.byEmployee = false;    
    }
    else if (code == 'employee') {
      this.request.byRole = false;
      this.request.byEmployee = true;      
    }
    this.reset();
  }




  mapToTreeViewItem(item: DmModule): TreeviewItem {

    //var _this = this;
    return new TreeviewItem({
      // checked: item.hasRight,
      checked: this.request?.modules?.includes(item.id),
      text: item.moduleName,
      value: item.id,
      children: item.children?.map(x => this.mapToTreeViewItem(x))
    });

  }

  async setEmployees(item) {
    this.request.rightRequest.idRole = null;
    this.request.rightRequest.idStaff = null;


    this.staffs = [];
    this.reset();
    await this.getEmployees(item.id);

  }

  reset() {

    this.model = [];
    this.request.rightRequest.editRight = false;
    this.request.rightRequest.downloadRight = false;
    this.request.rightRequest.deleteRight = false;
    this.request.rightRequest.uploadRight = false;
  }

  openContextMenu(content, event) {
    event.preventDefault();
    var item = this.selectedItem;
    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: 10000000,
      uploadFileExtensions: 'png,jpg,jpeg,pdf,doc,xlsx',
      uploadLimit: 10,
      containerId: FileContainerList.DataManagement,
      token: "",
      fileDescription: null
    }

    modalRef.componentInstance.fromParent = fileInfo;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {

          // this.attachements.push(element);

        });
      }

    }, (reason) => {

    });
    //this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
  }

  onSelectedChange(event) {
    this.selectedItem = event;
  }

  changeRoles() {
    this.request.rightRequest.idOffice = null;
    this.request.rightRequest.idStaff = null;
  }

  resetRoles() {
    this.request.rightRequest.idRole = null;
  }

  resetOffice() {
    this.request.rightRequest.idOffice = null;
    this.request.rightRequest.idStaff = null;
    this.staffs = [];
  }

  resetEmployees() {
    this.request.rightRequest.idStaff = null;
  }

  getStaffListBySearch() {

    this.userManagementMaster.staffRequest = new CommonDataSourceRequest();

    if (this.request.rightRequest.idStaff) {
      this.userManagementMaster.staffRequest.id = this.request.rightRequest.idStaff;
    } else {
      this.userManagementMaster.staffRequest.id = null;
    }
    

    this.userManagementMaster.staffInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.userManagementMaster.staffLoading = true),
      switchMap(term => term
        ? this.referenceService.getStaffList(this.userManagementMaster.staffRequest, term)
        : this.referenceService.getStaffList(this.userManagementMaster.staffRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])),
            tap(() => this.userManagementMaster.staffLoading = false))
      ))
      .subscribe(data => {
        this.userManagementMaster.staffList = data;
        this.userManagementMaster.staffLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getStaffData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.userManagementMaster.staffRequest.searchText = this.userManagementMaster.staffInput.getValue();
      this.userManagementMaster.staffRequest.skip = this.userManagementMaster.staffList.length;
    }

    this.userManagementMaster.staffLoading = true;
    this.referenceService.getStaffList(this.userManagementMaster.staffRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(staffData => {

        if (staffData && staffData.length > 0) {
          this.userManagementMaster.staffList = this.userManagementMaster.staffList.concat(staffData);
        }
        if (isDefaultLoad) {
          this.userManagementMaster.staffRequest.skip = 0;
          this.userManagementMaster.staffRequest.take = ListSize;
        }
        this.userManagementMaster.staffLoading = false;
      }),
      error => {
        this.userManagementMaster.staffLoading = false;
        this.setError(error);
      };
  }

  clearStaff() {
    this.getStaffListBySearch();
  }
}




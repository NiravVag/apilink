import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Country, FileContainerList, ListSize, SupplierType } from '../../common/static-data-common';
import { DataManagementItem, DataManagementItemResult, DataManagementMaster, DataManagementRight, DataManagementRightResult, fileData, HierarchyData, Position, SaveDataManagementItemResult, SaveDataManagementRightRequest } from '../../../_Models/data-management/dm.model';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest } from '../../../_Models/common/common.model';
import { CustomerService } from '../../../_Services/customer/customer.service';
import { DataManagementService } from '../../../_Services/data-management/dm.service';
import { DmModule, ModuleListResult } from '../../../_Models/data-management/dm.module.model';
import { FileStoreService } from '../../../_Services/filestore/filestore.service';
import { AttachmentFile, FileInfo } from '../../../_Models/fileupload/fileupload';
import { OfficeService } from '../../../_Services/office/office.service';
import { ResponseResult } from '../../../_Models/useraccount/userprofile.model';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { TreeviewConfig, TreeviewItem } from 'ngx-treeview';
import { order } from '@amcharts/amcharts4/.internal/core/utils/Number';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';


@Component({
  selector: 'app-dataManagementEdit',
  templateUrl: './data-management-edit.component.html',
  styleUrls: ['./data-management-edit.component.scss']
})
export class DataManagementEdit extends DetailComponent {
  public model: DataManagementItem;
  private jsonHelper: any;
  public validator: Validator;
  public selectedcustomerList: Array<any>;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public _saveloader: boolean = false;
  public regionloader: boolean = false;
  public cityloader: boolean = false;
  public customerList: any;
  public customerInput: BehaviorSubject<string>;
  public customerLoading: boolean;
  public requestCustomerModel: CommonDataSourceRequest;
  public componentDestroyed$: Subject<boolean> = new Subject();
  public module: DmModule;
  public attachements: Array<AttachmentFile>;
  public uploadFileExtensions: string;
  public uploadLimit: number;
  public fileSize: number;
  public uploadfileimage: string = "assets/images/uploaded-files.svg";
  public SmallUploadTitle = "Upload File";
  public SmallUploadSubTitle = "";
  public officeList: Array<any>;
  public officeListLoading: boolean;
  public jobsLoading: boolean;
  public positionList: Array<Position>;
  editTab1: boolean;
  labelIndex: number = 0;
  labelValues: string[] = new Array("Module", "Service", "Product Category", "FileType")

  moduleList: any;
  serviceList: any;
  productCategoryList: any;
  fileTypeList: any;

  moduleId: number;
  serviceId: number;
  productCategoryId: number;
  fileTypeId: number;

  moduleLoading: boolean = false;
  serviceLoading: boolean = false;
  productCategoryLoading: boolean = false;
  fileTypeLoading: boolean = false;
  showCustomer: boolean = false;

  public treeviewList: TreeviewItem[];
  public hierarchyList: HierarchyData[];

  public request: SaveDataManagementRightRequest;

  config: any;
  buttonClass: any;

  selectedPath: any;
  selectedValue: number;
  disableTreeView: boolean = false;

  items: any;
  itemList: any;

  isEdit: boolean;

  dataManagementMaster: DataManagementMaster;
  isUpload = false;

  brandLoading: boolean;
  brandInput: BehaviorSubject<string>;
  brandList: any;
  brandSearchRequest: CommonCustomerSourceRequest;
  filterDataShown: boolean;

  deptLoading: boolean;
  deptInput: BehaviorSubject<string>;
  deptList: any;
  deptSearchRequest: CommonCustomerSourceRequest;

  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    route: ActivatedRoute,
    jsonHelper: JsonHelper,
    router: Router,
    public brandService: CustomerBrandService,
    public deptService: CustomerDepartmentService,
    public modalService: NgbModal, public locationService: LocationService,
    private service: DataManagementService, private customerService: CustomerService, private fileStoreService: FileStoreService, private officeService: OfficeService, public utility: UtilityService) {
    super(router, route, translate, toastr);
    this._translate = translate;
    this._toastr = toastr;
    this.jsonHelper = jsonHelper;
    this.officeList = [];
    this.officeListLoading = false;
    this.brandInput = new BehaviorSubject<string>("");
    this.deptInput = new BehaviorSubject<string>("");

    this.module = {
      children: [],
      selected: null,
      id: 0,
      moduleName: '',
      needCustomer: false,
      parentId: null,
      ranking: 0
    }

    this.requestCustomerModel = new CommonDataSourceRequest();
    this.customerInput = new BehaviorSubject<string>("");
    this.attachements = [];
    this.uploadFileExtensions = "";
    this.uploadLimit = 10;
    this.fileSize = 50000000;
    this.jobsLoading = false;
    this.positionList = [];
    this.ExpandAllTab();
    this.treeviewList = [];

    this.request = {
      id: null,
      modules: [],
      rightRequest: {
        deleteRight: false,
        uploadRight: false,
        downloadRight: false,
        editRight: false,
        idOffice: null,
        idRole: null,
        idStaff: null
      },
      byRole: true,
      byEmployee: false
    };

    this.config = TreeviewConfig.create({
      hasAllCheckBox: false,
      hasCollapseExpand: true,
      hasFilter: false,
      maxHeight: 500
    });
    this.buttonClass = "treeview-trigger";
    this.dataManagementMaster = new DataManagementMaster();

  }
  @Input() modelData: any;
  onInit(id?: any) {
    this.init(id);
  }

  getViewPath(): string {
    return "";
  }

  getEditPath(): string {
    return "data-management/dmedit";
  }

  public getAddPath(): string {
    return "data-management/dmadd";
  }



  async init(id?) {

    this.loading = true;

    this.model = new DataManagementItem();

    this.model.countryIds = [];

    this.validator = Validator.getValidator(this.model, "data-management/edit.valid.json", this.jsonHelper, false, this._toastr, this._translate);
    this.validator.isSubmitted = false;

    this.getCustomerListBySearch();
    // this.getCustomerBasedDetails(this.model.idCustomer);
    this.getCountryListBySearch();

    this.getUploadRightModules();

    await this.getModuleList();

    await this.getOfficeList();
    await this.getJobs();
    this.brandSearchRequest = new CommonCustomerSourceRequest();
    this.deptSearchRequest = new CommonCustomerSourceRequest();
    this.isEdit = false;
    let trigger = document.querySelector('.treeview-trigger');
    trigger.innerHTML = String("Select File Type");
    if (id) {
      this.isEdit = true;
      await this.getItem(id);
    }
  }

  /* ngAfterViewInit() {

    let trigger = document.querySelector('.treeview-trigger');
    if (!this.isEdit)
      trigger.innerHTML = String("Module Name");
    else
      trigger.innerHTML = String("Select File Type");
  } */

  async getUploadRightModules() {

    this.loading = true;
    try {
      this.request.rightRequest.uploadRight = true;
      const response = await this.service.getRights(this.request.rightRequest);
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
        case DataManagementRightResult.NotFound: {
          this.showError("DATA_MANAGEMENT.LBL_DATA_MANAGEMENT","DATA_MANAGEMENT.MSG_NO_UPLOAD_RIGHTS")
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

  mapToTreeViewItem(item: DataManagementRight): TreeviewItem {

    return new TreeviewItem({
      checked: item.hasRight,
      collapsed: true,
      //checked : false,
      text: item.moduleName,
      value: item.idModule,
      children: item.children?.map(x => this.mapToTreeViewItem(x))
    });

    //var _this = this;


  }

  getChildrenId(childModule) {
    if (!childModule.children)
      return childModule.id;
    else
      this.getChildrenId(childModule.children);
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

  getJobs() {
    this.jobsLoading = true;

    this.officeService.getPositions()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1)
            this.positionList = response.positionList;
          this.jobsLoading = false;
        },
        error => {
          this.setError(error);
          this.jobsLoading = false;
        });
  }


  private async getModules() {

    this.loading = true;
    try {
      const response = await this.service.getMainModuleList();
      this.loading = false;


      switch (response.result) {
        case ModuleListResult.Success:
          this.module = {
            children: response.list,
            selected: null,
            id: 0,
            moduleName: '',
            needCustomer: false,
            parentId: null,
            ranking: 0
          };
          this.clearModule(this.module);
          break;
        case ModuleListResult.NotFound:
          this.module = {
            children: [],
            selected: null,
            id: 0,
            moduleName: '',
            needCustomer: false,
            parentId: null,
            ranking: 0
          }
          break;
        default:
          console.log(response.result.toString());
      }
    }
    catch (e) {
      console.error(e);
      this.loading = false;
    }
  }


  ChangeModule(item: DmModule) {
    console.log("changeModule", item);
    if (item.children)
      for (let child of item.children)
        this.clearModule(child);
  }

  clearModule(item: DmModule) {
    console.log("clearModule", item);
    if (item.children && item.children.length > 0) {
      item.selected = item.children[0].id;
      /* item.titleName = "Module";
      item.labelIndex = this.labelIndex; */
    }
    else
      item.selected = null;

    //this.labelIndex = this.labelIndex + 1;

    if (item.children)
      for (let child of item.children)
        this.clearModule(child);
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
        console.log("**** customerData");
        console.log("**** customerData", customerData);

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
        this.setError(error);
      };
  }

  async getItem(id: number) {

    this.loading = true;
    try {
      const response = await this.service.getDataManagementItem(id);
      this.loading = false;

      switch (response.result) {
        case DataManagementItemResult.Success: {
          this.model = response.item;

          if (this.model.moduleId)
            this.selectedValue = this.model.moduleId;

          var module = this.moduleList.find(x => x.id == this.model.moduleId);

          if (module) {
            let trigger = document.querySelector('.treeview-trigger');
            trigger.innerHTML = module.name;
          }

          var hierarchyList = this.getModulePath(this.model.moduleId);
          hierarchyList = hierarchyList.sort((a, b) => a.order < b.order ? 1 : -1);
          this.selectedPath = hierarchyList.map(x => x.moduleName).join(' > ');


          var fileAttachments = [];

          this.attachements = [];

          this.model.fileAttachments.forEach(attachment => {
            var fileAttachment = new AttachmentFile();
            fileAttachment.id = attachment.id;
            fileAttachment.fileName = attachment.fileName;
            fileAttachment.fileUrl = attachment.fileUrl;
            fileAttachment.uniqueld = attachment.fileId;
            fileAttachment.fileSize = attachment.fileSize;
            fileAttachments.push(fileAttachment);
          });

          this.attachements = fileAttachments;

          this.isCustomerRequired();
          this.getCustomerBasedDetails(this.model.idCustomer);
          this.getCustomerListBySearch();
          break;
        }
        case DataManagementItemResult.NotFound:
          this.model = new DataManagementItem();
          break;
        case DataManagementItemResult.NotAuthorized:
          this.model = new DataManagementItem();
          this.showError('DATA_MANAGEMENT.SAVE_RESULT', 'DATA_MANAGEMENT.NOTAUTHRORIZED');
        default:
          console.log(response.result);
      }
    }
    catch (e) {
      console.error(e);
      this.loading = false;
    }
  }

  getCustomerListBySearch() {
    //push the customerid to  customer id list
    if (this.model.idCustomer) {
      if (this.requestCustomerModel.idList == null)
        this.requestCustomerModel.idList = [];
      this.requestCustomerModel.idList.push(this.model.idCustomer);
    }
    else {
      this.requestCustomerModel.idList = null;
    }
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
        this.customerList = data;
        this.customerLoading = false;
      });
  }



  getSelectedModule(module: DmModule): number {

    console.log("module", module);
    if (module.children == null || module.children.length == 0)
      return module.id;

    const item = module.children.filter(x => x.selected || x.selected == null)[0];

    return this.getSelectedModule(item);
  }

  /*  getSelectedModule() {

      if (this.fileTypeId)
         this.model.idModule = this.fileTypeId;
       else if (this.productCategoryId)
         this.model.idModule = this.productCategoryId;
       else if (this.serviceId)
         this.model.idModule = this.serviceId;
       else if (this.moduleId)
         this.model.idModule = this.moduleId;

   } */

  isDMDataValid() {
    if (!this.model.moduleId) {
      this.showWarning("DATA_MANAGEMENT.LBL_TITLE", "DATA_MANAGEMENT.MSG_SELECT_FILE_TYPE");
      return false;
    }
    if (!this.attachements || this.attachements.length == 0) {
      this.showWarning("DATA_MANAGEMENT.LBL_TITLE", "DATA_MANAGEMENT.MSG_UPLOAD_FILE");
      return false;
    }
    return true;
  }

  reset() {
    this.model = new DataManagementItem();
    let trigger = document.querySelector('.treeview-trigger');
    trigger.innerHTML = String("Select File Type");
    this.attachements = [];
    this.selectedPath = "";
  }

  save() {

    /*     console.log("this.model.idModule ", this.model.moduleId);
        this.validator.initTost();
        this.validator.isSubmitted = true;
        console.log(this.validator.isFormValid());
         */


    if (this.isDMDataValid()) {
      this._saveloader = true;

      if (this.attachements && this.attachements.length > 0) {
        var fileAttachments = [];

        this.attachements.forEach(attachment => {
          var fileAttachment = new fileData();
          fileAttachment.id = attachment.id;
          fileAttachment.fileName = attachment.fileName;
          fileAttachment.fileUrl = attachment.fileUrl;
          fileAttachment.fileId = attachment.uniqueld;
          fileAttachment.fileSize = attachment.fileSize / Math.pow(1024, 2);
          fileAttachments.push(fileAttachment);
        });

        this.model.fileAttachments = fileAttachments;

        this.service.save(this.model).then(response => {
          this._saveloader = false;
          if (response.result == SaveDataManagementItemResult.Success) {
            this.showSuccess('DATA_MANAGEMENT.SAVE_RESULT', 'DATA_MANAGEMENT.SAVE_OK');

            this.reset();

            if (!this.model.id)
              this.return("data-management/dmsummary");
          }
          else {
            this._saveloader = false;
            this.showError('DATA_MANAGEMENT.SAVE_RESULT', response.result.toString());
          }
        }).catch(err => {
          console.error(err);
          this._saveloader = false;
          this.showError('DATA_MANAGEMENT.MSG_UPLOAD_FILE', 'DATA_MANAGEMENT.MSG_UNKNONW_ERROR');
        });
      }

      //var fileAttachments

      /*  this.fileStoreService.uploadFiles(FileContainerList.DataManagement, this.attachements).subscribe(response => {
         if (response && response.fileUploadDataList) {
           this.attachements[0].fileUrl = response.fileUploadDataList[0].fileCloudUri;
           // save  the other data
           this.model.fileName = this.attachements[0].fileName;
           this.model.fileId = this.attachements[0].uniqueld;
           this.service.save(this.model).then(response => {
             this._saveloader = false;
             console.log(" get response");
             if (response.result == SaveDataManagementItemResult.Success) {
               this.showSuccess('DATA_MANAGEMENT.SAVE_RESULT', 'DATA_MANAGEMENT.SAVE_OK');
               this.init(this.model.id);
             }
             else {
               this._saveloader = false;
               this.showError('DATA_MANAGEMENT.SAVE_RESULT', response.result.toString());
             }
           }).catch(err => {
             console.error(err);
             this._saveloader = false;
             this.showError('DATA_MANAGEMENT.MSG_UPLOAD_FILE', 'DATA_MANAGEMENT.MSG_UNKNONW_ERROR');
           });

         }
         else {
           console.log(" file not added");
           this._saveloader = false;
           this.showError('DATA_MANAGEMENT.MSG_UPLOAD_FILE', 'DATA_MANAGEMENT.FILENOTADDED');
         }
       },
         error => {
           this._saveloader = false;
           this.showError('DATA_MANAGEMENT.MSG_UPLOAD_FILE', 'DATA_MANAGEMENT.MSG_UNKNONW_ERROR');
         }); */
    }
  }


  cancel() {

  }

  getChild = (selected: number, data: Array<DmModule>) => data.filter(x => x.id == selected)[0];



  anyNeedCust = (module: DmModule): boolean => {

    if (module == null)
      return false;

    if (module.needCustomer)
      return true;

    if (!module.children)
      return false;

    for (let item of module.children) {

      if (item.selected) {
        const sub = item.children.filter(x => x.id == item.selected)[0];

        if (this.anyNeedCust(sub))
          return true;
      }

    }
    return false;

  }




  selectFiles(event) {
    if (event && !event.error && event.files) {

      for (let file of event.files) {

        let fileItem: AttachmentFile = {
          fileName: file.name,
          file: file,
          fileSize: this.fileSize,
          isNew: true,
          id: 0,
          status: 0,
          mimeType: file.type,
          fileUrl: "",
          uniqueld: this.newGuid(),
          isSelected: false,
          fileDescription: null
        };
        this.attachements.push(fileItem);
      }

      //}



      //event.srcElement.value = null;
    }
    else if (event && event.error && event.errorMessage) {
      this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', event.errorMessage);
    }
  }

  removeAttachment(index) {
    this.attachements.splice(index, 1);
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }


  ExpandAllTab() {
    this.editTab1 = true;
  }

  async getModuleList() {
    this.moduleList = [];
    this.moduleLoading = true;

    let moduleDataResponse = await this.service.getModuleDataList();

    this.moduleLoading = false;

    if (moduleDataResponse && moduleDataResponse.result == DataSourceResult.Success)
      this.moduleList = moduleDataResponse.moduleList;
    else
      this.showError('DATA_MANAGEMENT.LBL_TITLE', 'DATA_MANAGEMENT.LBL_MODULE_NOT_FOUND');

  }


  isCustomerRequired() {
    this.showCustomer = false;
    if (this.model.moduleId) {
      var module = this.moduleList.find(x => x.id == this.model.moduleId);
      if (module && module.needCustomer)
        this.showCustomer = true;
    }

  }

  uploadFileData() {

    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: 50000000,
      uploadFileExtensions: 'png,jpg,jpeg,pdf,doc,docx,xlsx,pptx,ppt,xls',
      uploadLimit: 10,
      containerId: FileContainerList.DataManagement,
      token: "",
      fileDescription: null
    }

    modalRef.componentInstance.fromParent = fileInfo;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {

          this.attachements.push(element);

        });
      }

    }, (reason) => {

    });

  }

  getFile(file) {
    //this.downloadloading = true;
    this.fileStoreService.downloadBlobFile(file.uniqueld, FileContainerList.DataManagement)
      .subscribe(res => {
        this.downloadFile(res, file.mimeType, file.fileName);
      },
        error => {
          //this.downloadloading = false;
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
        });

  }

  downloadFile(data, mimeType, filename) {
    let windowNavigator: any = window.Navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = filename ? filename : "export";//url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
    /* this.downloadloading = false;
    this.exportProductLoading = false; */
  }

  onValueChange(value: number): void {
    console.log('valueChange raised with value: ' + value);
  }

  /**
   * Function to handle select event of treeview
   * @param {TreeviewItem} item
   */
  select(item: TreeviewItem): void {

    if (!item.children) {
      this.selectedValue = item.value;
      var hierarchyList = this.getModulePath(item.value);
      hierarchyList = hierarchyList.sort((a, b) => a.order < b.order ? 1 : -1);
      this.selectedPath = hierarchyList.map(x => x.moduleName).join(' > ');
      this.model.moduleId = this.selectedValue;
      this.isCustomerRequired();

      let trigger = document.querySelector('.treeview-trigger');
      trigger.innerHTML = String(item.text);
    }
  }

  checkUploadRight(moduleId) {
    this.service.isUploadRight(moduleId).subscribe(x => {
    })
  }

  /**
   * Function to get the reverse tree path of the selected element
   * @param {Object} model data set
   * @param {number} value select value
   */
  getPath(model, value) {
    var path,
      item = (item && item.length) ? item.push(model.text) : model.text;

    if (!model || typeof model !== 'object') return;

    if (model.value === value) return [item];

    (model.children || []).some(child => path = this.getPath(child, value));
    return path && [item, ...path];

  }

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

  getImageIcon(file) {

    if (file.fileName) {
      var splitFiles = file.fileName.split('.');
      var fileExtension = splitFiles[splitFiles.length - 1].toLocaleLowerCase();

      if (fileExtension == "pdf")
        return "assets/images/new-set/pdf-icon.svg";
      else if (fileExtension == "doc" || fileExtension == "docx")
        return "assets/images/new-set/word-icon.svg";
      else if (fileExtension == "png" || fileExtension == "jpg" || fileExtension == "jpeg")
        return "assets/images/new-set/image-icon.svg";
      else if (fileExtension == "xlsx")
        return "assets/images/new-set/excel-icon.svg";

    }
  }

  //apply all the office to the model
  onSelectAllOffices() {
    var officeIdList = this.officeList.map(x => x.id);
    this.model.offices = officeIdList;
  }

  //clear the office
  onClearAllOffices() {
    this.model.offices = [];
  }

  //remove the selected office
  unselectOffice(item) {
    const deletedIndex = this.model.offices.findIndex(x => x == item.id);
    if (deletedIndex != -1) {
      this.model.offices.splice(deletedIndex, 1);
      this.model.offices = [...this.model.offices];
    }
  }

  //apply all the positions to the model
  onSelectAllPositions() {
    var positionIds = this.positionList.map(x => x.id);
    this.model.positions = positionIds;
  }

  //clear the positions
  onClearAllPositions() {
    this.model.positions = [];
  }

  //remove the selected position
  unselectPosition(item) {
    const deletedIndex = this.model.positions.findIndex(x => x == item.id);
    if (deletedIndex != -1) {
      this.model.positions.splice(deletedIndex, 1);
      this.model.positions = [...this.model.positions];
    }
  }

  //fetch the country data with virtual scroll
  getCountryData() {

    this.dataManagementMaster.countryRequest.searchText = this.dataManagementMaster.countryInput.getValue();
    this.dataManagementMaster.countryRequest.skip = this.dataManagementMaster.countryList.length;

    this.dataManagementMaster.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.dataManagementMaster.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.dataManagementMaster.countryList = this.dataManagementMaster.countryList.concat(customerData);
        }

        this.dataManagementMaster.countryRequest = new CountryDataSourceRequest();
        this.dataManagementMaster.countryLoading = false;
      }),
      error => {
        this.dataManagementMaster.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.dataManagementMaster.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.dataManagementMaster.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.dataManagementMaster.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.dataManagementMaster.countryRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.dataManagementMaster.countryLoading = false))
      ))
      .subscribe(data => {
        this.dataManagementMaster.countryList = data;
        this.dataManagementMaster.countryLoading = false;
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

  changeCustomer(cusitem) {
    //clear the selected values
    this.model.brandIds = [];
    this.model.departmentIds = [];
    //clear the list
    this.brandList = [];
    this.deptList = [];
    if (cusitem != null && cusitem.id != null) {
      this.getCustomerBasedDetails(cusitem.id);
    }
  }
  getCustomerBasedDetails(customerId) {
    if (customerId) {
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

}




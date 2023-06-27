import { BehaviorSubject } from "rxjs/internal/BehaviorSubject";
import { CommonDataSourceRequest, CountryDataSourceRequest } from "../common/common.model";

export interface DataManagementListResponse {
  totalCount: number;
  index: number;
  pageSize: number;
  list: Array<DataManagementItem>;
  result: DataManagementListResult;
}

export enum DataManagementListResult {
  Success = 1,
  NotFound = 2
}

export class DataManagementItem {
  id: number;

  moduleId: number;
  serviceId: number;
  productCategoryId: number;
  fileTypeId: number;

  /* fileName: string;
  fileType: string;
  fileSize: number;
  fileId: string; */

  description: string;
  idCustomer: number | null;
  customerName: string;
  createdTime: string;
  offices: Array<any>;
  positions: Array<any>;
  countryIds: Array<any>;
  canEdit: boolean;
  canDelete: boolean;
  canDownload: boolean;
  canUpload: boolean;

  fileAttachments: Array<fileData>;
  editFileId: number;
  brandIds: number[] = [];
  departmentIds: number[] = [];

  constructor() {
    this.id = 0;
    this.customerName = "";
    this.description = "";
    this.idCustomer = null;
    this.moduleId = null;
    this.serviceId = null;
    this.productCategoryId = null;
    this.fileTypeId = null;

    this.offices = [];
    this.positions = [];
    this.canEdit = false;
    this.canDelete = false;
    this.canDownload = false;
    this.canUpload = false;
    this.fileAttachments = [];
    this.countryIds = [];
  }
}

export class fileData {
  id: number;
  fileName: string;
  fileUrl: string;
  fileId: string;
  fileSize: number;
  constructor() {
    this.fileName = "";
    this.fileId = "";
    this.fileUrl = "";
  }
}

export interface DataManagementListRequest {
  idModule: number;
  idCustomer: number | null;
  description: string;
  index: number;
  pageSize: number;
}

export interface DataManagementItemResponse {
  item: DataManagementItem;
  result: DataManagementItemResult;
}

export enum DataManagementItemResult {
  Success = 1,
  NotFound = 2,
  NotAuthorized = 3
}

export interface SaveDataManagementItemResponse {
  item: DataManagementItem;
  result: SaveDataManagementItemResult;
}

export enum SaveDataManagementItemResult {
  Success = 1,
  NotFound = 2,
  Error = 3
}


export interface DataManagementRight {
  idModule: number;
  moduleName: string;
  hasRight: boolean;
  children: Array<DataManagementRight>;
}


export interface DataManagementRightResponse {
  idStaff: number | null;
  idRole: number | null;
  modules: Array<DataManagementRight>;
  result: DataManagementRightResult;
  editRight: boolean;
  deleteRight: boolean;
  downloadRight: boolean;
  uploadRight: boolean;
  alreadyExistModules: string;
}


export enum DataManagementRightResult {
  Success = 1,
  NotFound = 2,
  Error = 3,
  RequestRequired = 4,
  IdStaffOrIdRoleRequired = 5,
  RightsRequired = 6,
  RightsAlreadyConfigured = 7
}

export interface DataManagementRightRequest {
  idStaff: number | null;
  idRole: number | null;
  idOffice: number | null;
  editRight: boolean;
  downloadRight: boolean;
  deleteRight: boolean;
  uploadRight: boolean;
}

export interface SaveDataManagementRightRequest {
  rightRequest: DataManagementRightRequest;
  id: number | null;
  modules: Array<number>;
  byRole: boolean;
  byEmployee: boolean;
}

export class Position {
  id: number;
  name: string;

}

export class HierarchyData {
  moduleName: string;
  order: number;
}

export class RightRequest {
  idStaff: number;
  idRole: number | null;
  idOffice: number | null;
  uploadRight: boolean;
  editRight: boolean;
  downloadRight: boolean;
  deleteRight: boolean;
  constructor() {
    this.editRight = false;
    this.downloadRight = false;
    this.deleteRight = false;
    this.uploadRight = false;
  }
}

export class UserManagementMaster {
  staffList: any;
  staffLoading: boolean
  staffInput: BehaviorSubject<string>;
  staffRequest: CommonDataSourceRequest;

  constructor() {
    this.staffRequest = new CommonDataSourceRequest();
    this.staffInput = new BehaviorSubject<string>("");
  }
}

export class DataManagementMaster {
  countryRequest: CountryDataSourceRequest;
  countryList: any;
  countryLoading: boolean
  countryInput: BehaviorSubject<string>;
  constructor() {
    this.countryInput = new BehaviorSubject<string>("");
    this.countryList = [];
    this.countryRequest = new CountryDataSourceRequest();
    this.countryLoading = false;
  }
}


export class DMUserManagementDataEditResponse {
  dmRole: SaveDataManagementRightRequest;
  result: DataManagementItemResult;
}

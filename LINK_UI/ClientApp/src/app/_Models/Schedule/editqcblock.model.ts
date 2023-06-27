import { BehaviorSubject } from "rxjs";
import { ListSize } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, DataSource, ProductCategorySourceRequest, ProductSubCategory2SourceRequest, ProductSubCategorySourceRequest } from "../common/common.model";

export class QCBlockEdit {
    id: number;
    qcId: number;
    customerIds: number[];
    supplierIds: number[];
    factoryIds: number[];
    productCategoryIds: number[];
    productCategorySubIds: number[];
    productCategorySub2Ids: number[];
}

export enum SaveQCBlockResponseResult
{
    Success = 1,
    Failure = 2,
    RequestNotCorrectFormat = 3,
    NoDataFound = 4,
    IsExists = 5,
    SelectAnyOtherField = 6
}

export class SaveQCBlockResponse
{
     id: number;
     result: SaveQCBlockResponseResult;
}


export class QCBlockDataSourceRequest {
    public searchText: string;
    public skip: number;
    public take: number;
    constructor() {
      this.searchText = "";
      this.skip = 0;
      this.take = ListSize;
    }
  }

export class QCBlockMasterData {
    customerList: Array<DataSource>;
    supplierList: Array<DataSource>;
    factoryList: Array<DataSource>;
    productCategoryList: Array<DataSource>;
    productCategorySubList: Array<DataSource>;
    productCategorySub2List: Array<DataSource>;
    qcList: Array<DataSource>;

    isQCDisabled: boolean;
    customerLoading: boolean;
    supplierLoading: boolean;
    factoryLoading: boolean;
    productCategoryLoading: boolean;
    productCategorySubLoading: boolean;
    productCategorySub2Loading:boolean;
    qcLoading: boolean;
    saveLoading: boolean;

    customerInput: BehaviorSubject<string>;
    supplierInput: BehaviorSubject<string>;
    factoryInput: BehaviorSubject<string>;
    productCategoryInput: BehaviorSubject<string>;
    productCategorySubInput: BehaviorSubject<string>;
    productCategorySub2Input: BehaviorSubject<string>;
    qcInput: BehaviorSubject<string>;

    customerModelRequest: CommonDataSourceRequest;
    factoryModelRequest: CommonDataSourceRequest;
    supplierModelRequest: CommonDataSourceRequest;
    qcModelRequest: CommonDataSourceRequest;

    productCategoryModelRequest: ProductCategorySourceRequest;
    productCategorySubModelRequest: ProductSubCategorySourceRequest;
    productCategorySub2ModelRequest: ProductSubCategory2SourceRequest;

    constructor() {
        this.productCategoryModelRequest = new ProductCategorySourceRequest();
        this.productCategorySubModelRequest = new ProductSubCategorySourceRequest();
        this.productCategorySub2ModelRequest = new ProductSubCategory2SourceRequest();

        this.customerModelRequest = new CommonDataSourceRequest();
        this.factoryModelRequest = new CommonDataSourceRequest();
        this.supplierModelRequest = new CommonDataSourceRequest();
        this.qcModelRequest = new CommonDataSourceRequest();

        this.customerInput = new BehaviorSubject<string>("");
        this.supplierInput = new BehaviorSubject<string>("");
        this.factoryInput = new BehaviorSubject<string>("");
        this.qcInput  = new BehaviorSubject<string>("");
        this.productCategoryInput  = new BehaviorSubject<string>("");
        this.productCategorySub2Input = new BehaviorSubject<string>("");
        this.productCategorySubInput = new BehaviorSubject<string>("");
        
        this.customerList = new Array<DataSource>();
        this.supplierList = new Array<DataSource>();
        this.qcList  = new Array<DataSource>();
        this.factoryList  = new Array<DataSource>();
        this.productCategorySubList = new Array<DataSource>();
        this.productCategoryList  = new Array<DataSource>();
        this.productCategorySub2List = new Array<DataSource>();
    }
}

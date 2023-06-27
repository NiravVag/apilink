import { DataSource } from "@amcharts/amcharts4/core";
import { BehaviorSubject } from "rxjs";
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';

export class SubjectConfigMasterModel {
    isUseByEmailSend: boolean;
    customerLoading: boolean;
    customerList: Array<DataSource>;
    customerInput: BehaviorSubject<string>;
    saveLoading: boolean;
    customerModelRequest: CommonDataSourceRequest;
    fieldColumnList: Array<SubConfigColumnModel>;
    popUpLoading: boolean;
    dateFormats: Array<DataSource>;


    templateId: number;
    FieldLoading: boolean;
    dateFormatLoading: boolean;
    popupTitle: string;

    emailTypeLoading: boolean;
    emailTypeList: Array<DataSource>;
    moduleLoading: boolean;
    moduleList: Array<DataSource>;
    delimiterList: Array<EmailSubjectDelimiter>;
    filteredDelimiterList: Array<EmailSubjectDelimiter>;
    delimiterLoading: boolean;

    constructor() {
        this.customerList = new Array<DataSource>();
        this.customerInput = new BehaviorSubject<string>("");
        this.fieldColumnList = new Array<SubConfigColumnModel>();
        this.customerModelRequest = new CommonDataSourceRequest();
        this.popupTitle = null;
    }
}

export class TemplatePopup {

    maxChar: number;
    isTitle: boolean;
    titleCustomName: string;
    maxItems: number;
    dateFormat: number;
    isDateSeperator: boolean;
    dataType: number;
    constructor() {
        this.maxChar = null;
        this.isTitle = null;
        this.titleCustomName = null;
        this.maxItems = null;
        this.dateFormat = null;
        this.isDateSeperator = false;
        this.dataType = null;
    }
}

export class SubjectConfigModel {
    customerId: number;
    templateName: string;
    templateDisplayName: string;
    emailTypeId: number;
    delimiterId: number;
    delimiter: string;
    moduleId: number;
    id: number;
    templateColumnList: Array<SubConfigColumnModel>;

    constructor() {
        this.templateColumnList = new Array<SubConfigColumnModel>();
    }
}

export class SubConfigColumnModel {
    id: number;
    name: string;
    fieldId: number;
    alaisName: string;
    maxChar: number;
    isTitle: boolean;
    titleCustomName: string;
    maxItems: number;
    dateFormat: number;
    isDateSeperator: boolean;
    dataType: number;
    sort: number;
}

export enum SubConfigResponseResult {
    Success = 1,
    NotFound = 2,
    Failure = 3,
    RequestNotCorrectFormat = 4,
    TemplateNameExists = 5,
    TemplateFieldsExists = 6,
    NoExists = 7,
    MappedEmailRule = 8
}

export enum PreDefinedFieldDataType {
    Date = 1,
    List = 2
}

export enum EmailSubjectModule {
    Subject = 1,
    FileName = 2
}

export class EmailSubjectDelimiter {
    id: number;
    name: string;
    isFile: boolean;
}

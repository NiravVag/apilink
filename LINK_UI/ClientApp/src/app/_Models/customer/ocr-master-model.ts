export class OcrMasterModel {
    ocrTableNameList: Array<string>;
    ocrTableData: Array<OcrTableItem>;
    ocrName: string;
    ocrList: Array<Ocr>;
    ocrValidators: Array<any>;
    isAllSelected: boolean;
    showOcrExport: boolean;
    exportOcrLoading: boolean;
    ocrFileUniqueld: string;
    constructor() {
        this.ocrTableNameList = [];
        this.ocrTableData = [];
        this.ocrList = [];
        this.ocrValidators = [];
        this.isAllSelected = false;
        this.showOcrExport = false;
        this.exportOcrLoading = false;
    }
}

export class OcrTableRequest {
    file: string;
    brand_name: string;
    brand_format: string;
}

export class OcrTableItem{
    ocrTableName: string;
    ocrTableList: Array<Ocr>;
    constructor(){
        this.ocrTableList = [];
    }
}

export class Ocr {
    id: number;
    productId: number;
    productFileId: number;
    code: string;
    description: string;
    mpcode: string;
    required: number;
    tolerance1Up: number;
    tolerance1Down: number;
    tolerance2Up: number;
    tolerance2Down: number;
    sort: number;
    isSelected: boolean;
    constructor() {
        this.isSelected = false;
    }
}


export class EditAuditmodel {

    public auditid: number;

    public auditbookingno: string;

    public customerid: number;

    public brandid: number;

    public departmentid: number;

    public servicetypeid: number;

    public servicedatefrom: any;

    public servicedateto: any;

    public seasonid: number;

    public seasonyearid: number;

    public evaluationroundid: number

    public applicantname: string;

    public applicantemail: string;

    public applicantphno: string;

    public supplierid: number;

    public factoryid: number;

    public factorycreationdate: any;

    public noofcustomers: string;

    public noofsuppliers: string;

    public annualproduction: string;

    public maximumcapacity: string;

    public factorysurfacearea: string;

    public totalcapacitybycustomer: string;

    public factoryextension: string;

    public manufactureproducts: string;

    public brandsproduced: string;

    public numberofsites: string;

    public openhour: string;

    public annualholidays: string;

    public productionstaff: number;

    public adminstaff: number;

    public qualitystaff: number;

    public totalstaff: number;

    public salesstaff: number;

    public investment: string;

    public liability: string;

    public tradeassociation: string;

    public accreditations: string;

    public customerComments: string;

    public aPIComments: string;

    public isemailrequired: boolean;

    public ponumber: string;

    public reportno: string;

    public officeid: number;

    public internalComments: string;

    public customercontactlistitems: any[];

    public suppliercontactlistitems: any[];

    public factorycontactlistitems: any[];

    public StatusId: number;

    public attachments: Array<AttachmentFile>;

    public createdbyUserType: number;

    public auditors: any[];

    public auditCS: any[];

    public audittypeid: number;

    public auditworkprocessItems: any[];

    public customerBookingNo: string;
    public auditProductCategoryId: number;

   
    public isEaqf: boolean = false;
    public isSupplierOrFactoryEmailSend: boolean;
    public isCustomerEmailSend: boolean;
}
export class AttachmentFile {
    public id: number;
    public uniqueld: string;
    public fileName: string;
    public isNew: boolean;
    public mimeType: string;
    public file: File;
    public fileUrl: string;
}


export class AuditMaster {
    customerServiceList: any;
    constructor() {
        this.customerServiceList = [];
    }
}

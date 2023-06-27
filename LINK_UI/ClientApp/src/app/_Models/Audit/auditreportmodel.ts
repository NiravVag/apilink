export class Auditreportmodel {
    public auidtid:number;
    public servicedatefrom:string;
    public servicedateto:string;
    public auditors:any[];
    public attachments:Array<AttachmentFile>;
    public comment:string;
}
export class AttachmentFile {
    public id: number;
    public uniqueld: string; 
    public fileName: string;
    public isNew: boolean;
    public mimeType: string;
    public file: File;       
    public guid: any; 
    public fileUrl: string;
  }

import { FileContainerList } from "src/app/components/common/static-data-common";

export class AttachmentFile {
  public id: number;
  public uniqueld: string;
  public fileName: string;
  public fileSize: number;
  public isNew: boolean;
  public mimeType: string;
  public fileUrl: string;
  public file: File;
  public isSelected: boolean;
  public status: number = 0;
  public fileDescription: string;
}


export class FileInfo {
  public uploadLimit: number
  public fileSize: number;
  public uploadFileExtensions: string;
  public containerId: FileContainerList;
  public token: string;
  public fileDescription: string;
}


export class FileUploadResponse {
  fileUploadDataList:Array<FileUploadData>;
}

export class FileUploadData {
  fileName: string;
  fileCloudUri: string;
  fileUniqueId: string;
  result: FileUploadResponseResult;
}

export enum FileUploadResponseResult {
  Sucess = 1,
  Failure = 2,
  FileSizeExceed = 3
}

import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { AttachmentFile, FileUploadResponse } from 'src/app/_Models/fileupload/fileupload';
import { FileContainerList } from 'src/app/components/common/static-data-common';
import { UtilityService } from '../common/utility.service';


@Injectable({ providedIn: 'root' })

export class FileStoreService {


  fileServer: string

  devEnvironment: boolean

  constructor(private http: HttpClient, public utility: UtilityService, @Inject('FILE_SERVER_URL') fileServerUrl: string, @Inject('DEV_ENV') devEnv: boolean) {

    this.fileServer = fileServerUrl;
    this.devEnvironment = devEnv;

    if (this.fileServer.charAt(this.fileServer.length - 1) == '/')
      this.fileServer = this.fileServer.substring(0, this.fileServer.length - 1);
  }

  uploadFiles(cloudContainer, attachedList: Array<AttachmentFile>) {

    var entity = Number(this.utility.getEntityId());

    if (this.devEnvironment) {
      cloudContainer = FileContainerList.DevContainer
    }

    const formData: FormData = new FormData();

    attachedList.map(x => {

      formData.append('files', x.file, x.uniqueld);

    });

    const headers = new HttpHeaders().append('Content-Disposition', 'mulipart/form-data');



    return this.http.post<any>(`${this.fileServer}/api/FileStore/savefile/${cloudContainer}/${entity}`, formData, { headers: headers })
      .pipe(map(response => {
        return response;
      }));

  }

  async uploadFileData(cloudContainer, attachedList: Array<AttachmentFile>) : Promise<FileUploadResponse> {

    var entity = Number(this.utility.getEntityId());

    if (this.devEnvironment) {
      cloudContainer = FileContainerList.DevContainer
    }

    const formData: FormData = new FormData();

    attachedList.map(x => {

      formData.append('files', x.file, x.uniqueld);

    });

    const headers = new HttpHeaders().append('Content-Disposition', 'mulipart/form-data');



    return this.http.post<any>(`${this.fileServer}/api/FileStore/savefile/${cloudContainer}/${entity}`, formData, { headers: headers })
      .pipe(map(response => {
        return response;
      })).toPromise();

  }

  downloadBlobFile(id, cloudContainer) {
    if (this.devEnvironment) {
      cloudContainer = FileContainerList.DevContainer
    }
    var entityId = Number(this.utility.getEntityId());
    return this.http.get(`${this.fileServer}/api/FileStore/downloadfile/${id}/${cloudContainer}/${entityId}`, { responseType: 'blob' });
  }

}

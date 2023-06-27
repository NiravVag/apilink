import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpBackend, HttpParams } from '@angular/common/http';
import { map, delay } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { AutoPoNumberPage } from '../../_Models/purchaseorder/edit-purchaseorder.model';
import { TCFRequestData } from 'src/app/_Models/tcf/tcflanding.model';
import { AttachmentFile } from 'src/app/_Models/fileupload/fileupload';
import { TCFDocumentUpload } from 'src/app/_Models/tcf/tcfdetail.model';
import { APIServiceEnum } from 'src/app/components/common/static-data-common';
import { GenericAPIGETRequest, GenericFileUploadRequest } from 'src/app/_Models/genericapi/genericapirequest.model';

export interface Person {
    id: string;
    isActive: boolean;
    age: number;
    name: string;
    gender: string;
    company: string;
    email: string;
    phone: string;
    disabled?: boolean;
}

@Injectable({ providedIn: 'root' })
export class TCFService {
    url: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;
        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }


    getData(tcfRequestData: GenericAPIGETRequest) {
        return this.http.post<any>(`${this.url}/api/apigateway/GetData`, tcfRequestData)
            .pipe(map(response => {
                return response;
            }));
    }

    postData(request) {
        return this.http.post<any>(`${this.url}/api/apigateway/`, request)
            .pipe(map(response => {
                return response;
            }));
    }

    putData(request) {
        return this.http.put<any>(`${this.url}/api/apigateway/`, request)
            .pipe(map(response => {
                return response;
            }));
    }

    formatDate(dateObject) {
        return dateObject.year + "-" + dateObject.month + "-" + dateObject.day;
    }

    getFileExtension(fileName) {
        return fileName.substring(fileName.lastIndexOf(".") + 1)
    }

    uploadTCFDocument(tcfDocumentUpload: TCFDocumentUpload, attachedList: Array<AttachmentFile>, userToken) {


        const formData: FormData = new FormData();

        attachedList.map(x => {

            formData.append('files', x.file, x.fileName);

        });

        formData.append('documentName', tcfDocumentUpload.documentName);
        formData.append('standardIds', tcfDocumentUpload.standardIds);
        if (tcfDocumentUpload.typeId)
            formData.append('typeId', tcfDocumentUpload.typeId.toString());
        if (tcfDocumentUpload.issuerId)
            formData.append('issuerId', tcfDocumentUpload.issuerId.toString());
        formData.append('documentIssueDate', this.formatDate(tcfDocumentUpload.issueDate));
        if (tcfDocumentUpload.tcfId)
            formData.append('tcfId', tcfDocumentUpload.tcfId.toString());
        formData.append('apiService', APIServiceEnum.TCF.toString());
        formData.append('userToken', userToken);

        const headers = new HttpHeaders().append('Content-Disposition', 'mulipart/form-data');

        headers.append('Content-Disposition', 'application/json');

        return this.http.post<any>(`${this.url}/api/apigateway/attachdocument`, formData, { headers: headers })
            .pipe(map(response => {
                return response;
            }));
    }

    downloadBlobFile(tcfRequestData) {
        return this.http.post<Blob>(`${this.url}/api/apigateway/downloadfile`, tcfRequestData, { responseType: 'blob' as 'json' });
        /*   .pipe(map(response => {
              return response;
          })); */
    }

    uploadTCFiles(genericFileUploadRequest: GenericFileUploadRequest, attachedList: Array<AttachmentFile>) {


        const formData: FormData = new FormData();

        attachedList.map(x => {
            formData.append('files', x.file, x.fileName);
        });

        formData.append('requestUrl', genericFileUploadRequest.requestUrl);
        formData.append('client', APIServiceEnum.TCF.toString());
        formData.append('token', genericFileUploadRequest.token);

        const headers = new HttpHeaders().append('Content-Disposition', 'mulipart/form-data');
        headers.append('Content-Disposition', 'application/json');

        return this.http.post<any>(`${this.url}/api/apigateway/uploadfiles`, formData, { headers: headers },)
            .pipe(map(response => {
                return response;
            }));
    }
}

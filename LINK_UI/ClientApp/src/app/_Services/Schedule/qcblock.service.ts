import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { QCBlockEdit } from "src/app/_Models/Schedule/editqcblock.model";
import { QCBlockRequestModel } from "src/app/_Models/Schedule/qcblocksummary.model";

@Injectable({
    providedIn: 'root'
})

export class QCBlockService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }
    save(model: QCBlockEdit) {
        return this.http.post<any>(`${this.url}/api/QCBlock`, model);
    }
    edit(id: number) {
        return this.http.get<any>(`${this.url}/api/QCBlock/${id}`);
    }
    search(model: QCBlockRequestModel) {
        return this.http.post<any>(`${this.url}/api/QCBlock/search`, model);
    }
    export(model: QCBlockRequestModel) {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/QCBlock/export-summary`, model, { headers: headers, responseType: 'blob' as 'json' });
    }
    delete(ids: number[]) {
        return this.http.post<any>(`${this.url}/api/QCBlock/delete`, ids);
    }
}
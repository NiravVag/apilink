import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { DataSourceResponse } from "../../_Models/common/common.model";
import { OtherMandayEditModel, OtherMandayModel } from "src/app/_Models/otherManday/otherManday.model";

@Injectable({
    providedIn: 'root'
})

export class OtherMandayService {

    url: string

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;
        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async saveOtherManday(model: OtherMandayEditModel) {
       
        if (model.id > 0) {
            return await this.http.put<any>(`${this.url}/api/OtherManday/update`, model)
                .toPromise();
        }

        else {
                return await this.http.post<any>
                    (`${this.url}/api/OtherManday/save`, model).toPromise();
        }
    }

    async editOtherManday(id: number) {
        return await this.http.get<any>(`${this.url}/api/OtherManday/edit-other-manday/${id}`)
            .toPromise();
    }

    async getOtherMandaySummary(model: OtherMandayModel) {
        return await this.http.post<any>
            (`${this.url}/api/OtherManday/other-manday-summary`, model).toPromise();
    }


    async deleteOtherManday(id: number) {
        return await this.http.delete<any>(`${this.url}/api/OtherManday/delete/${id}`)
            .toPromise();
    }

    async getPurposeList() {
        return await this.http.get<any>(`${this.url}/api/OtherManday/getPurposeList`)
            .toPromise();
    }

    exportSummary(request: OtherMandayModel) {
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/OtherManday/ExportOtherMandaySummary`, request, { headers: headers, responseType: 'blob' as 'json' });
      }
}

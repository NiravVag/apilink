import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EditWorkLoadMatrixModel } from "src/app/_Models/workloadmatrix/workloadmatrix.model";
import { map } from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})

export class WorkLoadMatrixService {
    url: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async saveWorkLoadMatrix(model: EditWorkLoadMatrixModel) {

        if (model.id > 0) {
            return await this.http.put<any>(`${this.url}/api/workloadmatrix/update`, model)
                .toPromise();
        }
        else {
            return await this.http.post<any>(`${this.url}/api/workloadmatrix/save`, model)
                .toPromise()
        }
    }

    async getWorkLoadMatrixById(id, workLoadMatrixNotConfigured: boolean) { 
        return this.http.get<any>(`${this.url}/api/workloadmatrix/edit-work-load-matrix/${id}/${workLoadMatrixNotConfigured}`).toPromise();
    }

    async deleteWorkLoadMatrixById(id) {
        return await this.http.delete<any>(`${this.url}/api/workloadmatrix/delete/${id}`).toPromise();
    }

    getWorkLoadMatrixSearchSummary(request) {
        return this.http.post<any>(`${this.url}/api/workloadmatrix/work-load-matrix-summary`, request)
            .pipe(map(response => { return response; }));
    }

    async getWorkLoadMatrixByProdCatSub3Id(prodCatSub3Id: number) { 
        return this.http.get<any>(`${this.url}/api/workloadmatrix/work-load-matrix-by-prodCatSub3Id/${prodCatSub3Id}`).toPromise();
    }

    exportSummary(request) {
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/workloadmatrix/Export-workload-matrix`, request, { headers: headers, responseType: 'blob' as 'json' });
      }
}
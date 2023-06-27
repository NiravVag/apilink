import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { TravelMatixSaveResponse, TravelMatrixSummary, TravelMatixSearchResponse, QuotationTravelMatrixResponse, TravelMatrixRequest } from "src/app/_Models/invoice/travelmatrix";
import { promise } from "protractor";
import { map } from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})
export class TravelMatrixService {

    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }
    getTarifftypes() {
        return this.http.get<any>(`${this.url}/api/TravelMatrix/travelMatrixTypes`);
    }
    async save(model: Array<any>): Promise<TravelMatixSaveResponse> {
        return await this.http.post<TravelMatixSaveResponse>(`${this.url}/api/TravelMatrix/save`, model).toPromise();
    }    
    search(model: TravelMatrixSummary) {
        return this.http.post<any>(`${this.url}/api/TravelMatrix/search`, model);
    }
    async getProvinceLists(countryIds: number[]) {
        return await this.http.post<any>(`${this.url}/api/TravelMatrix/getProvinceLists`, countryIds).toPromise();
    }

    async getCityLists(provinceIds: number[]) {
        return await this.http.post<any>(`${this.url}/api/TravelMatrix/getCityLists`, provinceIds).toPromise();
    }

    async getCountyLists(cityIds: number[]) {
        return await this.http.post<any>(`${this.url}/api/TravelMatrix/getCountyLists`, cityIds).toPromise();
    }
    delete(ids: number[]) {
        return this.http.post<any>(`${this.url}/api/TravelMatrix/delete`, ids);
    }
    exportSummary(model: TravelMatrixSummary) {
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/TravelMatrix/export`, model, { headers: headers, responseType: 'blob' as 'json' });
      }
      
  async getTravelMatrixData(request: TravelMatrixRequest): Promise<QuotationTravelMatrixResponse> {
    return await this.http.post<QuotationTravelMatrixResponse>(`${this.url}/api/Quotation/getTravelMatrixData`, request).toPromise();
  }

  getCountyListByCountry(countyName, countryId) {  
       return this.http.get<any>(`${this.url}/api/TravelMatrix/getCountyListByCountry/${countryId}/${countyName}`)
        .pipe(map(response => {
          return response.dataSourceList;
        }));
  }  
}  

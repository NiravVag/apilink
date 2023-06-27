import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { TravelTariffDeleteResponse, TravelTariffGetAllResponse, TravelTariffGetResponse, TravelTariffSaveRequest, TravelTariffSaveResponse, TravelTariffSearchRequest } from "src/app/_Models/traveltariff/traveltariff";
import { DataSourceResponse } from "src/app/_Models/common/common.model";

@Injectable({
    providedIn: 'root'
})

export class TravelTariffService {

    url: string

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) 
    {
        this.url = baseUrl;
        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async saveTravelTariff(model: TravelTariffSaveRequest): Promise<TravelTariffSaveResponse> 
    {
        return await this.http.post<TravelTariffSaveResponse>
                     (`${this.url}/api/TravelTariff`, model).toPromise();
    }       

    async getTravelTariff(id: number) 
    {
        return await this.http.get<TravelTariffGetResponse>(`${this.url}/api/TravelTariff/${id}`)
        .toPromise();
    }

    async getStartPortList() 
    {
        return await this.http.get<DataSourceResponse>(`${this.url}/api/TravelTariff/StartPotList`)
        .toPromise();
    }

   async getAllTravelTariff(model:TravelTariffSearchRequest): Promise<TravelTariffGetAllResponse> 
    {
        return await this.http.post<TravelTariffGetAllResponse>
        (`${this.url}/api/TravelTariff/GetAllTravelTariff`, model).toPromise();
    }

    async updateTravelTariff(model: TravelTariffSaveRequest): Promise<TravelTariffSaveResponse> 
    {
        return await this.http.put<TravelTariffSaveResponse>(`${this.url}/api/TravelTariff/${model.id}`, model)
        .toPromise();
    } 

    async deleteTravelTariff(id: number) 
    {
        return await this.http.delete<TravelTariffDeleteResponse>(`${this.url}/api/TravelTariff/${id}`)
        .toPromise();
    } 
    
    exportSummary(request: TravelTariffSearchRequest) {
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/TravelTariff/Export`, request, { headers: headers, responseType: 'blob' as 'json' });
      }
}  

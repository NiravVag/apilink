import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { StartingPortEditModel, StartingPortGetResponse, StartingPortModel, StartingPortSaveResponse } from "../../_Models/expense/startingport.model";
import { DataSourceResponse } from "../../_Models/common/common.model";

@Injectable({
    providedIn: 'root'
})

export class StartingPortService {

    url: string

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;
        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async saveStartingPort(model: StartingPortEditModel): Promise<StartingPortSaveResponse> {
        if (model.id > 0) {
            return await this.http.put<StartingPortSaveResponse>(`${this.url}/api/StartingPort/${model.id}`, model)
                .toPromise();
        }

        else {
                return await this.http.post<StartingPortSaveResponse>
                    (`${this.url}/api/StartingPort`, model).toPromise();
        }
    }

    async getStartingPort(id: number) {
        return await this.http.get<StartingPortGetResponse>(`${this.url}/api/StartingPort/${id}`)
            .toPromise();
    }

    async getAllStartingPort(model: StartingPortModel): Promise<StartingPortGetResponse> {
        return await this.http.post<StartingPortGetResponse>
            (`${this.url}/api/StartingPort/get_starting_port`, model).toPromise();
    }


    async deleteStartingPort(id: number) {
        return await this.http.delete<StartingPortSaveResponse>(`${this.url}/api/StartingPort/${id}`)
            .toPromise();
    }
}

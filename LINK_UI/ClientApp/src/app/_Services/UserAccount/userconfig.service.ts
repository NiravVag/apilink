import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { UserAccountSummaryModel, UserAccountModel } from 'src/app/_Models/useraccount/useraccount.model';
import { UserDataAccess, SaveUserConfigResponse, EditUserConfigResponse } from 'src/app/_Models/useraccount/userconfig.model';

@Injectable({
  providedIn: 'root'
})

export class UserConfigService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      this.url = baseUrl;
  
      if (this.url.charAt(this.url.length - 1) == '/')
        this.url = this.url.substring(0, this.url.length - 1);
    }

    async save(model: UserDataAccess) {
      return await this.http.post<SaveUserConfigResponse>(`${this.url}/api/UserConfig`, model).toPromise();
    }

    async edit(userId: number) {
      return await this.http.get<EditUserConfigResponse>(`${this.url}/api/UserConfig/${userId}`).toPromise();
    }
}
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { UserAccountSummaryModel, UserAccountModel } from 'src/app/_Models/useraccount/useraccount.model';
import { UserNameResponse } from 'src/app/_Models/useraccount/userconfig.model';
import { promise } from 'protractor';
import { UserDataSourceRequest } from 'src/app/_Models/common/common.model';

@Injectable({
  providedIn: 'root'
})

export class UserAccountService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      this.url = baseUrl;
  
      if (this.url.charAt(this.url.length - 1) == '/')
        this.url = this.url.substring(0, this.url.length - 1);
    }

    getUserAccountSummary() {
        return this.http.get<any>(`${this.url}/api/UserAccount`)
          .pipe(map(Response => { return Response; }));
    }

    getUserTokenToFB() {
      return this.http.get<any>(`${this.url}/api/UserAccount/getTokentoFB`)
        .pipe(map(Response => { return Response; }));
  }

    getUserAccountSearchSummary(request) {
      return this.http.post<any>(`${this.url}/api/UserAccount`, request)
        .pipe(map(response => { return response; }));
    }

    getUserAccountDetail(request) {
      return this.http.post<any>(`${this.url}/api/UserAccount/edit`, request)
        .pipe(map(response => { return response; }));
    }

    saveUserAccount(model: UserAccountModel) {
      if (!model.id)//add
      return this.http.post<any>(`${this.url}/api/UserAccount/save`, model)
        .pipe(map(response => {
          return response;
        }));
      else
        return this.updateUserAccount(model);
    }

    updateUserAccount(model: UserAccountModel) {
      return this.http.put<any>(`${this.url}/api/UserAccount/save`, model)
        .pipe(map(response => {
          return response;
        }));
    }

    deleteUserAccountById(id) {
      return this.http.delete<any>(`${this.url}/api/UserAccount/${id}`)
        .pipe(map(Response => { return Response; }));
    }
    async loggedUserRoleExist(roleId: number) : Promise<boolean> {
      return await this.http.get<boolean>(`${this.url}/api/UserAccount/loggedUserRoleExists/${roleId}`).toPromise();
    }

    loggedUserRoleExists(roleId: number)  {
      return this.http.get<any>(`${this.url}/api/UserAccount/loggedUserRoleExists/${roleId}`);
    }

   async getUserName(id: number) {
      return await this.http.get<UserNameResponse>(`${this.url}/api/UserAccount/getUserName/${id}`).toPromise();
    }

    saveUser(model: UserAccountModel) {
      return this.http.post<any>(`${this.url}/api/UserAccount/save-user`, model)
        .pipe(map(response => {
          return response;
        }));
    }

    getUserDetails(contactId: number, usertypeId: number) {
      return this.http.get<any>(`${this.url}/api/UserAccount/user-detail/${contactId}/${usertypeId}`);
    }

    getStaffList(model: UserDataSourceRequest, term?: string) {
      model.searchText = term ? term : "";
      return this.http.post<any>(`${this.url}/api/UserAccount/getuserdatasource`, model)
        .pipe(map(response => {
          return response.dataSourceList;
        }));
    }

  forgotpassword(username: string) {
    return this.http.get<any>(`${this.url}/api/UserAccount/forgotpassword/${encodeURIComponent(username)}`);
  }

}

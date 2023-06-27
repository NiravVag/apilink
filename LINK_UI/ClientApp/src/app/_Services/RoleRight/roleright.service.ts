import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { RoleRightModel } from '../../_Models/roleright/rolerightmodel'

@Injectable({
    providedIn: 'root'
})

export class RoleRightService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      this.url = baseUrl;
  
      if (this.url.charAt(this.url.length - 1) == '/')
        this.url = this.url.substring(0, this.url.length - 1);
    }

    getRoleRightSummary() {
        return this.http.get<any>(`${this.url}/api/RoleRight`)
          .pipe(map(Response => { return Response; }));
    }

    getRoleRightByRoleId(id) {
      return this.http.get<any>(`${this.url}/api/RoleRight/${id}`)
        .pipe(map(Response => { return Response; }));
  }

  saveRoleRight(model: RoleRightModel) {
    return this.http.put<any>(`${this.url}/api/RoleRight`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getRoleList(userId: number) {
    return this.http.get<any>(`${this.url}/api/RoleRight/getRoleList/${userId}`)
      .pipe(map(Response => { return Response; }));
  }
}
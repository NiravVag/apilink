import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { GetUserProfileResponse, SaveUserProfileResponse, UserProfileModel } from 'src/app/_Models/useraccount/userprofile.model';

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      this.url = baseUrl;
  
      if (this.url.charAt(this.url.length - 1) == '/')
        this.url = this.url.substring(0, this.url.length - 1);
    }

    async save(model: UserProfileModel) {
      return await this.http.post<SaveUserProfileResponse>(`${this.url}/api/UserProfile`, model).toPromise();
    }

    async getUserProfileSummary(userId: number) {
      return await this.http.get<GetUserProfileResponse>(`${this.url}/api/UserProfile/${userId}`).toPromise();
    }
}
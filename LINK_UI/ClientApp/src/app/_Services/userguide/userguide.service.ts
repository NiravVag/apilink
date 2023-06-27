import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { UserGuideDetailResponse } from 'src/app/_Models/userguide/userguide.model';

@Injectable({
    providedIn: 'root'
})
export class UserGuideService {
    url: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async getUserGuideDetails(): Promise<UserGuideDetailResponse> {
        return await this.http.get<UserGuideDetailResponse>(`${this.url}/api/userguide/`).toPromise();
    }

    downloadFile(userGuideId) {
        return this.http.get(`${this.url}/api/userguide/downloadFile/${userGuideId}`, { responseType: 'blob' });
    }


}
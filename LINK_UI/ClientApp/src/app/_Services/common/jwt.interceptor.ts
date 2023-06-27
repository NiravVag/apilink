import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { UtilityService } from './utility.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private translate: TranslateService,public utility:UtilityService) {

  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    let token = JSON.parse(localStorage.getItem('_APItoken'));
    let entity= localStorage.getItem('_entityId')
   
    if (token) {
        request = request.clone({
            setHeaders: { 
              Authorization: `Bearer ${token}`,
              entityId: entity
          },
          setParams: {
            lang: this.translate.currentLang
          }
        });
    }
        return next.handle(request);
    }
}

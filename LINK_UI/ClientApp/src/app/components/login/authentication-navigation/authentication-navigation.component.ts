import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { HRProfileEnum, RoleEnum, UserType } from '../../common/static-data-common';

@Component({
  selector: 'app-authentication-navigation',
  templateUrl: './authentication-navigation.component.html'
})

export class AuthenticationNavigationComponent implements OnInit {
  
  returnUrl: string;

  constructor(
    private router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    private authenticationService: AuthenticationService,
    private activateRoute: ActivatedRoute,) {
  }
  
  //onInit
  ngOnInit(id?: any, inputparam?: ParamMap): void {
    // get return url from route parameters or default to '/'
    this.returnUrl = this.activateRoute.snapshot.queryParams['returnUrl'] || '/';    
    this.authenticationService.redirectToLanding();
  }

  getViewPath(): string {
    throw new Error('Method not implemented.');
  }

  getEditPath(): string {
    throw new Error('Method not implemented.');
  }
}

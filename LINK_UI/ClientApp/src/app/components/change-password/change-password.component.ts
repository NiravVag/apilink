import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { Validator, WaitingService, JsonHelper } from '../common'
import { AuthenticationService } from '../../_Services/user/authentication.service';
import { ChangePassword } from 'src/app/_Models/user/changepassword.model';
import { from } from 'rxjs';
import { DetailComponent } from '../common/detail.component';
import { APIService, ResetPasswordResult, UserType } from '../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent extends DetailComponent {
  public model: ChangePassword;
  public jsonHelper: JsonHelper;
  saveloading: boolean = false;
  userName: string;
  isResetPassword: boolean = false;
  constructor(
    public authService: AuthenticationService,
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    public utility: UtilityService,
    route: ActivatedRoute,
    router: Router,
    public plaiRoute: Router,
    public pathroute: ActivatedRoute
  ) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("user/changepassword.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.model = new ChangePassword();

  }

  onInit() {
    this.pathroute.queryParams.subscribe(
      params => {
        if (params != null && params['username'] != null) {
          this.userName = params['username'];
          this.isResetPassword = true;
        }
      });
  }

  getEditPath() { return ""; }
  getViewPath() { return ""; }

  cancel() {
    this.model.confirmPassword = "";
    this.model.currentPassword = "";
    this.model.newPassword = "";
  }

  changePassword() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {

      if (this.validatePassword(this.model.newPassword)) {

        if (this.comparePassword(this.model.newPassword, this.model.confirmPassword)) {
          this.saveloading = true;
          this.authService.changePassword(this.model)
            .subscribe(res => {
              if (res && res.result == 1) {
                this.saveloading = false;
                this.showSuccess('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_CHANGEPASSWORD_SUCCESS');
                //this.ReturnToPage();
                this.plaiRoute.navigate(["/landing"]);
              }
              else if (res && res.result == 2) {
                this.saveloading = false;
                this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_PASSWORD_NOTSAVED');
              }
              else if (res && res.result == 3) {
                this.saveloading = false;
                this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_CURRENT_PASSWORDNOTAVAIALABLE');
              }
              else if (res && res.result == 4) {
                this.saveloading = false;
                this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_COMPARE_PASSWD');
              }
              else if (res && res.result == 5) {
                this.saveloading = false;
                this.showSuccess('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_NEW_PASSWD_RULE');
              }
              this.saveloading = false;
            })
        }
        else {
          this.saveloading = false;
          this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_COMPARE_PASSWD');
        }
      }
    }

  }

  resetPassword() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {

      if (this.validatePassword(this.model.newPassword)) {

        if (this.comparePassword(this.model.newPassword, this.model.confirmPassword)) {
          this.saveloading = true;
          this.model.userName = this.userName;
          this.authService.resetPassword(this.model)
            .subscribe(res => {
              if (res && res.result == ResetPasswordResult.Success) {
                this.saveloading = false;
                this.showSuccess('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_CHANGEPASSWORD_SUCCESS');
                this.plaiRoute.navigate(["/login"]);
              }
              else if (res && res.result == ResetPasswordResult.PasswordNotSaved) {
                this.saveloading = false;
                this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_PASSWORD_NOTSAVED');
              }
              else if (res && res.result == ResetPasswordResult.PasswordNotMatch) {
                this.saveloading = false;
                this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_COMPARE_PASSWD');
              }
              else if (res && res.result == ResetPasswordResult.InvalidPassword) {
                this.saveloading = false;
                this.showSuccess('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_NEW_PASSWD_RULE');
              }
              this.saveloading = false;
            })
        }
        else {
          this.saveloading = false;
          this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_COMPARE_PASSWD');
        }
      }
    }

  }
  // ReturnToPage() {
  //   var user = this.authService.getCurrentUser();
  //   if (user && user.usertype == UserType.InternalUser) {
  //     // check user has tcf Access
  //     if (user.serviceAccess.filter(x=>x==APIService.Tcf).length>0)
  //       this.return("/tcfsummary/tcf-summary");
  //     else
  //       this.returnToDashboard();
  //   }
  //   else if (user.usertype == UserType.Customer || user.usertype == UserType.Supplier) {
  //     this.plaiRoute.navigate(["/landing"])
  //   }
  //   else//all external user redirect to booking search page.
  //   {
  //     this.return("/inspsummary/booking-summary");
  //   }
  // }

  checkUserHasAccess(serivceAccess): boolean {
    var user = JSON.parse(localStorage.getItem('currentUser'));
    return user.serviceAccess.filter(x => x == serivceAccess).length > 0
  }
  validatePassword(str) {

    if (str.length < 6) {
      this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_NEW_PASSWD_LENGTH_RULE');
      return false;
    } else if (str.search(/\d/) == -1) {
      this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_NEW_PASSWD_NUMBER_RULE');
      return false;
    } else if (str.search(/[a-zA-Z]/) == -1) {
      this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_NEW_PASSWD_CHAR_RULE');
      return false;
    } else if (str.search(/[@#_-]/) == -1) {
      this.showError('LOGIN.MSG_PASSWD_RESULT', 'LOGIN.MSG_NEW_PASSWD_SPECIAL_CHAR_RULE');
      return false;
    }
    return true;
  }


  comparePassword(currentPwd, confirmPwd) {
    return (currentPwd == confirmPwd);
  }

  isFormValid() {
    return this.isCurrentPasswordValid()
      && this.validator.isValid('newPassword')
      && this.validator.isValid('confirmPassword')
  }

  isCurrentPasswordValid() {
    if (this.isResetPassword)
      return true;
    else
      return this.validator.isValid('currentPassword')
  }
}

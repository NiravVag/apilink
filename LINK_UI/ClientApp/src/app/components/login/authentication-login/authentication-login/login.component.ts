import { Component, Directive, ElementRef, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

import * as CryptoJS from 'crypto-js';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';

import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { EmailUserVerication, HRProfileEnum, UserType, RoleEnum, FBRedirectionType } from 'src/app/components/common/static-data-common';
import { ToastrService } from 'ngx-toastr';
import { SideBarService } from 'src/app/_Services/common/side-bar-service.service';
import { JsonHelper, Validator } from 'src/app/components/common';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ForgotPassword } from 'src/app/_Models/user/forgotpassword.model';


@Component({ templateUrl: 'login.component.html' })

export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    error = '';
    year = 0;
    languages = [{ "id": "en", "name": "English" }];// { "id": "fr", "name": "French" }
    selectedlanguage = "";
    emailUserVerication = EmailUserVerication;
    passwordicon = "";
    user: any;
    @ViewChild("txtUserName") _txtUserName: ElementRef;
    public model: ForgotPassword;
    public modelRef: NgbModalRef;
    public jsonHelper: JsonHelper;
    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        public translate: TranslateService,
        public accService: UserAccountService,
        private toastr: ToastrService,
        public validator: Validator,
        public modalService: NgbModal,

        private sideBarService: SideBarService) {
        var d = new Date();
        this.year = d.getFullYear();
        this.selectedlanguage = "en";
        this.passwordicon = "../assets/images/eye-icon.svg"
        this.validator.isSubmitted = false;
        this.validator.setJSON("user/forgotpassword.valid.json");
        this.validator.setModelAsync(() => this.model);
        this.jsonHelper = validator.jsonHelper;
        this.model = new ForgotPassword();
    }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });

        this.authenticationService.logout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    }

    ngAfterViewInit() {
        this._txtUserName.nativeElement.focus();
    }

    // convenience getter for easy access to form fields
    get f() { return this.loginForm.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }

        this.loading = true;

        var key = CryptoJS.enc.Utf8.parse('1234567891012345');
        var iv = CryptoJS.enc.Utf8.parse('1234567891012345');
        var encryptedPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(this.f.password.value.toString().trim()), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });


        this.authenticationService.login(this.f.username.value.toString().trim(), encryptedPassword.toString())
            .pipe()
            .subscribe(
                data => {
                    if (data && data.result == 1) {
                        this.processSuccessLoginData(data);
                    }
                    else {
                        this.error = "Login failure";
                        this.loading = false;
                        // TODO check error from result
                    }

                },
                error => {
                    this.error = error;
                    this.loading = false;
                });
    }

    showPassword(input: any): any {
        if (input.type === 'password') {
            input.type = 'text';
            this.passwordicon = "../assets/images/eye-close.svg"
        }
        else {
            input.type = 'password';
            this.passwordicon = "../assets/images/eye-icon.svg"
        }
    }

    //process data after successful login
    processSuccessLoginData(data) {
        //Read query string values
        this.route.queryParams.subscribe(params => {
            //if callfrom param is there then redirect fullbridge mission link
            if (params.callFrom) {
                this.navigateToFullBridge(params);
            }
            else if (this.returnUrl && this.returnUrl != '/' && this.returnUrl.includes('/customerdecision/')) {
                this.router.navigate([decodeURI(this.returnUrl)]);
            }
            //if no params value is there then actual login
            else {
                this.processActualLogin(data);
            }
        });
    }

    //based on the user given criteria navigate to full bridge  
    navigateToFullBridge(params) {
        this.user = this.authenticationService.getCurrentUser();
        if (this.user && this.user.fbUserId) {
            switch (parseInt(params.callFrom)) {
                case this.emailUserVerication.ScheduleQCEmail: {
                    this.redirectToFullBridge(params.missionId, FBRedirectionType.Mission);
                    this.loading = false;
                    break;
                }
                case this.emailUserVerication.ReportFilledToReportChecker: {
                    this.processReportFilledEmail(params);
                    break;
                }

                default:
                    {
                        this.redirectToFullBridge(params.missionId, FBRedirectionType.Mission);
                        this.loading = false;
                        break;
                    }
            }
        }
        else {
            this.showError('EMAIL_USER_VERIFICATION.TITLE', 'EMAIL_USER_VERIFICATION.MSG_FB_NO_ACCESS');
            this.loading = false;
        }
    }

    //report filled notification email to report checker
    processReportFilledEmail(params) {

        if (params.missionId)
            this.redirectToFullBridge(params.missionId, FBRedirectionType.Mission);
        if (params.reportId)
            this.redirectToFullBridge(params.reportId, FBRedirectionType.Report);

    }

    redirectToFullBridge(id, fRedirectType) {

        this.accService.getUserTokenToFB()
            .subscribe(
                response => {
                    if (response != null) {
                        if (fRedirectType == FBRedirectionType.Mission)
                            window.open(response.missionUrl + id + "?token=" + response.token + "", "_self");
                        if (fRedirectType == FBRedirectionType.Report)
                            window.open(response.reportUrl + id + "?token=" + response.token + "", "_self");
                    }
                },
                error => {
                    this.showError('EMAIL_USER_VERIFICATION.TITLE', 'EMAIL_USER_VERIFICATION.MSG_UNKNOWN_ERROR');
                }
            );

    }


    //redirect to api home page on successful login
    processActualLogin(data) {
        if (!data.user.changePassword) // change password first time
        {
            this.router.navigate(["/changepassword"]);
        }
        else {
            this.toggleSideBar();
            this.authenticationService.redirectToLanding();
        }
    }

    //call side bar service to update side bar value(that can be shared among the independent components)
    toggleSideBar() {
        this.user = this.authenticationService.getCurrentUser();
        if (this.user.usertype == UserType.Customer)
            this.sideBarService.changeToggleSideBar(true);
        else
            this.sideBarService.changeToggleSideBar(false);
    }

    languageChange(value) {
        if (value)
            this.translate.use(value.id);
    }
    public showError(title: string, msg: string, _disableTimeOut?: boolean) {
        let tradTitle: string = "";
        let tradMessage: string = "";

        this.translate.get(title).subscribe((text: string) => { tradTitle = text });
        this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

        this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
    }

    public showSuccess(title: string, msg: string, _disableTimeOut?: boolean) {
        let tradTitle: string = "";
        let tradMessage: string = "";

        this.translate.get(title).subscribe((text: string) => { tradTitle = text });
        this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

        this.toastr.success(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
    }

    openForgotPasswordPopUp(forgotPassword) {
        this.loginForm.markAsUntouched();
        this.validator.isSubmitted = false;
        this.model.userName = "";
        this.modelRef = this.modalService.open(forgotPassword, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
    }

    //check hold booking reason is valid or mandatory
    isuserNameValid() {
        this.validator.initTost();
        this.validator.isSubmitted = true;
        return this.validator.isValid("userName");
    }

    forGotConfirm() {
        if (this.isuserNameValid()) {
            this.accService.forgotpassword(this.model.userName).subscribe(
                response => {
                    if (response && response.result == 1) {
                        this.showSuccess('LOGIN.LBL_RESET_PASSWORD_MAIN', 'LOGIN.MSG_EMAIL_RESET_PASSWORD');
                        this.modelRef.close();
                    } else {
                        this.showError('LOGIN.FORGOT_PASSWORD_TITLE', 'LOGIN.MSG_FORGOTPASSWORD_NOTEXIST');
                    }
                },
                error => {
                    this.showError('LOGIN.FORGOT_PASSWORD_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                }
            );
        }

    }
}



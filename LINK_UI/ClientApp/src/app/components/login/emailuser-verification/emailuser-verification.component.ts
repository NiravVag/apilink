import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { EmailUserVerication, FBRedirectionType } from '../../../components/common/static-data-common';
import { ToastrService } from "ngx-toastr";

@Component({
    selector: 'app-emailuserverification',
    templateUrl: './emailuser-verification.component.html',
    styleUrls: ['./emailuser-verification.component.scss']
})

export class EmailUserVerificationComponent implements OnInit {

    public emailUserVerication = EmailUserVerication;
    public currentUser: any;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        public translate: TranslateService,
        public accService: UserAccountService,
        public authService: AuthenticationService,
        private toastr: ToastrService
    ) { }

    ngOnInit() {
        this.VerifyUserEmailData();
    }

    //verify and process data coming from the external email
    VerifyUserEmailData() {
        this.route.queryParams.subscribe(params => {
            if (params.callFrom) {
                switch (parseInt(params.callFrom)) {
                    case this.emailUserVerication.ScheduleQCEmail:
                        this.processScheduleQCEmail(params.missionId, params.callFrom);
                        break;
                    case this.emailUserVerication.ReportFilledToReportChecker:
                        this.processReportFilledEmail(params);
                        break;
                    default:
                        this.processScheduleQCEmail(params.missionId, params.callFrom);
                        break;
                }
            }
        });
    }

    //check if session is available redirect to fullbridge else to login page
    processScheduleQCEmail(missionId, callFrom) {
        this.currentUser = this.authService.getCurrentUser();
        if (this.currentUser && this.currentUser.fbUserId) {
            this.redirectToFullBridge(missionId, FBRedirectionType.Mission);
        }
        else {
            this.router.navigate(['/login'], { queryParams: { missionId: missionId, callFrom: callFrom } });
        }
    }

    //process the report filled email
    processReportFilledEmail(params) {
        this.currentUser = this.authService.getCurrentUser();
        if (this.currentUser && this.currentUser.fbUserId) {
            if (params.missionId)
                this.redirectToFullBridge(params.missionId, FBRedirectionType.Mission);
            if (params.reportId)
                this.redirectToFullBridge(params.reportId, FBRedirectionType.Report);
        }
        else {
            if (params.missionId && params.callFrom)
                this.router.navigate(['/login'], { queryParams: { missionId: params.missionId, callFrom: params.callFrom } });
            else if (params.reportId && params.callFrom)
                this.router.navigate(['/login'], { queryParams: { reportId: params.reportId, callFrom: params.callFrom } });
        }
    }

    //redirect to fullbridge mission/report page
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

    //error showing function.
    public showError(title: string, msg: string, _disableTimeOut?: boolean) {
        let tradTitle: string = "";
        let tradMessage: string = "";

        this.translate.get(title).subscribe((text: string) => { tradTitle = text });
        this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

        this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
    }
}

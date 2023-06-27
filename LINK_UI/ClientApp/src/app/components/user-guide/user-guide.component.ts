import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserGuideDetail, UserGuideDetailResponse, UserGuideDetailResult, UserGuideMaster } from 'src/app/_Models/userguide/userguide.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserGuideService } from 'src/app/_Services/userguide/userguide.service';

@Component({
    selector: 'app-user-guide',
    templateUrl: './user-guide.component.html',
    styleUrls: ['./user-guide.component.scss']
})
export class UserGuideComponent implements OnInit {

    userGuideMaster: UserGuideMaster;
    userGuideDetailResponse: UserGuideDetailResponse;
    moduleId: number;
    componentDestroyed$: Subject<boolean> = new Subject();

    constructor(public userGuideService: UserGuideService, private translate: TranslateService,public utility: UtilityService,
         private toastr: ToastrService) { }

    //intialize the object
    ngOnInit() {
        this.userGuideMaster = new UserGuideMaster();
        this.userGuideDetailResponse = new UserGuideDetailResponse();
        //add the image icon list
        this.addmoduleImageList();
        //get the user guide details
        this.getUserGuideDetails();
    }

    ngOnDestroy(): void {
        this.componentDestroyed$.next(true);
        this.componentDestroyed$.complete(); 
      }

    //add the module image list
    addmoduleImageList() {
        this.userGuideMaster.moduleImageList.push("assets/images/setting-icon1.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-light-green.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-magenta.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-brown.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-peach.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-green.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-dark-green.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-light-purple.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-dark-purple.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-lemon.svg");
        this.userGuideMaster.moduleImageList.push("assets/images/setting-dark-blue.svg");
    }

    //get the user guide details
    async getUserGuideDetails() {
        try {
            //get the user guide detail data
            this.userGuideMaster.userGuideDetailLoading = true;
            this.userGuideDetailResponse = await this.userGuideService.getUserGuideDetails();
            this.userGuideMaster.userGuideDetailLoading = false;
        }
        catch (e) {
            this.showError('USER_GUIDE.TITLE', "USER_GUIDE.MSG_UNKNOWN_ERROR");
            this.userGuideMaster.userGuideDetailLoading = false;
        }
        //if response is success
        if (this.userGuideDetailResponse.result == UserGuideDetailResult.Success) {
            //if user guide details is available
            if (this.userGuideDetailResponse.userGuideDetails && this.userGuideDetailResponse.userGuideDetails.length > 0) {
                //assign the userguidedetails
                this.userGuideMaster.userGuideDetails = this.userGuideDetailResponse.userGuideDetails;
                //map the module icon images
                this.mapModuleIcon();
            }
        }
        else if (this.userGuideDetailResponse.result == UserGuideDetailResult.NotFound) {
            this.showError('USER_GUIDE.TITLE', "USER_GUIDE.MSG_USER_GUIDE_NOT_FOUND");
        }
    }

    //map the module icon images
    mapModuleIcon() {
        var index = 0;
        var iconLength = 10;
        this.userGuideMaster.userGuideDetails.forEach(element => {
            //if index reaches the iconlength(10) and reset the index to 0 to start using the icon image from the start
            if (index == iconLength)
                index = 0;
            element.imageIcon = this.userGuideMaster.moduleImageList[index];
            index = index + 1;
        });
    }

    //change the module list
    changeModuleList(event) {
        if (event && event.id)
            this.userGuideMaster.userGuideDetails = this.userGuideDetailResponse.userGuideDetails.filter(x => x.id == event.id);
        else
            this.userGuideMaster.userGuideDetails = this.userGuideDetailResponse.userGuideDetails

    }

    //clear the module list
    clearModuleList() {
        this.userGuideMaster.userGuideDetails = this.userGuideDetailResponse.userGuideDetails;
    }

    //download the user guide detail
    downloaduserGuideDetail(item: UserGuideDetail) {
        if (item) {
            this.userGuideService.downloadFile(item.id)
                .pipe(takeUntil(this.componentDestroyed$))
                .subscribe(res => {
                    if (res)
                        this.downloadFile(res, "application/pdf", item.name + ".pdf");
                    else
                        this.showError('USER_GUIDE.TITLE', "USER_GUIDE.MSG_FILE_NOT_FOUND");
                },
                    error => {
                        this.showError('USER_GUIDE.TITLE', "USER_GUIDE.MSG_UNKNOWN_ERROR");
                    });
        }

    }
    //download the file using byte data
    downloadFile(data, mimeType, filename) {
        const blob = new Blob([data], { type: mimeType });
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
        }
        else {
            const url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = filename ? filename : "export";//url.substr(url.lastIndexOf('/') + 1);
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }
    }

    //show error message
    public showError(title: string, msg: string, _disableTimeOut?: boolean) {
        let tradTitle: string = "";
        let tradMessage: string = "";

        this.translate.get(title).subscribe((text: string) => { tradTitle = text });
        this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

        this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
    }
}

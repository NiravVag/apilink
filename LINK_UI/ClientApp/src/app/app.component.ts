import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { UserType } from './components/common/static-data-common';
import { UserModel } from './_Models/user/user.model';
import { SideBarService } from './_Services/common/side-bar-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  sideBarRequired: boolean = true;
  user: UserModel;

  constructor(public translate: TranslateService, public sideBarService: SideBarService) {
    translate.addLangs(['en', 'fr']);
    translate.use('en');
    this.triggerToggleSideBar();
  }

  ngOnInit() {
    this.sideBarService.sideBarRequired.subscribe((data) => {
      this.sideBarRequired = data;
    });
  }

  triggerToggleSideBar(){
    //if user is available update the toggle side bar
    if (localStorage.getItem('currentUser')) {
      this.user = JSON.parse(localStorage.getItem('currentUser'));
      if (this.user.usertype == UserType.Customer)
        this.sideBarService.changeToggleSideBar(true);
      else
        this.sideBarService.changeToggleSideBar(false);
    }
  }
}



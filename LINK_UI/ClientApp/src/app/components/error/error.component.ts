import { Component, NgModule } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NetworkCheckService } from '../common/network-check';

@Component({
  selector: 'app-Error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})

export class ErrorComponent {

  title: string;
  message: string;
  returnUrl: string;
  public _image: string;
  public _IsOnline: boolean;
  constructor(private translate: TranslateService, private route: ActivatedRoute, private router: Router,networkservice:NetworkCheckService) {
    networkservice.createOnline$().subscribe(x => this._IsOnline = x);
    if (this._IsOnline) {
      let id = route.snapshot.paramMap.get("id");
      this.returnUrl = route.snapshot.queryParams['returnUrl'] || '/';
      this._image = id == '404' ? "assets/images/404.svg" : id == '401' ? "assets/images/went-wrong.svg" : "assets/images/no-data.svg"
      if (id == '404') {
        this.translate.get('ERROR.MSG_TITLE_404').subscribe((text: string) => { this.title = text });
        this.translate.get('ERROR.MSG_MESSAGE_404').subscribe((text: string) => { this.message = text });
      }
      else if (id == '401') {
        // this.translate.get('ERROR.MSG_TITLE_401').subscribe((text: string) => { this.title = text });
        //this.translate.get('ERROR.MSG_MESSAGE_401').subscribe((text: string) => { this.message = text });
        window.location.replace("/login");//refresh the page.
      }
      else {
        this.translate.get('ERROR.MSG_TITLE_GEN').subscribe((text: string) => { this.title = text });
        this.translate.get('ERROR.MSG_MESSAGE_GEN').subscribe((text: string) => { this.message = text });
      }
    }
  }
  RedirectToDashBoard() {
    window.location.replace(this.returnUrl ? this.returnUrl : "./");
  }
}

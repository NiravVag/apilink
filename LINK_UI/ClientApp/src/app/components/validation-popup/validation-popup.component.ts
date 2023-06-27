import { Component, OnInit, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
 

@Component({
  selector: 'app-validation-popup',
  templateUrl: './validation-popup.component.html' 
})
export class ValidationPopupComponent implements OnInit {
  private _translate: TranslateService;
  constructor(translate: TranslateService,public activeModal: NgbActiveModal) 
  {
    this._translate = translate;
   }

   @Input() messages=[];
   @Input() title:string;
   @Input() buttonText:string;


   ngOnInit() {
     
    }
    closeModal() 
  {
    this.activeModal.close();
  }
}

import { Component, Input,Output,EventEmitter, OnInit,ViewChildren,AfterViewInit,QueryList  }  from '@angular/core';
import { FormGroup,FormControl, Validators }                 from '@angular/forms';

import { DynamicControlBase } from '../../../_Models/dynamiccontrols/dynamicontrolbase.model';

import { DynamicControlService } from '../../../_Services/dynamiccontrols/dynamic-control.service'

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamicform.component.html',
  providers: [ DynamicControlService ]
})
export class DynamicFormComponent implements OnInit {

  @Input() dynamicControls: DynamicControlBase<any>[] = [];
  @Input() dynamicControlBaseData:any;

  @Output()
  submitted: EventEmitter<any> = new EventEmitter();
  
  form: FormGroup;
  payLoad = '';

  constructor(private dynamicControlService: DynamicControlService) {  
  }

  ngOnInit() {
    this.form = this.dynamicControlService.toFormGroup(this.dynamicControls);
    /* var frmControl=this.form.get('1') as FormControl
    frmControl.setValidators([Validators.required,Validators.email]); */
  }

  /* ngAfterViewInit() {
    console.log(this.dynamicControls);

   this.dynamicControls.forEach(element => {
    if (element.controlType == "dropdown") {
        element.controlAttributeList.forEach(element => {
        this.dropdown.forEach((t, index) => {
          //t.element.setAttribute(element.key.toString(), element.value.toString());
          t.element.setAttribute("multiple", "false");
        });
      });
    }
   });
  } */

  /* onSubmit() {
    this.payLoad = JSON.stringify(this.form.value);
  } */
}

import { Injectable } from '@angular/core';
import { DynamicControlBase } from '../../_Models/dynamiccontrols/dynamicontrolbase.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ControlAttribute } from '../../components/common/static-data-common';
@Injectable()
export class DynamicControlService {
  _controlAttribute=ControlAttribute;
  constructor() { }

  toFormGroup(dynamiccontrols: DynamicControlBase<any>[]) {

    /*  this.registerForm = this.formBuilder.group({
       firstName: ['', Validators.required],
       lastName: ['', Validators.required],
       email: ['', [Validators.required, Validators.email]],
       password: ['', [Validators.required, Validators.minLength(6)]]
     });
  */
    let group: any = {};

    dynamiccontrols.forEach(dynamiccontrol => {
    /*   group[dynamiccontrol.key] = dynamiccontrol.required ? new FormControl(dynamiccontrol.value || '', [Validators.required,Validators.email])
        : new FormControl(dynamiccontrol.value || ''); */
        /* var requiredValidation = false;
        var requiredAttribute = dynamiccontrol.controlAttributeList.filter(x => x.attributeid == this._controlAttribute.RequiredValidation); */
        group[dynamiccontrol.key] = new FormControl(dynamiccontrol.value || '');
    });
    return new FormGroup(group);
  }
}


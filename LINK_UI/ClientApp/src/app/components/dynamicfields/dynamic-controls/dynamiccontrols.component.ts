import { Component, Input, ViewChildren, AfterViewInit, QueryList, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DynamicControlBase, ControlAttributes } from '../../../_Models/dynamiccontrols/dynamicontrolbase.model';
import { ControlAttribute } from '../../common/static-data-common'

@Component({
  selector: 'app-dynamic-controls',
  templateUrl: './dynamiccontrols.component.html'
})
export class DynamicFormControlComponent {
  @Input() dynamiccontrol: DynamicControlBase<any>;
  @Input() dynamiccontrols: any;
  @Input() form: FormGroup;
  @ViewChildren('textbox') txtBox: any;
  @ViewChildren('datetimepicker') datetimepicker: any;
  @ViewChildren('dropdown') dropdown: any;
  @ViewChildren('textarea') textarea: any;
  disabled: boolean = true;
  _controlAttribute = ControlAttribute;

  ngOnInit() {
    /*
    console.log(this.dynamiccontrol);
    var requiredValidation = false;
    var emailValidation = false;
    var requiredAttribute = this.dynamiccontrol.controlAttributeList.filter(x => x.attributeid == this._controlAttribute.RequiredValidation);
    if (requiredAttribute) {
      requiredValidation = requiredAttribute.length > 0 ? requiredAttribute[0].value == "true" ? true : false : false;
    }
    var emailAttribute = this.dynamiccontrol.controlAttributeList.filter(x => x.attributeid == this._controlAttribute.EmailValidation);
    if (emailAttribute) {
      emailValidation = emailAttribute.length > 0 ? emailAttribute[0].value == "true" ? true : false : false;
    }

    if (requiredValidation && emailValidation) {
      var ctrl = this.form.controls[this.dynamiccontrol.key] as FormControl;
      ctrl.setValidators([Validators.required, Validators.email]);
    }
    else if (requiredValidation) {
      var ctrl = this.form.controls[this.dynamiccontrol.key] as FormControl;
      ctrl.setValidators([Validators.required]);
    }
    else if (emailValidation) {
      var ctrl = this.form.controls[this.dynamiccontrol.key] as FormControl;
      ctrl.setValidators([Validators.email]);
    } */
  }

  ngAfterViewInit() {

  }

  get isValid() { return this.form.controls[this.dynamiccontrol.key].valid; }

  getattributeValue(controlAttributeId) {
    var controlAttribute = this.dynamiccontrol.controlAttributeList.find(x => x.attributeid == controlAttributeId);
    return controlAttribute.value;
  }
  changeDropDown(event, dynamiccontrol) {
    if (dynamiccontrol.required && !event) 
      dynamiccontrol.requiredNotFound = true;
    else 
      dynamiccontrol.requiredNotFound = false;

    if (this.dynamiccontrol.controlTypeId == 3) {
      var dropDowns = [];
      this.dynamiccontrols.forEach(control => {
        control.controlAttributeList.forEach(atrribute => {
          if (atrribute.attributeid == this._controlAttribute.ParentDropDown && atrribute.value == this.dynamiccontrol.dataSourceType) {
            if (this.dynamiccontrol.value) {
              var dropDownOptions = control.dataSource.filter(x => x.parentId == this.dynamiccontrol.value);
              control.value = null;
              control.options = dropDownOptions;
            }
            else {
              var dropDownOptions = control.dataSource;
              control.options = dropDownOptions;
            }

          }

        });
      });

    }
  }

  /* getValidator(required, value,submitted) {
    if (required && submitted && !value)
      return true;
    else
      return false;

  } */
  getEmailValidator(required, value,submitted) {
    if (required && submitted && !value)
      return true;
    else
      return false;

  }
}

import { Injectable }       from '@angular/core';

import { DynamicControlBase } from '../../_Models/dynamiccontrols/dynamicontrolbase.model';
import { TextboxControl }     from '../../_Models/dynamiccontrols/control-textbox.model';
import { DropDownControl }  from '../../_Models/dynamiccontrols/control-dropdown.model';
import { DateTimePickerControl } from 'src/app/_Models/dynamiccontrols/control-datepicker.model';

@Injectable({ providedIn: 'root' })
export class ControlService {

  // TODO: get from a remote source of question metadata
  // TODO: make asynchronous
  getQuestions() {
    let questions: DynamicControlBase<any>[] = [

      new DropDownControl({
        key: 'brave',
        label: 'Bravery Rating',
        options: [
          {key: 'solid',  value: 'Solid'},
          {key: 'great',  value: 'Great'},
          {key: 'good',   value: 'Good'},
          {key: 'unproven', value: 'Unproven'}
        ],
        order: 3,
        multiple:"true"
      }),

      new DateTimePickerControl({
        key: 'firstName',
        label: 'First name',
        value: 'Bombasto',
        required: true,
        order: 1
      }),

      new DateTimePickerControl({
        key: 'emailAddress',
        label: 'Email',
        type: 'text',
        required: true,
        order: 2
      }),
      new TextboxControl({
        key: 'dob',
        label: 'DOB',
        type: 'date',
        required: true,
        order: 4
      })
    ];

    return questions.sort((a, b) => a.order - b.order);
  }
}

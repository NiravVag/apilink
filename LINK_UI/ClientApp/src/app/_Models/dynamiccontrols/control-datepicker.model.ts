import { DynamicControlBase } from './dynamicontrolbase.model';

export class DateTimePickerControl extends DynamicControlBase<string> {
  controlType = 'datetimepicker';
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options['type'] || '';
  }
}

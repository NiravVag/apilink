import { DynamicControlBase } from './dynamicontrolbase.model';

export class MultipleDropDown extends DynamicControlBase<string> {
  controlType = 'multipledropdown';
  options: {key: string, value: string}[] = [];
  multiple="true";

  constructor(options: {} = {}) {
    super(options);
    this.options = options['options'] || [];
  }
}

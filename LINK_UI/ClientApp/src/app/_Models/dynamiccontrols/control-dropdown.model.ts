import { DynamicControlBase } from './dynamicontrolbase.model';

export class DropDownControl extends DynamicControlBase<string> {
  controlType = 'dropdown';
  options: {key: string, value: string}[] = [];
  dataSource: {key: string, value: string}[] = [];

  constructor(options: {} = {}) {
    super(options);
    this.options = options['options'] || [];
    this.dataSource = options['options'] || [];
  }
}

import { DynamicControlBase } from './dynamicontrolbase.model';

export class TextboxControl extends DynamicControlBase<string> {
  controlType = 'textbox';
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options['type'] || '';
  }
}

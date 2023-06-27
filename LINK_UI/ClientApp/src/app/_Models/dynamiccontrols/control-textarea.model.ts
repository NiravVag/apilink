import { DynamicControlBase } from './dynamicontrolbase.model';

export class TextAreaControl extends DynamicControlBase<string> {
  controlType = 'textarea';
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options['type'] || '';
  }
}

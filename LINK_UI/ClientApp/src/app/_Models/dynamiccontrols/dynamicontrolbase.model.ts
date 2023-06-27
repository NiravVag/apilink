export class DynamicControlBase<T> {
  value: T;
  key: string;
  label: string;
  required:boolean;
  requiredNotFound:boolean
  emailNotValid:boolean;
  order: number;
  controlType: string;
  controlTypeId:number;
  controlConfigurationId:number;
  controlAttributeList: Array<ControlAttributes>;
  dataSourceType:number;

  constructor(options: {
    value?: T,
    key?: string,
    label?: string,
    required?: boolean,
    order?: number,
    controlType?: string,
    controlTypeId?: number,
    controlConfigurationId?: number,
    dataSourceType?:number
  } = {}) {
    this.value = options.value;
    this.key = options.key || '';
    this.label = options.label || '';
    this.order = options.order === undefined ? 1 : options.order;
    this.controlType = options.controlType || '';
    this.controlTypeId = options.controlTypeId === undefined ? 1 : options.controlTypeId;
    this.controlConfigurationId = options.controlConfigurationId === undefined ? 1 : options.controlConfigurationId;
    this.controlAttributeList = [];
    this.dataSourceType=options.dataSourceType;
  }


}

export class ControlAttributes {
  public id: number;
  public attributeid:number;
  public key: number;
  public value: string;
  public active: boolean
}

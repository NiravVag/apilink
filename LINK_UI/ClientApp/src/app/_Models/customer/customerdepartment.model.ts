export class CustomerDepartmentmodel
{
    public customerValue:number;
    public departmentList:Array<departmentmodel>=[];
}

export class departmentmodel{
    public id:number;
    public name:string;
    public code:string;
}

export class customerDepartmentToRemove {
    constructor() {
    }
    id: number;
    name: string;
  }
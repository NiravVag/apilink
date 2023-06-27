import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dataFieldAccessPipe'
})
export class DataFieldAccessPipe implements PipeTransform {
  transform(fieldId: number,dataFieldAccess): boolean {

    if(dataFieldAccess){
      var dataField = dataFieldAccess.find(x => x.id == fieldId);
      return dataField ? true : false;
    }
    return false;
  }
}

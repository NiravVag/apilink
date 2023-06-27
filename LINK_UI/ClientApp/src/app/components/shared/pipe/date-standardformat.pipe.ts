import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'dateStandardFormatPipe'
  })
  export class DateStandardFormatPipe implements PipeTransform {
    transform(dateInput: any): string {
      if(dateInput)
      {
        return dateInput.day + "/" + dateInput.month + "/" + dateInput.year;
      }
      return "";   
    }
  }

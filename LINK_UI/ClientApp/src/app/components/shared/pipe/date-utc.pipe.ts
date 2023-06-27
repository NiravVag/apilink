import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'dateUTCPipe'
  })
  export class DateUTCPipe implements PipeTransform {
    transform(utcDate: string): string {
      if(utcDate)
      {
        let dateParts = utcDate.split("/");
        let newdate = +dateParts[1] +"-"+dateParts[0]+"-" +dateParts[2];
        return new Date(newdate + ' UTC').toString();
      }
      return "";   
    }
  }

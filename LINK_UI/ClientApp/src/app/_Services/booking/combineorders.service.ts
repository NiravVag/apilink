import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, first } from 'rxjs/operators';
import { SaveCombineOrdersRequest} from 'src/app/_Models/booking/bookingcombineorders.model';
@Injectable({
    providedIn: 'root'
  })
  export class CombineOrdersService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      this.url = baseUrl;
  
      if (this.url.charAt(this.url.length - 1) == '/')
        this.url = this.url.substring(0, this.url.length - 1);
    }

    getBookingOrders(id:number) {  
      return this.http.get<any>(`${this.url}/api/CombineOrder/${id}`);
    }

    getBookingPoDetails(bookingId:number,productRefId:number) {  
      return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingPoListbyProduct/${bookingId}/${productRefId}`);
    }

    getSamplingQuantityByProductList(bookingID,productList) {
      return this.http.post<any>(`${this.url}/api/combineorder/getSamplingQuantityByListOfProducts/${bookingID}`, productList )
        .pipe(map(response => {
          return response;
        }));
    }

    getSamplingQuantity(bookingID,request,aqlId) {
      return this.http.post<any>(`${this.url}/api/combineorder/getsamplingQuantity/${bookingID}/${aqlId}`, request )
        .pipe(map(response => {
          return response;
        }));
    }

    exportSummary(id:number) {
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      });
      return this.http.post<Blob>(`${this.url}/api/combineorder/ExportCombineOrders/${id}`,null,
       { headers: headers, responseType: 'blob' as 'json' });
    }

    saveCombineOrder(model: Array<SaveCombineOrdersRequest>,inspectionID) {
      return this.http.post<any>(`${this.url}/api/combineorder/savecombineOrders/${inspectionID}`, model)
        .pipe(map(response => {
          return response;
        }));
    }

  }

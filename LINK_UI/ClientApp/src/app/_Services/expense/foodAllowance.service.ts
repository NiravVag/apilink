import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first, catchError } from 'rxjs/operators';
import { ExpenseClaimModel, ExpenseClaimResponse, ExpenseClaimReceipt } from '../../_Models/expense/expenseclaim.model';
import { Observable } from 'rxjs';
import { FoodAllowanceEditModel, FoodAllowanceModel } from '../../_Models/expense/foodallowance.model';

@Injectable({ providedIn: 'root' })
export class FoodAllowanceService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getFoodAllowanceSummary(model: FoodAllowanceModel) {
    return this.http.post<any>(`${this.url}/api/foodallowance/get_food_allowance`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  save(model: FoodAllowanceEditModel) {
    if (model && model.id > 0) {
      return this.http.post<any>(`${this.url}/api/foodAllowance/update_food_allowance`, model)
        .pipe(map(response => {
          return response;
        }));
    }
    else {
        return this.http.post<any>(`${this.url}/api/foodAllowance/save_food_allowance`, model)
          .pipe(map(response => {
            return response;
          }));
    }
  }

  editFoodAllowance(id: number) {
    return this.http.get<any>(`${this.url}/api/foodallowance/get_food_allowance_by_id/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  
  deleteFoodAllowance(id: number) {
    return this.http.post<any>(`${this.url}/api/foodallowance/delete_food_allowance`, id)
      .pipe(map(response => {
        return response;
      }));
  }
}

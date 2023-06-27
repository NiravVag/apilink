import { Component } from '@angular/core';
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CurrencyService } from '../../../_Services/currency/currency.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SummaryComponent } from '../../common/summary.component';
import { RateMatrixModel } from '../../../_Models/currency/ratematrix.model';
import { Validator } from '../../common/validator'
import { Currency } from '../../../_Models/currency/echange-rate.model';
import { NgbModalRef, NgbModal, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { CurrencyItem } from '../../../_Models/currency/ratematrix.data.model';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-ratematrix',
  templateUrl: './ratematrix.component.html',
  styleUrls: ['./ratematrix.component.css'],
  animations: [
    trigger('expandCollapse', [
        state('open', style({
            'height': '*',
            'opacity': 1,
            'padding-top': '24px',
            'padding-bottom': '24px'
        })),
        state('close', style({
            'height': '0px',
            'opacity': 0,
            'padding-top': 0,
            'padding-bottom': 0
        })),
        transition('open <=> close', animate(300))
    ])
]
})

export class RateMatrixComponent extends SummaryComponent<RateMatrixModel> {

  public data: any;
  public dataItems: Array<CurrencyItem>;
  public isSearch: boolean = false;
  public monthsInterval: Array<any>;
  private currentRoute: ActivatedRoute;
  public fromSummary: boolean = false;
  public paramParent: string;
  private _router: Router;
  private _route: ActivatedRoute
  public _searchloader:boolean=false;
  public months: Array<any> = [
    { id: 1, name: 'January' },
    { id: 2, name: 'February' },
    { id: 3, name: 'March' },
    { id: 4, name: 'April' },
    { id: 5, name: 'May' },
    { id: 6, name: 'June' },
    { id: 7, name: 'July' },
    { id: 8, name: 'August' },
    { id: 9, name: 'September' },
    { id: 10, name: 'October' },
    { id: 11, name: 'November' },
    { id: 12, name: 'December' }];
  public years: Array<any>;

  loading = false;
  error = '';
  public exportDataLoading = false;
  isFilterOpen: boolean;

  constructor(private service: CurrencyService, public validator: Validator, router: Router, 
    route: ActivatedRoute,   public utility: UtilityService,
    private modalService: NgbModal, private calendar: NgbCalendar,translate: TranslateService) {
    super(router, validator, route,translate);
    this.currentRoute = route; 
    this.data = [];
    this.model = new RateMatrixModel();
    this.validator.setJSON("currency/rate-matrix.valid.json");
    this.validator.isSubmitted = false;
    this.validator.setModelAsync(() => this.model);
    this._router = router;
    this._route = route; 
    this.isFilterOpen = true;
  }

  onInit() {

    this.setYears();
this.loading=true;
    this.service.getExchangeRateSummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;

            if (this.setDefaultParams())
              this.refresh(); 
          }
          else if (data && data.result == 2) {
          }
          else {
            this.error = data.result;
           
            // TODO check error from result
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
      });

    this._route.queryParams.subscribe(
      params => {
        //// (+) before `params.get()` turns the string into a number
        //this.selectedId = +params.get('id');
        //return this.service.getHeroes();
        if (params != null && params['paramParent'] != null) {
          this.paramParent = params['paramParent'];
          this.fromSummary = true;
        }

      }
    );
  }

  setYears() {
    let today = this.calendar.getToday();
    let year = today.year - 10;

    this.years = [];

    for (var i = year; i <= year+20; i++) {
      this.years.push({ name: i });
    }

  }

  getPathDetails(): string {
    return "";
  }

  setDefaultParams() : boolean {

    let existsParams: boolean = true;

    let id = this.currentRoute.snapshot.paramMap.get("id");
    if (id != null)
      this.model.currency = this.data.currencyList.filter(x => x.id == id)[0];
    else
      existsParams = false;

    let fromDate = this.currentRoute.snapshot.paramMap.get("from");

    if (fromDate != null) {
      var tab = fromDate.split("-");
      this.model.fromDate = { year: +tab[2], month: +tab[1], day: +tab[0] };
    }
    else {
      this.model.fromDate = this.calendar.getToday();
      existsParams = false; 
    }

    let toDate = this.currentRoute.snapshot.paramMap.get("to");
    if (toDate != null)  {
      var tab2 = toDate.split("-");
      this.model.toDate = { year: +tab2[2], month: +tab2[1], day: +tab2[0] };
    }
    else {
      this.model.toDate = this.calendar.getToday();
      existsParams = false; 
    }

    let type = this.currentRoute.snapshot.paramMap.get("type");
    if (type != null)
      this.model.exchangeType = this.data.rateTypeList.filter(x => x.id == type)[0];
    else
      existsParams = false;

    return existsParams; 
  }

  getData() {
    this.isSearch = false; 
    this._searchloader=true;
    this.service.getDataMatrix(this.model)
      .pipe()
      .subscribe(
        response => {
      
          if (response && response.result == 1) {
            this.isSearch = true;
            this.dataItems = response.data;
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
          this._searchloader=false;
        },
        error => {
          this._searchloader=false;
          this.setError(error);
          this.loading = false;
        });
  }

  getBackgroundColor(item1: Currency, item2:Currency) {
    if (item1.id == item2.id)
      return '#DFF7F4';
    else
      return "";
  }

  getBorderwidth(item1: Currency, item2: Currency) {
    if (item1.id == item2.id)
      return '2px';
    else
      return "";
  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
    this.exportDataLoading = false;
  }

  export() {
    this.exportDataLoading = true;
    let fromDate = `${this.model.fromDate.day}-${this.model.fromDate.month}-${this.model.fromDate.year}`;
    let todate = `${this.model.toDate.day}-${this.model.toDate.month}-${this.model.toDate.year}`;

    this.service.getFile(this.model.currency.id, fromDate, todate, this.model.exchangeType.id)
        .subscribe(res => {
          this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        },
      error => {
        this.exportDataLoading = false;
      });
  }

  public return() {
    let entity: string = this.utility.getEntityName();
    this._router.navigate([`/${entity}/exchangerate/edit-exchange`], {queryParams:{ param: this.paramParent }});
  }

  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "FromDate": { 
        this.model.fromDate=null;
        break; 
     } 
     case "ToDate": { 
      this.model.toDate=null;
        break; 
     } 
    }
  }

}

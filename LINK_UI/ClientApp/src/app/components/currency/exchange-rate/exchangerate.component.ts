import { Component } from '@angular/core';
import { first } from 'rxjs/operators';
import { CurrencyService } from '../../../_Services/currency/currency.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SummaryComponent } from '../../common/summary.component';
import { CurrencyRateModel } from '../../../_Models/currency/currencyrate.model';
import { Validator } from '../../common/validator'
import { CurrencyRateItem, Currency, RateItem } from '../../../_Models/currency/echange-rate.model';
import { NgbModalRef, NgbModal, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-exchangeRate',
  templateUrl: './exchangerate.component.html',
  styleUrls: ['./exchangerate.component.css']
})

export class ExchangeRateComponent extends SummaryComponent<CurrencyRateModel> {

  public data: any;
  public dataItems: Array<CurrencyRateItem>;
  public currencyList: Array<Currency>;
  public currenciesToAdd: Array<Currency>;
  public currentCurrencies: Array<Currency>;
  private modelRef: NgbModalRef;
  public isSearch: boolean = false;
  public monthsInterval: Array<any>;
  private months: Array<string> = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  loading = false;
  error = '';
  private onSave: boolean = false;

  public styleOverFlow: string = "auto";


  constructor(private service: CurrencyService, public validator: Validator, router: Router, route: ActivatedRoute,
    private modalService: NgbModal, private calendar: NgbCalendar, translate: TranslateService,  public utility: UtilityService,
    toastr: ToastrService) {
    super(router, validator, route,translate, toastr);

    this.data = [];
    this.model = new CurrencyRateModel();
    this.validator.setJSON("currency/exchange-rate.valid.json");
    this.validator.isSubmitted = false;
    this.validator.setModelAsync(() => this.model);
  }

  onInit() {
    this.service.getExchangeRateSummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;

          }
          else if (data && data.result == 2) {
          }
          else {
            this.error = data.result;
            this.loading = false;
            // TODO check error from result
          }
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getPathDetails(): string {
    return "";
  }

  getData() {

    this.dataItems = [];
    this.currencyList = [];

    this.service.getDataRate(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.isSearch = true;
            this.dataItems = response.data;
            this.currencyList = response.currencyList;


            this.currenciesToAdd = this.data.currencyList.filter(x => !this.currencyList.some(y => y.id == x.id) && x.id != this.model.currency.id);
            //this.setMonthsInterval(this.model.fromDate, this.model.toDate);
          }
          else if (response && response.result == 2) {
            this.isSearch = true;
            this.currenciesToAdd = this.data.currencyList.filter(x => x.id != this.model.currency.id);
            this.model.noFound = true;
            //  this.setMonthsInterval(this.model.fromDate, this.model.toDate);
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getItem(itemInfo: CurrencyRateItem, itemCurrency: Currency): RateItem {

    var item = itemInfo.rateList.filter(x => x.currencyId == itemCurrency.id);

    if (item.length > 0)
      return item[0];

    let newitem: RateItem = {
      currencyId: itemCurrency.id,
      value: 0,
      conversionId: 0
    };

    itemInfo.rateList.push(newitem);

    return newitem;
  }

  addCurrency() {

    if (this.currentCurrencies != null && this.currentCurrencies.length > 0) {

      if (this.currencyList == null)
        this.currencyList = [];

      for (let item of this.currentCurrencies) {
        this.currencyList.push(item);

        for (let element of this.dataItems) {
          if (element.rateList == null)
            element.rateList = [];

          element.rateList.push({
            currencyId: item.id,
            value: 0,
            conversionId: 0
          });
        }
      }

      this.currenciesToAdd = this.data.currencyList.filter(x => !this.currencyList.some(y => y.id == x.id) && x.id != this.model.currency.id);
      this.modelRef.close();
      this.currentCurrencies = null;
    }
  }

  openAddCurrency(content) {

    this.modelRef = this.modalService.open(content, { windowClass : "mdModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  addExchange(iteminfo: CurrencyRateItem) {


    let item: CurrencyRateItem = {
      beginDate: {},
      endDate: {},
      rateList: [],
      isNew: true
    };
    if (iteminfo != null) {
      for (let element of iteminfo.rateList) {
        item.rateList.push({
          currencyId: element.currencyId,
          value: element.value,
          conversionId: 0
        });
      }
    }

    this.model.noFound = false;
    this.dataItems.splice(0, 0, item);
    this.dataItems = [...this.dataItems];
  }

  getCurrencyCount(): number {
    return this.currencyList == null ? 6 : this.currencyList.length + 6
  }

  //setMonthsInterval(from: any, to: any) {
  //  this.monthsInterval = [];

  //  let item = from;
  //  item.day = 1;

  //  while ((item.year < to.year) || (item.year == to.year && item.month <= to.month)) {

  //    if (!this.dataItems.some(x => x.date.year == item.year && x.date.month == item.month && x.date.day == item.day))
  //      this.monthsInterval.push({ date: item, label: this.getMonthLabel(item) });

  //    item = this.calendar.getNext(item, 'm', 1);
  //  }
  //}

  getIntervalLabel(beginDate: any, endDate: any): string {
    return `${beginDate.day} ${this.months[beginDate.month - 1]} ${beginDate.year} to ${endDate.day} ${this.months[endDate.month - 1]} ${endDate.year}`;
  }


  removeExchange(item: CurrencyRateItem) {
    let index: number = this.dataItems.indexOf(item);

    if (index >= 0) {
      this.dataItems.splice(index, 1);
      //this.setMonthsInterval(this.model.fromDate, this.model.toDate);
    }

  }

  isValidData(): boolean {

    if (this.onSave) {

      for (let item of this.dataItems) {
        for (let subitem of this.dataItems.filter(x => x != item)) {
          if (this.isBetween(subitem.beginDate, item.beginDate, item.endDate) || this.isBetween(subitem.endDate, item.beginDate, item.endDate))
            return false; 
        }
      }
    }
    return true;


  }

  isBetween(date: any, startDate: any, endDdate: any): boolean {
    if (date.year < startDate.year)
      return false;

    if (date.year > endDdate.year)
      return false;

    if (date.year == startDate.year && date.month < startDate.month)
      return false;

    if (date.year == endDdate.year && date.month > endDdate.month)
      return false;

    if (date.year == startDate.year && date.month == startDate.month && date.day < startDate.day)
      return false;

    if (date.year == endDdate.year && date.month == endDdate.month && date.day > endDdate.day)
      return false;

    return true;

  }

  save() {
    this.onSave = true;

    if (this.isValidData()) {

      var request = {
        currencyTargetId: this.model.currency.id,
        exRateTypeId: this.model.exchangeType.id,
        data: this.dataItems
      };

      this.service.save(request)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.showSuccess('EXCHANGE_RATE.TITLE', 'EXCHANGE_RATE.MSG_SAVED');
              this.refresh();
            }
            else {
              this.error = response.result;
              this.loading = false;
              // TODO check error from result
            }
          },
          error => {
            this.setError(error);
            this.loading = false;
          });
    }
  }

  //setDate($event: any, iteminfo: CurrencyRateItem) {
  //  iteminfo.date = $event.date;
  //  //this.setMonthsInterval(this.model.fromDate, this.model.toDate);
  //}

  geturl(beginDate: any, endDate: any) {
    let user: UserModel = JSON.parse(localStorage.getItem('currentUser'));

    let data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }


//    console.log("this.model.fromDate");

  //  console.log(this.model.fromDate);

   return [`/${this.utility.getEntityName()}/ratematrix/rate-matrix/${this.model.currency.id}/${beginDate.day}-${beginDate.month}-${beginDate.year}/${endDate.day}-${endDate.month}-${endDate.year}/${this.model.exchangeType.id}`, { paramParent: encodeURI(JSON.stringify(currentItem)) }];
  }

  onSelectAll() {
    this.currentCurrencies = this.currenciesToAdd;
  }

  onClearAll() {
    this.currentCurrencies = [];
  }

  disableOverFlow() {
    this.styleOverFlow = "unset";
  }

  enableOverflow() {
    this.styleOverFlow = "auto";
  }
}

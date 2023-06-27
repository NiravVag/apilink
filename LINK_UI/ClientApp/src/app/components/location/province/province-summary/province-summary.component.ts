import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocationService } from '../../../../_Services/location/location.service'
import { Provincesummarymodel, ProvinceSummaryItemModel } from '../../../../_Models/location/provincesummarymodel'
import { first } from 'rxjs/operators';
import { PageSizeCommon } from '../../../common/static-data-common'
import { Validator } from "../../../common/validator"
import { SummaryComponent } from '../../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-province-summary',
  templateUrl: './province-summary.component.html',
  styleUrls: ['./province-summary.component.css'],
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
export class ProvinceSummaryComponent extends SummaryComponent<Provincesummarymodel> {
  onInit(): void {
    this.Intialize();
  }
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return "province/edit-province";
  }
  public model: Provincesummarymodel;
  public data: any;
  error: string = '';
  loading: boolean = false;
  searchloading:boolean=false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  provinceList: Array<any>[];
  public isProvinceDetails: boolean = false;
  isFilterOpen: boolean;
  constructor(private service: LocationService,
    public utility: UtilityService,
    public validator: Validator,router: Router, route: ActivatedRoute, translate: TranslateService) {
    super(router,validator,route,translate);
    this.model = new Provincesummarymodel();
    this.validator.setJSON("location/provinse-summary.valid.json");
    this.validator.isSubmitted = false;
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }
  Intialize() {
    this.loading = true;
    this.data = this.service.getCountrySummary()
      .pipe()
      .subscribe(
        resultdata => {
          if (resultdata && resultdata.result == 1) {
            if (resultdata && resultdata.result == 1) {
              this.data = resultdata;
            }
            else {
              this.error = resultdata.result;
            }
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        }
      );
  }
  refreshprovince(countryid) {
    if (countryid != null && countryid.id > 0) {
      this.loading = true;
      this.service.getprovincebycountryid(countryid.id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.provinceList = result.data;
            }
            else {
              this.provinceList = [];
            }
            this.loading = false;
          },
          error => {
            this.provinceList = [];
            this.loading = false;
          });
    }
    else {
      this.provinceList = [];
      this.model.ProvinceValues = [];
    }
  }
  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }
  GetSearchData() {
    this.validator.initTost();
    this.searchloading=true;
    this.service.getProvinceSearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data.map((x) => {
              var tabItem: ProvinceSummaryItemModel = {
                id: x.id,
                country: x.countryName,
                provincename: x.name,
                prvincecode: x.code
              }
              return tabItem;
            });
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
          }
          console.log(response);
          this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.searchloading = false;
        });
  }
  IsFormvalid(): boolean {
    return this.validator.isValid('CountryValues');
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}

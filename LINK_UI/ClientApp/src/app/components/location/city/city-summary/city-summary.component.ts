import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocationService } from '../../../../_Services/location/location.service'
import { Citysummarymodel, CitySummaryItemModel } from '../../../../_Models/location/citysummarymodel'
import { first } from 'rxjs/operators';
import { PageSizeCommon } from '../../../common/static-data-common'
import { Validator } from "../../../common/validator"
import { SummaryComponent } from '../../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-city-summary',
  templateUrl: './city-summary.component.html',
  styleUrls: ['./city-summary.component.css'],
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
export class CitySummaryComponent extends SummaryComponent<Citysummarymodel> {
  onInit(): void {
    this.model = new Citysummarymodel();
    this.validator.setJSON("location/city-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.selectedPageSize = PageSizeCommon[0]
    this.Initialize();
  }
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return 'city/edit-city';
  }
  public model: Citysummarymodel;
  public data: any;
  public loading: boolean = false;
  public searchloading: boolean = false;
  public error: string = "";
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  provinceList: Array<any>[];
  citylist: Array<any>[];
  isFilterOpen: boolean;
  constructor(private service: LocationService,  public utility: UtilityService,public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService) {
    super(router, validator, route,translate);
    this.isFilterOpen = true;
  }
  Initialize() {
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
        },
        error => {
          this.setError(error);
          this.loading = false;
        }
      );
  }
  refreshprovince(countryid) {
    if (countryid != null && countryid > 0) {
      this.loading = true;
      this.service.getprovincebycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.provinceList = result.data;
            }
            else {
              this.provinceList = [];
              this.citylist = [];
            }
            this.loading = false;
          },
          error => {
            this.provinceList = [];
            this.citylist = [];
            this.loading = false;
          });
    }
    else {
      this.provinceList = [];
      this.model.provinceValues = null;
      this.citylist = [];
      this.model.Cityvalues = [];
    }
  }
  refreshDataByCountry(countryid) {
    this.refreshprovince(countryid);
    this.refresCitybyCountryId(countryid);
  }
  refresCitybyCountryId(countryid) {
    if (countryid != null && countryid > 0) {
      this.loading = true;
      this.service.getcitybycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.citylist = result.data;
            }
            else {
              this.citylist = [];
            }
            this.loading = false;
          },
          error => {
            this.citylist = [];
            this.loading = false;
          });
    }
    else {
      this.citylist = [];
      this.model.Cityvalues = [];
    }
  }
  refreshcity(province) {
    if (province != null && province.id > 0) {
      this.loading = true;
      this.service.getcitybyprovinceid(province.id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.citylist = result.data;
            }
            else {
              this.citylist = [];
            }
            this.loading = false;
          },
          error => {
            this.citylist = [];
            this.loading = false;
          });
    }
    else {
      this.citylist = [];
      this.model.Cityvalues = [];
    }
  }
  SearchDetails() {
    this.validator.initTost();
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }
  GetSearchData() {
    this.searchloading = true;
    this.service.getCitySearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data.map((x) => {
             
              var tabItem: CitySummaryItemModel = {
                id: x.id,
                countryname: x.countryName,
                cityname: x.name,
                officename: x.officeName,
                phcode: x.phoneCode,
                provincename: x.provinceName,
                traveltime: x.travelTimeHH,
                zone: x.zoneName
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
          this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.searchloading = false;
        });
  }
  formValid(): boolean {
    return this.validator.isValid('Countryvalues');
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}


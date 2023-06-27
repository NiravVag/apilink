import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocationService } from '../../../../_Services/location/location.service'
import { Countrysummarymodel, CountrySummaryItemModel } from '../../../../_Models/location/countrysummarymodel'
import { first } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Validator } from '../../../common/validator'
import { PageSizeCommon } from '../../../common/static-data-common'
import { SummaryComponent } from '../../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-country-summary',
  templateUrl: './country-summary.component.html',
  styleUrls: ['./country-summary.component.css'],
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
export class CountrySummaryComponent extends SummaryComponent<Countrysummarymodel> {
  onInit(): void {
    this.validator.setJSON('location/edit-city.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.Intitialize();
  }
  getPathDetails(): string {
    return "country/edit-country";
  }
  public model: Countrysummarymodel;
  public data: any;
  error = '';
  loading = false;
  searchloading = false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  isFilterOpen: boolean;
  constructor(private service: LocationService, 
    public utility: UtilityService,
    public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService) {
    super(router, validator, route,translate);
    this.model = new Countrysummarymodel();
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }
  Intitialize() {
    this.loading = true;
    this.data = this.service.getCountrySummary()
      .pipe()
      .subscribe(
        resultdata => {
          if (resultdata && resultdata.result == 1) {
            this.data = resultdata;
          }
          else {
            this.error = resultdata.result;
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }
  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }
  getData(): void {
    this.searchloading = true;
    this.service.getCountrySearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data.map((x) => {
              var tabItem: CountrySummaryItemModel = {
                id: x.id,
                countryname: x.countryName,
                area: x.area,
                countrycode: x.countrycode,
                alphacode: x.alphacode
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
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}

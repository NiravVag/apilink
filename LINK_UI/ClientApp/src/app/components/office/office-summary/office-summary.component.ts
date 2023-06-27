import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OfficeService } from '../../../_Services/office/office.service'
import { first } from 'rxjs/operators';
import { Validator } from '../../common/validator'
import { PageSizeCommon } from '../../common/static-data-common'
import { OfficeSummaryModel, OfficeSummaryItemModel } from '../../../_Models/office/officesummarymodel'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
//import { TestBed } from '@angular/core/testing';

@Component({
  selector: 'app-office-summary',
  templateUrl: './office-summary.component.html',
  styleUrls: ['./office-summary.component.css'],
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

export class OfficeSummaryComponent extends SummaryComponent<OfficeSummaryModel> {

  public model: OfficeSummaryModel;
  public data: any = [];
  public dataCountry: any = [];
  public searchloading: boolean = false;
  error = '';
  loading = false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  currentOfficeid: number;
  isFilterOpen: boolean;
  public operationcountryList: Array<any> = [];
  constructor(
    private service: OfficeService,
    public validator: Validator,
    public utility: UtilityService,
    router: Router, route: ActivatedRoute,
     translate: TranslateService) {
    super(router, validator, route,translate);
    this.model = new OfficeSummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.validator.setJSON("office/office-master.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.Intitialize();
  }

  Intitialize() {
    this.loading = true;
    this.data = this.service.getOfficeSummary()
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
          this.error = error;
          this.loading = false;
        });
  }

  SearchDetails() {
      this.model.pageSize = this.selectedPageSize;
      this.search();
  }

  getData(): void {
    this.searchloading = true;
    this.service.getOfficeSearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data.map((x) => {
              var tabItem: OfficeSummaryItemModel = {
                id: x.id,
                officename: x.name,
                address1: x.address,
                address2: x.address2,
                locationtypename: x.type.name,
                city: x.city.name,
                country: x.country.countryName,
                operationcountriesname: x.operationCountriesName
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
          this.error = error;
          this.searchloading = false;
        });
  }
  getPathDetails(): string {
    return "office/edit-office";
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}

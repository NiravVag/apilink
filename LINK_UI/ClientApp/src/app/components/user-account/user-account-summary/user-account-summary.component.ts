import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { UserAccountService } from '../../../_Services/UserAccount/useraccount.service'
import { first } from 'rxjs/operators';
import { Validator } from '../../common/validator'
import { PageSizeCommon, usertypelst } from '../../common/static-data-common'
import { UserAccountSummaryModel, UserAccountSummaryItemModel } from '../../../_Models/useraccount/useraccount.model'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { LocationService } from 'src/app/_Services/location/location.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
//import { TestBed } from '@angular/core/testing';

@Component({
  selector: 'app-user-account-summary',
  templateUrl: './user-account-summary.component.html',
  styleUrls: ['./user-account-summary.component.scss'],
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

export class UserAccountSummaryComponent extends SummaryComponent<UserAccountSummaryModel> {

  public model: UserAccountSummaryModel;
  public data: any = [];
  public dataCountry: any = [];
  public userTypeList: any = usertypelst;
  public searchloading: boolean = false;
  public isInternalUser: boolean = false;
  error = '';
  loading = false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  isFilterOpen: boolean;
  constructor(
    private service: UserAccountService,
    public validator: Validator,
    router: Router, route: ActivatedRoute, public route1: Router, translate: TranslateService,  public locationservice: LocationService,public utility:UtilityService) {
    super(router, validator, route,translate);
    this.model = new UserAccountSummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.validator.setJSON("useraccount/user-account-master.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.Intitialize();
  }

  Intitialize() {
    this.loading = true;
    this.validator.isSubmitted = false;
       this.locationservice.getCountrySummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.dataCountry = data.countryList;
          }
          else {
            this.error = data.result;
          }
          this.loading = false;
        },
        error => {
  this.dataCountry = [];
this.loading = false;
          this.setError(error);
        });
  }

  getDetailsUser(id, type) { 
    let entity: string = this.utility.getEntityName();

    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }

    this.route1.navigate([`/${entity}/${this.getPathDetails()}/${id}/${type}`], {queryParams:{ paramParent: encodeURI(JSON.stringify(currentItem)) }});
  }

  changePageSize() {
    this.validator.initTost();
    this.model.pageSize = this.selectedPageSize;
    this.search();
}

  getData(): void {
    this.validator.initTost();
      this.searchloading = true;
    this.service.getUserAccountSearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data.map((x) => {
              var tabItem: UserAccountSummaryItemModel = {
                id: x.id,
                name: x.name,
                gender: x.gender,
                departmentName: x.departmentName,
                position: x.position,
                office: x.office,
                country: x.country,
                hasAccount: x.hasAccount,
                userTypeId: x.userTypeId
              }
              return tabItem;
            });
            if(this.model.userTypeId == 1) {
              this.isInternalUser = true;
            }
            else {
              this.isInternalUser = false;
            }
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
          }
          this.searchloading = false;
          this.validator.isSubmitted = false;
        },
        error => {
          this.error = error;
          this.searchloading = false;
          this.validator.isSubmitted = false;
        });
  }
  getPathDetails(): string {
    return "useredit/edit-user-account";
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  } 
}

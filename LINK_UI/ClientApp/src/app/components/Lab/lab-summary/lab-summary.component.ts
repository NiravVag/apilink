import { Component } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import { SummaryComponent } from '../../common/summary.component';
import { LabSummaryModel, LabToRemove, LabSummaryMaster, LabSummaryItemModel } from 'src/app/_Models/lab/labsummary.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Validator } from '../../common';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { first } from 'rxjs/operators';
import { LabService } from 'src/app/_Services/lab/lab.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-lab-summary',
  templateUrl: './lab-summary.component.html',
  styleUrls: ['./lab-summary.component.css'],
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
export class LabSummaryComponent extends SummaryComponent<LabSummaryModel> {
  private modelRef: NgbModalRef;
  private deleteLoading: boolean = false;
  private currentUser: UserModel;
  searchloader: boolean = false;
  model: LabSummaryModel;
  modelRemove: LabToRemove;
  masterData: LabSummaryMaster;
  isFilterOpen: boolean;

  constructor(router: Router, route: ActivatedRoute, public validator: Validator,
    private service: LabService, private modalService: NgbModal, public utility: UtilityService, authService: AuthenticationService,translate: TranslateService) {
    super(router, validator, route,translate);
    this.validator.setJSON("lab/lab-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model = new LabSummaryModel();
    this.masterData = new LabSummaryMaster();
    this.currentUser = authService.getCurrentUser();
    this.isFilterOpen = true;

  }
  onInit(): void {
    this.masterData.mainTypeLoading = true;
    this.masterData.labNameLoading = true;
    this.masterData.countryLoading = true;
    this.getMainType();
    this.getCountry();
    this.getLabName();
  }
  getMainType() {
    this.service.getMainType()
      .pipe().subscribe(
        response => {
          if (response && response.result == 1)
            this.masterData.mainTypeList = response.typeList;
          else
            this.error = response.result;
          this.masterData.mainTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.mainTypeLoading = false;
        });
  }
  getCountry() {
    this.service.getCountry()
      .pipe().subscribe(
        response => {
          if (response && response.result == 1)
            this.masterData.countryList = response.countryList;
          if (this.currentUser.countryId != null) {
            //select current user country
            this.model.countryValues = [this.masterData.countryList
              .filter(x => x.id == this.currentUser.countryId)[0].id];
          }
          else
            this.error = response.result;
          this.masterData.countryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.countryLoading = false;
        });
  }
  getLabName() {
    this.service.getLabName()
      .pipe().subscribe(
        response => {
          if (response && response.result == 1)
            this.masterData.labNameList = response.labList;
          else
            this.error = response.result;
          this.masterData.labNameLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.labNameLoading = false;
        });
  }
  getData() {
    this.validator.initTost();
    this.searchloader = true;
    this.service.getDataSearch(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data.map((x) => {
              var item: LabSummaryItemModel = {
                id: x.id,
                labName: x.labName,
                countryName: x.countryName,
                provinceName: x.provinceName,
                cityName: x.cityName,
                labType: x.typeName
              }
              return item;
            });
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            // TODO check error from result
          }
          this.searchloader = false;
        },
        error => {
          this.searchloader = false;
        });
  }
  getPathDetails(): string {
    return "labedit/edit-lab";
  }
  deleteLab(item: LabToRemove) {
    if (item && item.id > 0) {
      this.deleteLoading = true;
      this.service.deleteLab(item.id)
        .pipe()
        .subscribe(
          response => {
            if (response && (response.result == 1)) {
              // refresh
              this.refresh();
              this.getLabName();// after delete refresh the name.
            }
            else {
              this.error = response.result;
              // TODO check error from result
            }
            this.deleteLoading = false;
          },
          error => {
            this.error = error;
            this.deleteLoading = false;
          });
    }
    this.modelRef.close();
  }

  getEditDetails(id) {
    this.getDetails(id);
  }
  openConfirm(id, name, content) {
    this.modelRemove = {
      id: id,
      name: name
    };
    this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}

import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, ChildActivationEnd } from "@angular/router";
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { Validator } from "../../common";
import { ToastrService } from "ngx-toastr";
import { RoleRightService } from '../../../_Services/RoleRight/roleright.service'
import { DetailComponent } from '../../common/detail.component';
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview'
import { RoleRightModel } from '../../../_Models/roleright/rolerightmodel';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-role-right-configuration',
  templateUrl: './role-right-configuration.component.html',
  styleUrls: ['./role-right-configuration.component.scss']
})
export class RoleRightConfigurationComponent extends DetailComponent {
  public model: RoleRightModel;
  public roleList: Array<any> = [];
  public data: any;
  public rightList: TreeviewItem[];
  public noItem: Boolean = true;
  public saveLoading: Boolean = false;
  config = TreeviewConfig.create({
    hasAllCheckBox: false,
    hasFilter: false,
    hasCollapseExpand: true,
    decoupleChildFromParent: false,
    maxHeight: 500
 });
  constructor(
    private service: RoleRightService,
    route: ActivatedRoute,
    public validator: Validator,
    translate: TranslateService,
    notification: ToastrService,
    public utility: UtilityService,
    router: Router
  ) {
    super(router, route, translate, notification);
    this.model = new RoleRightModel();
  }

  onInit(): void {
    this.Intitialize();
  }
  getViewPath(): string {
    throw new Error("Method not implemented.");
  }
  getEditPath(): string {
    return "rolerights/role-right-configuration"
  }

  Intitialize() {
    this.loading = true;
    this.data = this.service.getRoleRightSummary()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.data = response;
            this.roleList = response.roleList;
          }
          else {
            this.error = response.result;
          }
          this.loading = false;
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }
  onChange() {
    this.getData();
  }
  getData(){
    this.loading = true;
    this.data = this.service.getRoleRightByRoleId(this.model.roleId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.noItem = false;
            this.data = response;
            var lst = [];
            response.rightList.forEach(item => {
              var i = mapToTreeViewItem(item)
              lst.push(i);
            });
            this.rightList = lst;
          }
          else {
            this.error = response.result;
          }
          this.loading = false;
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }
  Reset() {
    this.noItem = true;
    this.model = new RoleRightModel();
  }
  save() {
    this.saveLoading = true;
    this.model.rightIdList = GetAllItems(this.rightList, this.model.rightIdList);
    this.service.saveRoleRight(this.model)
    .subscribe(
      response => {
        if(response && response.result == 1) {
          this.showSuccess("ROLE_RIGHT.TITLE", "ROLE_RIGHT.MSG_SAVE_SUCCESS");
        }
        else {
          this.showError("ROLE_RIGHT.TITLE", "ROLE_RIGHT.MSG_CANNOT_SAVE");
        }
        this.saveLoading = false;
      },
      error => {
        this.error = error;
        this.saveLoading = false;
      });
  }
}

function mapToTreeViewItem(item: any){
  var child = [];
  var result = null;
  if(item.children){
    var childItem = null;
    item.children.forEach(i => {
      childItem = mapToTreeViewItem(i);
      child.push(childItem);
    }); 
  }
    result = new TreeviewItem({
      checked: item.checked,
      text: item.text,
      value: item.value,
      children: child
    });
  return result;
};

function GetAllItems(list: any, item: any){
  var lstItem: Array<any> = item;
  for(var _i = 0, list_1 = list; _i < list_1.length; _i++){
    var root = list_1[_i];
    if(root.children != undefined){
      if(root.internalChecked != false){
          lstItem.push(root.value);
          for (var _j = 0, _a = root.children; _j < _a.length; _j++) {
            var child = _a[_j];
            if(child.internalChecked != false)
            {
                lstItem.push(child.value);
            }
        }
      }
    }
  }
  lstItem = lstItem.filter(distinct);
  lstItem = lstItem.sort(function(a, b){return a-b});
  return lstItem;
}

const distinct = (value, index, self) => {
  return self.indexOf(value) === index;
}
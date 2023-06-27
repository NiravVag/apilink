import { Component, NgModule, OnChanges, SimpleChanges, OnInit, Input } from '@angular/core';
import { first, switchMap } from 'rxjs/operators';

import { Router, ActivatedRoute } from '@angular/router';
import { UserModel } from '../../../_Models/user/user.model';
import { MenuItemModel } from '../../../_Models/user/menuitem.model';
import { tryParse } from 'selenium-webdriver/http';

@Component({
  selector: 'app-breadCrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})

export class BreadcrumbComponent {

  private User: UserModel;
  public items: Array<string>;
  public toplevel1: string;
  public toplevel2: string;
  public toplevel3: string;
  private activemenuid: number;
  @Input() activemenuname?: string;
  constructor(private router: Router, private route: ActivatedRoute) {
    if (localStorage.getItem('currentUser')) {
      this.User = JSON.parse(localStorage.getItem('currentUser'));
    }
    // console.log(this.router.url);
    // console.log(this.route.snapshot);
    // this.items = this.getItems();
  }
  ngOnInit() {
    if (localStorage.getItem('_activemenuid'))
    this.activemenuid = parseInt(localStorage.getItem('_activemenuid'));
  this.SetValues(this.activemenuid);
	}
  SetValues(activemenuid) {
    this.toplevel3 = this.activemenuname;
    if (activemenuid) {
      var supermenuname = "";
      var submenuname = "";
      var issubexit:boolean=false;
      for (let menu of this.User.rights) {
        if (menu.id == activemenuid) {
          break;
        }
        supermenuname = menu.titleName;
        if (menu.children.length > 0) {
          for (let submenu of menu.children) {
            submenuname = submenu.titleName;
            if (submenu.id == activemenuid) {
              this.toplevel2 = submenuname;
              this.toplevel1 = supermenuname;
              break;
            }
            if (submenu.children.length > 0 && submenu.children.filter(x => x.id == activemenuid).length > 0) {
              this.toplevel1 = supermenuname;
              this.toplevel2 = submenuname;
              this.toplevel3=this.activemenuname==null?submenu.children.filter(x => x.id == activemenuid)[0].menuName:this.activemenuname;
              break;
            }
          }
        }
      }
    }
  }
  // getItems() : Array<string> {
  //   let url: string = this.router.url;
  //   let data: Array<string> = [];
  //   let itemValues: Array<string> = [];

  //   let segments = this.route.snapshot.url;
  //   let paramkeys = Object.keys(this.route.snapshot.params);
  //   let params = this.route.snapshot.params;

  //   for (let segment of segments) {

  //     if (!paramkeys.some(x => params[x] == segment.path)) {
  //       itemValues.push(segment.path);
  //     }
  //   }

  //   if (itemValues.length > 0) {

  //     let value = itemValues[0];

  //     for (let item of this.User.rights) {

  //       if (this.find(item, value, data)) {
  //         console.log("data");
  //         console.log(data);
  //         return data;
  //       }
  //     }
  //   }

  //   console.log("itemValues");
  //   console.log(itemValues);


  //   return data;
  // }

  // private find(item: MenuItemModel, value: string, data: Array<string>): boolean {
  //   console.log(item.path);
  //   if (value == item.path) {

  //     if (data == null)
  //       data = [];

  //     data.push(item.path);
  //     console.log(data);

  //     return true;
  //   }


  //   for (let child of item.children) {
  //     let dataCopy = [];

  //     if (this.find(child, value, dataCopy)) {

  //       if (data == null)
  //         data = [];

  //       for (let element of dataCopy)
  //         data.push(element);

  //       return true;
  //     }
  //   }

  //   return false; 
  // }

}

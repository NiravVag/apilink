import { RoleModel } from './role.model';
import { MenuItemModel } from './menuitem.model';
import{UserType}from '../../components/common/static-data-common'
  import { from } from 'rxjs';
export class UserModel {
  id: number | null;
  userName: string;
  fullName: string;
  roles: Array<RoleModel> | null;
  rights: Array<MenuItemModel> | null;
  entityName: string;
  countryId: number; 
  usertype:UserType;
  customerid:number;
  supplierid:number;
  factoryid:number;
  staffId:number;
  showHeader: boolean;
  serviceAccess: Array<number> | null;
  rightsDB: Array<MenuItemModel> | null;
  profiles:[];
  fbUserId:number;
  constructor() {

  }


}

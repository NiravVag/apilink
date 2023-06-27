export class MenuItemModel {
  id: number | null;
  parentId: number | null;
  titleName: string;
  menuName: string;
  path: string;
  isHeading: boolean;
  active: boolean;
  glyphicons: string;
  ranking: number | null;
  children: Array<MenuItemModel> | null;
  isExpand: boolean;
  showMenu: boolean;
  rightType: number | null;

  constructor() {
    this.isExpand = false;
  }
  
}

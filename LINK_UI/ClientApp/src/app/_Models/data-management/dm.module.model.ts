
export interface DMModuleListResponse {
    list: Array<DmModule>; 
    result : ModuleListResult; 
}

export interface DmModule {
  id: number; 
  parentId: number; 
  moduleName: string;
  ranking: number; 
  needCustomer: boolean;
  children: Array<DmModule>;
  selected: number | null; 
}

export enum ModuleListResult {
  Success = 1,
  NotFound = 3,
  NotAUthroized = 4
}


    export interface ModuleListResponse
    {
      data: Array<ModuleItem>;
      result: ModuleListResult;
    }

    export interface ModuleItem
    {
     id: number; 
     name: string;
    }

    export enum ModuleListResult
    {
        Success = 1, 
        NotFound = 2
    }

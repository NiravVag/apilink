
export class RoleModel {
  id: number | null;
  roleName: string;
}

export interface RolesResponse {
  list: Array<Role>;
  result: RolesResult;
}

export enum RolesResult {
  Success = 1,
  NotFound = 2
}

export  interface Role {
  id: number; 
  name: string; 
}


export class EditCustomerServiceConfigModel {
    public id: number;
    public customerID: number; 
    public customerName: number; 
    public service: number;
    public serviceType: number; 
    public pickType: number; 
    public levelPick1: number;  
    public levelPick2: number;  
    public criticalPick1: number;
    public criticalPick2: number;
    public majorTolerancePick1: number;
    public majorTolerancePick2: number;
    public minorTolerancePick1: number;
    public minorTolerancePick2: number; 
    public allowAQLModification:boolean;
    public ignoreAcceptanceLevel:boolean;
    public defectClassification: number; 
    public checkMeasurementPoints:boolean;
    public reportUnit: number; 
    public productCategory: number; 
    public active:boolean;
    public customServicetypeName:string;
    public customerRequirementIndex:number;
    public dpPoint: number; 
    constructor() {
      this.id = 0;    
    }
  }
  
  
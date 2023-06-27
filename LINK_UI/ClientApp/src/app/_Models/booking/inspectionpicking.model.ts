export class InspectionPicking {

    public id:number;
    public labId:number;
    public labName:string
    public labNameImgPath:string
    public showLabName:boolean;
    public poId:number;
    public productId:number;
    public customerId:number;
    public labAddressId:number;
    public labAddress:string
    public showLabAddress:boolean;
    public labAddressImgPath:string
    public cusAddressId:number;
    public poTranId:number;
    public pickingQuantity:number;
    public remarks:string
    public active:boolean;
    public labType:number;
    public labContactLoading:boolean;
    public labAddressLoading:boolean;
    public labContactTypeItems: any[];
    public labAddressList:any[];
    public labContactList:any[];
    public inspectionPickingContacts:Array<InspectionPickingContact>;

}

export class PickingProducts{
    public id:number;
    public productId:number;
}

export class InspectionPickingData
{
    public id:number;
    public labId:number;
    public customerId:number;
    public labAddressId:number;
    public cusAddressId:number;
    public poTranId:number;
    public pickingQty:number;
    public remarks:string
    public active:boolean;
    public inspectionPickingContacts:Array<InspectionPickingContact>;

        //public IEnumerable<InspectionPickingContact> InspectionPickingContacts { get; set; }
}

export class InspectionPickingContact{
    public id:number;
    public pickingTranId:number;
    public labContactId:number;
    public cusContactId:number;
    public active:boolean;
}
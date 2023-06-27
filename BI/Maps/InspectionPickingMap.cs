using DTO.Common;
using DTO.Inspection;
using DTO.InspectionPicking;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public  class InspectionPickingMap: ApiCommonData
    {
        public  List<InspectionPickingData> GetInspectionPickingList(InspTransaction entity)
        {
            List<InspectionPickingData> objPickingList = new List<InspectionPickingData>();
            List<InspectionPickingContact> objPickingContactList = new List<InspectionPickingContact>();
            List<PickingProduct> pickingProductList = new List<PickingProduct>();
            if (entity == null)
                return null;
            var activePOTransactions = entity.InspPurchaseOrderTransactions.Where(x =>x.PickingQuantity>0 && x.Active.HasValue && x.Active.Value);

            foreach (var poData in activePOTransactions)
            {
                foreach (var pickingInfo in poData.InspTranPickings)
                {
                    if (pickingInfo.Active)
                    {
                        var pickingData = new InspectionPickingData()
                        {
                            PoId = poData.PoId,
                            PoName = poData.Po?.Pono,
                            PoTranId = poData.Id,
                            Id = pickingInfo.Id,
                            LabId = pickingInfo.LabId,
                            CustomerId = pickingInfo.CustomerId,
                            LabAddressId = pickingInfo.LabAddressId,
                            CusAddressId = pickingInfo.CusAddressId,
                            PickingQuantity = pickingInfo.PickingQty,
                            Remarks = pickingInfo.Remarks,
                            ProductID=poData.ProductRef.ProductId,
                            Active = pickingInfo.Active
                        };

                        if (pickingInfo.InspTranPickingContacts != null)
                        {
                            objPickingContactList = new List<InspectionPickingContact>();
                            foreach (var pickingContacts in pickingInfo.InspTranPickingContacts)
                            {
                                if (pickingContacts.Active)
                                {
                                    objPickingContactList.Add(new InspectionPickingContact()
                                    {
                                        Id = pickingContacts.Id,
                                        LabContactId = pickingContacts.LabContactId,
                                        CusContactId = pickingContacts.CusContactId,
                                        PickingTranId = pickingContacts.PickingTranId,
                                        Active = pickingContacts.Active
                                    });
                                }
                            }
                            pickingData.InspectionPickingContacts = objPickingContactList;
                        }

                        objPickingList.Add(pickingData);
                    }
                }
            }

            return objPickingList;
        }

        public  List<PickingProduct> GetPickingProductList(InspTransaction entity)
        {
            List<PickingProduct> pickingProductList = new List<PickingProduct>();
            if (entity == null)
                return null;

            var inspectionPOTransactions = entity.InspPurchaseOrderTransactions?.
                Where(x => x.Active.HasValue && x.Active.Value && x.PickingQuantity > 0);


            foreach (var poData in inspectionPOTransactions)
            {

                var pickingProduct = new PickingProduct();
                pickingProduct.inspectionID = entity.Id;
                pickingProduct.poID = poData.PoId;
                pickingProduct.poName = poData.Po?.Pono;
                pickingProduct.productID = poData.ProductRef.ProductId;
                pickingProduct.productName = poData.ProductRef?.Product?.ProductId;
                pickingProduct.pickingQuantity = poData.PickingQuantity;
                pickingProduct.poTransactionID = poData.Id;
                pickingProductList.Add(pickingProduct);

            }

            return pickingProductList.OrderBy(x => x.productName).ToList();
        }
    }
}

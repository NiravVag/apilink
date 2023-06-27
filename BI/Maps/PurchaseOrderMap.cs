using DTO.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DTO.Common;
using Entities;
using DTO.Inspection;
using DTO.CommonClass;
using Entities.Enums;

namespace BI.Maps
{
    public class PurchaseOrderMapData : ApiCommonData
    {
        public PurchaseOrderSearchData GetPurchaseOrder(PurchaseOrderRepoData entity, List<PurchaseOrderDetailsRepoData> poDetails,
            List<POBookingRepo> bookingNoPOIdList)
        {
            if (entity == null)
                return null;

            bool _isExistInBooking = bookingNoPOIdList.Any(x => x.PoId == entity.Id && x.StatusId != (int)BookingStatus.Cancel);

            int _bookingNo = bookingNoPOIdList.Where(x => x.PoId == entity.Id).Select(x => x.BookingNumber).FirstOrDefault();

            int _bookingCount = bookingNoPOIdList.Where(x => x.PoId == entity.Id).Select(x => x.BookingNumber).Distinct().Count();

            return new PurchaseOrderSearchData
            {
                Id = entity.Id,
                CustomerName = entity.CustomerName,
                DestinationCountry = string.Join(',', poDetails.Where(x => x.PoId == entity.Id && x.DestinationCountry != "").
                Select(x => x.DestinationCountry).Distinct().ToArray()),
                ETD = string.Join(',', poDetails.Where(x => x.ETD != null && x.PoId == entity.Id)
                .Select(x => x.ETD?.ToString(StandardDateFormat)).Distinct().ToArray()),
                Active = true,
                IsBooked = false,
                Pono = entity.Pono,
                PoId = entity.PoId,
                IsDelete = _isExistInBooking,
                BookingNumber = _bookingNo,
                //showing one booking number and rest as a count. so subract by 1
                BookingCount = _bookingCount,
                ShowBookingCount = _bookingCount - 1
            };
        }

        public PurchaseOrderSearchData GetPurchaseOrderList(CuPurchaseOrder entity)
        {
            if (entity == null)
                return null;

            return new PurchaseOrderSearchData
            {
                Id = entity.Id,
                Pono = entity.Pono
            };
        }
        //Map PO Product details
        public PurchaseOrderDetails GetPurchaseOrderDetailsList(PurchaseOrderRepo entity, IEnumerable<int> poProducts)
        {
            if (entity == null)
                return null;

            var _isBooked = poProducts.Where(x => x == entity.ProductId).ToList().Count() > 0 ? true : false;
            return new PurchaseOrderDetails
            {
                Id = entity.Id,
                PoId = entity.PoId,
                BookingStatus = entity.BookingStatus,
                DestinationCountryId = entity.DestinationCountryId,
                DestinationCountryName = entity.DestinationCountryName,
                Etd = entity.Etd.HasValue ? new DateObject(entity.Etd.Value.Year, entity.Etd.Value.Month, entity.Etd.Value.Day) : null,
                FactoryId = entity.FactoryId,
                FactoryReference = entity.FactoryReference,
                ProductDesc = entity.ProductDesc,
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                Quantity = entity.Quantity,
                SupplierId = entity.SupplierId,
                SupplierName = entity.SupplierName,
                IsBooked = _isBooked,

                Active = true
            };
        }
        //Export PurchaseOrder

        public PurchaseOrderExportDataItem GetPurchaseOrderExportData(PurchaseOrderDetailsRepo entity, List<POMappedSupplier> poMappedSuppliers, List<POMappedFactory> poMappedFactories)
        {
            if (entity == null)
                return null;
            return new PurchaseOrderExportDataItem
            {
                CustomerName = entity.CustomerName,
                PO = entity.Pono,
                CustomerReferenceNo = entity.CustomerRefNo,
                ProductId = entity.ProductId,
                ProductDescription = entity.ProductDescription,
                DestinationCountry = entity.DestinationCountry,
                ETD = entity.ETD?.ToString(StandardDateFormat),
                Supplier = string.Join(", ", poMappedSuppliers.
                        Where(y => y.PoId == entity.PoId).Select(z => z.SupplierName).ToList()),
                Factory = string.Join(", ", poMappedFactories.
                        Where(y => y.PoId == entity.PoId).Select(z => z.FactoryName).ToList()),
                Qty = entity.Qty
            };
        }

        public List<POProductData> MapPoProductDataList(List<POProductRelatedData> poProductRelatedDataList,
                    List<InspectionProductSubCategory> productSubCategoryList, List<InspectionProductSubCategory2> productSubCategory2List,
                    List<InspectionProductSubCategory3> productSubCategory3List)
        {
            List<POProductData> PoProductDataList = new List<POProductData>();

            foreach (var poProductRelatedData in poProductRelatedDataList)
            {
                POProductData poProductData = new POProductData();
                poProductData.Id = poProductRelatedData.Id;
                poProductData.Name = poProductRelatedData.Name;
                poProductData.Description = poProductRelatedData.Description;
                poProductData.PoQuantity = poProductRelatedData.PoQuantity;

                poProductData.BarCode = poProductRelatedData.BarCode;
                poProductData.FactoryReference = poProductRelatedData.FactoryReference;
                poProductData.IsNewProduct = poProductRelatedData.IsNewProduct;
                poProductData.Etd = Static_Data_Common.GetCustomDate(poProductRelatedData.Etd);
                poProductData.DestinationCountryId = poProductRelatedData.DestinationCountryId;
                poProductData.DestinationCountryName = poProductRelatedData.DestinationCountryName;
                poProductData.Remarks = poProductRelatedData.Remarks;
                poProductData.PoId = poProductRelatedData.PoId;
                poProductData.PoName = poProductRelatedData.PoName;

                //map the product category id
                if (poProductRelatedData.ProductCategoryId != null)
                {
                    poProductData.ProductCategoryId = poProductRelatedData.ProductCategoryId;
                    poProductData.ProductCategoryName = poProductRelatedData.ProductCategoryName;
                }

                //map the product sub category list if product sub category id is not there
                if (poProductRelatedData.ProductSubCategoryId == null && poProductRelatedData.ProductCategoryId != null)
                {
                    poProductData.ProductSubCategoryList = productSubCategoryList.
                            Where(x => x.ProductCategoryId == poProductRelatedData.ProductCategoryId).
                            Select(x => new CommonDataSource()
                            {
                                Id = x.ProductSubCategoryId.GetValueOrDefault(),
                                Name = x.ProductSubCategoryName
                            }).ToList();
                }
                //map the product sub category id
                else if (poProductRelatedData.ProductSubCategoryId != null)
                {
                    poProductData.ProductSubCategoryId = poProductRelatedData.ProductSubCategoryId;
                    poProductData.ProductSubCategoryName = poProductRelatedData.ProductSubCategoryName;
                }
                //map the product sub category2 list if product sub category2 id is not there
                if (poProductRelatedData.ProductSubCategory2Id == null && poProductRelatedData.ProductSubCategoryId != null)
                {
                    poProductData.ProductSubCategory2List = productSubCategory2List.
                            Where(x => x.ProductSubCategoryId == poProductRelatedData.ProductSubCategoryId).
                            Select(x => new CommonDataSource()
                            {
                                Id = x.ProductSubCategory2Id.GetValueOrDefault(),
                                Name = x.ProductSubCategory2Name
                            }).ToList();
                }
                //map the product sub category2 id
                else if (poProductRelatedData.ProductSubCategory2Id != null)
                {
                    poProductData.ProductSubCategory2Id = poProductRelatedData.ProductSubCategory2Id;
                    poProductData.ProductSubCategory2Name = poProductRelatedData.ProductSubCategory2Name;
                }
                //map the product sub category3 list if product sub category3 id is not there
                if (poProductRelatedData.ProductSubCategory3Id == null && poProductRelatedData.ProductSubCategory2Id != null)
                {
                    poProductData.ProductSubCategory3List = productSubCategory3List.
                            Where(x => x.ProductSubCategory2Id == poProductRelatedData.ProductSubCategory2Id).
                            Select(x => new CommonDataSource()
                            {
                                Id = x.ProductSubCategory3Id.GetValueOrDefault(),
                                Name = x.ProductSubCategory3Name
                            }).ToList();
                }
                //map the product sub category3 id
                else if (poProductRelatedData.ProductSubCategory3Id != null)
                {
                    poProductData.ProductSubCategory3Id = poProductRelatedData.ProductSubCategory3Id;
                    poProductData.ProductSubCategory3Name = poProductRelatedData.ProductSubCategory3Name;
                }

                PoProductDataList.Add(poProductData);
            }

            return PoProductDataList;
        }






    }
}

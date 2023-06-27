using DTO.Common;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public  class QCBlockMap: ApiCommonData
    {
        //map the data from DB
        public  QCBlockRequest MapQCBlockEdit(QcBlockList request)
        {
            return new QCBlockRequest()
            {
                Id = request.Id,
                CustomerIds = request.QcBlCustomers.Select(x => x.CustomerId.GetValueOrDefault()).ToList(),
                FactoryIds = request.QcBlSupplierFactories.Where(x => x.TypeId == (int)Supplier_Type.Factory).Select(x => x.SupplierFactoryId.GetValueOrDefault()).ToList(),
                ProductCategoryIds = request.QcBlProductCatgeories.Select(x => x.ProductCategoryId.GetValueOrDefault()).ToList(),
                ProductCategorySub2Ids = request.QcBlProductSubCategory2S.Select(x => x.ProductSubCategory2Id.GetValueOrDefault()).ToList(),
                ProductCategorySubIds = request.QcBlProductSubCategories.Select(x => x.ProductSubCategoryId.GetValueOrDefault()).ToList(),
                QCId = request.Qcid,
                SupplierIds = request.QcBlSupplierFactories.Where(x => x.TypeId == (int)Supplier_Type.Supplier_Agent).Select(x => x.SupplierFactoryId.GetValueOrDefault()).ToList()
            };
        }

        //map the summary data
        public  QCBlockSummaryItem MapQCSummaryData(QCBlockSummaryRepo qcblockData, QcblockSummaryDetailsRepo details)
        {
            var customernames = details.CustomerNameList.Where(x => x.Id == qcblockData.QCBlockId).Select(x => x.Name).ToList();
            var factorynames = details.FactoryList.Where(x => x.Id == qcblockData.QCBlockId).Select(x => x.Name).ToList();
            var productcategory = details.ProductCategoryList.Where(x => x.Id == qcblockData.QCBlockId).Select(x => x.Name).ToList();
            var productcategory2 = details.ProductCategorySub2List.Where(x => x.Id == qcblockData.QCBlockId).Select(x => x.Name).ToList();
            var productsubcategory = details.ProductCateogrySubList.Where(x => x.Id == qcblockData.QCBlockId).Select(x => x.Name).ToList();
            var suppliernames = details.SupplierList.Where(x => x.Id == qcblockData.QCBlockId).Select(x => x.Name).ToList();
            return new QCBlockSummaryItem()
            {
                
                CustomerNames = string.Join(", ", customernames),
                FactoryNames = string.Join(", ", factorynames),
                ProductCategoryNames = string.Join(", ", productcategory),
                ProductCategorySub2Names = string.Join(", ", productcategory2),
                ProductCategorySubNames = string.Join(", ", productsubcategory),
                QCName = qcblockData.QCName,
                QCBlockId = qcblockData.QCBlockId,
                SupplierNames = string.Join(",", suppliernames)
            };
        }
    }
}

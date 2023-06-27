using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.ProductManagement;
using DTO.References;
using DTO.Common;

namespace BI.Maps
{
    public class ProductManagementMap: ApiCommonData
    {
        #region Product Category
        public  ProductCategory GetProductCategory(RefProductCategory entity)
        {
            if (entity == null)
                return null;
            return new ProductCategory
            {
                Id = entity.Id,
                Name = entity.Name,
                Active = entity.Active,
                BusinessLineId = entity.BusinessLineId,
                BusinessLine = entity.BusinessLine?.BusinessLine
            };
        }
        public  RefProductCategory MapProductCategoryEntity(ProductCategory request,int entityid)
        {

            if (request == null)
                return null;

            return new RefProductCategory
            {
                Name = request.Name,
                Active = request.Active,
                EntityId = entityid,
                BusinessLineId = request.BusinessLineId
            };
        }
        #endregion

        #region Product Sub Category
        public  ProductSubCategory GetProductSubCategory(RefProductCategorySub entity)
        {
            if (entity == null)
                return null;
            return new ProductSubCategory
            {
                Id = entity.Id,
                Name = entity.Name,
                ProductCategory = new ProductCategory
                {
                    Id = entity.ProductCategoryId,
                    Name = entity.ProductCategory?.Name
                },
                Active = entity.Active
            };
        }
        public  RefProductCategorySub MapProductSubCategoryEntity(ProductSubCategory request,int entityid)
        {

            if (request == null)
                return null;

            return new RefProductCategorySub
            {
                Name = request.Name,
                Active = request.Active,
                ProductCategoryId = request.ProductCategoryId,
                EntityId = entityid
            };
        }
        #endregion

        #region Product Category Sub2
        public  ProductCategorySub2 GetProductCategorySub2(RefProductCategorySub2 entity)
        {
            if (entity == null)
                return null;
            return new ProductCategorySub2
            {
                Id = entity.Id,
                Name = entity.Name,
                ProductSubCategory = new ProductSubCategory
                {
                    Id = entity.ProductSubCategoryId,
                    Name = entity.ProductSubCategory?.Name
                },
                ProductCategory = new ProductCategory
                {
                    Id = entity.ProductSubCategory.ProductCategory.Id,
                    Name = entity.ProductSubCategory?.ProductCategory?.Name
                },
                Active = entity.Active
            };
        }
        public  RefProductCategorySub2 MapProductCategorySub2Entity(ProductCategorySub2 request, int entityid)
        {

            if (request == null)
                return null;

            return new RefProductCategorySub2
            {
                Name = request.Name,
                Active = request.Active,
                ProductSubCategoryId = request.ProductSubCategoryId,
                EntityId = entityid
            };
        }
        #endregion
    }
}

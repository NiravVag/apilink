using AutoMapper;
using DTO.Common;
using DTO.CustomerProducts;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BI
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //CreateMap<ProductCategory, RefProductCategory>();
            //CreateMap<ProductSubCategory, RefProductCategorySub>();
            //CreateMap<ProductTypeInternal, RefProductTypeInternal>();
            //CreateMap<FileAttachment, CuProductFileAttachment>()
            //    .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            //    .ForMember(dest => dest.GuidId, src => src.MapFrom(x => x.uniqueld))
            //    .ForMember(dest => dest.File, src => src.MapFrom(x => x.FileName))
            //    .ForMember(dest => dest.UploadDate, src => src.MapFrom(x => DateTime.Now))
            //    .ForMember(dest => dest.Active, src => src.MapFrom(x => true))
            //    .ForMember(dest => dest.UserId, src => src.MapFrom(x => _ApplicationContext.UserId));


            //CreateMap<CuProductFileAttachment, FileAttachment>()
            //  .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            //  .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.FileName))
            //  .ForMember(dest => dest.uniqueld, src => src.MapFrom(x => x.GuidId));



            //CreateMap<CustomerProduct, CuProduct>()
            //    .ForMember(dest => dest.CuProductFileAttachments, src => src.MapFrom(x => x.CuProductFileAttachments));

            //CreateMap<CuProduct, CustomerProduct>()
            //    .ForMember(dest => dest.CuProductFileAttachments, src => src.MapFrom(x => x.CuProductFileAttachments));

            //CreateMap<CuProduct,CustomerProductSearchData>()
            //    .ForMember(dest => dest.CustomerName, src => src.MapFrom(x => x.Customer.CustomerName))
            //    .ForMember(dest => dest.ProductCategory, src => src.MapFrom(x => x.ProductCategoryNavigation.Name))
            //    .ForMember(dest => dest.ProductSubCategory, src => src.MapFrom(x => x.ProductSubCategoryNavigation.Name))
            //    .ForMember(dest => dest.ProductInternalType, src => src.MapFrom(x => x.InternalProductTypeNavigation.Name));

        }
    }
}

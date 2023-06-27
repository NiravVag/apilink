using AutoMapper;
using Contracts.Managers;
using DTO.Audit;
using DTO.Common;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.Inspection;
using DTO.PurchaseOrder;
using DTO.References;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class MappingProfile : Profile
    {
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ITenantProvider _filterService = null;
        public MappingProfile(IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _ApplicationContext = applicationContext;
            _filterService = filterService;
        }

        public MappingProfile()
        {
            CreateMap<ProductCategory, RefProductCategory>().ReverseMap();
            CreateMap<ProductSubCategory, RefProductCategorySub>().ReverseMap();
            CreateMap<ProductCategorySub2, RefProductCategorySub2>().ReverseMap();


            CreateMap<RefProductCategorySub2, ProductCategorySub2>()
              .ForMember(dest => dest.ProductCategory, src => src.Ignore());

            CreateMap<ProductAttachment, CuProductFileAttachment>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.UniqueId, src => src.MapFrom(x => x.uniqueld))
                .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.FileName))
                .ForMember(dest => dest.FileUrl, src => src.MapFrom(x => x.FileUrl))
                .ForMember(dest => dest.UploadDate, src => src.MapFrom(x => DateTime.Now))
                .ForMember(dest => dest.Active, src => src.MapFrom(x => true))
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => _ApplicationContext.UserId))
                .ForMember(dest=>dest.EntityId,src=>src.MapFrom(x=> _filterService.GetCompanyId()))
                .ForMember(dest => dest.CuProductMscharts, src => src.MapFrom(x => x.ProductMsCharts));


            CreateMap<CuProductFileAttachment, ProductAttachment>()
              .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.FileName))
              .ForMember(dest => dest.uniqueld, src => src.MapFrom(x => x.UniqueId))
              .ForMember(dest => dest.ProductMsCharts, src => src.MapFrom(x => x.CuProductMscharts));

            CreateMap<ProductMSChart, CuProductMschart>().ReverseMap();

            CreateMap<CustomerProduct, CuProduct>()
                .ForMember(dest => dest.CuProductFileAttachments, src => src.MapFrom(x => x.CuProductFileAttachments));

            CreateMap<CuProduct, CustomerProduct>()
                .ForMember(dest => dest.CuProductFileAttachments, src => src.MapFrom(x => x.CuProductFileAttachments)).
                AfterMap((src, dest) =>
                {
                    if (src.CuPurchaseOrderDetails.Count > 0)
                    {
                        foreach (var item in src.CuPurchaseOrderDetails)
                        {
                            var inspectionIds = item.Po.InspPurchaseOrderTransactions.Where(x => x.Inspection.StatusId != (int)BookingStatus.Cancel
                                                                 && x.Active.HasValue && x.Active.Value && x.ProductRef.ProductId == item.ProductId)
                                                                 .Select(x => x.InspectionId).Distinct();
                            if (inspectionIds != null && inspectionIds.Count() > 1)
                            {
                                dest.isProductBooked = true;
                            }
                            if (inspectionIds != null && inspectionIds.Any())
                            {
                                dest.isBooked = true;
                            }
                        }
                    }
                });



            CreateMap<CuProduct, CustomerProductSearchData>()
                .ForMember(dest => dest.CustomerName, src => src.MapFrom(x => x.Customer.CustomerName))
                .ForMember(dest => dest.FactoryReference, src => src.MapFrom(x => x.FactoryReference))
                .ForMember(dest => dest.ProductCategory, src => src.MapFrom(x => x.ProductCategoryNavigation.Name))
                .ForMember(dest => dest.ProductSubCategory, src => src.MapFrom(x => x.ProductSubCategoryNavigation.Name))
                .ForMember(dest => dest.ProductCategorySub2, src => src.MapFrom(x => x.ProductCategorySub2Navigation.Name))
                .ForMember(dest => dest.ProductCategorySub3, src => src.MapFrom(x => x.ProductCategorySub3Navigation.Name))
                .ForMember(dest => dest.SampleSize8h, src => src.MapFrom(x => x.SampleSize8h))
                .ForMember(dest => dest.TimePreparation, src => src.MapFrom(x => x.TimePreparation));
            //.ForMember(dest => dest.IsBooked, src => src.MapFrom(x => false)).
            // AfterMap((src, dest) =>
            // {
            //     if (src.CuPurchaseOrderDetails.Count > 0)
            //     {
            //         foreach (var item in src.CuPurchaseOrderDetails)
            //         {
            //             var inspectionIds = item.Po.InspPurchaseOrderTransactions.Where(x => x.Inspection.StatusId != (int)BookingStatus.Cancel
            //                                                  && x.Active.HasValue && x.Active.Value && x.ProductRef.ProductId == item.ProductId)
            //                                                  .Select(x => x.InspectionId).Distinct();

            //             if (inspectionIds != null && inspectionIds.Any())
            //             {
            //                 dest.IsBooked = true;
            //             }
            //         }
            //     }
            // });

            CreateMap<PurchaseOrderAttachement, CuPurchaseOrderAttachment>().ReverseMap();
            
            CreateMap<PurchaseOrderDetails, CuPurchaseOrderDetail>()
                .ForMember(dest => dest.Etd, src => src.MapFrom(x => x.Etd.ToDateTime()))
                .ReverseMap()
                .ForMember(dest => dest.ProductDesc, src => src.MapFrom(x => x.Product.ProductDescription));
            
            
            CreateMap<CuPurchaseOrder, PurchaseOrderDetailedInfo>()
                 .ForMember(dest => dest.PurchaseOrderDetails, src => src.MapFrom(x => x.CuPurchaseOrderDetails.Where(y => y.Active.HasValue && y.Active.Value)))
                 .ForMember(dest => dest.PurchaseOrderAttachments, src => src.MapFrom(x => x.CuPurchaseOrderAttachments.Where(y => y.Active)));


            CreateMap<FileAttachment, CuPurchaseOrderAttachment>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.GuidId, src => src.MapFrom(x => x.uniqueld))
                .ForMember(dest => dest.UploadDate, src => src.MapFrom(x => DateTime.Now))
                .ForMember(dest => dest.Active, src => src.MapFrom(x => true))
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => _ApplicationContext.UserId));


            CreateMap<CuPurchaseOrderAttachment, FileAttachment>()
              .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.FileName))
              .ForMember(dest => dest.uniqueld, src => src.MapFrom(x => x.GuidId));


            CreateMap<PurchaseOrderDetailedInfo, CuPurchaseOrder>()
                .ForMember(dest => dest.Pono, src => src.MapFrom(x => x.Pono.Trim()))
                .ForMember(dest => dest.CuPurchaseOrderAttachments, src => src.MapFrom(x => x.PurchaseOrderAttachments));

            CreateMap<CuPurchaseOrder, PurchaseOrderDetailedInfo>()
                .ForMember(dest => dest.PurchaseOrderAttachments, src => src.MapFrom(x => x.CuPurchaseOrderAttachments.Where(y => y.Active)))
                .ForMember(dest => dest.Pono, src => src.MapFrom(x => x.Pono.Trim()))
                .ForMember(dest => dest.PurchaseOrderDetails, src => src.MapFrom(x => x.CuPurchaseOrderDetails.Where(y => y.Active.HasValue && y.Active.Value)));




            CreateMap<InspectionBookingDetails, InspTransaction>()
                 .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => _ApplicationContext.UserId))
                 .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now));

            CreateMap<SaveInsepectionRequest, InspTransaction>()
                        .ForMember(dest => dest.ServiceDateFrom, src => src.MapFrom(x => x.ServiceDateFrom.ToDateTime()))
                        .ForMember(dest => dest.ServiceDateTo, src => src.MapFrom(x => x.ServiceDateTo.ToDateTime()))
                        .ForMember(dest => dest.FirstServiceDateFrom, src => src.MapFrom(x => x.FirstServiceDateFrom.ToDateTime()))
                        .ForMember(dest => dest.FirstServiceDateTo, src => src.MapFrom(x => x.FirstServiceDateTo.ToDateTime()))
                        .ForMember(dest => dest.ShipmentDate, src => src.MapFrom(x => x.ShipmentDate.ToDateTime()))
                        .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now));


            CreateMap<InspTransaction, InspectionBookingDetails>().ReverseMap();

            CreateMap<InspectionPODetails, InspPoTransaction>()
                 .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => _ApplicationContext.UserId))
                 .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now))
                 .ForMember(dest => dest.Active, src => src.MapFrom(x => true));

            CreateMap<SaveInspectionPODetails, InspPoTransaction>()
              .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => _ApplicationContext.UserId))
              .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now))
              .ForMember(dest => dest.Active, src => src.MapFrom(x => true));

            CreateMap<InspPoTransaction, InspectionPODetails>();

            CreateMap<InspectionCustomerContact, InspTranCuContact>()
                 .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => _ApplicationContext.UserId))
                 .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now))
                 .ForMember(dest => dest.Active, src => src.MapFrom(x => true));

            CreateMap<InspTranCuContact, InspectionCustomerContact>();

            CreateMap<InspectionSupplierContact, InspTranSuContact>()
                 .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => _ApplicationContext.UserId))
                 .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now))
                 .ForMember(dest => dest.Active, src => src.MapFrom(x => true));

            CreateMap<InspTranSuContact, InspectionSupplierContact>();

            CreateMap<InspectionFactoryContact, InspTranFaContact>()
                 .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => _ApplicationContext.UserId))
                 .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now))
                 .ForMember(dest => dest.Active, src => src.MapFrom(x => true));

            CreateMap<InspTranFaContact, InspectionFactoryContact>();

            CreateMap<InspectionServiceType, InspTranServiceType>()
                 .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => _ApplicationContext.UserId))
                 .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => DateTime.Now))
                 .ForMember(dest => dest.Active, src => src.MapFrom(x => true));

            CreateMap<InspTranServiceType, InspectionServiceType>();

            CreateMap<InspTransaction, InspectionBookingDetails>()
                .ForMember(dest => dest.InspectionFileAttachmentList, src => src.MapFrom(x => x.InspTranFileAttachments))
                .ForMember(dest => dest.InspectionCustomerContactList, src => src.MapFrom(x => x.InspTranCuContacts))
                .ForMember(dest => dest.InspectionSupplierContactList, src => src.MapFrom(x => x.InspTranSuContacts))
                .ForMember(dest => dest.InspectionFactoryContactList, src => src.MapFrom(x => x.InspTranFaContacts))
                .ForMember(dest => dest.InspectionPoList, src => src.MapFrom(x => x.InspPurchaseOrderTransactions))
                .ForMember(dest => dest.InspectionServiceTypeList, src => src.MapFrom(x => x.InspTranServiceTypes));


            CreateMap<Language, CustomerSource>().ReverseMap();

            CreateMap<RefProspectStatus, CustomerSource>().ReverseMap();

            CreateMap<RefMarketSegment, CustomerSource>().ReverseMap();

            CreateMap<RefBusinessType, CustomerSource>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name));

            CreateMap<RefAddressType, CustomerSource>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name)); ;

            CreateMap<RefInvoiceType, CustomerSource>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name)); ;

            CreateMap<AudWorkProcess, AuditWorkProcess>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name));

            CreateMap<CuRefAccountingLeader, CustomerSource>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name));

            CreateMap<CuRefBrandPriority, CustomerSource>()
               .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name));

            CreateMap<CuRefActivitiesLevel, CustomerSource>()
               .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name));

            CreateMap<CuRefRelationshipStatus, CustomerSource>()
               .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name));
        }
    }
}

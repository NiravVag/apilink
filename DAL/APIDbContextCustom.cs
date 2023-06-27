using DAL.Reflexion;
using DTO.Common;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.DynamicFilters;
using Contracts.Managers;

namespace DAL
{
    public partial class API_DBContext
    {
        private ITenantProvider _filterService = null;

        public API_DBContext(DbContextOptions<API_DBContext> options, ITenantProvider filterService)
       : base(options)
        {
            _filterService = filterService;
        }

        private void FilterEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RefServiceType>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<RefProductCategory>()
                 .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<RefProductCategorySub>()
                 .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<RefProductCategorySub2>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<RefLocation>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EmExchangeRate>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuServiceType>()
               .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuBuyerApiService>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspLabDetail>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuBuyer>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuProduct>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrOfficeControl>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuBrand>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<RefMarketSegment>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<DfCuConfiguration>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCollection>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCheckPoint>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCheckPointsBrand>()
          .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuDepartment>()
         .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCheckPointsDepartment>()
          .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCheckPointsDepartment>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCheckPointsServiceType>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            //modelBuilder.Entity<RefBillingEntity>()
            //  .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvRefBank>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvRefOffice>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuPriceCategory>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuPrDetail>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrDepartment>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());
            modelBuilder.Entity<RefExpertise>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EcExpencesClaim>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EcExpensesClaimDetai>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrLeave>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrHoliday>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<DaUserCustomer>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuPurchaseOrder>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuPurchaseOrderDetail>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<QcBlockList>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspTransaction>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspProductTransaction>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspPurchaseOrderTransaction>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspContainerTransaction>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<AudTransaction>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<QuQuotation>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvTmDetail>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvAutTranDetail>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvExfTransaction>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspIcTransaction>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EsSuTemplateMaster>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EsDetail>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<ItUserRole>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());
            modelBuilder.Entity<RefCityDetail>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());
            modelBuilder.Entity<RefBudgetForecast>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());
            modelBuilder.Entity<MidTask>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());
            modelBuilder.Entity<MidNotification>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());
            modelBuilder.Entity<RefZone>()
          .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EcFoodAllowance>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EcAutTravelTariff>()
             .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EntFeatureDetail>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<DmDetail>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<DmFile>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<DmRefModule>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<DmRight>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvManTransaction>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvDisTranDetail>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvCreTransaction>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrOutSourceCompany>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());


            modelBuilder.Entity<EntMasterConfig>()
              .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<ClmTransaction>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspBookingRule>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EcAutRefStartPort>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EcAutQcTravelExpense>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EcAutQcFoodExpense>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCustomer>()
            .HasQueryFilter(x => x.CuEntities.Any(x => x.Active == true && x.EntityId == _filterService.GetCompanyId()));

            modelBuilder.Entity<SuSupplier>()
          .HasQueryFilter(x => x.SuEntities.Any(x => x.Active == true && x.EntityId == _filterService.GetCompanyId()));

            modelBuilder.Entity<ItRight>()
            .HasQueryFilter(x => x.ItRightEntities.Any(x => x.Active == true && x.EntityId == _filterService.GetCompanyId()));

            modelBuilder.Entity<ItRightEntity>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrStaff>().HasQueryFilter(x => x.HrEntityMaps.Any(x => x.EntityId == _filterService.GetCompanyId()));

            modelBuilder.Entity<CuContact>().HasQueryFilter(x => x.CuContactEntityMaps.Any(x => x.EntityId == _filterService.GetCompanyId()));

            modelBuilder.Entity<SuContact>().HasQueryFilter(x => x.SuContactEntityMaps.Any(x => x.EntityId == _filterService.GetCompanyId()));

            modelBuilder.Entity<InspTransactionDraft>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspRepCusDecisionTemplate>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<AudTranStatusLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EventBookingLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<FbBookingRequestLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<FbReportManualLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InspTranStatusLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvAutTranStatusLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvExfTranStatusLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<JobScheduleLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            // commented the below due to the email sending issue
            //modelBuilder.Entity<LogBookingReportEmailQueue>()
            //.HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            //modelBuilder.Entity<LogEmailQueue>()
            //.HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            //modelBuilder.Entity<LogEmailQueueAttachment>()
            //.HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<QuTranStatusLog>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<RefBillingEntity>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<RefSeason>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuSeasonConfig>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<EsSuPreDefinedField>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrPosition>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<FbReportTemplate>()
           .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<HrPayrollCompany>()
           .HasQueryFilter(x => x.Entity == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCheckPointsCountry>()
            .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            // modelBuilder.Entity<HrEmployeeType>()
            //.HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuSisterCompany>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuContactSisterCompany>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<SuGrade>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<CuCustomerGroup>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<DmRole>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<AudCuProductCategory>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            modelBuilder.Entity<InvDaTransaction>()
                .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());

            // don't add this table because we are using the Id to fetch the data
            //modelBuilder.Entity<EmExchangeRateType>()
            //    .HasQueryFilter(x => x.EntityId == _filterService.GetCompanyId());
        }


        partial void OnModelCreatingExt(ModelBuilder modelBuilder)
        {
            FilterEntity(modelBuilder);
        }
    }
}

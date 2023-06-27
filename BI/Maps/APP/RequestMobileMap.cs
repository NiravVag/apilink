using DTO.Common;
using DTO.Dashboard;
using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.MobileApp;
using DTO.RepoRequest.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Maps.APP
{
    public static class RequestMobileMap
    {
        public static CustomerDashboardFilterRequest MapDashboardRequest(InspDashboardMobileRequest request)
        {
            var response = new CustomerDashboardFilterRequest()
            {
                CustomerId = request.customerId,
                ServiceDateFrom = !CheckDate(request.serviceFromDate) ? Static_Data_Common.GetCustomDate(DateTime.Today.AddMonths(-1)) : request.serviceFromDate,
                ServiceDateTo = !CheckDate(request.serviceToDate) ? Static_Data_Common.GetCustomDate(DateTime.Today) : request.serviceToDate
            };

            return response;
        }

        public static InspectionSummarySearchRequest MapInspRequest(InspSummaryMobileRequest request)
        {
            var response = new InspectionSummarySearchRequest()
            {
                SearchTypeId = request.searchTypeId.GetValueOrDefault(),
                SearchTypeText = request.searchTypeText,
                DateTypeid = !(request.dateTypeid > 0 )? (int)SearchType.ServiceDate : request.searchTypeId.GetValueOrDefault(),
                CustomerId = request.customerId,
                FromDate = !CheckDate(request.fromDate) ? Static_Data_Common.GetCustomDate(DateTime.Today.AddMonths(-1)) : Static_Data_Common.GetCustomDate(request.fromDate.ToDateTime()),
                ToDate = !CheckDate(request.toDate) ? Static_Data_Common.GetCustomDate(DateTime.Today) : Static_Data_Common.GetCustomDate(request.toDate.ToDateTime()),
                SupplierId = request.supplierId,
                FactoryIdlst = request.factoryIdlst,
                StatusIdlst = request.statusIdlst,
                ServiceTypelst = request.serviceTypelst,
                pageSize = request.pageSize,
                Index = request.pageIndex,
                AdvancedSearchtypeid = "0",
                AdvancedSearchtypetext = "",
                IsQuotationSearch = false,
                QuotationsStatusIdlst = request.quotationStatusIdlst,
                quotationId=request.quotationId,
                BookingType = request.BookingType
            };

            return response;
        }

        public static CustomerDecisionSaveRequest MapCustomerDecisionRequest(CustomerDecisionMobileSaveRequest request)
        {
            var response = new CustomerDecisionSaveRequest()
            {
                ReportId = request.reportId,
                CustomerResultId = request.resultId,
                Comments = request.resultComments
            };

            return response;
        }

        private static bool CheckDate(DateObject date)
        {
            try
            {
                var dt = date.ToDateTime();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}

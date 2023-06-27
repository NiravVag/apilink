using DTO.Expense;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DTO.Common;
using DTO.HumanResource;
using DTO.CommonClass;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class ExpenseMap : ApiCommonData
    {
        public ExpenseTypeSource GetExpenseType(EcExpensesType entity)
        {
            if (entity == null)
                return null;

            return new ExpenseTypeSource
            {
                Id = entity.Id,
                Label = entity.TypeTransId.GetTranslation(entity.Description),
                IsTravel = entity.IsTravel != null ? entity.IsTravel.Value : false
            };

        }

        public ExpenseClaimGroup GetExpenseClaimGroup(IEnumerable<ExpenseDataRepo> entityList, int staffId, IEnumerable<int> roleList, IEnumerable<StaffInfo> stafflist, IEnumerable<EcExpenseClaimtype> claimTypeList,
       List<ExpenseDetailsRepo> expenseDetailItems)
        {
            if (entityList == null)
                return null;

            var items = entityList.Select(x => GetExpenseClaim(x, staffId, roleList, stafflist, claimTypeList, expenseDetailItems));

            return new ExpenseClaimGroup
            {
                Items = items,
                ExpenseAmout = Math.Round(items.Sum(x => x.ExpenseAmout == null ? 0 : x.ExpenseAmout.Value), 2),
                FoodAllowance = Math.Round(items.Sum(x => x.FoodAllowance == null ? 0 : x.FoodAllowance.Value), 2),
                TotalAmount = Math.Round(items.Sum(x => x.TotalAmount == null ? 0 : x.TotalAmount.Value), 2)
            };
        }

        private (string, string) GetUserStatus(EcExpencesClaim entity)
        {
            switch ((ExpenseClaimStatus)entity.StatusId)
            {
                case ExpenseClaimStatus.Approved:
                    return (entity.Approved?.FullName, entity.ApprovedDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Checked:
                    return (entity.Checked?.FullName, entity.CheckedDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Paid:
                    return (entity.Paid?.FullName, entity.PaidDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Rejected:
                    return (entity.Reject?.FullName, entity.RejectDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Cancelled:
                    return (entity.Cancel?.FullName, entity.CancelDate?.ToString(StandardDateFormat));
                default:
                    return ("", "");
            }

        }

        public ExpenseClaim GetExpenseClaim(EcExpencesClaim entity, IEnumerable<ExpenseClaimReceipt> files, Func<string, string> _funcGetMimeType,
                        List<ExpenseQCPort> travelPortList)
        {
            if (entity == null)
                return null;

            var statusData = GetUserStatus(entity);

            var activeExpenseClaimDetails = entity.EcExpensesClaimDetais?.Where(x => x.Active.Value).OrderBy(x => x.ExpenseDate).ToList();

            return new ExpenseClaim
            {
                ClaimDate = entity.ClaimDate.GetCustomDate(),
                ClaimNo = entity.ClaimNo,
                CountryId = entity.CountryId,
                CountryName = entity.Country?.CountryName,
                CurrencyId = entity.Staff?.PayrollCurrencyId,
                CurrencyName = entity.Staff?.PayrollCurrency?.CurrencyName,
                ExpensePuropose = entity.ExpensePurpose,
                Id = entity.Id,
                LocationId = entity.LocationId,
                LocationName = entity.Location.LocationName,
                Name = entity.Staff?.PersonName,
                StaffId = entity.StaffId,
                StatusId = entity.StatusId,
                Status = entity.Status?.Description,
                StatusUserName = statusData.Item1,
                StatusDate = statusData.Item2,
                ExpenseList = activeExpenseClaimDetails?.Select(x => GetExpenseDetails(x, files, _funcGetMimeType, entity.ClaimTypeId, travelPortList)),
                StaffEmail = entity.Staff?.CompanyEmail,
                TotalAmount = activeExpenseClaimDetails.Sum(x => x.Amount),
                FoodAllowance = activeExpenseClaimDetails?.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.FoodAllowance).Sum(x => x.Amount),
                ExpenseAmout = activeExpenseClaimDetails?.Where(x => x.ExpenseTypeId != (int)Entities.Enums.ExpenseType.FoodAllowance).Sum(x => x.Amount),
                Comment = entity.Comment,
                ClaimTypeId = entity.ClaimTypeId ?? null,
                IsAutoExpense = activeExpenseClaimDetails?.Any(x => x.IsAutoExpense.GetValueOrDefault()),
                IsTraveAllowanceExist = activeExpenseClaimDetails?.
                                              Any(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingOtherModes),
                IsFoodAllowanceExist = activeExpenseClaimDetails?.
                                              Any(x => x.ExpenseTypeId == (int)ClaimExpenseType.FoodAllowance),
                CreatedOn = entity.CreatedDate.ToString(StandardDateFormat),
                EmployeeTypeId = entity.Staff?.EmployeeTypeId,
                EmployeeType = entity.Staff?.EmployeeType?.EmployeeTypeName
            };
        }

        public ExpenseClaimDetails GetExpenseDetails(EcExpensesClaimDetai entity, IEnumerable<ExpenseClaimReceipt> files, Func<string, string> _funcGetMimeType, int? claimTypeId,
            List<ExpenseQCPort> travelPortList)
        {
            if (entity == null)
                return null;

            LocationMap LocationMap = new LocationMap();

            var travelData = travelPortList.FirstOrDefault(x => x.TravelQCExpenseId == entity.QcTravelExpenseId);

            return new ExpenseClaimDetails
            {
                Id = entity.Id,
                ActualAmount = entity.AmmountHk != null ? entity.AmmountHk.Value : 0,
                Amount = entity.Amount,
                CurrencyId = entity.CurrencyId,
                CurrencyName = entity.Currency?.CurrencyName,
                Description = entity.Description,
                DestCity = LocationMap.GetCity(entity.ArrivalCity),
                ExchangeRate = entity.ExchangeRate,
                ExpenseDate = entity.ExpenseDate.GetCustomDate(),
                ExpenseTypeId = entity.ExpenseTypeId,
                ExpenseTypeLabel = entity.ExpenseType?.Description,
                TripMode = entity.TripType,
                Receipt = entity.Receipt,
                StartCity = LocationMap.GetCity(entity.StartCity),
                QcId = entity.Expense?.Staff?.Id,
                QcName = entity.Expense?.Staff?.PersonName,
                Tax = entity.Tax,
                TaxAmount = entity.TaxAmount,
                ManDay = entity.ManDay,
                Files = files?.Where(x => x.ExpenseId == entity.Id)?.Select(x => GetFile(x, _funcGetMimeType)),
                BookingNo = (claimTypeId == (int)ClaimTypeEnum.Inspection) ?
                                                entity.InspectionId :
                             (claimTypeId == (int)ClaimTypeEnum.Audit) ?
                                                entity.AuditId : 0,
                StartPortName = travelData?.StartPort,
                EndPortName = travelData?.EndPort
            };
        }

        public ExpenseStatus GetStatus(EcExpClaimStatus entity)
        {
            if (entity == null)
                return null;
            return new ExpenseStatus
            {
                Id = entity.Id,
                Label = entity.TranId.GetTranslation(entity.Description)
            };
        }

        private ExpenseClaimReceipt GetFile(ExpenseClaimReceipt entity, Func<string, string> _funcGetMimeType)
        {
            if (entity == null)
                return null;

            return new ExpenseClaimReceipt
            {
                Id = entity.Id,
                FileName = entity.FileName,
                GuidId = entity.GuidId,
                IsNew = false,
                FileUrl = entity.FileUrl,
                MimeType = _funcGetMimeType(Path.GetExtension(entity.FileName)),
                Uniqueld = entity.Uniqueld
            };
        }

        public ExpenseClaimType GetExpenseClaimType(EcExpenseClaimtype entity)
        {
            if (entity == null)
                return null;
            return new ExpenseClaimType
            {
                Id = entity.Id,
                Name = entity.Name,
                Active = entity.Active
            };
        }

        //map the allowance and expense data of the claims
        public ExpenseClaimVoucherData ExportExpenseVoucherMap(ExpenseClaimListRequest request, List<ExpenseClaimVoucherItem> data, List<ExpenseBookingData> bookingData, List<ExpenseAuditData> auditData)
        {
            ExpenseClaimVoucherData response = new ExpenseClaimVoucherData();
            List<ExpenseVoucherExportItem> claimExpenseData = new List<ExpenseVoucherExportItem>();

            response.DateFrom = request.StartDate.ToDateTime().ToString(StandardDateFormat1);
            response.DateTo = request.EndDate.ToDateTime().ToString(StandardDateFormat1);
            response.Office = request.LocationValues != null && request.LocationValues.Distinct().Count() == 1 ? data.Where(x => x.LocationId == request.LocationValues.Select(y => y.Id).FirstOrDefault()).Select(x => x.LocationName).FirstOrDefault() : all;
            response.EnglishName = data.Select(x => x.StaffName).FirstOrDefault();
            response.RegionalName = data.Select(x => x.RegionalName).FirstOrDefault();
            response.ClaimType = data.Select(x => x.ClaimTypeId).Distinct().Count() > 1 ? all : data.Select(x => x.ClaimType).FirstOrDefault();

            //loop each claim id
            foreach (var id in request.ClaimIdList)
            {
                int count = 0;
                var claimData = data.Where(x => x.Id == id).ToList();
                var bookingIdList = bookingData?.Where(x => x.ClaimId == id).Where(x => x.BookingId != null)
                                        .Select(x => x.BookingId.GetValueOrDefault()).Distinct();

                var auditIdList = auditData?.Where(x => x.ClaimId == id).Where(x => x.AuditId != null)
                                        .Select(x => x.AuditId.GetValueOrDefault()).Distinct();

                List<int> bookingAuditIdList = new List<int>();
                List<int> claimDetailIdList = new List<int>();

                //get the distinct booking Id count and booking Ids
                if (bookingIdList != null && bookingIdList.Any())
                {
                    count = bookingIdList.Count();
                    bookingAuditIdList.AddRange(bookingIdList.Distinct());
                }

                //get the distinct audit Id count and audit Ids
                if (auditIdList != null && auditIdList.Any())
                {
                    count = auditIdList.Count();
                    bookingAuditIdList.AddRange(auditIdList.Distinct());
                }

                //if non inspection
                if ((bookingIdList == null && auditIdList == null) || (!bookingIdList.Any() && !auditIdList.Any()))
                {
                    count = 1;
                    claimDetailIdList = claimData.Select(x => x.ClaimDetailId).Distinct().ToList();
                }

                //loop each booking/ audit no
                for (int i = 0; i < count; i++)
                {
                    if (claimData.Select(x => x.ClaimTypeId).FirstOrDefault() == (int)ExpenseBookingDetailEnum.Inspection)
                    {
                        claimDetailIdList = bookingData?.Where(x => x.ClaimId == id && x.BookingId == bookingAuditIdList[i]).Select(x => x.ClaimDetailId).ToList();
                    }

                    else if (claimData.Select(x => x.ClaimTypeId).FirstOrDefault() == (int)ExpenseBookingDetailEnum.Audit)
                    {
                        claimDetailIdList = auditData?.Where(x => x.ClaimId == id && x.AuditId == bookingAuditIdList[i]).Select(x => x.ClaimDetailId).ToList();
                    }

                    //get the claim details based on claimdetail Id
                    var item = claimData.Where(x => claimDetailIdList.Contains(x.ClaimDetailId)).ToList();

                    var travelAllowance = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravelAllowance).Sum(x => x.Amount.GetValueOrDefault());
                    var reportAllowance = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.ReportAllowance).Sum(x => x.Amount.GetValueOrDefault());
                    var planeExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByPlane).Sum(x => x.Amount.GetValueOrDefault());
                    var ferryExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByFerry).Sum(x => x.Amount.GetValueOrDefault());
                    var taxiExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTaxi).Sum(x => x.Amount.GetValueOrDefault());
                    var trainExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTrain).Sum(x => x.Amount.GetValueOrDefault());
                    var busExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByBus).Sum(x => x.Amount.GetValueOrDefault());
                    var otherTravelExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingOtherModes).Sum(x => x.Amount.GetValueOrDefault());
                    var hotelExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.HotelExpenses).Sum(x => x.Amount.GetValueOrDefault());
                    var airportExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.AirportTaxes).Sum(x => x.Amount.GetValueOrDefault());
                    var visaExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.Visa).Sum(x => x.Amount.GetValueOrDefault());
                    var otherExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.OtherExpenses).Sum(x => x.Amount.GetValueOrDefault());
                    var miscExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.MiscellaneousExpense).Sum(x => x.Amount.GetValueOrDefault());
                    var entertainmentExpense = item.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.Entertainment).Sum(x => x.Amount.GetValueOrDefault());

                    var allowanceTotal = travelAllowance + reportAllowance;
                    var tavelFeeTotal = planeExpense + ferryExpense + taxiExpense + trainExpense + busExpense + otherTravelExpense;

                    var isAutoExpense = item.Any(x => x.IsAutoExpense.GetValueOrDefault());

                    claimExpenseData.Add(new ExpenseVoucherExportItem()
                    {
                        Id = id,
                        Date = item.Select(x => x.Date).FirstOrDefault().ToString(StandardDateFormat1),
                        InspNo = bookingIdList != null && bookingIdList.Any() ? bookingAuditIdList[i].ToString() : "",
                        AuditNo = auditIdList != null && auditIdList.Any() ? bookingAuditIdList[i].ToString() : "",
                        TravelAllowance = Math.Round(travelAllowance, 2),
                        ReportAllowance = Math.Round(reportAllowance, 2),
                        AddOrDeductAllowance = 0,
                        AllowanceTotal = Math.Round(allowanceTotal, 2),
                        PlaneTravel = Math.Round(planeExpense, 2),
                        FerryTravel = Math.Round(ferryExpense, 2),
                        TaxiTravel = Math.Round(taxiExpense, 2),
                        DdTravel = 0,
                        TrainTravel = Math.Round(trainExpense, 2),
                        BusTravel = Math.Round(busExpense, 2),
                        OtherTravel = Math.Round(otherTravelExpense, 2),
                        HotelExpense = Math.Round(hotelExpense, 2),
                        AirportTax = Math.Round(airportExpense, 2),
                        Visa = Math.Round(visaExpense, 2),
                        OtherExpense = Math.Round(otherExpense, 2),
                        MiscellaneousExpense = Math.Round(miscExpense, 2),
                        EntertainmentExpense = Math.Round(entertainmentExpense, 2),
                        TravelFeeTotal = Math.Round(tavelFeeTotal, 2),
                        Total = Math.Round(item.Sum(x => x.Amount.GetValueOrDefault()), 2),
                        ClaimNo = item.Select(x => x.ClaimNo).FirstOrDefault(),
                        ExpenseStatus = item.Select(x => x.Status).FirstOrDefault(),
                        OfficeName = item.Select(x => x.LocationName).FirstOrDefault(),
                        PayrollCompanyName = item.Select(x => x.PayrollCompanyName).FirstOrDefault(),
                        AutoExpense = isAutoExpense ? ExpenseSumaryYes : ExpenseSumaryNo
                    });
                }
            }

            response.ClaimData = claimExpenseData;

            return response;
        }

        //map the claim data
        public ExportExpenseClaimSummaryKpiResponse ExportExpenseClaimSummaryKpisMap(ExpenseExportKPIMap expenseExportKPIMap)
        {
            ExportExpenseClaimSummaryKpiResponse response = new ExportExpenseClaimSummaryKpiResponse();
            List<ExportExpenseClaimSummaryKpi> res = new List<ExportExpenseClaimSummaryKpi>();

            response.FromDate = expenseExportKPIMap.request.StartDate.ToDateTime().ToString(StandardDateFormat);
            response.ToDate = expenseExportKPIMap.request.EndDate.ToDateTime().ToString(StandardDateFormat);

            var claimdIds = expenseExportKPIMap.expenseClaimList.Select(x => x.Id).Distinct().ToList();

            foreach (var id in claimdIds)
            {
                string customerNames = string.Empty;
                string countryNames = string.Empty;

                var customerNameList = new List<string>();
                var countryNameList = new List<string>();

                var claimData = expenseExportKPIMap.expenseClaimList.Where(x => x.Id == id).ToList();

                var claimInspIds = claimData.Where(x => x.InspectionId > 0).Select(x => x.InspectionId).ToList();
                var claimAuditIds = claimData.Where(x => x.AuditId > 0).Select(x => x.AuditId).ToList();

                if (claimInspIds != null && claimInspIds.Any())
                {
                    customerNameList = expenseExportKPIMap.InspectionBookingList.Where(x => claimInspIds.Contains(x.BookingId)).Select(x => x.CustomerName).ToList();
                    countryNameList = expenseExportKPIMap.InspFactoryAddressList.Where(x => claimInspIds.Contains(x.BookingId)).Select(x => x.CountryName).ToList();
                }
                else if (claimAuditIds != null && claimAuditIds.Any())
                {
                    customerNameList = expenseExportKPIMap.AuditBookingList.Where(x => claimAuditIds.Contains(x.AuditId)).Select(x => x.CustomerName).ToList();
                    countryNameList = expenseExportKPIMap.AuditFactoryAddressList.Where(x => claimAuditIds.Contains(x.BookingId)).Select(x => x.CountryName).ToList();
                }

                if (customerNameList.Any())
                {
                    customerNames = string.Join(", ", customerNameList);
                }
                if (countryNameList.Any())
                {
                    countryNames = string.Join(", ", countryNameList);
                }

                var travelAllowance = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravelAllowance).Sum(x => x.Amount.GetValueOrDefault());
                var reportAllowance = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.ReportAllowance).Sum(x => x.Amount.GetValueOrDefault());
                var planeExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByPlane).Sum(x => x.Amount.GetValueOrDefault());
                var ferryExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByFerry).Sum(x => x.Amount.GetValueOrDefault());
                var taxiExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTaxi).Sum(x => x.Amount.GetValueOrDefault());
                var trainExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTrain).Sum(x => x.Amount.GetValueOrDefault());
                var busExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByBus).Sum(x => x.Amount.GetValueOrDefault());
                var phoneExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.Phone).Sum(x => x.Amount.GetValueOrDefault());
                var visaExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.Visa).Sum(x => x.Amount.GetValueOrDefault());
                var hotelExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.HotelExpenses).Sum(x => x.Amount.GetValueOrDefault());
                var mandayCost = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.MandayCost).Sum(x => x.Amount.GetValueOrDefault());
                var foodAllowance = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.FoodAllowance).Sum(x => x.Amount.GetValueOrDefault());

                var otherTravelExpense = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingOtherModes).Sum(x => x.Amount.GetValueOrDefault());
                var totalTravelExpense = planeExpense + ferryExpense + taxiExpense + trainExpense + busExpense + otherTravelExpense;

                var expenseTotal = totalTravelExpense + travelAllowance + reportAllowance + phoneExpense + visaExpense + hotelExpense + mandayCost+ foodAllowance;

                var totalAmt = Math.Round(claimData.Sum(x => x.Amount.GetValueOrDefault()), 2);
                var otherExpense = Math.Round(totalAmt - expenseTotal, 2);

                var isAutoExpense = claimData.Any(x => x.IsAutoExpense.GetValueOrDefault());

                res.Add(new ExportExpenseClaimSummaryKpi
                {
                    Location = claimData.Select(x => x.LocationName).FirstOrDefault(),
                    DeptName = expenseExportKPIMap.deptData?.Where(x => x.Id == claimData.Select(y => y.DeptId).FirstOrDefault()).Select(x => x.Name).FirstOrDefault(),
                    ClaimDate = claimData.Select(x => x.Date).FirstOrDefault().ToString(StandardDateFormat),
                    //ToDate = expenseData.DateTo,
                    ClaimNo = claimData.Select(x => x.ClaimNo).FirstOrDefault(),
                    EmpName = claimData.Select(x => x.StaffName).FirstOrDefault(),
                    RegionalEmpName = claimData.Select(x => x.RegionalName).FirstOrDefault(),
                    BankAccNo = claimData.Select(x => x.BankAccountNo).FirstOrDefault(),
                    TotalTravelExpense = totalTravelExpense,
                    TravelAllowance = travelAllowance,
                    ReportAllowance = reportAllowance,
                    OtherExpense = otherExpense,
                    TotalAmt = totalAmt,
                    PayrollCurrency = claimData.Select(x => x.PayrollCurrency).FirstOrDefault(),
                    ExpenseStatus = claimData.Select(x => x.Status).FirstOrDefault(),
                    ClaimType = claimData.Select(x => x.ClaimType).FirstOrDefault(),
                    PaymentStatus = claimData.Select(x => x.PaidId).FirstOrDefault() > 0 ? paid : notPaid,
                    PayrollCompanyName = claimData.Select(x => x.PayrollCompanyName).FirstOrDefault(),
                    ExpenseDate = string.Join(", ", claimData.Select(x => x.ExpenseDate.ToString(StandardDateFormat)).Distinct().ToList()),
                    CustomerName = customerNames,
                    Country = countryNames,
                    MandayCost = mandayCost,
                    HotelExpense = hotelExpense,
                    Phone = phoneExpense,
                    Visa = visaExpense,
                    AutoExpense = isAutoExpense ? ExpenseSumaryYes : ExpenseSumaryNo,
                    FoodAllowance = foodAllowance
                });
            }

            response.Data = res;
            return response;
        }

        //map the expense details kpi data
        public List<ExportExpenseSummaryDetailKpi> ExportExpenseDetailsKpiMap(ExpenseExportKPIMap expenseExportKPIMap)
        {
            List<ExportExpenseSummaryDetailKpi> res = new List<ExportExpenseSummaryDetailKpi>();
            
            //get claim ids
            var claimIds = expenseExportKPIMap.expenseClaimList.Select(x => x.Id).Distinct().ToList();

            //looping the claim ids
            foreach (var claimId in claimIds)
            {
                var claimData = expenseExportKPIMap.expenseClaimList.Where(x => x.Id == claimId).ToList();
                
                //total amount of claim
                double? totalAmt = Math.Round(claimData.Sum(x => x.Amount.GetValueOrDefault()), 2);
                
                var otherExp = totalAmt - CalculateExpense(claimData);

                double? otherExpense = otherExp == 0 ? null : Math.Round(otherExp.GetValueOrDefault(), 2);

                //get claim details ids
                var claimDetailIds = expenseExportKPIMap.expenseClaimList.Where(x => x.Id == claimId).Select(x => x.ClaimDetailId).Distinct().ToList();

                //loop the claim details 
                foreach (var claimDetailId in claimDetailIds)
                {
                    ExpenseBookingDataMap expenseBookingData = new ExpenseBookingDataMap();

                    var claimDetailData = expenseExportKPIMap.expenseClaimList.Where(x => x.ClaimDetailId == claimDetailId).FirstOrDefault();

                    //first row insert actual amt rest all zero
                    if (res.Where(x => x.ClaimNumber == claimDetailData.ClaimNo).Any())
                    {
                        totalAmt = null;
                        otherExpense = null;
                    }

                    var claimInspId = claimDetailData.InspectionId;
                    var claimAuditId = claimDetailData.AuditId;
                    
                    if (claimInspId != null && claimInspId > 0)
                    {
                       expenseBookingData = InspectionBookingMap(expenseExportKPIMap, claimInspId);
                    }
                    else if (claimAuditId != null && claimAuditId > 0)
                    {
                        expenseBookingData = AuditBookingMap(expenseExportKPIMap, claimAuditId);
                    }

                    var busExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.TravellingByBus ? claimDetailData.Amount : null;
                    var taxiExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTaxi ? claimDetailData.Amount : null;
                    var trainExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTrain ? claimDetailData.Amount : null;
                    var ferryExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.TravellingByFerry ? claimDetailData.Amount : null;
                    var planeExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.TravellingByPlane ? claimDetailData.Amount : null;
                    var otherTravelExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.TravellingOtherModes ? claimDetailData.Amount : null;

                    var hotelExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.HotelExpenses ? claimDetailData.Amount : null;
                    var foodAllowance = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.FoodAllowance ? claimDetailData.Amount : null;

                    var phoneExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.Phone ? claimDetailData.Amount : null;
                    var visaExpense = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.Visa ? claimDetailData.Amount : null;
                    var mandayCost = claimDetailData.ExpenseTypeId == (int)ClaimExpenseType.MandayCost ? claimDetailData.Amount : null;

                    res.Add(new ExportExpenseSummaryDetailKpi
                    {
                        ClaimNumber = claimDetailData.ClaimNo,
                        Country = expenseBookingData.CountryName,
                        Province = expenseBookingData.Province,
                        City = expenseBookingData.City,
                        County = expenseBookingData.County,
                        ExpenseDate = claimDetailData.ExpenseDate.ToString(StandardDateFormat),
                        FromCity = claimDetailData.FromCity,
                        ToCity = claimDetailData.ToCity,
                        StartPort = claimDetailData.StartPortName,
                        EndPort = expenseBookingData.Town,
                        Bus = busExpense,
                        Train = trainExpense,
                        Taxi = taxiExpense,
                        Ferry = ferryExpense,
                        Air = planeExpense,
                        TravelOtherModes = otherTravelExpense,
                        HotelExpense = hotelExpense,
                        FoodAllowance = foodAllowance,
                        Visa = visaExpense,
                        Phone = phoneExpense,
                        MandayCost = mandayCost,
                        Others = otherExpense,
                        Total = totalAmt,
                        Currency = claimDetailData.PayrollCurrency,
                        ExpenseStatus = claimDetailData.ExpenseStatus,
                        TripType = claimDetailData.TripTypeName,
                        Customer = expenseBookingData.CustomerName,
                        Factory = expenseBookingData.FactoryName,
                        ServiceType = expenseBookingData.ServiceTypeName,
                        ClaimInspectionNumber = claimInspId,
                        ClaimAuditNumber = claimAuditId,
                        BookingStatus = expenseBookingData.StatusName,
                        InspDate = expenseBookingData.InspDate?.ToString(StandardDateFormat),
                        EmployeeName = claimDetailData.StaffName
                    });
                }
            }
            return res;
        }

        public static double CalculateExpense(List<ExpenseClaimVoucherItem> claimData)
        {
            //sum the expense total
            var busExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByBus).Sum(x => x.Amount.GetValueOrDefault());
            var taxiExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTaxi).Sum(x => x.Amount.GetValueOrDefault());
            var trainExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByTrain).Sum(x => x.Amount.GetValueOrDefault());
            var ferryExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByFerry).Sum(x => x.Amount.GetValueOrDefault());
            var planeExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingByPlane).Sum(x => x.Amount.GetValueOrDefault());
            var otherTravelExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.TravellingOtherModes).Sum(x => x.Amount.GetValueOrDefault());
            var hotelExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.HotelExpenses).Sum(x => x.Amount.GetValueOrDefault());
            var foodAllowanceTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.FoodAllowance).Sum(x => x.Amount.GetValueOrDefault());
            var phoneExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.Phone).Sum(x => x.Amount.GetValueOrDefault());
            var visaExpenseTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.Visa).Sum(x => x.Amount.GetValueOrDefault());
            var mandayCostTotal = claimData.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.MandayCost).Sum(x => x.Amount.GetValueOrDefault());

            //total - above all mentioned expense
            var expenseTotal = busExpenseTotal + taxiExpenseTotal + trainExpenseTotal + ferryExpenseTotal + planeExpenseTotal +
                otherTravelExpenseTotal + hotelExpenseTotal + foodAllowanceTotal + phoneExpenseTotal +
                visaExpenseTotal + mandayCostTotal;

            return Math.Round(expenseTotal, 2);
        }

        public static ExpenseBookingDataMap InspectionBookingMap(ExpenseExportKPIMap expenseExportKPIMap, int? claimInspId)
        {
            var ExpenseBookingList = new ExpenseBookingDataMap();
            var inspData = expenseExportKPIMap.InspectionBookingList.Where(x => x.BookingId == claimInspId).FirstOrDefault();
            var factoryAddressData = expenseExportKPIMap.InspFactoryAddressList.Where(x => x.BookingId == claimInspId).FirstOrDefault();

            var inspServiceData = expenseExportKPIMap.InspServiceTypeList.Where(x => x.InspectionId == claimInspId).FirstOrDefault();

            if (inspData != null)
            {
                ExpenseBookingList.CustomerName = inspData.CustomerName;
                ExpenseBookingList.FactoryName = inspData.FactoryName;
                ExpenseBookingList.StatusName = inspData.StatusName;
                ExpenseBookingList.InspDate = inspData.ServiceFrom;

                if (factoryAddressData != null)
                {
                    ExpenseBookingList.CountryName = factoryAddressData.CountryName;
                    ExpenseBookingList.Province = factoryAddressData.ProvinceName;
                    ExpenseBookingList.City = factoryAddressData.CityName;
                    ExpenseBookingList.County = factoryAddressData.CountyName;
                    ExpenseBookingList.Town = factoryAddressData.TownName;
                }
            }

            if (inspServiceData != null)
            {
                ExpenseBookingList.ServiceTypeName = inspServiceData.serviceTypeName;
            }
            return ExpenseBookingList;
        }

        public static ExpenseBookingDataMap AuditBookingMap(ExpenseExportKPIMap expenseExportKPIMap, int? claimAuditId)
        {
            var ExpenseBookingList = new ExpenseBookingDataMap();

            var auditData = expenseExportKPIMap.AuditBookingList.Where(x => x.AuditId == claimAuditId).FirstOrDefault();
            var factoryAddressData = expenseExportKPIMap.AuditFactoryAddressList.Where(x => x.BookingId == claimAuditId).FirstOrDefault();

            var auditServiceData = expenseExportKPIMap.AuditServiceTypeList.Where(x => x.AuditId == claimAuditId).FirstOrDefault();

            if (auditData != null)
            {
                ExpenseBookingList.CustomerName = auditData.CustomerName;
                ExpenseBookingList.FactoryName = auditData.FactoryName;
                ExpenseBookingList.StatusName = auditData.StatusName;
                ExpenseBookingList.InspDate = auditData.ServiceFromDate;

                if (factoryAddressData != null)
                {
                    ExpenseBookingList.CountryName = factoryAddressData.CountryName;
                    ExpenseBookingList.Province = factoryAddressData.ProvinceName;
                    ExpenseBookingList.City = factoryAddressData.CityName;
                    ExpenseBookingList.County = factoryAddressData.CountyName;
                    ExpenseBookingList.Town = factoryAddressData.TownName;
                }
            }
            if (auditServiceData != null)
            {
                ExpenseBookingList.ServiceTypeName = auditServiceData.ServiceTypeName;

            }
            return ExpenseBookingList;
        }

        //expense summary map 
        public ExpenseClaim GetExpenseClaim(ExpenseDataRepo expenseData, int staffId, IEnumerable<int> roleList, IEnumerable<StaffInfo> stafflist, IEnumerable<EcExpenseClaimtype> claimTypeList,
         List<ExpenseDetailsRepo> expenseDetailItems)
        {
            if (expenseData == null)
                return null;

            var expenseDetailList = expenseDetailItems.Where(x => x.ExpenseId == expenseData.Id).ToList();

            var statusData = GetUserStatus(expenseData);
            var totalamount = expenseDetailList?.Sum(x => x.Amount) ?? 0;
            var totalfoodallowance = expenseDetailList?.Where(x => x.ExpenseTypeId == (int)ClaimExpenseType.FoodAllowance).Sum(x => x.Amount) ?? 0;
            var expamount = expenseDetailList?.Where(x => x.ExpenseTypeId != (int)ClaimExpenseType.FoodAllowance).Sum(x => x.Amount) ?? 0;
            //check the manager access
            bool ismanager = stafflist == null || !stafflist.Any(x => x.Id == expenseData.StaffId) ? false : true;

            var isAutoExpense = expenseDetailList?.Any(x => x.IsAutoExpense.GetValueOrDefault());

            return new ExpenseClaim
            {
                ClaimDate = expenseData.ClaimDate.GetCustomDate(),
                ClaimNo = expenseData.ClaimNo,
                CountryId = expenseData.CountryId,
                CurrencyId = expenseData.PayrollCurrencyId,
                CurrencyName = expenseData.PayrollCurrencyName,
                ExpensePuropose = expenseData.ExpensePurpose,
                Id = expenseData.Id,
                LocationId = expenseData.LocationId,
                LocationName = expenseData.LocationName,
                Name = expenseData.PersonName,
                StaffId = expenseData.StaffId,
                StatusId = expenseData.StatusId,
                Status = expenseData.StatusName,
                TotalAmount = Math.Round(totalamount, 2),
                FoodAllowance = Math.Round(totalfoodallowance, 2),
                ExpenseAmout = Math.Round(expamount, 2),
                StatusUserName = statusData.Item1,
                StatusDate = statusData.Item2,
                CanApprove = staffId != expenseData.StaffId && ismanager && roleList.Contains((int)RoleEnum.Management),
                Comment = expenseData.Comment,
                ClaimType = claimTypeList?.FirstOrDefault(x => x.Id == expenseData.ClaimTypeId)?.Name,
                IsAutoExpense = expenseData.IsAutoExpense.GetValueOrDefault(),
                CreatedOn = expenseData.CreatedDate.ToString(StandardDateFormat),
                EmployeeTypeId = expenseData.EmployeeTypeId,
                EmployeeType = expenseData.EmployeeTypeName,
                OutSourceCompanyName = expenseData.HrOutSourceCompanyName,
                PayrollCompanyName = expenseData.PayrollCompanyName,
                OfficeName = expenseData?.LocationName,
                AutoExpense = expenseData.IsAutoExpense.GetValueOrDefault() ? ExpenseSumaryYes : ExpenseSumaryNo,
                BookingIdList = expenseDetailList.Where(x => x.BookingId > 0).Select(x => x.BookingId).ToList()
            };
        }       

        //expense status
        private (string, string) GetUserStatus(ExpenseDataRepo expenseData)
        {
            switch ((ExpenseClaimStatus)expenseData.StatusId)
            {
                case ExpenseClaimStatus.Approved:
                    return (expenseData.ApproveByFullName, expenseData.ApprovedDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Checked:
                    return (expenseData.CheckedByFullName, expenseData.CheckedDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Paid:
                    return (expenseData.PaidByFullName, expenseData.PaidDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Rejected:
                    return (expenseData.RejectByFullName, expenseData.RejectDate?.ToString(StandardDateFormat));
                case ExpenseClaimStatus.Cancelled:
                    return (expenseData.CancelByFullName, expenseData.CancelDate?.ToString(StandardDateFormat));
                default:
                    return ("", "");
            }
        }
    }
}
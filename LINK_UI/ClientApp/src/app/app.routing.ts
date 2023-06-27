import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { AuthGuard } from './_Services/user/auth.guard';
import { MainComponent } from './layouts/main.component';
import { ErrorComponent } from './components/error/error.component'

const appRoutes: Routes = [
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    children: [

      {
        path: ':entity/auditreport',
        loadChildren: () => import('./components/Audit/audit-report/auditreport.module').then(m => m.AuditreportModule)
      },
      {
        path: ':entity/auditsummary',
        loadChildren: () => import('./components/Audit/audit-summary/auditsummary.module').then(m => m.AuditsummaryModule)
      },
      {
        path: ':entity/auditcancel',
        loadChildren: () => import('./components/Audit/cancel-audit/auditcancel.module').then(m => m.AuditcancelModule)
      },
      {
        path: ':entity/auditedit',
        loadChildren: () => import('./components/Audit/edit-audit/auditedit.module').then(m => m.AuditeditModule)
      },
      {
        path: ':entity/inspsummary',
        loadChildren: () => import('./components/booking/booking-summary/inspsummary.module').then(m => m.InspsummaryModule)
      },

      {
        path: ':entity/reportsummary',
        loadChildren: () => import('./components/booking/report-summary/reportsummary.module').then(m => m.ReportsummaryModule)
      },

      {
        path: ':entity/bookingDetail',
        loadChildren: () => import('./components/booking/booking-detail/bookingdetail.module').then(m => m.BookingDetailModule)
      },

      {
        path: ':entity/fullbridge',
        loadChildren: () => import('./components/fullbridge/fullbridge-summary/fullbridge-summary.module').then(m => m.FullbridgesummaryModule)
      },

      {
        path: ':entity/inspcancel',
        loadChildren: () => import('./components/booking/cancel-booking/inspcancel.module').then(m => m.InspcancelModule)
      },
      {
        path: ':entity/inspedit',
        loadChildren: () => import('./components/booking/edit-booking/inspedit.module').then(m => m.InspeditModule)
      },
      {
        path: ':entity/inspcombine',
        loadChildren: () => import('./components/booking/edit-combineproducts/inspcombine.module').then(m => m.InspcombineModule)
      },
      {
        path: ':entity/insppicking',
        loadChildren: () => import('./components/booking/edit-inspectionpicking/insppicking.module').then(m => m.InsppickingModule)
      },
      {
        path: ':entity/inspsplit',
        loadChildren: () => import('./components/booking/split-booking/inspsplit.module').then(m => m.InspsplitModule)
      },
      {
        path: ':entity/exchangerate',
        loadChildren: () => import('./components/currency/exchange-rate/exchangerate.module').then(m => m.ExchangerateModule)
      },
      {
        path: ':entity/ratematrix',
        loadChildren: () => import('./components/currency/rate-matrix/ratematrix.module').then(m => m.RatematrixModule)
      },
      {
        path: ':entity/csconfigedit',
        loadChildren: () => import('./components/customer/cs-config-register/csconfigreg.module').then(m => m.CsconfigregModule)
      },
      {
        path: ':entity/csconfigsearch',
        loadChildren: () => import('./components/customer/cs-config-summary/csconfigsearch.module').then(m => m.CsconfigsearchModule)
      },
      {
        path: ':entity/cusbuyer',
        loadChildren: () => import('./components/customer/customer-buyer/customerbuyer.module').then(m => m.CustomerbuyerModule)
      },
      {
        path: ':entity/cuscontactsearch',
        loadChildren: () => import('./components/customer/customer-contact-summary/cuscontactsearch.module').then(m => m.CuscontactsearchModule)
      },
      {
        path: ':entity/cusdep',
        loadChildren: () => import('./components/customer/customer-department/customerdep.module').then(m => m.CustomerdepModule)
      },
      {
        path: ':entity/cusproductsearch',
        loadChildren: () => import('./components/customer/customer-product-summary/cusproductsearch.module').then(m => m.CusproductsearchModule)
      },
      {
        path: ':entity/cusstylesearch',
        loadChildren: () => import('./components/customer/customer-style-summary/cusstylesearch.module').then(m => m.CusstylesearchModule)
      },
      {
        path: ':entity/cusservicesearch',
        loadChildren: () => import('./components/customer/customer-service-configuration-summary/cusservicesearch.module').then(m => m.CusservicesearchModule)
      },
      {
        path: ':entity/cussearch',
        loadChildren: () => import('./components/customer/customer-summary/customersearch.module').then(m => m.CustomersearchModule)
      },
      {
        path: ':entity/csallocation',
        loadChildren: () => import('./components/customer/cs-allocation/cs-allocation.module').then(m => m.CsAllocationModule)
      },
      {
        path: ':entity/cusedit',
        loadChildren: () => import('./components/customer/edit-customer/customeredit.module').then(m => m.CustomereditModule)
      },
      {
        path: ':entity/cusbrand',
        loadChildren: () => import('./components/customer/edit-customer-brand/customerbrand.module').then(m => m.CustomerbrandModule)
      },
      {
        path: ':entity/cuscollection',
        loadChildren: () => import('./components/customer/customer-collection/customer-collection.module').then(m => m.CustomerCollectionModule)
      },
      {
        path: ':entity/cuscontactedit',
        loadChildren: () => import('./components/customer/edit-customer-contact/cuscontactedit.module').then(m => m.CuscontacteditModule)
      },
      {
        path: ':entity/cusproductedit',
        loadChildren: () => import('./components/customer/edit-customer-product/cusproductedit.module').then(m => m.CusproducteditModule)
      },
      {
        path: ':entity/cusstyleedit',
        loadChildren: () => import('./components/customer/edit-customer-style/cusstyleedit.module').then(m => m.CusstyleeditModule)
      },
      {
        path: ':entity/cusserviceedit',
        loadChildren: () => import('./components/customer/edit-customer-service-configuration/cusserviceedit.module').then(m => m.CusserviceeditModule)
      },
      {
        path: ':entity/expenseclaim',
        loadChildren: () => import('./components/expense/expense-claim/expenseclaim.module').then(m => m.ExpenseclaimModule)
      },
      {
        path: ':entity/expensesearch',
        loadChildren: () => import('./components/expense/expense-claim-list/expensesearch.module').then(m => m.ExpensesearchModule)
      },
      {
        path: ':entity/staffedit',
        loadChildren: () => import('./components/hr/edit-staff/staffedit.module').then(m => m.StaffeditModule)
      },
      {
        path: ':entity/holiday',
        loadChildren: () => import('./components/hr/holiday-master/holiday.module').then(m => m.HolidayModule)
      },
      {
        path: ':entity/leaverequest',
        loadChildren: () => import('./components/hr/leave-request/leaverequest.module').then(m => m.LeaverequestModule)
      },
      {
        path: ':entity/leavesearch',
        loadChildren: () => import('./components/hr/leave-summary/leavesearch.module').then(m => m.LeavesearchModule)
      },
      {
        path: ':entity/officeconfig',
        loadChildren: () => import('./components/hr/office-control/officeconfig.module').then(m => m.OfficeconfigModule)
      },
      {
        path: ':entity/staffsearch',
        loadChildren: () => import('./components/hr/staff-summary/staffsearch.module').then(m => m.StaffsearchModule)
      },
      {
        path: ':entity/labedit',
        loadChildren: () => import('./components/lab/edit-lab/labedit.module').then(m => m.LabeditModule)
      },
      {
        path: ':entity/labsearch',
        loadChildren: () => import('./components/lab/lab-summary/labsearch.module').then(m => m.LabsearchModule)
      },
      {
        path: ':entity/city',
        loadChildren: () => import('./components/location/city/city.module').then(m => m.CityModule)
      },
      {
        path: ':entity/country',
        loadChildren: () => import('./components/location/country/country.module').then(m => m.CountryModule)
      },
      {
        path: ':entity/province',
        loadChildren: () => import('./components/location/province/province.module').then(m => m.ProvinceModule)
      },
      {
        path: ':entity/county',
        loadChildren: () => import('./components/location/county/county.module').then(m => m.CountyModule)
      },
      {
        path: ':entity/town',
        loadChildren: () => import('./components/location/town/town.module').then(m => m.TownModule)
      },
      {
        path: ':entity/office',
        loadChildren: () => import('./components/office/office.module').then(m => m.OfficeModule)
      },
      {
        path: ':entity/productcategory',
        loadChildren: () => import('./components/product-management/product-category/productcategory.module').then(m => m.ProductcategoryModule)
      },
      {
        path: ':entity/productsubcategory',
        loadChildren: () => import('./components/product-management/product-sub-category/productsubcategory.module').then(m => m.ProductsubcategoryModule)
      },
      {
        path: ':entity/productsub2category',
        loadChildren: () => import('./components/product-management/product-category-sub2/productsub2category.module').then(m => m.Productsub2categoryModule)
      },
      {
        path: ':entity/poedit',
        loadChildren: () => import('./components/purchaseorder/edit-purchaseorder/poedit.module').then(m => m.PoeditModule)
      },
      {
        path: ':entity/posearch',
        loadChildren: () => import('./components/purchaseorder/purchaseorder-summary/posearch.module').then(m => m.PosearchModule)
      },
      {
        path: ':entity/poupload',
        loadChildren: () => import('./components/purchaseorder/upload-purchaseorder/poupload.module').then(m => m.PouploadModule)
      },
      {
        path: ':entity/useredit',
        loadChildren: () => import('./components/user-account/edit-user-account/useredit.module').then(m => m.UsereditModule)
      },
      {
        path: ':entity/usersearch',
        loadChildren: () => import('./components/user-account/user-account-summary/usersearch.module').then(m => m.UsersearchModule)
      },
      {
        path: ':entity/rolerights',
        loadChildren: () => import('./components/user-account/role-right-configuration/roleright.module').then(m => m.RolerightModule)
      },
      {
        path: ':entity/supplieredit',
        loadChildren: () => import('./components/supplier/edit-supplier/supplieredit.module').then(m => m.SuppliereditModule)
      },
      {
        path: ':entity/suppliersearch',
        loadChildren: () => import('./components/supplier/supplier-summary/suppliersearch.module').then(m => m.SuppliersearchModule)
      },
      {
        path: ':entity/supplierlite',
        loadChildren: () => import('./components/supplier/edit-supplier-lite/supplierlite.module').then(m => m.SupplierliteModule)
      },
      {
        path: ':entity/purchaseorderlite',
        loadChildren: () => import('./components/purchaseorder/upload-purchaseorder-lite/purchaseorder-lite.module').then(m => m.PurchaseOrderliteModule)
      },
      {
        path: ':entity/quotations',
        loadChildren: () => import('./components/quotation/edit-quotation/quotationedit.module').then(m => m.QuotationeditModule)
      },
      {
        path: ':entity/cuscheckpointedit',
        loadChildren: () => import('./components/customer/edit-customer-checkpoint/edit-customer-checkpoint.module').then(m => m.EditCustomerCheckPointModule)
      },
      {
        path: ':entity/cuscomplaintedit',
        loadChildren: () => import('./components/customer/edit-customer-complaint/edit-customer-complaint.module').then(m => m.EditCustomerComplaintModule)
      },
      {
        path: ':entity/cuscomplaintsummary',
        loadChildren: () => import('./components/customer/customer-complaint-summary/customer-complaint-summary.module').then(m => m.CustomerComplaintSummaryModule)
      },
      {
        path: ':entity/changepassword',
        loadChildren: () => import('./components/change-password/changepassword.module').then(m => m.ChangepasswordModule)
      },
      {
        path: ':entity/intdashboard',
        loadChildren: () => import('./components/dashboards/internal/dashboard/intdashboard.module').then(m => m.IntdashboardModule)
      },
      {
        path: ':entity/schedule',
        loadChildren: () => import('./components/Schedule/schedule.module').then(m => m.ScheduleModule)
      },
      {
        path: ':entity/report',
        loadChildren: () => import('./components/reports/reports.module').then(m => m.ReportModule)
      },
      {
        path: ':entity/inspectioncertificateedit',
        loadChildren: () => import('./components/inspection-certificate/edit-inspectioncertificate/edit-inspectioncertificate.module').then(m => m.EditInspectioncertificateModule)
      },
      {
        path: ':entity/inspectioncertificatesearch',
        loadChildren: () => import('./components/inspection-certificate/inspectioncertificate-summary/inspectioncertificate-summary.module').then(m => m.InspectioncertificateSummaryModule)
      },
      {
        path: ':entity/inspectioncertificatepending',
        loadChildren: () => import('./components/inspection-certificate/pending-inspectioncertificate/pending-inspectioncertificate.module').then(m => m.PendingInspectioncertificateModule)
      },
      {
        path: ':entity/cusdashboard',
        loadChildren: () => import('./components/dashboards/customer/cusdashboard/cusdashboard.module').then(m => m.CusdashboardModule)
      },
      {
        path: ':entity/audcusreport',
        loadChildren: () => import('./components/Audit/audit-cus-report/audit-cus-report.module').then(m => m.AuditCusReportModule)
      },
      {
        path: ':entity/dfcustomerconfig',
        loadChildren: () => import('./components/dynamicfields/editdfcustomerconfiguration/editdfcustomerconfiguration.module').then(m => m.DFCustomerConfigurationModule)
      },
      {
        path: ':entity/dfcustomerconfigsummary',
        loadChildren: () => import('./components/dynamicfields/dfcustomerconfigsummary/dfcustomerconfigsummary.module').then(m => m.DFCustomerConfigSummaryModule)
      },
      {
        path: ':entity/userconfigcustomer',
        loadChildren: () => import('./components/user-account/user-config/userconfig.module').then(m => m.UserConfigModule)
      },
      {
        path: ':entity/kpi',
        loadChildren: () => import('./components/kpi/kpi.module').then(m => m.kpiModule)
      },
      {
        path: ':entity/pricecardsummary',
        loadChildren: () => import('./components/customer/customer-price-card-summary/customer-price-card-summary.module').then(m => m.CustomerPriceCardSummaryModule)
      },
      {
        path: ':entity/pricecardedit',
        loadChildren: () => import('./components/customer/edit-customer-price-card/edit-customer-price-card.module').then(m => m.EditCustomerPriceCardModule)
      },
      {
        path: ':entity/customkpi',
        loadChildren: () => import('./components/kpi/custom-kpi/custom-kpi.module').then(m => m.CustomKpiModule)
      },
      {
        path: ':entity/qcavailability',
        loadChildren: () => import('./components/Schedule/schedule.module').then(m => m.ScheduleModule)
      },
      {
        path: ':entity/travelmatrix',
        loadChildren: () => import('./components/invoice/travel-matrix/travel-matrix.module').then(m => m.TravelMatrixModule)
      },
      {
        path: ':entity/invoicediscountedit',
        loadChildren: () => import('./components/invoice/invoice-discount-register/invoice-discount-register.module').then(m => m.InvoiceDiscountRegisterModule)
      },
      {
        path: ':entity/invoicediscountsearch',
        loadChildren: () => import('./components/invoice/invoice-discount-summary/invoice-discount-summary.module').then(m => m.InvoiceDiscountSummaryModule)
      },
      {
        path: ':entity/inspection-occupancy',
        loadChildren: () => import('./components/inspection-occupancy/inspection-occupancy.module').then(m => m.InspectionOccupancyModule)
      },
      {
        path: ':entity/qcdashboard',
        loadChildren: () => import('./components/dashboards/qc/qcdashboard/qcdashboard.module').then(m => m.QcdashboardModule)
      },
      {
        path: ':entity/mandaydashboard',
        loadChildren: () => import('./components/statistics/manday-dashboard/manday-dashboard.module').then(m => m.MandayDashboardModule)

      },
      {
        path: ':entity/tcfsummary',
        loadChildren: () => import('./components/tcfdata/tcf-summary/tcf-summary.module').then(m => m.TCFSummaryModule)
      },
      {
        path: ':entity/tcfdetail',
        loadChildren: () => import('./components/tcfdata/tcf-detail/tcf-detail.module').then(m => m.TCFDetailModule)
      },
      {
        path: ':entity/tcfdocument',
        loadChildren: () => import('./components/tcfdata/tcf-document/tcf-document.module').then(m => m.TCFDocumentModule)
      },
      {
        path: ':entity/tcfdashboard',
        loadChildren: () => import('./components/tcfdata/tcf-dashboard/tcf-dashboard.module').then(m => m.TCFDashboardModule)
      },
      {
        path: ':entity/tcf',
        loadChildren: () => import('./components/tcfdata/supplier-score/supplier-score.module').then(m => m.SupplierScoreModule)
      },
      {
        path: ':entity/utilizationdashboard',
        loadChildren: () => import('./components/statistics/manday-utilization-dashboard/manday-utilization-dashboard.module').then(m => m.MandayUtilizationDashboardModule)
      },
      {
        path: ':entity/invoicesummary',
        loadChildren: () => import('./components/invoice/invoice-summary/invoice-summary.module').then(m => m.InvoiceSummaryModule)
      },
      {
        path: ':entity/saleinvoicesummary',
        loadChildren: () => import('./components/invoice/sale-invoice-summary/sale-invoice-summary.module').then(m => m.SaleInvoiceSummaryModule)
      },
      {
        path: ':entity/invoicestatus',
        loadChildren: () => import('./components/invoice/invoice-status/invoice-status.module').then(m => m.InvoiceStatusModule)
      },
      {
        path: ':entity/editinvoice',
        loadChildren: () => import('./components/invoice/edit-invoice/edit-invoice.module').then(m => m.EditInvoiceModule)
      },
      {
        path: ':entity/invoicegenerate',
        loadChildren: () => import('./components/invoice/invoice-generate/invoice-generate.module').then(m => m.InvoiceGenerateModule)
      },
      {
        path: ':entity/manualinvoicesearch',
        loadChildren: () => import('./components/invoice/manual-invoice-summary/manual-invoice-summary.module').then(m => m.ManualInvoiceSummaryModule)
      },
      {
        path: ':entity/manualinvoiceedit',
        loadChildren: () => import('./components/invoice/edit-manual-invoice/edit-manual-invoice.module').then(m => m.EditManualInvoiceModule)
      },
      {
        path: ':entity/userprofile',
        loadChildren: () => import('./components/user-account/user-profile/userprofile.module').then(m => m.UserProfileModule)
      },
      {
        path: ':entity/#',
        loadChildren: () => import('./components/login/authentication-navigation/authentication-navigation.module').then(m => m.AuthenticationNavigationModule)
      },
      {
        path: ':error/:id',
        component: ErrorComponent
      },
      {
        path: '',
        loadChildren: () => import('./components/login/authentication-navigation/authentication-navigation.module').then(m => m.AuthenticationNavigationModule)
      },
      {
        path: ':entity/invoicebanksummary',
        loadChildren: () => import('./components/invoice/invoice-bank/invoice-bank.module').then(m => m.InvoiceBankModule)
      },
      {
        path: ':entity/invoicebankedit',
        loadChildren: () => import('./components/invoice/edit-invoice-bank/edit-invoice-bank.module').then(m => m.EditInvoiceBankModule)
      },
      {
        path: ':entity/extrafeesedit',
        loadChildren: () => import('./components/invoice/edit-extra-fees/edit-extra-fees.module').then(m => m.EditExtraFeesModule)
      },
      {
        path: ':entity/extrafeessummary',
        loadChildren: () => import('./components/invoice/extra-fees-summary/extra-fees-summary.module').then(m => m.ExtraFeesSummaryModule)
      },
      {
        path: ':entity/qcblock',
        loadChildren: () => import('./components/Schedule/schedule.module').then(m => m.ScheduleModule)
      },
      {
        path: ':entity/summary',
        loadChildren: () => import('./components/Schedule/schedule.module').then(m => m.ScheduleModule)
      },
      {
        path: ':entity/supdashboard',
        loadChildren: () => import('./components/dashboards/supplierfactory/sup-fact-dashboard/sup-fact-dashboard.module').then(m => m.SupFactDashboardModule)
      },
      {
        path: ':entity/email',
        loadChildren: () => import('./components/email-send/edit-email-subject/edit-email-subject.module').then(m => m.EditEmailSubjectModule)
      },
      {
        path: ':entity/emailsub',
        loadChildren: () => import('./components/email-send/email-subject-summary/email-subject-summary.module').then(m => m.EmailSubjectSummaryModule)
      },
      {
        path: ':entity/managementdashboard',
        loadChildren: () => import('./components/dashboards/management/managementdashboard/managementdashboard.module').then(m => m.ManagementdashboardModule)
      },
      {
        path: ':entity/quantitativedashboard',
        loadChildren: () => import('./components/statistics/quantitativedashboard/quantitativedashboard/quantitativedashboard.module').then(m => m.QuantitativedashboardModule)
      },
      {
        path: ':entity/defect',
        loadChildren: () => import('./components/statistics/defect-dashboard/defect-dashboard.module').then(m => m.DefectDashboardModule)
      },
      {
        path: ':entity/defectpareto',
        loadChildren: () => import('./components/statistics//defect-pareto/defect-pareto.module').then(m => m.DefectParetoModule)
      },
      {
        path: ':entity/emailconfig',
        loadChildren: () => import('./components/email-send/edit-email-configuration/edit-email-configuration.module').then(m => m.EditEmailConfigurationModule)
      },
      {
        path: ':entity/econfig',
        loadChildren: () => import('./components/email-send/email-configuration-summary/email-configuration-summary.module').then(m => m.EmailConfigurationSummaryModule)
      },
      {
        path: ':entity/reject',
        loadChildren: () => import('./components/statistics/reject/rejectdashboard/rejectdashboard.module').then(m => m.RejectdashboardModule)
      },
      {
        path: ':entity/rejectrate',
        loadChildren: () => import('./components/statistics/reject/rejection-rate/rejection-rate.module').then(m => m.RejectionRateModule)
      },
      {
        path: ':entity/esend',
        loadChildren: () => import('./components/email-send/email-send-summary/email-send-summary.module').then(m => m.EmailSendSummaryModule)
      },
      {
        path: ':entity/emailsend',
        loadChildren: () => import('./components/email-send/edit-email-send/edit-email-send.module').then(m => m.EditEmailSendModule)
      },
      {
        path: ':entity/invoicesend',
        loadChildren: () => import('./components/invoice/edit-invoice-send/edit-invoice-send.module').then(m => m.EditInvoiceSendModule)
      },
      {
        path: ':entity/finance',
        loadChildren: () => import('./components/statistics/finance-dashboard/finance-dashboard/finance-dashboard.module').then(m => m.FinanceDashboardModule)
      },
      {
        path: ':entity/cs',
        loadChildren: () => import('./components/dashboards/csdashboard/csdashboard.module').then(m => m.CsdashboardModule)
      },
      {
        path: ':entity/userguide',
        loadChildren: () => import('./components/user-guide/user-guide.module').then(m => m.UserGuideModule)
      },
      {
        path: ':entity/customerdecision',
        loadChildren: () => import('./components/booking/customer-decision/inspcustomerdecision.module').then(m => m.CustomerDecisionModule)
      },
      {
        path: ':entity/traveltariffsummary',
        loadChildren: () => import('./components/traveltariff/travel-tariff/travel-tariff.module').then(m => m.TravelTariffModule)
      },
      {
        path: ':entity/edittraveltariff',
        loadChildren: () => import('./components/traveltariff/edit-travel-tariff/edit-travel-tariff.module').then(m => m.EditTravelTariffModule)
      },
      {
        path: ':entity/foodallowance',
        loadChildren: () => import('./components/expense/food-allowance/food-allowance.module').then(m => m.FoodAllowanceModule)
      },
      {
        path: ':entity/pendingexpense',
        loadChildren: () => import('./components/expense/pending-expense/pending-expense.module').then(m => m.PendingExpenseModule)
      },
      {
        path: ':entity/startingportsummary',
        loadChildren: () => import('./components/expense/starting-port/starting-port.module').then(m => m.StartingPortModule)
      },
      {
        path: ':entity/productsub3category',
        loadChildren: () => import('./components/product-management/product-category-sub3/product-category-sub3.module').then(m => m.ProductCategorySub3Module)
      },
      {
        path: ':entity/workloadmatrix',
        loadChildren: () => import('./components/work-load-matrix/work-load-matrix.module').then(m => m.WorkLoadMatrixModule)
      },
      {
        path: ':entity/othermanday',
        loadChildren: () => import('./components/other-manday/other-manday.module').then(m => m.OtherMandayModule)
      },
      {
        path: ':entity/datamanagement',
        loadChildren: () => import('./components/data-management/data-management.module').then(m => m.DataManagementModule)
      },
      {
        path: ':entity/editClaim',
        loadChildren: () => import('./components/claim/claim.module').then(m => m.ClaimModule)
      },
      {
        path: ':entity/InvoiceDataAccess',
        loadChildren: () => import('./components/invoice/invoice-data-access/invoice-data-access.module').then(m => m.InvoiceDataAccessModule)
      },
      {
        path: ':entity/shared',
        loadChildren: () => import('./components/shared/shared.module').then(m => m.SharedModule)
      },
      {
        path: ':entity/auditdashboard',
        loadChildren: () => import('./components/Audit/audit-dashboard/audit-dashboard.module').then(m => m.AuditDashboardModule)
      }
    ]
  },

  { path: 'login', loadChildren: () => import('./components/login/authentication-login/authentication-login/authentication-login.module').then(m => m.AuthenticationLoginModule) },


  { path: 'changepassword', loadChildren: () => import('./components/change-password/changepassword.module').then(m => m.ChangepasswordModule) },

  {
    path: 'emailuserverification',
    loadChildren: () => import('./components/login/emailuser-verification/emailuser-verification.module').then(m => m.EmailUserVerificationModule)
  },
  {
    path: 'landing',
    loadChildren: () => import('./components/landing/landing.module').then(m => m.LandingModule)
  },
  {
    path: 'landing',
    loadChildren: () => import('./components/landing/landing.module').then(m => m.LandingModule)
  }
];

export const routing = RouterModule.forRoot(appRoutes, { relativeLinkResolution: 'legacy' });

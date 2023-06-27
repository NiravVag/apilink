using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum RoleEnum
    {
        Cs = 1,
        Inspector = 2,
        IT_Team = 4,
        Management = 5, // approver leave and expense (Manager)
        CSManagement = 6,
        ExpenseClaim = 7, //expense finance team
        TechnicalTeamManagement = 8,
        OperationManagement = 9,
        HumanResource = 10, //leave access 
        Accounting = 12,
        LeaveHr = 18, // Hr Leave
        ExpenseClaimNotification = 19, // expense claim email access
        QuotationRequest = 20,
        QuotationManager = 21,
        QuotationConfirmation = 22,
        InspectionRequest = 23,
        InspectionConfirmed = 24,
        InspectionVerified = 25,
        InspectionScheduled = 26,
        QuotationSend = 27,
        InspectionCertificate = 32, // issue ic 
        EditInspectionCustomerDecision = 33,
        ViewInspectionCustomerDecision = 34,
        ReportChecker = 35,
        Supplier = 30,
        Factory = 31,
        TCFUser = 37,
        TCFCustomer = 38,
        TCFSupplier = 39,
        TCFIT = 40,
        Accounting_InspectionCancel = 41,
        AutoQCExpenseAccounting = 43,
        ClaimCreate = 44,
        ClaimAnalyze = 45,
        ClaimValidate = 46,
        ClaimAccounting = 47,
        OutsourceAccounting=48,        
        AccountingCreditNote=50,
        Customer = 29

    }
}

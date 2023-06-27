using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_UserMaster")]
    public partial class ItUserMaster
    {
        public ItUserMaster()
        {
            AudCuProductCategoryCreatedByNavigations = new HashSet<AudCuProductCategory>();
            AudCuProductCategoryDeletedByNavigations = new HashSet<AudCuProductCategory>();
            AudFbReportCheckpointCreatedByNavigations = new HashSet<AudFbReportCheckpoint>();
            AudFbReportCheckpointDeletedByNavigations = new HashSet<AudFbReportCheckpoint>();
            AudTranAuditorCreatedByNavigations = new HashSet<AudTranAuditor>();
            AudTranAuditorDeletedByNavigations = new HashSet<AudTranAuditor>();
            AudTranCCreatedByNavigations = new HashSet<AudTranC>();
            AudTranCDeletedByNavigations = new HashSet<AudTranC>();
            AudTranCancelReschedules = new HashSet<AudTranCancelReschedule>();
            AudTranCuContactCreatedByNavigations = new HashSet<AudTranCuContact>();
            AudTranCuContactDeletedByNavigations = new HashSet<AudTranCuContact>();
            AudTranFaContactCreatedByNavigations = new HashSet<AudTranFaContact>();
            AudTranFaContactDeletedByNavigations = new HashSet<AudTranFaContact>();
            AudTranFaProfileCreatedByNavigations = new HashSet<AudTranFaProfile>();
            AudTranFaProfileDeletedByNavigations = new HashSet<AudTranFaProfile>();
            AudTranFileAttachmentDeletedByNavigations = new HashSet<AudTranFileAttachment>();
            AudTranFileAttachmentUsers = new HashSet<AudTranFileAttachment>();
            AudTranReport1S = new HashSet<AudTranReport1>();
            AudTranReportCreatedByNavigations = new HashSet<AudTranReport>();
            AudTranReportDeletedByNavigations = new HashSet<AudTranReport>();
            AudTranReportDetails = new HashSet<AudTranReportDetail>();
            AudTranServiceTypeCreatedByNavigations = new HashSet<AudTranServiceType>();
            AudTranServiceTypeDeletedByNavigations = new HashSet<AudTranServiceType>();
            AudTranStatusLogs = new HashSet<AudTranStatusLog>();
            AudTranSuContactCreatedByNavigations = new HashSet<AudTranSuContact>();
            AudTranSuContactDeletedByNavigations = new HashSet<AudTranSuContact>();
            AudTranWorkProcessCreatedByNavigations = new HashSet<AudTranWorkProcess>();
            AudTranWorkProcessDeletedByNavigations = new HashSet<AudTranWorkProcess>();
            AudTransactions = new HashSet<AudTransaction>();
            ClmTranAttachmentCreatedByNavigations = new HashSet<ClmTranAttachment>();
            ClmTranAttachmentDeletedByNavigations = new HashSet<ClmTranAttachment>();
            ClmTranAttachmentUpdatedByNavigations = new HashSet<ClmTranAttachment>();
            ClmTranClaimRefundCreatedByNavigations = new HashSet<ClmTranClaimRefund>();
            ClmTranClaimRefundDeletedByNavigations = new HashSet<ClmTranClaimRefund>();
            ClmTranClaimRefundUpdatedByNavigations = new HashSet<ClmTranClaimRefund>();
            ClmTranCustomerRequestCreatedByNavigations = new HashSet<ClmTranCustomerRequest>();
            ClmTranCustomerRequestDeletedByNavigations = new HashSet<ClmTranCustomerRequest>();
            ClmTranCustomerRequestRefundCreatedByNavigations = new HashSet<ClmTranCustomerRequestRefund>();
            ClmTranCustomerRequestRefundDeletedByNavigations = new HashSet<ClmTranCustomerRequestRefund>();
            ClmTranCustomerRequestRefundUpdatedByNavigations = new HashSet<ClmTranCustomerRequestRefund>();
            ClmTranCustomerRequestUpdatedByNavigations = new HashSet<ClmTranCustomerRequest>();
            ClmTranDefectFamilyCreatedByNavigations = new HashSet<ClmTranDefectFamily>();
            ClmTranDefectFamilyDeletedByNavigations = new HashSet<ClmTranDefectFamily>();
            ClmTranDefectFamilyUpdatedByNavigations = new HashSet<ClmTranDefectFamily>();
            ClmTranDepartmentCreatedByNavigations = new HashSet<ClmTranDepartment>();
            ClmTranDepartmentDeletedByNavigations = new HashSet<ClmTranDepartment>();
            ClmTranDepartmentUpdatedByNavigations = new HashSet<ClmTranDepartment>();
            ClmTranFinalDecisionCreatedByNavigations = new HashSet<ClmTranFinalDecision>();
            ClmTranFinalDecisionDeletedByNavigations = new HashSet<ClmTranFinalDecision>();
            ClmTranFinalDecisionUpdatedByNavigations = new HashSet<ClmTranFinalDecision>();
            ClmTranReportCreatedByNavigations = new HashSet<ClmTranReport>();
            ClmTranReportDeletedByNavigations = new HashSet<ClmTranReport>();
            ClmTranReportUpdatedByNavigations = new HashSet<ClmTranReport>();
            ClmTransactionAnalyzedByNavigations = new HashSet<ClmTransaction>();
            ClmTransactionClosedByNavigations = new HashSet<ClmTransaction>();
            ClmTransactionCreatedByNavigations = new HashSet<ClmTransaction>();
            ClmTransactionDeletedByNavigations = new HashSet<ClmTransaction>();
            ClmTransactionUpdatedByNavigations = new HashSet<ClmTransaction>();
            ClmTransactionValidatedByNavigations = new HashSet<ClmTransaction>();
            CompComplaintCreatedByNavigations = new HashSet<CompComplaint>();
            CompComplaintDeletedByNavigations = new HashSet<CompComplaint>();
            CompTranComplaintsDetailCreatedByNavigations = new HashSet<CompTranComplaintsDetail>();
            CompTranComplaintsDetailDeletedByNavigations = new HashSet<CompTranComplaintsDetail>();
            CompTranPersonInChargeCreatedByNavigations = new HashSet<CompTranPersonInCharge>();
            CompTranPersonInChargeDeletedByNavigations = new HashSet<CompTranPersonInCharge>();
            CuAddressCreatedByNavigations = new HashSet<CuAddress>();
            CuAddressDeletedByNavigations = new HashSet<CuAddress>();
            CuAddressUpdatedByNavigations = new HashSet<CuAddress>();
            CuApiServiceCreatedByNavigations = new HashSet<CuApiService>();
            CuApiServiceDeletedByNavigations = new HashSet<CuApiService>();
            CuBrandCreatedByNavigations = new HashSet<CuBrand>();
            CuBrandDeletedByNavigations = new HashSet<CuBrand>();
            CuBrandUpdatedByNavigations = new HashSet<CuBrand>();
            CuBuyerApiServiceCreatedByNavigations = new HashSet<CuBuyerApiService>();
            CuBuyerApiServiceDeletedByNavigations = new HashSet<CuBuyerApiService>();
            CuBuyerCreatedByNavigations = new HashSet<CuBuyer>();
            CuBuyerDeletedByNavigations = new HashSet<CuBuyer>();
            CuBuyerUpdatedByNavigations = new HashSet<CuBuyer>();
            CuCheckPointCreatedByNavigations = new HashSet<CuCheckPoint>();
            CuCheckPointDeletedByNavigations = new HashSet<CuCheckPoint>();
            CuCheckPointModifiedByNavigations = new HashSet<CuCheckPoint>();
            CuCheckPointsBrandCreatedByNavigations = new HashSet<CuCheckPointsBrand>();
            CuCheckPointsBrandUpdatedByNavigations = new HashSet<CuCheckPointsBrand>();
            CuCheckPointsDepartmentCreatedByNavigations = new HashSet<CuCheckPointsDepartment>();
            CuCheckPointsDepartmentUpdatedByNavigations = new HashSet<CuCheckPointsDepartment>();
            CuCheckPointsServiceTypeCreatedByNavigations = new HashSet<CuCheckPointsServiceType>();
            CuCheckPointsServiceTypeUpdatedByNavigations = new HashSet<CuCheckPointsServiceType>();
            CuCollectionCreatedByNavigations = new HashSet<CuCollection>();
            CuCollectionDeletedByNavigations = new HashSet<CuCollection>();
            CuCollectionUpdatedByNavigations = new HashSet<CuCollection>();
            CuContactBrandCreatedByNavigations = new HashSet<CuContactBrand>();
            CuContactBrandDeletedByNavigations = new HashSet<CuContactBrand>();
            CuContactCreatedByNavigations = new HashSet<CuContact>();
            CuContactDeletedByNavigations = new HashSet<CuContact>();
            CuContactDepartmentCreatedByNavigations = new HashSet<CuContactDepartment>();
            CuContactDepartmentDeletedByNavigations = new HashSet<CuContactDepartment>();
            CuContactServiceCreatedByNavigations = new HashSet<CuContactService>();
            CuContactServiceDeletedByNavigations = new HashSet<CuContactService>();
            CuContactSisterCompanyCreatedByNavigations = new HashSet<CuContactSisterCompany>();
            CuContactSisterCompanyDeletedByNavigations = new HashSet<CuContactSisterCompany>();
            CuContactUpdatedByNavigations = new HashSet<CuContact>();
            CuCsOnsiteEmails = new HashSet<CuCsOnsiteEmail>();
            CuCustomerCreatedByNavigations = new HashSet<CuCustomer>();
            CuCustomerDeletedByNavigations = new HashSet<CuCustomer>();
            CuCustomerUpdatedByNavigations = new HashSet<CuCustomer>();
            CuDepartmentCreatedByNavigations = new HashSet<CuDepartment>();
            CuDepartmentDeletedByNavigations = new HashSet<CuDepartment>();
            CuDepartmentUpdatedByNavigations = new HashSet<CuDepartment>();
            CuEntityCreatedByNavigations = new HashSet<CuEntity>();
            CuEntityDeletedByNavigations = new HashSet<CuEntity>();
            CuPoFactoryCreatedByNavigations = new HashSet<CuPoFactory>();
            CuPoFactoryDeletedByNavigations = new HashSet<CuPoFactory>();
            CuPoSupplierCreatedByNavigations = new HashSet<CuPoSupplier>();
            CuPoSupplierDeletedByNavigations = new HashSet<CuPoSupplier>();
            CuPrBrandCreatedByNavigations = new HashSet<CuPrBrand>();
            CuPrBrandDeletedByNavigations = new HashSet<CuPrBrand>();
            CuPrBrandUpdatedByNavigations = new HashSet<CuPrBrand>();
            CuPrBuyerCreatedByNavigations = new HashSet<CuPrBuyer>();
            CuPrBuyerDeletedByNavigations = new HashSet<CuPrBuyer>();
            CuPrBuyerUpdatedByNavigations = new HashSet<CuPrBuyer>();
            CuPrCityCreatedByNavigations = new HashSet<CuPrCity>();
            CuPrCityDeletedByNavigations = new HashSet<CuPrCity>();
            CuPrCityUpdatedByNavigations = new HashSet<CuPrCity>();
            CuPrCountryCreatedByNavigations = new HashSet<CuPrCountry>();
            CuPrCountryDeletedByNavigations = new HashSet<CuPrCountry>();
            CuPrCountryUpdatedByNavigations = new HashSet<CuPrCountry>();
            CuPrDepartmentCreatedByNavigations = new HashSet<CuPrDepartment>();
            CuPrDepartmentDeletedByNavigations = new HashSet<CuPrDepartment>();
            CuPrDepartmentUpdatedByNavigations = new HashSet<CuPrDepartment>();
            CuPrDetailCreatedByNavigations = new HashSet<CuPrDetail>();
            CuPrDetailDeletedByNavigations = new HashSet<CuPrDetail>();
            CuPrDetailUpdatedByNavigations = new HashSet<CuPrDetail>();
            CuPrHolidayTypeCreatedByNavigations = new HashSet<CuPrHolidayType>();
            CuPrHolidayTypeDeletedByNavigations = new HashSet<CuPrHolidayType>();
            CuPrHolidayTypeUpdatedByNavigations = new HashSet<CuPrHolidayType>();
            CuPrInspectionLocationCreatedByNavigations = new HashSet<CuPrInspectionLocation>();
            CuPrInspectionLocationDeletedByNavigations = new HashSet<CuPrInspectionLocation>();
            CuPrPriceCategoryCreatedByNavigations = new HashSet<CuPrPriceCategory>();
            CuPrPriceCategoryDeletedByNavigations = new HashSet<CuPrPriceCategory>();
            CuPrPriceCategoryUpdatedByNavigations = new HashSet<CuPrPriceCategory>();
            CuPrProductCategoryCreatedByNavigations = new HashSet<CuPrProductCategory>();
            CuPrProductCategoryDeletedByNavigations = new HashSet<CuPrProductCategory>();
            CuPrProductCategoryUpdatedByNavigations = new HashSet<CuPrProductCategory>();
            CuPrProductSubCategoryCreatedByNavigations = new HashSet<CuPrProductSubCategory>();
            CuPrProductSubCategoryDeletedByNavigations = new HashSet<CuPrProductSubCategory>();
            CuPrProductSubCategoryUpdatedByNavigations = new HashSet<CuPrProductSubCategory>();
            CuPrProvinceCreatedByNavigations = new HashSet<CuPrProvince>();
            CuPrProvinceDeletedByNavigations = new HashSet<CuPrProvince>();
            CuPrProvinceUpdatedByNavigations = new HashSet<CuPrProvince>();
            CuPrServiceTypeCreatedByNavigations = new HashSet<CuPrServiceType>();
            CuPrServiceTypeDeletedByNavigations = new HashSet<CuPrServiceType>();
            CuPrServiceTypeUpdatedByNavigations = new HashSet<CuPrServiceType>();
            CuPrSupplierCreatedByNavigations = new HashSet<CuPrSupplier>();
            CuPrSupplierDeletedByNavigations = new HashSet<CuPrSupplier>();
            CuPrSupplierUpdatedByNavigations = new HashSet<CuPrSupplier>();
            CuPrTranSpecialRuleCreatedByNavigations = new HashSet<CuPrTranSpecialRule>();
            CuPrTranSpecialRuleDeletedByNavigations = new HashSet<CuPrTranSpecialRule>();
            CuPrTranSubcategoryCreatedByNavigations = new HashSet<CuPrTranSubcategory>();
            CuPrTranSubcategoryDeletedByNavigations = new HashSet<CuPrTranSubcategory>();
            CuProductApiServiceCreatedByNavigations = new HashSet<CuProductApiService>();
            CuProductApiServiceDeletedByNavigations = new HashSet<CuProductApiService>();
            CuProductFileAttachmentDeletedByNavigations = new HashSet<CuProductFileAttachment>();
            CuProductFileAttachmentUsers = new HashSet<CuProductFileAttachment>();
            CuProductMschartCreatedByNavigations = new HashSet<CuProductMschart>();
            CuProductMschartOcrMapCreatedByNavigations = new HashSet<CuProductMschartOcrMap>();
            CuProductMschartOcrMapDeletedByNavigations = new HashSet<CuProductMschartOcrMap>();
            CuProductMschartUpdatedByNavigations = new HashSet<CuProductMschart>();
            CuProducts = new HashSet<CuProduct>();
            CuPurchaseOrderAttachments = new HashSet<CuPurchaseOrderAttachment>();
            CuPurchaseOrderCreatedByNavigations = new HashSet<CuPurchaseOrder>();
            CuPurchaseOrderDeletedByNavigations = new HashSet<CuPurchaseOrder>();
            CuPurchaseOrderDetailCreatedByNavigations = new HashSet<CuPurchaseOrderDetail>();
            CuPurchaseOrderDetailDeletedByNavigations = new HashSet<CuPurchaseOrderDetail>();
            CuPurchaseOrderUpdatedByNavigations = new HashSet<CuPurchaseOrder>();
            CuServiceTypeCreatedByNavigations = new HashSet<CuServiceType>();
            CuServiceTypeDeletedByNavigations = new HashSet<CuServiceType>();
            CuServiceTypeUpdatedByNavigations = new HashSet<CuServiceType>();
            CuSisterCompanyCreatedByNavigations = new HashSet<CuSisterCompany>();
            CuSisterCompanyDeletedByNavigations = new HashSet<CuSisterCompany>();
            DaUserByBrands = new HashSet<DaUserByBrand>();
            DaUserByBuyers = new HashSet<DaUserByBuyer>();
            DaUserByDepartments = new HashSet<DaUserByDepartment>();
            DaUserByFactoryCountries = new HashSet<DaUserByFactoryCountry>();
            DaUserByProductCategories = new HashSet<DaUserByProductCategory>();
            DaUserByRoles = new HashSet<DaUserByRole>();
            DaUserByServices = new HashSet<DaUserByService>();
            DaUserCustomerCreatedByNavigations = new HashSet<DaUserCustomer>();
            DaUserCustomerUsers = new HashSet<DaUserCustomer>();
            DaUserRoleNotificationByOffices = new HashSet<DaUserRoleNotificationByOffice>();
            DfCuConfigurationCreatedByNavigations = new HashSet<DfCuConfiguration>();
            DfCuConfigurationDeletedByNavigations = new HashSet<DfCuConfiguration>();
            DfCuConfigurationUpdatedByNavigations = new HashSet<DfCuConfiguration>();
            DmDetailCreatedByNavigations = new HashSet<DmDetail>();
            DmDetailDeletedByNavigations = new HashSet<DmDetail>();
            DmDetailUpdatedByNavigations = new HashSet<DmDetail>();
            DmFileCreatedByNavigations = new HashSet<DmFile>();
            DmFileDeletedByNavigations = new HashSet<DmFile>();
            DmRoleCreatedByNavigations = new HashSet<DmRole>();
            DmRoleDeletedByNavigations = new HashSet<DmRole>();
            DmRoleUpdatedByNavigations = new HashSet<DmRole>();
            EcAutQcFoodExpenseCreatedByNavigations = new HashSet<EcAutQcFoodExpense>();
            EcAutQcFoodExpenseDeletedByNavigations = new HashSet<EcAutQcFoodExpense>();
            EcAutQcFoodExpenseUpdatedByNavigations = new HashSet<EcAutQcFoodExpense>();
            EcAutQcTravelExpenseCreatedByNavigations = new HashSet<EcAutQcTravelExpense>();
            EcAutQcTravelExpenseDeletedByNavigations = new HashSet<EcAutQcTravelExpense>();
            EcAutQcTravelExpenseUpdatedByNavigations = new HashSet<EcAutQcTravelExpense>();
            EcAutRefStartPortCreatedByNavigations = new HashSet<EcAutRefStartPort>();
            EcAutRefStartPortDeletedByNavigations = new HashSet<EcAutRefStartPort>();
            EcAutRefStartPortUpdatedByNavigations = new HashSet<EcAutRefStartPort>();
            EcAutTravelTariffCreatedByNavigations = new HashSet<EcAutTravelTariff>();
            EcAutTravelTariffDeletedByNavigations = new HashSet<EcAutTravelTariff>();
            EcAutTravelTariffUpdatedByNavigations = new HashSet<EcAutTravelTariff>();
            EcExpencesClaimApproveds = new HashSet<EcExpencesClaim>();
            EcExpencesClaimCancels = new HashSet<EcExpencesClaim>();
            EcExpencesClaimCheckeds = new HashSet<EcExpencesClaim>();
            EcExpencesClaimPaids = new HashSet<EcExpencesClaim>();
            EcExpencesClaimRejects = new HashSet<EcExpencesClaim>();
            EcExpenseClaimsAuditCreatedByNavigations = new HashSet<EcExpenseClaimsAudit>();
            EcExpenseClaimsAuditDeletedByNavigations = new HashSet<EcExpenseClaimsAudit>();
            EcExpenseClaimsInspectionCreatedByNavigations = new HashSet<EcExpenseClaimsInspection>();
            EcExpenseClaimsInspectionDeletedByNavigations = new HashSet<EcExpenseClaimsInspection>();
            EcFoodAllowanceDeletedByNavigations = new HashSet<EcFoodAllowance>();
            EcFoodAllowanceUpdatedByNavigations = new HashSet<EcFoodAllowance>();
            EcFoodAllowanceUsers = new HashSet<EcFoodAllowance>();
            EcReceiptFileAttachmentCreatedbyNavigations = new HashSet<EcReceiptFileAttachment>();
            EcReceiptFileAttachmentDeletedByNavigations = new HashSet<EcReceiptFileAttachment>();
            EcReceiptFiles = new HashSet<EcReceiptFile>();
            EmExchangeRates = new HashSet<EmExchangeRate>();
            EntFeatureDetailCreatedByNavigations = new HashSet<EntFeatureDetail>();
            EntFeatureDetailDeletedByNavigations = new HashSet<EntFeatureDetail>();
            EntRefFeatureCreatedByNavigations = new HashSet<EntRefFeature>();
            EntRefFeatureDeletedByNavigations = new HashSet<EntRefFeature>();
            EsAdditionalRecipients = new HashSet<EsAdditionalRecipient>();
            EsDetails = new HashSet<EsDetail>();
            EsDetailDeletedByNavigations = new HashSet<EsDetail>();
            EsDetailUpdatedByNavigations = new HashSet<EsDetail>();
            EsRecipientTypes = new HashSet<EsRecipientType>();
            EsRefEmailSizes = new HashSet<EsRefEmailSize>();
            EsRefFileTypes = new HashSet<EsRefFileType>();
            EsRefRecipientTypes = new HashSet<EsRefRecipientType>();
            EsRefReportInEmails = new HashSet<EsRefReportInEmail>();
            EsRefReportSendTypes = new HashSet<EsRefReportSendType>();
            EsRefSpecialRules = new HashSet<EsRefSpecialRule>();
            EsSpecialRules = new HashSet<EsSpecialRule>();
            EsSuModules = new HashSet<EsSuModule>();
            EsSuPreDefinedFieldCreatedByNavigations = new HashSet<EsSuPreDefinedField>();
            EsSuPreDefinedFieldDeletedByNavigations = new HashSet<EsSuPreDefinedField>();
            EsSuPreDefinedFieldUpdatedByNavigations = new HashSet<EsSuPreDefinedField>();
            EsSuTemplateDetails = new HashSet<EsSuTemplateDetail>();
            EsSuTemplateMasterCreatedByNavigations = new HashSet<EsSuTemplateMaster>();
            EsSuTemplateMasterDeletedByNavigations = new HashSet<EsSuTemplateMaster>();
            EsSuTemplateMasterUpdatedByNavigations = new HashSet<EsSuTemplateMaster>();
            EsTranFileCreatedByNavigations = new HashSet<EsTranFile>();
            EsTranFileDeletedByNavigations = new HashSet<EsTranFile>();
            EventBookingLogs = new HashSet<EventBookingLog>();
            FbReportDetailCreatedByNavigations = new HashSet<FbReportDetail>();
            FbReportDetailDeletedByNavigations = new HashSet<FbReportDetail>();
            FbReportDetailUpdatedByNavigations = new HashSet<FbReportDetail>();
            FbReportManualLogCreatedByNavigations = new HashSet<FbReportManualLog>();
            FbReportManualLogDeletedByNavigations = new HashSet<FbReportManualLog>();
            HrAttachmentDeletedByNavigations = new HashSet<HrAttachment>();
            HrAttachmentUsers = new HashSet<HrAttachment>();
            HrFileAttachments = new HashSet<HrFileAttachment>();
            HrLeaveApproveds = new HashSet<HrLeave>();
            HrLeaveCheckeds = new HashSet<HrLeave>();
            HrOutSourceCompanyCreatedByNavigations = new HashSet<HrOutSourceCompany>();
            HrOutSourceCompanyDeletedByNavigations = new HashSet<HrOutSourceCompany>();
            HrOutSourceCompanyUpdatedByNavigations = new HashSet<HrOutSourceCompany>();
            HrPhotoDeletedByNavigations = new HashSet<HrPhoto>();
            HrPhotoUsers = new HashSet<HrPhoto>();
            HrStaffServiceCreatedByNavigations = new HashSet<HrStaffService>();
            HrStaffServiceDeletedByNavigations = new HashSet<HrStaffService>();
            InspContainerTransactionCreatedByNavigations = new HashSet<InspContainerTransaction>();
            InspContainerTransactionDeletedByNavigations = new HashSet<InspContainerTransaction>();
            InspContainerTransactionUpdatedByNavigations = new HashSet<InspContainerTransaction>();
            InspDfTransactionCreatedByNavigations = new HashSet<InspDfTransaction>();
            InspDfTransactionDeletedByNavigations = new HashSet<InspDfTransaction>();
            InspDfTransactionUpdatedByNavigations = new HashSet<InspDfTransaction>();
            InspIcTranProductCreatedByNavigations = new HashSet<InspIcTranProduct>();
            InspIcTranProductDeletedByNavigations = new HashSet<InspIcTranProduct>();
            InspIcTranProductUpdatedByNavigations = new HashSet<InspIcTranProduct>();
            InspIcTransactionCreatedByNavigations = new HashSet<InspIcTransaction>();
            InspIcTransactionDeletedByNavigations = new HashSet<InspIcTransaction>();
            InspIcTransactionUpdatedByNavigations = new HashSet<InspIcTransaction>();
            InspProductTransactionCreatedByNavigations = new HashSet<InspProductTransaction>();
            InspProductTransactionDeletedByNavigations = new HashSet<InspProductTransaction>();
            InspProductTransactionUpdatedByNavigations = new HashSet<InspProductTransaction>();
            InspPurchaseOrderColorTransactionCreatedByNavigations = new HashSet<InspPurchaseOrderColorTransaction>();
            InspPurchaseOrderColorTransactionDeletedByNavigations = new HashSet<InspPurchaseOrderColorTransaction>();
            InspPurchaseOrderColorTransactionUpdatedByNavigations = new HashSet<InspPurchaseOrderColorTransaction>();
            InspPurchaseOrderTransactionCreatedByNavigations = new HashSet<InspPurchaseOrderTransaction>();
            InspPurchaseOrderTransactionDeletedByNavigations = new HashSet<InspPurchaseOrderTransaction>();
            InspPurchaseOrderTransactionUpdatedByNavigations = new HashSet<InspPurchaseOrderTransaction>();
            InspRepCusDecisions = new HashSet<InspRepCusDecision>();
            InspTranCCreatedByNavigations = new HashSet<InspTranC>();
            InspTranCCs = new HashSet<InspTranC>();
            InspTranCDeletedByNavigations = new HashSet<InspTranC>();
            InspTranCancelCreatedByNavigations = new HashSet<InspTranCancel>();
            InspTranCancelModifiedByNavigations = new HashSet<InspTranCancel>();
            InspTranCuBrandCreatedByNavigations = new HashSet<InspTranCuBrand>();
            InspTranCuBrandDeletedByNavigations = new HashSet<InspTranCuBrand>();
            InspTranCuBuyerCreatedByNavigations = new HashSet<InspTranCuBuyer>();
            InspTranCuBuyerDeletedByNavigations = new HashSet<InspTranCuBuyer>();
            InspTranCuContactCreatedByNavigations = new HashSet<InspTranCuContact>();
            InspTranCuContactDeletedByNavigations = new HashSet<InspTranCuContact>();
            InspTranCuDepartmentCreatedByNavigations = new HashSet<InspTranCuDepartment>();
            InspTranCuDepartmentDeletedByNavigations = new HashSet<InspTranCuDepartment>();
            InspTranCuMerchandiserCreatedByNavigations = new HashSet<InspTranCuMerchandiser>();
            InspTranCuMerchandiserDeletedByNavigations = new HashSet<InspTranCuMerchandiser>();
            InspTranFaContactCreatedByNavigations = new HashSet<InspTranFaContact>();
            InspTranFaContactDeletedByNavigations = new HashSet<InspTranFaContact>();
            InspTranFileAttachmentDeletedByNavigations = new HashSet<InspTranFileAttachment>();
            InspTranFileAttachmentUsers = new HashSet<InspTranFileAttachment>();
            InspTranFileAttachmentZipCreatedByNavigations = new HashSet<InspTranFileAttachmentZip>();
            InspTranFileAttachmentZipDeletedByNavigations = new HashSet<InspTranFileAttachmentZip>();
            InspTranPickingContactCreatedByNavigations = new HashSet<InspTranPickingContact>();
            InspTranPickingContactDeletedByNavigations = new HashSet<InspTranPickingContact>();
            InspTranPickingCreatedByNavigations = new HashSet<InspTranPicking>();
            InspTranPickingDeletedByNavigations = new HashSet<InspTranPicking>();
            InspTranPickingUpdatedByNavigations = new HashSet<InspTranPicking>();
            InspTranRescheduleCreatedByNavigations = new HashSet<InspTranReschedule>();
            InspTranRescheduleModifiedByNavigations = new HashSet<InspTranReschedule>();
            InspTranServiceTypeCreatedByNavigations = new HashSet<InspTranServiceType>();
            InspTranServiceTypeDeletedByNavigations = new HashSet<InspTranServiceType>();
            InspTranShipmentTypeCreatedByNavigations = new HashSet<InspTranShipmentType>();
            InspTranShipmentTypeDeletedByNavigations = new HashSet<InspTranShipmentType>();
            InspTranStatusLogs = new HashSet<InspTranStatusLog>();
            InspTranSuContactCreatedByNavigations = new HashSet<InspTranSuContact>();
            InspTranSuContactDeletedByNavigations = new HashSet<InspTranSuContact>();
            InspTransactionCreatedByNavigations = new HashSet<InspTransaction>();
            InspTransactionDraftCreatedByNavigations = new HashSet<InspTransactionDraft>();
            InspTransactionDraftDeletedByNavigations = new HashSet<InspTransactionDraft>();
            InspTransactionDraftUpdatedByNavigations = new HashSet<InspTransactionDraft>();
            InspTransactionUpdatedByNavigations = new HashSet<InspTransaction>();
            InvAutTranCommunications = new HashSet<InvAutTranCommunication>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvAutTranStatusLogs = new HashSet<InvAutTranStatusLog>();
            InvAutTranTaxes = new HashSet<InvAutTranTax>();
            InvCreTranClaimDetailCreatedByNavigations = new HashSet<InvCreTranClaimDetail>();
            InvCreTranClaimDetailDeletedByNavigations = new HashSet<InvCreTranClaimDetail>();
            InvCreTranClaimDetailUpdatedByNavigations = new HashSet<InvCreTranClaimDetail>();
            InvCreTranContactCreatedByNavigations = new HashSet<InvCreTranContact>();
            InvCreTranContactDeletedByNavigations = new HashSet<InvCreTranContact>();
            InvCreTransactionCreatedByNavigations = new HashSet<InvCreTransaction>();
            InvCreTransactionDeletedByNavigations = new HashSet<InvCreTransaction>();
            InvCreTransactionUpdatedByNavigations = new HashSet<InvCreTransaction>();
            InvDaCustomerCreatedByNavigations = new HashSet<InvDaCustomer>();
            InvDaCustomerDeletedByNavigations = new HashSet<InvDaCustomer>();
            InvDaCustomerUpdatedByNavigations = new HashSet<InvDaCustomer>();
            InvDaInvoiceTypeCreatedByNavigations = new HashSet<InvDaInvoiceType>();
            InvDaInvoiceTypeDeletedByNavigations = new HashSet<InvDaInvoiceType>();
            InvDaInvoiceTypeUpdatedByNavigations = new HashSet<InvDaInvoiceType>();
            InvDaOfficeCreatedByNavigations = new HashSet<InvDaOffice>();
            InvDaOfficeDeletedByNavigations = new HashSet<InvDaOffice>();
            InvDaOfficeUpdatedByNavigations = new HashSet<InvDaOffice>();
            InvDaTransactionCreatedByNavigations = new HashSet<InvDaTransaction>();
            InvDaTransactionDeletedByNavigations = new HashSet<InvDaTransaction>();
            InvDaTransactionUpdatedByNavigations = new HashSet<InvDaTransaction>();
            InvDisTranCountryCreatedByNavigations = new HashSet<InvDisTranCountry>();
            InvDisTranCountryDeletedByNavigations = new HashSet<InvDisTranCountry>();
            InvDisTranDetailCreatedByNavigations = new HashSet<InvDisTranDetail>();
            InvDisTranDetailDeletedByNavigations = new HashSet<InvDisTranDetail>();
            InvDisTranDetailUpdatedByNavigations = new HashSet<InvDisTranDetail>();
            InvDisTranPeriodInfoCreatedByNavigations = new HashSet<InvDisTranPeriodInfo>();
            InvDisTranPeriodInfoDeletedByNavigations = new HashSet<InvDisTranPeriodInfo>();
            InvExfTranDetailCreatedByNavigations = new HashSet<InvExfTranDetail>();
            InvExfTranDetailDeletedByNavigations = new HashSet<InvExfTranDetail>();
            InvExfTranDetailUpdatedByNavigations = new HashSet<InvExfTranDetail>();
            InvExfTranStatusLogs = new HashSet<InvExfTranStatusLog>();
            InvExfTransactionCreatedByNavigations = new HashSet<InvExfTransaction>();
            InvExfTransactionDeletedByNavigations = new HashSet<InvExfTransaction>();
            InvExfTransactionUpdatedByNavigations = new HashSet<InvExfTransaction>();
            InvExtTranTaxes = new HashSet<InvExtTranTax>();
            InvManTranDetailCreatedByNavigations = new HashSet<InvManTranDetail>();
            InvManTranDetailDeletedByNavigations = new HashSet<InvManTranDetail>();
            InvManTranDetailUpdatedByNavigations = new HashSet<InvManTranDetail>();
            InvManTranTaxes = new HashSet<InvManTranTax>();
            InvManTransactionCreatedByNavigations = new HashSet<InvManTransaction>();
            InvManTransactionDeletedByNavigations = new HashSet<InvManTransaction>();
            InvManTransactionUpdatedByNavigations = new HashSet<InvManTransaction>();
            InvRefBankCreatedByNavigations = new HashSet<InvRefBank>();
            InvRefBankDeletedByNavigations = new HashSet<InvRefBank>();
            InvRefBankUpdatedByNavigations = new HashSet<InvRefBank>();
            InvTmDetailCreatedByNavigations = new HashSet<InvTmDetail>();
            InvTmDetailDeletedByNavigations = new HashSet<InvTmDetail>();
            InvTmDetailUpdatedByNavigations = new HashSet<InvTmDetail>();
            InvTranBankTaxCreatedByNavigations = new HashSet<InvTranBankTax>();
            InvTranBankTaxDeletedByNavigations = new HashSet<InvTranBankTax>();
            InvTranBankTaxUpdatedByNavigations = new HashSet<InvTranBankTax>();
            InvTranFileCreatedByNavigations = new HashSet<InvTranFile>();
            InvTranFileDeletedByNavigations = new HashSet<InvTranFile>();
            InvTranInvoiceRequestContactCreatedByNavigations = new HashSet<InvTranInvoiceRequestContact>();
            InvTranInvoiceRequestContactDeletedByNavigations = new HashSet<InvTranInvoiceRequestContact>();
            InvTranInvoiceRequestContactUpdatedByNavigations = new HashSet<InvTranInvoiceRequestContact>();
            InvTranInvoiceRequestCreatedByNavigations = new HashSet<InvTranInvoiceRequest>();
            InvTranInvoiceRequestDeletedByNavigations = new HashSet<InvTranInvoiceRequest>();
            InvTranInvoiceRequestUpdatedByNavigations = new HashSet<InvTranInvoiceRequest>();
            InverseCreatedByNavigation = new HashSet<ItUserMaster>();
            InverseDeletedByNavigation = new HashSet<ItUserMaster>();
            InverseUpdatedByNavigation = new HashSet<ItUserMaster>();
            ItLoginLogs = new HashSet<ItLoginLog>();
            ItUserCuBrands = new HashSet<ItUserCuBrand>();
            ItUserCuDepartments = new HashSet<ItUserCuDepartment>();
            ItUserRoles = new HashSet<ItUserRole>();
            KpiTemplates = new HashSet<KpiTemplate>();
            LogBookingFbQueues = new HashSet<LogBookingFbQueue>();
            LogEmailQueueAttachments = new HashSet<LogEmailQueueAttachment>();
            LogEmailQueues = new HashSet<LogEmailQueue>();
            MidNotifications = new HashSet<MidNotification>();
            MidTasks = new HashSet<MidTask>();
            OmDetailCreatedByNavigations = new HashSet<OmDetail>();
            OmDetailDeletedByNavigations = new HashSet<OmDetail>();
            OmDetailUpdatedByNavigations = new HashSet<OmDetail>();
            QcBlCustomers = new HashSet<QcBlCustomer>();
            QcBlProductCatgeories = new HashSet<QcBlProductCatgeory>();
            QcBlProductSubCategories = new HashSet<QcBlProductSubCategory>();
            QcBlProductSubCategory2S = new HashSet<QcBlProductSubCategory2>();
            QcBlSupplierFactories = new HashSet<QcBlSupplierFactory>();
            QcBlockListCreatedByNavigations = new HashSet<QcBlockList>();
            QcBlockListDeletedByNavigations = new HashSet<QcBlockList>();
            QuPdfversions = new HashSet<QuPdfversion>();
            QuQuotationAudMandayCreatedByNavigations = new HashSet<QuQuotationAudManday>();
            QuQuotationAudMandayDeletedByNavigations = new HashSet<QuQuotationAudManday>();
            QuQuotationAudMandayUpdatedByNavigations = new HashSet<QuQuotationAudManday>();
            QuQuotationAudits = new HashSet<QuQuotationAudit>();
            QuQuotationInspMandayCreatedByNavigations = new HashSet<QuQuotationInspManday>();
            QuQuotationInspMandayDeletedByNavigations = new HashSet<QuQuotationInspManday>();
            QuQuotationInspMandayUpdatedByNavigations = new HashSet<QuQuotationInspManday>();
            QuQuotationInsps = new HashSet<QuQuotationInsp>();
            QuQuotationPdfVersions = new HashSet<QuQuotationPdfVersion>();
            QuQuotations = new HashSet<QuQuotation>();
            QuTranStatusLogs = new HashSet<QuTranStatusLog>();
            QuWorkLoadMatrixCreatedByNavigations = new HashSet<QuWorkLoadMatrix>();
            QuWorkLoadMatrixDeletedByNavigations = new HashSet<QuWorkLoadMatrix>();
            QuWorkLoadMatrixUpdatedByNavigations = new HashSet<QuWorkLoadMatrix>();
            RefBudgetForecastCreatedByNavigations = new HashSet<RefBudgetForecast>();
            RefBudgetForecastDeletedByNavigations = new HashSet<RefBudgetForecast>();
            RefBudgetForecastUpdatedByNavigations = new HashSet<RefBudgetForecast>();
            RefCountyCreatedByNavigations = new HashSet<RefCounty>();
            RefCountyDeletedByNavigations = new HashSet<RefCounty>();
            RefCountyModifiedByNavigations = new HashSet<RefCounty>();
            RefDelimiters = new HashSet<RefDelimiter>();
            RefProductCategoryApiServiceCreatedByNavigations = new HashSet<RefProductCategoryApiService>();
            RefProductCategoryApiServiceDeletedByNavigations = new HashSet<RefProductCategoryApiService>();
            RefProductCategorySub3CreatedByNavigations = new HashSet<RefProductCategorySub3>();
            RefProductCategorySub3DeletedByNavigations = new HashSet<RefProductCategorySub3>();
            RefProductCategorySub3UpdatedByNavigations = new HashSet<RefProductCategorySub3>();
            RefTownCreatedByNavigations = new HashSet<RefTown>();
            RefTownDeletedByNavigations = new HashSet<RefTown>();
            RefTownModifiedByNavigations = new HashSet<RefTown>();
            RestApiLogs = new HashSet<RestApiLog>();
            SchQctypeCreatedByNavigations = new HashSet<SchQctype>();
            SchQctypeDeletedByNavigations = new HashSet<SchQctype>();
            SchQctypeModifiedByNavigations = new HashSet<SchQctype>();
            SchScheduleCCreatedByNavigations = new HashSet<SchScheduleC>();
            SchScheduleCDeletedByNavigations = new HashSet<SchScheduleC>();
            SchScheduleCModifiedByNavigations = new HashSet<SchScheduleC>();
            SchScheduleQcCreatedByNavigations = new HashSet<SchScheduleQc>();
            SchScheduleQcDeletedByNavigations = new HashSet<SchScheduleQc>();
            SchScheduleQcModifiedByNavigations = new HashSet<SchScheduleQc>();
            SuApiServiceCreatedByNavigations = new HashSet<SuApiService>();
            SuApiServiceDeletedByNavigations = new HashSet<SuApiService>();
            SuContactApiServiceCreatedByNavigations = new HashSet<SuContactApiService>();
            SuContactApiServiceDeletedByNavigations = new HashSet<SuContactApiService>();
            SuContacts = new HashSet<SuContact>();
            SuCreditTermCreatedByNavigations = new HashSet<SuCreditTerm>();
            SuCreditTermDeletedByNavigations = new HashSet<SuCreditTerm>();
            SuEntityCreatedByNavigations = new HashSet<SuEntity>();
            SuEntityDeletedByNavigations = new HashSet<SuEntity>();
            SuGradeCreatedByNavigations = new HashSet<SuGrade>();
            SuGradeDeletedByNavigations = new HashSet<SuGrade>();
            SuGradeUpdatedByNavigations = new HashSet<SuGrade>();
            SuStatusCreatedByNavigations = new HashSet<SuStatus>();
            SuStatusDeletedByNavigations = new HashSet<SuStatus>();
            SuSupplierCreatedByNavigations = new HashSet<SuSupplier>();
            SuSupplierDeletedByNavigations = new HashSet<SuSupplier>();
            SuSupplierModifiedByNavigations = new HashSet<SuSupplier>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Login_name")]
        [StringLength(50)]
        public string LoginName { get; set; }
        [StringLength(500)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        public bool Active { get; set; }
        public short StatusId { get; set; }
        public int? StaffId { get; set; }
        public int UserTypeId { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public bool? ChangePassword { get; set; }
        [Column("Fb_User_Id")]
        public int? FbUserId { get; set; }
        public int? CustomerContactId { get; set; }
        public int? SupplierContactId { get; set; }
        public int? FactoryContactId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column("TCFUserID")]
        public int? TcfuserId { get; set; }
        public string FileUrl { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InverseCreatedByNavigation")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("ItUserMasters")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("CustomerContactId")]
        [InverseProperty("ItUserMasters")]
        public virtual CuContact CustomerContact { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InverseDeletedByNavigation")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FactoryId")]
        [InverseProperty("ItUserMasterFactories")]
        public virtual SuSupplier Factory { get; set; }
        [ForeignKey("FactoryContactId")]
        [InverseProperty("ItUserMasterFactoryContacts")]
        public virtual SuContact FactoryContact { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("ItUserMasters")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("ItUserMasterSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("SupplierContactId")]
        [InverseProperty("ItUserMasterSupplierContacts")]
        public virtual SuContact SupplierContact { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InverseUpdatedByNavigation")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [ForeignKey("UserTypeId")]
        [InverseProperty("ItUserMasters")]
        public virtual ItUserType UserType { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudCuProductCategory> AudCuProductCategoryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudCuProductCategory> AudCuProductCategoryDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudFbReportCheckpoint> AudFbReportCheckpointCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudFbReportCheckpoint> AudFbReportCheckpointDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranAuditor> AudTranAuditorCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranAuditor> AudTranAuditorDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranC> AudTranCCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranC> AudTranCDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranCancelReschedule> AudTranCancelReschedules { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranCuContact> AudTranCuContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranCuContact> AudTranCuContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranFaContact> AudTranFaContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranFaContact> AudTranFaContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranFaProfile> AudTranFaProfileCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranFaProfile> AudTranFaProfileDeletedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranFileAttachment> AudTranFileAttachmentDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AudTranFileAttachment> AudTranFileAttachmentUsers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AudTranReport1> AudTranReport1S { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranReport> AudTranReportCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranReport> AudTranReportDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AudTranReportDetail> AudTranReportDetails { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranServiceType> AudTranServiceTypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranServiceType> AudTranServiceTypeDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranStatusLog> AudTranStatusLogs { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranSuContact> AudTranSuContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranSuContact> AudTranSuContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTranWorkProcess> AudTranWorkProcessCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<AudTranWorkProcess> AudTranWorkProcessDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranAttachment> ClmTranAttachmentCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranAttachment> ClmTranAttachmentDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranAttachment> ClmTranAttachmentUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranClaimRefund> ClmTranClaimRefundCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranClaimRefund> ClmTranClaimRefundDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranClaimRefund> ClmTranClaimRefundUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranCustomerRequest> ClmTranCustomerRequestCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranCustomerRequest> ClmTranCustomerRequestDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefundCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefundDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranCustomerRequestRefund> ClmTranCustomerRequestRefundUpdatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranCustomerRequest> ClmTranCustomerRequestUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranDefectFamily> ClmTranDefectFamilyCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranDefectFamily> ClmTranDefectFamilyDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranDefectFamily> ClmTranDefectFamilyUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranDepartment> ClmTranDepartmentCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranDepartment> ClmTranDepartmentDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranDepartment> ClmTranDepartmentUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranFinalDecision> ClmTranFinalDecisionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranFinalDecision> ClmTranFinalDecisionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranFinalDecision> ClmTranFinalDecisionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTranReport> ClmTranReportCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTranReport> ClmTranReportDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTranReport> ClmTranReportUpdatedByNavigations { get; set; }
        [InverseProperty("AnalyzedByNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionAnalyzedByNavigations { get; set; }
        [InverseProperty("ClosedByNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionClosedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("ValidatedByNavigation")]
        public virtual ICollection<ClmTransaction> ClmTransactionValidatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CompComplaint> CompComplaintCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CompComplaint> CompComplaintDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CompTranComplaintsDetail> CompTranComplaintsDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CompTranComplaintsDetail> CompTranComplaintsDetailDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CompTranPersonInCharge> CompTranPersonInChargeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CompTranPersonInCharge> CompTranPersonInChargeDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuAddress> CuAddressCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuAddress> CuAddressDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuAddress> CuAddressUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuApiService> CuApiServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuApiService> CuApiServiceDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuBrand> CuBrandCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuBrand> CuBrandDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuBrand> CuBrandUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuBuyerApiService> CuBuyerApiServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuBuyerApiService> CuBuyerApiServiceDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuBuyer> CuBuyerCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuBuyer> CuBuyerDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuBuyer> CuBuyerUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuCheckPoint> CuCheckPointCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuCheckPoint> CuCheckPointDeletedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<CuCheckPoint> CuCheckPointModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuCheckPointsBrand> CuCheckPointsBrandCreatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuCheckPointsBrand> CuCheckPointsBrandUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuCheckPointsDepartment> CuCheckPointsDepartmentCreatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuCheckPointsDepartment> CuCheckPointsDepartmentUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuCheckPointsServiceType> CuCheckPointsServiceTypeCreatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuCheckPointsServiceType> CuCheckPointsServiceTypeUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuCollection> CuCollectionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuCollection> CuCollectionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuCollection> CuCollectionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuContactBrand> CuContactBrandCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuContactBrand> CuContactBrandDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuContact> CuContactCreatedByNavigations { get; set; }       
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuContact> CuContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuContactDepartment> CuContactDepartmentCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuContactDepartment> CuContactDepartmentDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuContactService> CuContactServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuContactService> CuContactServiceDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuContactSisterCompany> CuContactSisterCompanyCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuContactSisterCompany> CuContactSisterCompanyDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuContact> CuContactUpdatedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CuCsOnsiteEmail> CuCsOnsiteEmails { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuCustomer> CuCustomerCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuCustomer> CuCustomerDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuCustomer> CuCustomerUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuDepartment> CuDepartmentCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuDepartment> CuDepartmentDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuDepartment> CuDepartmentUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuEntity> CuEntityCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuEntity> CuEntityDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPoFactory> CuPoFactoryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPoFactory> CuPoFactoryDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPoSupplier> CuPoSupplierCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPoSupplier> CuPoSupplierDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrBrand> CuPrBrandCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrBrand> CuPrBrandDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrBrand> CuPrBrandUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrBuyer> CuPrBuyerCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrBuyer> CuPrBuyerDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrBuyer> CuPrBuyerUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrCity> CuPrCityCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrCity> CuPrCityDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrCity> CuPrCityUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrCountry> CuPrCountryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrCountry> CuPrCountryDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrCountry> CuPrCountryUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrDepartment> CuPrDepartmentCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrDepartment> CuPrDepartmentDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrDepartment> CuPrDepartmentUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrHolidayType> CuPrHolidayTypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrHolidayType> CuPrHolidayTypeDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrHolidayType> CuPrHolidayTypeUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrInspectionLocation> CuPrInspectionLocationCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrInspectionLocation> CuPrInspectionLocationDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrPriceCategory> CuPrPriceCategoryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrPriceCategory> CuPrPriceCategoryDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrPriceCategory> CuPrPriceCategoryUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrProductCategory> CuPrProductCategoryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrProductCategory> CuPrProductCategoryDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrProductCategory> CuPrProductCategoryUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrProductSubCategory> CuPrProductSubCategoryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrProductSubCategory> CuPrProductSubCategoryDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrProductSubCategory> CuPrProductSubCategoryUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrProvince> CuPrProvinceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrProvince> CuPrProvinceDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrProvince> CuPrProvinceUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrServiceType> CuPrServiceTypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrServiceType> CuPrServiceTypeDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrServiceType> CuPrServiceTypeUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrSupplier> CuPrSupplierCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrSupplier> CuPrSupplierDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPrSupplier> CuPrSupplierUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrTranSpecialRule> CuPrTranSpecialRuleCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrTranSpecialRule> CuPrTranSpecialRuleDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPrTranSubcategory> CuPrTranSubcategoryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPrTranSubcategory> CuPrTranSubcategoryDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuProductApiService> CuProductApiServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuProductApiService> CuProductApiServiceDeletedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuProductFileAttachment> CuProductFileAttachmentDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CuProductFileAttachment> CuProductFileAttachmentUsers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuProductMschart> CuProductMschartCreatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuProductMschartOcrMap> CuProductMschartOcrMapCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuProductMschartOcrMap> CuProductMschartOcrMapDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuProductMschart> CuProductMschartUpdatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuProduct> CuProducts { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CuPurchaseOrderAttachment> CuPurchaseOrderAttachments { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrderCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrderDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuPurchaseOrderDetail> CuPurchaseOrderDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuPurchaseOrderDetail> CuPurchaseOrderDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrderUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<CuSisterCompany> CuSisterCompanyCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<CuSisterCompany> CuSisterCompanyDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserByBrand> DaUserByBrands { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserByBuyer> DaUserByBuyers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserByDepartment> DaUserByDepartments { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserByFactoryCountry> DaUserByFactoryCountries { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserByProductCategory> DaUserByProductCategories { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserByRole> DaUserByRoles { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserByService> DaUserByServices { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserCustomer> DaUserCustomerCreatedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DaUserCustomer> DaUserCustomerUsers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DaUserRoleNotificationByOffice> DaUserRoleNotificationByOffices { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurationCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurationDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurationUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DmDetail> DmDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<DmDetail> DmDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<DmDetail> DmDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DmFile> DmFileCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<DmFile> DmFileDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<DmRole> DmRoleCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<DmRole> DmRoleDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<DmRole> DmRoleUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EcAutQcFoodExpense> EcAutQcFoodExpenseCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcAutQcFoodExpense> EcAutQcFoodExpenseDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EcAutQcFoodExpense> EcAutQcFoodExpenseUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenseCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenseDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenseUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EcAutRefStartPort> EcAutRefStartPortCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcAutRefStartPort> EcAutRefStartPortDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EcAutRefStartPort> EcAutRefStartPortUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EcAutTravelTariff> EcAutTravelTariffCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcAutTravelTariff> EcAutTravelTariffDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EcAutTravelTariff> EcAutTravelTariffUpdatedByNavigations { get; set; }
        [InverseProperty("Approved")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaimApproveds { get; set; }
        [InverseProperty("Cancel")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaimCancels { get; set; }
        [InverseProperty("Checked")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaimCheckeds { get; set; }
        [InverseProperty("Paid")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaimPaids { get; set; }
        [InverseProperty("Reject")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaimRejects { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EcExpenseClaimsAudit> EcExpenseClaimsAuditCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcExpenseClaimsAudit> EcExpenseClaimsAuditDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EcExpenseClaimsInspection> EcExpenseClaimsInspectionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcExpenseClaimsInspection> EcExpenseClaimsInspectionDeletedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcFoodAllowance> EcFoodAllowanceDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EcFoodAllowance> EcFoodAllowanceUpdatedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<EcFoodAllowance> EcFoodAllowanceUsers { get; set; }
        [InverseProperty("CreatedbyNavigation")]
        public virtual ICollection<EcReceiptFileAttachment> EcReceiptFileAttachmentCreatedbyNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EcReceiptFileAttachment> EcReceiptFileAttachmentDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<EcReceiptFile> EcReceiptFiles { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<EmExchangeRate> EmExchangeRates { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EntFeatureDetail> EntFeatureDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EntFeatureDetail> EntFeatureDetailDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EntRefFeature> EntRefFeatureCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EntRefFeature> EntRefFeatureDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsAdditionalRecipient> EsAdditionalRecipients { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EsDetail> EsDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EsDetail> EsDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsRecipientType> EsRecipientTypes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsRefEmailSize> EsRefEmailSizes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsRefFileType> EsRefFileTypes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsRefRecipientType> EsRefRecipientTypes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsRefReportInEmail> EsRefReportInEmails { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsRefReportSendType> EsRefReportSendTypes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsRefSpecialRule> EsRefSpecialRules { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsSpecialRule> EsSpecialRules { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsSuModule> EsSuModules { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsSuPreDefinedField> EsSuPreDefinedFieldCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EsSuPreDefinedField> EsSuPreDefinedFieldDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EsSuPreDefinedField> EsSuPreDefinedFieldUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsSuTemplateDetail> EsSuTemplateDetails { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasterCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasterDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasterUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EsTranFile> EsTranFileCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<EsTranFile> EsTranFileDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<EventBookingLog> EventBookingLogs { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<FbReportDetail> FbReportDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<FbReportDetail> FbReportDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<FbReportDetail> FbReportDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<FbReportManualLog> FbReportManualLogCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<FbReportManualLog> FbReportManualLogDeletedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<HrAttachment> HrAttachmentDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<HrAttachment> HrAttachmentUsers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<HrFileAttachment> HrFileAttachments { get; set; }
        [InverseProperty("Approved")]
        public virtual ICollection<HrLeave> HrLeaveApproveds { get; set; }
        [InverseProperty("Checked")]
        public virtual ICollection<HrLeave> HrLeaveCheckeds { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<HrOutSourceCompany> HrOutSourceCompanyCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<HrOutSourceCompany> HrOutSourceCompanyDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<HrOutSourceCompany> HrOutSourceCompanyUpdatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<HrPhoto> HrPhotoDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<HrPhoto> HrPhotoUsers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<HrStaffService> HrStaffServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<HrStaffService> HrStaffServiceDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspContainerTransaction> InspContainerTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspContainerTransaction> InspContainerTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspContainerTransaction> InspContainerTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspDfTransaction> InspDfTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspDfTransaction> InspDfTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspDfTransaction> InspDfTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspIcTranProduct> InspIcTranProductCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspIcTranProduct> InspIcTranProductDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspIcTranProduct> InspIcTranProductUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspIcTransaction> InspIcTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspIcTransaction> InspIcTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspIcTransaction> InspIcTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspPurchaseOrderColorTransaction> InspPurchaseOrderColorTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspPurchaseOrderColorTransaction> InspPurchaseOrderColorTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspPurchaseOrderColorTransaction> InspPurchaseOrderColorTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspPurchaseOrderTransaction> InspPurchaseOrderTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspRepCusDecision> InspRepCusDecisions { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranC> InspTranCCreatedByNavigations { get; set; }
        [InverseProperty("Cs")]
        public virtual ICollection<InspTranC> InspTranCCs { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranC> InspTranCDeletedByNavigations { get; set; }        
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranCancel> InspTranCancelCreatedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<InspTranCancel> InspTranCancelModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranCuBrand> InspTranCuBrandCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranCuBrand> InspTranCuBrandDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranCuBuyer> InspTranCuBuyerCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranCuBuyer> InspTranCuBuyerDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranCuContact> InspTranCuContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranCuContact> InspTranCuContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranCuDepartment> InspTranCuDepartmentCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranCuDepartment> InspTranCuDepartmentDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranCuMerchandiser> InspTranCuMerchandiserCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranCuMerchandiser> InspTranCuMerchandiserDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranFaContact> InspTranFaContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranFaContact> InspTranFaContactDeletedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranFileAttachment> InspTranFileAttachmentDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<InspTranFileAttachment> InspTranFileAttachmentUsers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranFileAttachmentZip> InspTranFileAttachmentZipCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranFileAttachmentZip> InspTranFileAttachmentZipDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranPickingContact> InspTranPickingContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranPickingContact> InspTranPickingContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranPicking> InspTranPickingCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranPicking> InspTranPickingDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspTranPicking> InspTranPickingUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranReschedule> InspTranRescheduleCreatedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<InspTranReschedule> InspTranRescheduleModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranServiceType> InspTranServiceTypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranServiceType> InspTranServiceTypeDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranShipmentType> InspTranShipmentTypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranShipmentType> InspTranShipmentTypeDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranStatusLog> InspTranStatusLogs { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTranSuContact> InspTranSuContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTranSuContact> InspTranSuContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTransaction> InspTransactionCreatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDraftCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDraftDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDraftUpdatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InspTransaction> InspTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvAutTranCommunication> InvAutTranCommunications { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvAutTranStatusLog> InvAutTranStatusLogs { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvAutTranTax> InvAutTranTaxes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvCreTranClaimDetail> InvCreTranClaimDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvCreTranClaimDetail> InvCreTranClaimDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvCreTranClaimDetail> InvCreTranClaimDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvCreTranContact> InvCreTranContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvCreTranContact> InvCreTranContactDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvCreTransaction> InvCreTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvCreTransaction> InvCreTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvCreTransaction> InvCreTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvDaCustomer> InvDaCustomerCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvDaCustomer> InvDaCustomerDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvDaCustomer> InvDaCustomerUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvDaInvoiceType> InvDaInvoiceTypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvDaInvoiceType> InvDaInvoiceTypeDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvDaInvoiceType> InvDaInvoiceTypeUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvDaOffice> InvDaOfficeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvDaOffice> InvDaOfficeDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvDaOffice> InvDaOfficeUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvDaTransaction> InvDaTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvDaTransaction> InvDaTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvDaTransaction> InvDaTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvDisTranCountry> InvDisTranCountryCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvDisTranCountry> InvDisTranCountryDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvDisTranDetail> InvDisTranDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvDisTranDetail> InvDisTranDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvDisTranDetail> InvDisTranDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvDisTranPeriodInfo> InvDisTranPeriodInfoCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvDisTranPeriodInfo> InvDisTranPeriodInfoDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvExfTranDetail> InvExfTranDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvExfTranDetail> InvExfTranDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvExfTranDetail> InvExfTranDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvExfTranStatusLog> InvExfTranStatusLogs { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvExfTransaction> InvExfTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvExfTransaction> InvExfTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvExfTransaction> InvExfTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvExtTranTax> InvExtTranTaxes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvManTranDetail> InvManTranDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvManTranDetail> InvManTranDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvManTranDetail> InvManTranDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvManTranTax> InvManTranTaxes { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactionCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactionDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactionUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvRefBank> InvRefBankCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvRefBank> InvRefBankDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvRefBank> InvRefBankUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvTmDetail> InvTmDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvTmDetail> InvTmDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvTmDetail> InvTmDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvTranBankTax> InvTranBankTaxCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvTranBankTax> InvTranBankTaxDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvTranBankTax> InvTranBankTaxUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvTranFile> InvTranFileCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvTranFile> InvTranFileDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvTranInvoiceRequestContact> InvTranInvoiceRequestContactCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvTranInvoiceRequestContact> InvTranInvoiceRequestContactDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvTranInvoiceRequestContact> InvTranInvoiceRequestContactUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequestCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequestDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<InvTranInvoiceRequest> InvTranInvoiceRequestUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<ItUserMaster> InverseCreatedByNavigation { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<ItUserMaster> InverseDeletedByNavigation { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<ItUserMaster> InverseUpdatedByNavigation { get; set; }
        [InverseProperty("UserIt")]
        public virtual ICollection<ItLoginLog> ItLoginLogs { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ItUserCuBrand> ItUserCuBrands { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ItUserCuDepartment> ItUserCuDepartments { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ItUserRole> ItUserRoles { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<KpiTemplate> KpiTemplates { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<LogBookingFbQueue> LogBookingFbQueues { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<LogEmailQueueAttachment> LogEmailQueueAttachments { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<LogEmailQueue> LogEmailQueues { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<MidNotification> MidNotifications { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<MidTask> MidTasks { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<OmDetail> OmDetailCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<OmDetail> OmDetailDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<OmDetail> OmDetailUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QcBlCustomer> QcBlCustomers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QcBlProductCatgeory> QcBlProductCatgeories { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QcBlProductSubCategory> QcBlProductSubCategories { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QcBlProductSubCategory2> QcBlProductSubCategory2S { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QcBlSupplierFactory> QcBlSupplierFactories { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QcBlockList> QcBlockListCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<QcBlockList> QcBlockListDeletedByNavigations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<QuPdfversion> QuPdfversions { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QuQuotationAudManday> QuQuotationAudMandayCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<QuQuotationAudManday> QuQuotationAudMandayDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<QuQuotationAudManday> QuQuotationAudMandayUpdatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<QuQuotationAudit> QuQuotationAudits { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QuQuotationInspManday> QuQuotationInspMandayCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<QuQuotationInspManday> QuQuotationInspMandayDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<QuQuotationInspManday> QuQuotationInspMandayUpdatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<QuQuotationInsp> QuQuotationInsps { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<QuQuotationPdfVersion> QuQuotationPdfVersions { get; set; }
        [InverseProperty("ValidatedByNavigation")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QuTranStatusLog> QuTranStatusLogs { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<QuWorkLoadMatrix> QuWorkLoadMatrixCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<QuWorkLoadMatrix> QuWorkLoadMatrixDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<QuWorkLoadMatrix> QuWorkLoadMatrixUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<RefBudgetForecast> RefBudgetForecastCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<RefBudgetForecast> RefBudgetForecastDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<RefBudgetForecast> RefBudgetForecastUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<RefCounty> RefCountyCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<RefCounty> RefCountyDeletedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<RefCounty> RefCountyModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<RefDelimiter> RefDelimiters { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<RefProductCategoryApiService> RefProductCategoryApiServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<RefProductCategoryApiService> RefProductCategoryApiServiceDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<RefProductCategorySub3> RefProductCategorySub3CreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<RefProductCategorySub3> RefProductCategorySub3DeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<RefProductCategorySub3> RefProductCategorySub3UpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<RefTown> RefTownCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<RefTown> RefTownDeletedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<RefTown> RefTownModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<RestApiLog> RestApiLogs { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SchQctype> SchQctypeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SchQctype> SchQctypeDeletedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<SchQctype> SchQctypeModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SchScheduleC> SchScheduleCCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SchScheduleC> SchScheduleCDeletedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<SchScheduleC> SchScheduleCModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SchScheduleQc> SchScheduleQcCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SchScheduleQc> SchScheduleQcDeletedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<SchScheduleQc> SchScheduleQcModifiedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuApiService> SuApiServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SuApiService> SuApiServiceDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuContactApiService> SuContactApiServiceCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SuContactApiService> SuContactApiServiceDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuContact> SuContacts { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuCreditTerm> SuCreditTermCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SuCreditTerm> SuCreditTermDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuEntity> SuEntityCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SuEntity> SuEntityDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuGrade> SuGradeCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SuGrade> SuGradeDeletedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<SuGrade> SuGradeUpdatedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuStatus> SuStatusCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SuStatus> SuStatusDeletedByNavigations { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<SuSupplier> SuSupplierCreatedByNavigations { get; set; }
        [InverseProperty("DeletedByNavigation")]
        public virtual ICollection<SuSupplier> SuSupplierDeletedByNavigations { get; set; }
        [InverseProperty("ModifiedByNavigation")]
        public virtual ICollection<SuSupplier> SuSupplierModifiedByNavigations { get; set; }
    }
}
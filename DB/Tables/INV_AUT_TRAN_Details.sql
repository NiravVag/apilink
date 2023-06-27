
CREATE TABLE [dbo].[INV_AUT_TRAN_Details]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
	[InvoiceNo] nvarchar(1000),
	[InvoiceDate] DateTime null,
	[PostedDate] DateTime null,
	[UnitPrice] float null,
	[InspectionFees] float null,
	[TravelAirFees] float null,
	[TravelLandFees] float null,
	[HotelFees] float null,
	[OtherFees] float null,
	[Discount] float null,
	[TotalTaxAmount] float null,
	[TaxValue] float null,
	[TotalInvoiceFees] float null,
	[TotalSampleSize] int null,
	[PriceCardCurrency] int null,
	[InvoiceCurrency] int null,
	[ExchangeRate] float null,
	[RuleExchangeRate] float null,
	[InvoiceTo] int null,
	[InvoiceMethod] int null,
	[ManDays] float null,
	[TravelMatrixType] int null,
	[InvoicedName] nvarchar(1000) null,
	[InvoicedAddress] nvarchar(2000) null,
	[Office] int null,
	[PaymentTerms] nvarchar(2000),
	[PaymentDuration] nvarchar(1000),
	[BankId] int null,
	[IsAutomation] Bit null,
	[IsInspection] Bit null,
	[IsTravelExpense] bit null,
	[InspectionId] int null,
	[AuditId] int null,
	[ProrateBookingNumbers] nvarchar(1000) NULL,
	[InvoiceStatus] int null,
	[InvoicePaymentStatus] int null,
	[InvoicePaymentDate] DateTime null,
	[RuleId] int null,
	InvoiceType INT Null,
	[ServiceId] INT NULL,
	[CalculateInspectionFee] int null,
	[CalculateTravelExpense] int null,
	[CalculateHotelFee] int null,
	[CalculateDiscountFee] int null,
	[CalculateOtherFee] int null,
	[Remarks] nvarchar(max) null,
	[Subject] nvarchar(max) null,
	[CreatedBy] int null,
	[CreatedOn] DateTime null,	
	[PriceCalculationType] INT NULL,
	[PriceComplexType] INT NULL,
	[TemplateId] INT Null,
	[Additional_BD_Tax] FLOAT NULL,


	CONSTRAINT INV_AUT_TRAN_Details_RuleId FOREIGN KEY ([RuleId]) REFERENCES [dbo].[CU_PR_Details](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InspectionId FOREIGN KEY ([InspectionId]) REFERENCES [dbo].[INSP_Transaction](Id),

	CONSTRAINT INV_AUT_TRAN_Details_AuditId FOREIGN KEY ([AuditId]) REFERENCES [dbo].[AUD_Transaction](Id),

	CONSTRAINT INV_AUT_TRAN_Details_BankId FOREIGN KEY ([BankId]) REFERENCES [dbo].[INV_REF_Bank](Id),

	CONSTRAINT INV_AUT_TRAN_Details_Office FOREIGN KEY ([Office]) REFERENCES [dbo].[INV_REF_Office](Id),

	CONSTRAINT INV_AUT_TRAN_Details_TravelMatrixType FOREIGN KEY ([TravelMatrixType]) REFERENCES [dbo].[INV_TM_Type](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceMethod FOREIGN KEY ([InvoiceMethod]) REFERENCES [dbo].[QU_BillMethod](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceTo FOREIGN KEY ([InvoiceTo]) REFERENCES [dbo].[QU_PaidBy](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceCurrency FOREIGN KEY ([InvoiceCurrency]) REFERENCES [dbo].[REF_Currency](Id),

	CONSTRAINT INV_AUT_TRAN_Details_PriceCardCurrency FOREIGN KEY ([PriceCardCurrency]) REFERENCES [dbo].[REF_Currency](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceStatus FOREIGN KEY ([InvoiceStatus]) REFERENCES [dbo].[INV_Status](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoicePaymentStatus FOREIGN KEY ([InvoicePaymentStatus]) REFERENCES [dbo].[INV_Payment_Status](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateInspectionFee FOREIGN KEY ([CalculateInspectionFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateTravelExpense FOREIGN KEY ([CalculateTravelExpense]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateHotelFee FOREIGN KEY ([CalculateHotelFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateDiscountFee FOREIGN KEY ([CalculateDiscountFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateOtherFee FOREIGN KEY ([CalculateOtherFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	,

	CONSTRAINT FK_InvoiceType FOREIGN KEY (InvoiceType) REFERENCES REF_InvoiceType(Id),

	CONSTRAINT INV_AUT_TRAN_Details_ServiceId  FOREIGN KEY(ServiceId) REFERENCES [REF_Service](Id),

	CONSTRAINT INV_AUT_TRAN_Details_PriceCalculationType FOREIGN KEY ([PriceCalculationType]) REFERENCES [dbo].[INV_REF_PriceCalculationType](Id),

	CONSTRAINT INV_AUT_TRAN_Details_PriceComplexType  FOREIGN KEY(PriceComplexType) REFERENCES [dbo].[CU_PR_RefComplexType](Id)
)
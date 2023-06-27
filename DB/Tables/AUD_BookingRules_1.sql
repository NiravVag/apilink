CREATE TABLE [dbo].[AUD_BookingRules]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Customer_id] [int] NULL,
	[LeadDays] INT NOT NULL, 
    [Factory_CountryId] INT NULL, 
    [IsDefault] BIT NOT NULL, 
    [Active] BIT NOT NULL, 
    [Booking_Rule] NVARCHAR(3000) NULL, 
    [EntityId] INT NULL, 
    FOREIGN KEY ([Customer_id]) REFERENCES [dbo].[CU_Customer](Id),
	FOREIGN KEY([Factory_CountryId]) REFERENCES [dbo].[REF_Country](Id),
	 FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id)
)
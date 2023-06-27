CREATE TABLE [dbo].[REF_ServiceType]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[Active] [bit] NOT NULL,
	[EntityId] INT NULL, 
    [IsReInspectedService] BIT NULL, 
    [Fb_ServiceType_Id] INT NULL, 
	[Abbreviation] [nvarchar] (50) NULL,
	ServiceId INT,
	BusinessLineId INT,
	ShowServiceDateTo BIT,
	[IsAutoQCExpenseClaim] BIT NULL,
	Is100Inspection BIT,
	CONSTRAINT FK_SERVICE FOREIGN KEY(ServiceId) REFERENCES [dbo].[REF_SERVICE],
	CONSTRAINT FK_BUSSINESS_LINE FOREIGN KEY(BusinessLineId) REFERENCES [dbo].[REF_BUSINESS_LINE],
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

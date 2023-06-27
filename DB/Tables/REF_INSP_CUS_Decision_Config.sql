CREATE TABLE [dbo].[REF_INSP_CUS_Decision_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[CustomDecisionName] NVARCHAR(200) NULL, 
	[CustomerId] int NULL,
	[Active] BIT NULL,
	[Default] BIT NULL,
	[CusDecId] int NOT NULL,
	CONSTRAINT FK_CDCustomerId FOREIGN KEY(CustomerId) REFERENCES [CU_Customer](Id),
	CONSTRAINT FK_CDCusDecId FOREIGN KEY(CusDecId) REFERENCES [REF_INSP_CUS_decision](Id)
)
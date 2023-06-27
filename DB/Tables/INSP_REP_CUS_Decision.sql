CREATE TABLE [dbo].[INSP_REP_CUS_Decision]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[ReportId] int NOT NULL,
	[CustomerResultId] int NOT NULL,
	[Comments] NVARCHAR(max) NULL, 
	[Active] BIT NULL,
	[CreatedBy] INT NULL, 
	[CreatedOn] DATETIME NULL default GETDATE(), 	
	[IsAutoCustomerDecision] BIT NULL,
	CONSTRAINT FK_CDCreatedBy FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	CONSTRAINT FK_CDReportId FOREIGN KEY(ReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_CDCustomerResultId FOREIGN KEY(CustomerResultId) REFERENCES [REF_INSP_CUS_Decision_Config](Id)
)
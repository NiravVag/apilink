CREATE TABLE [dbo].[AUD_TRAN_Report_Details]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Audit_Id] INT NOT NULL,
	[ServiceDate_From] DATETIME NOT NULL, 
    [ServiceDate_To] DATETIME NOT NULL, 
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Active] BIT NOT NULL, 
	[Comments] NVARCHAR(2000) NULL, 
    FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id)	
)

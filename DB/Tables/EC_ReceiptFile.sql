CREATE TABLE [dbo].[EC_ReceiptFile](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[GuidId] [uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE, 
	[EntityId] INT NULL, 
	[FullFileName] [nvarchar](200) NOT NULL,
	[File] VARBINARY(MAX) FILESTREAM NULL,
	[UserId] [int] NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[ExpenseId] INT NOT NULL,
	FOREIGN KEY ([ExpenseId]) REFERENCES [dbo].[EC_ExpensesClaimDetais](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT EC_ReceiptFile_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
	)

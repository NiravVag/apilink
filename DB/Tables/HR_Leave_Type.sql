CREATE TABLE [dbo].[HR_Leave_Type](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Description] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[Total_Days] [int] NULL,
	IdTran INT NULL,
	[EntityId] INT NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

CREATE TABLE [dbo].[HR_Department](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Department_Name] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[Department_Code] [nvarchar](50) NULL, 
	[DeptParentId] INT NULL,
	[EntityId] INT NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id),
	FOREIGN KEY([DeptParentId]) REFERENCES [dbo].[HR_Department](Id)
	)

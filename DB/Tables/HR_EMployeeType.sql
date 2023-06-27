CREATE TABLE [dbo].[HR_EMployeeType](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[EmployeeTypeName] [nvarchar](200) NOT NULL,
	[EntityId] INT NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
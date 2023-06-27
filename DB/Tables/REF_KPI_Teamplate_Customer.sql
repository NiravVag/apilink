CREATE TABLE [dbo].[REF_KPI_Teamplate_Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[TeamplateId] [int] NULL,
	[CustomerId] [int] NULL,
	[Active] BIT NOT NULL, 
	[UserTypeId] [int] NULL,
	FOREIGN KEY([TeamplateId]) REFERENCES REF_KPI_Teamplate(Id),
	FOREIGN KEY([CustomerId]) REFERENCES CU_Customer(Id),
	FOREIGN KEY([UserTypeId]) REFERENCES IT_UserType(Id)
)
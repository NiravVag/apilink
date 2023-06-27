CREATE TABLE [dbo].[REF_KPI_Teamplate]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [CustomerId] INT NULL, 
    [Active] BIT NOT NULL, 
    [TypeId] INT NULL,
    [IsDefault] Bit,
    IsDefaultCustomer BIT,
	FOREIGN KEY (TypeId) REFERENCES [REF_KPI_Template_Type](Id)
)

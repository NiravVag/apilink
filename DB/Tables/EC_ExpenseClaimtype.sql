CREATE TABLE [dbo].[EC_ExpenseClaimtype]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(100) NOT NULL, 
    [Active] BIT NOT NULL
)

CREATE TABLE [dbo].[CU_CS_Onsite_Email]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [EmailId] NVARCHAR(MAX) NOT NULL, 
    [Active] BIT NOT NULL,
	FOREIGN KEY ([UserId]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY ([CustomerId]) REFERENCES Cu_Customer(Id)
)

CREATE TABLE [dbo].[REF_City](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Province_Id] [int] NOT NULL,
	[City_Name] [nvarchar](150) NOT NULL,
	[Active] [bit] NOT NULL,
    [Ph_Code] NCHAR(20) NULL
    FOREIGN KEY ([Province_Id]) REFERENCES[dbo].[REF_Province](Id)
)

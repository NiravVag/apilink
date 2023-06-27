CREATE TABLE [dbo].[REF_Province](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Province_Code] [nvarchar](50) NULL,
	[Country_Id] [int] NOT NULL,
	[Province_Name] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL DEFAULT 1,
	[FB_ProvinceId] INT  NULL ,
	[Longitude] DECIMAL(12, 9) NULL, 
    [Latitude] DECIMAL(12, 9) NULL, 
    FOREIGN KEY([Country_Id]) REFERENCES [dbo].[REF_Country](Id)
)

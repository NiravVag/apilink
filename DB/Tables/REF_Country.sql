
CREATE TABLE [dbo].[REF_Country](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Country_Code] [int] NULL,
	[Alpha2Code] [char](2) NULL,
	[Area_id] [int] NULL,
	[Country_Name] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[Priority] INT NOT NULL DEFAULT 0, 
	[FB_CountryId] INT  NULL,
    [Longitude] DECIMAL(12, 9) NULL, 
    [Latitude] DECIMAL(12, 9) NULL, 
    FOREIGN KEY([Area_id]) REFERENCES[dbo].[REF_Area] (Id)
	)

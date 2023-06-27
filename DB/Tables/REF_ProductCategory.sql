
CREATE TABLE [dbo].[REF_ProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[Active] [bit] NOT NULL,
	[EntityId] INT NULL,
	[BusinessLineId] INT NULL,
	[Fb_ProductCategory_Id] INT NULL, 
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id),
	FOREIGN KEY([BusinessLineId]) REFERENCES REF_BUSINESS_LINE(Id)

)
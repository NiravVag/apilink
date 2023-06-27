CREATE TABLE [dbo].[FB_Report_Template]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(1000) NULL, 
    [FbTemplateId] INT NULL, 
    [Active] BIT NULL,
    Company_Id int null
    CONSTRAINT FK_FB_Report_Template_Company_Id FOREIGN KEY (Company_Id) REFERENCES AP_Entity(Id)
)

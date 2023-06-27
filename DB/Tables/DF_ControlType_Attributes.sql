CREATE TABLE [dbo].[DF_ControlType_Attributes] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[AttributeId] INT NOT NULL,
	[ControlTypeId] INT NOT NULL,
	[DefaultValue] NVARCHAR(50) NULL,
	[Active] BIT NOT NULL, 
    FOREIGN KEY([AttributeId]) REFERENCES DF_Attributes(Id),
	FOREIGN KEY([ControlTypeId]) REFERENCES DF_ControlTypes(Id)
)

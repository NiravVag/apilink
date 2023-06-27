CREATE TABLE [dbo].[DF_Control_Attributes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ControlAttributeId] INT NOT NULL,
	[Value] NVARCHAR(50) NOT NULL,
	[ControlConfigurationID] INT NOT NULL,
	[Active] BIT NOT NULL,
	FOREIGN KEY(ControlConfigurationID) REFERENCES DF_CU_Configuration(Id),
	FOREIGN KEY(ControlAttributeId) REFERENCES DF_ControlType_Attributes(Id)
)

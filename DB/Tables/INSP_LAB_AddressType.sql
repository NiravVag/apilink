CREATE TABLE [dbo].[INSP_LAB_AddressType]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Address_type] [nvarchar](50) NOT NULL,
	[TranslationId] [int] NULL,
	[EntityId] [int] NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

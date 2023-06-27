CREATE TABLE [dbo].[SU_AddressType](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Address_Type] [nvarchar](50) NOT NULL,
	[Address_Type_Flag] [char](1) NOT NULL,
	[TranslationId] INT NULL,
	[EntityId] INT NULL, 
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

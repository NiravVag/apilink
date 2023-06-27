CREATE TABLE [dbo].[EC_PaymenTypes](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[Description] [nvarchar](50) NOT NULL,
	[TransId] INT NULL	,
	 [EntityId] INT NULL,
	 FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
	)

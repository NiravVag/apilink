CREATE TABLE [dbo].[REF_CITY_DETAILS]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY, 
    [City_Id] INT NOT NULL, 
    [Location_Id] INT NULL, 
    [Zone_Id] INT NULL, 
    [Travel_Time] FLOAT NULL, 
    [EntityId] INT NULL, 
    [Active] BIT NOT NULL
	FOREIGN KEY([Location_Id])REFERENCES [dbo].[REF_Location] ([Id]),
	FOREIGN KEY([City_Id])REFERENCES [dbo].[REF_City] ([Id]),
	FOREIGN KEY([Zone_Id])REFERENCES [dbo].[REF_Zone] ([Id]),
	FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id)
)

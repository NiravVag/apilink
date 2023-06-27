CREATE TABLE [dbo].[SU_Level_Custom](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[LevelId] [int] NULL,
	[CustomerId] [int] NULL,
	[IsDefault] [bit] NOT NULL,
	[CustomName] [nvarchar](500) NULL,
	CONSTRAINT FK_SU_Level_Custom_CustomerId FOREIGN KEY(CustomerId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_SU_Level_Custom_LevelId FOREIGN KEY(LevelId) REFERENCES SU_Level(Id)
)
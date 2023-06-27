CREATE TABLE [IT_Right]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[ParentId] [int] NULL,
	[TitleName] [varchar](500) NULL,
	[MenuName] [varchar](500) NULL,
	[Path] [varchar](500) NULL,
	[IsHeading] [bit] NULL,
	[Active] [bit] NULL,
	[Glyphicons] [varchar](500) NULL,
	[Ranking] [int] NULL,
	[MenuName_IdTran] INT NULL,
	[TitleName_IdTran] INT NULL,
	[EntityId] INT NULL, 
	[ShowMenu] BIT NOT NULL DEFAULT(1),
	[RightType] INT NULL, 
    FOREIGN KEY([ParentId]) REFERENCES [dbo].[IT_Right](Id),
	FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY([RightType]) REFERENCES [dbo].[IT_Right_Type](Id)
)

CREATE TABLE [dbo].[IT_Role]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[RoleName] [varchar](500) NULL,
	[Active] [bit] NULL,
	[EntityId] INT NULL, 
	[PrimaryRole] [bit] NOT NULL DEFAULT 1,
	[SecondaryRole] [bit] NOT NULL DEFAULT 0
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

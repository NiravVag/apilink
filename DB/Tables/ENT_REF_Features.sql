CREATE TABLE [dbo].[ENT_REF_Features]
(
	[Id] int not null primary key identity(1,1),
	[Name] nvarchar(500),	
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	[Active] [bit] NULL,
	CONSTRAINT [FK_ENT_REF_Features_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_ENT_REF_Features_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
)

CREATE TABLE [dbo].[SU_Contact](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[Supplier_id] [int] NOT NULL,
	[Contact_name] [nvarchar](100) NULL,
	[Phone] [nvarchar](200) NULL,
	[Fax] [nvarchar](200) NULL,
	[Mail] [nvarchar](100) NULL,
	[active] [bit] NOT NULL DEFAULT(1),
	[JobTitle] [nvarchar](500) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Comment] [nvarchar](1000) NULL,
	[PrimaryEntity] [int] NULL,
	FOREIGN KEY([Supplier_id]) REFERENCES [dbo].[SU_Supplier](Id),
	CONSTRAINT FK_SU_Contact_PrimaryEntity FOREIGN KEY(PrimaryEntity) REFERENCES AP_Entity(Id)
)
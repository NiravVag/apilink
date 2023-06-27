CREATE TABLE [dbo].[REF_File_Extension](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExtensionName] [nvarchar](50) NULL,
	[Active] [bit] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_REF_File_Extension] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[REF_File_Extension] ADD  CONSTRAINT [DF_REF_File_Extension_Active]  DEFAULT ((1)) FOR [Active]
GO
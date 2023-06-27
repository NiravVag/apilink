CREATE TABLE [dbo].[CU_REF_RelationshipStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Active] [int] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_CU_REF_RelationshipStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CU_REF_RelationshipStatus] ADD  CONSTRAINT [DF_CU_REF_RelationshipStatus_Active]  DEFAULT ((1)) FOR [Active]
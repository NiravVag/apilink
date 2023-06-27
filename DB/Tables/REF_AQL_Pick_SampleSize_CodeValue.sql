CREATE TABLE [dbo].[REF_AQL_Pick_SampleSize_CodeValue]
(
    [Sample_Size_Code] [nvarchar](1) NOT NULL,
	[Sample_Size] [smallint] NOT NULL,
	CONSTRAINT [PK_REF_AQL_Pick_SampleSize_CodeValue] PRIMARY KEY CLUSTERED ([Sample_Size_Code] ASC) 
)

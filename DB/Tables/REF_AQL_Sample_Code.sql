CREATE TABLE [dbo].[REF_AQL_Sample_Code]
(
	[Sample_Size_range_Code_Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Min_size] [int] NOT NULL,
	[Max_size] [int] NOT NULL,
	[Level_I_Sample_Size_Code] [nvarchar](1) NOT NULL,
	[Level_II_Sample_Size_Code] [nvarchar](1) NOT NULL,
	[Level_III_Sample_Size_Code] [nvarchar](1) NOT NULL,
	[LEVEL_S1_SAMPLE_SIZE_CODE] [nvarchar](1) NOT NULL,
	[LEVEL_S2_SAMPLE_SIZE_CODE] [nvarchar](1) NOT NULL,
	[LEVEL_S3_SAMPLE_SIZE_CODE] [nvarchar](1) NOT NULL,
	[LEVEL_S4_SAMPLE_SIZE_CODE] [nvarchar](1) NOT NULL,

	CONSTRAINT [PK_REF_AQL_Sample_Code] PRIMARY KEY CLUSTERED ([Sample_Size_range_Code_Id] ASC) ,

	FOREIGN KEY([Level_I_Sample_Size_Code]) REFERENCES [dbo].[REF_AQL_Pick_SampleSize_CodeValue](Sample_Size_Code),
	FOREIGN KEY([Level_II_Sample_Size_Code]) REFERENCES [dbo].[REF_AQL_Pick_SampleSize_CodeValue](Sample_Size_Code),
	FOREIGN KEY([Level_III_Sample_Size_Code]) REFERENCES [dbo].[REF_AQL_Pick_SampleSize_CodeValue](Sample_Size_Code),
	FOREIGN KEY([LEVEL_S1_SAMPLE_SIZE_CODE]) REFERENCES [dbo].[REF_AQL_Pick_SampleSize_CodeValue](Sample_Size_Code),
	FOREIGN KEY([LEVEL_S2_SAMPLE_SIZE_CODE]) REFERENCES [dbo].[REF_AQL_Pick_SampleSize_CodeValue](Sample_Size_Code),
	FOREIGN KEY([LEVEL_S3_SAMPLE_SIZE_CODE]) REFERENCES [dbo].[REF_AQL_Pick_SampleSize_CodeValue](Sample_Size_Code),
	FOREIGN KEY([LEVEL_S4_SAMPLE_SIZE_CODE]) REFERENCES [dbo].[REF_AQL_Pick_SampleSize_CodeValue](Sample_Size_Code)
)


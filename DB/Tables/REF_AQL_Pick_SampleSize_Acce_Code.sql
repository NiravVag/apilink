CREATE TABLE [dbo].[REF_AQL_Pick_SampleSize_Acce_Code]
(
	[Id] INT NOT NULL PRIMARY KEY,
	Sample_Size_Code varchar(10),
	PickValue float,
	Acc_sample_Size_Code varchar(10),
	Accepted int,
	Rejected int
)

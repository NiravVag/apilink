CREATE TABLE [dbo].[HR_Staff_Expertise](
	[staff_id] [int] NOT NULL,
	[ExpertiseId] [int] NOT NULL,
	PRIMARY KEY([staff_id], [ExpertiseId]),
	FOREIGN KEY([staff_id]) REFERENCES [dbo].[HR_Staff](Id),
	FOREIGN KEY([ExpertiseId]) REFERENCES [dbo].[REF_Expertise](Id)	
	)

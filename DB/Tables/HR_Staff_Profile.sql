CREATE TABLE [dbo].[HR_Staff_Profile]
(
	[staff_id] [int] NOT NULL,
	[profile_id] INT NOT NULL,
	PRIMARY KEY([staff_id], [profile_id]),
	FOREIGN KEY([staff_id]) REFERENCES [dbo].[HR_Staff](Id),
	FOREIGN KEY([profile_id]) REFERENCES [dbo].[HR_Profile](Id)	
)


CREATE TABLE [dbo].[HR_Staff_OpCountry]
(
	[staff_id] [int] NOT NULL,
	[country_id] [int] NOT NULL,
	PRIMARY KEY([staff_id], [country_id]),
	FOREIGN KEY ([country_id]) REFERENCES [dbo].[REF_Country](Id),
	FOREIGN KEY([staff_id]) REFERENCES [dbo].[HR_Staff](Id)
)
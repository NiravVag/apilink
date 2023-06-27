CREATE TABLE [dbo].[HR_Staff_MarketSegment](
	[staff_id] [int] NOT NULL,
	[MarketSegmentId] [int] NOT NULL,
	PRIMARY KEY([staff_id], [MarketSegmentId]),
	FOREIGN KEY([MarketSegmentId]) REFERENCES [dbo].[REF_MarketSegment](Id),
	FOREIGN KEY([staff_id]) REFERENCES [dbo].[HR_Staff](Id))

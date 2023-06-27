CREATE TABLE [dbo].[HR_Staff_ProductCategory](
	[staff_id] [int] NOT NULL,
	[ProductCategoryId] [int] NOT NULL,
	PRIMARY KEY([staff_id], [ProductCategoryId]),
	FOREIGN KEY([staff_id]) REFERENCES [dbo].[HR_Staff](Id),
	FOREIGN KEY([ProductCategoryId]) REFERENCES [dbo].[REF_ProductCategory]	(Id)	
)


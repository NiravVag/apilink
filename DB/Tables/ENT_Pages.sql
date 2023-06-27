﻿CREATE TABLE [dbo].[ENT_Pages]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[RightId] INT,
	[ServiceId] INT,
	[Active] BIT,
	[Remarks] NVARCHAR(500),
	CONSTRAINT FK_ENT_PAGE_SERVICE FOREIGN KEY(ServiceId) REFERENCES [dbo].[REF_Service]
)

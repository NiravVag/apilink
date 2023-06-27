﻿CREATE TABLE [dbo].[CU_Season_Config]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	SeasonId INT,
	CustomerId INT,
	Active BIT,
	IsDefault BIT,
	EntityId INT,
	CONSTRAINT FK_CUSTOMER_CU_SEASON_CONFIG FOREIGN KEY(CustomerId) REFERENCES [dbo].[CU_Customer],
	CONSTRAINT FK_SEASON_CU_SEASON_CONFIG FOREIGN KEY(SeasonId) REFERENCES [dbo].[REF_Season],
	CONSTRAINT FK_CU_SEASON_CONFIG_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]
)
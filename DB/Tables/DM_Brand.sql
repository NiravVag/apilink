﻿CREATE TABLE DM_Brand(
Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
BrandId INT NOT NULL,
DMFileId INT NULL,
CONSTRAINT FK_DM_Brand_BrandId FOREIGN KEY(BrandId) REFERENCES CU_Brand(Id),
CONSTRAINT FK_DM_Brand_DMFileId FOREIGN KEY(DMFileId) REFERENCES DM_File(Id)
)
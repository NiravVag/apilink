﻿CREATE TABLE INSP_REF_PackingStatus (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    Name NVARCHAR(200),
    Active BIT,
    Sort INT
);
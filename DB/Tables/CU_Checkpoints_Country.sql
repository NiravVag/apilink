CREATE TABLE CU_CheckPoints_Country(
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	CheckpointId int NOT NULL,
	CountryId int NOT NULL,
	Active bit NOT NULL,
	EntityId int NULL,
	CreatedBy int NULL,
	CreatedOn datetime NULL,
	DeletedBy int NULL,
	DeletedOn datetime NULL,
	FOREIGN KEY(CheckpointId) REFERENCES CU_CheckPoints (Id),
	FOREIGN KEY(CountryId) REFERENCES Ref_country(Id),
	FOREIGN KEY(EntityId) REFERENCES AP_Entity (Id)
)
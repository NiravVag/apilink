CREATE TABLE REF_Country_Location (
	CountryId INT NOT NULL, 
	LocationId INT NOT NULL,
	PRIMARY KEY(CountryId, LocationId),
	FOREIGN KEY(CountryId) REFERENCES  [dbo].REF_Country(Id),
	FOREIGN KEY(LocationId) REFERENCES  [dbo].REF_Location(Id)
)
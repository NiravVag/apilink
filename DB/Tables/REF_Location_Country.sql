CREATE TABLE [REF_Location_Country]
(
	[LocationID] INT NOT NULL,
	[CountryID] INT NOT NULL
	PRIMARY KEY([LocationID], [CountryID]),
	FOREIGN KEY([LocationID]) REFERENCES [REF_Location]([Id]),
	FOREIGN KEY([CountryID]) REFERENCES [REF_Country]([Id])
)

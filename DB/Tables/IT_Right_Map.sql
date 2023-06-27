CREATE TABLE [IT_Right_Map] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [RightId] int NOT NULL,
    [RightTypeId] int NOT NULL,
	[Active] bit NOT NULL,
    FOREIGN KEY ([RightId]) REFERENCES [IT_Right]([Id]),
	FOREIGN KEY ([RightTypeId]) REFERENCES [IT_Right_Type]([Id])
)
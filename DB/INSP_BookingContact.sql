CREATE TABLE [dbo].[INSP_BookingContact]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Factory_Country_Id] INT NULL,
	[Office_Id] INT NOT NULL,
	[Booking_EmailTo] NVARCHAR(500) NULL,
    [Booking_EmailCC] NVARCHAR(500) NULL,
    [Penalty_Email] NVARCHAR(500) NULL,
    [Contact_Information] NVARCHAR(2500) NULL,
    [Active] BIT NOT NULL,
    [EntityId] INT NULL,
    FOREIGN KEY([Office_Id]) REFERENCES [dbo].[REF_Location](Id),
    FOREIGN KEY([Factory_Country_Id]) REFERENCES [dbo].[REF_Country](Id),
	FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id)
)

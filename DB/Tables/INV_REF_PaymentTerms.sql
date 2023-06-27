   CREATE TABLE INV_REF_PaymentTerms
   (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(1000),
		[Duration] INT NULL,
		[Active]  Bit
   )

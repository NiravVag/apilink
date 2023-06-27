  CREATE TABLE INV_REF_Office
	(
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(100),		
		[Address] NVARCHAR(max),
		[Phone] NVARCHAR(100),
		[Fax] NVARCHAR(100),
		[Website] NVARCHAR(100),
		[Mail] NVARCHAR(100),
		[Active]  Bit
	)

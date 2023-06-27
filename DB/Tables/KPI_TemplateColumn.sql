	CREATE TABLE KPI_TemplateColumn (
		Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
		IdTemplate INT NOT NULL, 
		IdColumn INT NULL,
		[ColumnName] NVARCHAR(200)  NULL,
		[SumFooter] BIT NULL,
		[Group] BIT NULL,
		[OrderColumn] INT NULL, 
		[OrderFilter] INT NULL, 
		[SelectMultiple] BIT NULL,
		[Required] BIT NOT NULL,
		[FilterLazy] BIT NULL,
		[Valuecolumn] NVARCHAR(300) NULL
		FOREIGN  KEY (IdTemplate) REFERENCES [KPI_Template](Id),
		FOREIGN  KEY (IdColumn) REFERENCES [KPI_Column](Id)
	)
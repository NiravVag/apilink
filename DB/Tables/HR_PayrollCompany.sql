CREATE TABLE HR_PayrollCompany(
[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
[CompanyName] NVARCHAR(200),
[Active] BIT NOT NULL,
[Sort] INT,
[AccountEmail] NVARCHAR(1000),
[Entity] INT NOT NULL,
CONSTRAINT FK_HR_Payrol_Entity FOREIGN KEY([Entity]) REFERENCES [dbo].[AP_Entity](Id))
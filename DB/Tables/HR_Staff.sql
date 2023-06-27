﻿
CREATE TABLE [dbo].[HR_Staff](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,	
	[Person_Name] [nvarchar](50) NULL,
	[Gender] [char](1) NULL,
	[Marital_Status] [char](1) NULL,
	[Birth_Date] [datetime] NULL,
	[Join_Date] [datetime] NULL,
	[salary_Currency_Id] [int] NULL,
	[Parent_Staff_Id] [int] NULL,
	[Department_Id] [int] NULL,
	[Qualification_Id] [int] NULL,
	[Location_Id] [int] NULL,
	[Active] [bit] NULL,
	[Paid_Holiday_Days_Per_Year] [float] NULL,
	[Comments] [nvarchar](400) NULL,
	[Nationality_Country_Id] [int] NULL,
	[Name_Chinese] [nvarchar](50) NULL,
	[Working_Days_Of_Week] [float] NULL,
	[Passport_no] [nvarchar](100) NULL,
	[Probation_Period] [int] NULL,
	[place_of_Birth] [nvarchar](100) NULL,
	[Bank_Name] [nvarchar](100) NULL,
	[Bank_Account_No] [nvarchar](50) NULL,
	[Salary] [float] NULL,
	[Emp_no] [nvarchar](50) NULL,
	[Emergency_Call] [nvarchar](50) NULL,
	[Leave_Date] [datetime] NULL,
	[GL_Code] [nvarchar](50) NULL,
	[Probation_Expired_Date] [datetime] NULL,
	[Labor_Contract_Expired_Date] [datetime] NULL,
	[Created_At] [datetime] NULL,
	[Created_By] [numeric](18, 0) NULL,
	[Modified_At] [datetime] NULL,
	[Modified_By] [numeric](18, 0) NULL,
	[Prefer_Currency_Id] [int] NULL,
	[OutSource] [int] NULL,
	[Position_Id] [int] NULL,
	[Start_Port] [int] NULL,
	[GraduateSchool] [nvarchar](200) NULL,
	[EmergencyContactName] [nvarchar](200) NULL,
	[GraduateDate] [datetime] NULL,
	[EmergencyContactPhone] [varchar](100) NULL,
	[EmaiLAddress] [varchar](50) NULL,
	[CompanyMobileNo] [varchar](50) NULL,
	[SocialInsuranceCardNo] [varchar](50) NULL,
	[HousingFuncard] [varchar](50) NULL,
	[PlacePurchasingSIHF] [varchar](50) NULL,
	[LaborContractPeriod] [varchar](50) NULL,
	[Current_ZipCode] [varchar](50) NULL,
	[Current_Town] [varchar](50) NULL,
	[StartWorkingDate] [datetime] NULL,
	[TotalWorkingYears] [int] NULL,
	[Home_CityId] [int] NULL,
	[Home_Address] [nvarchar](200) NULL,
	[Current_CityId] [int] NULL,	
	[Current_Address] [nvarchar](200) NULL,
	[PayrollCurrencyId] [int] NULL,
	[EmployeeTypeId] INT NOT NULL,
	[ManagerId] INT NULL,
	[SkypeId] NVARCHAR(10) NULL,
	[LocalLanguage] NVARCHAR(10) NULL,
	[CompanyEmail] NVARCHAR(100) NULL,
	[AnnualLeave] NVARCHAR(10) NULL,	
	[PrimaryEntity] int NULL,
	[IsForecastApplicable] BIT NULL,
	StatusId INT,
	BandId INT,
	SocialInsuranceTypeId INT,
	HukoLocationId INT,
	MajorSubject nvarchar(500),
	EmergencyContactRelationship nvarchar(500),
	GlobalGrading nvarchar(500),
	NoticePeriod int
	FOREIGN KEY(Position_Id) REFERENCES [dbo].[HR_Position](Id),
	[StartPortId] INT NULL, 
	[CompanyId] [int] NULL,
	[PayrollCompany] INT NULL,
	FOREIGN KEY([PayrollCompany]) REFERENCES [HR_PayrollCompany](Id),
    FOREIGN KEY([CompanyId]) REFERENCES [dbo].[AP_Entity](Id),
    FOREIGN KEY([salary_Currency_Id]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY([Parent_Staff_Id]) REFERENCES [dbo].[HR_Staff](Id),
	FOREIGN KEY([ManagerId]) REFERENCES [dbo].[HR_Staff](Id),
	FOREIGN KEY([Qualification_Id]) REFERENCES [dbo].[HR_Qualification](Id),
	FOREIGN KEY([Location_Id]) REFERENCES [dbo].[REF_Location](Id),
	FOREIGN KEY([Nationality_Country_Id]) REFERENCES [dbo].[REF_Country](Id),
	FOREIGN KEY([Prefer_Currency_Id]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY([PayrollCurrencyId]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY([Home_CityId]) REFERENCES [dbo].[REF_City](Id),
	FOREIGN KEY([Current_CityId]) REFERENCES [dbo].[REF_City](Id),
	FOREIGN KEY([Department_Id]) REFERENCES [dbo].[HR_Department](Id),
	CONSTRAINT FK_HR_Staff_StatusId FOREIGN KEY(StatusId) REFERENCES [dbo].[HR_REF_Status],
	CONSTRAINT FK_HR_Staff_BandId FOREIGN KEY(BandId) REFERENCES [dbo].[HR_REF_Band],
	CONSTRAINT FK_HR_Staff_SocialInsuranceTypeId FOREIGN KEY(SocialInsuranceTypeId) REFERENCES [dbo].[HR_REF_Social_Insurance_type],
	CONSTRAINT FK_HR_Staff_HukoLocationId FOREIGN KEY(HukoLocationId) REFERENCES [dbo].[Ref_City],
	FOREIGN KEY([EmployeeTypeId]) REFERENCES [dbo].[HR_EMployeeType](Id),	
	CONSTRAINT FK_Hr_Staff_PrimaryEntity FOREIGN KEY(PrimaryEntity) REFERENCES AP_Entity(Id),
	FOREIGN KEY([StartPortId]) REFERENCES [dbo].[EC_AUT_REF_StartPort](Id),
	)
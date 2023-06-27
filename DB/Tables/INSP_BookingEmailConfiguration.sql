Create Table INSP_BookingEmailConfiguration(
Id int NOT NULL PRIMARY KEY IDENTITY (1,1),
CustomerId int NOT NULL,
Email nvarchar(max) NOT NULL,
FactoryCountryId int NULL,
BookingStatusId int NOT NULL,
Active BIT NOT NULL,
EntityId int,
CONSTRAINT FK_INSP_BookingEmailConfiguration_CustomerId FOREIGN KEY (CustomerId) REFERENCES CU_Customer(Id),
CONSTRAINT FK_INSP_BookingEmailConfiguration_BookingStatusId FOREIGN KEY (BookingStatusId) REFERENCES INSP_Status(Id),
CONSTRAINT FK_INSP_BookingEmailConfiguration_FactoryCountryId FOREIGN KEY (FactoryCountryId) REFERENCES REF_Country(Id),
CONSTRAINT FK_INSP_BookingEmailConfiguration_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
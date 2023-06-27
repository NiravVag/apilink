﻿CREATE TABLE DM_Role(
	Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	RoleId int,
	StaffId int,
	EditRight bit NOT NULL,
	DownloadRight bit NOT NULL,
	DeleteRight bit NOT NULL,
	UploadRight bit NOT NULL, 
	EntityId int,
	CreatedBy int NULL,
	CreatedOn datetime NULL,
	UpdatedBy int NULL,
	UpdatedOn datetime NULL,
	Active bit NULL,
	DeletedBy int NULL,
	DeletedOn datetime NULL,
	CONSTRAINT FK_DM_Role_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id),
	CONSTRAINT FK_DM_Role_RoleId FOREIGN KEY(RoleId) REFERENCES IT_Role(Id),
	CONSTRAINT FK_DM_Role_StaffId FOREIGN KEY(StaffId) REFERENCES HR_Staff(Id),
	CONSTRAINT FK_DM_Role_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_DM_Role_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_DM_Role_DeletedyBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster(Id)
)
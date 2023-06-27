CREATE TABLE EC_Status_Role(
	IdRole INT NOT NULL, 
	IdStatus INT NOT NULL,
	PRIMARY KEY(IdRole, IdStatus),
	FOREIGN KEY (idRole) REFERENCES [dbo].[IT_Role](Id),
	FOREIGN KEY (IdStatus) REFERENCES [dbo].[EC_ExpClaimStatus](Id)
)

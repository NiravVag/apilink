CREATE VIEW V_GetStaffs
	AS
	SELECT HS.Id, RC.Country_Name, RL.Location_Name
	FROM HR_STAFF HS
	INNER JOIN REF_Country RC  ON RC.Id = HS.Nationality_Country_Id
	INNER JOIN REF_Location RL ON RL.Id = HS.Location_Id


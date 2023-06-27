CREATE  PROCEDURE [dbo].[AddViewInsp]
	AS 
BEGIN
	DECLARE @sSQL VARCHAR(MAX), 
			@sIdentityCols VARCHAR(MAX)

	SET @sIdentityCols  = '' 
	SELECT @sIdentityCols =( @sIdentityCols +CASE WHEN @sIdentityCols <> '' THEN ', ' ELSE  '' END +   + '[' + Label + ']') 
	FROM  DF_CU_Configuration
	
	SET @sSQL = ' IF EXISTS(SELECT 1  FROM sysobjects where name = ''V_Inspections'' AND xtype = ''V'' )
					DROP VIEW V_Inspections '
	PRINT @sSQL
	EXECUTE(@sSQL)

	IF @sIdentityCols = ''
		SET @sSQL = ' 	 CREATE VIEW V_Inspections  AS 
			SELECT IT.*, SU.Supplier_Name,   CU.Customer_Name, INS.Status
					from INSP_Transaction IT
					INNER JOIN CU_Customer CU ON CU.Id = IT.Customer_Id
					INNER JOIN SU_Supplier SU ON SU.id = IT.Supplier_id
					INNER JOIN INSP_Status INS ON INS.Id =IT.Status_Id 
		'
	ELSE
	SET @sSQL = ' 	 CREATE VIEW V_Inspections 
					AS 
					WITH T AS
					(
					SELECT   
					Id, '+ @sIdentityCols +' 
					FROM  
					(select  IT.Id, V.Label, V.Value
					from INSP_Transaction IT
					LEFT JOIN (SELECT c.Id,cucon.Label,trans.BookingId,
								case when cucon.ControlTypeId=3 then (SELECT top 1 dd.Name from DF_DDL_Source dd where dd.Id=trans.Value ) else trans.Value END as [Value] 		
								from DF_CU_Configuration cucon
								join CU_Customer c on c.Id=cucon.CustomerId
								JOIN INSP_DF_Transaction trans on trans.ControlConfigurationId=cucon.Id
								join DF_ControlTypes typ on typ.Id=cucon.ControlTypeId) V ON V.BookingId = IT.Id) As sourceTable
					PIVOT  
					(  
					MIN(Value)
					FOR Label IN ('+ @sIdentityCols +' )  
					) AS PivotTable )
					SELECT IT.*, SU.Supplier_Name,   CU.Customer_Name, INS.Status,' + @sIdentityCols +'
					from INSP_Transaction IT
					INNER JOIN CU_Customer CU ON CU.Id = IT.Customer_Id
					INNER JOIN SU_Supplier SU ON SU.id = IT.Supplier_id
					INNER JOIN INSP_Status INS ON INS.Id =IT.Status_Id 
					LEFT JOIN  T  ON T.Id = IT.Id
										
									  '

							PRINT @sSQL
							EXECUTE (@sSQL)

END
CREATE  TRIGGER [dbo].[TRG_KPI_COLUMNS_Dynamic]
   ON   [dbo].[DF_CU_Configuration]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	EXEC [dbo].[AddViewInsp] ;

	IF EXISTS(SELECT 1 FROM inserted) AND NOT EXISTS(SELECT 1 FROM deleted) -- INSERT 
		INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId])
		SELECT  inserted.Label AS [FieldLabel],  inserted.Label AS [FieldName], 'VARCHAR' AS [FieldType], [AP_SubModule].Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 0 AS [IsCustomerId]
		FROM [AP_SubModule], inserted
		WHERE [Name] = 'Report'
	ELSE IF NOT EXISTS(SELECT 1 FROM inserted) AND EXISTS(SELECT 1 FROM deleted) -- DELETE
	BEGIN
		DECLARE @IdColumn INT

		SELECT @IdColumn = KPI_Column.Id 
		FROM KPI_Column
		INNER JOIN AP_SubModule ASM ON ASM.Id = KPI_Column.IdSubModule 
		INNER JOIN inserted I ON  I.Label = KPI_Column.FieldName
		WHERE ASM.Name = 'Report'

		IF(@IdColumn IS NOT NULL)
		BEGIN
			DELETE FROM KPI_TemplateColumn WHERE IdColumn = @IdColumn
			DELETE FROM KPI_Column WHERE Id  = @IdColumn
		END
	END
	ELSE  -- UPDATE
		UPDATE KC
			SET KC.FieldName = I.Label,
				KC.FieldLabel = I.Label,
				KC.Active = I.Active	
		FROM KPI_Column KC
		INNER JOIN AP_SubModule ASM ON ASM.Id = KC.IdSubModule 
		INNER JOIN inserted I ON  I.Label = KC.FieldName
		WHERE ASM.Name = 'Report'
END

ALTER TABLE [dbo].[DF_CU_Configuration] ENABLE TRIGGER [TRG_KPI_COLUMNS_Dynamic]

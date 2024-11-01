

CREATE proc [dbo].[GetSelectOption]
	@TypeCode NVARCHAR(64),
	@Id INT = NULL,
	@OperationBy uniqueidentifier
AS
BEGIN
	IF @TypeCode = N'ImportExcelTable'
	BEGIN
		SELECT
		D.Name AS [Text],
		CONVERT(VARCHAR(8), D.Id) + '|' + REPLACE(REPLACE(LTRIM(RTRIM(ISNULL([ColumnNames], ''))), '	', '|'), ' ', '')
		+ '[Template]' + ISNULL(D.Template,'') + '[/Template]'
		AS Value,
		0 AS Checked
		FROM ImportExcelTable D
		ORDER BY Priority DESC, Id ASC
		FOR XML PATH('SelectOption'), ROOT('ArrayOfSelectOption')
	END
END
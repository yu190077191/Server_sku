

CREATE PROCEDURE [dbo].[GetDictionaryProc]
	@pagesize INT = 10,
	@pageIndex INT = 1,
	@CreatedBy uniqueidentifier = NULL,
	@Keyword NVARCHAR(256) = NULL,
	@RecordStatus INT = 0,
	@Id INT = NULL
AS
BEGIN
	DECLARE @start int,@end int  
	SET @start = (@pageIndex-1)*@pagesize+1; 
	SET @end = @pageIndex*@pagesize;
	;WITH Temp AS
	(
		SELECT
		D.*
		, ROW_NUMBER() OVER(ORDER BY D.Id DESC) AS RowNumber
		FROM [Dictionary] D WITH (NOLOCK)
		WHERE (@Id IS NULL OR D.Id = @Id)
		AND D.RecordStatus = 0 
		--AND (@CreatedBy IS NULL OR D.CreatedBy = @CreatedBy)
		AND (@Keyword IS NULL 
		OR CHARINDEX(@Keyword, D.TypeCode) > 0
		OR CHARINDEX(@Keyword, D.Name) > 0
		OR CHARINDEX(@Keyword, D.ValueString) > 0
		)
	)
	, Total AS
	(
		SELECT ISNULL(COUNT(*), 0) AS TotalCount FROM Temp
	)
	SELECT
	(
		SELECT * FROM Temp WHERE RowNumber BETWEEN @start AND @end
		ORDER BY Id DESC
		FOR XML PATH('Dictionary'), Type
	)
	, TotalCount FROm Total
	FOR XML PATH('DictionaryList')
END
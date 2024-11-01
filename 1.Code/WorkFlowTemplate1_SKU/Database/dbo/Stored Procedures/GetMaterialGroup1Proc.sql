
create PROCEDURE [dbo].GetMaterialGroup1Proc
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
		FROM [MaterialGroup1] D WITH (NOLOCK)
		WHERE (@Id IS NULL OR D.Id = @Id)
		AND (@Keyword IS NULL 
		OR CHARINDEX(@Keyword, D.Name) > 0
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
		FOR XML PATH('MaterialGroup1'), Type
	)
	, TotalCount FROm Total
	FOR XML PATH('MaterialGroup1List')
END
CREATE proc GetSAPStepsByRequestId
	@Id INT,
	@pagesize INT = 10,
	@pageIndex INT = 1,
	@CreatedBy uniqueidentifier = NULL,
	@Keyword NVARCHAR(256) = NULL,
	@RecordStatus INT = 0,
	@StepName NVARCHAR(128) = NULL,
	@BU nvarchar(128) = null,
	@Plant nvarchar(128) = null
as
begin
	declare @type varchar(64) = dbo.GetRequestType(@Id)

	if isnull(@Plant,'') = ''
	begin
		SELECT distinct E.Type, T.BU, T.Plant 
		from SAPEmail E
		JOIN SAPEmailTo T ON T.SAPEmailId = E.Id
		WHERE E.Type = @type AND E.RecordStatus = 0
		FOR XML PATH('SAPStep'), ROOT('SAPStepList')
		RETURN;
	end

	;WITH Temp AS
	(
		select
		T.StepNumber
		, T.BU
		, T.Plant
		, T.Email
		, Emp.DisplayName
		, S.StepName
		, T.Id
		, E.Type
		, ROW_NUMBER() OVER(ORDER BY T.StepNumber ASC) AS RowNumber
		from SAPEmail E
		JOIN SAPEmailTo T ON T.SAPEmailId = E.Id
			AND T.BU = @BU AND T.Plant = @Plant
		JOIN SAPStep S ON S.Id = E.StepId
		JOIN Employee Emp with (nolock) on Emp.Id = T.EmployeeId
		WHERE E.Type = @type AND E.RecordStatus = 0
	)
	, Total AS
	(
		SELECT ISNULL(COUNT(*), 0) AS TotalCount FROM Temp
	)
	SELECT
	(
		SELECT * FROM Temp --WHERE RowNumber BETWEEN @start AND @end
		ORDER BY StepNumber ASC
		FOR XML PATH('SAPStep'), Type
	)
	, TotalCount FROm Total
	FOR XML PATH('SAPStepList')
end
CREATE proc [dbo].[GetSAPEmail]
	@Id INT,
	@xml nvarchar(max)=NULL,
	@CreatedBy uniqueidentifier
as
begin
	declare @type varchar(64) = dbo.GetRequestType(@Id)

	declare
	@Keyword NVARCHAR(256) = NULL,
	@RecordStatus INT = 0,
	@StepName NVARCHAR(128) = NULL,
	@BU nvarchar(128) = null,
	@Plant nvarchar(128) = null

	if isnull(@xml,'')<>''
	begin
		declare @X XML = convert(XML, @xml)
		select top 1
		@BU = T.c.value('BU[1]','nvarchar(128)'),
		@Plant = T.c.value('Plant[1]','nvarchar(128)')
		from @X.nodes('/root') as T(c)
	end

	if isnull(@Plant,'') = ''
	begin
		SELECT distinct E.Type, T.BU, T.Plant 
		from SAPEmail E
		JOIN SAPEmailTo T ON T.SAPEmailId = E.Id
		WHERE E.Type = @type AND E.RecordStatus = 0
		FOR XML PATH('SAPEmail'), ROOT('SAPEmailList')
		RETURN;
	end

	;WITH Temp AS
	(
		select
		T.StepNumber
		, ISNULL(ES.Checked,0) Checked
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
		LEFT JOIN [dbo].[SAPEmailToSetting] ES ON ES.RequestId = @Id AND ES.[SAPEmailToId] = T.Id
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
		FOR XML PATH('SAPEmail'), Type
	)
	, TotalCount FROm Total
	FOR XML PATH('SAPEmailList')
end
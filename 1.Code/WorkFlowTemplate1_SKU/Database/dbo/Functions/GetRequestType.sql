create function dbo.GetRequestType(@RequestId int)
returns varchar(64)
as
begin
	declare @type varchar(64)

	select
	top 1
	@type =
	(case when CHARINDEX('HK ', D.Value) = 1 THEN 'HK ' ELSE 'CN ' END) + 
	(case when R.Type = 'Extend Plant' THEN 'Extension-' 
		when R.Type = 'Creation' THEN 'Creation-' 
		ELSE R.Type + '-' END) + 
	(case when CHARINDEX('Wyeth', D.Value) > 0 THEN 'Wyeth' ELSE 'Non Wyeth' END)
	from RequestData D
	JOIN Request R ON R.Id = D.RequestId
	JOIN Field F on F.Name='Business' AND F.Id = D.FieldId
	WHERE D.RequestId = @RequestId
	AND D.RecordStatus = 0

	IF @type NOT IN
	(
	'CN Creation-Non Wyeth',
	'CN Creation-Wyeth',
	'CN Extension-Non Wyeth',
	'CN Extension-Wyeth',
	'HK Creation-Non Wyeth',
	'HK Creation-Wyeth',
	'HK Extension-Non Wyeth',
	'HK Extension-Wyeth'
	)
	BEGIN
		SET @type = ''
	END

	return @type;
end
CREATE proc Update_Brand_From_Excel
as
begin
	--[Temp_Brand20180731]

	if object_id('tempdb..#dict') is not null
	drop table #dict
	create table #dict(TypeCode nvarchar(64), Name nvarchar(128))

	insert #dict(TypeCode, Name)
	select
	'Corporate Brand' AS TypeCode,
	LTRIM(RTRIM(REPLACE([Corporate Brand],'  ', ' ')))
	from [Temp_Brand20180731]
	where ISNULL([Corporate Brand],'') <> ''

	insert #dict(TypeCode, Name)
	select
	'Range Brand' AS TypeCode,
	LTRIM(RTRIM(REPLACE([Range Brand],'  ', ' ')))
	from [Temp_Brand20180731]
	where ISNULL([Range Brand],'') <> ''

	insert #dict(TypeCode, Name)
	select
	'Brand Denomination' AS TypeCode,
	LTRIM(RTRIM(REPLACE([Brand Denomination],'  ', ' ')))
	from [Temp_Brand20180731]
	where ISNULL([Brand Denomination],'') <> ''

	delete D
	from Dictionary D
	LEFT JOIN #dict T ON T.TypeCode = D.TypeCode AND T.Name = D.Name
	where 
	D.TypeCode in ('Corporate Brand','Range Brand','Brand Denomination')
	AND T.TypeCode is null
	
	INSERT INTO [dbo].[Dictionary]
           ([TypeCode]
           ,[Name]
           ,[Value]
           ,[ValueString]
           ,[Priority]
           ,[Description]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedOn])
		   select
		    T.[TypeCode]
           ,T.[Name]
           ,0 [Value]
           ,NULL [ValueString]
           ,0 [Priority]
           ,NULL [Description]
           ,0 [RecordStatus]
           ,NULL [CreatedBy]
           ,NULL [CreatedTime]
           ,NULL [ModifiedBy]
           ,NULL [ModifiedOn]
		   from #dict T
		LEFT JOIN Dictionary D ON T.TypeCode = D.TypeCode AND T.Name = D.Name
		where D.TypeCode is null 
end
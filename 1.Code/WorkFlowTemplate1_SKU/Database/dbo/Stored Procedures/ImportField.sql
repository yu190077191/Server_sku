
CREATE proc [dbo].[ImportField]
as
begin

--truncate table [Field]
update Temp_ImportedData set TableName=replace(TableName, N'$','')
update Temp_ImportedData set TableName=replace(TableName, N'''','')

INSERT INTO [dbo].[Field]
           ([Type]
           ,[Category]
           ,[Name]
           ,[InputType]
		   ,Specification
           ,[Values]
           ,[TypeCode]
           ,[Colspan]
           ,[Priority]
           ,[Description]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedOn]
		   )
     select
             TableName --(<Type, nvarchar(64),>
           , A --<Category, nvarchar(128),>
           , LTRIM(RTRIM(B)) --<Name, nvarchar(128),>
           , D --<InputType, nvarchar(128),>
		   , ISNULL(G,'')
           , E --<Values, nvarchar(max),>
           , case when isnull(C,'') = 'Y' then 'mandatory' else '' end --<TypeCode, nvarchar(64),>
           , 1 --<Colspan, int,>
           , RowNumber --<Priority, float,>
           , ISNULL(F,'') --<Description, nvarchar(max),>
           , 0 --<RecordStatus, int,>
           , CreatedBy --<CreatedBy, uniqueidentifier,>
           , CreatedTime --<CreatedTime, datetime,>
           , NULL --<ModifiedBy, uniqueidentifier,>
           , NULL --<ModifiedOn, datetime,>)
		   FROM Temp_ImportedData
		   where RowNumber > 1
		   and isnull(A,'') <>''
		   and isnull(B,'') <>''
		   and B not like '%test%'
		   ORDER BY RecordId

		   ;with d as
		   (
			select
			*
			,ROW_NUMBER() OVER(partition by Type, Category, Name order by Id) AS RowNumber
			from Field
		   )
		   delete from d where RowNumber > 1

	update Field set Description ='Packaging Data.xlsx' where Name like '%Packaging%'
END
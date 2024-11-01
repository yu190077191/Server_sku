
create proc [dbo].[ImportDictionary]
as
begin

--truncate table [Field]
update Temp_ImportedData set TableName=replace(TableName, N'$','')
update Temp_ImportedData set TableName=replace(TableName, N'''','')

insert [Dictionary]
  (
   [TypeCode]
      ,[Name]
      ,[Value]
      ,[Priority]
      ,[RecordStatus]
  )
     select
         TableName [TypeCode]
      ,A [Name]
      ,0[Value]
      ,0[Priority]
      ,0[RecordStatus]
		   FROM Temp_ImportedData
		   where isnull(A,'') <>''
		   ORDER BY RecordId
END
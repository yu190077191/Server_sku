CREATE proc [dbo].[ImportSAPEmailTemplate]
as
begin

declare @Id uniqueidentifier = 'DAD7D548-6839-4CBD-BFAD-3E0356C2D059'

truncate table SAPStep
INSERT INTO [dbo].[SAPStep]
           ([Priority]
           ,[StepNumber]
           ,[StepName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedOn])
     select distinct
	 0 [Priority]
    ,0 [StepNumber]
    ,B [StepName]
    ,0 [RecordStatus]
    ,[CreatedBy]
    ,getdate() [CreatedTime]
    ,null [ModifiedBy]
    ,null [ModifiedOn]
	FROM Temp_ImportedData 
where Id = 'DAD7D548-6839-4CBD-BFAD-3E0356C2D059' and TableName<>N'邮件需要插入的内容$'
and RowNumber > 1 order by B 

update S
SET
S.Priority = CONVERT(int, T.A),
S.StepNumber = CONVERT(int, T.A)
FROM SAPStep S
JOIN Temp_ImportedData T ON T.Id = 'DAD7D548-6839-4CBD-BFAD-3E0356C2D059' and T.TableName<>N'邮件需要插入的内容$' AND T.B = S.StepName

SELECT  distinct B 
FROM Temp_ImportedData 
where Id = 'DAD7D548-6839-4CBD-BFAD-3E0356C2D059' and TableName<>N'邮件需要插入的内容$'
and RowNumber > 1
and ltrim(rtrim(B)) not in (select StepName from SAPStep)

truncate table [SAPEmail]

INSERT INTO [dbo].[SAPEmail]
           ([Type]
           ,[StepNumber]
           ,[StepId]
           ,[EmailSubject]
           ,[EmailBody]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedOn])
SELECT replace(replace(T.TableName,'''',''),'$','') as [Type]
           ,convert(int,T.A) [StepNumber]
           ,S.Id [StepId]
           ,ltrim(rtrim(C)) [EmailSubject]
           ,ltrim(rtrim(D)) [EmailBody]
           ,0 [RecordStatus]
           ,T.[CreatedBy]
           ,T.[CreatedTime]
           ,null [ModifiedBy]
           ,null [ModifiedOn]
FROM Temp_ImportedData T
JOIN SAPStep S ON S.StepName = ltrim(rtrim(T.B))
where T.Id = 'DAD7D548-6839-4CBD-BFAD-3E0356C2D059' and T.TableName<>N'邮件需要插入的内容$'
and T.RowNumber > 1


end
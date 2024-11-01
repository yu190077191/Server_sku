CREATE proc [dbo].[ImportSAPEmailTo]
 @OperationBy UNIQUEIDENTIFIER,
 @UniqueId UNIQUEIDENTIFIER,
 @TableNames NVARCHAR(512) = NULL
as
begin
	DECLARE	@StartRowNumber INT,@TableName NVARCHAR(512)

	SELECT
	TOP 1 @TableName = TableName
	, @StartRowNumber = RowNumber
	FROM Temp_ImportedData
	WHERE
	Id = @UniqueId
	AND CreatedBy = @OperationBy
	AND CHARINDEX('Senario', [A]) > 0
	AND CHARINDEX('Email', [G]) > 0

	IF @TableName IS NULL
	BEGIN
		INSERT INTO [dbo].[UploadResult]
		   ([Guid]
		   ,[TableName]
		   ,[RowNumber]
		   ,[Name]
		   ,[Message])
		SELECT 
		   @UniqueId [Guid]
		   ,''
		   ,0 [RowNumber]
		   ,N'Result' [Name]
		   , N'<b style="color:red;">bad format</b>'
		
		SELECT * FROM [UploadResult] 
		WHERE [Guid] = @UniqueId
		FOR XML PATH('UploadResult'), ROOT('UploadResultList')

		RETURN;
	END

INSERT INTO [dbo].[UploadResult]
		   ([Guid]
		   ,[TableName]
		   ,[RowNumber]
		   ,[Name]
		   ,[Message])
		SELECT distinct
		   @UniqueId [Guid]
		   ,@TableName
		   ,0 [RowNumber]
		   ,N'Step name is not correct!' [Name]
		   , N'<b style="color:red;">' +ltrim(rtrim(C))+ N'</b>'
		   FROM Temp_ImportedData T
		   where T.Id = @UniqueId and T.TableName=@TableName
			and T.RowNumber > 1
			AND ISNULL(T.C,'')<>''
			and ltrim(rtrim(C)) not in (select StepName from SAPStep)

			;with temp as
			(
			SELECT 
       REPLACE(ltrim(rtrim(T.A)),'_','-') [Type]
      ,SE2.[StepNumber]
      ,SE2.[StepId]
      ,SE2.[EmailSubject]
      ,SE2.[EmailBody]
      ,SE2.[RecordStatus]
      ,NULL [CreatedBy]
      ,GETDATE() [CreatedTime]
      ,@OperationBy [ModifiedBy]
      ,NULL [ModifiedOn]
		   , ROW_NUMBER() OVER(partition by SE2.StepId order by SE2.Id desc) AS RowNumber
		   FROM Temp_ImportedData T
		   join SAPStep S on S.StepName = ltrim(rtrim(T.C))
		   left join SAPEmail SE ON REPLACE(SE.Type,'_','-') = REPLACE(ltrim(rtrim(T.A)),'_','-') and SE.StepId = S.Id
		   JOIN SAPEmail SE2 on SE2.StepId = S.Id AND SE2.RecordStatus = 0
		   where T.Id = @UniqueId and T.TableName=@TableName
			and T.RowNumber > 1
			AND ISNULL(T.C,'')<>''
			and (SE.Type is null)
			)
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
		   select
		   [Type]
           ,[StepNumber]
           ,[StepId]
           ,[EmailSubject]
           ,[EmailBody]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedOn]
		   from temp where RowNumber=1

INSERT INTO [dbo].[UploadResult]
		   ([Guid]
		   ,[TableName]
		   ,[RowNumber]
		   ,[Name]
		   ,[Message])
		SELECT distinct
		   @UniqueId [Guid]
		   ,@TableName
		   ,0 [RowNumber]
		   ,N'Type+Step is not correct!' [Name]
		   , N'<b style="color:red;">' +ltrim(rtrim(A))+ ' Step:' + S.StepName + N'</b>'
		   FROM Temp_ImportedData T
		   join SAPStep S on S.StepName = ltrim(rtrim(T.C))
		   left join SAPEmail SE ON REPLACE(SE.Type,'_','-') = REPLACE(ltrim(rtrim(T.A)),'_','-') and SE.StepId = S.Id
		   where T.Id = @UniqueId and T.TableName=@TableName
			and T.RowNumber > 1
			AND ISNULL(T.C,'')<>''
			and (SE.Type is null)

if OBJECT_ID('tempdb..#SAPEmailTo') is not null
drop table #SAPEmailTo
create table #SAPEmailTo
(
	[SAPEmailId] [int] NOT NULL,
	[StepNumber] [int] NOT NULL,
	[BU] [nvarchar](128) NOT NULL,
	[Plant] [nvarchar](128) NOT NULL,
	[EmployeeId] [uniqueidentifier] NULL,
	[Email] [varchar](max) NULL,
	Mail varchar(512),
	TempMail varchar(512)
)

insert #SAPEmailTo
(
	[SAPEmailId],
	[StepNumber],
	[BU],
	[Plant],
	[EmployeeId],
	[Email]
)
SELECT 
SE.Id [SAPEmailId],
	convert(int,ltrim(rtrim(T.B))) [StepNumber],
	ltrim(rtrim(T.E)) [BU],
	ltrim(rtrim(T.D)) [Plant],
	NULL [EmployeeId],
	ltrim(rtrim(T.G)) [Email]
FROM Temp_ImportedData T
join SAPStep S on S.StepName = ltrim(rtrim(T.C))
join SAPEmail SE ON REPLACE(SE.Type,'_','-') = REPLACE(ltrim(rtrim(T.A)),'_','-') and SE.StepId = S.Id
where T.Id = @UniqueId and T.TableName=@TableName
and T.RowNumber > 1
AND ISNULL(T.C,'')<>''

update #SAPEmailTo set Email = REPLACE(REPLACE(Email,'<',';'),'>',';')
update #SAPEmailTo set Email = REPLACE(REPLACE(Email,'；',';'),',',';')

--update T
--SET
--T.EmployeeId = E.Id
--FROM #SAPEmailTo T
--join Employee E with (nolock) on CONVERT(varchar(512), T.Email) = E.Mail

--update T
--SET
--T.EmployeeId = E.Id
--FROM #SAPEmailTo T
--join Employee E with (nolock) on CONVERT(varchar(512), LEFT(T.Email, CHARINDEX(';', T.Email)-1)) = E.Mail
--WHERE CHARINDEX(';', T.Email) > 1

while exists(select * from #SAPEmailTo where isnull(Email,'')<>'')
begin
	update #SAPEmailTo
	set
	TempMail = SUBSTRING(Email,1,(case when charindex(';',Email) > 0 then charindex(';',Email) - 1 else len(Email) end))
	where isnull(Email,'')<>''
	update #SAPEmailTo
	set
	Email = case when charindex(';',Email) > 0 then SUBSTRING(Email,charindex(';',Email) + 1, 4000) else '' end
	where isnull(Email,'')<>''

	insert #SAPEmailTo
	(
		[SAPEmailId],
		[StepNumber],
		[BU],
		[Plant],
		[EmployeeId],
		Mail
	)
	select
	T.[SAPEmailId],
		T.[StepNumber],
		T.[BU],
		T.[Plant],
		E.Id [EmployeeId],
		E.Mail
		from #SAPEmailTo T
		join Employee E with (nolock,index([index_Employee_Email])) on 
			convert(varchar(512), T.TempMail) = E.Mail
		WHERE charindex('@',T.TempMail)>1

	update #SAPEmailTo set TempMail = ''
end

update F set F.RecordStatus =2, F.ModifiedBy=@OperationBy, F.ModifiedOn = GETDATE()
from SAPEmailTo F
		   left join #SAPEmailTo T on F.[SAPEmailId] = T.[SAPEmailId] and F.EmployeeId = T.EmployeeId
			AND F.Plant = T.Plant
			AND F.BU = T.BU
			AND ISNULL(T.Mail,'')<>''
		   where  F.RecordStatus = 0 
		   AND F.Plant IN (select distinct [Plant] from #SAPEmailTo) 
		   and T.[SAPEmailId] is null

INSERT INTO [dbo].[SAPEmailTo]
           ([SAPEmailId]
           ,[StepNumber]
           ,[BU]
           ,[Plant]
           ,[EmployeeId]
           ,[Email]
           ,[NeedAllMemebersApproval]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedOn])
		   select distinct
		    T.[SAPEmailId]
           ,T.[StepNumber]
           ,T.[BU]
           ,T.[Plant]
           ,T.[EmployeeId]
           ,T.[Mail] AS [Email]
           ,1 [NeedAllMemebersApproval]
           ,0 [RecordStatus]
           ,@OperationBy [CreatedBy]
           ,getdate() [CreatedTime]
           ,null [ModifiedBy]
           ,null [ModifiedOn]
		   from #SAPEmailTo T
		   left join SAPEmailTo F on F.[SAPEmailId] = T.[SAPEmailId] and F.EmployeeId = T.EmployeeId and F.RecordStatus = 0
			AND F.Plant = T.Plant
			AND F.BU = T.BU
		   where ISNULL(T.Mail,'')<>''
		   and F.[SAPEmailId] is null

		   UPDATE F
		   SET
		   F.RecordStatus = 2,ModifiedBy=@OperationBy, ModifiedOn = GETDATE()
		   from [SAPEmailTo] F
		   JOIN Employee E ON E.Id = F.[EmployeeId]
		   where datediff(day,E.UpdateTime,getdate()) > 10

INSERT INTO [dbo].[UploadResult]
		   ([Guid]
		   ,[TableName]
		   ,[RowNumber]
		   ,[Name]
		   ,[Message])
		SELECT 
		   @UniqueId [Guid]
		   ,''
		   ,0 [RowNumber]
		   ,N'Result' [Name]
		   , N'<b style="color:green;">成功上传' + convert(varchar(16),(select count(distinct Mail) from #SAPEmailTo where ISNULL(Mail,'')<>'')) + N'条记录！</b>'
		
		SELECT * FROM [UploadResult] 
		WHERE [Guid] = @UniqueId
		FOR XML PATH('UploadResult'), ROOT('UploadResultList')

end
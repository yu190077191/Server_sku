CREATE proc [dbo].[SetSAPEmail]
	@Id INT,
	@xml XML,
	@CreatedBy uniqueidentifier
as
begin
	declare @InstanceId int = 14
	
	insert debug (Time, Text) values(GETDATE(),CONVERT(nvarchar(max),@xml))

	declare @type varchar(64) = dbo.GetRequestType(@Id)

	if OBJECT_ID('tempdb..#t') is not null
	drop table #t
	CREATE TABLE #t(
	[RequestId] [int] NOT NULL,
	[SAPEmailToId] [int] NOT NULL,
	[Checked] [bit] NOT NULL
	)

	insert #t
	(
		RequestId,
		SAPEmailToId,
		Checked
	)
	select @Id,
	T.c.value('SAPEmailToId[1]','int'),
	T.c.value('Checked[1]','bit')
	from @xml.nodes('/root/item') as T(c)

	update F
	set
	F.Checked = T.Checked,
	F.[ModifiedBy] = @CreatedBy,
	F.[ModifiedOn] = GETDATE()
	FROM #t T
	left join SAPEmailToSetting F ON F.RequestId = T.RequestId and F.SAPEmailToId = T.SAPEmailToId and F.Checked <> T.Checked

	;with new as
	(
		select
		T.*
		FROM #t T
		left join SAPEmailToSetting F ON F.RequestId = T.RequestId and F.SAPEmailToId = T.SAPEmailToId
		where F.RequestId is null
	)
	insert SAPEmailToSetting
	(
      [RequestId]
      ,[SAPEmailToId]
      ,[Checked]
      ,[RecordStatus]
      ,[CreatedBy]
      ,[CreatedTime]
      ,[ModifiedBy]
      ,[ModifiedOn]
	  )
	  select
	  [RequestId]
      ,[SAPEmailToId]
      ,[Checked]
      ,0 [RecordStatus]
      ,@CreatedBy [CreatedBy]
      , GETDATE() [CreatedTime]
      ,null [ModifiedBy]
      ,null [ModifiedOn]
	  from new

	IF OBJECT_ID('tempdb..#WorkFlowApprover') is not null
	drop table #WorkFlowApprover
	CREATE TABLE #WorkFlowApprover(
		[WorkFlowPatternEventContainerId] [int] NOT NULL,
		[ApproverEmployeeId] [uniqueidentifier] NOT NULL,
		[Sequence] [float] NOT NULL,
		RecordStatus int
	)

	;with temp as
	(
		select
		V.[WorkFlowPatternEventContainerId]
		, ET.EmployeeId [ApproverEmployeeId]
		, ET.StepNumber [Sequence]
		, case when ES.Checked = 1 then 0 else 2 end as RecordStatus
		from SAPEmailToSetting ES
		join SAPEmailTo ET on ET.Id = ES.SAPEmailToId
		join SAPEmail SE on SE.Id = ET.SAPEmailId
		join SAPStep S on S.Id = SE.StepId
		JOIN [vwStepDetails] V on V.InstanceId = @InstanceId
			AND V.Step = REPLACE(S.StepName,'&','and')
			AND V.WorkFlowPatternName='SAPSteps'
		where ES.RequestId = @Id
	)
	insert #WorkFlowApprover
	(
		[WorkFlowPatternEventContainerId],
		[ApproverEmployeeId],
		[Sequence],
		RecordStatus
	)
	select distinct
	[WorkFlowPatternEventContainerId],
	[ApproverEmployeeId],
	[Sequence],
	RecordStatus
	from temp

declare @WorkFlowId bigint = (
	select
	top 1 Id
	from WorkFlow
	where InstanceId = @InstanceId
	AND RequestVersionId = @Id
	and RecordStatus = 0
)

declare @MaxSequence float = (SELECT MAX(WFA.Sequence) 
		FROM WorkFlow WF WITH (NOLOCK)
		JOIN WorkFlowApprover WFA WITH (NOLOCK) ON WFA.InstanceId = WF.InstanceId
			AND WFA.WorkFlowId = WF.Id
			AND WFA.RecordStatus = 0
		WHERE 
		WF.InstanceId = @InstanceId
		AND WF.RequestVersionId = @Id
		AND WF.RecordStatus = 0
)

update #WorkFlowApprover set Sequence = Sequence + @MaxSequence

update F
SET
F.RecordStatus = T.RecordStatus
, F.[ModifiedBy] = @CreatedBy
, F.[ModifiedOn] = GETDATE()
FROM #WorkFlowApprover T
join [WorkFlowApprover] F ON F.InstanceId = @InstanceId
and F.[WorkFlowId] = @WorkFlowId
and F.[WorkFlowPatternEventContainerId] = T.[WorkFlowPatternEventContainerId]
and F.[ApproverEmployeeId] = T.[ApproverEmployeeId]

;with new as
(
	select
	T.*
	FROM #WorkFlowApprover T
	left join [WorkFlowApprover] F ON F.InstanceId = @InstanceId
	and F.[WorkFlowId] = @WorkFlowId
	and F.[WorkFlowPatternEventContainerId] = T.[WorkFlowPatternEventContainerId]
	and F.[ApproverEmployeeId] = T.[ApproverEmployeeId] 
	AND F.RecordStatus = 0
	where T.RecordStatus = 0
	and F.InstanceId is null
)
INSERT INTO [dbo].[WorkFlowApprover]
           ([InstanceId]
           ,[WorkFlowId]
           ,[WorkFlowPatternEventContainerId]
           ,[ApproverEmployeeId]
           ,[Status]
           ,[Sequence]
           ,[NeedAllMemebersApproval]
           ,[MinWeight]
           ,[Weight]
           ,[ReviewResult]
           ,[ReviewTime]
           ,[ReviewComment]
           ,[IsManuallyAdded]
           ,[EscalationEventId]
           ,[EscalationTime]
           ,[ApproveType]
           ,[ApprovalXml]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedTime]
           ,[ModifiedBy]
           ,[ModifiedOn])
     SELECT
            @InstanceId
           ,@WorkFlowId
           ,WorkFlowPatternEventContainerId
           ,[ApproverEmployeeId]
           ,0 -- <Status, int,>
           ,Sequence
           ,1 -- <NeedAllMemebersApproval, bit,>
           ,0 -- <MinWeight, float,>
           ,0 -- <Weight, float,>
           ,0 -- <ReviewResult, int,>
           ,NULL -- <ReviewTime, datetime,>
           ,NULL -- <ReviewComment, nvarchar(max),>
           ,0 -- <IsManuallyAdded, bit,>
           ,0 -- <EscalationEventId, int,>
           ,NULL -- <EscalationTime, datetime,>
           ,0 -- <ApproveType, int,>
           ,NULL -- <ApprovalXml, nvarchar(max),>
           ,0 -- <RecordStatus, int,>
           ,@CreatedBy -- <CreatedBy, uniqueidentifier,>
           ,GETDATE() -- <CreatedTime, datetime,>
           ,NULL -- <ModifiedBy, uniqueidentifier,>
           ,NULL -- <ModifiedOn, datetime,>)
		   FROM new
	
	INSERT INTO [dbo].[WorkFlowHistory]
           ([InstanceId]
           ,[RequestId]
           ,[RequestVersionId]
           ,[WorkFlowApproverId]
           ,[StepName]
           ,[Title]
           ,[Name]
           ,[ReviewResult]
           ,[ReviewComment]
           ,[ReviewTime]
           ,[EmployeeId]
           ,[RecordStatus])
     VALUES
           (
		    @InstanceId--@InstanceId --<InstanceId, int,>
           ,@id--@RequestId --<RequestId, int,>
           ,@id--@RequestVersionId --<RequestVersionId, int,>
           ,0 --<WorkFlowApproverId, bigint,>
           ,N'MDC Setup SAP Approvers' --<StepName, nvarchar(512),>
           ,N'MDC' --<Title, nvarchar(512),>
           ,N'MDC' --<Name, nvarchar(512),>
           ,N'Setup SAP Work Flow' --<ReviewResult, nvarchar(512),>
           ,'' --<ReviewComment, nvarchar(max),>
           ,GETDATE() --<ReviewTime, datetime,>
           ,@CreatedBy
           ,0 --<RecordStatus, int,>
		   )

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
		from 
		SAPEmail E
		JOIN SAPEmailTo T ON T.SAPEmailId = E.Id
		JOIN [dbo].[SAPEmailToSetting] ES ON ES.RequestId = @Id AND ES.[SAPEmailToId] = T.Id
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
CREATE proc [dbo].[UpdateSKUNumber]
as
begin

if object_id('tempdb..#t') is not null
drop table #t

;with temp as(
select 
R.Type
,D.Name AS Status
,convert(nvarchar(512),'') as [Current Step]
,CONVERT(nvarchar(max),'') AS [Company Code]
,CONVERT(nvarchar(max),ISNULL(R.[CustomerNumber],'')) AS [MaterialNumber]
,CONVERT(nvarchar(max),ISNULL(R.[CustomerName],'')) AS [MaterialName]
, H.* 
, ROW_NUMBER() over(partition by H.RequestVersionId order by H.Id asc) as RowNumber
, convert(decimal(12,2), 0) as Hours
from WorkFlowHistory H
join [SKURegistration].[dbo].[Request] R on R.Id = H.RequestVersionId and R.RecordStatus = 0
	and (len(isnull(CustomerNumber,'')) < 7 or ISNUMERIC(isnull(CustomerNumber,'')) = 0)
join [SKURegistration].[dbo].Dictionary D ON D.TypeCode='RequestStatus' and D.Value = R.State
where H.InstanceId = 14
)
select * into #t from temp

;with currentWF as
(
	select
	  T.[Current Step]
	--, E.DisplayName
	,EC.Name as StepName
	, row_number() over(partition by WFA.WorkFlowId order by WFA.Sequence ASC, T.RowNumber DESC) as RowNumber
	from #t T
	join [Nestle.WorkFlow_Prod].dbo.WorkFlow WF ON WF.InstanceId = T.InstanceId
		AND WF.RequestVersionId = T.RequestVersionId
	join [Nestle.WorkFlow_Prod].dbo.WorkFlowApprover WFA on WFA.WorkFlowId = WF.Id AND WFA.Status = 0 and WFA.RecordStatus = 0
	join [Nestle.WorkFlow_Prod].dbo.WorkFlowPatternEventContainer C on C.Id = WFA.WorkFlowPatternEventContainerId
	join[Nestle.WorkFlow_Prod].dbo.EventContainer EC ON EC.Id = C.EventContainerId
	--JOIN Employee E ON E.Id = WFA.ApproverEmployeeId
	where T.Status = 'Submitted'
)
update currentWF set [Current Step] = StepName where RowNumber=1

	update T2
	set
	T2.Hours = datediff(hour, T1.ReviewTime, T2.ReviewTime) 
	from #t T1
	join #t T2 on T2.RequestVersionId = T1.RequestVersionId and T2.RowNumber = T1.RowNumber + 1

update T1
	set
	T1.[Company Code] = T2.Value
	from #t T1
	join [SKURegistration].[dbo].RequestData T2 on T2.RequestId = T1.RequestVersionId and T2.RecordStatus = 0
	JOIN [SKURegistration].[dbo].Field F ON F.Id = T2.FieldId AND F.Name like '%Sales Organization%'
update T1
	set
	T1.[MaterialName] = T2.Value
	from #t T1
	join [SKURegistration].[dbo].RequestData T2 on T2.RequestId = T1.RequestVersionId and T2.RecordStatus = 0
	JOIN [SKURegistration].[dbo].Field F ON F.Id = T2.FieldId AND F.Name like '%Material Description%'
	WHERE T1.[MaterialName] = ''
update T1
	set
	T1.[MaterialNumber] = T2.Value
	from #t T1
	join [SKURegistration].[dbo].RequestData T2 on T2.RequestId = T1.RequestVersionId and T2.RecordStatus = 0
	JOIN [SKURegistration].[dbo].Field F ON F.Id = T2.FieldId AND F.Name like '%SKU Code%'
	WHERE T1.[MaterialNumber] = ''

	update #t set ReviewComment =REPLACE( REPLACE(ReviewComment, char(10),''),char(13),'')

	;with temp as
	(
		select
		*
		,ROW_NUMBER() over(partition by RequestVersionId order by RowNumber desc) as R
		from  #t
		where StepName='MDC' --and Status = 'Approved'
	)
	UPDATE temp set ReviewComment = dbo.GetNumber(ReviewComment, 7)
	where R = 1

	;with temp as
	(
		select
		F.CustomerNumber,
		T.*
		,ROW_NUMBER() over(partition by T.RequestVersionId order by T.RowNumber desc) as R
		from  #t T
		JOIN Request F ON F.Id = T.RequestId AND (ISNULL(F.CustomerNumber,'')='' OR ISNUMERIC(F.CustomerNumber) = 0)
		where T.StepName='MDC' --and T.Status = 'Approved'
	)
	update temp set CustomerNumber = ReviewComment where R = 1 and ISNUMERIC(ReviewComment) = 1

	;with temp as
	(
		select
		dbo.[GetMaterialNumber](T.ReviewComment) AS SKU,
		F.CustomerNumber,
		T.*
		,ROW_NUMBER() over(partition by T.RequestVersionId order by T.RowNumber desc) as R
		from  #t T
		JOIN Request F ON F.Id = T.RequestId AND (ISNULL(F.CustomerNumber,'')='' OR ISNUMERIC(F.CustomerNumber) = 0)
		where T.StepName like '%input SKU Code%'
	)
	update temp set CustomerNumber = SKU where R = 1 and ISNUMERIC(SKU) = 1

	;with temp as
	(
		select
		T.*
		,ROW_NUMBER() over(partition by T.RequestVersionId order by T.RowNumber desc) as R
		from  #t T
		where T.StepName='MDC' --and T.Status = 'Approved'
	)
	select *,  ISNUMERIC(ReviewComment) from temp where R = 1 order by ISNUMERIC(ReviewComment)

end
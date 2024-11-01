
CREATE proc [dbo].[SKU_Email_Tool_API]
@typeCode nvarchar(max),
@id int,
@xml nvarchar(max) = null,
@computerName nvarchar(512),
@UserName nvarchar(512)
as
begin
	insert Debug values(GETDATE(), @typeCode)

	declare @result TABLE 
	(
		Sequence INT,
		Id NVARCHAR(MAX)
	)
	insert @result select * from dbo.GetTable(@typeCode,'|')
	--Email=" + to.Text + "|" + comboBoxStep.Text + "|" + subject);
	if charindex('Email_',@typeCode)=1
	begin
		declare @StepName nvarchar(256) = (select top 1 Id from @result where Sequence = 2)
		declare @a int = charindex('.',@StepName)
		if @a > 0 and ISNUMERIC(left(@StepName, @a - 1)) = 1
		begin
			select @StepName = LTRIM(SUBSTRING(@StepName, @a + 1, 256))
		end

		declare @EmployeeEmails nvarchar(max) = REPLACE((select top 1 Id from @result where Sequence = 1), 'Email_','')

		EXEC [AddStep]
			@InstanceId = 14,
			@Id = @id,
			@StepName = @StepName,
			@EmployeeEmails = @EmployeeEmails,
			@UserName = @UserName

		INSERT INTO [Nestle.WorkFlow_Test].[dbo].[WorkFlowHistory]
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
		    14--@InstanceId --<InstanceId, int,>
           ,@id--@RequestId --<RequestId, int,>
           ,@id--@RequestVersionId --<RequestVersionId, int,>
           ,0 --<WorkFlowApproverId, bigint,>
           ,N'MDC Notify SAP Approvers' --<StepName, nvarchar(512),>
           ,@EmployeeEmails --<Title, nvarchar(512),>
           ,@EmployeeEmails --<Name, nvarchar(512),>
           ,N'Send Email' --<ReviewResult, nvarchar(512),>
           ,'' --<ReviewComment, nvarchar(max),>
           ,GETDATE() --<ReviewTime, datetime,>
           ,(select Id from Employee where AccountName=REPLACE(@UserName,'NESTLE\','')) --<EmployeeId, uniqueidentifier,>
           ,0 --<RecordStatus, int,>
		   )
	end

	declare @CC varchar(512) = (select top 1
	E.Mail
	from Request R
	JOIN Employee E with (nolock) ON E.Id = R.CreatedBy
	where R.Id = @id
	)

	if CHARINDEX('_Test', DB_NAME()) > 1
	begin
		select @CC = ''
	end

	select
	N'1. Create SKU code in SAP|2. LDO approve creation request in SAP|3. Assign BPC and notify Finance to Maintain BPC |4. Maintain BPC|5. Trigger workflow for SDR 1st circle in SAP|6. SDR 1st circle released by related function|7. LDO release SDR 1st circle|8. BOM |9. Master Recipe|10. PV|11. Calculate Standard Cost|12. Trigger workflow for SDR 2nd circle in manufacturing plant |13. LDO release SDR 2nd circle for manufacturing plant|14. Calculate DC cost|15. Trigger workflow 2nd circle in distribution center|16. Workflow released by DC Finance|17. LDO release workflow 2nd circle in distribution center|18. Change MDR status to Z3|19. LDO approve change request' Step,
	N'' AS Subject,
	'' AS [To],
	@CC AS [Cc],
	N'{url}' AS Body
	for XML PATH('APIInfo'), ROOT('ArrayOfAPIInfo')
end
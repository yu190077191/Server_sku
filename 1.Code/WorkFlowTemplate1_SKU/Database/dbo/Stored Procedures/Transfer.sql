create PROC [dbo].[Transfer]
	@NewEmail VARCHAR(512),
	@OldEmail VARCHAR(512),
	@Run bit = 0
AS
BEGIN
	DECLARE @NewEmployeeId UNIQUEIDENTIFIER, @OldEmployeeId UNIQUEIDENTIFIER
	SELECT @NewEmployeeId = (SELECT Id FROM Employee WITH (NOLOCK) WHERE mail = @NewEmail or AccountName=@NewEmail or DisplayName=@NewEmail)
	SELECT @OldEmployeeId = (SELECT Id FROM Employee WITH (NOLOCK) WHERE mail = @OldEmail or AccountName=@OldEmail or DisplayName=@OldEmail)
	if @NewEmployeeId is null or @OldEmployeeId is null
	begin
		select 'wrong emails!'
		return;
	end

	declare @time datetime = getdate()

	

	select 'Request' as Type, * from Request where CreatedBy=@OldEmployeeId
	select 'WF' as Type, wf.* from [Nestle.WorkFlow_Prod].dbo.WorkFlowApprover wfa join [Nestle.WorkFlow_Prod].dbo.WorkFlow wf on wf.Id = wfa.WorkFlowId 
	where ApproverEmployeeId=@OldEmployeeId

	if @Run = 0
	return;

	update Request set CreatedBy=@NewEmployeeId  where CreatedBy=@OldEmployeeId
	update RequestData set CreatedBy=@NewEmployeeId  where CreatedBy=@OldEmployeeId
	update Attachment set CreatedBy=@NewEmployeeId  where CreatedBy=@OldEmployeeId

	UPDATE [Nestle.WorkFlow_Prod].dbo.WorkFlow set Originator =@NewEmployeeId, CreatedBy =@NewEmployeeId, ModifiedOn = @time
	where Originator = @OldEmployeeId

	UPDATE [Nestle.WorkFlow_Prod].dbo.DepartmentRole set EmployeeId=@NewEmployeeId, ModifiedOn = @time where EmployeeId = @OldEmployeeId and RecordStatus = 0
	UPDATE [Nestle.WorkFlow_Prod].dbo.WorkFlowApprover set ApproverEmployeeId=@NewEmployeeId, ModifiedOn = @time 		
	where ApproverEmployeeId = @OldEmployeeId and Status = 0 and RecordStatus = 0

	select 'Request' as Type, * from Request where CreatedBy=@NewEmployeeId
	select 'WF' as Type, wf.* from [Nestle.WorkFlow_Prod].dbo.WorkFlowApprover wfa join [Nestle.WorkFlow_Prod].dbo.WorkFlow wf on wf.Id = wfa.WorkFlowId 
	where ApproverEmployeeId=@NewEmployeeId
END
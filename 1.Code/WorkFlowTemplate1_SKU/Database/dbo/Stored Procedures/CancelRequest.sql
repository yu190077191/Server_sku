CREATE proc [dbo].[CancelRequest]
@Id int
as
begin

update Request set State = 7 where Id = @Id

update WFA
set WFA.Status = 1, ModifiedOn = GETDATE()
FROM WorkFlow WF
	JOIN WorkFlowApprover WFA WITH (NOLOCK) ON
		WFA.WorkFlowId = WF.Id AND 
		WFA.RecordStatus = 0
		AND WF.RecordStatus = 0
		AND WFA.Status = 0
	WHERE WF.InstanceId = 14
	AND WF.RequestVersionId = @Id
	AND WF.StatusId = 1
end
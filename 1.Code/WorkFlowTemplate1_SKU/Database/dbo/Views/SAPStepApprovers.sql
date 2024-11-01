create view SAPStepApprovers
as
		select
		T.StepNumber
		, T.BU
		, T.Plant
		, T.Email
		, Emp.DisplayName
		, S.StepName
		, T.Id
		, E.Type
		, ROW_NUMBER() OVER(ORDER BY T.StepNumber ASC) AS RowNumber
		from SAPEmail E
		JOIN SAPEmailTo T ON T.SAPEmailId = E.Id
		JOIN SAPStep S ON S.Id = E.StepId
		JOIN Employee Emp with (nolock) on Emp.Id = T.EmployeeId
		WHERE E.RecordStatus = 0
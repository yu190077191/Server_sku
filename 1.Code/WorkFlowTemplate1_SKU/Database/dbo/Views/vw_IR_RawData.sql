
CREATE view [dbo].[vw_IR_RawData]
as
	SELECT
	D.Name as Status
	, W.Id AS RequestID
	, W.Type
	, D1.Value BU
	, D2.Value [Renovation/ Innovation/ Normal]
	, ISNULL(D3.Value, W.CustomerNumber) SKU
	, W.CustomerName SKUName
	, D4.Value [NPDI Number]
	, E.DisplayName AS [PreparedBy]
	, W.CreatedTime
	, 'http://cnbeii0002.nestle.com/SKURegistration/WF/Preview?id=' + CONVERT(varchar(8), W.Id) as URL
	FROM Request W WITH (NOLOCK)
	LEFT JOIN Employee E WITH (NOLOCK,INDEX([Index_EmployeeId])) ON E.Id = W.CreatedBy
	JOIN Dictionary D ON D.TypeCode='RequestStatus' AND D.Value= W.State and D.Name = 'Approved'
	LEFT JOIN [RequestData] D1 on D1.RequestId = W.Id and D1.RecordStatus = 0 and D1.FieldId in (select Id from Field where Name = 'Business')
	LEFT JOIN [RequestData] D2 on D2.RequestId = W.Id and D2.RecordStatus = 0 and D2.FieldId in (select Id from Field where Name = 'Renovation/ Innovation/ Normal')
	LEFT JOIN [RequestData] D3 on D3.RequestId = W.Id and D3.RecordStatus = 0 and D3.FieldId in (select Id from Field where Name = 'SKU Code')
	JOIN [RequestData] D4 on D4.RequestId = W.Id and D4.RecordStatus = 0 and D4.FieldId in (select Id from Field where Name = 'NPDI Number')
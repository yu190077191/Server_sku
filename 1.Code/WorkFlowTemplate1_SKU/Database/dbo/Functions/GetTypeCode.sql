CREATE function [dbo].[GetTypeCode](@Id INT)
RETURNS VARCHAR(128)
as
begin
	DECLARE @TypeCode VARCHAR(128), @Type VARCHAR(128), @PPP BIT, @NHW BIT
	SET @TypeCode = ''

	DECLARE @Business nvarchar(max) = (
		select top 1 D.Value 
		from Field F
		JOIN RequestData D ON D.FieldId = F.Id AND D.RequestId = @Id AND D.RecordStatus = 0
		where 
		F.Name = 'Business'
	)

	SET @NHW = 0
	IF (
		CHARINDEX('Beverages', @Business) > 0 OR
		CHARINDEX('Chilled', @Business) > 0 OR
		CHARINDEX('Coffee', @Business) > 0 OR
		CHARINDEX('Confectionary', @Business) > 0 OR
		CHARINDEX('Dairy', @Business) > 0 OR
		CHARINDEX('Food', @Business) > 0 OR
		CHARINDEX('Grocery', @Business) > 0 OR
		CHARINDEX('Ice Cream', @Business) > 0 OR
		CHARINDEX('Infant Nutrition', @Business) > 0 OR
		CHARINDEX('NHS', @Business) > 0 OR
		CHARINDEX('NP', @Business) > 0 OR
		CHARINDEX('Water', @Business) > 0 OR
		CHARINDEX('CPW', @Business) > 0
		OR CHARINDEX('NIN', @Business) > 0
	)
	BEGIN
		SET @NHW = 1
	END

	SELECT TOP 1
	@Type = R.Type
	, @PPP = CASE WHEN D1.Id is not null AND (D4.Id is not null or D2.Id is not null) THEN 1 ELSE 0 END
	FROM Request R
	LEFT JOIN RequestData D1 ON D1.RequestId = R.Id AND CHARINDEX('Mainland', D1.Value) > 0 AND D1.RecordStatus = 0
	LEFT JOIN RequestData D2 ON D2.RequestId = R.Id AND D2.Value IN ('Renovation','Innovation') AND D2.RecordStatus = 0
	LEFT JOIN RequestData D4 ON D4.RequestId = R.Id AND D4.Value IN ('P Premium') AND D4.RecordStatus = 0
	WHERE R.Id = @Id

	IF @Type = 'Creation'
	BEGIN
		IF @PPP = 1 AND @NHW =1
		BEGIN
			SET @TypeCode = 'default'
		END
		ELSE IF @PPP = 0 AND @NHW =0
		BEGIN
			SET @TypeCode = 'No_PPP_NHW'
		END
		ELSE IF @PPP = 0 AND @NHW =1
		BEGIN
			SET @TypeCode = 'No_PPP'
		END
		ELSE IF @PPP = 1 AND @NHW =0
		BEGIN
			SET @TypeCode = 'No_NHW'
		END
	END
	ELSE IF @Type = 'Extend Plant'
	BEGIN
		IF @PPP = 1 AND @NHW =1
		BEGIN
			SET @TypeCode = 'ExtendToPlant'
		END
		ELSE IF @PPP = 0 AND @NHW =0
		BEGIN
			SET @TypeCode = 'ExtendToPlant_No_PPP_NHW'
		END
		ELSE IF @PPP = 0 AND @NHW =1
		BEGIN
			SET @TypeCode = 'ExtendToPlant_No_PPP'
		END
		ELSE IF @PPP = 1 AND @NHW =0
		BEGIN
			SET @TypeCode = 'ExtendToPlant_No_NHW'
		END
	END
	ELSE IF @Type = 'Extend DC'
	BEGIN
		SET @TypeCode = 'ExtendToDC'
	END
	ELSE IF @Type = 'Update General'
	BEGIN
		SET @TypeCode = 'General'
	END
	ELSE IF @Type = 'Update Sales'
	BEGIN
		SET @TypeCode = 'General'
	END
	ELSE IF @Type = 'Update GTIN'
	BEGIN
		SET @TypeCode = 'LDO'
	END
	ELSE IF @Type = 'Update NHW'
	BEGIN
		SET @TypeCode = 'NHW'
	END
	ELSE IF @Type = 'Update Packaging'
	BEGIN
		SET @TypeCode = 'AG'
	END
	ELSE IF @Type = 'Update Logistics'
	BEGIN
		SET @TypeCode = 'Logistics'
	END
	ELSE IF @Type = 'Update QA'
	BEGIN
		SET @TypeCode = 'QA'
	END
	ELSE IF @Type = 'Update F&C'
	BEGIN
		SET @TypeCode = 'FC'
	END
	
	RETURN @TypeCode;
END
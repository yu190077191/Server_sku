CREATE function [dbo].[GetMaterialNumber](@str nvarchar(max))
returns varchar(32)
as
begin
	select @str = REPLACE(@str,'	','')
	select @str = REPLACE(@str,' ','')
	declare @number varchar(32) = ''
	declare @i int = 1
	declare @len int = len(@str)
	declare @a char(1)
	declare @isValid bit = 0
	while @i <= @len
	begin
		select @a = SUBSTRING(@str,@i,1)
		if ISNUMERIC(@a)=1 AND @a NOT IN('-', '.', ',')
		BEGIN
			select @number = @number + @a
			set @isValid = 1
		END
		ELSE if @isValid = 1
		BEGIN
			return @number;
		END

		set @i =  @i + 1
	end


	return @number;
end
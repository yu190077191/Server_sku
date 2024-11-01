CREATE function [dbo].[GetNumber](@str nvarchar(max), @length int)
returns varchar(32)
as
begin
	select @str = REPLACE(@str,'	','')
	select @str = REPLACE(@str,' ','')
	declare @number varchar(32) = ''
	declare @i int = 1
	declare @len int = len(@str)
	declare @a char(1)
	while @i <= @len
	begin
		select @a = SUBSTRING(@str,@i,1)
		if ISNUMERIC(@a)=1 AND @a NOT IN('-', '.', ',')
		BEGIN
			select @number = @number + @a
		END
		ELSE
		BEGIN
			if len(@number) >= @length
			begin
				return @number;
				set @i =  @len + 1
			end
			else
			begin
				set @number = ''
			end
		END

		set @i =  @i + 1
	end


	return @number;
end
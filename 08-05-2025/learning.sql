
use pubs;

-- Stored Procedure to filter the products based on the type of CPU
go
CREATE OR ALTER PROC proc_FilterProducts(@pcpu varchar(20), @pcount int out)
  AS
  BEGIN
      SET @pcount = (SELECT count(*) FROM products WHERE 
	  TRY_CAST(json_value(details,'$.specs.CPU') as nvarchar(20)) =@pcpu)
  END;

BEGIN
	DECLARE @cnt int 
	exec proc_FilterProducts 'i5', @cnt out
	print(CONCAT('Total number of CPU with i7 spec: ', @cnt))
END;


-- Bulk Insert from a CSV file
CREATE TABLE People ( id INT PRIMARY KEY, name NVARCHAR(20), age INT );

go
CREATE OR ALTER PROC proc_BulkInsertFromCSV( @filepath NVARCHAR(500))
AS 
BEGIN 
	DECLARE @insertQuery NVARCHAR(MAX)
	SET @insertQuery = 'BULK INSERT People FROM ''' + @filepath + '''
							WITH ( 
								FIRSTROW = 2,
								FIELDTERMINATOR = '','',
								ROWTERMINATOR = ''\n''
							)'
	exec sp_executesql @insertQuery
END;

exec proc_BulkInsertFromCSV 'C:\Users\VC\Downloads\Data(in).csv'

SELECT * from People;


-- Logging the status of Bulk Insert using EXCEPTION HANDLING - TRY CATCH
CREATE TABLE BulkInsertLog
(LogId int identity(1,1) primary key,
FilePath nvarchar(1000),
status nvarchar(50) constraint chk_status Check(status in('Success','Failed')),
Message nvarchar(1000),
InsertedOn DateTime default GetDate())

go
CREATE OR ALTER PROC proc_BulkInsertWithLog(@filepath nvarchar(500))
AS
BEGIN
  BEGIN TRY
	   DECLARE @insertQuery nvarchar(max)

	   SET @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
	   with(
	   FIRSTROW =2,
	   FIELDTERMINATOR='','',
	   ROWTERMINATOR = ''\n'')'

	   exec sp_executesql @insertQuery

	   INSERT into BulkInsertLog(filepath,status,message)
	   VALUES(@filepath,'Success','Bulk insert completed')
  END TRY
  BEGIN CATCH
		INSERT into BulkInsertLog(filepath,status,message)
		VALUES(@filepath,'Failed',Error_Message())
  END CATCH
END

exec proc_BulkInsertWithLog 'C:\Users\VC\Downloads\Data(in).csv'

select * from BulkInsertLog


-- Common Table Expressions ( CTE )
WITH cteAuthors
AS
(SELECT au_id, CONCAT(au_fname,' ',au_lname) author_name FROM authors)

select * from cteAuthors


-- Pagination
DECLARE @page int = 1, @pageSize int =10;
WITH PaginatedBooks AS
	( 
		SELECT title_id, title, price, ROW_NUMBER() over ( ORDER BY price DESC ) as RowNum from titles
	)

SELECT * FROM PaginatedBooks WHERE RowNum BETWEEN (@pageNum-1)*(@pageSize+1) AND @pageNum*@pageSize



-- Create a Stored Procedure that takes the pageNumber and pageSize as params and print the Books
go
CREATE OR ALTER PROC proc_PrintBooksUsingPagination (@pageNum int, @pageSize int)
AS 
BEGIN 
	WITH PaginatedBooks AS
		( 
			SELECT title_id, title, price, ROW_NUMBER() over ( ORDER BY price DESC ) as RowNum from titles
		)

	SELECT * FROM PaginatedBooks WHERE RowNum BETWEEN (@pageNum-1)*(@pageSize+1) AND @pageNum*@pageSize

	-- Using offset
	SELECT title_id, title, price FROM titles ORDER BY price DESC offset @pageNum rows FETCH next @pageSize rows only
END;

exec proc_PrintBooksUsingPagination 2, 5


-- Functions ( Have to return a value whereas Stored Procedure need not )
go
CREATE FUNCTION fn_CalculateTax( @basePrice float, @tax float ) RETURNS float 
AS 
BEGIN 
	return (@basePrice + (@basePrice * @tax /100 ))
END;

go
SELECT dbo.fn_CalculateTax( 1000, 10 ) as Tax

SELECT title, price, dbo.fn_CalculateTax( price, 20 ) as Tax FROM titles


-- Return table function
go 
CREATE FUNCTION fn_TableSample(@minprice float)
  returns table
  as
    return SELECT title,price FROM titles WHERE price>= @minprice

go

SELECT * FROM dbo.fn_tableSample(10)

-- Older and slower method, but supports more logic
go 
CREATE FUNCTION fn_TableSampleOldMethod(@minprice float)
  returns @Result TABLE(BookName nvarchar(100), price float)
  as
  BEGIN
    INSERT into @Result SELECT title, price FROM titles WHERE price >= @minprice
  RETURN 
END;

go

SELECT * FROM dbo.fn_TableSampleOldMethod(10)
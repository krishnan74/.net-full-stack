-- First Stored Procedure
go
create procedure proc_First
as
begin print 'Hello World!'
end;

exec proc_First


-- Creating product table 
create table Products ( id int identity(1,1) constraint pk_productId primary key,
name nvarchar(100) not null, 
details nvarchar(max))

-- Stored Procedure to insert product with a JSON data
go
create proc proc_InsertProduct(@pname nvarchar(100), @pdetails nvarchar(max))
as
begin
	insert into Products(name, details) values (@pname, @pdetails)
end;

exec proc_InsertProduct 'Laptop', '{"brand": "Dell", "specs": {"RAM": "16GB", "CPU": "i5"} }'

-- Query the json data
select JSON_QUERY(details, '$.specs') Product_Specification from Products;

-- Stored Procedure to update product
go
create proc proc_UpdateProduct(@pid int, @newvalue nvarchar(20))
as
begin
	update Products set details = JSON_MODIFY(details, '$.spec.RAM', @newvalue) where id = @pid
end;

exec proc_UpdateProduct 1, '36GB'

select id, name, JSON_VALUE(details, '$.brand') BrandName from Products;


-- Bulk inserting from JSON

-- Create the Posts table
create table Posts (
    id int primary key,
    title nvarchar(100),
    user_id int,
    body nvarchar(max)
);

-- Create stored procedure
go
create proc proc_BulkInsertPosts(@jsondata nvarchar(max))
as
begin
    
    insert into Posts(user_id, id, title, body)
    select
        userId,
        id,
        title,
        body
    from openjson(@jsondata)
    with (
        userId int '$.userId',
        id int '$.id',
        title nvarchar(100) '$.title',
        body nvarchar(max) '$.body'
    );
end;
go

-- Execute the procedure with JSON input
exec proc_BulkInsertPosts N'
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }
]';

-- View inserted data
select * from Posts;


-- Converting the data type to check for comparison
select * from Products where try_cast(JSON_VALUE(details, '$.spec.CPU') as nvarchar(20)) = 'i7'

-- Create a procedure that brings post by taking the user_id as parameter
go
create proc proc_GetPostByUserId(@user_id int)
as 
begin 
	select * from Posts where user_id = @user_id
end;

go
exec proc_GetPostByUserId 1
select * from Orders;
select * from Customers;
select * from Employees;
select * from Products;
select * from Categories;
select * from Suppliers;
select * from "Order Details";

-- 1) List all orders with the customer name and the employee who handled the order.

SELECT ord.*, cus.ContactName as CustomerName,
CONCAT(emp.FirstName, ' ', emp.LastName) as EmployeeName FROM Orders ord
JOIN Customers cus on ord.CustomerID = cus.CustomerID
JOIN Employees emp on ord.EmployeeID = emp.EmployeeID;


-- 2) Get a list of products along with their category and supplier name.

SELECT prd.*, ctg.CategoryName, sup.ContactName as SupplierName, sup.CompanyName as SupplierCompanyName
FROM Products prd
JOIN Categories ctg on prd.CategoryID = ctg.CategoryID
JOIN Suppliers sup on prd.SupplierID = sup.SupplierID;


-- 3) Show all orders and the products included in each order with quantity and unit price.

SELECT ord.*, prd.ProductName, orddet.UnitPrice, orddet.Quantity
 FROM "Order Details" orddet
JOIN Orders ord on orddet.OrderID = ord.OrderID
JOIN Products prd on prd.ProductID = orddet.ProductID;


-- 4) List employees who report to other employees (manager-subordinate relationship).

SELECT DISTINCT CONCAT(emp.FirstName, ' ', emp.LastName) as EmployeeName, 
CONCAT(boss.FirstName, ' ', boss.LastName) as BossName  FROM Employees emp
JOIN Employees boss on emp.ReportsTo = boss.EmployeeID;


-- 5) Display each customer and their total order count.

SELECT cus.ContactName as CustomerName, COUNT(*) as OrderCount FROM Orders ord
JOIN Customers cus ON ord.CustomerID = cus.CustomerID 
GROUP BY cus.ContactName;


-- 6) Find the average unit price of products per category.

SELECT ctg.CategoryName, AVG(prd.UnitPrice) as AverageUnitPrice FROM Products prd
JOIN Categories ctg on prd.CategoryID = ctg.CategoryID
GROUP BY ctg.CategoryName;


-- 7) List customers where the contact title starts with 'Owner'.
SELECT ContactName as CustomerName, ContactTitle FROM Customers WHERE ContactTitle LIKE 'Owner%';

-- 8) Show the top 5 most expensive products.
SELECT TOP 5 * FROM Products ORDER BY UnitPrice DESC;

-- 9) Return the total sales amount (quantity × unit price) per order.
SELECT OrderID, SUM(Quantity * UnitPrice) as TotalSalesAmount FROM "Order Details"
GROUP BY OrderID;

-- 10) Create a stored procedure that returns all orders for a given customer ID.
go
CREATE OR ALTER PROC proc_OrdersByCustomerID(@customerID nvarchar(10))
AS
	BEGIN
		SELECT * FROM Orders WHERE CustomerID = @customerID;
	END;


exec proc_OrdersByCustomerID 'ALFKI'


-- 11) Write a stored procedure that inserts a new product.
go
CREATE OR ALTER PROC proc_InsertNewProduct(
	@productName nvarchar(100), @supplierID int, @categoryID int,
	@quantityPerUnit nvarchar(50), @unitPrice float, @unitsInStock int, @unitsOnOrder int,
	@reorderLevel int, @discontinued int)
AS
	BEGIN
		INSERT INTO Products VALUES
		(@productName, @supplierID, @categoryID, @quantityPerUnit,
		@unitPrice, @unitsInStock, @unitsOnOrder, @reorderLevel, @discontinued);
	END;

exec proc_InsertNewProduct 'Monster White', 1, 1, '1 Bottle', 180.00, 40, 20, 8, 0;
SELECT * FROM Products WHERE ProductName = 'Monster White';


-- 12) Create a stored procedure that returns total sales per employee.
go 
CREATE PROC proc_totalSalesPerEmployee
AS
	BEGIN 
		SELECT CONCAT(emp.FirstName, ' ', emp.LastName) as EmployeeName , 
			SUM(orddet.Quantity * orddet.UnitPrice) as TotalSalesAmount 
		FROM "Order Details" orddet
		JOIN Orders ord on orddet.OrderID = ord.OrderID
		JOIN Employees emp on emp.EmployeeID = ord.EmployeeID
		GROUP BY CONCAT(emp.FirstName, ' ', emp.LastName);
END;

exec proc_totalSalesPerEmployee


-- 13) Use a CTE to rank products by unit price within each category.
WITH cteRankProducts
	AS 
	( SELECT ProductName, UnitPrice, ctg.CategoryName,
	ROW_NUMBER() over ( PARTITION BY prd.CategoryID ORDER BY UnitPrice DESC ) as PriceRank FROM Products prd
	JOIN Categories ctg on ctg.CategoryID = prd.CategoryID
	)

SELECT * from cteRankProducts;


-- 14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.
WITH cteTotalRevenuePerProduct
	AS 
		( 
		SELECT prd.ProductName, SUM(orddet.Quantity * orddet.UnitPrice) as TotalRevenue FROM "Order Details" orddet
		JOIN Products prd on orddet.ProductID = prd.ProductID
		GROUP BY prd.ProductName
		HAVING SUM(orddet.Quantity * orddet.UnitPrice) > 10000
		) 
		
SELECT * FROM cteTotalRevenuePerProduct;


-- 15) Use a CTE with recursion to display employee hierarchy.
WITH OrgChart AS (
    -- Anchor member: start with the top-level manager
    SELECT 
        EmployeeID,
        FirstName,
        LastName,
        ReportsTo,
        0 AS Level
    FROM Employees
    WHERE ReportsTo IS NULL

    UNION ALL

    -- Recursive member: get employees who report to previous level
    SELECT 
        E.EmployeeID,
        E.FirstName,
        E.LastName,
        E.ReportsTo,
        OC.Level + 1
    FROM Employees E
    JOIN OrgChart OC ON E.ReportsTo = OC.EmployeeID
)

SELECT 
    EmployeeID,
    CONCAT(FirstName, ' ', LastName) AS EmployeeName,
    ReportsTo,
    Level
FROM OrgChart
ORDER BY Level, EmployeeName;
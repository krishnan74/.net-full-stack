-- https://neon.tech/postgresql/postgresql-getting-started/postgresql-sample-database
-- -----------------------------------------------------------------
SELECT * From Film;
SELECT * FROM Customer;
SELECT * FROM Rental;
SELECT * FROM Payment;

-- SELECT Queries

-- 1) List all films with their length and rental rate, sorted by length descending.
SELECT title, length, rental_rate FROM Film;

-- 2) Find the top 5 customers who have rented the most films.

SELECT CONCAT(first_name, ' ', last_name) as Customer_Name, Count(*) as Films_Rented FROM Customer c
JOIN Rental r ON c.customer_id = r.customer_id GROUP BY Customer_Name ORDER BY Films_Rented DESC LIMIT 5; 

-- 3) Display all films that have never been rented.

SELECT * FROM Inventory inv LEFT JOIN Film f on inv.film_id = f.film_id
WHERE inv.inventory_id NOT IN (SELECT inventory_id FROM Rental);
--or 
SELECT * FROM Film f JOIN Inventory i on f.film_id = i.film_id
LEFT JOIN Rental r on i.inventory_id = r.inventory_id WHERE r.rental_id IS NULL;


-- JOIN Queries
-- 4) List all actors who appeared in the film ‘Academy Dinosaur’.
SELECT DISTINCT CONCAT(first_name, ' ', last_name) FROM Actor act
JOIN Film_Actor fa ON act.actor_id = fa.actor_id
JOIN Film f on fa.film_id = f.film_id WHERE f.title = 'Academy Dinosaur'; 

-- 5) List each customer along with the total number of rentals they made and the total amount paid.
SELECT CONCAT( first_name, ' ', last_name ) as Customer_Name, COUNT(DISTINCT re.rental_id) as No_of_Rental,
SUM(pay.amount) as Total_Amount_Paid FROM Customer cus
LEFT JOIN Rental re ON cus.customer_id = re.customer_id
LEFT JOIN Payment pay ON pay.customer_id = cus.customer_id 
GROUP BY cus.customer_id, Customer_Name
ORDER BY Customer_Name;

-- CTE-Based Queries

-- 6) Using a CTE, show the top 3 rented movies by number of rentals.
WITH cte_rental_counts AS (
    SELECT f.title, COUNT(r.rental_id) AS rental_count
    FROM film f
    JOIN inventory i ON f.film_id = i.film_id
    JOIN rental r ON i.inventory_id = r.inventory_id
    GROUP BY f.title
)
SELECT title, rental_count
FROM cte_rental_counts
ORDER BY rental_count DESC
LIMIT 3;

-- 7) Find customers who have rented more than the average number of films.
-- Use a CTE to compute the average rentals per customer, then filter.
WITH cte_AvgRentalsPerCustomer AS
	(
	SELECT AVG(Rental_Count) FROM 
		( 
			SELECT customer_id, COUNT(*) AS Rental_Count FROM rental 
			GROUP BY customer_id
		) AS Avg_Rental_Count
	)
	
SELECT customer_id, COUNT(*) FROM rental
GROUP BY customer_id
HAVING COUNT(*) > SELECT Avg_Rental_Count FROM cte_AvgRentalsPerCustomer);

--  Function Questions
-- 8) Write a function that returns the total number of rentals for a given customer ID.
-- Function: get_total_rentals(customer_id INT)
CREATE OR REPLACE FUNCTION get_total_rentals(cust_id INT) 
RETURNS INT AS 
$$ 
    DECLARE total_rentals INT; 
    BEGIN 
        SELECT COUNT(*) INTO total_rentals FROM rental WHERE customer_id = cust_id; 
        RETURN total_rentals; 
    END; 
$$ 
LANGUAGE plpgsql

-- Stored Procedure Questions
-- 9) Write a stored procedure that updates the rental rate of a film by film ID and new rate.
-- Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)
CREATE OR REPLACE PROCEDURE proc_Update_Rental_Rate(p_film_id INT, new_rate NUMERIC)
LANGUAGE plpgsql
AS 
$$ 
BEGIN 
	UPDATE Film SET rental_rate = new_rate WHERE film_id = p_film_id;
END;
$$;

CALL proc_Update_Rental_Rate(1, 30);

-- 10) Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
-- Procedure: get_overdue_rentals() that selects relevant columns.
CREATE OR REPLACE PROCEDURE proc_Get_OverDue_Rentals()
LANGUAGE plpgsql
AS 
$$
BEGIN 
	SELECT * FROM Rental WHERE return_date IS NULL AND rental_date > CURRENT_DATE + interval '7days';
END;
$$;

CALL proc_Get_OverDue_Rentals(); 
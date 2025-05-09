SELECT * FROM Film;
SELECT * FROM CUSTOMER;
SELECT * FROM RENTAL;
SELECT * FROM PAYMENT;
SELECT * FROM STORE;
SELECT * FROM INVENTORY;

-- Cursor-Based Questions (5)
-- 1) Write a cursor that loops through all films and prints titles longer than 120 minutes.
DO
$$
DECLARE 
	p_title VARCHAR(255); p_length INT;
	film_cursor CURSOR FOR 
	SELECT title, length FROM Film;

BEGIN
	OPEN film_cursor;
	LOOP
		FETCH NEXT FROM film_cursor INTO p_title, p_length;
		EXIT WHEN NOT FOUND;
		
		IF p_length > 120 THEN
			RAISE NOTICE 'Film Name: %, Length: % minutes', p_title, p_length;
		END IF;
	END LOOP;
	CLOSE film_cursor;
END;
$$

-- 2) Create a cursor that iterates through all customers and counts how many rentals each made.
DO
$$
DECLARE 
	v_customer_id INT;
	v_customer_name VARCHAR(90);
	v_rental_count INT;
	customer_cursor CURSOR FOR 
	SELECT customer_id, CONCAT(first_name, last_name) FROM Customer;

BEGIN
	OPEN customer_cursor;
	LOOP
		FETCH NEXT FROM customer_cursor INTO v_customer_id, v_customer_name;
		EXIT WHEN NOT FOUND;
		
		SELECT COUNT(*) INTO v_rental_count FROM Rental WHERE customer_id = v_customer_id;
		RAISE NOTICE 'Customer Name: %, Rental Count: %', v_customer_name, v_rental_count;
	END LOOP;
	CLOSE customer_cursor;
END;
$$

-- 3) Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.
DO
$$
DECLARE 
	v_film_id INT;
	v_film_name VARCHAR(90);
	v_rental_count INT;
	film_cursor CURSOR FOR 
	SELECT film_id, title FROM Film;

BEGIN
	OPEN film_cursor;
	LOOP
		FETCH NEXT FROM film_cursor INTO v_film_id, v_film_name;
		EXIT WHEN NOT FOUND;
		
		SELECT COUNT(*) INTO v_rental_count FROM Inventory i
    		JOIN Rental r ON i.inventory_id = r.inventory_id WHERE i.film_id = v_film_id;

		IF v_rental_count < 5 THEN
			UPDATE Film SET rental_rate = rental_rate + 1 WHERE film_id = v_film_id;
			RAISE NOTICE 'Rental Rate increased for % by $1', v_film_name;
		END IF;
	END LOOP;
	CLOSE film_cursor;
END;
$$

-- 4) Create a function using a cursor that collects titles of all films from a particular category.
CREATE OR REPLACE FUNCTION get_titles_by_category(p_category VARCHAR(50))
$$

$$
LANGUAGE plpgsql

-- 5) Loop through all stores and count how many distinct films are available in each store using a cursor.
DO
$$

DECLARE 
v_film_count INT;
v_store_id INT;

store_cursor CURSOR FOR
	SELECT store_id FROM Store;
BEGIN
	OPEN store_cursor;
	LOOP
		FETCH NEXT FROM store_cursor INTO v_store_id;
		EXIT WHEN NOT FOUND;

		SELECT COUNT(DISTINCT film_id) INTO v_film_count FROM Inventory WHERE store_id = v_store_id;

		RAISE NOTICE 'Store ID: %, Number of films: %', v_store_id, v_film_count;
	END LOOP;
	CLOSE store_cursor;
END;
$$

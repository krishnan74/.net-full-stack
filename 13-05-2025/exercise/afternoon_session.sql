
-- 1) Cursor: List all customers and how many rentals each made. Insert into summary table.
CREATE TABLE customer_rental_summary(summary_id SERIAL PRIMARY KEY, customer_id INT, customer_name VARCHAR(100), rental_count INT, summary_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP);

DO
$$
DECLARE 
  v_customer_id INT;
  v_customer_name VARCHAR(100);
  v_rental_count INT;
  customer_cursor CURSOR FOR
    SELECT customer_id, CONCAT(first_name, ' ', last_name) FROM Customer;
BEGIN
  OPEN customer_cursor;
  LOOP
    FETCH NEXT FROM customer_cursor INTO v_customer_id, v_customer_name;
    EXIT WHEN NOT FOUND;

    SELECT COUNT(*) INTO v_rental_count FROM Rental 
    WHERE customer_id = v_customer_id;

    RAISE NOTICE 'Customer Name: %, Rental Count: %', v_customer_name, v_rental_count;

    INSERT INTO customer_rental_summary(customer_id, customer_name, rental_count)
    VALUES (v_customer_id, v_customer_name, v_rental_count);
  END LOOP;
  CLOSE customer_cursor;
END;
$$;

SELECT * FROM customer_rental_summary;

-- 2) Cursor: Print titles of 'Comedy' films rented more than 10 times
DO
$$
DECLARE 
  v_film_id INT;
  v_film_name VARCHAR(255);
  v_rental_count INT;
  film_cursor CURSOR FOR
    SELECT f.film_id, f.title FROM Film f 
    JOIN Film_Category fc ON f.film_id = fc.film_id
    JOIN Category c ON c.category_id = fc.category_id
    WHERE c.name = 'Comedy';
BEGIN
  OPEN film_cursor;
  LOOP
    FETCH NEXT FROM film_cursor INTO v_film_id, v_film_name;
    EXIT WHEN NOT FOUND;

    SELECT COUNT(*) INTO v_rental_count FROM Inventory i
    JOIN Rental r ON i.inventory_id = r.inventory_id 
    WHERE i.film_id = v_film_id;

    IF v_rental_count > 10 THEN
      RAISE NOTICE 'Film Name: %, Rental Count: %', v_film_name, v_rental_count;
    END IF;
  END LOOP;
  CLOSE film_cursor;
END;
$$;

-- 3) Cursor: Count distinct films per store and insert into report

CREATE TABLE store_film_report(store_id INT PRIMARY KEY, film_count INT, report_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP);
DO
$$
DECLARE
  v_store_id INT;
  v_film_count INT;
  store_cursor CURSOR FOR SELECT store_id FROM store;
BEGIN
  OPEN store_cursor;
  LOOP
    FETCH NEXT FROM store_cursor INTO v_store_id;
    EXIT WHEN NOT FOUND;

    SELECT COUNT(DISTINCT film_id) INTO v_film_count
    FROM inventory WHERE store_id = v_store_id;

    INSERT INTO store_film_report(store_id, film_count)
    VALUES (v_store_id, v_film_count);
  END LOOP;
  CLOSE store_cursor;
END;
$$;

SELECT * FROM store_film_report;

-- 4) Insert inactive customers (no rentals in last 6 months)
CREATE TABLE inactive_customer (customer_id INT, first_name VARCHAR(50), last_name VARCHAR(50), marked_inactive_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP);
Do
$$
DECLARE 
	v_customer_id INT;
	v_first_name VARCHAR(50);
	v_last_name VARCHAR(50);
	v_last_rental TIMESTAMP;
	customer_cursor CURSOR FOR
		SELECT customer_id, first_name, last_name FROM Customer;

	BEGIN
		OPEN customer_cursor;
		LOOP
			FETCH NEXT FROM customer_cursor INTO v_customer_id, v_first_name, v_last_name;
			EXIT WHEN NOT FOUND;

			SELECT MAX(rental_date) INTO v_last_rental FROM Rental
			WHERE customer_id = v_customer_id;

			IF v_last_rental IS NULL OR v_last_rental < CURRENT_DATE - INTERVAL '6 months' THEN
				INSERT INTO inactive_customer(customer_id, first_name, last_name)
				VALUES (v_customer_id, v_first_name, v_last_name);
			END IF;
		END LOOP;
		CLOSE customer_cursor;
	END;
$$;
SELECT * FROM inactive_customer;
		
-- Transactions Examples
ABORT;
-- 1) Atomic insert of customer, rental, and payment
BEGIN;
INSERT INTO customer (first_name, last_name, store_id, email) VALUES ('Mark', 'Grayson', 1, 'markgrayson@gmail.com') RETURNING customer_id INTO new_customer_id;
INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id) VALUES (NOW(), 1, new_customer_id, 1) RETURNING rental_id INTO new_rental_id;
INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date) VALUES (new_customer_id, 1, new_rental_id, 9.99, NOW());
COMMIT;

-- 2) Simulate failure and rollback
BEGIN;
UPDATE film SET rental_duration = 7 WHERE film_id = 1;
INSERT INTO inventory (film_id, store_id) VALUES (99999, 1); -- Invalid film_id
ROLLBACK;

-- 3) Use SAVEPOINT to rollback one update
BEGIN;
SAVEPOINT start_payment;
UPDATE payment SET amount = amount + 1 WHERE payment_id = 1;
SAVEPOINT after_first;
UPDATE payment SET amount = amount + 1 WHERE payment_id = 2;
ROLLBACK TO SAVEPOINT after_first;
COMMIT;

-- 4) Transfer inventory between stores
BEGIN;
DELETE FROM inventory WHERE inventory_id = 100 AND store_id = 1;
INSERT INTO inventory (inventory_id, film_id, store_id) VALUES (100, 50, 2);
COMMIT;

-- 5) Delete customer and associated records
BEGIN;
DELETE FROM payment WHERE customer_id = 5;
DELETE FROM rental WHERE customer_id = 5;
DELETE FROM customer WHERE customer_id = 5;
COMMIT;

-- Triggers

-- 1) Prevent zero or negative payments
CREATE OR REPLACE FUNCTION prevent_negative_payments()
RETURNS TRIGGER AS $$
BEGIN
  IF NEW.amount <= 0 THEN
    RAISE EXCEPTION 'Payment amount must be positive';
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_prevent_negative_payment
BEFORE INSERT ON payment
FOR EACH ROW EXECUTE FUNCTION prevent_negative_payments();

-- 2) Auto-update 'last_update' on film
CREATE OR REPLACE FUNCTION update_film_last_update()
RETURNS TRIGGER AS $$
BEGIN
  NEW.last_update := NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_update_film_last_update
BEFORE UPDATE OF title, rental_rate ON film
FOR EACH ROW EXECUTE FUNCTION update_film_last_update();

-- 3) Log film rentals over 3 times per week
CREATE TABLE rental_log(log_id SERIAL PRIMARY KEY, film_id INT, log_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP, message VARCHAR(100));

CREATE OR REPLACE FUNCTION log_frequent_rental()
RETURNS TRIGGER AS $$
DECLARE
  rental_count INT;
BEGIN
  SELECT COUNT(*) INTO rental_count
  FROM rental
  WHERE film_id = NEW.film_id
   AND rental_date >= CURRENT_DATE - INTERVAL '7 days';

  IF rental_count > 3 THEN
    INSERT INTO rental_log(log_id, film_id, log_time, message)
    VALUES (NEW.film_id, NOW(), 'High rental frequency');
  END IF;

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_frequent_rental
AFTER INSERT ON rental
FOR EACH ROW EXECUTE FUNCTION log_frequent_rental();
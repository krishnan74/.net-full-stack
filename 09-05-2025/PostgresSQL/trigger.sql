
-- Trigger-Based Questions (5)
-- 6) Write a trigger that logs whenever a new customer is inserted.
CREATE TABLE Log ( 
	log_id SERIAL PRIMARY KEY,
	action VARCHAR(200),
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
)

CREATE OR REPLACE FUNCTION log_new_customer()
RETURNS TRIGGER AS
	$$
	BEGIN 
		INSERT INTO Log ( action ) VALUES ( 
		CONCAT('New Customer created with Name: ', NEW.first_name, ' ',NEW.last_name, ', Email: ', NEW.email));
		RETURN NEW;
	END;
$$ 
LANGUAGE plpgsql;

CREATE TRIGGER trig_log_new_customer 
AFTER INSERT ON Customer
FOR EACH ROW EXECUTE FUNCTION log_new_customer();

INSERT INTO Customer (store_id, first_name, last_name, email, address_id, activebool, active, create_date)
VALUES
(2, 'James', 'Bond', 'james.bond@sakilacustomer.org', 5, true, 1, CURRENT_TIMESTAMP);

-- 7) Create a trigger that prevents inserting a payment of amount 0.
CREATE OR REPLACE FUNCTION prevent_zero_payment()
RETURNS TRIGGER AS
	$$
	BEGIN 
		IF NEW.amount = 0 THEN
			-- INSERT INTO Log ( action ) VALUES ( 'Creating new payment failed due to amount zero' );
			RAISE EXCEPTION 'Creating new payment failed due to amount zero';
		END IF;
			RAISE NOTICE 'New Payment record created succesfully';
		RETURN NEW;
	END;
	$$ 
LANGUAGE plpgsql;

CREATE TRIGGER trig_prevent_zero_payment 
BEFORE INSERT ON Payment
FOR EACH ROW EXECUTE FUNCTION prevent_zero_payment();

-- Succeeds
INSERT INTO Payment( customer_id, staff_id, rental_id, amount, payment_date)
VALUES ( 341, 2, 1520, 10, CURRENT_TIMESTAMP);
-- Fails
INSERT INTO Payment( customer_id, staff_id, rental_id, amount, payment_date)
VALUES ( 341, 2, 1520, 0, CURRENT_TIMESTAMP);
SELECT * FROM Log;

-- 8) Set up a trigger to automatically set last_update on the film table before update.
CREATE OR REPLACE FUNCTION set_last_update()
RETURNS TRIGGER AS
$$
	BEGIN 
		UPDATE Film SET last_update = CURRENT_TIMESTAMP WHERE film_id = NEW.film_id;
	RETURN NEW;
	END;
$$
LANGUAGE plpgsql;

CREATE TRIGGER trig_set_last_update 
BEFORE UPDATE ON Film FOR EACH ROW
EXECUTE FUNCTION set_last_update();

-- 9) Create a trigger to log changes in the inventory table (insert/delete).
CREATE OR REPLACE FUNCTION log_inventory_changes()
RETURNS TRIGGER AS
$$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO log (action) VALUES (
            CONCAT(
                'Inserted new record in inventory with the values: ',
                'inventory_id: ', NEW.inventory_id,
                ', film_id: ', NEW.film_id,
                ', store_id: ', NEW.store_id
            )
        );
       
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO log (action) VALUES (
            CONCAT('Deleted record in inventory with the id: ', OLD.inventory_id)
        );
    END IF;

    RETURN NULL;
END;
$$
LANGUAGE plpgsql;

CREATE TRIGGER trig_log_inventory_changes 
AFTER INSERT OR DELETE ON Inventory
FOR EACH ROW EXECUTE FUNCTION log_inventory_changes();

-- 10) Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.
CREATE OR REPLACE FUNCTION prevent_rental_if_customer_owes()
RETURNS TRIGGER AS
$$
DECLARE
    total_paid NUMERIC;
    total_due NUMERIC;
BEGIN
    SELECT COALESCE(SUM(amount), 0) INTO total_paid
    FROM Payment
    WHERE customer_id = NEW.customer_id;

    SELECT SUM(f.rental_rate) INTO total_due  
    FROM Rental re
	JOIN Inventory inv ON re.inventory_id = inv.inventory_id
	JOIN Film f ON inv.film_id = f.film_id
    WHERE re.customer_id = NEW.customer_id;

    IF (total_due - total_paid) > 50 THEN
        RAISE EXCEPTION 'Customer has outstanding balance over $50. Cannot proceed with rental.';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_check_customer_debt
BEFORE INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION prevent_rental_if_customer_owes();
 
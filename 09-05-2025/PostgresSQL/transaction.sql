
-- Transaction-Based Questions (5)
-- 11) Write a transaction that inserts a customer and an initial rental in one atomic operation.

SELECT * FROM CUSTOMER ORDER BY customer_id DESC;
BEGIN;

INSERT INTO customer (store_id, first_name, last_name, email, address_id, active, create_date)
VALUES (
    1, 'John', 'Durairaj', 'john.durairaj@example.com', 5, 1, CURRENT_DATE
)
RETURNING customer_id;

INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
VALUES (
    CURRENT_TIMESTAMP,
    (SELECT inventory_id FROM inventory LIMIT 1),
    607,
    1
);

COMMIT;

-- 12) Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.

BEGIN;

UPDATE film
SET title = 'New Title'
WHERE film_id = 1;

INSERT INTO inventory (film_id, store_id)
VALUES (1, NULL);  

ROLLBACK;

-- 13) Create a transaction that transfers an inventory item from one store to another.

BEGIN;

UPDATE inventory
SET store_id = 2
WHERE inventory_id = 105 AND store_id = 1;

INSERT INTO inventory_log (inventory_id, film_id, store_id, operation)
SELECT inventory_id, film_id, 2, 'TRANSFER'
FROM inventory
WHERE inventory_id = 105;

COMMIT;

-- 14) Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.

BEGIN;

UPDATE payment
SET amount = amount + 1
WHERE payment_id = 1;

SAVEPOINT before_second_update;

UPDATE payment
SET amount = amount + 2
WHERE payment_id = 2;

ROLLBACK TO SAVEPOINT before_second_update;

COMMIT;


-- 15) Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.

BEGIN;

DELETE FROM payment
WHERE customer_id = 599;

DELETE FROM rental
WHERE customer_id = 599;

DELETE FROM customer
WHERE customer_id = 599;

COMMIT; 
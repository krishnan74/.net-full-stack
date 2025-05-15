CREATE EXTENSION IF NOT EXISTS pgcrypto;

--  1. Create a stored procedure to encrypt a given text
-- Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.

CREATE OR REPLACE PROCEDURE proc_encrypt_text(
  IN p_plain_text TEXT,
  IN p_key TEXT,
  OUT p_encrypted BYTEA
)
LANGUAGE plpgsql
AS $$
BEGIN
  p_encrypted := pgp_sym_encrypt(p_plain_text, p_key);
END;
$$;

Do
$$
DECLARE 
p_input TEXT; p_encrypted BYTEA; p_key TEXT;

BEGIN
	p_input := 'Hello';
	p_key := 'KEY';
	CALL proc_encrypt_text(p_input, p_key, p_encrypted);
	RAISE NOTICE 'Original Text : %, Encrypted Text: %', p_input, p_encrypted;
END;
$$


-- 2. Create a stored procedure to compare two encrypted texts
-- Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.

CREATE OR REPLACE PROCEDURE proc_compare_encrypted(
  IN p_enc1 BYTEA,
  IN p_enc2 BYTEA,
  IN p_key TEXT,
  OUT p_result BOOLEAN
)
LANGUAGE plpgsql
AS $$
DECLARE
  plain1 TEXT;
  plain2 TEXT;
BEGIN
  plain1 := pgp_sym_decrypt(p_enc1, p_key);
  plain2 := pgp_sym_decrypt(p_enc2, p_key);
  p_result := (plain1 = plain2);
EXCEPTION
  WHEN OTHERS THEN
    p_result := FALSE;
END;
$$;

Do
$$
DECLARE 
v_input TEXT; v_encrypted_1 BYTEA; v_encrypted_2 BYTEA; v_key TEXT; v_result BOOLEAN;

BEGIN
	v_input := 'Hello';
	v_key := 'KEY';
	CALL proc_encrypted_text(v_input, v_key, v_encrypted_1);
	CALL proc_encrypted_text(v_input, v_key, v_encrypted_2);

	CALL proc_compare_encrypted(v_encrypted_1, v_encrypted_2, v_key, v_result);

	IF v_result THEN
		RAISE NOTICE 'The two encrypted text are the same';
	ELSE
		RAISE NOTICE 'The two encrypted text are not the same';
	END IF;
END;
$$


--  3. Create a stored procedure to partially mask a given text
-- Task: Write a procedure sp_mask_text that:
-- Shows only the first 2 and last 2 characters of the input string
-- Masks the rest with *

CREATE OR REPLACE PROCEDURE proc_mask_text(
  IN p_input TEXT,
  OUT p_masked TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
  len INT := LENGTH(p_input);
BEGIN
  IF len <= 4 THEN
    p_masked := p_input;
  ELSE
    p_masked := LEFT(p_input, 2) || REPEAT('*', len - 4) || RIGHT(p_input, 2);
  END IF;
  
END;
$$;

Do
$$
DECLARE 
p_input TEXT; p_masked TEXT;

BEGIN
	p_input := 'harrykane@gmail.com';
	CALL proc_mask_text(p_input, p_masked);
	RAISE NOTICE 'Original Name: %, Masked Name: %', p_input, p_masked;

	p_input := 'haom';
	CALL proc_mask_text(p_input, p_masked);
	RAISE NOTICE 'Original Name: %, Masked Name: %', p_input, p_masked;
	
END;
$$


-- 4. Create a procedure to insert into customer with encrypted email and masked name
-- Task:
-- Call sp_encrypt_text for email
-- Call sp_mask_text for first_name
-- Insert masked and encrypted values into the customer table
-- Use any valid address_id and store_id to satisfy FK constraints.

ALTER TABLE customer ADD COLUMN IF NOT EXISTS encrypted_email BYTEA;
ALTER TABLE customer ADD COLUMN IF NOT EXISTS masked_name TEXT;

CREATE OR REPLACE PROCEDURE sp_insert_customer(
  p_first_name TEXT,
  p_last_name TEXT,
  p_email TEXT,
  p_key TEXT,
  p_address_id INT,
  p_store_id INT
)
LANGUAGE plpgsql
AS $$
DECLARE
  v_encrypted_email BYTEA;
  v_masked_name TEXT;
BEGIN
  -- Mask first name
  CALL sp_mask_text(p_first_name, v_masked_name);

  -- Encrypt email
  CALL sp_encrypt_text(p_email, p_key, v_encrypted_email);

  -- Insert into customer
  INSERT INTO customer (
    store_id, first_name, last_name, email, address_id, create_date,
    encrypted_email, masked_name
  )
  VALUES (
    p_store_id, v_masked_name, p_last_name, NULL, p_address_id, NOW(),
    v_encrypted_email, v_masked_name
  );
END;
$$;
CALL sp_insert_customer('Harry', 'Kane', 'harrykane@gmail.com', 'KEY', 1, 1);


-- 5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
-- Task:
-- Write sp_read_customer_masked() that:
-- Loops through all rows
-- Decrypts email
-- Displays customer_id, masked first name, and decrypted email

DROP PROCEDURE sp_read_customer_masked;
CREATE OR REPLACE PROCEDURE sp_read_customer_masked(
  IN p_key TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
  v_customer_id INT;
  v_masked_name TEXT;
  v_encrypted_email BYTEA;
  v_email TEXT;
  customer_cursor CURSOR FOR 
  SELECT customer_id, masked_name, encrypted_email FROM customer LIMIT 10;
BEGIN
  OPEN customer_cursor;
  LOOP
    BEGIN
	FETCH NEXT FROM customer_cursor INTO v_customer_id, v_masked_name, v_encrypted_email;
      v_email := pgp_sym_decrypt(v_encrypted_email, p_key);
	  EXIT WHEN NOT FOUND;
    EXCEPTION
      WHEN OTHERS THEN
        v_email := '[Decryption Error]';
    END;

    RAISE NOTICE 'Customer ID: %, Masked Name: %, Email: %',
      v_customer_id, v_masked_name, v_email;
  END LOOP;
END;
$$;

CALL sp_read_customer_masked('KEY');
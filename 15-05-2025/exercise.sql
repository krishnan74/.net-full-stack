--  1. Create a function to encrypt a given text
-- Task: Write a function fn_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
CREATE OR REPLACE FUNCTION fn_encrypt_text(
  p_plain_text TEXT,
  p_key TEXT
)
RETURNS BYTEA

AS $$
BEGIN
  RETURN pgp_sym_encrypt(p_plain_text, p_key);
END;
$$ LANGUAGE plpgsql;


-- 2. Create a function to compare two encrypted texts
-- Task: Write a function fn_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.

CREATE OR REPLACE FUNCTION fn_compare_encrypted(
  p_enc1 BYTEA,
  p_enc2 BYTEA,
  p_key TEXT
)
RETURNS BOOLEAN
LANGUAGE plpgsql
AS $$
DECLARE
  plain1 TEXT;
  plain2 TEXT;
BEGIN
  plain1 := pgp_sym_decrypt(p_enc1, p_key);
  plain2 := pgp_sym_decrypt(p_enc2, p_key);
  RETURN (plain1 = plain2);
EXCEPTION
  WHEN OTHERS THEN
    RETURN FALSE;
END;
$$;


-- 3. Create a function to partially mask a given text

CREATE OR REPLACE FUNCTION fn_mask_text(p_input TEXT)
RETURNS TEXT
LANGUAGE plpgsql
AS $$
DECLARE
  len INT := LENGTH(p_input);
BEGIN
  IF len <= 4 THEN
    RETURN REPEAT('*', len);
  ELSE
    RETURN LEFT(p_input, 2) || REPEAT('*', len - 4) || RIGHT(p_input, 2);
  END IF;
END;
$$;


-- 4. Create a function to insert into customer with encrypted email and masked name

ALTER TABLE customer ADD COLUMN IF NOT EXISTS encrypted_email BYTEA;
ALTER TABLE customer ADD COLUMN IF NOT EXISTS masked_name TEXT;

CREATE OR REPLACE FUNCTION fn_insert_customer(
  p_first_name TEXT,
  p_last_name TEXT,
  p_email TEXT,
  p_key TEXT,
  p_address_id INT,
  p_store_id INT
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
DECLARE
  v_encrypted_email BYTEA;
  v_masked_name TEXT;
BEGIN
  v_masked_name := fn_mask_text(p_first_name);
  v_encrypted_email := fn_encrypt_text(p_email, p_key);

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


-- 5. Create a function to fetch and display masked first_name and decrypted email for all customers

CREATE OR REPLACE FUNCTION fn_read_customer_masked(p_key TEXT)
RETURNS TABLE(customer_id INT, masked_name TEXT, decrypted_email TEXT)
LANGUAGE plpgsql
AS $$
BEGIN
  RETURN QUERY
  SELECT
    c.customer_id,
    c.masked_name,
    CASE 
      WHEN c.encrypted_email IS NOT NULL THEN pgp_sym_decrypt(c.encrypted_email, p_key)
      ELSE '[No Email]'
    END AS decrypted_email
  FROM customer c;
END;
$$;
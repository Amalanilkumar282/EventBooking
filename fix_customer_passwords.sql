-- ====================================
-- SQL Script to Fix Customer Password Hashes
-- ====================================

-- 1. Check current state of Customers table
SELECT "Id", "Email", "FirstName", "LastName", 
       CASE 
           WHEN "PasswordHash" = '' THEN 'EMPTY'
           WHEN "PasswordHash" IS NULL THEN 'NULL'
           WHEN LENGTH("PasswordHash") > 0 THEN 'HAS PASSWORD'
       END as "PasswordStatus"
FROM "Customers";

-- 2. Update all customers with empty or null password hashes
-- Default password for all: "Password123"
-- BCrypt hash: $2a$12$iklihucJjkhCg/IQeZ6Wr.GG5J8lCoGZywmBwZL9PKqzht6hZ6QIi
UPDATE "Customers" 
SET "PasswordHash" = '$2a$12$iklihucJjkhCg/IQeZ6Wr.GG5J8lCoGZywmBwZL9PKqzht6hZ6QIi' 
WHERE "PasswordHash" = '' OR "PasswordHash" IS NULL;

-- 3. Verify the update
SELECT "Id", "Email", "FirstName", "LastName", 
       LENGTH("PasswordHash") as "PasswordHashLength",
       LEFT("PasswordHash", 10) as "PasswordHashPrefix"
FROM "Customers";

-- 4. Show count of customers with valid password hashes
SELECT COUNT(*) as "CustomersWithPassword"
FROM "Customers"
WHERE "PasswordHash" IS NOT NULL AND "PasswordHash" != '';

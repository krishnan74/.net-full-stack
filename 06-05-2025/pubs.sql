-- 1) Select all book titles
SELECT title FROM titles;

-- 2) Select all books published by id 1389 
SELECT * FROM titles WHERE pub_id = 1389;

-- 3) Select books with price between 10 and 15
SELECT * FROM titles WHERE price BETWEEN 10 AND 15;

-- 4) Select books where price is NULL
SELECT * FROM titles WHERE price IS NULL;

-- 5) Select books whose title starts with "The"
SELECT * FROM titles WHERE title LIKE 'The%';

-- 6) Select books whose title does NOT contain "v"
SELECT * FROM titles WHERE title NOT LIKE '%v%';

-- 7) Order books by royalty in ascending order
SELECT * FROM titles ORDER BY royalty ASC;

-- 8) Left join titles with publishers and order by pub_name DESC, type ASC, price DESC
SELECT * 
FROM titles t
LEFT JOIN publishers p ON t.pub_id = p.pub_id
ORDER BY p.pub_name DESC, t.type ASC, t.price DESC;

-- 9) Print the average price of books for each type
SELECT type, AVG(price) AS AvgPrice
FROM titles
GROUP BY type;
 
-- 10) Print all the types in unique
Select distinct type from titles;
 
-- 11) Print the first 2 costliest books
Select title from titles order by price desc limit 2;
 
-- 12) Print books that are of type business and have price less than 20 which also have advance greater than 7000
Select title from titles where type = "Business" and price < 20 and advance > 7000; 
 
-- 13) Select those publisher id and number of books which have price between 15 to 25 and have 'It' in its name. Print only those which have count greater than 2. Also sort the result in ascending order of count
Select pub_id, COUNT(*) as BookCount from titles where  price between 15 and 25  and title like "%It%" GROUP BY pub_id Having BookCount > 2 order by BookCount asc;
 
-- 14) Print the Authors who are from 'CA'
Select CONCAT(au_fname, au_lname) as AuthorName from authors where state = "CA";
 
-- 15) Print the count of authors from every state
Select state, Count(*) as AuthorCount from authors GROUP BY state;


-- How can you produce a list of the start times for bookings by members named 'David Farrell'?
SELECT b.starttime FROM CD.BOOKINGS b JOIN CD.MEMBERS m ON b.memid = m.memid 
WHERE m.firstname = 'David' AND m.surname = 'Farrell';

-- How can you produce a list of the start times for bookings for tennis courts, for the date '2012-09-21'? Return a list of start time and facility name pairings, ordered by the time.
SELECT b.starttime as start, f.name as name FROM CD.FACILITIES f 
JOIN CD.BOOKINGS b ON f.facid = b.facid WHERE DATE(b.starttime) = '2012-09-21' 
AND f.name IN ('Tennis Court 1', 'Tennis Court 2') ORDER BY b.starttime; 

-- How can you output a list of all members who have recommended another member? Ensure that there are no duplicates in the list, and that results are ordered by (surname, firstname).
SELECT DISTINCT firstname, surname FROM CD.MEMBERS WHERE memid IN (
  SELECT recommendedby from CD.MEMBERS WHERE recommendedby IS NOT NULL
  ) 
ORDER BY surname, firstname;
-- or
SELECT DISTINCT rec.firstname AS firstname , rec.surname AS surname FROM CD.MEMBERS mem
JOIN CD.MEMBERS rec on mem.recommendedby = rec.memid
ORDER BY surname, firstname;

-- How can you output a list of all members, including the individual who recommended them (if any)? Ensure that results are ordered by (surname, firstname).
SELECT mem.firstname AS memfname , mem.surname AS memsname, 
rec.firstname AS recfname, rec.surname AS recsname FROM CD.MEMBERS mem
LEFT JOIN CD.MEMBERS rec on mem.recommendedby = rec.memid
ORDER BY mem.surname, mem.firstname;

-- How can you produce a list of all members who have used a tennis court? Include in your output the name of the court, and the name of the member formatted as a single column. Ensure no duplicate data, and order by the member name followed by the facility name.
SELECT DISTINCT CONCAT(m.firstname,' ', m.surname) AS member, f.name AS facility FROM CD.MEMBERS m
JOIN CD.BOOKINGS b ON m.memid = b.memid
JOIN CD.FACILITIES f ON b.facid = f.facid
WHERE f.name IN ('Tennis Court 1', 'Tennis Court 2')
ORDER BY member, facility;

-- How can you produce a list of bookings on the day of 2012-09-14 which will cost the member (or guest) more than $30? Remember that guests have different costs to members (the listed costs are per half-hour 'slot'), and the guest user is always ID 0. Include in your output the name of the facility, the name of the member formatted as a single column, and the cost. Order by descending cost, and do not use any subqueries.
SELECT CONCAT(m.firstname, ' ', m.surname) as member, f.name as facility, 
CASE WHEN m.memid = 0 THEN b.slots*f.guestcost ELSE b.slots*f.membercost END as cost 
FROM CD.MEMBERS m
JOIN CD.BOOKINGS b on m.memid = b.memid
JOIN CD.FACILITIES f on f.facid = b.facid
WHERE DATE(b.starttime) = '2012-09-14' AND (
(m.memid = 0 AND b.slots * f.guestcost > 30) or (m.memid !=0 AND b.slots * f.membercost > 30) )
ORDER BY cost DESC;

-- How can you output a list of all members, including the individual who recommended them (if any), without using any joins? Ensure that there are no duplicates in the list, and that each firstname + surname pairing is formatted as a column and ordered.
SELECT DISTINCT CONCAT(firstname,' ', surname) as member,
	(SELECT CONCAT(rec.firstname,' ', rec.surname) as recommender 
	 	FROM CD.MEMBERS rec WHERE rec.memid = mem.recommendedby)
FROM CD.MEMBERS mem
ORDER BY member;


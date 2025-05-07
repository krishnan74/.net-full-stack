use pubs;
go

-- print the publisher details of the publisher who never published 
Select * from publishers where pub_id not in ( Select distinct pub_id from titles );

-- Select the author_id, book_name for all the books 
Select au_id, title from titles join titleauthor on titles.title_id = titleauthor.title_id;

-- Select Author Name and Book Name 
select concat(au_fname, au_lname) AuthorName , title BookName from authors au 
join titleauthor ta on au.au_id = ta.au_id
join titles t on ta.title_id = t.title_id;

-- Print the publisher'name, book name, and the order date of the books

select pub_name PublisherName, title BookName, ord_date DateOfOrder from publishers p 
join titles t on p.pub_id = t.pub_id
join sales s on t.title_id = s.title_id;


-- Print the publisher's name and the first book sale date for all the publishers

select pub_name PublisherName, MIN(ord_date) DateOfOrder from publishers p 
left outer join titles t on p.pub_id = t.pub_id
left outer join sales s on t.title_id = s.title_id group by p.pub_name order by 2 desc;

-- Print the book name and the store address of the sale
select title BookName, stor_address StoreAddress from titles t
join sales s on t.title_id = s.title_id
join stores st on s.stor_id = st.stor_id order by BookName;

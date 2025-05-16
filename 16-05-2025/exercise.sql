-- Phase 1: DATABASE DESIGN
-- student
--     student_id serial int primary key 
--     first_name VARCHAR
--     last_name VARCHAR
--     email VARCHAR

-- trainer
--     trainer_id serial primary key
--     first_name VARCHAR
--     last_name VARCHAR
--     email VARCHAR

-- course
--     course_id serial primary key
--     course_name VARCHAR
--     trainer_id foreign key trainer(trainer_id)

-- student_course
--     student_course_id serial primary key
--     student_id foreign key student(student_id)
--     course_id foreign key course(course_id)
--     enrollment_date TIMESTAMP

-- certificate
--     certificate_id serial primary key
--     student_course_id foreign key student_course(student_course_id)
--     serial_number VARCHAR
--     issue_date DATE

-- course_trainer
--     trainer_id foreign key Trainer(trainer_id)
--     course_id foreign key Course(course_id)

-- Phase 2: DDL & DML

CREATE TABLE Student(
    student_id SERIAL PRIMARY KEY, 
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(10) NOT NULL
);

CREATE TABLE Trainer(
    trainer_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
	expertise VARCHAR(100)
);

CREATE TABLE Course(
    course_id SERIAL PRIMARY KEY,
    course_name VARCHAR(100) NOT NULL,
    category VARCHAR(50) NOT NULL,
    duration_in_days INT CHECK (duration > 0),
    trainer_id INT NOT NULL REFERENCES Trainer(trainer_id)
);


CREATE TABLE Student_Course_Enrollment(
    enrollment_id SERIAL PRIMARY KEY,
    student_id INT NOT NULL REFERENCES Student(student_id),
    course_id INT NOT NULL REFERENCES Course(course_id),
    enrollment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE Certificate(
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INT NOT NULL REFERENCES Student_Course_Enrollment(enrollment_id),
    serial_number VARCHAR(50) NOT NULL UNIQUE,
    issue_date TIMESTAMP NOT NULL
)

CREATE TABLE Course_Trainer(
    handling_id SERIAL PRIMARY KEY,
    course_id INT NOT NULL REFERENCES Course(course_id),
    trainer_id INT NOT NULL REFERENCES Trainer(trainer_id)
)

-- Inserting sample data into the created tables 

INSERT INTO Student (first_name, last_name, email, phone) VALUES
('Levi', 'Ackerman', 'levi@example.com', '9943194032'),
('John', 'Snow', 'john@example.com', '9473829837'),
('Charlie', 'Brown', 'charlie@example.com', '7473975638');

SELECT * FROM TRAINER;

INSERT INTO Trainer (first_name, last_name, email, expertise) VALUES
('Dr. Walter', 'White', 'walter.white@example.com', 'RDBMS'),
('Mr. Pink', 'Man', 'pink.man@example.com', 'Full Stack Development');
('Kartik', 'Talwar', 'kartik.talwar@eample.com', 'Blockchain Development');

INSERT INTO Course (course_name, category, duration_in_days, trainer_id) VALUES
('PostgresSQL', 'Database', 20, 12),
('.NET Full Stack Development', 'Full Stack Development', 50, 13),
('Starknet Dojo 101', 'Blockchain Game Development',30, 14 );

SELECT * FROM STUDENT;

INSERT INTO Course_Trainer (course_id, trainer_id) VALUES
(7, 12), 
(8, 13);

INSERT INTO Student_Course_Enrollment (student_id, course_id) VALUES
(14, 7),
(15, 7),
(16, 8);

SELECT * FROM Student_Course_Enrollment;

INSERT INTO certificate (enrollment_id, serial_number, issue_date) VALUES
(1, 'CERT-PY-001', '2025-05-01'),
(3, 'CERT-DS-001', '2025-05-03');

-- Creating Indexes 
CREATE INDEX idx_student_id ON Student(student_id);
CREATE INDEX idx_student_email ON Student(email);
CREATE INDEX idx_course_id ON Course(course_id);


-- Phase 3: SQL JOINS

-- 1. List students and the courses they enrolled in
SELECT CONCAT( s.first_name, ' ', s.last_name ) as Student_Name , c.course_name FROM Student s
JOIN Student_Course_Enrollment sce ON s.student_id = sce.student_id
JOIN Course c ON sce.course_id = c.course_id;

-- 2. Show students who received certificates with trainer names
SELECT CONCAT( s.first_name, ' ', s.last_name ) as Student_Name, c.course_name, 
CONCAT( t.first_name, ' ', t.last_name ) as Trainer_Name FROM Certificate cert
JOIN Student_Course_Enrollment sce ON cert.enrollment_id = sce.enrollment_id
JOIN Student s ON sce.student_id = s.student_id
JOIN Course c ON sce.course_id = c.course_id
JOIN Trainer t ON c.trainer_id = t.trainer_id;

-- 3. Count number of students per course
SELECT c.course_name, Count(DISTINCT sce.student_id) AS Student_Count FROM Course c 
LEFT JOIN Student_Course_Enrollment sce ON sce.course_id = c.course_id 
GROUP BY c.course_name, c.course_id;


-- Phase 4: Functions & Stored Procedures
CREATE OR REPLACE FUNCTION get_certified_students(p_course_id INT)
RETURNS TABLE(student_id INT, full_name TEXT, email VARCHAR(255)) AS $$
BEGIN
  RETURN QUERY
  SELECT s.student_id,
         s.first_name || ' ' || s.last_name AS full_name,
         s.email
  FROM student s
  JOIN student_course_enrollment e ON s.student_id = e.student_id
  JOIN certificate c ON c.enrollment_id = e.enrollment_id
  WHERE e.course_id = p_course_id;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_certified_students(8);


CREATE OR REPLACE PROCEDURE sp_enroll_student(
    p_student_id INT,
    p_course_id INT,
    p_completed BOOLEAN DEFAULT FALSE
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_enrollment_id INT;
BEGIN
    INSERT INTO student_course_enrollment (student_id, course_id)
    VALUES (p_student_id, p_course_id)
    RETURNING enrollment_id INTO v_enrollment_id;

    IF p_completed THEN
        INSERT INTO certificate (enrollment_id, serial_number, issue_date)
        VALUES (v_enrollment_id,
                'CE-' || p_course_id || '-' || LPAD(v_enrollment_id::TEXT, 3, '0'),
                CURRENT_DATE);
    END IF;
END;
$$;

CALL sp_enroll_student(17, 9, true);

-- Phase 5: Cursor

INSERT INTO Student (first_name, last_name, email, phone) VALUES
('Ark', 'Noah', 'ark@example.com', '9483234032'),
('Mark', 'Grayson', 'mark@example.com', '8390398379');

INSERT INTO Student_Course_Enrollment (student_id, course_id) VALUES
(17, 8),
(18, 7);

DO $$
DECLARE
    rec RECORD;
    student_cursor CURSOR FOR
        SELECT s.first_name, s.last_name, s.email
        FROM student s
        JOIN student_course_enrollment e ON s.student_id = e.student_id
        WHERE e.course_id = 7
        AND e.enrollment_id NOT IN (
            SELECT enrollment_id FROM certificate
        );
BEGIN
    OPEN student_cursor;
    LOOP
        FETCH NEXT FROM student_cursor INTO rec;
        EXIT WHEN NOT FOUND;
        RAISE NOTICE 'Student: % %, Email: %', rec.first_name, rec.last_name, rec.email;
    END LOOP;
    CLOSE student_cursor;
END $$;

-- Phase 6: Security & Roles

-- Create role
CREATE ROLE readonly_user LOGIN PASSWORD 'readonly_pass';

-- Grant SELECT privileges
GRANT CONNECT ON DATABASE "Sample" TO readonly_user;
GRANT USAGE ON SCHEMA public TO readonly_user;
GRANT SELECT ON student, course, certificate TO readonly_user;

-- Create role
CREATE ROLE data_entry_user LOGIN PASSWORD 'entry_pass';

-- Grant access
GRANT CONNECT ON DATABASE "Sample" TO data_entry_user;
GRANT USAGE ON SCHEMA public TO data_entry_user;

-- Insert-only rights
GRANT INSERT ON student, student_course_enrollment TO data_entry_user;
-- Restrict access to certificate
REVOKE ALL ON certificate FROM data_entry_user;


-- Phase 7: Transactions & Atomicity

BEGIN;

WITH enrollment_ins AS (
    INSERT INTO student_course_enrollment (student_id, course_id)
    VALUES (18, 9) 
    RETURNING enrollment_id
)

INSERT INTO certificate (enrollment_id, serial_number, issue_date)
SELECT enrollment_id,
       'CE-' || 9 || '-' || LPAD(enrollment_id::TEXT, 3, '0'),
       CURRENT_DATE
FROM enrollment_ins;

COMMIT;

SELECT * FROM CERTIFICATE;
SELECT * FROM student_course_enrollment;
SELECT * FROM COURSE;
SELECT * FROM STUDENT;


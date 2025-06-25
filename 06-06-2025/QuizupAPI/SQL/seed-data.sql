DELETE FROM answers;

DELETE FROM "quizSubmissions";

DELETE FROM "quizQuestions";

DELETE FROM questions;

DELETE FROM quizzes;

DELETE FROM students;

DELETE FROM teachers;

DELETE FROM users;

SELECT setval(
  pg_get_serial_sequence('answers', 'Id'), 1, false
);

SELECT setval(
  pg_get_serial_sequence('"quizSubmissions"', 'Id'), 1, false
);

SELECT setval(
  pg_get_serial_sequence('"quizQuestions"', 'Id'), 1, false
);

SELECT setval(
  pg_get_serial_sequence('questions', 'Id'), 1, false
);
SELECT setval(
  pg_get_serial_sequence('quizzes', 'Id'), 1, false
);

SELECT setval(
  pg_get_serial_sequence('students', 'Id'), 1
);

SELECT setval(
  pg_get_serial_sequence('teachers', 'Id'), 1, false
);




 
-- =====================================================

-- 1. INSERT USERS

-- =====================================================

  

-- Teacher Users

INSERT INTO users ("Username", "Role", "HashedPassword", "IsDeleted", "CreatedAt") VALUES
('john.smith@school.edu', 'Teacher', '$2a$12$bZp4LcJYv4HSNAiM5rJ3Ju3r7cN0gN2HvYb1Ft8qz4dN5PlKXk49W', false, '2025-01-15 09:00:00'),
('sarah.johnson@school.edu', 'Teacher', '$2a$12$b6iUGGte87qqw24.QyI7m.azUz21bJpRVwUqbfQagyjdn57xJa4Xy', false, '2025-01-16 10:30:00'),
('michael.brown@school.edu', 'Teacher', '$2a$12$QQbRFnEbA9ITQYwe.10y0uHCo2NHkwOab.UtUeodq6hZG8h9rxWf2', false, '2025-01-17 11:15:00'),
('emily.davis@school.edu', 'Teacher', '$2a$12$BVINySFI6qjRnCQsUzczW.WuqAv4evfuS/z0o767q49U4abqAuYEK', false, '2025-01-18 14:20:00'),
('david.wilson@school.edu', 'Teacher', '$2a$12$PK5jFqB.GA70v/giaO7UQuW3xDBBVnKTYGhHMJUNbFa.DSLyFHRym', false, '2025-01-19 16:45:00');

  

-- Student Users

INSERT INTO users ("Username", "Role", "HashedPassword", "IsDeleted", "CreatedAt") VALUES
('alice.student@school.edu', 'Student', '$2a$12$PK5jFqB.GA70v/giaO7UQuW3xDBBVnKTYGhHMJUNbFa.DSLyFHRym', false, '2025-01-20 08:00:00'),
('bob.student@school.edu', 'Student', '$2a$12$jOZ1CGxYsFrEVLFWGqdPl.f3E7atGecW4tsHWDG1qHHxo.07ydGpa', false, '2025-01-20 08:05:00'),
('carol.student@school.edu', 'Student', '$2a$12$Y2taquKsCkwIkN0ClxEtluyEPxSCy46cbMKlh7VxQ8jmaCJ2mQaiy', false, '2025-01-20 08:10:00'),
('dave.student@school.edu', 'Student', '$2a$12$t7zmnBquBeCNoAQwe5Es1ee0eKGIiqGFxuHvmwPP7ZJXYQ9J33RJS', false, '2025-01-20 08:15:00'),
('eve.student@school.edu', 'Student', '$2a$12$Zojs1Wj47Uem1P9p73lzB.riLNWJyolzIEXJcFVK2LG0/H0lf/CY6', false, '2025-01-20 08:20:00'),
('frank.student@school.edu', 'Student', '$2a$12$s6zFELEMzBQuSgChI6pN9ejNMsKqgi9EGEHX3Mc1GuxTBf3Dtc3jy', false, '2025-01-20 08:25:00'),
('grace.student@school.edu', 'Student', '$2a$12$upmP5p02zhn6QNbmZemNzeZc7eG4gHLW3jJ.KXYGbyF1jq6YP7Dga', false, '2025-01-20 08:30:00'),
('henry.student@school.edu', 'Student', '$2a$12$sHUazHgKP6UdwDXtpfBXCegcDcm2CUntJ5lfXdgkstB6GgWdGC4su', false, '2025-01-20 08:35:00'),
('iris.student@school.edu', 'Student', '$2a$12$l6RixAtHyB2vc4//P8k61uRwzE03okBaXAhhzKj5Ko.6b.Znd5bFK', false, '2025-01-20 08:40:00'),
('jack.student@school.edu', 'Student', '$2a$12$D.Ju5uo8o/d/4i.K1gz1.OWinggzjmXXM6penXQllcyeBInR6MfvO', false, '2025-01-20 08:45:00');

  

-- =====================================================

-- 2. INSERT TEACHERS

-- =====================================================

  

INSERT INTO teachers ("Email", "FirstName", "LastName", "Subject", "IsDeleted", "CreatedAt") VALUES
('john.smith@school.edu', 'John', 'Smith', 'Mathematics', false, '2025-01-15 09:00:00'),
('sarah.johnson@school.edu', 'Sarah', 'Johnson', 'Science', false, '2025-01-16 10:30:00'),
('michael.brown@school.edu', 'Michael', 'Brown', 'History', false, '2025-01-17 11:15:00'),
('emily.davis@school.edu', 'Emily', 'Davis', 'English', false, '2025-01-18 14:20:00'),
('david.wilson@school.edu', 'David', 'Wilson', 'Computer Science', false, '2025-01-19 16:45:00');

  

-- =====================================================

-- 3. INSERT STUDENTS

-- =====================================================

  

INSERT INTO students ("Email", "FirstName", "LastName", "Class", "IsDeleted", "CreatedAt") VALUES
('alice.student@school.edu', 'Alice', 'Anderson', 'Grade 10A', false, '2025-01-20 08:00:00'),
('bob.student@school.edu', 'Bob', 'Baker', 'Grade 10A', false, '2025-01-20 08:05:00'),
('carol.student@school.edu', 'Carol', 'Clark', 'Grade 10B', false, '2025-01-20 08:10:00'),
('dave.student@school.edu', 'Dave', 'Davis', 'Grade 10B', false, '2025-01-20 08:15:00'),
('eve.student@school.edu', 'Eve', 'Evans', 'Grade 11A', false, '2025-01-20 08:20:00'),
('frank.student@school.edu', 'Frank', 'Fisher', 'Grade 11A', false, '2025-01-20 08:25:00'),
('grace.student@school.edu', 'Grace', 'Garcia', 'Grade 11B', false, '2025-01-20 08:30:00'),
('henry.student@school.edu', 'Henry', 'Harris', 'Grade 11B', false, '2025-01-20 08:35:00'),
('iris.student@school.edu', 'Iris', 'Irwin', 'Grade 12A', false, '2025-01-20 08:40:00'),
('jack.student@school.edu', 'Jack', 'Jackson', 'Grade 12A', false, '2025-01-20 08:45:00');

  

-- =====================================================

-- 4. INSERT QUIZZES

-- =====================================================

  

INSERT INTO quizzes ("Title", "Description", "TeacherId", "CreatedAt", "DueDate", "IsActive", "IsDeleted") VALUES

-- Math Quizzes (Teacher ID 1)

('Algebra Basics', 'Introduction to algebraic expressions and equations', 1, '2025-02-01 09:00:00', '2025-02-15 23:59:59', true, false),

('Geometry Fundamentals', 'Basic geometric shapes and properties', 1, '2025-02-10 10:00:00', '2025-02-25 23:59:59', true, false),

('Calculus Introduction', 'Limits and derivatives basics', 1, '2025-03-01 11:00:00', '2025-03-15 23:59:59', false, false),

  

-- Science Quizzes (Teacher ID 2)

('Chemistry Basics', 'Introduction to atoms and molecules', 2, '2025-02-05 14:00:00', '2025-02-20 23:59:59', true, false),

('Physics Fundamentals', 'Basic physics concepts and formulas', 2, '2025-02-15 15:00:00', '2025-03-01 23:59:59', true, false),

('Biology Essentials', 'Cell structure and function', 2, '2025-03-05 16:00:00', '2025-03-20 23:59:59', true, false),

  

-- History Quizzes (Teacher ID 3)

('Ancient Civilizations', 'Egypt, Greece, and Rome', 3, '2025-02-08 13:00:00', '2025-02-23 23:59:59', true, false),

('World War II', 'Major events and figures of WWII', 3, '2025-02-20 14:00:00', '2025-03-05 23:59:59', true, false),

  

-- English Quizzes (Teacher ID 4)

('Shakespeare Literature', 'Hamlet and Macbeth analysis', 4, '2025-02-12 12:00:00', '2025-02-27 23:59:59', true, false),

('Grammar Fundamentals', 'Parts of speech and sentence structure', 4, '2025-03-02 13:00:00', '2025-03-17 23:59:59', true, false),

  

-- Computer Science Quizzes (Teacher ID 5)

('Programming Basics', 'Introduction to programming concepts', 5, '2025-02-18 16:00:00', '2025-03-03 23:59:59', true, false),

('Database Design', 'SQL and database fundamentals', 5, '2025-03-08 17:00:00', '2025-03-23 23:59:59', true, false);

  

-- =====================================================

-- 5. INSERT QUESTIONS

-- =====================================================

-- Math Questions
INSERT INTO questions ("Text", "CorrectAnswer", "Options") VALUES

-- Algebra Questions
('What is the value of x in the equation 2x + 5 = 13?', '4', ARRAY['2', '3', '4', '5']),
('Simplify the expression: 3x + 2y - x + 4y', '2x + 6y', ARRAY['2x + 6y', '4x + 6y', '2x + 2y', '4x + 2y']),
('Solve for y: 2y - 8 = 12', '10', ARRAY['4', '6', '8', '10']),
('What is the slope of the line y = 3x + 2?', '3', ARRAY['2', '3', '5', '6']),
('Factor the expression: x² - 4', '(x + 2)(x - 2)', ARRAY['(x + 2)(x - 2)', '(x + 4)(x - 4)', '(x + 1)(x - 4)', '(x + 2)(x + 2)']),

-- Geometry Questions
('What is the area of a circle with radius 5?', '25π', ARRAY['10π', '25π', '50π', '100π']),
('How many sides does a hexagon have?', '6', ARRAY['4', '5', '6', '8']),
('What is the sum of angles in a triangle?', '180°', ARRAY['90°', '180°', '270°', '360°']),
('What is the perimeter of a square with side length 4?', '16', ARRAY['8', '12', '16', '20']),
('What is the volume of a cube with side length 3?', '27', ARRAY['9', '18', '27', '36']),

-- Science Questions
('What is the chemical symbol for gold?', 'Au', ARRAY['Ag', 'Au', 'Fe', 'Cu']),
('What is the atomic number of carbon?', '6', ARRAY['4', '6', '8', '12']),
('What is the formula for water?', 'H₂O', ARRAY['CO₂', 'H₂O', 'NaCl', 'O₂']),
('What is the speed of light?', '3 × 10⁸ m/s', ARRAY['3 × 10⁶ m/s', '3 × 10⁷ m/s', '3 × 10⁸ m/s', '3 × 10⁹ m/s']),
('What is the largest planet in our solar system?', 'Jupiter', ARRAY['Mars', 'Saturn', 'Jupiter', 'Neptune']),

-- History Questions
('Who was the first emperor of Rome?', 'Augustus', ARRAY['Julius Caesar', 'Augustus', 'Nero', 'Constantine']),
('In what year did World War II end?', '1945', ARRAY['1943', '1944', '1945', '1946']),
('Who was the leader of Nazi Germany?', 'Adolf Hitler', ARRAY['Benito Mussolini', 'Adolf Hitler', 'Joseph Stalin', 'Winston Churchill']),
('What was the capital of the Byzantine Empire?', 'Constantinople', ARRAY['Rome', 'Athens', 'Constantinople', 'Alexandria']),
('When did the United States declare independence?', '1776', ARRAY['1775', '1776', '1777', '1783']),

-- English Questions
('Who wrote "Romeo and Juliet"?', 'William Shakespeare', ARRAY['Charles Dickens', 'William Shakespeare', 'Jane Austen', 'Mark Twain']),
('What is a metaphor?', 'A comparison without using like or as', ARRAY['A comparison using like or as', 'A comparison without using like or as', 'A word that sounds like what it means', 'A word that means the opposite']),
('What is the past tense of "go"?', 'went', ARRAY['goed', 'went', 'gone', 'going']),
('What is a synonym for "happy"?', 'joyful', ARRAY['sad', 'angry', 'joyful', 'tired']),
('What is the plural of "child"?', 'children', ARRAY['childs', 'children', 'childes', 'child']),

-- Computer Science Questions
('What does HTML stand for?', 'HyperText Markup Language', ARRAY['High Tech Modern Language', 'HyperText Markup Language', 'Home Tool Markup Language', 'Hyperlink Text Markup']),
('What is the primary function of RAM?', 'Temporary storage', ARRAY['Permanent storage', 'Temporary storage', 'Processing', 'Display']),
('What does SQL stand for?', 'Structured Query Language', ARRAY['Simple Query Language', 'Structured Query Language', 'Standard Query Language', 'System Query Language']),
('What is a variable in programming?', 'A container for storing data', ARRAY['A type of computer', 'A container for storing data', 'A programming language', 'A database']),
('What does CPU stand for?', 'Central Processing Unit', ARRAY['Computer Personal Unit', 'Central Processing Unit', 'Central Program Utility', 'Computer Processing Unit']);


-- =====================================================

-- 6. INSERT QUIZ-QUESTION RELATIONSHIPS

-- =====================================================

  

-- Algebra Basics Quiz (Quiz ID 1) - Questions 1-5

INSERT INTO "quizQuestions" ("QuizId", "QuestionId") VALUES

(1, 1), (1, 2), (1, 3), (1, 4), (1, 5);

  

-- Geometry Fundamentals Quiz (Quiz ID 2) - Questions 6-10

INSERT INTO "quizQuestions" ("QuizId", "QuestionId") VALUES

(2, 6), (2, 7), (2, 8), (2, 9), (2, 10);

  

-- Chemistry Basics Quiz (Quiz ID 4) - Questions 11-13

INSERT INTO "quizQuestions" ("QuizId", "QuestionId") VALUES

(4, 11), (4, 12), (4, 13);

  

-- Physics Fundamentals Quiz (Quiz ID 5) - Questions 14-15

INSERT INTO "quizQuestions" ("QuizId", "QuestionId") VALUES

(5, 14), (5, 15);

  

-- Ancient Civilizations Quiz (Quiz ID 7) - Questions 16-18

INSERT INTO "quizQuestions" ("QuizId", "QuestionId") VALUES

(7, 16), (7, 18), (7, 19);

  

-- Shakespeare Literature Quiz (Quiz ID 9) - Questions 21-22

INSERT INTO "quizQuestions" ("QuizId", "QuestionId") VALUES

(9, 21), (9, 22);

  

-- Programming Basics Quiz (Quiz ID 11) - Questions 26-28

INSERT INTO "quizQuestions" ("QuizId", "QuestionId") VALUES

(11, 26), (11, 27), (11, 28);

  

-- =====================================================

-- 7. INSERT QUIZ SUBMISSIONS

-- =====================================================

  

-- Student 1 (Alice) - Multiple submissions

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 1, '2025-02-10 14:30:00', 'Submitted', 80),

(2, 1, '2025-02-20 15:45:00', 'Submitted', 90),

(4, 1, '2025-02-12 16:20:00', 'Submitted', 85),

(7, 1, '2025-02-15 13:10:00', 'Submitted', 75);

  

-- Student 2 (Bob) - Some Submitted, some in progress

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 2, '2025-02-11 10:15:00', 'Submitted', 70),

(2, 2, '2025-02-21 11:30:00', 'InProgress', NULL),

(4, 2, '2025-02-13 14:45:00', 'Submitted', 80),

(5, 2, '2025-02-25 16:00:00', 'Saved', NULL);

  

-- Student 3 (Carol) - Good performance

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 3, '2025-02-09 09:20:00', 'Submitted', 95),

(2, 3, '2025-02-19 10:35:00', 'Submitted', 88),

(4, 3, '2025-02-11 12:50:00', 'Submitted', 92),

(7, 3, '2025-02-14 15:25:00', 'Submitted', 85);

  

-- Student 4 (Dave) - Mixed performance

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 4, '2025-02-12 13:40:00', 'Submitted', 60),

(2, 4, '2025-02-22 14:55:00', 'Submitted', 75),

(4, 4, '2025-02-14 16:10:00', 'InProgress', NULL);

  

-- Student 5 (Eve) - Excellent performance

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 5, '2025-02-08 08:30:00', 'Submitted', 100),

(2, 5, '2025-02-18 09:45:00', 'Submitted', 95),

(4, 5, '2025-02-10 11:00:00', 'Submitted', 98),

(7, 5, '2025-02-13 13:15:00', 'Submitted', 90);

  

-- Student 6 (Frank) - Average performance

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 6, '2025-02-13 15:20:00', 'Submitted', 75),

(2, 6, '2025-02-23 16:35:00', 'Submitted', 80),

(4, 6, '2025-02-15 17:50:00', 'Submitted', 78);

  

-- Student 7 (Grace) - Some submissions

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 7, '2025-02-14 10:25:00', 'Submitted', 85),

(4, 7, '2025-02-16 12:40:00', 'Submitted', 88);

  

-- Student 8 (Henry) - Few submissions

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 8, '2025-02-15 14:55:00', 'Submitted', 70),

(2, 8, '2025-02-25 16:10:00', 'InProgress', NULL);

  

-- Student 9 (Iris) - Good performance

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 9, '2025-02-16 09:15:00', 'Submitted', 90),

(2, 9, '2025-02-26 10:30:00', 'Submitted', 85),

(4, 9, '2025-02-18 12:45:00', 'Submitted', 92);

  

-- Student 10 (Jack) - Mixed performance

INSERT INTO "quizSubmissions" ("QuizId", "StudentId", "SubmissionDate", "SubmissionStatus", "Score") VALUES

(1, 10, '2025-02-17 11:20:00', 'Submitted', 65),

(2, 10, '2025-02-27 12:35:00', 'Submitted', 80),

(4, 10, '2025-02-19 14:50:00', 'Saved', NULL);

  

-- =====================================================

-- 8. INSERT ANSWERS (Sample answers for Submitted submissions)

-- =====================================================

  

-- Alice's answers for Algebra Basics Quiz (Submission ID 1)

INSERT INTO answers ("QuestionId", "QuizSubmissionId", "SelectedAnswer") VALUES

(1, 1, '4'),
(2, 1, '2x + 6y'),
(3, 1, '10'),
(4, 1, '3'),
(5, 1, '2x + 2y'); 

  

-- Alice's answers for Geometry Fundamentals Quiz (Submission ID 2)

INSERT INTO answers ("QuestionId", "QuizSubmissionId", "SelectedAnswer") VALUES

(6, 2, '25π'),
(7, 2, '6'),
(8, 2, '180°'),
(9, 2, '16'),
(10, 2, '27');

  

-- Bob's answers for Algebra Basics Quiz (Submission ID 3)

INSERT INTO answers ("QuestionId", "QuizSubmissionId", "SelectedAnswer") VALUES

(1, 3, '3'),
(2, 3, '2x + 6y'),
(3, 3, '10'),
(4, 3, '3'),
(5, 3, '2x + 2y'); 

  

-- Carol's answers for Algebra Basics Quiz (Submission ID 4)

INSERT INTO answers ("QuestionId", "QuizSubmissionId", "SelectedAnswer") VALUES

(1, 4, '4'),
(2, 4, '2x + 6y'),
(3, 4, '10'),
(4, 4, '3'),
(5, 4, '(x + 2)(x - 2)');

  

-- Eve's answers for Algebra Basics Quiz (Submission ID 5)

INSERT INTO answers ("QuestionId", "QuizSubmissionId", "SelectedAnswer") VALUES

(1, 5, '4'),
(2, 5, '2x + 6y'),
(3, 5, '10'),
(4, 5, '3'),
(5, 5, '(x + 2)(x - 2)');


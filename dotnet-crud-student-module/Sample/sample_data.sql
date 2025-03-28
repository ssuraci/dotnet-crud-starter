
-- Sample data for student table
INSERT INTO student (id, birth_date, email, first_name, last_name, school_id)
VALUES 
(1, '2000-01-01', 'student1@example.com', 'John', 'Doe', 1),
(2, '2001-02-02', 'student2@example.com', 'Jane', 'Smith', 1),
(3, '2002-03-03', 'student3@example.com', 'Jim', 'Beam', 2);

-- Sample data for course table
INSERT INTO course (id, description, end_date, start_date, title, teacher_id, repository_id)
VALUES 
(1, 'Introduction to Programming', '2023-12-31', '2023-01-01', 'Programming 101', 1, 1),
(2, 'Advanced Mathematics', '2023-12-31', '2023-01-01', 'Math 201', 2, 2),
(3, 'History of Art', '2023-12-31', '2023-01-01', 'Art History', 3, 3);

-- Sample data for enrolment table
INSERT INTO enrolment (id, enrolment_date, course_id, student_id)
VALUES 
(1, '2023-01-15', 1, 1),
(2, '2023-01-16', 2, 2),
(3, '2023-01-17', 3, 3),
(4, '2023-01-18', 1, 2),
(5, '2023-01-19', 2, 3);

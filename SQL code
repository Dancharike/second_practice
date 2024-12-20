#CREATE DATABASE AS;

#USE AS;

CREATE TABLE user_roles (
    role_id INT AUTO_INCREMENT PRIMARY KEY,
    role_name ENUM('Admin', 'Lecturer', 'Student') NOT NULL
);

CREATE TABLE admins (
    admin_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL
);

CREATE TABLE lecturers (
    lecturer_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL
);

CREATE TABLE student_groups (
    group_id INT AUTO_INCREMENT PRIMARY KEY,
    group_name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE students (
    student_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    group_id INT,
    FOREIGN KEY (group_id) REFERENCES student_groups(group_id) ON DELETE SET NULL
);

CREATE TABLE users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(50) NOT NULL,
    role_id INT NOT NULL,
    role_specific_id INT,
    FOREIGN KEY (role_id) REFERENCES user_roles(role_id) ON DELETE CASCADE
);

CREATE TABLE courses (
    course_id INT AUTO_INCREMENT PRIMARY KEY,
    course_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE lecturer_courses (
    lecturer_course_id INT AUTO_INCREMENT PRIMARY KEY,
    lecturer_id INT NOT NULL,
    course_id INT NOT NULL,
    FOREIGN KEY (lecturer_id) REFERENCES lecturers(lecturer_id) ON DELETE CASCADE,
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE
);

CREATE TABLE group_courses (
    group_course_id INT AUTO_INCREMENT PRIMARY KEY,
    group_id INT NOT NULL,
    course_id INT NOT NULL,
    FOREIGN KEY (group_id) REFERENCES student_groups(group_id) ON DELETE CASCADE,
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE
);

CREATE TABLE grade_categories (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(50) NOT NULL UNIQUE,
    weight INT CHECK (weight BETWEEN 0 AND 100)
);

CREATE TABLE subjects (
    subject_id INT AUTO_INCREMENT PRIMARY KEY,
    subject_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE lecturer_subjects (
    lecturer_id INT NOT NULL,
    subject_id INT NOT NULL,
    FOREIGN KEY (lecturer_id) REFERENCES lecturers(lecturer_id) ON DELETE CASCADE,
    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id) ON DELETE CASCADE,
    PRIMARY KEY (lecturer_id, subject_id)
);

CREATE TABLE grades (
    grade_id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT NOT NULL,
    subject_id INT,
    category_id INT NOT NULL,
    grade_value INT CHECK (grade_value BETWEEN 1 AND 10),
    FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES grade_categories(category_id) ON DELETE CASCADE,
    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id)
);

CREATE TABLE course_subjects (
    course_subject_id INT AUTO_INCREMENT PRIMARY KEY,
    course_id INT NOT NULL,
    subject_id INT NOT NULL,
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE,
    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id) ON DELETE CASCADE
);

CREATE TABLE subject_grades (
    grade_id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT NOT NULL,
    subject_id INT NOT NULL,
    lecturer_id INT NOT NULL,
    grade_value INT CHECK (grade_value BETWEEN 1 AND 10),
    FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE,
    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id) ON DELETE CASCADE,
    FOREIGN KEY (lecturer_id) REFERENCES lecturers(lecturer_id) ON DELETE CASCADE
);

CREATE TABLE student_subject_grades (
    id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT NOT NULL,
    subject_id INT NOT NULL,
    grade_value INT CHECK (grade_value BETWEEN 1 AND 10),
    FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE,
    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id) ON DELETE CASCADE
);

INSERT INTO user_roles (role_name) VALUES ('Admin'), ('Lecturer'), ('Student');
INSERT INTO grade_categories (category_name, weight) VALUES ('Homework', 10), ('Practice', 20), ('Control', 20), ('Exam', 50);
INSERT INTO users (user_id, username, password, role_id, role_specific_id) VALUES (1, 'Vaidas', 'Liubinas', 1, 1);
INSERT INTO admins (admin_id, first_name, last_name) VALUES (1, 'Vaidas', 'Liubinas');

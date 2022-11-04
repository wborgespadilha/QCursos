CREATE TABLE MasterAdmin(
	id INT NOT NULL,
    registry INT NOT NULL,
	cpf VARCHAR2(11) NOT NULL,
	password VARCHAR2(50) NOT NULL,

	constraint MASTERADMIN_PK PRIMARY KEY (id));

CREATE sequence MASTERADMIN_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_MASTERADMIN_ID
  before insert on MasterAdmin
  for each row
begin
  select MASTERADMIN_ID_SEQ.nextval into :NEW.id from dual;
end;

CREATE sequence EMPLOYEE_REGISTRY_SEQ
START WITH 10011
MINVALUE 10011
INCREMENT BY 11;

CREATE OR REPLACE TRIGGER BI_MASTERADMIN_REGISTRY
  before insert on MASTERADMIN
  for each row
begin
  select EMPLOYEE_REGISTRY_SEQ.nextval into :NEW.registry from dual;
end;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Institutions (
	id INT NOT NULL,
	name VARCHAR2(80) NOT NULL,
	constraint INSTITUTIONS_PK PRIMARY KEY (id));

CREATE sequence INSTITUTIONS_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_INSTITUTIONS_ID
  before insert on Institutions
  for each row
begin
  select INSTITUTIONS_ID_SEQ.nextval into :NEW.id from dual;
end;

--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Admins (
	id INT NOT NULL,
    registry INT NOT NULL,
	cpf VARCHAR2(11) NOT NULL,
	password VARCHAR2(50) NOT NULL,
    fk_institution INT NOT NULL,
	constraint ADMINS_PK PRIMARY KEY (id));

CREATE sequence ADMINS_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_ADMINS_ID
  before insert on Admins
  for each row
begin
  select ADMINS_ID_SEQ.nextval into :NEW.id from dual;
end;


CREATE OR REPLACE TRIGGER BI_ADMINS_REGISTRY
  before insert on ADMINS
  for each row
begin
  select EMPLOYEE_REGISTRY_SEQ.nextval into :NEW.registry from dual;
end;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Courses (
	id INT NOT NULL,
	name VARCHAR2(80) NOT NULL,
	fk_institution INT NOT NULL,
	constraint COURSES_PK PRIMARY KEY (id));

CREATE sequence COURSES_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_COURSES_ID
  before insert on Courses
  for each row
begin
  select COURSES_ID_SEQ.nextval into :NEW.id from dual;
end;

--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Classes (
	id INT NOT NULL,
	name VARCHAR2(10) NOT NULL,
	fk_course INT NOT NULL,
	fk_teacher INT,
	constraint CLASSES_PK PRIMARY KEY (id));

CREATE sequence CLASSES_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_CLASSES_ID
  before insert on Classes
  for each row
begin
  select CLASSES_ID_SEQ.nextval into :NEW.id from dual;
end;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Teachers (
	id INT NOT NULL,
	name VARCHAR2(100) NOT NULL,
    registry INT NOT NULL,
	cpf VARCHAR2(11) NOT NULL,
	password VARCHAR2(50) NOT NULL,
    fk_institution INT,
	constraint TEACHERS_PK PRIMARY KEY (id));

CREATE sequence TEACHERS_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_TEACHERS_ID
  before insert on Teachers
  for each row
begin
  select TEACHERS_ID_SEQ.nextval into :NEW.id from dual;
end;

CREATE OR REPLACE TRIGGER BI_TEACHERS_REGISTRY
  before insert on TEACHERS
  for each row
begin
  select EMPLOYEE_REGISTRY_SEQ.nextval into :NEW.registry from dual;
end;



--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Students (
	id INT NOT NULL,
    fk_institution INT NOT NULL,
	name VARCHAR2(100) NOT NULL,
	registry INT NOT NULL,
	born_date DATE NOT NULL,
	constraint STUDENTS_PK PRIMARY KEY (id));

CREATE sequence STUDENTS_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_STUDENTS_ID
  before insert on Students
  for each row
begin
  select STUDENTS_ID_SEQ.nextval into :NEW.id from dual;
end;

CREATE sequence STUDENTS_REGISTRY_SEQ
START WITH 502
MINVALUE 502
INCREMENT BY 1;

CREATE OR REPLACE TRIGGER BI_STUDENTS_REGISTRY
  before insert on Students
  for each row
begin
  select STUDENTS_REGISTRY_SEQ.nextval into :NEW.registry from dual;
end;

CREATE TABLE StudentsClasses(
	id INT NOT NULL, 
	fk_student INT NOT NULL,
	fk_class INT NOT NULL,
    constraint STUDENTSCLASSES_PK PRIMARY KEY (id));
    
CREATE sequence STUDENTSCLASSES_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_STUDENTSCLASSES_ID
  before insert on STUDENTSCLASSES
  for each row
begin
  select STUDENTSCLASSES_ID_SEQ.nextval into :NEW.id from dual;
end;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE TestsTypes(
	id INT NOT NULL,
	name VARCHAR2(50) NOT NULL,
	constraint TESTSTYPES_PK PRIMARY KEY (id));

CREATE sequence TESTSTYPES_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_TESTSTYPES_ID
  before insert on TestTypes
  for each row
begin
  select TESTSTYPES_ID_SEQ.nextval into :NEW.id from dual;
end;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Tests (
	id INT NOT NULL,
	name VARCHAR2(80) NOT NULL,
	test_date DATE NOT NULL,
	fk_type INT NOT NULL,
	fk_class INT NOT NULL,
	constraint TESTS_PK PRIMARY KEY (id));

CREATE sequence TESTS_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_TESTS_ID
  before insert on Tests
  for each row
begin
  select TESTS_ID_SEQ.nextval into :NEW.id from dual;
end;

CREATE TABLE AppliedTests(
	id INT NOT NULL,
    grade NUMBER,
    fk_student INT NOT NULL,
   	fk_test INT NOT NULL,
    CONSTRAINT APPLIEDTESTS_PK PRIMARY KEY(id));

CREATE SEQUENCE APLLIEDATESTS_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_APPLIEDTESTS_ID
  before insert on AppliedTests
  for each row
begin
  select APLLIEDATESTS_ID_SEQ.nextval into :NEW.id from dual;
end;

--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Lessons(
	id INT NOT NULL,
    lesson_date DATE,
    fk_class INT NOT NULL,
    constraint LESSON_PK PRIMARY KEY(id));
    
CREATE SEQUENCE LESSONS_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_LESSONS_ID_ID
  before insert on Lessons
  for each row
begin
  select LESSONS_ID_SEQ.nextval into :NEW.id from dual;
end;


CREATE TABLE Attendance (
	id INT NOT NULL,
	absence INT,
	fk_student INT NOT NULL,
	fk_lesson INT NOT NULL,
	constraint ATTENDANCE_PK PRIMARY KEY (id));

CREATE sequence ATTENDANCE_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_ATTENDANCE_ID
  before insert on Attendance
  for each row
begin
  select ATTENDANCE_ID_SEQ.nextval into :NEW.id from dual;
end;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Averages (
	id INT NOT NULL,
    grade NUMBER NOT NULL,
	situation VARCHAR2(30) NOT NULL,
    frequency NUMBER,
	fk_student INT NOT NULL,
	fk_class INT NOT NULL,
	constraint AVERAGES_PK PRIMARY KEY (id));

CREATE sequence AVERAGES_ID_SEQ;

CREATE OR REPLACE TRIGGER BI_AVERAGES_ID
  before insert on Averages
  for each row
begin
  select AVERAGES_ID_SEQ.nextval into :NEW.id from dual;
end;

SELECT * FROM masteradmin;

--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE Admins ADD CONSTRAINT Admins_fk0 FOREIGN KEY (fk_institution) REFERENCES Institutions(id);

ALTER TABLE Courses ADD CONSTRAINT Courses_fk0 FOREIGN KEY (fk_institution) REFERENCES Institutions(id);

ALTER TABLE Teachers ADD CONSTRAINT Teachers_fk0 FOREIGN KEY (fk_institution) REFERENCES Institutions(id);

ALTER TABLE Classes ADD CONSTRAINT Classes_fk0 FOREIGN KEY (fk_course) REFERENCES Courses(id);
ALTER TABLE Classes ADD CONSTRAINT Classes_fk1 FOREIGN KEY (fk_teacher) REFERENCES Teachers(id);

ALTER TABLE Students ADD CONSTRAINT Students_fk0 FOREIGN KEY (fk_institution) REFERENCES Institutions(id);

ALTER TABLE StudentsClasses ADD CONSTRAINT StudentsClasses_fk0 FOREIGN KEY (fk_student) REFERENCES Students(id);
ALTER TABLE StudentsClasses ADD CONSTRAINT StudentsClasses_fk1 FOREIGN KEY (fk_class) REFERENCES Classes(id);

ALTER TABLE Tests ADD CONSTRAINT Tests_fk0 FOREIGN KEY (fk_type) REFERENCES TestsTypes(id);
ALTER TABLE Tests ADD CONSTRAINT Tests_fk1 FOREIGN KEY (fk_class) REFERENCES Classes(id);

ALTER TABLE AppliedTests ADD CONSTRAINT AppliedTests_fk0 FOREIGN KEY (fk_student) REFERENCES Students  (id);
ALTER TABLE AppliedTests ADD CONSTRAINT AppliedTests_fk1 FOREIGN KEY (fk_test) REFERENCES Tests(id)

ALTER TABLE Attendance ADD CONSTRAINT Attendance_fk0 FOREIGN KEY (fk_student) REFERENCES Students(id);
ALTER TABLE Attendance ADD CONSTRAINT Attendance_fk1 FOREIGN KEY (fk_lesson) REFERENCES Lessons(id);

ALTER TABLE Averages ADD CONSTRAINT Averages_fk0 FOREIGN KEY (fk_student) REFERENCES Students(id);
ALTER TABLE Averages ADD CONSTRAINT Averages_fk1 FOREIGN KEY (fk_class) REFERENCES Classes(id);

-- Create institutions
CREATE OR REPLACE PROCEDURE createInstitution(i_name VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkInstitution(i_name);
    
	IF valid = TRUE THEN
		INSERT INTO institutions (name) VALUES (i_name);
        msg := 'Instituição cadastrada!';
        httpstatus := 200;
    ELSE
    	msg := 'Nome de já em uso!';   
        httpstatus := 400;
  	END IF;  
END;


-- Update institutions
CREATE OR REPLACE PROCEDURE updateInstitution(i_id INT, new_name VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
  	valid := checkInstitution(new_name);
    
  	IF valid = TRUE THEN
     	UPDATE institutions SET name = new_name WHERE id = i_id;
        msg := 'Instituição atualizada!';
        httpstatus := 200;
    ELSE
    	msg := 'Nome de instituição inválido!'; 
        httpstatus := 400;
	END IF; 
END;


-- Delete institution
CREATE OR REPLACE PROCEDURE deleteInstitution(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS 
  	valid BOOLEAN;
BEGIN
  	valid := checkDeleteInstitution(i_id);  
    
  	IF valid = TRUE THEN
  		DELETE institutions WHERE id = i_id;
        msg := 'Instituição removida!';
        httpstatus := 200;
    ELSE 
    	msg := 'Instituição não pode ser removida! Existem cursos vinculados a esta instituição!';
        httpstatus := 400;
  	END IF;
END;

    


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create test type
CREATE OR REPLACE PROCEDURE createTestType(i_name VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkTestType(i_name);  
    
	IF valid = TRUE THEN
		INSERT INTO teststypes (name) VALUES (i_name);
        msg := 'Tipo de teste cadastrado!';
        httpstatus := 200;
    ELSE 
    	msg := 'Tipo de teste já existe!';
        httpstatus := 400;
	END IF;  
END;

-- Update test type
CREATE OR REPLACE PROCEDURE updateTestType(i_id INT, new_name VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
  	valid BOOLEAN;
BEGIN
  	valid := checkTestType(new_name); 
    
  	IF valid = TRUE THEN
     	UPDATE teststypes SET name = new_name WHERE id = i_id;
        msg := 'Tipo de teste atualizado!';
        httpstatus := 200;
    ELSE
    	msg := 'Nome de tipo de teste já em uso!';
        httpstatus := 400;
  	END IF; 
END;

-- Delete test type
CREATE OR REPLACE PROCEDURE deleteTestType(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkDeleteTestType(i_id);	
    
    IF valid = TRUE THEN
  		DELETE teststypes WHERE id = i_id;
        msg := 'Tipo de teste removido';
        httpstatus := 200;
    ELSE 
    	msg := 'Tipo de teste não pode ser removido! Existem testes vinculados a este tipo!';
        httpstatus := 400;
    END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create institution admin
CREATE OR REPLACE PROCEDURE createInstitutionAdmin(institution_id INT, admin_cpf VARCHAR2, password VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
  	valid BOOLEAN;
  	validCPF BOOLEAN;
BEGIN
	valid := checkAdmin(institution_id, admin_cpf, password);

  	IF valid = TRUE THEN
		validCPF := checkCPF(admin_cpf);
		IF validCPF = TRUE THEN
			INSERT INTO admins (fk_institution, cpf, password) VALUES (institution_id, admin_cpf, password);
            msg := 'Admin cadastrado!';
            httpstatus := 200;
        ELSE
        	msg := 'CPF inválido!';
            httpstatus := 400;
		END IF;
    ELSE
    	msg := 'Admin já existe';
        httpstatus := 400;
	END IF; 
END;

-- Update institution admin
CREATE OR REPLACE PROCEDURE updateInstitutionAdmin(i_id INT, new_institution_id INT, new_cpf VARCHAR2, new_password VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	institution_admin_exists NUMBER;
	valid BOOLEAN;
BEGIN
	institution_admin_exists := 0;
	SELECT COUNT(*) INTO institution_admin_exists FROM admins WHERE id = i_id; 
  
	IF new_cpf IS NOT NULL THEN
		IF new_password IS NOT NULL THEN
			IF institution_admin_exists != 0 THEN
				valid := checkCPF(new_cpf);
				IF valid = TRUE THEN       
					UPDATE admins 
					SET 
					fk_institution = new_institution_id,
					cpf = new_cpf,
					password = new_password
					WHERE id = i_id;
                    msg := 'Admin atualizado';
                    httpstatus := 200;
                ELSE   
                    msg := 'CPF inválido';
                    httpstatus := 400;
                END IF;
            ELSE
                msg := 'Admin não existe';
                httpstatus := 404;
			END IF;
         ELSE
            msg := 'Senha inválida';
            httpstatus := 400;
    	 END IF;
    ELSE
		msg := 'CPF Inválido';
		httpstatus := 400;
  	END IF; 
END;

-- Delete institution admin
CREATE OR REPLACE PROCEDURE deleteInstitutionAdmin(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	institution_admin_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO institution_admin_exists FROM admins WHERE id = i_id;
	
    IF institution_admin_exists != 0 THEN
		DELETE admins WHERE id = i_id;
        msg := 'Admin removido!';
        httpstatus := 200;
	ELSE
    	msg := 'Admin não existe!';
        httpstatus := 404;
    END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create course
CREATE OR REPLACE PROCEDURE createCourse(i_name VARCHAR2, institution_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkCourse(i_name, institution_id);
     
	IF valid = TRUE THEN
		INSERT INTO courses (name, fk_institution) VALUES (i_name, institution_id);
		msg := 'Curso cadastrado!';
        httpstatus := 200;
	ELSE 
  		msg := 'Curso já existe';
        httpstatus := 400;
	END IF; 
END;

-- Update course
CREATE OR REPLACE PROCEDURE updateCourse(i_id INT, new_name VARCHAR2, institution_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	course_exists NUMBER;
BEGIN
	course_exists := 0;
	SELECT COUNT(*) INTO course_exists 
    FROM courses 
    INNER JOIN institutions ON institutions.id = courses.fk_institution
    WHERE institutions.id = institution_id;

	IF new_name IS NOT NULL THEN  
		IF institution_id IS NOT NULL THEN
			IF course_exists != 0 THEN
				UPDATE courses SET name = new_name WHERE id = i_id;
                msg := 'Curso atualizado!';
                httpstatus := 200;
             ELSE
                msg := 'Curso não existe!';
                httpstatus := 404;
            END IF;
        ELSE
            msg := 'Instituição inválida!';
            httpstatus := 400;
		END IF;
    ELSE
        msg := 'Nome de curso já em uso!';
        httpstatus := 400;
	END IF; 
END;

-- Delete course
CREATE OR REPLACE PROCEDURE deleteCourse(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkDeleteCourse(i_id);   
    
    IF valid = TRUE THEN
    	DELETE courses WHERE id = i_id;
        msg := 'Curso removido!';
        httpstatus := 200;
  	ELSE
    	msg := 'Curso não pode ser removido! Existem turmas vinculadas a ele!';
        httpstatus := 400;
    END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create class
CREATE OR REPLACE PROCEDURE createClass(i_name VARCHAR2, course_id INT, teacher_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkClass(i_name, course_id, teacher_id);  
    
	IF valid = TRUE THEN
		INSERT INTO classes (name, fk_course, fk_teacher) VALUES (i_name, course_id, teacher_id);
     	msg := 'Turma criada';
        httpstatus := 200;
    ELSE
    	msg := 'Turma já existe';
        httpstatus := 400;
	END IF; 
END;

-- Update class
CREATE OR REPLACE PROCEDURE updateClass(i_id INT, new_name VARCHAR, new_teacher_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	class_exists NUMBER;
BEGIN
	class_exists := 0;
	SELECT COUNT(*) INTO class_exists FROM classes WHERE id = i_id;
  
	IF new_name IS NOT NULL THEN
		IF class_exists != 0 THEN
			UPDATE classes 
			SET 
			name = new_name,
			fk_teacher = new_teacher_id
			WHERE id = i_id;
			msg := 'Turma atualizada';
			httpstatus := 200;
        ELSE 
			msg := 'Turma não existe'; 
			httpstatus := 404;    
        END IF;
    ELSE
    	msg := 'Nome inválido!';
        httpstatus := 400;
  	END IF;
END;

-- Delete class
CREATE OR REPLACE PROCEDURE deleteClass(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkDeleteClass(i_id);
    
    IF valid = TRUE THEN
    	DELETE classes WHERE id = i_id;
        msg := 'Turma removida!';
        httpstatus := 200;
	ELSE
     	msg := 'Turma não pode ser removida! Existem alunos vinculados a esta turma';
        httpstatus := 404;
    END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create teacher
CREATE OR REPLACE PROCEDURE createTeacher(i_name VARCHAR2, teacher_cpf VARCHAR2, password VARCHAR2, i_institution_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
	validCPF BOOLEAN;
BEGIN
	valid := checkTeacher(i_name, teacher_cpf, password, i_institution_id);

	IF valid = TRUE THEN
		validCPF := checkCPF(teacher_cpf);
		IF validCPF = TRUE THEN   
			INSERT INTO teachers (name, cpf, password, fk_institution) VALUES (i_name, teacher_cpf, password, i_institution_id);
			msg := 'Professor cadastrado!';
            httpstatus := 200;
      	ELSE
        	msg := 'CPF inválido!';
            httpstatus := 400;
		END IF;
    ELSE
    	msg := 'Professor já cadastrado!';
        httpstatus := 400;
	END IF;  
END;

-- Update teacher
CREATE OR REPLACE PROCEDURE updateTeacher(i_id INT, new_name VARCHAR2, new_cpf VARCHAR2, new_password VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	teacher_exists NUMBER;
	valid BOOLEAN;
BEGIN
	teacher_exists := 0;
	SELECT COUNT(*) INTO teacher_exists FROM teachers WHERE id = i_id;
  
	IF teacher_exists != 0 THEN
		IF new_name IS NOT NULL THEN
			IF new_cpf IS NOT NULL THEN	
				valid := checkCPF(new_cpf);
    			IF valid = TRUE THEN   
      				UPDATE teachers
                    SET
                    name = new_name,
                    cpf = new_cpf,
                    password = new_password
                    WHERE id = i_id;
                    msg := 'Professor atualizado!';
                    httpstatus := 200;
    			ELSE
                	msg := 'CPF inválido!';
                    httpstatus := 400;
                END IF;
            ELSE
            	msg := 'CPF inválido!';
                httpstatus := 400;
            END IF;
      	ELSE
        	msg := 'Nome inválido';
            httpstatus := 400;
		END IF;
	ELSE 
    	msg := 'Professor não existe!';
        httpstatus := 404;
	END IF;
END;

-- Delete teacher
CREATE OR REPLACE PROCEDURE deleteTeacher(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	teacher_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO teacher_exists FROM teachers WHERE id = i_id;
    
    IF teacher_exists != 0 THEN
    	UPDATE classes
    	SET
    	fk_teacher = NULL
    	WHERE fk_teacher = i_id;
    
		DELETE teachers WHERE id = i_id;
        msg := 'Professor removido!';
        httpstatus := 200;
	ELSE
    	msg := 'Professor não existe!';
        httpstatus := 404;
    END IF;
END;


SELECT * FROM attendance;

--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create student
CREATE OR REPLACE PROCEDURE createStudent(i_institution_id INT, i_name VARCHAR2, born_date DATE, msg OUT VARCHAR2, httpstatus OUT INT)
AS	
BEGIN
    
	IF i_institution_id IS NULL THEN
    	msg := 'Instituição inválida';
        httpstatus := 400;
	ELSIF i_name IS NULL THEN
		msg := 'Nome inválido!';
        httpstatus := 400;
	ELSIF born_date IS NULL THEN
		msg := 'Data de nascimento inválida!';
        httpstatus := 400;
	ELSE
		INSERT INTO students (fk_institution, name, born_date ) VALUES (i_institution_id, i_name, born_date);
        msg := 'Estudante cadastrado!';
        httpstatus := 200;
	END IF;
END;

-- Update student
CREATE OR REPLACE PROCEDURE updateStudent(i_id INT, new_name VARCHAR2, new_bo0rn_date DATE, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	student_exists NUMBER;
BEGIN
	student_exists := 0;
	SELECT COUNT(*) INTO student_exists FROM students WHERE id = i_id;
  
	IF new_name IS NULL THEN
    	msg := 'Negado: nome inválido!';
   	 	httpstatus := 400;
  	ELSIF new_born_date IS NULL THEN
    	msg := 'Negado: data de nascimento inválida!';
    	httpstatus := 400;
  	ELSE
    	UPDATE students
    	SET
    	name = new_name,
    	born_date = new_born_date
    	WHERE id = i_id;
    	msg := 'Estudante atualizado!';
   	 httpstatus := 200;
  END IF;
END;

-- Delete student
CREATE OR REPLACE PROCEDURE deleteStudent(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	student_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO student_exists FROM students WHERE id = i_id;
    
	IF student_exists != 0 THEN      	
    	DELETE studentsClasses WHERE fk_student = i_id;
    	DELETE attendance WHERE fk_student = i_id;
    	DELETE appliedTests WHERE fk_student = i_id;
   		DELETE averages WHERE fk_student = i_id;
         
		DELETE students WHERE id = i_id;
	    msg := 'Estudante removido!';
        httpstatus := 200;
    ELSE 
    	msg := 'Estudante não existe!';
        httpstatus := 404;
    END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create test
CREATE OR REPLACE PROCEDURE createTest(i_name VARCHAR2, i_test_date DATE, i_type_id INT, i_class_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid VARCHAR2(30);
BEGIN
	valid := checkTest(i_name, i_test_date, i_type_id, i_class_id);
  
	IF valid = 'Válido' THEN      
    	INSERT INTO tests (name, test_date, fk_type, fk_class) VALUES (i_name, i_test_date, i_type_id, i_class_id);
                     
		msg := 'Teste aplicado!';
        httpstatus := 200;
	ELSE
		msg := valid;
        httpstatus := 400;
	END IF;   
END;

-- Create applied test
CREATE OR REPLACE PROCEDURE createAppliedTest(i_id INT)
AS
BEGIN

	IF i_id IS NOT NULL THEN           
		DECLARE
			cursor_student studentsClasses.fk_student%TYPE;
      
			CURSOR cursor_applyTests IS           
			SELECT studentsClasses.fk_student 
			FROM studentsClasses 
			INNER JOIN tests ON tests.fk_class = studentsClasses.fk_class
            WHERE tests.id = i_id;       			
		BEGIN 
			OPEN cursor_applyTests;
				LOOP
					FETCH cursor_applyTests INTO cursor_student;
                    
					EXIT WHEN cursor_applyTests%NOTFOUND;
   	
					INSERT INTO appliedTests (grade, fk_student, fk_test) VALUES (0, cursor_student, i_id);
                        
				END LOOP;
			CLOSE cursor_applyTests;  
		END;        
	END IF;
END;


-- Update test
CREATE OR REPLACE PROCEDURE updateAppliedTest(i_id INT, i_grade NUMBER, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	test_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO test_exists FROM appliedTests WHERE id = i_id;
  	
	IF test_exists != 0 THEN
    	IF i_grade >= 0 AND i_grade <= 10 THEN
        	UPDATE AppliedTests
			SET 
			grade = i_grade
			WHERE id = i_id;
        	msg := 'Teste atualizado!';
        	httpstatus := 200;
        ELSE
        	msg := 'Nota inválida!';
        	httpstatus := 400;
        END IF;       
    ELSE
    	msg := 'Teste não existe!';
        httpstatus := 404;
    END IF;
END;

-- Delete test
CREATE OR REPLACE PROCEDURE deleteTest(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	tests_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO tests_exists FROM tests WHERE id = i_id;


	IF tests_exists != 0 THEN
     	DELETE appliedTests WHERE fk_test = i_id;
		DELETE tests WHERE id = i_id;       
    	msg := 'Teste removido!';
        httpstatus := 200;
    ELSE
    	msg := 'Teste não existe!';
        httpstatus := 400;
    END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create average
CREATE OR REPLACE PROCEDURE createAverage(i_grade NUMBER, i_situation VARCHAR, i_frequency NUMBER, i_student_id INT, i_class_id INT, msg OUT VARCHAR2)
AS	
average_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO average_exists FROM averages WHERE fk_student = i_student_id AND fk_class = i_class_id;
	
    IF average_exists != 0 THEN
    	DELETE averages WHERE fk_student = i_student_id AND fk_class = i_class_id;
    END IF;
    
	INSERT INTO averages (grade, situation, frequency, fk_student, fk_class) VALUES (TRUNC(i_grade, 2), i_situation, i_frequency, i_student_id, i_class_id);
    msg := 'Média gerada com sucesso!';
END;



--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create lesson
CREATE OR REPLACE PROCEDURE createLesson(i_lesson_date DATE, i_class_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	valid BOOLEAN;
BEGIN
	valid := checkLesson(i_lesson_date, i_class_id);
    
    IF valid = TRUE THEN
    	INSERT INTO Lessons (lesson_date, fk_class) VALUES (i_lesson_date, i_class_id);
        msg := 'Aula registrada!';
        httpstatus := 200;
    ELSE
    	msg := 'Informações inválidas ou turma não possui alunos, verifique';
        httpstatus := 400;
    END IF;
END;


-- Create attendance
CREATE OR REPLACE PROCEDURE createAttendance(i_id INT)
AS
BEGIN
	IF i_id IS NOT NULL THEN             
		DECLARE
			cursor_student studentsClasses.fk_student%TYPE;
      
			CURSOR cursor_applyAttendance IS           
			SELECT studentsClasses.fk_student 
			FROM studentsClasses 
			INNER JOIN lessons ON lessons.fk_class = studentsClasses.fk_Class
			WHERE lessons.id = i_id;           			
		BEGIN 
			OPEN cursor_applyAttendance;
				LOOP
					FETCH cursor_applyAttendance INTO cursor_student;
                    
					EXIT WHEN cursor_applyAttendance%NOTFOUND;
   	
					INSERT INTO attendance (absence, fk_student, fk_lesson) VALUES (0, cursor_student, i_id);
                        
				END LOOP;
			CLOSE cursor_applyAttendance;  
		END;        
	END IF;
END;

-- Update attendance
CREATE OR REPLACE PROCEDURE updateAttendance(i_id INT, i_absence INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	attendance_exists NUMBER;
BEGIN 
	SELECT COUNT(*) INTO attendance_exists FROM attendance WHERE id = i_id;
  
	IF attendance_exists = 0 THEN
		msg := 'Presença não existe!';
        httpstatus := 404;
	ELSE
		UPDATE attendance
		SET
		absence = i_absence
		WHERE id = i_id;
    	msg := 'Presença atualizada';
        httpstatus := 200;
  	END IF;
END;

-- Delete lesson
CREATE OR REPLACE PROCEDURE deleteLesson(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	lesson_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO lesson_exists FROM lessons WHERE id = i_id;
	
    IF lesson_exists != 0 THEN
    	DELETE attendance WHERE fk_lesson = i_id;
        DELETE lessons WHERE id = i_id;    			
    	msg := 'Presença deletada!';
        httpstatus := 200;
    ELSE
    	msg := 'Presença não existe';
        httpstatus := 404;
    END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create student Class
CREATE OR REPLACE PROCEDURE createStudentClass(i_student_id INT, i_class_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
    student_exists NUMBER;
	class_exists NUMBER;
    student_class_exists NUMBER;
    
    student_course_exists NUMBER;
    
    class_institution NUMBER;
    student_institution NUMBER;  
BEGIN 
	SELECT COUNT(*) INTO student_course_exists 
    FROM studentsClasses 
    INNER JOIN classes ON classes.id = studentsClasses.fk_class  
    INNER JOIN courses ON courses.id = classes.fk_course
    WHERE courses.id = (SELECT courses.id FROM classes INNER JOIN courses ON courses.id = classes.fk_course WHERE classes.id = i_class_id AND studentsClasses.fk_student = i_student_id);

	SELECT COUNT(*) INTO student_exists FROM students WHERE id = i_student_id;
    SELECT COUNT(*) INTO class_exists FROM classes WHERE id = i_class_id;
    
    SELECT COUNT(*) INTO student_class_exists 
    FROM studentsClasses 
    WHERE fk_student = i_student_id AND fk_class = i_class_id;
	
    SELECT fk_institution INTO student_institution FROM students WHERE id = i_student_id;
    
    SELECT courses.fk_institution INTO class_institution 
    FROM classes INNER JOIN courses ON courses.id = classes.fk_course
    WHERE classes.id = i_class_id;
    
    
	IF student_exists != 0 THEN
		IF class_exists != 0 THEN
        	IF student_course_exists = 0 THEN
        		IF student_class_exists = 0 THEN
        			IF class_institution = student_institution THEN
						INSERT INTO studentsclasses (fk_student, fk_class) VALUES (i_student_id, i_class_id);
           				msg := 'Estudante Registrado!';
            			httpstatus := 200; 
					ELSE
            			msg := 'Turma não pertence a escola do aluno!';
            			httpstatus := 400; 
            		END IF; 
           		ELSE
            		msg := 'Estudante já está registrado nesta turma';
                	httpstatus := 400;
            	END IF;     
            ELSE
                msg := 'Estudante já está registrado neste curso';
                httpstatus := 400;
            END IF;         
        ELSE 
           	msg := 'Turma não existe!';
            httpstatus := 404;
		END IF;
    ELSE
    	msg := 'Estudante não existe';
        httpstatus := 404;
	END IF;
END;

-- Update student Class
CREATE OR REPLACE PROCEDURE updateStudentClass(i_id INT, i_student_id INT, i_class_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
    student_exists NUMBER;
	student_class_exists NUMBER;
    old_class INT;
    old_course INT;
    new_course INT;
BEGIN 

	SELECT COUNT(*) INTO student_exists FROM students WHERE id = i_student_id;
    SELECT COUNT(*) INTO student_class_exists FROM classes WHERE id = i_class_id;
		
    SELECT fk_class INTO old_class FROM studentsClasses WHERE id = i_id;   
    
    SELECT courses.id INTO old_course FROM classes INNER JOIN courses ON classes.fk_course = courses.id WHERE classes.id = old_class;
    SELECT courses.id INTO new_course FROM classes INNER JOIN courses ON classes.fk_course = courses.id WHERE classes.id = i_class_id;

      
    IF new_course = old_course THEN
        IF student_exists != 0 THEN
			IF student_class_exists != 0 THEN
				UPDATE studentsClasses
				SET 
				fk_class = i_class_id
				WHERE studentsClasses.id = i_id; 
				msg := 'Estudante atualizado!';
				httpstatus := 200;                 
            ELSE 
				msg := 'Turma não existe!';
				httpstatus := 404;
            END IF;
        ELSE
            msg := 'Estudante não existe';
            httpstatus := 404;
        END IF;
	ELSE
		msg := 'Turma não possui o curso que o aluno está matriculado!';
        httpstatus := 400;	
	END IF;
END;

-- Delete student class
CREATE OR REPLACE PROCEDURE deleteStudentClass(i_id INT, msg OUT VARCHAR2, httpstatus OUT INT)
AS
    student_exists NUMBER;
BEGIN 
	SELECT COUNT(*) INTO student_exists FROM studentsClasses WHERE id = i_id;

	IF student_exists != 0 THEN
		DELETE studentsClasses WHERE id = i_id;
        msg := 'Estudante removido!'; 
        httpstatus := 200;
	ELSE
    	msg := 'Estudante não existe!';
        httpstatus := 404;
  	END IF;
END;




--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Frequency
CREATE OR REPLACE PROCEDURE generateFrequency(i_student_id INT, i_class_id INT, student_frequency OUT NUMBER)
AS
	total_lessons INT;
    total_attendance INT;
BEGIN
	SELECT COUNT(*)
    INTO total_lessons
	FROM lessons 
    WHERE fk_class = i_class_id;
    
    SELECT COUNT(*)
    INTO total_attendance 
    FROM attendance
    INNER JOIN lessons ON lessons.id = attendance.fk_lesson
    WHERE attendance.fk_student = i_student_id AND lessons.fk_class = i_class_id AND absence != 1;
    
    IF total_lessons != 0 THEN
    	student_frequency := TRUNC(((total_attendance/total_lessons)*100), 2); 
    ELSE
    	student_frequency := 0;
    END IF; 
END;



-- Certificate attendance
CREATE OR REPLACE PROCEDURE  generateCertificateAttendance(i_student_id INT, i_class_id INT, i_student_name OUT VARCHAR2, i_registry OUT NUMBER, 
							  							   i_born_date OUT DATE, i_institution_name OUT VARCHAR2, i_course_name OUT VARCHAR2, 
                              							   i_class_name OUT VARCHAR2, msg OUT VARCHAR2, httpstatus OUT INT)
AS
	st_name VARCHAR2(100);
    st_registry NUMBER;    
    st_born_date DATE;
       
    it_name VARCHAR2(80);
    co_name VARCHAR2(80);
	cl_name VARCHAR2(10);
    
    student_exists NUMBER;
BEGIN
	
	SELECT COUNT(*) INTO student_exists FROM students INNER JOIN studentsClasses ON studentsClasses.fk_student = students.id WHERE studentsClasses.fk_class = i_class_id;
	   
    IF student_exists != 0 THEN
		SELECT students.name INTO st_name FROM students WHERE students.id = i_student_id;
    	SELECT students.registry INTO st_registry FROM students WHERE students.id = i_student_id;
    	SELECT students.born_date INTO st_born_date FROM students WHERE students.id = i_student_id;
    
    	SELECT institutions.name INTO it_name FROM institutions INNER JOIN students ON students.fk_institution = institutions.id WHERE students.id = i_student_id;   
    	SELECT courses.name INTO co_name FROM courses INNER JOIN classes ON classes.fk_course = courses.id WHERE classes.id = i_class_id;   
   		SELECT classes.name INTO cl_name FROM classes WHERE classes.id = i_class_id;
       
    	i_student_name := st_name;
    	i_registry := st_registry;
    	i_born_date := st_born_date;
    
    	i_institution_name := it_name;
    	i_course_name := co_name;
    	i_class_name := cl_name;
    
    	msg := 'Frequência gerada com sucesso!';
    	httpstatus := 200;
	ELSE
    	msg := 'Estudante não encontrado na turma informada!';   
        httpstatus := 404;
    END IF;
END;

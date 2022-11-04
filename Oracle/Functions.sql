-- Fnction Check CPF
CREATE OR REPLACE FUNCTION checkCPF(p_cpf IN VARCHAR2) RETURN BOOLEAN
IS
     m_total     NUMBER := 0;
     m_digit     NUMBER := 0;
     total       NUMBER := 0;
     first_digit NUMBER := 0;
BEGIN
     
	IF LENGTH(p_cpf) != 11 THEN
    	RETURN FALSE;
	END IF;

	first_digit := substr(p_cpf,1, 1);
     
	FOR i IN 1..11 LOOP
		total := total + substr(p_cpf,i,1);
	END LOOP;
     
          
	IF (total/11) = first_digit THEN
       RETURN FALSE;
	END IF;

	FOR i IN 1..9 LOOP
		m_total := m_total + substr(p_cpf,i,1) * (11 - i);
	END LOOP;

	m_digit := 11 - mod(m_total,11);

	IF m_digit > 9 THEN
    	m_digit := 0;
	END IF;

	IF m_digit != substr(p_cpf,10,1) THEN
        RETURN FALSE;
	END IF;

	m_digit := 0;
	m_total  := 0;

    FOR i IN 1..10 LOOP
		m_total := m_total + substr(p_cpf,i,1) * (12 - i);
	END LOOP;

	m_digit := 11 - mod(m_total,11);

	IF m_digit > 9 THEN
		m_digit := 0;
	END IF;

	IF m_digit != substr(p_cpf,11,1) THEN
		RETURN FALSE;
	END IF;

	RETURN TRUE;
END;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Institution Is Valid
CREATE OR REPLACE FUNCTION checkInstitution(i_name VARCHAR2) RETURN BOOLEAN
AS
	institution_exists NUMBER;
BEGIN
	institution_exists := 0;
	SELECT COUNT(*) INTO institution_exists FROM institutions  WHERE name = i_name;

    IF i_name IS NULL THEN
		RETURN FALSE;
	ELSIF institution_exists != 0 THEN
		RETURN FALSE;
	ELSE 
		RETURN TRUE;
	END IF;  
END; 


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Test Type Is Valid
CREATE OR REPLACE FUNCTION checkTestType(i_name VARCHAR2) RETURN BOOLEAN
AS
	test_type_exists NUMBER;
BEGIN
	test_type_exists := 0;
	SELECT COUNT(*) INTO test_type_exists FROM teststypes WHERE name = i_name;

	IF i_name IS NULL THEN
		RETURN FALSE;
	ELSIF test_type_exists != 0 THEN
		RETURN FALSE;
	ELSE
		RETURN TRUE;
	END IF;   
END;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Admin Is Valid
CREATE OR REPLACE FUNCTION checkAdmin(institution_id INT, admin_cpf VARCHAR2, password VARCHAR2) RETURN BOOLEAN
AS
	institution_admin_exists NUMBER;
BEGIN
	institution_admin_exists := 0;
	SELECT COUNT(*) INTO institution_admin_exists FROM admins WHERE cpf = admin_cpf;

	IF admin_cpf IS NULL THEN
     	RETURN FALSE;
  	ELSIF password IS NULL THEN  
     	RETURN FALSE;
 	 ELSIF institution_admin_exists != 0 THEN     
     	RETURN FALSE;
  	ELSE 
     	RETURN TRUE;
  	END IF; 
END;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Course Is Vald
CREATE OR REPLACE FUNCTION checkCourse(i_name VARCHAR2, institution_id INT) RETURN BOOLEAN
AS
  	course_exists NUMBER;
BEGIN
 	course_exists := 0;
  	SELECT COUNT(*) INTO course_exists 
    FROM institutions 
    INNER JOIN courses ON courses.fk_institution = institution_id
    WHERE courses.name = i_name;

  	IF i_name IS NULL THEN   
    	 RETURN FALSE;
  	ELSIF institution_id IS NULL THEN
    	 RETURN FALSE;
 	ELSIF course_exists != 0 THEN
    	RETURN FALSE;
 	ELSE
		RETURN TRUE;
	END IF;
END;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Class Is Vald
CREATE OR REPLACE FUNCTION checkClass(i_name VARCHAR2, course_id INT, teacher_id INT) RETURN BOOLEAN
AS
  	class_exists NUMBER;
BEGIN
  	class_exists := 0;
  	SELECT COUNT(*) INTO class_exists 
    FROM classes 
    INNER JOIN courses ON courses.id = classes.fk_course
    INNER JOIN institutions ON institutions.id = courses.fk_institution
    WHERE classes.name = i_name;

  	IF i_name IS NULL THEN     
		RETURN FALSE;
	ELSIF course_id IS NULL THEN
		RETURN FALSE;
 	ELSIF class_exists != 0 THEN
     	RETURN FALSE;
    ELSE 
    	RETURN TRUE;
  	END IF;
END;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Teacher Is Valid
CREATE OR REPLACE FUNCTION checkTeacher(i_name VARCHAR2, teacher_cpf VARCHAR2, password VARCHAR2, i_institution_id INT) RETURN BOOLEAN
AS
	teacher_exists NUMBER;
    institution_exists NUMBER;
BEGIN
	teacher_exists := 0;
	SELECT COUNT(*) INTO teacher_exists 
    FROM teachers 
    WHERE teachers.fk_institution = i_institution_id AND teachers.cpf = teacher_cpf;
    
    institution_exists := 0;
    SELECT COUNT(*) INTO institution_exists FROM institutions WHERE id = i_institution_id;
  
  	IF i_name IS NULL THEN
 		RETURN FALSE;
  	ELSIF teacher_cpf IS NULL THEN
  		RETURN FALSE;
  	ELSIF password IS NULL THEN
  		RETURN FALSE;
  	ELSIF teacher_exists != 0THEN
  		RETURN FALSE;
    ELSIF institution_exists = 0 THEN
    	RETURN FALSE;
    ELSE
    	RETURN TRUE;
  	END IF;
END;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Test Is Valid
CREATE OR REPLACE FUNCTION checkTest(i_name VARCHAR2, i_test_date DATE, i_type_id INT, i_class_id INT) RETURN VARCHAR2
AS
	class_exists NUMBER;
    class_students NUMBER;
    type_exists NUMBER;
    test_exists NUMBER;
BEGIN
	SELECT COUNT(*) INTO class_exists FROM classes WHERE id = i_class_id;
    SELECT COUNT(*) INTO type_exists FROM testsTypes WHERE id = i_type_id;
	
    SELECT COUNT(*) INTO class_students FROM studentsClasses WHERE fk_class = i_class_id;
    
	SELECT COUNT(*) INTO test_exists FROM tests WHERE name = i_name AND fk_class = i_class_id AND fk_type = i_type_id;
    
    
	IF i_name IS NULL THEN
		RETURN 'Nome inválido!';
 	ELSIF i_test_date IS NULL THEN
    	RETURN 'Data inválida!';
  	ELSIF i_type_id IS NULL OR type_exists = 0 THEN
    	RETURN 'Tipo inválido!';
  	ELSIF i_class_id IS NULL OR class_exists = 0 THEN
    	RETURN 'Turma inválida!';
  	ELSIF test_exists != 0 THEN
    	RETURN 'Turma já possui este teste!';
 	ELSIF class_students = 0 THEN
    	RETURN 'Turma não possui estudantes!';
    ELSE
  		RETURN 'Válido';
  	END IF;
END;


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check Lesson Is Valid
CREATE OR REPLACE FUNCTION checkLesson(i_lesson_date DATE, i_class_id INT) RETURN BOOLEAN
AS
	lesson_exists NUMBER;
    class_exists NUMBER;
    class_students NUMBER;
BEGIN
	lesson_exists := 0;
    SELECT COUNT(*) INTO lesson_exists 
    FROM Lessons
    WHERE fk_class = i_class_id AND lesson_date = i_lesson_date;
        
    class_exists := 0;
    SELECT COUNT(*) INTO class_exists FROM classes WHERE id = i_class_id;
    
    SELECT COUNT(*) INTO class_students FROM studentsClasses WHERE fk_class = i_class_id;
    
    
    IF i_lesson_date IS NULL OR lesson_exists != 0 THEN
    	RETURN FALSE;
    ELSIF i_class_id IS NULL OR class_exists = 0 THEN
     	RETURN FALSE;
 	ELSIF class_students = 0 THEN
    	RETURN FALSE;
    ELSE
    	RETURN TRUE;
    END IF;
END;

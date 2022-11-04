-- Check delete institution is valid
CREATE OR REPLACE FUNCTION checkDeleteInstitution(i_institution_id INT) RETURN BOOLEAN
AS
 courses_vinculated NUMBER;
BEGIN
	SELECT COUNT(*) INTO courses_vinculated 
    FROM courses 
    INNER JOIN institutions ON institutions.id = courses.fk_institution
    WHERE institutions.id = i_institution_id;
    
    IF courses_vinculated = 0 THEN
    	RETURN TRUE;
    ELSE
    	RETURN FALSE;
    END IF;
END;



--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check delete type test is valid
CREATE OR REPLACE FUNCTION checkDeleteTestType(i_test_type_id INT) RETURN BOOLEAN
AS
	tests_vinculated NUMBER;
BEGIN
	SELECT COUNT(*) INTO tests_vinculated
    FROM testsTypes 
    INNER JOIN tests ON tests.fk_type = testsTypes.id
    WHERE testsTypes.id = i_test_type_id;

    IF tests_vinculated = 0 THEN
    	RETURN TRUE;
    ELSE
    	RETURN FALSE;
    END IF;
END;



--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check delete course is valid
CREATE OR REPLACE FUNCTION checkDeleteCourse(i_course_id INT) RETURN BOOLEAN
AS
	classes_vinculated NUMBER;
BEGIN
	SELECT COUNT(*) INTO classes_vinculated
    FROM courses
    INNER JOIN classes ON classes.fk_course = courses.id
    WHERE courses.id = i_course_id;

    IF classes_vinculated = 0 THEN
    	RETURN TRUE;
    ELSE
    	RETURN FALSE;
    END IF;
END;



--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------
-- Check delete class is valid
CREATE OR REPLACE FUNCTION checkDeleteClass(i_class_id INT) RETURN BOOLEAN
AS
	students_vinculated NUMBER;
BEGIN
	SELECT COUNT(*) INTO students_vinculated
    FROM studentsClasses
    WHERE fk_class = i_class_id;

    IF students_vinculated = 0 THEN
    	RETURN TRUE;
    ELSE
    	RETURN FALSE;
    END IF;
END;

-- Update class on tests 
CREATE OR REPLACE TRIGGER AU_updateTestClass_FK_CLASS
AFTER UPDATE ON studentsClasses
FOR EACH ROW
BEGIN
	UPDATE attendance
    SET 
    fk_class = :new.fk_class
    WHERE fk_class = :old.fk_class AND fk_student = fk_student;
	
    UPDATE tests
    SET
    fk_class = :new.fk_class
    WHERE fk_class = :old.fk_class AND fk_student = fk_student;
END;

namespace API_QCursos.Models.Costumized
{
    public class StudentClassesWithNames
    {


        private int id;
        private int fk_student;
        private int fk_class;
        private int fk_course;
        private string course_name = "";
        private string class_name = "";


        public int Id { get => id; set => id = value; }
        public int Fk_student { get => fk_student; set => fk_student = value; }
        public int Fk_class { get => fk_class; set => fk_class = value; }
        public string Course_name { get => course_name; set => course_name = value; }
        public string Class_name { get => class_name; set => class_name = value; }
        public int Fk_course { get => fk_course; set => fk_course = value; }
    }
}

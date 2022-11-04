namespace API_QCursos.Models.Costumized
{
    public class ClassWithCourse
    {

        private int id;
        private string name = "";
        private int fk_course;
        private int fk_teacher;
        private string course_name = "";

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Fk_course { get => fk_course; set => fk_course = value; }
        public int Fk_teacher { get => fk_teacher; set => fk_teacher = value; }
        public string Course_name { get => course_name; set => course_name = value; }
    }
}

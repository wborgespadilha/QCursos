namespace API_QCursos.Models.Costumized
{
    public class StudentsWithClassAndCourse
    {
        private int id;
        private string name = "";
        private int registry;
        private int class_id;
        private string class_name = "";
        private int course_id;
        private string course_name = "";

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Registry { get => registry; set => registry = value; }
        public int Class_id { get => class_id; set => class_id = value; }
        public string Class_name { get => class_name; set => class_name = value; }
        public int Course_id { get => course_id; set => course_id = value; }
        public string Course_name { get => course_name; set => course_name = value; }
    }
}

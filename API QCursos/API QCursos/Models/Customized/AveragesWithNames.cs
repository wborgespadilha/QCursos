namespace API_QCursos.Models.Costumized
{
    public class AveragesWithNames
    {
        private int id;
        private float grade;
        private float frequency;
        private string situation = "";
        private string student_name = "";
        private string class_name = "";
        private string course_name = "";

        public int Id { get => id; set => id = value; }
        public string Situation { get => situation; set => situation = value; }
        public string Student_name { get => student_name; set => student_name = value; }
        public string Class_name { get => class_name; set => class_name = value; }
        public float Grade { get => grade; set => grade = value; }
        public string Course_name { get => course_name; set => course_name = value; }
        public float Frequency { get => frequency; set => frequency = value; }
    }
}

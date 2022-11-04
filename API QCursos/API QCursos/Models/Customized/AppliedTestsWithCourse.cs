namespace API_QCursos.Models.Costumized
{
    public class AppliedTestsWithCourse
    {
        private int id;
        private float grade;
        private int fk_student;
        private int fk_test;
        private DateTime test_date;
        private string test_name = "";
        private string testtype_name = "";
        private string class_name = "";
        private string course_name = "";
        private int class_id;
        private int course_id;

        public int Id { get => id; set => id = value; }
        public float Grade { get => grade; set => grade = value; }
        public int Fk_student { get => fk_student; set => fk_student = value; }
        public int Fk_test { get => fk_test; set => fk_test = value; }
        public DateTime Test_date { get => test_date; set => test_date = value; }
        public string Test_name { get => test_name; set => test_name = value; }
        public string Class_name { get => class_name; set => class_name = value; }
        public string Course_name { get => course_name; set => course_name = value; }
        public int Class_id { get => class_id; set => class_id = value; }
        public int Course_id { get => course_id; set => course_id = value; }
        public string Testtype_name { get => testtype_name; set => testtype_name = value; }
    }
}

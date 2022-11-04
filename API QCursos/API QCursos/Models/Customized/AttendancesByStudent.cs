namespace API_QCursos.Models.Customized
{
    public class AttendancesByStudent
    {
        private int id;
        private int absence;
        private int fk_student;
        private int fk_lesson;
        private DateTime lesson_date;
        private string class_name = "";
        private int class_id;
        private string course_name = "";
        private int course_id;

        public int Id { get => id; set => id = value; }
        public int Absence { get => absence; set => absence = value; }
        public int Fk_student { get => fk_student; set => fk_student = value; }
        public int Fk_lesson { get => fk_lesson; set => fk_lesson = value; }
        public DateTime Lesson_date { get => lesson_date; set => lesson_date = value; }
        public string Class_name { get => class_name; set => class_name = value; }
        public int Class_id { get => class_id; set => class_id = value; }
        public string Course_name { get => course_name; set => course_name = value; }
        public int Course_id { get => course_id; set => course_id = value; }
    }
}

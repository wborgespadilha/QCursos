namespace API_QCursos.Models.Raw
{
    public class Attendances
    {

        private int id;
        private int absence;
        private int fk_student;
        private int fk_lesson;

        public int Id { get => id; set => id = value; }
        public int Absence { get => absence; set => absence = value; }
        public int Fk_student { get => fk_student; set => fk_student = value; }
        public int Fk_lesson { get => fk_lesson; set => fk_lesson = value; }
    }
}

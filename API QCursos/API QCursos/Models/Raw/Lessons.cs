namespace API_QCursos.Models.Raw
{
    public class Lessons
    {
        private int id;
        private DateTime lesson_date;
        private int fk_class;

        public int Id { get => id; set => id = value; }
        public DateTime Lesson_date { get => lesson_date; set => lesson_date = value; }
        public int Fk_class { get => fk_class; set => fk_class = value; }
    }
}

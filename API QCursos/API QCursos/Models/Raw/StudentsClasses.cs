namespace API_QCursos.Models.Raw
{
    public class StudentsClasses
    {

        private int id;
        private int fk_student;
        private int fk_class;

        public int Id { get => id; set => id = value; }
        public int Fk_student { get => fk_student; set => fk_student = value; }
        public int Fk_class { get => fk_class; set => fk_class = value; }
    }
}

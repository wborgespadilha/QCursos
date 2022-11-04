namespace API_QCursos.Models.Raw
{
    public class AppliedTests
    {
        private int id;
        private double grade;
        private int fk_student;
        private int fk_test;

        public int Id { get => id; set => id = value; }
        public double Grade { get => grade; set => grade = value; }
        public int Fk_student { get => fk_student; set => fk_student = value; }
        public int Fk_test { get => fk_test; set => fk_test = value; }
    }
}

namespace API_QCursos.Models.Raw
{
    public class Averages
    {
        private int id;
        private float grade;
        private string situation = "";
        private int fk_student;
        private int fk_class;

        public int Id { get => id; set => id = value; }
        public string Situation { get => situation; set => situation = value; }
        public float Grade { get => grade; set => grade = value; }
        public int Fk_student { get => fk_student; set => fk_student = value; }
        public int Fk_class { get => fk_class; set => fk_class = value; }
    }
}

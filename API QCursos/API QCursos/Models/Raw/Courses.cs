namespace API_QCursos.Models.Raw
{
    public class Courses
    {

        private int id;
        private string name = "";
        private int fk_institution;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Fk_institution { get => fk_institution; set => fk_institution = value; }
    }
}

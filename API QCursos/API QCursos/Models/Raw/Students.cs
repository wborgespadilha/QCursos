namespace API_QCursos.Models.Raw
{
    public class Students
    {
        private int id;
        private string name = "";
        private int registry;
        private DateTime born_date;
        private int fk_institution;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Registry { get => registry; set => registry = value; }
        public DateTime Born_date { get => born_date; set => born_date = value; }
        public int Fk_institution { get => fk_institution; set => fk_institution = value; }
    }
}

namespace API_QCursos.Models.Raw
{
    public class Teachers
    {

        private int id;
        private string name = "";
        private string cpf = "";
        private string password = "";
        private int registry;
        private int fk_institution;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Password { get => password; set => password = value; }
        public int Fk_institution { get => fk_institution; set => fk_institution = value; }
        public int Registry { get => registry; set => registry = value; }
    }
}

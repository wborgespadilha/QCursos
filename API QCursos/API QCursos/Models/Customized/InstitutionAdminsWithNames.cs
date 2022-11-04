namespace API_QCursos.Models.Costumized
{
    public class InstitutionAdminsWithNames
    {

        private int id;
        private string cpf = "";
        private string password = "";
        private int registry;
        private int fk_institution;
        private string institution_name = "";

        public int Id { get => id; set => id = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Password { get => password; set => password = value; }
        public int Fk_institution { get => fk_institution; set => fk_institution = value; }
        public string Institution_name { get => institution_name; set => institution_name = value; }
        public int Registry { get => registry; set => registry = value; }
    }

}

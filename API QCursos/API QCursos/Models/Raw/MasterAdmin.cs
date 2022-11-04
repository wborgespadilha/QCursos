namespace API_QCursos.Models.Raw
{
    public class MasterAdmin
    {
        private int id;
        private string cpf = "";
        private string password = "";
        private int registry;

        public int Id { get => id; set => id = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Password { get => password; set => password = value; }
        public int Registry { get => registry; set => registry = value; }
    }
}

namespace API_QCursos.Models.Login
{
    public class Login
    {

        private int registry;
        private string password = "";

        public int Registry { get => registry; set => registry = value; }
        public string Password { get => password; set => password = value; }
    }
}

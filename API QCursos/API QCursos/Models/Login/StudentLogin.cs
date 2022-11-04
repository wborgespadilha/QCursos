namespace API_QCursos.Models.Login
{
    public class StudentLogin
    {
        private int registry;
        private DateTime born_date;

        public int Registry { get => registry; set => registry = value; }
        public DateTime Born_date { get => born_date; set => born_date = value; }
    }
}

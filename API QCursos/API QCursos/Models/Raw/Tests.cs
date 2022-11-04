namespace API_QCursos.Models.Raw
{
    public class Tests
    {

        private int id;
        private string name = "";
        private DateTime test_date;
        private int fk_type;
        private int fk_class;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public DateTime Test_Date { get => test_date; set => test_date = value; }
        public int Fk_type { get => fk_type; set => fk_type = value; }
        public int Fk_class { get => fk_class; set => fk_class = value; }
    }
}

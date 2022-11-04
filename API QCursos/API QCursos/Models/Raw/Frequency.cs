namespace API_QCursos.Models.Raw
{
    public class Frequency
    {
        private double frequencyrate;
        private string name = "";
        private int registry;
        private DateTime born_date;
        private string institution_name = "";
        private string course_name = "";
        private string class_name = "";

        public double Frequencyrate { get => frequencyrate; set => frequencyrate = value; }
        public string Name { get => name; set => name = value; }
        public int Registry { get => registry; set => registry = value; }
        public DateTime Born_date { get => born_date; set => born_date = value; }
        public string Institution_name { get => institution_name; set => institution_name = value; }
        public string Class_name { get => class_name; set => class_name = value; }
        public string Course_name { get => course_name; set => course_name = value; }
    }
}

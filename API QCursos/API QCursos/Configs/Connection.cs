using Oracle.ManagedDataAccess.Client;

namespace API_QCursos.Configs
{
    public class Connection
    {
        public OracleConnection Connect()
        {
            string connectionString = "DATA SOURCE = (DESCRIPTION ="
                + "(ADDRESS = (PROTOCOL = TCP)(HOST =192.168.15.18)(PORT=1521))"
                + "(CONNECT_DATA =(SERVER = DEDICATED)"
                + "(SERVICE_NAME = TREINAMENTO))));"
                + "User Id=aluno13;Password=aluno13";

            return new OracleConnection(connectionString);
        }
    }
}

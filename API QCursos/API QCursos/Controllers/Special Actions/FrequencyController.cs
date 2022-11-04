using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
namespace API_QCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController
    {
        [HttpPost("Student")]
        public List<Frequency> GetStudentFrequency(Int32 student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Student", student);

            var builder = new SqlBuilder();
            builder.Select("fk_class");
            builder.Where("fk_student = :Student", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM studentsclasses /**where**/");

            List<Int32> classesIds = new();
            classesIds.AddRange(connection.Query<Int32>(builderTemplate.RawSql, builderTemplate.Parameters).ToList());

            List<Frequency> frequencyList = new();

            foreach (Int32 Class in classesIds)
            {

                var frequencyParameters = new DynamicParameters();
                frequencyParameters.Add(":i_student_id", student, direction: System.Data.ParameterDirection.Input);
                frequencyParameters.Add(":i_class_id", Class, direction: System.Data.ParameterDirection.Input);
                frequencyParameters.Add(":student_frequency", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<float>("generateFrequency", frequencyParameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                string str = frequencyParameters.Get<string>(":student_frequency");
                double frequencyValue = Convert.ToDouble(str);

                var certificateParameters = new DynamicParameters();
                certificateParameters.Add(":i_student_id", student, direction: System.Data.ParameterDirection.Input);
                certificateParameters.Add(":i_class_id", Class, direction: System.Data.ParameterDirection.Input);
                certificateParameters.Add(":i_student_name", "", direction: System.Data.ParameterDirection.Output);
                certificateParameters.Add(":i_registry", "", direction: System.Data.ParameterDirection.Output);
                certificateParameters.Add(":i_born_date", "", System.Data.DbType.DateTime, direction: System.Data.ParameterDirection.Output);
                certificateParameters.Add(":i_institution_name", "", direction: System.Data.ParameterDirection.Output);
                certificateParameters.Add(":i_course_name", "", direction: System.Data.ParameterDirection.Output);
                certificateParameters.Add(":i_class_name", "", direction: System.Data.ParameterDirection.Output);
                certificateParameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                certificateParameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("generateCertificateAttendance", certificateParameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                Frequency frequency = new();

                frequency.Frequencyrate = frequencyValue;
                frequency.Name = certificateParameters.Get<string>(":i_student_name");
                frequency.Registry = Convert.ToInt32(certificateParameters.Get<string>(":i_registry"));
                frequency.Born_date = certificateParameters.Get<DateTime>(":i_born_date");
                frequency.Institution_name = certificateParameters.Get<string>(":i_institution_name");
                frequency.Course_name = certificateParameters.Get<string>(":i_course_name");
                frequency.Class_name = certificateParameters.Get<string>(":i_class_name");

                frequencyList.Add(frequency);

            }

            return frequencyList;
        }
    }
}

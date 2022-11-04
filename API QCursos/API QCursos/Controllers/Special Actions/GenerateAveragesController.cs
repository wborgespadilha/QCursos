using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;

namespace API_QCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateAveragesController : ControllerBase
    {

        [HttpGet("{Class}")]

        public IActionResult CreateAveragesByClass(int Class)
        {

            Connection c = new Connection();
            using var connection = c.Connect();

            String message = "";
            List<Int32> studentsList = new List<Int32>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Class", Class);

            var builder = new SqlBuilder();
            builder.Select("students.id");
            builder.InnerJoin("students ON students.id = studentsclasses.fk_student");
            builder.Where("studentsclasses.fk_class = :Class", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM studentsclasses /**innerjoin**/ /**where**/");

            studentsList.AddRange(connection.Query<Int32>(builderTemplate.RawSql, builderTemplate.Parameters).ToList());

            foreach(Int32 student in studentsList)
            {
                List<AppliedTests> testsList = new List<AppliedTests>();

                DynamicParameters testParameters = new DynamicParameters();
                testParameters.Add(":Student", student);
                testParameters.Add(":Class", Class);

                var testSqlBuilder = new SqlBuilder();
                testSqlBuilder.Select("appliedtests.grade");
                testSqlBuilder.InnerJoin("tests ON tests.id = appliedtests.fk_test");
                testSqlBuilder.Where("appliedtests.fk_student = :Student AND tests.fk_class = :Class", testParameters);

                var testBuilderTemplate = testSqlBuilder.AddTemplate("SELECT /**select**/ FROM appliedtests /**innerjoin**/ /**where**/");

                testsList.AddRange(connection.Query<AppliedTests>(testBuilderTemplate.RawSql, testBuilderTemplate.Parameters).ToList());

                int numTests = 0;
                double average = 0;

                foreach (AppliedTests appTest in testsList)
                {
                    numTests++;
                    average += appTest.Grade;
                }

                average = numTests == 0 ? 0 : (average / numTests);

                var frequencyParameters = new DynamicParameters();
                frequencyParameters.Add(":i_student_id", student, direction: System.Data.ParameterDirection.Input);
                frequencyParameters.Add(":i_class_id", Class, direction: System.Data.ParameterDirection.Input);
                frequencyParameters.Add(":student_frequency", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<float>("generateFrequency", frequencyParameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                string str = frequencyParameters.Get<string>(":student_frequency");
                double frequency = Convert.ToDouble(str);

                string situation = "";

                if (average >= 7 && frequency >= 75)
                {
                    situation = "Aprovado";
                }
                else
                {
                    situation = "Reprovado";
                }

                var averageParameters = new DynamicParameters();
                averageParameters.Add(":i_grade", average, direction: System.Data.ParameterDirection.Input);
                averageParameters.Add(":i_situation", situation, direction: System.Data.ParameterDirection.Input);
                averageParameters.Add(":i_frequency", frequency, direction: System.Data.ParameterDirection.Input);
                averageParameters.Add(":i_student_id", student, direction: System.Data.ParameterDirection.Input);
                averageParameters.Add(":i_class_id", Class, direction: System.Data.ParameterDirection.Input);
                averageParameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("CreateAverage", averageParameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                message = averageParameters.Get<string>(":msg");

            }

            return Ok(message);
        }
    }
}


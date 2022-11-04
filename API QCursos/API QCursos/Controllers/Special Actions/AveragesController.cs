using Dapper;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Costumized;

namespace API_QCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AveragesController : ControllerBase
    {

        [HttpGet("{Student}")]

        public IEnumerable<AveragesWithNames> ListAveragesByStudent(int Student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Student", Student);

            var builder = new SqlBuilder();
            builder.Select("averages.grade");
            builder.Select("averages.situation");
            builder.Select("averages.frequency");
            builder.Select("students.name AS student_name");
            builder.Select("classes.name AS class_name");
            builder.Select("courses.name AS course_name");
            builder.InnerJoin("students ON students.id = averages.fk_student");
            builder.InnerJoin("classes ON classes.id = averages.fk_class");
            builder.InnerJoin("courses ON classes.fk_course = courses.id");
            builder.Where("students.id = :Student", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM averages /**innerjoin**/ /**where**/");

            return connection.Query<AveragesWithNames>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();

        }
    }
}

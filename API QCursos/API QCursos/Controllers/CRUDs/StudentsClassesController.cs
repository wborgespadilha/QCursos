using Dapper;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Costumized;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsClassesController : ControllerBase
    {

        [HttpGet("Institution")]

        public IEnumerable<StudentClassesWithNames> ListStudentsClassesByInstitution(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Institution", Institution);

            var builder = new SqlBuilder();
            builder.Select("studentsclasses.id");
            builder.Select("studentsclasses.fk_student");
            builder.Select("studentsclasses.fk_class");
            builder.Select("courses.name AS course_name");
            builder.Select("classes.name AS class_name");
            builder.Select("courses.id AS fk_course");
            builder.InnerJoin("students ON studentsclasses.fk_student = students.id");
            builder.InnerJoin("classes ON studentsclasses.fk_class = classes.id");
            builder.InnerJoin("courses ON classes.fk_course = courses.id");
            builder.Where("courses.fk_institution = :Institution", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM studentsclasses /**innerjoin**/ /**where**/");

            return connection.Query<StudentClassesWithNames>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPost]
        public IActionResult CreateStudentClass([FromBody] StudentsClasses studentClasses)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_student_id", studentClasses.Fk_student, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_class_id", studentClasses.Fk_class, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateStudentClass", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

            String message = parameters.Get<string>(":msg");
            string httpStatusStr = parameters.Get<string>(":httpstatus");

            int httpStatus = Convert.ToInt32(httpStatusStr);

            switch (httpStatus)
            {
                case 200:
                    return Ok(message);

                case 400:
                    return BadRequest(message);

                case 404:
                    return NotFound(message);
            }

            return BadRequest(message);
        }

        [HttpPut]
        public IActionResult UpdateStudentClass([FromBody] StudentsClasses studentClasses)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", studentClasses.Id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_student_id", studentClasses.Fk_student, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_class_id", studentClasses.Fk_class, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("updateStudentClass", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

            String message = parameters.Get<string>(":msg");
            string httpStatusStr = parameters.Get<string>(":httpstatus");

            int httpStatus = Convert.ToInt32(httpStatusStr);

            switch (httpStatus)
            {
                case 200:
                    return Ok(message);

                case 400:
                    return BadRequest(message);

                case 404:
                    return NotFound(message);
            }

            return BadRequest(message);
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteStudentClass(int id)
        {
            try
            {

                Connection c = new Connection();
                using var connection = c.Connect();

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("deleteStudentClass", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                String message = parameters.Get<string>(":msg");
                string httpStatusStr = parameters.Get<string>(":httpstatus");

                int httpStatus = Convert.ToInt32(httpStatusStr);

                switch (httpStatus)
                {
                    case 200:
                        return Ok(message);

                    case 400:
                        return BadRequest(message);

                    case 404:
                        return NotFound(message);
                }

                return BadRequest(message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

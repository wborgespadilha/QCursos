using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Costumized;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]

    public class CoursesController : ControllerBase
    {

        [HttpGet]

        public IEnumerable<Courses> GetAllCourses(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            return connection.Query<Courses>("SELECT * FROM courses");

        }


        [HttpGet("Institution")]

        public IEnumerable<Courses> ListCoursesByInstitution(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Institution", Institution);

            var builder = new SqlBuilder();
            builder.Where("courses.fk_institution = :Institution", parameters);

            var builderTemplate = builder.AddTemplate("SELECT * FROM courses /**where**/");

            return connection.Query<Courses>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();

        }

        [HttpGet("Student")]

        public IEnumerable<Courses> ListCoursesByStudent(int student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Student", student);

            var builder = new SqlBuilder();
            builder.Select("courses.id");
            builder.Select("courses.name");
            builder.Select("courses.fk_institution");
            builder.InnerJoin("classes ON studentsclasses.fk_class = classes.id");
            builder.InnerJoin("courses ON classes.fk_course = courses.id");
            builder.Where("studentsclasses.fk_student = :Student", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM studentsclasses /**innerjoin**/ /**where**/");

            return connection.Query<Courses>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPost]

        public IActionResult CreateCourse([FromBody] Courses course)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_name", course.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":institution_id", course.Fk_institution, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("createCourse", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult UpdateCourse([FromBody] Courses course)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", course.Id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_name", course.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":institution_id", course.Fk_institution, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("UpdateCourse", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult DeleteCourse(int id)
        {
            try
            {

                Connection c = new Connection(); ;
                using var connection = c.Connect();

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("DeleteCourse", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Costumized;


namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClassesController : ControllerBase
    {

        [HttpGet("Institution")]

        public IEnumerable<Classes> ListClassesByInstitution(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Institution", Institution);

            var builder = new SqlBuilder();
            builder.Select("classes.id");
            builder.Select("classes.name");
            builder.Select("classes.fk_course");
            builder.Select("classes.fk_teacher");
            builder.InnerJoin("courses ON classes.fk_course = courses.id");
            builder.Where("courses.fk_institution  = :Institution", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM classes /**innerjoin**/ /**where**/");

            return connection.Query<Classes>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpGet("Teacher")]

        public IEnumerable<ClassWithCourse> ListClassesByTeacher(int Teacher)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Teacher", Teacher);

            var builder = new SqlBuilder();
            builder.Select("classes.id");
            builder.Select("classes.name");
            builder.Select("classes.fk_course");
            builder.Select("classes.fk_teacher");
            builder.Select("courses.name AS course_name");
            builder.InnerJoin("courses ON classes.fk_course = courses.id");
            builder.Where("classes.fk_teacher = :Teacher", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM classes /**innerjoin**/ /**where**/");

            return connection.Query<ClassWithCourse>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpGet("Student")]

        public IEnumerable<ClassWithCourseAndTeacher> ListClassesByStudent(int Student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Student", Student);

            var builder = new SqlBuilder();
            builder.Select("classes.id");
            builder.Select("classes.name");
            builder.Select("classes.fk_course");
            builder.Select("classes.fk_teacher");
            builder.Select("teachers.name AS teacher_name");
            builder.Select("courses.name AS course_name");
            builder.InnerJoin("courses ON classes.fk_course = courses.id");
            builder.InnerJoin("teachers ON teachers.id = classes.fk_teacher");
            builder.InnerJoin("studentsclasses ON studentsclasses.fk_class = classes.id");
            builder.Where("studentsclasses.fk_student = :Student", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM classes /**innerjoin**/ /**where**/");

            return connection.Query<ClassWithCourseAndTeacher>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPost]

        public IActionResult CreateClass([FromBody] Classes Class)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_name", Class.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":course_id", Class.Fk_course, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":teacher_id", Class.Fk_teacher, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateClass", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult UpdateClass([FromBody] Classes Class)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", Class.Id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_name", Class.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_teacher_id", Class.Fk_teacher, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("UpdateClass", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult DeleteClass(int id)
        {
            try
            {
                Connection c = new Connection();
                using var connection = c.Connect();

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("DeleteClass", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

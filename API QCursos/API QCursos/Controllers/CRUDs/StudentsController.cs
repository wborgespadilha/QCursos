using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Costumized;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentsController : ControllerBase
    {

        [HttpGet("Institution")]

        public IEnumerable<Students> ListStudentsByInstitution(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Institution", Institution);

            var builder = new SqlBuilder();
            builder.Where("fk_institution = :Institution", parameters);

            var builderTemplate = builder.AddTemplate("SELECT * FROM students /**where**/");

            return connection.Query<Students>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpGet("Student")]

        public IEnumerable<StudentsWithClassAndCourse> ListStudentClassmates(int Student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            List<Classes> commonClasses = new();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Student", Student);

            var builder = new SqlBuilder();
            builder.Select("classes.id");
            builder.InnerJoin("studentsclasses ON studentsclasses.fk_class = classes.id");
            builder.Where("studentsclasses.fk_student = :Student", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM classes /**innerjoin**/ /**where**/");

            commonClasses.AddRange(connection.Query<Classes>(builderTemplate.RawSql, builderTemplate.Parameters).ToList());

            List<StudentsWithClassAndCourse> classmates = new();

            foreach (Classes Class in commonClasses)
            {

                DynamicParameters classmatesParameters = new DynamicParameters();
                classmatesParameters.Add(":Class", Class.Id);
                classmatesParameters.Add(":Student",Student);

                var classmatesSqlBuilder = new SqlBuilder();
                classmatesSqlBuilder.Select("students.id");
                classmatesSqlBuilder.Select("students.name");
                classmatesSqlBuilder.Select("classes.id AS class_id");
                classmatesSqlBuilder.Select("classes.name AS class_name");
                classmatesSqlBuilder.Select("courses.id AS course_id");
                classmatesSqlBuilder.Select("courses.name AS course_name");
                classmatesSqlBuilder.InnerJoin("studentsclasses ON studentsclasses.fk_student = students.id");
                classmatesSqlBuilder.InnerJoin("classes ON classes.id = studentsclasses.fk_class");
                classmatesSqlBuilder.InnerJoin("courses ON courses.id = classes.fk_course");
                classmatesSqlBuilder.Where("studentsclasses.fk_class = :Class AND students.id != :Student", classmatesParameters);

                var classmatesBuilderTemplate = classmatesSqlBuilder.AddTemplate("SELECT /**select**/ FROM students /**innerjoin**/ /**where**/");

                classmates.AddRange(connection.Query<StudentsWithClassAndCourse>(classmatesBuilderTemplate.RawSql, classmatesBuilderTemplate.Parameters).ToList());

            }

            return classmates;

        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] Students student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_institution_id", student.Fk_institution, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_name", student.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":born_date", student.Born_date, System.Data.DbType.DateTime, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateStudent", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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
        public IActionResult UpdateStudent([FromBody] Students student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", student.Id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_name", student.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_born_date", student.Born_date, System.Data.DbType.DateTime, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("UpdateStudent", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult DeleteStudent(int id)
        {

            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("deleteStudent", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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
    }
}

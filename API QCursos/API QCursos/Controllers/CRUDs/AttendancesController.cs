using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Customized;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {

        [HttpGet("Teacher")]

        public IEnumerable<Attendances> ListAttendancesByTeacher(int Teacher)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Teacher", Teacher);

            var builder = new SqlBuilder();
            builder.Select("attendance.id");
            builder.Select("attendance.absence");
            builder.Select("attendance.fk_student");
            builder.Select("attendance.fk_lesson");
            builder.InnerJoin("lessons ON lessons.id = attendance.fk_lesson");
            builder.InnerJoin("classes ON lessons.fk_class = classes.id");
            builder.Where("classes.fk_teacher = :Teacher", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM attendance /**innerjoin**/ /**where**/");

            return connection.Query<Attendances>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpGet("Student")]

        public IEnumerable<AttendancesByStudent> ListAttendancesByStudent(int Student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Student", Student);

            var builder = new SqlBuilder();
            builder.Select("attendance.id");
            builder.Select("attendance.absence");
            builder.Select("attendance.fk_student");
            builder.Select("attendance.fk_lesson");
            builder.Select("lessons.lesson_date");
            builder.Select("classes.name AS class_name");
            builder.Select("classes.id AS class_id");
            builder.Select("courses.name AS course_name");
            builder.Select("courses.id  AS course_id");
            builder.InnerJoin("lessons ON lessons.id = attendance.fk_lesson");
            builder.InnerJoin("classes ON classes.id = lessons.fk_class");
            builder.InnerJoin("courses ON courses.id = classes.fk_course");
            builder.Where("attendance.fk_student = :Student", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM attendance /**innerjoin**/ /**where**/");

            return connection.Query<AttendancesByStudent>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPut]

        public IActionResult UpdateAttendance([FromBody] List<AttendanceUpdate> attendances)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            int httpStatus = 0;
            string message = "";

            foreach (AttendanceUpdate attendance in attendances)
            {

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", attendance.Id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":i_absence", attendance.Absence, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("UpdateAttendance", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                message = parameters.Get<string>(":msg");
                string httpStatusStr = parameters.Get<string>(":httpstatus");

                httpStatus = Convert.ToInt32(httpStatusStr);
            }

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

        [HttpDelete]

        public IActionResult DeleteAttendance([FromBody] List<Int32> ids)
        {
            try
            {
                Connection c = new Connection();
                using var connection = c.Connect();

                String message = "";
                int httpStatus = 0;

                foreach(Int32 id in ids)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                    parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                    parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                    connection.Query<String>("DeleteAttendance", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                    message = parameters.Get<string>(":msg");
                    string httpStatusStr = parameters.Get<string>(":httpstatus");

                    httpStatus = Convert.ToInt32(httpStatusStr);
                }

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

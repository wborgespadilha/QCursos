using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {

        [HttpGet("Teacher")]

        public IEnumerable<Lessons> ListLessonsByTeacher(int Teacher)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Teacher", Teacher);

            var builder = new SqlBuilder();
            builder.Select("lessons.id");
            builder.Select("lessons.lesson_date");
            builder.Select("lessons.fk_class");
            builder.InnerJoin("classes ON lessons.fk_class = classes.id ");
            builder.Where("classes.fk_teacher = :Teacher", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM lessons /**innerjoin**/ /**where**/");

            return connection.Query<Lessons>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();

        }

        [HttpPost]

        public IActionResult CreateLesson([FromBody] Lessons lesson)
        {

            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_lesson_date", lesson.Lesson_date, System.Data.DbType.DateTime, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_class_id", lesson.Fk_class, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateLesson", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

            String message = parameters.Get<string>(":msg");
            string httpStatusStr = parameters.Get<string>(":httpstatus");

            int httpStatus = Convert.ToInt32(httpStatusStr);


            switch (httpStatus)
            {
                case 200:

                    List<Int32> lessonId = new();
                    lessonId.AddRange(connection.Query<Int32>($"SELECT MAX(id) FROM lessons"));


                    var attendancesParameters = new DynamicParameters();
                    attendancesParameters.Add(":i_id", lessonId[0], direction: System.Data.ParameterDirection.Input);

                    connection.Query("createAttendance", attendancesParameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                    return Ok(message);

                case 400:
                    return BadRequest(message);

                case 404:
                    return NotFound(message);
            }

            return BadRequest(message);
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteLesson(int id)
        {

            try
            {
                Connection c = new Connection();
                using var connection = c.Connect();

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("DeleteLesson", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

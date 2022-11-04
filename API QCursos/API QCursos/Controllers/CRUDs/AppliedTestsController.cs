using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Costumized;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppliedTestsController : ControllerBase
    {

        [HttpGet("Class")]

        public IEnumerable<AppliedTests> ListAppliedTestsByClass(int Class)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Class", Class);

            var builder = new SqlBuilder();
            builder.InnerJoin("tests ON tests.id = appliedtests.fk_test");
            builder.Where("tests.fk_class = :Class", parameters);

            var builderTemplate = builder.AddTemplate("SELECT * FROM appliedtests /**innerjoin**/ /**where**/");

            return connection.Query<AppliedTests>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpGet("Institution")]

        public IEnumerable<AppliedTests> ListAppliedTestsByInstitution(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Institution", Institution);

            var builder = new SqlBuilder();
            builder.Select("appliedtests.id");
            builder.Select("appliedtests.grade");
            builder.Select("appliedtests.fk_student");
            builder.Select("appliedtests.fk_test");
            builder.InnerJoin("tests ON tests.id = appliedtests.fk_test");
            builder.InnerJoin("classes ON classes.id = tests.fk_class");
            builder.InnerJoin("courses ON courses.id = classes.fk_course");
            builder.Where("courses.fk_institution = :Institution", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM appliedtests /**innerjoin**/ /**where**/");

            return connection.Query<AppliedTests>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }


        [HttpGet("Student")]

        public IEnumerable<AppliedTestsWithCourse> ListAppliedTestsByStudent(int Student)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Student", Student);

            var builder = new SqlBuilder();
            builder.Select("appliedtests.id");
            builder.Select("appliedtests.grade");
            builder.Select("appliedtests.fk_student");
            builder.Select("appliedtests.fk_test");
            builder.Select("tests.name AS test_name");
            builder.Select("tests.test_date");
            builder.Select("teststypes.name AS testtype_name");
            builder.Select("classes.name AS class_name");
            builder.Select("classes.id AS class_id");
            builder.Select("courses.name AS course_name");
            builder.Select("courses.id  AS course_id");
            builder.InnerJoin("tests ON tests.id = appliedtests.fk_test");
            builder.InnerJoin("teststypes ON tests.fk_type = teststypes.id");
            builder.InnerJoin("classes ON classes.id = tests.fk_class");
            builder.InnerJoin("courses ON courses.id = classes.fk_course");
            builder.Where("appliedtests.fk_student = :Student", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM appliedtests /**innerjoin**/ /**where**/");

            return connection.Query<AppliedTestsWithCourse>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPut]

        public IActionResult UpdateAppliedTest([FromBody] List<AppliedTests> tests)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            string message = "";
            int httpStatus = 0;

            foreach (AppliedTests test in tests)
            {

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", test.Id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":i_grade", test.Grade, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("UpdateAppliedTest", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult DeleteAppliedTest([FromBody] List<Int32> ids)
        {
            try
            {
                Connection c = new Connection();
                using var connection = c.Connect();

                String message = "";
                int httpStatus = 0;

                foreach (Int32 id in ids)
                {
                    var parameters = new DynamicParameters();

                    parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                    parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                    parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                    connection.Query<String>("DeleteTest", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

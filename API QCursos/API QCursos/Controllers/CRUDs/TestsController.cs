using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {

        [HttpGet("Teacher")]

        public IEnumerable<Tests> ListTestsByTeacherDistinct(int Teacher)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Teacher", Teacher);

            var builder = new SqlBuilder();
            builder.Select("tests.id");
            builder.Select("tests.name");
            builder.Select("tests.test_date");
            builder.Select("tests.fk_type");
            builder.Select("tests.fk_class");
            builder.InnerJoin("classes ON classes.id = tests.fk_class");
            builder.Where("classes.fk_teacher = :Teacher", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM tests /**innerjoin**/ /**where**/");

            return connection.Query<Tests>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPost]
        public IActionResult CreateTest([FromBody] Tests test)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_name", test.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_test_date", test.Test_Date, System.Data.DbType.DateTime, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_type_id", test.Fk_type, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_class_id", test.Fk_class, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateTest", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

            String message = parameters.Get<string>(":msg");
            string httpStatusStr = parameters.Get<string>(":httpstatus");

            int httpStatus = Convert.ToInt32(httpStatusStr);


            switch (httpStatus)
            {
                case 200:

                    List<Int32> testId = new();

                    DynamicParameters testParameters = new DynamicParameters();
                    testParameters.Add(":Name", test.Name);
                    testParameters.Add(":Fk_type", test.Fk_type);
                    testParameters.Add(":Fk_class", test.Fk_class);

                    var builder = new SqlBuilder();
                    builder.Select("id");
                    builder.Where("name = :Name AND fk_type = :Fk_type AND fk_class = :Fk_class", testParameters);

                    var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM tests /**where**/");

                    testId.AddRange(connection.Query<Int32>(builderTemplate.RawSql, builderTemplate.Parameters).ToList());



                    var appliedTestParameters = new DynamicParameters();
                    appliedTestParameters.Add(":i_id", testId[0], direction: System.Data.ParameterDirection.Input);

                    connection.Query("createAppliedTest", appliedTestParameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

                    return Ok(message);

                case 400:
                    return BadRequest(message);

                case 404:
                    return NotFound(message);
            }

            return BadRequest(message);
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteTest(int id)
        {
            try
            {
                Connection c = new Connection();
                using var connection = c.Connect();

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("DeleteTest", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]

    public class TestsTypesController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<TestsTypes> ListTestsTypes()
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            return connection.Query<TestsTypes>("SELECT * FROM teststypes");
        }


        [HttpPost]
        public IActionResult CreateTestType([FromBody] TestsTypes testType)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_name", testType.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateTestType", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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
        public IActionResult UpdateTestType([FromBody] TestsTypes testType)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", testType.Id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_name", testType.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("UpdateTestType", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult DeleteTestType(int id)
        {
            try
            {
                Connection c = new Connection();
                using var connection = c.Connect();

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("DeleteTestType", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

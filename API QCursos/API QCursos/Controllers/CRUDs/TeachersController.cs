using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]

    public class TeachersController : ControllerBase
    {

        [HttpGet("Institution")]

        public IEnumerable<Teachers> ListTeachersByInstitution(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Institution", Institution);

            var builder = new SqlBuilder();
            builder.Where("fk_institution = :Institution", parameters);

            var builderTemplate = builder.AddTemplate("SELECT * FROM teachers /**where**/");

            return connection.Query<Teachers>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }


        [HttpPost]
        public IActionResult CreateTeacher([FromBody] Teachers teacher)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_name", teacher.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":teacher_cpf", teacher.Cpf, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":password", teacher.Password, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":i_institution_id", teacher.Fk_institution, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateTeacher", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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
        public IActionResult UpdateTeacher([FromBody] Teachers teacher)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", teacher.Id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_name", teacher.Name, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_cpf", teacher.Cpf, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_password", teacher.Password, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("UpdateTeacher", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult DeleteTeacher(int id)
        {
            Connection c = new Connection(); ;
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("DeleteTeacher", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

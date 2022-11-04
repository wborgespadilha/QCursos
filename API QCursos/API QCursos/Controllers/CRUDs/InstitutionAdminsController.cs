using Dapper;
using Microsoft.AspNetCore.Mvc;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Costumized;

namespace API_QCursos.Controllers.CRUDs
{
    [Route("api/[controller]")]
    [ApiController]

    public class InstitutionAdminsController : ControllerBase
    {

        [HttpGet]

        public IEnumerable<InstitutionAdminsWithNames> ListInstitutuionAdmins()
        {
            Connection c = new Connection();

            using var connection = c.Connect();

            var builder = new SqlBuilder();
            builder.Select("admins.id");
            builder.Select("admins.cpf");
            builder.Select("admins.password");
            builder.Select("admins.fk_institution");
            builder.Select("admins.registry");
            builder.Select("institutions.name AS institution_name");
            builder.InnerJoin("institutions ON institutions.id = admins.fk_institution");

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM admins /**innerjoin**/");

            return connection.Query<InstitutionAdminsWithNames>(builderTemplate.RawSql).ToList();
        }


        [HttpGet("Institution")]

        public IEnumerable<InstitutionAdmins> ListInstitutuionAdminsByInstitution(int Institution)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Institution", Institution);

            var builder = new SqlBuilder();
            builder.Select("admins.id");
            builder.Select("admins.cpf");
            builder.Select("admins.password");
            builder.Select("admins.fk_institution");
            builder.Select("admins.registry");
            builder.Select("institutions.name AS institution_name");
            builder.InnerJoin("institutions ON institutions.id = admins.fk_institution");
            builder.Where("admins.fk_institution = :Institution", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM admins /**innerjoin**/ /**where**/");

            return connection.Query<InstitutionAdmins>(builderTemplate.RawSql,builderTemplate.Parameters).ToList();

        }

        [HttpPost]

        public IActionResult CreateInstitutionAdmin([FromBody] InstitutionAdmins admin)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":institution_id", admin.Fk_institution, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":admin_cpf", admin.Cpf, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":password", admin.Password, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("CreateInstitutionAdmin", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult UpdateInstitutionAdmin([FromBody] InstitutionAdmins admin)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            var parameters = new DynamicParameters();
            parameters.Add(":i_id", admin.Id, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_institution_id", admin.Fk_institution, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_cpf", admin.Cpf, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":new_password", admin.Password, direction: System.Data.ParameterDirection.Input);
            parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
            parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

            connection.Query<String>("UpdateInstitutionAdmin", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

        public IActionResult DeleteInstitutionAdmin(int id)
        {
            try
            {
                Connection c = new Connection();
                using var connection = c.Connect();

                var parameters = new DynamicParameters();
                parameters.Add(":i_id", id, direction: System.Data.ParameterDirection.Input);
                parameters.Add(":msg", "", direction: System.Data.ParameterDirection.Output);
                parameters.Add(":httpstatus", "", direction: System.Data.ParameterDirection.Output);

                connection.Query<String>("DeleteInstitutionAdmin", parameters, commandType: System.Data.CommandType.StoredProcedure).ToString();

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

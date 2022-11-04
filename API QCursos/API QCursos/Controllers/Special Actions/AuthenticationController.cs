using Dapper;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using API_QCursos.Configs;
using API_QCursos.Models.Raw;
using API_QCursos.Models.Login;

namespace API_QCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController
    {
        [HttpPost("loginIADM")]

        public IEnumerable<InstitutionAdmins> LoginInstitutionAdmin(Login loginInstituionADM)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Registry", loginInstituionADM.Registry);
            parameters.Add(":Password", loginInstituionADM.Password);

            var builder = new SqlBuilder();
            builder.Where("admins.registry = :Registry AND admins.password = :Password", parameters);

            var builderTemplate = builder.AddTemplate("SELECT * FROM admins /**where**/");

            return connection.Query<InstitutionAdmins>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();

        }

        [HttpPost("loginMADM")]
        public IEnumerable<MasterAdmin> LoginMasterAdmin(Login loginMasterADM)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Registry", loginMasterADM.Registry);
            parameters.Add(":Password", loginMasterADM.Password);

            var builder = new SqlBuilder();
            builder.Where("masteradmin.registry = :Registry AND masteradmin.password = :Password", parameters);

            var builderTemplate = builder.AddTemplate("SELECT * FROM masteradmin /**where**/");

            return connection.Query<MasterAdmin>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPost("loginTeacher")]
        public IEnumerable<TeachersWithInstitution> LoginTeacher(Login loginTeacher)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Registry", loginTeacher.Registry);
            parameters.Add(":Password", loginTeacher.Password);

            var builder = new SqlBuilder();
            builder.Select("teachers.id");
            builder.Select("teachers.name");
            builder.Select("teachers.cpf");
            builder.Select("teachers.password");
            builder.Select("teachers.fk_institution");
            builder.Select("teachers.registry");
            builder.Select("institutions.name AS institution_name");
            builder.InnerJoin("institutions ON institutions.id = teachers.fk_institution");
            builder.Where("teachers.registry = :Registry AND teachers.password = :Password", parameters);

            var builderTemplate = builder.AddTemplate("SELECT /**select**/ FROM teachers /**innerjoin**/ /**where**/");

            return connection.Query<TeachersWithInstitution>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

        [HttpPost("loginStudent")]
        public IEnumerable<Students> LoginStudent(StudentLogin loginStudent)
        {
            Connection c = new Connection();
            using var connection = c.Connect();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":Registry", loginStudent.Registry);
            parameters.Add(":Born_Date", loginStudent.Born_date, System.Data.DbType.DateTime);

            var builder = new SqlBuilder();
            builder.Where("students.registry = :Registry AND students.born_date = :Born_Date", parameters);

            var builderTemplate = builder.AddTemplate("SELECT * FROM students /**where**/");

            return connection.Query<Students>(builderTemplate.RawSql, builderTemplate.Parameters).ToList();
        }

    }
}

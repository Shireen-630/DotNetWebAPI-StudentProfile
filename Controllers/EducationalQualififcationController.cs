using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Student_Profile.Class;

namespace Student_Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationalQualififcationController : ControllerBase
    {
        private readonly string StrCon = "Server=DESKTOP-VFNAH5N;Database=profile;User ID=sa;Password=shireen;TrustServerCertificate=True;";

        [HttpPost("IUD")]
        public async Task<EducationalQualififcations> IUD(EducationalQualififcations eq, char action)
        {
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_EducationalQualifications_IUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentID", eq.StudentID);
            cmd.Parameters.AddWithValue("@Degree", eq.Degree);
            cmd.Parameters.AddWithValue("@College", eq.College ?? "");
            cmd.Parameters.AddWithValue("@UniversityType", eq.UniversityType ?? "");
            cmd.Parameters.AddWithValue("@StartYear", eq.StartYear);
            cmd.Parameters.AddWithValue("@EndYear", eq.EndYear);
            cmd.Parameters.AddWithValue("@ModeofStudy", eq.ModeofStudy ?? "");
            cmd.Parameters.AddWithValue("@CGPA", eq.CGPA);
            cmd.Parameters.AddWithValue("@ProjectTitle", eq.ProjectTitle ?? "");
            cmd.Parameters.AddWithValue("@Achievement", (object)eq.Achievement ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Certificate", eq.Certificate ?? "");
            cmd.Parameters.AddWithValue("@Action", action);

            try
            {
                await con.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();
                eq.Message = $"Rows affected: {rows}";
            }
            catch (Exception ex)
            {
                eq.Message = $"Error: {ex.Message}";
            }

            return eq;
        }

        [HttpGet("GetAll")]
        public async Task<List<EducationalQualififcations>> GetAll()
        {
            var list = new List<EducationalQualififcations>();
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_EducationalQualifications_GetAll", con) { CommandType = CommandType.StoredProcedure };

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new EducationalQualififcations
                {
                    StudentID = reader["StudentID"].ToString(),
                    Degree = reader["Degree"].ToString(),
                    College = reader["College"].ToString(),
                    UniversityType = reader["UniversityType"].ToString(),
                    StartYear = Convert.ToInt16(reader["StartYear"]),
                    EndYear = Convert.ToInt16(reader["EndYear"]),
                    ModeofStudy = reader["ModeofStudy"].ToString(),
                    CGPA = Convert.ToDecimal(reader["CGPA"]),
                    ProjectTitle = reader["ProjectTitle"].ToString(),
                    Achievement = reader["Achievement"].ToString(),
                    Certificate = reader["Certificate"].ToString()
                });
            }
            return list;
        }

        [HttpGet("GetById/{id}")]
        public async Task<List<EducationalQualififcations>> GetById(string id)
        {
            var list = new List<EducationalQualififcations>();
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_EducationalQualifications_GetByID", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@StudentID", id);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new EducationalQualififcations
                {
                    StudentID = reader["StudentID"].ToString(),
                    Degree = reader["Degree"].ToString(),
                    College = reader["College"].ToString(),
                    UniversityType = reader["UniversityType"].ToString(),
                    StartYear = Convert.ToInt16(reader["StartYear"]),
                    EndYear = Convert.ToInt16(reader["EndYear"]),
                    ModeofStudy = reader["ModeofStudy"].ToString(),
                    CGPA = Convert.ToDecimal(reader["CGPA"]),
                    ProjectTitle = reader["ProjectTitle"].ToString(),
                    Achievement = reader["Achievement"].ToString(),
                    Certificate = reader["Certificate"].ToString()
                });
            }
            return list;
        }
    }
}

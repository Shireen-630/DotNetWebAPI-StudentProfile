using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Student_Profile.Class;

namespace Student_Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly string StrCon = "Server=DESKTOP-VFNAH5N;Database=profile;User ID=sa;Password=shireen;TrustServerCertificate=True;";

        [HttpPost("IUD")]
        public async Task<Profile> IUD(Profile profile, char action)
        {
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_Profile_IUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentID", profile.StudentID);
            cmd.Parameters.AddWithValue("@FirstName", profile.FirstName ?? "");
            cmd.Parameters.AddWithValue("@LastName", (object)profile.LastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GivenName", profile.GivenName ?? "");
            cmd.Parameters.AddWithValue("@Mobile", profile.Mobile ?? "");
            cmd.Parameters.AddWithValue("@DOB", profile.DOB);
            cmd.Parameters.AddWithValue("@Email", profile.Email ?? "");
            cmd.Parameters.AddWithValue("@Gender", profile.Gender ?? "");
            cmd.Parameters.AddWithValue("@Action", action);

            try
            {
                await con.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();
                profile.Message = $"Rows affected: {rows}";
            }
            catch (Exception ex)
            {
                profile.Message = $"Error: {ex.Message}";
            }

            return profile;
        }

        [HttpGet("GetAll")]
        public async Task<List<Profile>> GetAll()
        {
            var list = new List<Profile>();
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_Profile_GetAll", con) { CommandType = CommandType.StoredProcedure };

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Profile
                {
                    StudentID = reader["StudentID"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    GivenName = reader["GivenName"].ToString(),
                    Mobile = reader["Mobile"].ToString(),
                    DOB = Convert.ToDateTime(reader["DOB"]),
                    Email = reader["Email"].ToString(),
                    Gender = reader["Gender"].ToString()
                });
            }
            return list;
        }

        [HttpGet("GetById/{id}")]
        public async Task<Profile> GetById(string id)
        {
            Profile profile = null;
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_Profile_GetByID", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@StudentID", id);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                profile = new Profile
                {
                    StudentID = reader["StudentID"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    GivenName = reader["GivenName"].ToString(),
                    Mobile = reader["Mobile"].ToString(),
                    DOB = Convert.ToDateTime(reader["DOB"]),
                    Email = reader["Email"].ToString(),
                    Gender = reader["Gender"].ToString()
                };
            }
            return profile;
        }
    }
}

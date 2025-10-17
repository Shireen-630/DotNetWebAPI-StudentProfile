using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Student_Profile.Class;

namespace Student_Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInformationController : ControllerBase
    {

        private readonly string StrCon = "Server=DESKTOP-VFNAH5N;Database=profile;User ID=sa;Password=shireen;TrustServerCertificate=True;";

        [HttpPost("IUD")]
        public async Task<ContactInformation> IUD(ContactInformation contact, char action)
        {
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_ContactInformation_IUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentID", contact.StudentID);
            cmd.Parameters.AddWithValue("@AddressLine1", contact.AddressLine1 ?? "");
            cmd.Parameters.AddWithValue("@AddressLine2", (object)contact.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@City", contact.City ?? "");
            cmd.Parameters.AddWithValue("@Pincode", contact.Pincode ?? "");
            cmd.Parameters.AddWithValue("@AlternateMobile", contact.AlternateMobile ?? "");
            cmd.Parameters.AddWithValue("@Achievements", contact.Achievements ?? "");
            cmd.Parameters.AddWithValue("@State", contact.State ?? "");
            cmd.Parameters.AddWithValue("@Country", contact.Country ?? "");
            cmd.Parameters.AddWithValue("@ContactMode", contact.ContactMode ?? "");
            cmd.Parameters.AddWithValue("@Action", action);

            try
            {
                await con.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();
                contact.Message = $"Rows affected: {rows}";
            }
            catch (Exception ex)
            {
                contact.Message = $"Error: {ex.Message}";
            }

            return contact;
        }

        [HttpGet("GetAll")]
        public async Task<List<ContactInformation>> GetAll()
        {
            var list = new List<ContactInformation>();
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_ContactInformation_GetAll", con) { CommandType = CommandType.StoredProcedure };

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ContactInformation
                {
                    StudentID = reader["StudentID"].ToString(),
                    AddressLine1 = reader["AddressLine1"].ToString(),
                    AddressLine2 = reader["AddressLine2"].ToString(),
                    City = reader["City"].ToString(),
                    Pincode = reader["Pincode"].ToString(),
                    AlternateMobile = reader["AlternateMobile"].ToString(),
                    Achievements = reader["Achievements"].ToString(),
                    State = reader["State"].ToString(),
                    Country = reader["Country"].ToString(),
                    ContactMode = reader["ContactMode"].ToString()
                });
            }
            return list;
        }

        [HttpGet("GetById/{id}")]
        public async Task<ContactInformation> GetById(string id)
        {
            ContactInformation contact = null;
            using var con = new SqlConnection(StrCon);
            using var cmd = new SqlCommand("SP_ContactInformation_GetByID", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@StudentID", id);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                contact = new ContactInformation
                {
                    StudentID = reader["StudentID"].ToString(),
                    AddressLine1 = reader["AddressLine1"].ToString(),
                    AddressLine2 = reader["AddressLine2"].ToString(),
                    City = reader["City"].ToString(),
                    Pincode = reader["Pincode"].ToString(),
                    AlternateMobile = reader["AlternateMobile"].ToString(),
                    Achievements = reader["Achievements"].ToString(),
                    State = reader["State"].ToString(),
                    Country = reader["Country"].ToString(),
                    ContactMode = reader["ContactMode"].ToString()
                };
            }
            return contact;
        }
    }
}
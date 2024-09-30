using Core.Entities;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace ContactServiceAPi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetByContactIdAsync(Guid contactId)
        {
            Contact contact = new Contact();

            using (var connectionsql = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM [TechChallenge1]..[Contact] WHERE ContactId = @ContactId";
                contact = connectionsql.QueryFirstOrDefault<Contact>(sql, new { UserName = contactId });
            }

            return Ok(contact);
        }


        [HttpGet]
        public IActionResult GetUsersAsync()
        {
            List<Contact> contacts = new List<Contact>();

            using (var connectionsql = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM [TechChallenge1]..[Contact]";
                contacts = connectionsql.Query<Contact>(sql).ToList();
            }

            return Ok(contacts);
        }
    }
}

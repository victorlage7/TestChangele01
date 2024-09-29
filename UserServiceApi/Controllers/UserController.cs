using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;

namespace UserServiceApi.Controllers
{

    [Route("api/[controller]/")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetUserAsync(string username)
        {
            using (var connectionsql = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM [TechChallenge1]..[User] WHERE Username = @UserName";
                var user = connectionsql.QueryFirstOrDefault<User>(sql, new { UserName = username });
                Console.WriteLine(user);
            }

            return Ok(User);
        }
    }
}

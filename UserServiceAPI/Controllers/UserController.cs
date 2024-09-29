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

        [HttpGet("{username}")]
        public IActionResult GetUserAsync(string username)
        {
            User user = new User();

            using (var connectionsql = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM [TechChallenge1]..[User] WHERE Username = @UserName";
                user = connectionsql.QueryFirstOrDefault<User>(sql, new { UserName = username });
                Console.WriteLine(user);
            }

            return Ok(user);
        }


        [HttpGet]
        public IActionResult GetUsersAsync()
        {
            List<User> user = new List<User>();

            using (var connectionsql = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM [TechChallenge1]..[User]";
                user = connectionsql.Query<User>(sql).ToList();
                Console.WriteLine(user);
            }

            return Ok(user);
        }
    }
}

using ContactCore;
using Dapper;
using Microsoft.AspNetCore.Mvc;
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
                contact = connectionsql.QueryFirstOrDefault<Contact>(sql, new { ContactId = contactId });
           

                if (contact != null)
                {
                    var ContactEmails = "SELECT * FROM [TechChallenge1]..[ContactEmails] WHERE ContactId = @ContactId";
                    contact.EmailAddresses = (List<EmailAddress>?)connectionsql.Query<EmailAddress>(ContactEmails, new { ContactId = contactId });
                }


                if (contact != null)
                {
                    var ContactPhoneNumbers = "SELECT * FROM [TechChallenge1]..[ContactPhoneNumbers] WHERE ContactId = @ContactId";
                    contact.PhoneNumbers = (List<PhoneNumber>?)connectionsql.Query<PhoneNumber>(ContactPhoneNumbers, new { ContactId = contactId });
                }

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


                foreach (var contact in contacts)
                {

                    if (contact != null)
                    {
                        var ContactEmails = "SELECT * FROM [TechChallenge1]..[ContactEmails] WHERE ContactId = @ContactId";
                        contacts[contacts.IndexOf(contact)] .EmailAddresses = (List<EmailAddress>?)connectionsql.Query<EmailAddress>(ContactEmails, new { ContactId = contact.ContactId });
                    }


                    if (contact != null)
                    {
                        var ContactPhoneNumbers = "SELECT * FROM [TechChallenge1]..[ContactPhoneNumbers] WHERE ContactId = @ContactId";
                        contacts[contacts.IndexOf(contact)].PhoneNumbers = (List<PhoneNumber>?)connectionsql.Query<PhoneNumber>(ContactPhoneNumbers, new { ContactId = contact.ContactId });
                    }
                }
            }


            

            return Ok(contacts);
        }
    }
}

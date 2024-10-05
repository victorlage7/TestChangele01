
namespace ContactCore
{
    public class Contact
    {
        public Contact() {
            this.EmailAddresses = new List<EmailAddress> { };
            this.PhoneNumbers = new List<PhoneNumber> { };
        }
        public Guid ContactId { get; set; }
        public string Name { get; set; }

        public List<EmailAddress>? EmailAddresses { get; set; }

        public List<PhoneNumber>? PhoneNumbers { get; set; }
    }
}

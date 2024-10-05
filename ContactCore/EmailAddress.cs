using Core.Enums;

namespace ContactCore
{
    public class EmailAddress
    {

        public EmailAddress(){}

        //public EmailAddress(EmailAddressType type, string address)
        //{
        //    Type = type;
        //    Address = address.ToLower();
        //}

        public EmailAddressType Type { get; set; }

        public string Address { get; set; }
    }
}

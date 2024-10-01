using Core.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContactCore
{
    public class PhoneNumber
    {
        public PhoneNumber() { }

        //public PhoneNumber(PhoneNumberType type, string countryCode, string areaCode, string number)
        //{
        //    Type = type;
        //    CountryCode = countryCode;
        //    AreaCode = areaCode;
        //    Number = number;
        //}

        public PhoneNumberType Type { get; set; }

        public string AreaCode { get; set; }

        public string Number { get; set; }

        public string CountryCode { get; set; }
    }
}

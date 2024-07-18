using System.ComponentModel;

namespace WebApi.Domain.Enums;

public enum PhoneNumberType
{
    [Description("Celular")]Cellular = 1,
    [Description("Comercial")]Commercial = 2,
    [Description("Residencial")]Home = 3
}
using System.ComponentModel;

namespace WebApi.Domain.Enums;

public enum EmailAddressType
{
    [Description("Pessoal")] Personal = 1,
    [Description("Comercial")] Commercial = 2
}
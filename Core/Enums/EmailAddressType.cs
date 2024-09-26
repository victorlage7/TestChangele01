using System.ComponentModel;

namespace Core.Enums;

public enum EmailAddressType
{
    [Description("Pessoal")] Personal = 1,
    [Description("Comercial")] Commercial = 2
}
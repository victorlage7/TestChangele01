using WebApi.Domain.Enums;

namespace WebApi.ViewModels;

public record PhoneNumberViewModel
{
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="type">Tipo de telefone</param>
    /// <param name="countryCode">Código do país</param>
    /// <param name="areaCode">Código de área</param>
    /// <param name="number">Número de telefone</param>
    public PhoneNumberViewModel(PhoneNumberType type, string countryCode, string areaCode, string number)
    {
        Type = type;
        CountryCode = countryCode;
        AreaCode = areaCode;
        Number = number;
    }

    /// <summary>
    /// Tipo Ex: Home
    /// </summary>
    public PhoneNumberType Type { get;}

    /// <summary>
    /// Código de área do país Ex: 55
    /// </summary>
    public string CountryCode { get;}

    /// <summary>
    /// Código de área do estado / cidade Ex: 11
    /// </summary>
    public string AreaCode { get;}

    /// <summary>
    /// Número de telefone. Ex: 992503949
    /// </summary>
    public string Number { get;}
};
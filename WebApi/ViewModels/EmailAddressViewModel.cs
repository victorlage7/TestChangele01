using System.ComponentModel.DataAnnotations;
using WebApi.Domain.Enums;

namespace WebApi.ViewModels;

public record EmailAddressViewModel
{
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="type">Tipo do Email</param>
    /// <param name="address">E-mail</param>
    public EmailAddressViewModel(EmailAddressType type, string address)
    {
        Type = type;
        Address = address;
    }

    /// <summary>
    /// Tipo Ex: Home
    /// </summary>
    public EmailAddressType Type { get; }
    
    /// <summary>
    /// Endereço de e-mail
    /// </summary>
    [EmailAddress]
    public string Address { get;}
    
};
using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain.Requests;

public class ContactRequestAll
{
    /// <summary>
    /// Nome do contato.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Pais do telefone.
    /// </summary>
    public string? PhoneCountryCode { get; set; }
    
    /// <summary>
    /// Codigo de area do telefone.
    /// </summary>
    public string? PhoneAreaCode { get; set; }
    
    /// <summary>
    /// Número do telefone.
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Endereço de e-mail.
    /// </summary>
    [EmailAddress] public string? EmailAddress { get; set; }
}
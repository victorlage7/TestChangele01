namespace WebApi.ViewModels;

public record ContactViewModel
{

    public ContactViewModel()
    {
            
    }
    
    /// <summary>
    /// Construtor de contato.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <param name="name">Nome do contato</param>
    /// <param name="emailAddresses">Lista de e-mails do contato</param>
    /// <param name="phoneNumbers">Lista de telefones do contato</param>
    public ContactViewModel(Guid contactId, string name, 
                            IEnumerable<EmailAddressViewModel> emailAddresses, 
                            IEnumerable<PhoneNumberViewModel> phoneNumbers)
    {
        ContactId = contactId;
        Name = name;
        EmailAddresses = emailAddresses;
        PhoneNumbers = phoneNumbers;
    }

    /// <summary>
    /// Identificador do contato.
    /// </summary>
    public Guid ContactId { get; init; }

    /// <summary>
    /// Nome do contato
    /// </summary>
    public string  Name { get; init; }

    /// <summary>
    /// Emails do contato
    /// </summary>
    public IEnumerable<EmailAddressViewModel> EmailAddresses { get; init; }

    /// <summary>
    /// Telefones do contato
    /// </summary>
    public IEnumerable<PhoneNumberViewModel> PhoneNumbers { get; init; }
};
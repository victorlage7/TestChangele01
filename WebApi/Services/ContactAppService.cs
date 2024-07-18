using System.ComponentModel.DataAnnotations;
using WebApi.Domain;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Domain.Interfaces.Services;
using WebApi.Domain.Requests;
using WebApi.Domain.ValueObjects;
using WebApi.Interfaces;
using WebApi.ViewModels;

namespace WebApi.Services;

/// <summary>
/// Serviço de aplicação de contato.
/// </summary>
public class ContactAppService : IContactAppService
{
    private readonly IContactRepository _contactRepository;
    private readonly IContactDomainService _contactDomainService;

    /// <summary>
    /// Condtrutor serviço de aplicação de contato.
    /// </summary>
    /// <param name="contactRepository">Repositorio do contato</param>
    /// <param name="contactDomainService">Dominio do contato</param>
    public ContactAppService(IContactRepository contactRepository, IContactDomainService contactDomainService)
    {
        _contactRepository = contactRepository;
        _contactDomainService = contactDomainService;
    }

    /// <summary>
    /// Retorna todos os contatos.
    /// </summary>
    /// <param name="requestAll">Campos com filtro do contato</param>
    /// <returns></returns>
    public async Task<IEnumerable<ContactViewModel>> GetAllAsync(ContactRequestAll requestAll)
    {
        var contacts = await _contactRepository.GetAllAsync(requestAll);

        var contactViewModels = contacts.Select(contact =>
            new ContactViewModel(contactId: contact.ContactId, name: contact.Name,
                phoneNumbers: contact.PhoneNumbers.Select(phone =>
                    new PhoneNumberViewModel(type: phone.Type, countryCode: phone.CountryCode,
                        areaCode: phone.AreaCode, number: phone.Number)),
                emailAddresses: contact.EmailAddresses.Select(email =>
                    new EmailAddressViewModel(type: email.Type, address: email.Address))));
        return contactViewModels;
    }

    /// <summary>
    /// Retorna um contato pelo Id.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns></returns>
    public async Task<ContactViewModel?> GetByContactIdAsync(Guid contactId)
    {
        var contact = await _contactRepository.GetByContactIdAsync(contactId);

        if (contact == null) return null;

        var contactViewModel = new ContactViewModel(contactId: contact.ContactId, name: contact.Name,
            phoneNumbers: contact.PhoneNumbers.Select(phone =>
                new PhoneNumberViewModel(type: phone.Type, countryCode: phone.CountryCode,
                    areaCode: phone.AreaCode, number: phone.Number)),
            emailAddresses: contact.EmailAddresses.Select(email =>
                new EmailAddressViewModel(type: email.Type, address: email.Address)));

        return contactViewModel;
    }

    /// <summary>
    /// Inclui um novo contato.
    /// </summary>
    /// <param name="contactViewModel">ViewModel do contato</param>
    /// <returns></returns>
    public async Task<ResultValidation> AddAsync(ContactViewModel contactViewModel)
    {
        var newContact = new Contact(contactViewModel.Name);

        foreach (var phoneViewModel in contactViewModel.PhoneNumbers)
            newContact.AddPhoneNumber(new PhoneNumber(phoneViewModel.Type,
                phoneViewModel.CountryCode,
                phoneViewModel.AreaCode,
                phoneViewModel.Number));

        foreach (var emailViewModel in contactViewModel.EmailAddresses)
            newContact.AddEmailAddress(new EmailAddress(emailViewModel.Type, emailViewModel.Address));

        var resulValidation = await _contactDomainService.AddAsync(newContact);

        if (resulValidation.Object is null) return resulValidation;

        var contact = (Contact)resulValidation.Object;

        var newContactViewModel = new ContactViewModel(contactId: contact.ContactId, name: contact.Name,
            phoneNumbers: contact.PhoneNumbers.Select(phone =>
                new PhoneNumberViewModel(type: phone.Type, countryCode: phone.CountryCode,
                    areaCode: phone.AreaCode, number: phone.Number)),
            emailAddresses: contact.EmailAddresses.Select(email =>
                new EmailAddressViewModel(type: email.Type, address: email.Address)));

        return new ResultValidation
        {
            Object = newContactViewModel
        };
    }

    /// <summary>
    /// Atualiza um novo contato.
    /// </summary>
    /// <param name="contactViewModel">ViewModel do contato</param>
    /// <returns></returns>
    public async Task<ResultValidation> UpdateAsync(ContactViewModel contactViewModel)
    {
        var contact = await _contactRepository.GetByContactIdAsync(contactViewModel.ContactId);

        if (contact == null)
            return new ResultValidation()
            {
                ValidationResults = new List<ValidationResult>
                    { new("Contato não localizado") }
            };

        contact.Update(contactViewModel.Name);

        if (contactViewModel.PhoneNumbers.Any())
        {
            foreach (var phoneNumber in contact.PhoneNumbers)
            {
                if (!contactViewModel.PhoneNumbers.Any(x =>
                        x.Type == phoneNumber.Type
                        && x.CountryCode == phoneNumber.CountryCode
                        && x.AreaCode == phoneNumber.AreaCode
                        && x.Number == phoneNumber.Number))
                    contact.RemovePhoneNumber(phoneNumber);

                ;
            }

            foreach (var phoneViewModel in contactViewModel.PhoneNumbers)
                contact.AddPhoneNumber(new PhoneNumber(phoneViewModel.Type,
                    phoneViewModel.CountryCode,
                    phoneViewModel.AreaCode,
                    phoneViewModel.Number));
        }
        else
        {
            contact.RemoveAllPhoneNumber();
        }


        if (contactViewModel.EmailAddresses.Any())
        {
            foreach (var email in contact.EmailAddresses)
            {
                if (!contactViewModel.EmailAddresses.Any(x =>
                        x.Type == email.Type
                        && x.Address == email.Address))
                    contact.RemoveEmailAddress(email);

                ;
            }

            foreach (var email in contactViewModel.EmailAddresses)
                contact.AddEmailAddress(new EmailAddress(email.Type,
                    email.Address));
        }
        else
        {
            contact.RemoveAllEmailAddress();
        }

        var resulValidation = await _contactDomainService.UpdateAsync(contact);

        if (resulValidation.Object is null) return resulValidation;

        contact = (Contact)resulValidation.Object;

        var updContactViewModel = new ContactViewModel(contactId: contact.ContactId, name: contact.Name,
            phoneNumbers: contact.PhoneNumbers.Select(phone =>
                new PhoneNumberViewModel(type: phone.Type, countryCode: phone.CountryCode,
                    areaCode: phone.AreaCode, number: phone.Number)),
            emailAddresses: contact.EmailAddresses.Select(email =>
                new EmailAddressViewModel(type: email.Type, address: email.Address)));

        return new ResultValidation
        {
            Object = updContactViewModel
        };
    }

    /// <summary>
    /// Exlui um contato.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(Guid contactId)
    {
        return await _contactDomainService.DeleteAsync(contactId);
    }
}
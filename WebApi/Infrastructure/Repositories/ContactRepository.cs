using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Domain.Requests;
using WebApi.Infrastructure.Context;

namespace WebApi.Infrastructure.Repositories;
/// <summary>
/// Repositório de contatos.
/// </summary>
public class ContactRepository : IContactRepository
{
    private readonly TechChallenge1DbContext _context;

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="context">DBContext</param>
    public ContactRepository(TechChallenge1DbContext context)
    {
        _context = context;
    }
    
     /// <summary>
     /// Retorna todos os contatos.
     /// </summary>
     /// <param name="requestAll">Campos do contato para filtro</param>
     /// <returns></returns>
    public async Task<IList<Contact>> GetAllAsync(ContactRequestAll requestAll)
    {
        var queryContact = _context.Contacts.AsQueryable();

        if (!string.IsNullOrEmpty(requestAll.Name))
            queryContact = queryContact.Where(c => c.Name.Trim().Contains(requestAll.Name.Trim()));

        if (requestAll.PhoneCountryCode != null)
            queryContact = queryContact.Where(c =>
                c.PhoneNumbers.Any(p => p.CountryCode.Equals(requestAll.PhoneCountryCode)));

        if (requestAll.PhoneAreaCode != null)
            queryContact = queryContact.Where(c =>
                c.PhoneNumbers.Any(p => p.AreaCode.Equals(requestAll.PhoneAreaCode)));

        if (requestAll.PhoneNumber != null)
            queryContact = queryContact.Where(c =>
                c.PhoneNumbers.Any(p => p.Number.Equals(requestAll.PhoneNumber)));

        if (requestAll.EmailAddress != null)
            queryContact = queryContact.Where(c =>
                c.EmailAddresses.Any(p => p.Address.Equals(requestAll.EmailAddress)));
        
        var contacts = queryContact.ToList();

        await Task.CompletedTask;
        return contacts;
    }
    
     /// <summary>
     /// Retorna um contato pelo Id.
     /// </summary>
     /// <param name="contactId">Identificador do contato</param>
     /// <returns>Retorna o contato</returns>
    public async Task<Contact?> GetByContactIdAsync(Guid contactId)
    {
        var contact = _context.Contacts.FirstOrDefault(x => x.ContactId == contactId);
        await Task.CompletedTask;
        return contact;
    }
    
     /// <summary>
     /// Incluir um novo contato.
     /// </summary>
     /// <param name="contact">Contato</param>
     /// <returns>Retorna o contato</returns>
    public async Task<Contact> AddAsync(Contact contact)
    {
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();

        return contact;
    }
     
    /// <summary>
    /// Atualiza um contato.
    /// </summary>
    /// <param name="contact">Contato</param>
    /// <returns>Retorna o contato</returns>
    public async Task<Contact> UpdateAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();

        return contact;
    }
    
    /// <summary>
    /// Exluir um contato.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns>Retorna o contato</returns>
    public async Task<bool> DeleteAsync(Guid contactId)
    {
        var result = _context.Contacts.SingleOrDefault(x => x.ContactId == contactId);
        if (result is null)
            return false;

        _context.Contacts.Remove(result);
        _context.SaveChanges();

        await Task.CompletedTask;
        return true;
    }
}
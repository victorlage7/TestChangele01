using WebApi.Domain.Entities;
using WebApi.Domain.Requests;

namespace WebApi.Domain.Interfaces.Repositories;

public interface IContactRepository
{
    /// <summary>
    /// Retorna todos os contatos.  
    /// </summary>
    /// <param name="requestAll">Campos para filtrar o contato</param>
    /// <returns></returns>
    Task<IList<Contact>> GetAllAsync(ContactRequestAll requestAll);
    /// <summary>
    /// Retorna um contato pelo Id. 
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns></returns>
    Task<Contact?> GetByContactIdAsync(Guid contactId);
    /// <summary>
    /// Inclui um novo contato. 
    /// </summary>
    /// <param name="contact">Contato</param>
    /// <returns></returns>
    Task<Contact> AddAsync(Contact contact);
    /// <summary>
    /// Atualiza um contato.    
    /// </summary>
    /// <param name="contact">Contato</param>
    /// <returns></returns>
    Task<Contact> UpdateAsync(Contact contact);
    /// <summary>
    /// Exlui um contato.   
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid contactId);
}
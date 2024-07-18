using WebApi.Domain;
using WebApi.Domain.Requests;
using WebApi.ViewModels;

namespace WebApi.Interfaces;

public interface IContactAppService
{
    /// <summary>
    /// Retorna todos os contatos.
    /// </summary>
    /// <param name="requestAll">Campos com filtro do contato</param>
    /// <returns></returns>
    Task<IEnumerable<ContactViewModel>> GetAllAsync(ContactRequestAll requestAll);
    
    /// <summary>   
    /// Retorna um contato pelo Id.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns></returns>
    Task<ContactViewModel?> GetByContactIdAsync(Guid contactId);
    
    /// <summary>
    /// Inclui um novo contato.
    /// </summary>
    /// <param name="contactViewModel">ViewModel do contato</param>
    /// <returns></returns>
    Task<ResultValidation> AddAsync(ContactViewModel contactViewModel);
    
    /// <summary>
    /// Atualiza um novo contato.
    /// </summary>
    /// <param name="contactViewModel">ViewModel do contato</param>
    /// <returns></returns>
    Task<ResultValidation> UpdateAsync(ContactViewModel contactViewModel);
    /// <summary>
    /// Exlui um contato.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid contactId);
}
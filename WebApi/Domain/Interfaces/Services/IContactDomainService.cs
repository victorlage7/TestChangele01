using WebApi.Domain.Entities;
using WebApi.Domain.Requests;

namespace WebApi.Domain.Interfaces.Services;

public interface IContactDomainService
{
    /// <summary>
    /// Inclui um novo contato.
    /// </summary>
    /// <param name="contact">Contato</param>
    /// <returns></returns>
    Task<ResultValidation> AddAsync(Contact contact);
    /// <summary>
    /// Atualiza um contato.
    /// </summary>
    /// <param name="contact">Contato</param>
    /// <returns></returns>
    Task<ResultValidation> UpdateAsync(Contact contact);
    /// <summary>
    /// Exclui um contato.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid contactId);
}
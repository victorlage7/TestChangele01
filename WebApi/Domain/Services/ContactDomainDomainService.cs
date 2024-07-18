using System.ComponentModel.DataAnnotations;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Domain.Interfaces.Services;

namespace WebApi.Domain.Services;

/// <summary>
/// Domínio de serviço de contato.
/// </summary>
public class ContactDomainDomainService : IContactDomainService
{
    private readonly IContactRepository _contactRepository;

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="contactRepository">Interface do repositorio do contato</param>
    public ContactDomainDomainService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    /// <summary>
    /// Inlcui um novo contato.
    /// </summary>
    /// <param name="contact">Contato</param>
    /// <returns>Retorna a validacao do contato</returns>
    public async Task<ResultValidation> AddAsync(Contact contact)
    {
        var validationResults = contact.GetValidationResults();

        if (validationResults.Any())
        {
            return new ResultValidation
            {
                ValidationResults = validationResults
            };
        }
        else
        {
            await _contactRepository.AddAsync(contact);
            return new ResultValidation
            {
                Object = contact
            };
        }
    }

    /// <summary>
    /// Atualiza um contato.
    /// </summary>
    /// <param name="contact">Contato</param>
    /// <returns>Retorna a validacao do contato</returns>
    public async Task<ResultValidation> UpdateAsync(Contact contact)
    {
        var validationResults = contact.GetValidationResults();

        if (validationResults.Any())
        {
            return new ResultValidation
            {
                ValidationResults = validationResults
            };
        }
        else
        {
            await _contactRepository.UpdateAsync(contact);
            return new ResultValidation
            {
                Object = contact
            };
        }

        ;
    }
    
    /// <summary>
    /// Excluir um contato.
    /// </summary>
    /// <param name="contactId">Identificador do contato</param>
    /// <returns>Retorna verdadeiro para contato excluido</returns>
    public async Task<bool> DeleteAsync(Guid contactId)
    {
        return await _contactRepository.DeleteAsync(contactId);
    }
}
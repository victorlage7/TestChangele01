using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Requests;
using WebApi.Interfaces;
using WebApi.ViewModels;

namespace WebApi.Controllers;

/// <summary>
/// Controller de Contatos.
/// </summary>
[Route("api/[controller]/")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IContactAppService _contactAppService;

    public ContactsController(IContactAppService contactAppService)
    {
        _contactAppService = contactAppService;
    }

    /// <summary>
    /// Inclui um novo contato.
    /// </summary>
    /// <param name="contactViewModel">Modelo de visualização do contato a ser criado.</param>
    /// <response code="200">Retorna o contato criado com sucesso.</response>
    /// <response code="400">Retorna uma mensagem de inconsistência caso validação do contato falha.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ContactViewModel), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> AddContactAsync([FromBody] ContactViewModel contactViewModel)
    {
        var resultValidation = await _contactAppService.AddAsync(contactViewModel);

        if (resultValidation.ValidationResults.Any())
            return BadRequest(resultValidation.ValidationResults);
        else
            return resultValidation.Object is not null
                ? Ok((ContactViewModel)resultValidation.Object)
                : BadRequest("Falha ao incluir o contato...");
    }

    /// <summary>
    /// Retorna todos os contatos.
    /// </summary>
    /// <param name="requestAll">Parâmetros de solicitação para filtrar os contatos.</param>
    /// <response code="200">Retorna uma lista de contatos.</response>
    /// <response code="404">Retorna uma mensagem de inconsistência caso nenhum contato seja encontrado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ContactViewModel[]), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetContactsAsync([FromQuery] ContactRequestAll requestAll)
    {
        var result = await _contactAppService.GetAllAsync(requestAll);

        if (result.Any())
            return Ok(result);

        return NotFound("Contato não localizado");
    }

    /// <summary>
    /// Retorna um contato pelo identificador(ID).
    /// </summary>
    /// <param name="contactId">Identificador do contato a ser obtido.</param>
    /// <response code="200">Retorna o contato correspondente ao ID.</response>
    /// <response code="404">Retorna uma mensagem de inconsistência caso nenhum contato seja encontrado.</response>
    [HttpGet("{contactId}")]
    [ProducesResponseType(typeof(ContactViewModel), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetByContactIdAsync(Guid contactId)
    {
        var result = await _contactAppService.GetByContactIdAsync(contactId);

        if (result is not null)
            return Ok(result);

        return NotFound("Contato não localizado");
    }


    /// <summary>
    /// Atualiza um contato existente.
    /// </summary>
    /// <param name="contactViewModel">Modelo de visualização do contato a ser atualizado.</param>
    /// <response code="200">Retorna o contato atualizado com sucesso.</response>
    /// <response code="400">Retorna uma mensagem de inconsistência caso a validação do contato falha.</response>
    [HttpPut]
    [ProducesResponseType(typeof(ContactViewModel), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> UpdateContactAsync([FromBody] ContactViewModel contactViewModel)
    {
        var resultValidation = await _contactAppService.UpdateAsync(contactViewModel);

        if (resultValidation.ValidationResults.Any())
            return BadRequest(resultValidation.ValidationResults);
        else
            return resultValidation.Object is not null
                ? Ok((ContactViewModel)resultValidation.Object)
                : BadRequest("Falha ao alterar o contato...");
    }

    /// <summary>
    /// Exclui um contato pelo idenficador(ID).
    /// </summary>
    /// <param name="contactId">Identificador do contato a ser excluído.</param>
    /// <response code="200">Retorna uma mensagem de sucesso após a exclusão do contato.</response>
    /// <response code="404">Retorna uma mensagem de erro caso nenhum contato seja encontrado.</response>
    [HttpDelete("{contactId}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> DeleteContactAsync(Guid contactId)
    {
        var result = await _contactAppService.DeleteAsync(contactId);

        return result ? Ok("Contato excluído com sucesso...") : NotFound("Contato não localizado");
    }
}
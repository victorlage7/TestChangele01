using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain;

public record ResultValidation
{
    /// <summary>
    /// Indica se a validação foi bem sucedida.
    /// </summary>
    public IEnumerable<ValidationResult> ValidationResults { get; init; } = new List<ValidationResult>();
    /// <summary>
    /// Objeto com entidade validada.
    /// </summary>
    public object? Object { get; init; }
}
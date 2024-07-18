using System.ComponentModel.DataAnnotations;
using WebApi.Domain.Enums;

namespace WebApi.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    public PhoneNumber(PhoneNumberType type, string countryCode, string areaCode, string number)
    {
        Type = type;
        CountryCode = countryCode;
        AreaCode = areaCode;
        Number = number;
    }

    /// <summary>
    /// Tipo Ex: Home
    /// </summary>
    public PhoneNumberType Type { get; }

    /// <summary>
    /// Código de área do país Ex: 55
    /// </summary>
    [Required(ErrorMessage = "O código do país é obrigatório.")]
    [StringLength(2, ErrorMessage = "O código do país deve ter no máximo 2 caracteres.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "O campo CountryCode só pode aceitar dígitos.")]
    public string CountryCode { get; }

    /// <summary>
    /// Código de área do estado / cidade Ex: 11
    /// </summary>
    [Required(ErrorMessage = "O código de área é obrigatório.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "O campo AreaCode só pode aceitar dígitos.")]
    [StringLength(4, ErrorMessage = "O código de área deve ter no máximo 4 caracteres.")]
    public string AreaCode { get; }

    /// <summary>
    /// Número de telefone. Ex: 992503949
    /// </summary>
    [Required(ErrorMessage = "O número de telefone é obrigatório.")]
    [RegularExpression(@"^\d{9,20}$", ErrorMessage = "O número de telefone deve conter de 9 a 20 dígitos.")]
    [StringLength(20, ErrorMessage = "O número de telefone deve ter no máximo 20 caracteres.")]
    public string Number { get; }

    public override string ToString()
    {
        return $"Type: {Type} CountryCode: {CountryCode}, AreaCode: {AreaCode}, Number: {Number}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return CountryCode;
        yield return AreaCode;
        yield return Number;
    }

    public IEnumerable<ValidationResult> GetValidationResults()
    {
        var validationContext = new ValidationContext(this, null, null);
        var validationResults = new List<ValidationResult>();

        Validator.TryValidateObject(this, validationContext, validationResults, true);

        return validationResults;
    }
}
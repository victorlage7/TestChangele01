using System.ComponentModel.DataAnnotations;
using WebApi.Domain.Enums;

namespace WebApi.Domain.ValueObjects;

public class EmailAddress : ValueObject
{
    public EmailAddress(EmailAddressType type, string address)
    {
        Type = type;
        Address = address.ToLower();
    }

    public EmailAddressType Type { get; }

    [EmailAddress] public string Address { get; }

    public override string ToString()
    {
        return $"Type: {Type.ToString()}, EmailAddress: {Address}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return Address;
    }

    public IEnumerable<ValidationResult> GetValidationResults()
    {
        var validationContext = new ValidationContext(this, null, null);
        var validationResults = new List<ValidationResult>();

        Validator.TryValidateObject(this, validationContext, validationResults, true);

        return validationResults;
    }
}
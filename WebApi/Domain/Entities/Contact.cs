using System.ComponentModel.DataAnnotations;
using WebApi.Domain.ValueObjects;

// ReSharper disable All

namespace WebApi.Domain.Entities
{
    public class Contact
    {
        /// <summary>
        /// Construtor para a criação de um novo contato.
        /// </summary>
        /// <param name="name">Nome do contato</param>
        public Contact(string name)
        {
            Name = name;

            EmailAddresses = new HashSet<EmailAddress>();
            PhoneNumbers = new HashSet<PhoneNumber>();
        }

        /// <summary>
        /// Identificador do contato.
        /// </summary>
        public Guid ContactId { get; init; }

        /// <summary>
        /// Nome do contato.
        /// </summary>
        [StringLength(250, ErrorMessage = "O nome do contado deve ter no máximo 250 caracteres.")]
        public string Name { get; private set; }

        /// <summary>
        /// Lista de endereços de e-mail do contato.
        /// </summary>
        public HashSet<EmailAddress> EmailAddresses { get; }

        /// <summary>
        /// Lista de números de telefone do contato.
        /// </summary>
        public HashSet<PhoneNumber> PhoneNumbers { get; }

        /// <summary>
        /// Adiciona um novo endereço de e-mail ao contato.
        /// </summary>
        /// <param name="emailAddress">E-mail do contato</param>
        public void AddEmailAddress(EmailAddress emailAddress)
        {
            EmailAddresses.Add(emailAddress);
        }

        /// <summary>
        /// Adiciona um novo número de telefone ao contato.
        /// </summary>
        /// <param name="phoneNumber">Telefone do contato</param>
        public void AddPhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumbers.Add(phoneNumber);
        }

        /// <summary>
        /// Remove um endereço de e-mail do contato.
        /// </summary>
        /// <param name="emailAddress">E-mail do contato</param>
        public void RemoveEmailAddress(EmailAddress emailAddress)
        {
            EmailAddresses.Remove(emailAddress);
        }

        /// <summary>
        /// Remove um número de telefone do contato.
        /// </summary>
        /// <param name="phoneNumber">Telefone do contato</param>
        public void RemovePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumbers.Remove(phoneNumber);
        }

        /// <summary>
        /// Remove todos os endereços de e-mail do contato.
        /// </summary>
        public void RemoveAllEmailAddress()
        {
            EmailAddresses.Clear();
        }

        /// <summary>
        /// Remove todos os números de telefone do contato.
        /// </summary>
        public void RemoveAllPhoneNumber()
        {
            PhoneNumbers.Clear();
        }

        /// <summary>
        /// Retorna uma lista de resultados de validação.
        /// </summary>
        /// <returns>Retorna o resultado da validação</returns>
        public ICollection<ValidationResult> GetValidationResults()
        {
            var emailValidationResults = EmailAddresses.SelectMany(email => email.GetValidationResults());
            var phoneNumbersValidationResults = PhoneNumbers.SelectMany(phone => phone.GetValidationResults());

            var validationContext = new ValidationContext(this, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, validationResults, validateAllProperties: true);

            List<ValidationResult> allValidationResults =
                [.. emailValidationResults, .. phoneNumbersValidationResults, .. validationResults];

            return allValidationResults;
        }

        /// <summary>
        /// Atualiza o nome do contato.
        /// </summary>
        /// <param name="name">Nome do contato</param>
        public void Update(string name)
        {
            Name = name;
        }
    }
}
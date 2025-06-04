using Moq;
using WebApi.Interfaces;
using WebApi.ViewModels;
using WebApi.Domain;
using WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Requests;
using System.ComponentModel.DataAnnotations;
using Core.Enums;
using Messaging.Interface;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace WebApiTest.Controllers;

public class ContactsControllerTest
{
    private Mock<IContactAppService> _contactAppServiceMock;
    private Mock<IRabbitMqService> _rabbitMqServiceMock;

    private IConfiguration _configuration;

    private ContactsController _controllerMock;
    private readonly ContactViewModel _invalidContactViewModel = new();

    private readonly ContactViewModel _contactViewModel = new (contactId: Guid.NewGuid(), name: "Test", 
        emailAddresses: new List<EmailAddressViewModel>
        {
            new (type: EmailAddressType.Commercial, address:"user@test.com" )
        },
        phoneNumbers: new List<PhoneNumberViewModel>
        {
            new (type: PhoneNumberType.Commercial, countryCode: "123", areaCode: "123", number: "999999999")
        }
    );

    [SetUp]
    public void Setup()
    {
        _contactAppServiceMock = new Mock<IContactAppService>();
        _rabbitMqServiceMock = new Mock<IRabbitMqService>();

        var inMemorySettings = new Dictionary<string, string>
        {
            {"RabbitMqConfiguration:QueueNameCreateContact", "queue"},
            {"RabbitMqConfiguration:QueueNameUpdateContact", "queue"},
            {"RabbitMqConfiguration:QueueNameDeleteContact", "queue"}
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _controllerMock = new ContactsController(_contactAppServiceMock.Object, _configuration, _rabbitMqServiceMock.Object);
    }

    [Test]
    public async Task GetContacts_ShouldReturnOk()
    {
        // Arrange
        var request = new ContactRequestAll
        {
            Name = "Test"
        };

        _contactAppServiceMock.Setup(x => x.GetAllAsync(request))
            .ReturnsAsync([_contactViewModel]);

        // Act
        var result = await _controllerMock.GetContactsAsync(request);
        var okResult = result as OkObjectResult;

        // Assert
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
    }

   

    [Test]
    public async Task CreateContact_ShouldReturnNotOk()
    {
        // Arrange

        _contactAppServiceMock.Setup(x => x.AddAsync(_invalidContactViewModel))
            .ReturnsAsync(new ResultValidation
            {
                ValidationResults = [new ValidationResult("error")]
            });

        // Act
        var result = await _controllerMock.AddContactAsync(_invalidContactViewModel);
        var badRequestResult = result as BadRequestObjectResult;

        // Assert
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task CreateContact_ShouldReturnOk()
    {
        // Arrange
        var connectionMock = new Mock<IConnection>();
        var modelMock = new Mock<IModel>();
        connectionMock.Setup(c => c.CreateModel()).Returns(modelMock.Object);
        _rabbitMqServiceMock.Setup(r => r.CreateChannel()).Returns(connectionMock.Object);

        _contactAppServiceMock.Setup(x => x.AddAsync(_contactViewModel))
            .ReturnsAsync(new ResultValidation { Object = _contactViewModel });

        // Act
        var result = await _controllerMock.AddContactAsync(_contactViewModel);
        var okResult = result as OkObjectResult;

        // Assert
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task GetContacts_ShouldReturnNotOk()
    {
        // Arrange
        var request = new ContactRequestAll
        {
            Name = "Test"
        };

        _contactAppServiceMock.Setup(x => x.GetAllAsync(request))
            .ReturnsAsync([]);

        // Act
        var result = await _controllerMock.GetContactsAsync(request);
        var notFoundResult = result as NotFoundObjectResult;

        // Assert
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
    }

  
}
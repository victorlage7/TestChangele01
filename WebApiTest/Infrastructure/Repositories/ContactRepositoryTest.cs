using Moq;
using WebApi.Infrastructure.Context;
using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Repositories;
using WebApi.Domain.Requests;

namespace WebApiTest.Controllers;

public class ContactRepositoryTest
{
    private ContactRepository _contactRepository;

    private readonly Contact _contact1 = new("Test1")
    {
        ContactId = Guid.NewGuid()
    };

    private readonly Contact _contact2 = new("Test2")
    {
        ContactId = Guid.NewGuid()
    };

    [SetUp]
    public void Setup()
    {
        var _contactsMock = new List<Contact>
        {
            _contact1,
            _contact2
        }.AsQueryable();

        var _mockSet = new Mock<DbSet<Contact>>();
        _mockSet.As<IQueryable>().Setup(m => m.Provider).Returns(_contactsMock.Provider);
        _mockSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(_contactsMock.Expression);
        _mockSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(_contactsMock.ElementType);
        _mockSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(() => _contactsMock.GetEnumerator());

        var options = new DbContextOptionsBuilder<TechChallenge1DbContext>()
            .Options;

        var _mockContext = new Mock<TechChallenge1DbContext>();
        _mockContext.Setup(c => c.Contacts).Returns(_mockSet.Object);
        _mockContext.Setup(c => c.Contacts.AsQueryable()).Returns(_contactsMock);
        _contactRepository = new ContactRepository(_mockContext.Object);
    }

    [Test]
    public async Task GetAll_ShouldFilterByName()
    {
        // Arrange
        var request = new ContactRequestAll
        {
            Name = _contact1.Name
        };

        // Act
        var result = await _contactRepository.GetAllAsync(request);
        
        //// Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo(_contact1.Name));
    }

    [Test]
    public async Task GetByContactId_ShouldReturnContact()
    {
        // Arrange & Act
        var result = await _contactRepository.GetByContactIdAsync(_contact1.ContactId);

        //// Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(_contact1.Name));
    }

    [Test]
    public async Task Create_ShouldCreateContact()
    {
        // Arrange
        var newContact = new Contact("teste");

        // Act
        var result = await _contactRepository.AddAsync(newContact);

        //// Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Contact>());
        Assert.That(result.Name, Is.EqualTo(newContact.Name));
    }

    [Test]
    public async Task Update_ShouldUpdateContact()
    {
        // Arrange
        var newContact = new Contact("NameUpdated")
        {
            ContactId = _contact1.ContactId
        };

        // Act
        var result = await _contactRepository.UpdateAsync(newContact);

        //// Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Contact>());
        Assert.That(result.Name, Is.EqualTo(newContact.Name));
    }

    [Test]
    public async Task Delete_ShouldReturnTrue()
    {
        // Arrange & Act
        var result = await _contactRepository.DeleteAsync(_contact1.ContactId);

        //// Assert
        Assert.That(result, Is.True);
    }
}
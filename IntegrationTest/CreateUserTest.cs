using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Core.Entities;

namespace IntegrationTest
{
    public  class CreateUserTest : IClassFixture<WebApiApplicationFactory>
    {

        private readonly HttpClient _httpClient;


        public CreateUserTest(WebApiApplicationFactory webApiApplicationFactory)
        {
            _httpClient = webApiApplicationFactory.CreateClient();

        }

        [Fact]
        public async Task GivenUserNotExiste_WhenUserControllerIsInvokedOnACtionAddUserAsync_ThenCreateUser()
        {
            // Arrange
            var request = new User(
                  username: "TesteName",
                  password: "PassTeste",
                  role: 0
            );

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/User", request);


            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

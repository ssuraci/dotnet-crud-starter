using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;
using Xunit;

namespace NetCrudStarter.Demo.IntegrationTesT;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly PostgreSqlContainer _postgreSqlContainer;
    private readonly HttpClient _client;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Set up PostgreSQL Testcontainer
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();

        _postgreSqlContainer.StartAsync().Wait();

        // Configure TestServer with connection string from Testcontainer
        factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var connString = _postgreSqlContainer.GetConnectionString();
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", connString)
                });
            });
        });

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetData_ReturnsExpectedResult()
    {
        // Act
        var response = await _client.GetAsync("/api/teacher");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Expected Response", content);
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}
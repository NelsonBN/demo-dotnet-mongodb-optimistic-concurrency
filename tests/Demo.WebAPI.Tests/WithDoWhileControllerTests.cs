using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Demo.WebAPI.Models;
using Demo.WebAPI.Tests.Config;
using PowerUtils.xUnit.Extensions.OrderTests;
using Xunit.Abstractions;

namespace Demo.WebAPI.Tests;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class WithDoWhileControllerTests
{
    private const string BASE_ENDPOINT = "WithDoWhile";
    private const int TOTAL_REQUEST = 100;

    private readonly IntegrationTestsFixture _testsFixture;
    private readonly ITestOutputHelper _output;

    private static Guid _id;

    public WithDoWhileControllerTests(
        IntegrationTestsFixture testsFixture,
        ITestOutputHelper output
    )
    {
        // Setup
        _testsFixture = testsFixture;
        _output = output;
    }


    [Fact]
    [TestPriority(1)]
    public async void WithDoWhile_Add_Id()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, BASE_ENDPOINT)
        {
            Content = JsonContent.Create("")
        };


        // Act
        var act = await _testsFixture.Client.SendAsync(postRequest);
        var content = await act.Content.ReadAsStringAsync();


        // Assert
        act.EnsureSuccessStatusCode();


        // Setup
        _id = JsonSerializer.Deserialize<Guid>(content);
    }


    [Fact]
    [TestPriority(2)]
    public async void WithDoWhile_Update()
    {
        // Arrange
        var uri = $"{BASE_ENDPOINT}/{_id}";


        // Act
        var tasks = Enumerable.Range(0, TOTAL_REQUEST).Select(async =>
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = JsonContent.Create("")
            };

            return _testsFixture.Client.SendAsync(postRequest);
        });

        var act = await Task.WhenAll(tasks);


        // Assert
        act.Should()
            .OnlyContain(o => o.IsSuccessStatusCode);
    }


    [Fact]
    [TestPriority(3)]
    public async void WithDoWhile_Get()
    {
        // Arrange && Act
        var act = await _testsFixture.Client.GetAsync($"{BASE_ENDPOINT}/{_id}");

        var content = await act.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<Counter>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        // Assert
        _output.WriteLine(response.Value.ToString());

        act.EnsureSuccessStatusCode();
        response.Value.Should()
            .Be(TOTAL_REQUEST);
    }
}

using ActiveMQ_PoC.ConsumerA.Consumers;
using System;
using System.Threading.Tasks;
using ActiveMQ_PoC.IntegrationTests.TestFramework;
using MassTransit.Testing;
using Xunit;

namespace ActiveMQ_PoC.IntegrationTests.Consumers;

public class GetTransportOrderStatusConsumerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _fixture;

    public GetTransportOrderStatusConsumerTests(ApiWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Consume_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var testHarness = _fixture.Services.GetTestHarness();
        using var client = _fixture.CreateClient();

        const string url = 

        // Act

        // Assert
        Assert.True(false);
    }
}
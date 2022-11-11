using System.Net.Http.Json;
using ActiveMQ_PoC.IntegrationTests.TestFramework;
using ActiveMQ_PoC.WebApp.Consumers;
using MassTransit;
using Bogus;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using ActiveMQ_PoC.Shared.Interfaces.Requests;
using FluentAssertions;

namespace ActiveMQ_PoC.IntegrationTests.Consumers;

public class GetStatusConsumerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _fixture;

    public GetStatusConsumerTests(ApiWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Consume_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var webApplicationFactory = _fixture.WebApplicationFactory;
        var testHarness = await StartTestHarness(webApplicationFactory.Services);
        var transportOrderStatusRequest = new GetStatusRequest(NewId.NextGuid().ToString());

        // Act
        SendMessage(transportOrderStatusRequest, webApplicationFactory.Services);

        // Assert
        await AssertMessageConsumedAsync(webApplicationFactory.Services, transportOrderStatusRequest, testHarness);
    }

    private static void SendMessage(IGetTransportOrderStatusRequest transportOrderStatusRequest, IServiceProvider provider)
    {
        var requestClient = provider.GetRequiredService<IBus>().CreateRequestClient<IGetTransportOrderStatusRequest>();
        requestClient.GetResponse<IGetTransportOrderStatusRequest>(transportOrderStatusRequest);
    }

    private static async Task<ITestHarness> StartTestHarness(IServiceProvider provider)
    {
        var testHarness = provider.GetRequiredService<ITestHarness>();
        await testHarness.Start();

        return testHarness;
    }

    private static async Task AssertMessageConsumedAsync(IServiceProvider serviceProvider, IGetTransportOrderStatusRequest transportOrderStatusRequest,
        ITestHarness testHarness)
    {
        Assert.True(await IsMessageConsumedByService(testHarness, transportOrderStatusRequest));
        Assert.True(await IsMessageConsumedByConsumer(serviceProvider, transportOrderStatusRequest));
    }

    private static async Task<bool> IsMessageConsumedByService(ITestHarness testHarness, IGetTransportOrderStatusRequest transportOrderStatusRequest)
        => await testHarness.Consumed.Any<IGetTransportOrderStatusRequest>(x => IsMessageReceived(x, transportOrderStatusRequest));

    private static async Task<bool> IsMessageConsumedByConsumer(IServiceProvider serviceProvider, IGetTransportOrderStatusRequest transportOrderStatusRequest)
        => await serviceProvider.GetRequiredService<IConsumerTestHarness<GetStatusConsumer>>()
            .Consumed.Any<IGetTransportOrderStatusRequest>(x => IsMessageReceived(x, transportOrderStatusRequest));

    private static bool IsMessageReceived(IReceivedMessage<IGetTransportOrderStatusRequest> receivedMessages, IGetTransportOrderStatusRequest expectedMessage)
    {
        var transportOrderStatusRequest = receivedMessages.MessageObject as IGetTransportOrderStatusRequest;

        return transportOrderStatusRequest?.ReferenceId == expectedMessage.ReferenceId;
    }
}
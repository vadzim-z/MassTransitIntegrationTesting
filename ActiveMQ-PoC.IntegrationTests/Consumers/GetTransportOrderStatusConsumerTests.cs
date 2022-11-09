using System.Net.Http.Json;
using ActiveMQ_PoC.IntegrationTests.TestFramework;
using ActiveMQ_PoC.WebApp.Consumers;
using MassTransit;
using Bogus;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using ActiveMQ_PoC.Shared.Interfaces.Requests;

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
        var faker = new Faker();
        var webApplicationFactory = _fixture.WebApplicationFactory;
        var testHarness = await StartTestHarness(webApplicationFactory.Services);
        var transportOrderStatusRequest = new GetTransportOrderStatusRequest(NewId.NextGuid().ToString());
        const string sendUrl = "WeatherForecast/Send";
        //var referenceId = new { ReferenceId = NewId.NextGuid().ToString() };

        //var endpointProvider = _fixture.Services.GetRequiredService<ISendEndpointProvider>();
        //var endpoint = await endpointProvider.GetSendEndpoint(new Uri($"queue:{QueueName.ConsumerA}"));

        // Act

        SendMessage(transportOrderStatusRequest, webApplicationFactory.Services);
        //var response = await client.PostAsync(sendUrl, JsonContent.Create(referenceId));
        //await endpoint.Send<ITransportOrderAmendedEvent>(new
        //{
        //    ReferenceId = faker.Random.AlphaNumeric(10),
        //    DatabaseId = faker.Random.Int(1, 999),
        //    EventDateTime = faker.Date.Future(),
        //    BlobUrl = faker.Internet.Url()
        //});

        // Assert
        await AssertMessageConsumedAsync(webApplicationFactory.Services, transportOrderStatusRequest, testHarness);
        //response.EnsureSuccessStatusCode();
        //Assert.True(false);
    }

    private static void SendMessage(IGetTransportOrderStatusRequest transportOrderStatusRequest/*string referenceId*/, IServiceProvider provider)
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
        => await serviceProvider.GetRequiredService<IConsumerTestHarness<GetTransportOrderStatusConsumer>>()
            .Consumed.Any<IGetTransportOrderStatusRequest>(x => IsMessageReceived(x, transportOrderStatusRequest));

    private static bool IsMessageReceived(IReceivedMessage<IGetTransportOrderStatusRequest> receivedMessages, IGetTransportOrderStatusRequest expectedMessage)
    {
        var transportOrderStatusRequest = receivedMessages.MessageObject as IGetTransportOrderStatusRequest;

        return transportOrderStatusRequest?.ReferenceId == expectedMessage.ReferenceId;
    }
}
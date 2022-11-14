using System.Net.Http.Json;
using ActiveMQ_PoC.IntegrationTests.TestFramework;
using ActiveMQ_PoC.IntegrationTests.TestFramework.Helpers;
using ActiveMQ_PoC.Shared.Entities;
using ActiveMQ_PoC.Shared.Interfaces.Repositories;
using ActiveMQ_PoC.WebApp.Consumers;
using MassTransit;
using Bogus;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using ActiveMQ_PoC.Shared.Interfaces.Requests;
using ActiveMQ_PoC.WebApp.Db;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ActiveMQ_PoC.IntegrationTests.Consumers;

public class GetStatusConsumerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _fixture;
    private readonly Mock<ITransportOrderRepository> _mockTransportOrderRepository;
    private readonly TransportOrderContext _transportOrderContext;
    private readonly Builder _builder;

    public GetStatusConsumerTests(ApiWebApplicationFactory fixture)
    {
        _fixture = fixture;
        _mockTransportOrderRepository = fixture.MockTransportOrderRepository;
        //_transportOrderContext = _fixture.WebApplicationFactory.Services.GetRequiredService<TransportOrderContext>();
        _builder = new Builder();
    }

    [Fact]
    public async Task Consume_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        using (var scope = _fixture.WebApplicationFactory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<TransportOrderContext>();

            Utilities.InitializeDbForTests(db);
        }

        var webApplicationFactory = _fixture.WebApplicationFactory;
        //var transportOrderEntity = _builder.CreateNew<TransportOrder>().Build();
        var testHarness = await StartTestHarness(webApplicationFactory.Services);
        var transportOrderStatusRequest = new GetStatusRequest(NewId.NextGuid().ToString());
        //await _transportOrderContext.TransportOrders.AddAsync(transportOrderEntity);
        //await _transportOrderContext.SaveChangesAsync();
        //_mockTransportOrderRepository.Setup(m => m.GetAllIdsAsync(CancellationToken.None));

        // Act
        await SendGetTransportOrderStatusMessage(transportOrderStatusRequest, webApplicationFactory.Services);
        var context = _fixture.WebApplicationFactory.Services.GetRequiredService<TransportOrderContext>();
        var tos = context.TransportOrders.ToList();
        // Assert
        await AssertMessageConsumedAsync(webApplicationFactory.Services, transportOrderStatusRequest, testHarness);
    }
    
    private static async Task SendGetTransportOrderStatusMessage(IGetTransportOrderStatusRequest transportOrderStatusRequest, IServiceProvider provider)
    {
        var requestClient = provider.GetRequiredService<IBus>().CreateRequestClient<IGetTransportOrderStatusRequest>();
        var response = await requestClient.GetResponse<ITransportOrderStatusResponse>(transportOrderStatusRequest);
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
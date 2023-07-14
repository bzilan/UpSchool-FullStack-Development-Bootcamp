using Application.Common.Models.WorkerService;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HubConnection _connection;
    private readonly HttpClient _httpClient;

    public Worker(ILogger<Worker> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;

        _connection = new HubConnectionBuilder()
           .WithUrl($"https://localhost:7217/Hubs/OrderHub")
           .WithAutomaticReconnect()
           .Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection.On<WorkerServiceNewOrderAddedDto>("NewOrderAdded", async (newOrderAddedDto) =>
        {


            Console.WriteLine(value: $"A new order added. Account Name is {newOrderAddedDto.Order.Id}");
            Console.WriteLine($"Our access token is {newOrderAddedDto.AccessToken}");

            // Crawler.StartAsync(order)

            await Task.Delay(10000, stoppingToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newOrderAddedDto.AccessToken);

            var result = await _httpClient.PostAsJsonAsync("Orders/CrawlerServiceExample", newOrderAddedDto, stoppingToken);

        });

        await _connection.StartAsync(stoppingToken);

        Console.WriteLine(_connection.State.ToString());
        Console.WriteLine(_connection.ConnectionId);
        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //await Task.Delay(1000, stoppingToken);
        }
    }
}
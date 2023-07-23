using Application.Common.Dtos;
using Application.Common.Models.WorkerService;
using Application.Features.Orders.Commands.Add;
using Domain.Entities;
using Domain.Enums;
using Domain.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CrawlerService
{
    public class Crawler
    {
        private readonly IWebDriver _driver;
        private readonly List<Product> Products;
        //private Guid orderId;
        private readonly HubConnection _orderHubConnection;
        private readonly HubConnection _seleniumLogHubConnection;
        private readonly HttpClient _httpClient;
        private string access_token;

        public Crawler(HttpClient httpClient)
        {
            Products = new List<Product>();

            _driver = new FirefoxDriver();

            _orderHubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7217/Hubs/OrderHub")
                .WithAutomaticReconnect()
                .Build();

            _seleniumLogHubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7217/Hubs/SeleniumLogHub")
                .WithAutomaticReconnect()
                .Build();

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"https://localhost:7217/api/");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            try
            {
                await _seleniumLogHubConnection.StartAsync();
                await _orderHubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }

            _orderHubConnection.On<WorkerServiceNewOrderAddedDto>(SignalRMethodKeys.Log.SendLogNotificationAsync, (tokenDto) =>
            {
                access_token = tokenDto.AccessToken;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            });

            _orderHubConnection.On<OrderDto>(SignalRMethodKeys.Order.Added, async (orderDto) =>
            {
                Products.Clear();
                await CrawlProductsAsync(orderDto, cancellationToken);
            });
        }

        private async Task CrawlProductsAsync(OrderDto orderDto, CancellationToken stoppingToken)
        {
            try
            {
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot started."));
                Console.WriteLine("How many items do you want to crawl?");
                int productCount;
                if (!int.TryParse(Console.ReadLine(), out productCount))
                {
                    Console.WriteLine("Invalid input! Please enter a number.");
                    return;
                }

                Console.WriteLine("What products do you want to crawl?");
                Console.WriteLine("A-) All");
                Console.WriteLine("B-) On Sale");
                Console.WriteLine("C-) Non Discount");

                string productOption = Console.ReadLine();

                var orderAddRequest = new OrderAddCommand()
                {
                    RequestedAmount = productCount,
                    TotalFoundAmount = 0,
                    ProductCrawlType = orderDto.ProductCrawlType,
                };

                switch (productOption.ToLower())
                {
                    case "a":
                        orderAddRequest.ProductCrawlType = ProductCrawlType.All;
                        break;
                    case "b":
                        orderAddRequest.ProductCrawlType = ProductCrawlType.OnDiscount;
                        break;
                    case "c":
                        orderAddRequest.ProductCrawlType = ProductCrawlType.NonDiscount;
                        break;
                    default:
                        Console.WriteLine("Invalid option! Please use a valid option.");
                        break;
                }

                var response = await _httpClient.PostAsJsonAsync("Orders/Add", orderAddRequest);
                if (!response.IsSuccessStatusCode)
                {
                    await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Failed to add order: {response.StatusCode}"));
                }

                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot stopped."));

                _driver.Quit();

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        async Task CrawlProductsAsync(OrderDto orderDto)
        {
            List<Product> products = new List<Product>();

            await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot started."));
            int productCount = 0;
            int page = 1;
            while (products.Count < productCount)
            {
                _driver.Navigate().GoToUrl($"https://4teker.net/?currentPage={page}");

                IReadOnlyCollection<IWebElement> productElements = _driver.FindElements(By.CssSelector(".card.h-100"));
                if (productElements != null)
                {
                    foreach (IWebElement productElement in productElements)
                    {
                        bool isOnSale = productElement.FindElements(By.CssSelector(".onsale")).Count > 0;
                        string name = productElement.FindElement(By.CssSelector(".fw-bolder.product-name")).Text;
                        string price = productElement.FindElement(By.CssSelector(".price")).Text;
                        string picture = productElement.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src");
                        price = price.Replace("$", "");
                        Guid orderId = Guid.NewGuid();

                        if (isOnSale)
                        {
                            string onSalePrice = productElement.FindElement(By.CssSelector(".sale-price")).Text;
                            onSalePrice = onSalePrice.Replace("$", "");
                            products.Add(new Product { Name = name, Price = decimal.Parse(price), Picture = picture, IsOnSale = isOnSale, SalePrice = decimal.Parse(onSalePrice), OrderId = orderId });
                        }
                        else
                        {
                            products.Add(new Product { Name = name, Price = decimal.Parse(price), Picture = picture, IsOnSale = isOnSale, OrderId = orderId });
                        }

                        if (products.Count >= productCount)
                            break;
                    }
                }
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Page {page} scanned."));
                Console.WriteLine($"{products.Count} products detected.");
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Total {products.Count} products detected."));
                page++;
            }

            foreach (Product product in products)
            {
                Console.WriteLine($"Product Name: {product.Name}");
                Console.WriteLine($"Product Price: {product.Price}");
                Console.WriteLine($"Product Image: {product.Picture}");
                Console.WriteLine($"Product Is On Sale: {product.IsOnSale}");
                Console.WriteLine($"Product Id: {product.OrderId}");
                if (product.IsOnSale)
                {
                    Console.WriteLine($"Product On Sale Price: {product.SalePrice}");
                }
                Console.WriteLine("S E R İ K Ö Z G E T İ R !");
            }
            await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync");

            async Task StopAsync()
            {
                _driver.Quit();
                await _seleniumLogHubConnection.StopAsync();
                await _orderHubConnection.StopAsync();
            }

            
        }
        SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);
    }
}

using Application.Common.Dtos;
using Application.Common.Models.WorkerService;
using Application.Features.OrderEvents.Commands.Add;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Update;
using Application.Features.Products.Commands.Add;
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
        private readonly HubConnection _orderHubConnection;
        private readonly HubConnection _seleniumLogHubConnection;
        private readonly HttpClient _httpClient;
        private int productCount = 0;
        private Guid orderId;
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
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) await DisposeAsync();

            try
            {
                await _seleniumLogHubConnection.StartAsync();
                await _orderHubConnection.StartAsync();
                OrderDto orderDto = new OrderDto();
            }
            catch (Exception ex)
            {
                await _seleniumLogHubConnection.SendAsync($"Error starting SignalR connection: {ex.Message}");
            }

            _seleniumLogHubConnection.On<WorkerServiceNewOrderAddedDto>(SignalRMethodKeys.Log.SendToken, (tokenDto) =>
            {
                access_token = tokenDto.AccessToken;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            });

            // Waiting for the signal that indicates a POST request has been sent to Add an Order.
            _orderHubConnection.On<OrderDto>(SignalRMethodKeys.Order.Added, async (orderDto) =>
            {
                // Clear the list before starting
                Products.Clear();

                await CrawlProductsAsync(orderDto, cancellationToken);

                await UpdateOrderAsync(orderDto.Id); // Updates the TotalFoundAmount after crawling process

            });

        }

        private async Task CrawlProductsAsync(OrderDto orderDto, CancellationToken stoppingToken)
        {
            try
            {
                await AddOrderEventAsync(OrderStatus.BotStarted, orderDto.Id);
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot started."));
                await _seleniumLogHubConnection.SendAsync("How many items do you want to crawl?");
                if (!int.TryParse(Console.ReadLine(), out productCount))
                {
                    await _seleniumLogHubConnection.SendAsync("Invalid input! Please enter a number.");
                    return;
                }

                await _seleniumLogHubConnection.SendAsync("What products do you want to crawl?");
                await _seleniumLogHubConnection.SendAsync("A-) All");
                await _seleniumLogHubConnection.SendAsync("B-) On Sale");
                await _seleniumLogHubConnection.SendAsync("C-) Non Discount");

                string productOption = Console.ReadLine();

                var orderAddRequest = new OrderAddCommand();

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
                        await _seleniumLogHubConnection.SendAsync("Invalid option! Please use a valid option.");
                        break;
                }

                var orderAddResponse = await _httpClient.PostAsJsonAsync("Orders/Add", orderAddRequest);
                if (!orderAddResponse.IsSuccessStatusCode)
                {
                    await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Failed to add order: {orderAddResponse.StatusCode}"));
                }

                orderId = await CreateNewOrderAsync(orderAddRequest.ProductCrawlType);
                if (orderId == Guid.Empty)
                {
                    return;
                }


                List<Product> products = new List<Product>();

                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot started."));
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
                    await _seleniumLogHubConnection.SendAsync($"{products.Count} products detected.");
                    await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Total {products.Count} products detected."));
                    page++;
                }
                if (products.Count != productCount)
                {
                    await _seleniumLogHubConnection.SendAsync("The number of crawled products does not match the requested amount.");
                    await UpdateOrderStatusAsync(orderId, OrderStatus.CrawlingFailed);
                    return;
                }

                // Add the products to the database.
                await AddProductsForOrderAsync(orderId, products);

                // Update order status to CrawlingCompleted.
                await UpdateOrderStatusAsync(orderId, OrderStatus.CrawlingCompleted);
                await AddOrderEventAsync(orderId, OrderStatus.BotStarted);

                foreach (Product product in products)
                {
                    await _seleniumLogHubConnection.SendAsync($"Product Name: {product.Name}");
                    await _seleniumLogHubConnection.SendAsync($"Product Price: {product.Price}");
                    await _seleniumLogHubConnection.SendAsync($"Product Image: {product.Picture}");
                    await _seleniumLogHubConnection.SendAsync($"Product Is On Sale: {product.IsOnSale}");
                    await _seleniumLogHubConnection.SendAsync($"Product Id: {product.OrderId}");
                    if (product.IsOnSale)
                    {
                        await _seleniumLogHubConnection.SendAsync($"Product On Sale Price: {product.SalePrice}");
                    }
                    await _seleniumLogHubConnection.SendAsync("S E R İ K Ö Z G E T İ R !");
                }
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync"); 

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (Exception ex)
            {
                await _seleniumLogHubConnection.SendAsync(ex.ToString());
                await UpdateOrderStatusAsync(orderId, OrderStatus.CrawlingFailed);
            }
        }
        private async Task<Guid> CreateNewOrderAsync(ProductCrawlType productCrawlType)
        {
            var orderAddRequest = new OrderAddCommand()
            {
                Id = Guid.NewGuid(),
                ProductCrawlType = productCrawlType,
                RequestedAmount = productCount,
            };

            var orderAddResponse = await _httpClient.PostAsJsonAsync("Orders/Add", orderAddRequest);
            if (!orderAddResponse.IsSuccessStatusCode)
            {
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Failed to add order: {orderAddResponse.StatusCode}"));
                return Guid.Empty;
            }

            var responseContent = await orderAddResponse.Content.ReadAsStringAsync();
            if (!Guid.TryParse(responseContent, out Guid orderId))
            {
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Failed to parse order ID from the response."));
                return Guid.Empty;
            }

            return orderId;
        }

        private async Task AddProductsForOrderAsync(Guid orderId, List<Product> products)
        {
            var productAddRequests = products.Select(product =>
                new ProductAddCommand()
                {
                    OrderId = orderId,
                    Name = product.Name,
                    Price = product.Price,
                    Picture = product.Picture,
                    IsOnSale = product.IsOnSale,
                    SalePrice = product.IsOnSale ? product.SalePrice : null,
                }).ToList();

            var productAddResponse = await _httpClient.PostAsJsonAsync("Products/Add", productAddRequests);
            if (!productAddResponse.IsSuccessStatusCode)
            {
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Failed to add products: {productAddResponse.StatusCode}"));
            }
        }

        private async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var orderUpdateRequest = new OrderUpdateCommand()
            {
                Id = orderId,
                TotalFoundAmount = productCount,
            };

            var orderUpdateResponse = await _httpClient.PutAsJsonAsync($"Orders/Update/{orderId}", orderUpdateRequest);
            if (!orderUpdateResponse.IsSuccessStatusCode)
            {
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Failed to update order status: {orderUpdateResponse.StatusCode}"));
            }
        }

        private async Task AddOrderEventAsync(Guid orderId, OrderStatus status)
        {
            var statusMessage = EnumExtensions.GetDisplayName(status);
            await _seleniumLogHubConnection.InvokeAsync(SignalRMethodKeys.Log.SendLogNotificationAsync, CreateLog(statusMessage));
            var orderEventAddRequest = new OrderEventAddCommand()
            {
                OrderId = orderId,
                Status = status,
            };

            var orderEventAddResponse = await _httpClient.PostAsJsonAsync("OrderEvents/Add", orderEventAddRequest);
            if (!orderEventAddResponse.IsSuccessStatusCode)
            {
                await _seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Failed to add order event: {orderEventAddResponse.StatusCode}"));
            }
        }
       
        private async Task DisposeAsync()
        {
            _driver.Quit();
            await _seleniumLogHubConnection.DisposeAsync();
            await _orderHubConnection.DisposeAsync();
        }

        SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);
    }
}

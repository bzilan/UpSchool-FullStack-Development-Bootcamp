using Application.Features.Orders.Commands.Add;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

Console.WriteLine("UpSchool Crawler");

Console.ReadKey();

new DriverManager().SetUpDriver(new FirefoxConfig());
IWebDriver driver = new FirefoxDriver();

var hubConnection = new HubConnectionBuilder()
    .WithUrl($"https://localhost:7182/Hubs/SeleniumLogHub")
    .WithAutomaticReconnect()
    .Build();
await hubConnection.StartAsync();
try
{
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot started."));
    var orderAddRequest = new OrderAddCommand();
    Console.WriteLine("How many items do you want to crawl?");
    string productCountInput = Console.ReadLine();
    int productCount;

    if (!int.TryParse(productCountInput, out productCount))
    {
        Console.WriteLine("Invalid login! Please enter a number.");
        return;
    }

    Console.WriteLine("What products do you want to crawl?");
    Console.WriteLine("A-) All");
    Console.WriteLine("B-) On Sale");
    Console.WriteLine("C-) Non Discount");

    string productOption = Console.ReadLine();

    switch (productOption.ToLower())
    {
        case "a":
            CrawlAllProductsAsync(productCount,hubConnection);
            orderAddRequest = new OrderAddCommand()
            {
                Id = Guid.NewGuid(),
                ProductCrawlType = ProductCrawlType.All,
            };
            break;
        case "b":
            CrawlOnDiscountProducts(productCount,hubConnection);
            orderAddRequest = new OrderAddCommand()
            {
                Id = Guid.NewGuid(),
                ProductCrawlType = ProductCrawlType.OnDiscount,
            };
            break;
        case "c":
            CrawlNonDiscountProducts(productCount,hubConnection);
            orderAddRequest = new OrderAddCommand()
            {
                Id = Guid.NewGuid(),
                ProductCrawlType = ProductCrawlType.NonDiscount,
            };
            break;
        default:
            Console.WriteLine("Invalid option! Please use a valid option.");
            break;
    }


    static async Task CrawlAllProductsAsync(int productCount,HubConnection hubConnection)
    {
        List<Product> products = new List<Product>();
        IWebDriver driver = new FirefoxDriver();

        Console.WriteLine($"All selected. The first {productCount} product is crawled...");
        int page = 1;
        while (products.Count < productCount)
        {
            driver.Navigate().GoToUrl($"https://4teker.net/?currentPage={page}");

            IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector(".card.h-100"));
            if (productElements != null)
            {
                foreach (IWebElement productElement in productElements)
                {
                    string name = productElement.FindElement(By.CssSelector(".fw-bolder.product-name")).Text;
                    string price = productElement.FindElement(By.CssSelector(".price")).Text;
                    string picture = productElement.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src");
                    price = price.Replace("$", "");
                    Guid orderId = Guid.NewGuid();
                    bool isOnSale = productElement.FindElements(By.CssSelector(".onsale")).Count > 0;
                    if (isOnSale == true)
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
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Page {page} scanned."));
            var totalProduct = products.Count;
            Console.WriteLine($"{totalProduct} products detected.");
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Total product {totalProduct} products detected."));
            
            
            page++;
        }
        foreach (Product product in products)
        {
            Console.WriteLine($"Product Name: {product.Name}");
            Console.WriteLine($"Product Price: {product.Price}");
            Console.WriteLine($"Product Image: {product.Picture}");
            Console.WriteLine($"Product Is On Sale: {product.IsOnSale}");
            Console.WriteLine($"Product Id: {product.OrderId}");
            if (product.IsOnSale == true)
            {
                Console.WriteLine($"Product On Sale Price: {product.SalePrice}");
            }
            Console.WriteLine("S E R İ K Ö Z G E T İ R !");
        }
        
        SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);
    }



    static async Task CrawlOnDiscountProducts(int productCount,HubConnection hubConnection)
    {
        List<Product> products = new List<Product>();
        IWebDriver driver = new FirefoxDriver();

        Console.WriteLine($"Discounts selected. The first {productCount} discounted product is crawled...");
        int page = 1;
        while (products.Count < productCount)
        {
            driver.Navigate().GoToUrl(($"https://4teker.net/?currentPage={page}"));

            IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector(".card.h-100"));
            if (productElements != null)
            {
                foreach (IWebElement productElement in productElements)
                {
                    bool isOnSale = productElement.FindElements(By.CssSelector(".onsale")).Count > 0;
                    if (isOnSale)
                    {
                        string name = productElement.FindElement(By.CssSelector(".fw-bolder.product-name")).Text;
                        string price = productElement.FindElement(By.CssSelector(".price")).Text;
                        string picture = productElement.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src");
                        price = price.Replace("$", "");
                        Guid orderId = Guid.NewGuid();
                        string onSalePrice = productElement.FindElement(By.CssSelector(".sale-price")).Text;
                        onSalePrice = onSalePrice.Replace("$", "");

                        products.Add(new Product
                        {
                            Name = name,
                            Price = decimal.Parse(price),
                            Picture = picture,
                            IsOnSale = isOnSale,
                            SalePrice = decimal.Parse(onSalePrice),
                            OrderId = orderId
                        });

                        if (products.Count >= productCount)
                            break;
                    }
                }
            }
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Page {page} scanned."));
            var totalProduct = products.Count;
            Console.WriteLine($"{totalProduct} products detected.");
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Total product {totalProduct} products detected."));
            page++;
        }

        foreach (Product product in products)
        {
            Console.WriteLine($"Product Name: {product.Name}");
            Console.WriteLine($"Product Price: {product.Price}");
            Console.WriteLine($"Product Image: {product.Picture}");
            Console.WriteLine($"Product Is On Sale: {product.IsOnSale}");
            Console.WriteLine($"Product On Sale Price: {product.SalePrice}");
            Console.WriteLine($"Product Id: {product.OrderId}");
            Console.WriteLine("S E R İ K Ö Z G E T İ R !");
        }
        SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);
    }

    static async Task CrawlNonDiscountProducts(int productCount, HubConnection hubConnection)
    {
        List<Product> products = new List<Product>();
        IWebDriver driver = new FirefoxDriver();

        Console.WriteLine($"Non discount  items were selected. The first {productCount} normal priced item is crawled...");
        int page = 1;
        while (products.Count < productCount)
        {
            driver.Navigate().GoToUrl(($"https://4teker.net/?currentPage={page}"));

            IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector(".card.h-100"));
            if (productElements != null)
            {
                foreach (IWebElement productElement in productElements)
                {
                    bool isOnSale = productElement.FindElements(By.CssSelector(".onsale")).Count > 0;
                    if (!isOnSale)
                    {
                        string name = productElement.FindElement(By.CssSelector(".fw-bolder.product-name")).Text;
                        string price = productElement.FindElement(By.CssSelector(".price")).Text;
                        string picture = productElement.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src");
                        price = price.Replace("$", "");
                        Guid orderId = Guid.NewGuid();

                        products.Add(new Product
                        {
                            Name = name,
                            Price = decimal.Parse(price),
                            Picture = picture,
                            IsOnSale = isOnSale,
                            OrderId = orderId
                        });

                        if (products.Count >= productCount)
                            break;
                    }
                }
            }
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Page {page} scanned."));
            var totalProduct = products.Count;
            Console.WriteLine($"{totalProduct} products detected.");
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Total product {totalProduct} products detected."));
            page++;
        }

        foreach (Product product in products)
        {
            Console.WriteLine($"Product Name: {product.Name}");
            Console.WriteLine($"Product Price: {product.Price}");
            Console.WriteLine($"Product Image: {product.Picture}");
            Console.WriteLine($"Product Is On Sale: {product.IsOnSale}");
            Console.WriteLine($"Product Id: {product.OrderId}");
            Console.WriteLine("S E R İ K Ö Z G E T İ R !");
        }
        SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);
    }
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot stopped."));

    driver.Quit();
}
catch (Exception exception)
{
    driver.Quit();
}
SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);
using Application.Common.Dtos;
using Application.Common.Models.WorkerService;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Update;
using Application.Features.Products.Commands.Add;
using Domain.Entities;
using Domain.Enums;
using Domain.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;

namespace CrawlerService
{
    public class Worker : BackgroundService
    {
        private readonly Crawler _crawler;

        private readonly HttpClient _httpClient;
        public Worker(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _crawler = new Crawler(_httpClient);
            
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _crawler.StartAsync(stoppingToken);
        }    
    }
}
        
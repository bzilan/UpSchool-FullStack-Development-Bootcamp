using Application.Common.Dtos;

namespace CrawlerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Crawler _crawler;
        private readonly HttpClient _httpClient;


        public Worker(ILogger<Worker> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _crawler = new Crawler(httpClient);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _crawler.StartAsync(stoppingToken);
        }
    }
}
        
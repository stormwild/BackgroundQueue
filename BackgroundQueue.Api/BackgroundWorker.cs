namespace BackgroundQueue.Api;

public class BackgroundWorker : BackgroundService
{
    private readonly IBackgroundQueue<Book> _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BackgroundWorker> _logger;

    public BackgroundWorker(IBackgroundQueue<Book> queue, IServiceScopeFactory scopeFactory,
        ILogger<BackgroundWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Type} is now running in the background.", nameof(BackgroundWorker));

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(500, stoppingToken);
                var book = _queue.Dequeue();

                if (book == null) continue;

                _logger.LogInformation("Book found! Starting to process ..");

                using (var scope = _scopeFactory.CreateScope())
                {
                    // var publisher = scope.ServiceProvider.GetRequiredService<IBookPublisher>();

                    // await publisher.Publish(book, stoppingToken);

                    // Do work
                    // Some work
                    await Task.Delay(3000);

                    _logger.LogInformation("Book processed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An error occurred when publishing a book. Exception: {@Exception}", ex);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace BackgroundQueue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBackgroundQueue<Book> _queue;

    public BookController(IBackgroundQueue<Book> queue)
    {
        _queue = queue;
    }

    [HttpPost]
    public async Task<IActionResult> Publish([FromBody] Book book)
    {
        _queue.Enqueue(book);

        return Accepted();
    }
}

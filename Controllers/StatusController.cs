using Microsoft.AspNetCore.Mvc;
using AtlanticQiAPI.Models;

[Route("api/Status")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly AppDbContext _context;

    public StatusController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetStatuses()
    {
        var statuses = _context.Status.ToList();
        return Ok(statuses);
    }
}

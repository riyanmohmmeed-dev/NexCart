using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexCart.Application.Categories.Queries.GetCategories;

namespace NexCart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all categories</summary>
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] bool includeInactive = false)
    {
        var result = await _mediator.Send(new GetCategoriesQuery(includeInactive));
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }
}

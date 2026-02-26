using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexCart.Application.Orders.Commands.PlaceOrder;
using NexCart.Application.Orders.Queries.GetOrders;

namespace NexCart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get paginated orders with optional filtering</summary>
    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? customerId = null,
        [FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetOrdersQuery(page, pageSize, customerId, status));
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>Place a new order</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetOrders), null, result.Data)
            : StatusCode(result.StatusCode, new { error = result.Error });
    }
}

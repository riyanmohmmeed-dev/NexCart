using Microsoft.AspNetCore.Mvc;
using NexCart.Domain.Interfaces;

namespace NexCart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    /// <summary>Get dashboard analytics summary</summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var (products, totalProducts) = await _unitOfWork.Products.GetPagedAsync(1, 1);
        var (orders, totalOrders) = await _unitOfWork.Orders.GetPagedAsync(1, 1);
        var (customers, totalCustomers) = await _unitOfWork.Customers.GetPagedAsync(1, 1);

        // Get recent orders for revenue calculation
        var (recentOrders, _) = await _unitOfWork.Orders.GetPagedAsync(1, 100);
        var totalRevenue = recentOrders.Sum(o => o.TotalAmount.Amount);

        return Ok(new
        {
            TotalProducts = totalProducts,
            TotalOrders = totalOrders,
            TotalCustomers = totalCustomers,
            TotalRevenue = totalRevenue,
            Currency = "USD"
        });
    }
}

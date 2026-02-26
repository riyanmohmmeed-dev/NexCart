using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Application.Products.Queries.GetProducts;

namespace NexCart.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    string Sku,
    decimal Price,
    int StockQuantity,
    Guid CategoryId,
    string? ImageUrl = null,
    string? Brand = null,
    bool IsFeatured = false,
    decimal? CompareAtPrice = null
) : IRequest<Result<ProductDto>>;

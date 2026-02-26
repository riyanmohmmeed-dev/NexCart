using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Application.Products.Queries.GetProducts;
using NexCart.Domain.Interfaces;

namespace NexCart.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductByIdHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id, ct);
        if (product is null)
            return Result<ProductDto>.NotFound($"Product with ID '{request.Id}' not found.");

        return Result<ProductDto>.Success(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Slug = product.Slug,
            Sku = product.Sku,
            Price = product.Price.Amount,
            Currency = product.Price.Currency,
            CompareAtPrice = product.CompareAtPrice?.Amount,
            DiscountPercentage = product.DiscountPercentage,
            StockQuantity = product.StockQuantity,
            IsInStock = product.IsInStock,
            IsActive = product.IsActive,
            IsFeatured = product.IsFeatured,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            AverageRating = product.AverageRating,
            ReviewCount = product.ReviewCount,
            CreatedAt = product.CreatedAt
        });
    }
}

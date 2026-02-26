using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Domain.Interfaces;

namespace NexCart.Application.Products.Queries.GetProducts;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, Result<PagedResult<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<ProductDto>>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var (products, totalCount) = await _unitOfWork.Products.GetPagedAsync(
            request.Page, request.PageSize, request.Search,
            request.CategoryId, true, request.SortBy, request.SortDescending, ct);

        var items = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Slug = p.Slug,
            Sku = p.Sku,
            Price = p.Price.Amount,
            Currency = p.Price.Currency,
            CompareAtPrice = p.CompareAtPrice?.Amount,
            DiscountPercentage = p.DiscountPercentage,
            StockQuantity = p.StockQuantity,
            IsInStock = p.IsInStock,
            IsActive = p.IsActive,
            IsFeatured = p.IsFeatured,
            ImageUrl = p.ImageUrl,
            Brand = p.Brand,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty,
            AverageRating = p.AverageRating,
            ReviewCount = p.ReviewCount,
            CreatedAt = p.CreatedAt
        }).ToList();

        var result = new PagedResult<ProductDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Result<PagedResult<ProductDto>>.Success(result);
    }
}

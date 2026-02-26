using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Application.Products.Queries.GetProducts;
using NexCart.Domain.Entities;
using NexCart.Domain.Interfaces;

namespace NexCart.Application.Products.Commands.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
            return Result<ProductDto>.NotFound($"Category with ID '{request.CategoryId}' not found.");

        var product = Product.Create(
            request.Name, request.Description, request.Sku,
            request.Price, request.StockQuantity, request.CategoryId,
            request.ImageUrl, request.Brand, request.IsFeatured);

        if (request.CompareAtPrice.HasValue)
            product.SetCompareAtPrice(request.CompareAtPrice.Value);

        await _unitOfWork.Products.AddAsync(product, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var dto = new ProductDto
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
            CategoryName = category.Name,
            AverageRating = product.AverageRating,
            ReviewCount = product.ReviewCount,
            CreatedAt = product.CreatedAt
        };

        return Result<ProductDto>.Created(dto);
    }
}

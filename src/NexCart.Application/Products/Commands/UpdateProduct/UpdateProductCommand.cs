using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Application.Products.Queries.GetProducts;
using NexCart.Domain.Interfaces;

namespace NexCart.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id, string Name, string Description, decimal Price,
    int StockQuantity, Guid CategoryId, string? ImageUrl,
    string? Brand, bool IsFeatured
) : IRequest<Result<ProductDto>>;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id, ct);
        if (product is null)
            return Result<ProductDto>.NotFound($"Product with ID '{request.Id}' not found.");

        product.Update(request.Name, request.Description, request.Price,
            request.StockQuantity, request.CategoryId, request.ImageUrl,
            request.Brand, request.IsFeatured);

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<ProductDto>.Success(new ProductDto
        {
            Id = product.Id, Name = product.Name, Description = product.Description,
            Slug = product.Slug, Sku = product.Sku, Price = product.Price.Amount,
            Currency = product.Price.Currency, StockQuantity = product.StockQuantity,
            IsInStock = product.IsInStock, IsActive = product.IsActive,
            IsFeatured = product.IsFeatured, ImageUrl = product.ImageUrl,
            Brand = product.Brand, CategoryId = product.CategoryId,
            AverageRating = product.AverageRating, ReviewCount = product.ReviewCount,
            CreatedAt = product.CreatedAt
        });
    }
}

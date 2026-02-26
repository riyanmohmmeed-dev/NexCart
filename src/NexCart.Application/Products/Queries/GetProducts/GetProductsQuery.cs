using MediatR;
using NexCart.Application.Common.Models;

namespace NexCart.Application.Products.Queries.GetProducts;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 12,
    string? Search = null,
    Guid? CategoryId = null,
    string? SortBy = null,
    bool SortDescending = false
) : IRequest<Result<PagedResult<ProductDto>>>;

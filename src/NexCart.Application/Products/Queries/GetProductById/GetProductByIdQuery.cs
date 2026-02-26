using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Application.Products.Queries.GetProducts;

namespace NexCart.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;

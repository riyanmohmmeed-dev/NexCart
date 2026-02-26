using MediatR;
using NexCart.Application.Common.Models;

namespace NexCart.Application.Categories.Queries.GetCategories;

public record CategoryDto(Guid Id, string Name, string Description, string Slug, string? ImageUrl, bool IsActive, Guid? ParentCategoryId);

public record GetCategoriesQuery(bool IncludeInactive = false) : IRequest<Result<List<CategoryDto>>>;

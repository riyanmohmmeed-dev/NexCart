using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Domain.Interfaces;

namespace NexCart.Application.Categories.Queries.GetCategories;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(request.IncludeInactive, ct);

        var dtos = categories.Select(c => new CategoryDto(
            c.Id, c.Name, c.Description, c.Slug, c.ImageUrl, c.IsActive, c.ParentCategoryId
        )).ToList();

        return Result<List<CategoryDto>>.Success(dtos);
    }
}

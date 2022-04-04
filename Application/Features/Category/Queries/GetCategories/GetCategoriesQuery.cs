﻿namespace Application.Features.Category.Queries.GetCategories;

using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetCategoriesQuery : IRequest<CategoriesDto>
{
    public Guid UserUniqueId { get; set; }
    public CategoryType CategoryType { get; set; }
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoriesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoriesDto> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.Category
            .Where(x => x.UserUniqueId == request.UserUniqueId && x.CategoryType == request.CategoryType)
            .OrderBy(x => x.Name)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new CategoriesDto
        {
            Items = items
        };
    }
}
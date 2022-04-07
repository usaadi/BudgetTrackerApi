using Application.Features.Category.Queries.GetCategories;
using Application.Features.Transaction.Queries.GetTransactions;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(x => x.CategoryType, opt => opt.MapFrom(y => y.CategoryTypeLookupId));

        CreateMap<Transaction, TransactionDto>()
            .ForMember(x => x.TransactionType, opt => opt.MapFrom(y => y.TransactionTypeLookupId))
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(y => y.Category.Name));
    }
}
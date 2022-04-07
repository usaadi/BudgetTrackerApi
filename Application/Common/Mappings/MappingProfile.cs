using Application.Features.Category.Queries.GetCategories;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(x => x.CategoryType, opt => opt.MapFrom(y => y.CategoryTypeLookupId));
        CreateMap<CategoryDto, Category>()
            .ForMember(x => x.CategoryTypeLookupId, opt => opt.MapFrom(y => y.CategoryType));

        //CreateMap<Transaction, TransactionDto>()
        //    .ForMember(x => x.TransactionType, opt => opt.MapFrom(y => y.TransactionTypeLookupId));
        //CreateMap<TransactionyDto, Transaction>()
        //    .ForMember(x => x.TransactionTypeLookupId, opt => opt.MapFrom(y => y.TransactionType));
    }
}
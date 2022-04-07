using Application.Features.Category.Queries.GetCategories;
using Application.Features.Transaction.Queries.GetTransactions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(x => x.TransactionType, opt => opt.MapFrom(y => (TransactionType)y.TransactionTypeLookupId));

        CreateMap<Transaction, TransactionDto>()
            .ForMember(x => x.TransactionType, opt => opt.MapFrom(y => (TransactionType)y.Category.TransactionTypeLookupId));
    }
}

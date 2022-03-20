using AutoMapper;
using transaction.api.Models;
using transaction.application.Features.Transactions.Commands.Deposit;

namespace transaction.api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateDepositDto, DepositCommand>();
    }
}

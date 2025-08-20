using AutoMapper;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Infrastructure.Data.CouchDB.Models;

namespace SupportHelper.Infrastructure.CrossCutting.AutoMapper.Profiles
{
    public class MachineProfile : Profile
    {
        public MachineProfile()
        {
            CreateMap<MachineAggregates, MachineModel>();
        }
    }
}

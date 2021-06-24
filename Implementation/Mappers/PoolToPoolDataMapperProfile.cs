using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;

namespace GudelIdService.Implementation.Mappers
{
    public class PoolToPoolDataMapperProfile : Profile
    {
        public PoolToPoolDataMapperProfile()
        {
            CreateMap<Pool, PoolData>().ForMember(m => m.Name, map => map.Ignore()).ForMember(m => m.Description, map => map.Ignore());
            CreateMap<PoolData, Pool>().ForMember(m => m.Name, map => map.Ignore()).ForMember(m => m.Description, map => map.Ignore());
        }
    }
}

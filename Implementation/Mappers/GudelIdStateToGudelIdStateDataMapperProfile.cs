using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;

namespace GudelIdService.Implementation.Mappers
{
    public class GudelIdStateToGudelIdStateDataMapperProfile : Profile
    {
        public GudelIdStateToGudelIdStateDataMapperProfile()
        {
            CreateMap<GudelIdState, GudelIdStateData>();
        }
    }
}

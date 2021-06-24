using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;

namespace GudelIdService.Implementation.Mappers
{
    public class GudelIdToGudelIdDataMapperProfile : Profile
    {
        public GudelIdToGudelIdDataMapperProfile()
        {
            CreateMap<GudelId, GudelIdData>();
            CreateMap<GudelIdData, GudelId>();
        }
    }
}
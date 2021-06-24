using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using System.Linq;

namespace GudelIdService.Implementation.Mappers
{
    public class ExtraFieldDefinitionToExtraFieldDefinitionDataMapperProfile : Profile
    {
        public ExtraFieldDefinitionToExtraFieldDefinitionDataMapperProfile()
        {
            CreateMap<ExtraFieldDefinition, ExtraFieldDefinitionData>()
                .ForMember(m => m.Name, map => map.Ignore()).ForMember(m => m.Description, map => map.Ignore())
                .ForMember(_ => _.State, map => map.MapFrom(x => x.ExtraFieldDefinitionGudelIdState== null?null:x.ExtraFieldDefinitionGudelIdState.Select(_ => _.GudelIdStateId)));

            CreateMap<ExtraFieldDefinitionData, ExtraFieldDefinition>()
                .ForMember(m => m.Name, map => map.Ignore()).ForMember(m => m.Description, map => map.Ignore())
                .ForMember(_ => _.ExtraFieldDefinitionGudelIdState, map => map.Ignore());
        }
    }
}

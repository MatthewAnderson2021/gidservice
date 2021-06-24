using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Services
{
    public class GudelIdStateService : IGudelIdStateService
    {
        private readonly IGudelIdStateRepository _stateRepository;
        private readonly IMapper _mapper;

        public GudelIdStateService(IGudelIdStateRepository stateRepository, IMapper mapper)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
        }

        public async Task<List<GudelIdStateData>> FindAll(string language)
        {
            var states = await _stateRepository.FindAll();
            return states.Select(_ => GetGudelIdStateData(language, _)).ToList();
        }

        public async Task<GudelIdStateData> FindById(int id, string language)
        {
            var state = await _stateRepository.FindById(id);
            return GetGudelIdStateData(language, state);
        }

        private GudelIdStateData GetGudelIdStateData(string language, GudelIdState state)
        {
            if (state == null) return null;
            return _mapper.Map<GudelIdStateData>(state, options => options.AfterMap((src, dest) =>
            {
                var srModel = ((GudelIdState)src);
                var desModel = ((GudelIdStateData)dest);

                desModel.Description = srModel.Description != null && srModel.Description.ContainsKey(language) ? srModel.Description[language] : string.Empty;
                desModel.Name = srModel.Name != null && srModel.Name.ContainsKey(language) ? srModel.Name[language] : string.Empty;
                var definitions = srModel.ExtraFieldDefinition
                    .Select(extraFieldDefinitionGudelIdState => _mapper.Map<ExtraFieldDefinitionData>(
                        extraFieldDefinitionGudelIdState, opt => opt
                            .AfterMap((src, dest) =>
                            {
                                var srModel = ((ExtraFieldDefinition)src);
                                var desModel = ((ExtraFieldDefinitionData)dest);
                                desModel.Description =
                                    srModel.Description != null && srModel.Description.ContainsKey(language) ? srModel.Description[language] : string.Empty;
                                desModel.Name = srModel.Name != null && srModel.Name.ContainsKey(language) ? srModel.Name[language] : string.Empty;
                            }))).ToList();

                desModel.ExtraFieldDefinitions = definitions;
                foreach(var follow in desModel.AllowedFollowupStates)
                {
                    follow.AllowedFollowupStates = new List<GudelIdStateData>(); follow.PossiblePreviousStates = new List<GudelIdStateData>();
                    var srFollow = srModel.AllowedFollowupStates.Find(_ => _.Id == follow.Id);
                    follow.Name = srFollow.Name != null && srFollow.Name.ContainsKey(language) ? srFollow.Name[language] : string.Empty;
                    follow.Description = srFollow.Description != null && srFollow.Description.ContainsKey(language) ? srFollow.Description[language] : string.Empty;
                }
                foreach (var previous in desModel.PossiblePreviousStates)
                {
                    previous.AllowedFollowupStates = new List<GudelIdStateData>(); previous.PossiblePreviousStates = new List<GudelIdStateData>();
                    var srPrevious = srModel.PossiblePreviousStates.Find(_ => _.Id == previous.Id);
                    previous.Name = srPrevious.Name != null && srPrevious.Name.ContainsKey(language) ? srPrevious.Name[language] : string.Empty;
                    previous.Description = srPrevious.Description != null && srPrevious.Description.ContainsKey(language) ? srPrevious.Description[language] : string.Empty;
                }
            }));
        }
    }
}
using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
namespace GudelIdService.Implementation.Services
{
    public class ExtraFieldService : IExtraFieldService
    {
        private readonly IExtraFieldRepository _extraFieldRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ExtraFieldService(IExtraFieldRepository extraFieldRepository, IMapper mapper, AppDbContext context)
        {
            _extraFieldRepository = extraFieldRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ExtraFieldDefinitionData> AddDefinition(ExtraFieldDefinitionData data, string language)
        {
            var definition = MapToExtraFieldDefinition(data, null, null);
            var added = await _extraFieldRepository.AddDefinition(definition);
            return MapToExtraFieldFedinitionData(added, language);
        }

        public async Task<ExtraField> CreateField(GudelIdData gudelId, ExtraFieldDefinitionData extraFieldDefinition, Dictionary<string, string> value, string language)
        {
            ExtraField extraField = new ExtraField()
            {
                ExtraFieldDefinitionId = extraFieldDefinition.Id,
                GudelId = gudelId.Id,
                Value = value
            };
            return await _extraFieldRepository.CreateExtraField(extraField);

        }
       public async Task<ExtraField> CreateOrUpdateField(GudelIdData gudelId, ExtraFieldDefinitionData extraFieldDefinition, Dictionary<string, string> value, string language)
        {
            ExtraField extraField = new ExtraField()
            {
                ExtraFieldDefinitionId = extraFieldDefinition.Id,
                GudelId = gudelId.Id,
                Value = value
            };
            var field = await _context.ExtraField.Where(f => f.GudelId == gudelId.Id && f.ExtraFieldDefinitionId == extraFieldDefinition.Id).FirstOrDefaultAsync();
            if(field != null) {
                field.Value = value;
                return await _extraFieldRepository.UpdateExtraField(field);
            }
            
          
            return await _extraFieldRepository.CreateExtraField(extraField);

        }


        public async Task<List<ExtraFieldDefinitionData>> FindAllDefinitions(string language)
        {
            var items = await _extraFieldRepository.FindAllDefinitions();

            var result = items.Select(_ =>
            {
                return MapToExtraFieldFedinitionData(_, language);
            }).ToList();

            return result;
        }


        public async Task<ExtraFieldDefinitionData> FindDefinitionByKey(string key, string language)
        {
            var items = await _extraFieldRepository.FindDefinitionByKey(key);

            var result = MapToExtraFieldFedinitionData(items, language);

            return result;
        }

        public async Task<List<ExtraFieldDefinitionData>> FindDefinitionsByState(int stateId, string language)
        {
            var items = await _extraFieldRepository.FindDefinitionsByStateId(stateId);

            var result = items.Select(_ => MapToExtraFieldFedinitionData(_, language)).ToList(); ;

            return result;
        }

        public async Task<int> RemoveDefinition(string key)
        {
            return await _extraFieldRepository.RemoveDefinition(key);
        }

        public async Task<ExtraFieldDefinitionData> UpdateDefinition(ExtraFieldDefinitionData definition, string language)
        {
            var exist = await _extraFieldRepository.FindDefinitionByKey(definition.Key);
            var extraFieldDefinition = MapToExtraFieldDefinition(definition, language, exist);
            var updated = await _extraFieldRepository.UpdateDefinition(extraFieldDefinition);

            return MapToExtraFieldFedinitionData(updated, language);
        }

        public async Task<ExtraField> UpdateExtraField(string gudelId, int fieldDefId, Dictionary<string, string> value)
        {
            var field = await _extraFieldRepository.FindExtraField(gudelId, fieldDefId);

            if (field == null) return null;

            field.Value = value;

            return await _extraFieldRepository.UpdateExtraField(field);
        }

        public ExtraFieldDefinition MapToExtraFieldDefinition(ExtraFieldDefinitionData data, string language, ExtraFieldDefinition existing)
        {
            return _mapper.Map<ExtraFieldDefinition>(data, options => options.AfterMap((src, dest) =>
            {
                string[] langs = language != null ? new string[] { language } : ConfigService.KNOWN_LANGS;
                var srModel = ((ExtraFieldDefinitionData)src);
                var desModel = ((ExtraFieldDefinition)dest);
                desModel.Description = existing == null || existing.Description == null ? new Dictionary<string, string>() : existing.Description;
                desModel.Name = existing == null || existing.Name == null ? new Dictionary<string, string>() : existing.Name;
                foreach (var lang in langs)
                {
                    desModel.Description[lang] = srModel.Description ?? string.Empty;
                    desModel.Name[lang] = srModel.Name ?? string.Empty;
                }

                desModel.ExtraFieldDefinitionGudelIdState = new List<ExtraFieldDefinitionGudelIdState>();
                foreach (var stateId in srModel.State)
                {
                    ExtraFieldDefinitionGudelIdState stateLink = new ExtraFieldDefinitionGudelIdState();
                    stateLink.ExtraFieldDefinitionId = data.Id;
                    stateLink.GudelIdStateId = stateId;
                    ((List<ExtraFieldDefinitionGudelIdState>) desModel.ExtraFieldDefinitionGudelIdState).Add(stateLink);
                }


            }));
        }
        public ExtraFieldDefinitionData MapToExtraFieldFedinitionData(ExtraFieldDefinition item, string language)
        {
            if (item == null) return null;
            return _mapper.Map<ExtraFieldDefinitionData>(item, options => options.AfterMap((src, dest) =>
            {
                var srModel = ((ExtraFieldDefinition)src);
                var desModel = ((ExtraFieldDefinitionData)dest);
                desModel.Description = srModel.Description != null && srModel.Description.ContainsKey(language) ? srModel.Description[language] : string.Empty;
                desModel.Name = srModel.Name != null && srModel.Name.ContainsKey(language) ? srModel.Name[language] : string.Empty;
                desModel.State = new List<int>();
                foreach (var state in srModel.ExtraFieldDefinitionGudelIdState)
                {
                    desModel.State.Add(state.GudelIdStateId);
                }
            }));
        }
    }
}
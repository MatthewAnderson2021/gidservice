using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IExtraFieldService
    {
        Task<ExtraField> CreateField(GudelIdData gudelId, ExtraFieldDefinitionData extraFieldDefinition, Dictionary<string, string> value, string language);
        Task<List<ExtraFieldDefinitionData>> FindAllDefinitions(string language);
        Task<ExtraFieldDefinitionData> FindDefinitionByKey(string key, string language);
        Task<List<ExtraFieldDefinitionData>> FindDefinitionsByState(int stateId, string language);
        Task<ExtraFieldDefinitionData> UpdateDefinition(ExtraFieldDefinitionData definition, string language);
        Task<ExtraFieldDefinitionData> AddDefinition(ExtraFieldDefinitionData definition, string language);
        Task<int> RemoveDefinition(string key);
        Task<ExtraField> UpdateExtraField(string gudelId, int fieldDefId, Dictionary<string, string> value);
        Task<ExtraField> CreateOrUpdateField(GudelIdData gudelId, ExtraFieldDefinitionData extraFieldDefinition, Dictionary<string, string> value, string language);
       

    }
}
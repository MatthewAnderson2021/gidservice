using GudelIdService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IExtraFieldRepository
    {
        Task<List<ExtraFieldDefinition>> FindAllDefinitions();
        Task<ExtraFieldDefinition> FindDefinitionByKey(string key);
        Task<List<ExtraFieldDefinition>> FindDefinitionsByStateId(int stateId);
        Task<ExtraFieldDefinition> UpdateDefinition(ExtraFieldDefinition definition);
        Task<ExtraFieldDefinition> AddDefinition(ExtraFieldDefinition definition);
        Task<int> RemoveDefinition(string key);

        Task<ExtraField> FindExtraField(string gudelId, int extraFieldDefId);
        Task<ExtraField> UpdateExtraField(ExtraField extrafield);
        Task<ExtraField> CreateExtraField(ExtraField extrafield);

    }
}
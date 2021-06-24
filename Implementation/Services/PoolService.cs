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
    public class PoolService : IPoolService
    {
        private readonly IPoolRepository _poolRepository;
        private readonly IGudelIdRepository _gudelIdRepository;
        private readonly IMapper _mapper;
        private readonly IConfigService _config;

        public PoolService(IPoolRepository poolRepository, IGudelIdRepository gudelIdRepository, IMapper mapper, IConfigService config)
        {
            _poolRepository = poolRepository;
            _gudelIdRepository = gudelIdRepository;
            _mapper = mapper;
            _config = config;
        }

        private PoolData mapPoolToData(Pool pool, string language)
        {
            if(string.IsNullOrEmpty(language))
            { language = ConfigService.LANG_DEFAULT; }

            return _mapper.Map<PoolData>(pool, options => options.AfterMap((src, dest) =>
            {
                var srModel = ((Pool)src);
                var desModel = ((PoolData)dest);
                desModel.Description = srModel.Description != null && srModel.Description.ContainsKey(language) ? srModel.Description[language] : string.Empty;
                desModel.Name = srModel.Name != null && srModel.Name.ContainsKey(language) ? srModel.Name[language] : string.Empty;
            }));
        }
        private Pool mapDataToPool(PoolData pooldata)
        {
            return mapDataToPool(pooldata, null, null);
        }
        private Pool mapDataToPool(PoolData pooldata, string language, Pool exisitingPool)
        {
            return _mapper.Map<Pool>(pooldata, options => options.AfterMap((src, dest) =>
            {
                string[] langs = language != null ? new string[]{ language } : ConfigService.KNOWN_LANGS;
                var srModel = ((PoolData)src);
                var desModel = ((Pool)dest);
                desModel.Description = exisitingPool == null || exisitingPool.Description == null ? new Dictionary<string, string>() : exisitingPool.Description;
                desModel.Name = exisitingPool == null || exisitingPool.Name == null ? new Dictionary<string, string>() : exisitingPool.Name;
                foreach (var lang in langs)
                {
                    desModel.Description[lang] = srModel.Description ?? string.Empty;
                    desModel.Name[lang] = srModel.Name ?? string.Empty;
                }
            }));
        }

        public async Task<PoolData> AddAsync(PoolData poolData, string language)
        {
            var pool = mapDataToPool(poolData);
            pool = await _poolRepository.AddAsync(pool);

            return mapPoolToData(pool, language);
        }

        public async Task<List<PoolData>> FindAll(string language)
        {
            var pools = await _poolRepository.FindAll();

            foreach (var pool in pools)
            {
                pool.Size = await _gudelIdRepository.Count(x => x.PoolId == pool.Id);
            }

            return pools.Select(_ => mapPoolToData(_,language)).ToList();
        }

        public async Task<PoolData> FindById(int id, string language)
        {
            var pool = await _poolRepository.FindById(id);
            if (pool == null) return null;
            var poolData = mapPoolToData(pool, language);
            poolData.Size = await _gudelIdRepository.Count(x => x.PoolId == pool.Id);
            return poolData;
        }

        public async Task<bool> Remove(int poolId)
        {
            await _poolRepository.Remove(await _poolRepository.FindById(poolId));
            return true;
        }

        public async Task<PoolData> Update(PoolData poolData, string language)
        {
            var existing = await _poolRepository.FindById(poolData.Id ?? -1);
            var pool = mapDataToPool(poolData, language, existing);
            pool = await _poolRepository.UpdateAsync(pool);
            return mapPoolToData(pool, language);
        }
    }
}
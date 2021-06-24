using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Services
{
    public class GudelIdService : IGudelIdService
    {
        private readonly IGudelIdRepository _gudelIdRepository;
        private readonly IActivityService _activityService;
        private readonly IConfigService _configService;
        private readonly IUtilsService _utilsService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public GudelIdService(IGudelIdRepository gudelIdRepository, IActivityService activityService, IConfigService configService, IMapper mapper, IUtilsService utilsService, AppDbContext context)
        {
            _gudelIdRepository = gudelIdRepository;
            _activityService = activityService;
            _configService = configService;
            _utilsService = utilsService;
            _mapper = mapper;
            _context = context;

        }

        private GudelIdData mapIdToData(GudelId id, string language)
        {
            return _mapper.Map<GudelIdData>(id, options => options.AfterMap((src, dest) =>
            {
                var srModel = ((GudelId)src);
                var desModel = ((GudelIdData)dest);
                desModel.Description = srModel.Description != null && srModel.Description.ContainsKey(language) ? srModel.Description[language] : string.Empty;
                desModel.Name = srModel.Name != null && srModel.Name.ContainsKey(language) ? srModel.Name[language] : string.Empty;
            }));
        }

        public async Task<GudelIdData> Find(string id, string language)
        {
            var result = await _gudelIdRepository.Find((x) => x.Id == id);
            return result != null ? mapIdToData(result,language) : null;
        }

        public async Task<List<GudelIdData>> FindAll(Expression<Func<GudelId, bool>> query, int pageSize = 1000, int page = 0, string language = "")
        {
            var items = await _gudelIdRepository.FindAll(pageSize, page, query);

            var result = items.Select(_ => mapIdToData(_, language)).ToList();

            return result;
        }

        public async Task<int> FindAllCount(Expression<Func<GudelId, bool>> query)
        {
            return await _gudelIdRepository.FindAllCount(query);
        }

        /**
         * Takes pre-created gudelIds or creates new gudelIds and adds them with the given type to the given pool
         */
        public async Task<List<GudelIdData>> CreateGudelIds(GudelIdRequest request, string userId, string language = ConfigService.LANG_DEFAULT)
        {
            Expression<Func<GudelId, bool>> query = x => x.StateId == GudelIdStates.CreatedId && x.PoolId == null;

            var gudelIds = await _gudelIdRepository.FindAll(request.Amount, 0, query);

            if (gudelIds.Count < request.Amount)
            {
                var newGudelIds = await GenerateGudelIds(request.Amount - gudelIds.Count, userId);
                gudelIds.AddRange(newGudelIds);
            }

            List<GudelId> updatedGudelIds = new List<GudelId>();
            if(request.poolId != null)
            {
               foreach(var gudelId in gudelIds)
                {
                    gudelId.TypeId = request.TypeId;
                    gudelId.PoolId = request.poolId;
                    updatedGudelIds.Add(await _gudelIdRepository.Update(gudelId));
                }
            }


            var result = updatedGudelIds.Select(_ => mapIdToData(_, language)).ToList();
            
            return result;
        }

        public async Task<GudelId> CreateGudelId(string gudelId, int? poolId, int? typeId, string userId)
        {
            var gid = new GudelId { Id = gudelId, StateId = GudelIdStates.CreatedId };

            if (poolId.HasValue)
            {
                gid.PoolId = poolId;
            }

            if (typeId.HasValue)
            {
                gid.TypeId = typeId.Value;
            }

            if (userId != null)
            {
                gid.CreatedBy = userId;
            }

            await _gudelIdRepository.Add(gid);

            return gid;

        }


        public async Task<GudelId> ReserveGudelId(string id, string userId)
        {
            var updatedGudelId = await _gudelIdRepository.Find(x => x.Id == id);

            updatedGudelId.StateId = GudelIdStates.ReservedId;
            updatedGudelId.ReservationDate = DateTime.Now;
            updatedGudelId.ReservedBy = userId;

            return await _gudelIdRepository.Update(updatedGudelId);
        }

        public async Task<List<GudelId>> ReserveGudelIds(List<GudelId> gudelIds, string userId, int? poolId, int? typeId = GudelIdTypes.SmartproductId)
        {
            if (gudelIds == null || !gudelIds.Any())
            {
                return new List<GudelId>(0);
            }

            List<GudelId> updatedGudelIds = new List<GudelId>();

            foreach (var item in gudelIds)
            {
                var gudelId = await _gudelIdRepository.Find(x => x.Id == item.Id);
                gudelId.StateId = 10;
                gudelId.TypeId = typeId.Value;
                gudelId.ReservationDate = DateTime.Now;
                gudelId.ReservedBy = userId;

                if (poolId.HasValue)
                {
                    gudelId.PoolId = poolId;
                }
                var updatedId = await _gudelIdRepository.Update(gudelId);
                var uid = Guid.NewGuid();
                var stateChangeAct = new Activity()
                {
                    CreatedBy = userId,
                    Key = "stateId",
                    OldValue = "0",
                    NewValue = "10",
                    Uid = uid,
                };
                var reserveAct = new Activity()
                {
                    CreatedBy = userId,
                    Key = "reservationDate",
                    NewValue = JsonConvert.SerializeObject(updatedId.ReservationDate),
                    Uid = uid,
                };
                await _activityService.CreateActivity(stateChangeAct, updatedId);
                await _activityService.CreateActivity(reserveAct, updatedId);
                updatedGudelIds.Add(updatedId);
            }

            return updatedGudelIds;
        }

        public async Task<List<GudelId>> GenerateGudelIds(int amount, string userId)
        {
            var list = new List<GudelId>();

            for (var i = 0; i < amount; i++)
            {
                list.Add(await GenerateGudelId(userId));
            }

            return list;
        }

        public async Task<GudelId> GenerateGudelId(string userId)
        {
            try
            {
                var id = _utilsService.GenerateRandomString(12, false);
                var item = new GudelId()
                {
                    Id = id,
                    CreatedBy = userId,
                    StateId = GudelIdStates.CreatedId
                };

                await _gudelIdRepository.Add(item);

                return item;
            }
            catch (DbUpdateException e)
            {
                return await GenerateGudelId(userId);
            }
        }

        public async Task<int> GetCountForPool(int poolId)
        {
            return await _gudelIdRepository.Count(x => x.PoolId == poolId);
        }

        public async Task<GudelId> UpdatePoolId(string id, int? poolId)
        {
            var gudelId = await _gudelIdRepository.Find(x => x.Id == id);
            gudelId.PoolId = poolId;
            return await _gudelIdRepository.Update(gudelId);
        }

        public async Task<GudelId> ChangeState(GudelIdData olGudelId, int stateId, string userId)
        {
            var gudelId = await _gudelIdRepository.Find(x => x.Id == olGudelId.Id);

            switch (stateId)
            {
                case GudelIdStates.CreatedId:
                    gudelId.CreationDate = DateTime.Now;
                    gudelId.CreatedBy = userId;
                    break;
                case GudelIdStates.AssignedId:
                    gudelId.AssignmentDate = DateTime.Now;
                    gudelId.AssignedBy = userId;
                    break;
                case GudelIdStates.ReservedId:
                    gudelId.ReservationDate = DateTime.Now;
                    gudelId.ReservedBy = userId;
                    break;
                case GudelIdStates.ProducedId:
                    gudelId.ProductionDate = DateTime.Now;
                    gudelId.ProducedBy = userId;
                    break;
                case GudelIdStates.VoidedId:
                    gudelId.VoidDate = DateTime.Now;
                    gudelId.VoidedBy = userId;
                    break;
            }
            gudelId.StateId = stateId;

            var dbItem = _context.GudelId.AsNoTracking().Where(_ => _.Id == gudelId.Id).FirstOrDefault();
  
            var updatedItem = await _gudelIdRepository.Update(gudelId);

            await _activityService.CreateActivityFromIds(dbItem, updatedItem, userId);

            return updatedItem;
        }
    }
}

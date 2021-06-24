using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Extensions;
using GudelIdService.Implementation.Persistence.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly AppDbContext _context;

        public ActivityService(IActivityRepository activityRepository, AppDbContext context)
        {
            _activityRepository = activityRepository;
            _context = context;
        }

        public async Task<int> CreateActivityFromIds(GudelId oldGudelId, GudelId newGudelId, string userName)
        {
            var uId = Guid.NewGuid();
            var differences = oldGudelId.GetDifferences(newGudelId);

            

            // check for differences between the gudelIds
            var activities = differences.Select(dif =>
             new Activity
                {
                    Uid = uId,
                    Key = dif.Prop,
                    OldValue = dif.OldVal,
                    NewValue = dif.NewVal,
                    CreatedBy = userName,
                    IsExtraField = false,
                    GudelId = newGudelId.Id
                });
            foreach(var act in activities)
            {
                await _activityRepository.AddAsync(act);
                
            }
            

            // specifically check if extrafields have been removed/added or changed
            List<ExtraField> oldExtraFields = oldGudelId.ExtraFields != null ? oldGudelId.ExtraFields.Where(f => f.ExtraFieldDefinition != null && !String.IsNullOrEmpty(f.ExtraFieldDefinition.Key)).ToList() : new List<ExtraField>();
            List<ExtraField> newExtraFields = newGudelId.ExtraFields != null ? newGudelId.ExtraFields.Where(f => f.ExtraFieldDefinition != null && !String.IsNullOrEmpty(f.ExtraFieldDefinition.Key)).ToList(): new List<ExtraField>();
            HashSet<string> oldKeys = oldExtraFields.Select(f => f.ExtraFieldDefinition.Key).ToHashSet();
            HashSet<string> newKeys = newExtraFields.Select(f => f.ExtraFieldDefinition.Key).ToHashSet();
            HashSet<string> allKeys = oldKeys.Union(newKeys).ToHashSet();

            foreach (string key in allKeys)
            {
                var activity = new Activity
                {
                    Uid = uId,
                    Key = key,
                    OldValue = string.Empty,
                    NewValue = string.Empty,
                    CreatedBy = userName,
                    IsExtraField = true,
                    GudelId = newGudelId.Id
                };

                if (oldKeys.Contains(key) && newKeys.Contains(key))
                {
                    activity.OldValue = JsonConvert.SerializeObject(oldExtraFields.Where(f => f.ExtraFieldDefinition.Key == key).Select(f => new { f.ExtraFieldDefinition.Name, f.Value }).First());
                    activity.NewValue = JsonConvert.SerializeObject(newExtraFields.Where(f => f.ExtraFieldDefinition.Key == key).Select(f => new { f.ExtraFieldDefinition.Name, f.Value }).First());
                }
                else if (oldKeys.Contains(key))
                {
                    activity.OldValue = JsonConvert.SerializeObject(oldExtraFields.Where(f => f.ExtraFieldDefinition.Key == key).Select(f => new { f.ExtraFieldDefinition.Name, f.Value }).First());
                }
                else if (newKeys.Contains(key))
                {
                    activity.NewValue = JsonConvert.SerializeObject(newExtraFields.Where(f => f.ExtraFieldDefinition.Key == key).Select(f => new { f.ExtraFieldDefinition.Name, f.Value }).First());
                }

                await _activityRepository.AddAsync(activity);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<Activity> CreateActivity(Activity activity, GudelId gudelId)
        {
            activity.GudelId = gudelId.Id;
            var result = await _activityRepository.AddAsync(activity);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<Activity>> FindAll(Expression<Func<Activity, bool>> query)
        {
            var result = await _activityRepository.FindAll(query);
            return result
                .OrderByDescending(_ => _.CreationDate)
                .ToList();
        }

        public object MapExtraFieldToView(Activity model, string language)
        {
            if (model.IsExtraField)
            {
                try
                {
                    string value;
                    var parsedOld = JsonConvert.DeserializeObject(model.OldValue);
                    ((Dictionary<string, string>)parsedOld).TryGetValue(language, out value);
                    model.OldValue = string.IsNullOrEmpty(value) ? model.OldValue : value;

                }
                catch (Exception)
                {
                    model.OldValue = model.OldValue;
                }
                try
                {
                    string value;
                    var parsedNew = JsonConvert.DeserializeObject(model.NewValue);
                    ((Dictionary<string, string>)parsedNew).TryGetValue(language, out value);
                    model.NewValue = string.IsNullOrEmpty(value) ? model.NewValue : value;

                }
                catch (Exception)
                {
                    model.NewValue = model.NewValue;
                }
            }
            return model;
        }
    }
}
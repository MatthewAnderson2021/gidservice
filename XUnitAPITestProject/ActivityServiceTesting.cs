using System;
using System.Linq;
using GudelIdService.Implementation.Persistence.Context;
using GudelIdService.Implementation.Services;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace XUnitAPITestProject
{
    public class ActivityServiceTesting
    {

        private ActivityService _activityService;
        private AppDbContext context;
        private IGudelIdRepository _gudelIdRepository;

        public ActivityServiceTesting()
        {
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var _activityRepository = scope.ServiceProvider.GetService<IActivityRepository>();
            _gudelIdRepository = scope.ServiceProvider.GetService<IGudelIdRepository>();
            context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _activityService = new ActivityService(_activityRepository, context);
        }

        /// <summary>
        /// Test findAll returns all the user activities 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllUserActivities()
        {
            //arrange
            var userId = "f32a23f5-95a3-4234-9dce-663a58f759cd";

            //act
            var activities = await _activityService.FindAll((x) => x.CreatedBy == userId);

            //assert
            Assert.IsType<List<Activity>>(activities);
            Assert.NotEmpty(activities);
        }

        /// <summary>
        /// Test create new activity
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateActivity()
        {
            //arrange
            var newActivity = new Activity()
            {
                CreatedBy = "f32a23f5-95a3-4234-9dce-663a58f759cd",
                Key = "stateId",
                OldValue = "0",
                NewValue = "10",
                Uid = Guid.NewGuid(),
            };

            var gudelId = new GudelId()
            {
                Id = "012CNP19Z8EE"
            };

            //act
            var createdActivitiy = await _activityService.CreateActivity(newActivity, gudelId);
            newActivity.GudelId = gudelId.Id;

            //assert
            Assert.IsType<Activity>(createdActivitiy);
            Assert.Equal(newActivity, createdActivitiy);
        }


        /// <summary>
        /// Test Map Extra Field to View  Method
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task mapExtraFieldToView()
        {
            //arrange
            var userId = "f32a23f5-95a3-4234-9dce-663a58f759cd";

            //act
            var activities = await _activityService.FindAll((x) => x.CreatedBy == userId);
            var result = new List<object>();

            activities.ForEach(activity =>
            {
                result.Add(_activityService.MapExtraFieldToView(activity, ConfigService.LANG_DEFAULT));
            });

            //assert
            Assert.NotEmpty(result);
            Assert.IsType<List<object>>(result);
        }

        /// <summary>
        /// Test CreateActivityFromIds
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateActivityFromIdsTest()
        {
            // Arrange
            string userId = "system";
            string gudelId = "012CNP19Z8EE";

            var gudelIdObj = await _gudelIdRepository.Find(x => x.Id == gudelId);
            var dbItem = context.GudelId.AsNoTracking().Where(_ => _.Id == gudelId).FirstOrDefault();

            gudelIdObj.StateId = 10;
            gudelIdObj.ProductionDate = DateTime.Now;
            gudelIdObj.ProducedBy = userId;

            var updatedItem = await _gudelIdRepository.Update(gudelIdObj);

            // Act
            int result = await _activityService.CreateActivityFromIds(dbItem, updatedItem, userId);

            // Assert
            Assert.True(result > 0);
        }
    }
}

using Xunit;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Core.Abstractions.Repositories;
using WebApi.Controllers;
using System;
using Core.Domain;
using WebApi.Models;
using Microsoft.Extensions.Logging;

namespace UnitTests
{
    public class GetActivityAsyncTest
    {

        private readonly Mock<IRepository<Activity>> _activityRepositoryMock;
        private readonly ActivityController _activityController;
        private readonly ILogger<ActivityController> _logger;

        public GetActivityAsyncTest()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _activityRepositoryMock = fixture.Freeze<Mock<IRepository<Activity>>>();
            _activityController = fixture.Build<ActivityController>().OmitAutoProperties().Create();
        }

        public Activity CreateBaseActivity()
        {
            var activity = new Activity()
            {
                Id = Guid.Parse("d75ca686-2e5f-4dfd-b4ad-3a5528b7fefe"),
                Name = "Serfing",
                IconName = "Serf",
                Spots = new List<Spot>()
                {
                    new Spot()
                    {
                        Id = Guid.Parse("4BCC4F3B-DB71-4442-A65B-556E2513700D"),
                        //PlaceId = Guid.Parse("2bca4fc2-359b-4150-9ebe-c19cfcc8a9b6"),
                        Note = "Sun place",
                        CreateDataTime = new DateTime(2022, 12, 20),
                        Latitude = 43.55896934593335,
                        Longitude = 39.70559056533847,
                        IsActive = true,
                        SpotPhotos = new List<SpotPhoto>()
                        {
                            new SpotPhoto()
                            {
                                Id = Guid.Parse("30BE0198-7A29-4EE6-8BAB-B702F7A357E2"),
                                FileName = "Lisa_Andersen.jpg",
                                Comment = "девушка с доской",
                                CreateDataTime = new DateTime(2023, 02, 14)
                            },
                            new SpotPhoto()
                            {
                                Id = Guid.Parse("78791d84-8822-40a6-ae71-025db1ce12e9"),
                                FileName = "sea-ocean-people-man.jpg",
                                Comment = "мужчина на доске",
                                CreateDataTime = new DateTime(2023, 02, 08)
                            },

                        }
                    }
                }
            };

            return activity;
        }


        [Fact]
        public async void GetActivityAsyncTest_ActivityIsNotFound_ReturnCompareId()
        {
            // Arrange
            var activityId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");

            var mock = new Mock<IRepository<Activity>>();
            mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid x) => new Activity { Id = x });
            var controller = new ActivityController(mock.Object, _logger);

            // Act
            var result = await controller.GetActivityAsync(activityId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ActivityResponse>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result).Value;
            var data = Assert.IsType<ActivityResponse>(returnValue);
            Assert.Equal(activityId, data.Id);
        }

        [Fact]
        public async void GetActivityAsyncTest_ActivityIsNotFound_ReturnNotFoundResult()
        {
            // Arrange
            var activityId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");
            Activity activity = null;

            _activityRepositoryMock.Setup(repo => repo.GetByIdAsync(activityId))
                .ReturnsAsync(activity);

            // Act
            var result = await _activityController.GetActivityAsync(activityId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GetActivityAsyncTest_ActivityIsFound_ReturnTypeOkObjectResult()
        {
            // Arrange
            var activityId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");

            var mock = new Mock<IRepository<Activity>>();
            mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid x) => new Activity { Id = x });
            var controller = new ActivityController(mock.Object, _logger);

            // Act
            var result = await controller.GetActivityAsync(activityId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetActivityAsyncTest_ActivityIsFullObject_ReturnCompareObjectName()
        {
            // Arrange
            var activityId = Guid.Parse("d75ca686-2e5f-4dfd-b4ad-3a5528b7fefe");
            Activity activity = CreateBaseActivity();

            _activityRepositoryMock.Setup(repo => repo.GetByIdAsync(activityId))
                .ReturnsAsync(activity);

            // Act
            var result = await _activityController.GetActivityAsync(activityId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ActivityResponse>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result).Value;
            var data = Assert.IsType<ActivityResponse>(returnValue);
            Assert.Equal("Serfing", data.Name);
        }
    }
}

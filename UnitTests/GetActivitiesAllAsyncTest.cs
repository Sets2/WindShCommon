using Xunit;
using System.Collections.Generic;
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
    public class GetActivityAllAsyncTest
    {

        private readonly ILogger<ActivityController> _logger;

        [Fact]
        public async void GetActivityAllAsyncTest_ActivityIsFound_ReturnsOkObjResult()
        {
            // Arrange
            var mock = new Mock<IRepository<Activity>>();
            mock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Activity> { new Activity(), new Activity() });
            var controller = new ActivityController(mock.Object, _logger);

            // Act
            var result = await controller.GetActivitiesAllAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetActivityAllAsyncTest_ActivityIsFound_CompareCount()
        {
            // Arrange
            var mock = new Mock<IRepository<Activity>>();
            mock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Activity> { new Activity(), new Activity() });
            var controller = new ActivityController(mock.Object, _logger);

            // Act
            var result = await controller.GetActivitiesAllAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<ActivityResponse>>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result).Value;
            var count = Assert.IsType<List<ActivityResponse>>(returnValue).Count;
            Assert.Equal(2, count);
        }
    }
}


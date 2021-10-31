using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Controllers;
using Payment.Api.Dtos;
using Payment.Api.Models;
using Payment.Api.Repository;
using Payment.Api.Services;
using Xunit;

namespace Payment.UnitTests
{
    public class DataControllerTests
    {
        [Fact]
        public void Users_Returns_List_Of_Users()
        {
            DataController controller = new DataController();
            var result = (controller.Users().Result as OkObjectResult).Value as IEnumerable<User>;

            result.Should().NotBeEmpty().And
            .NotBeNull()
            .And.OnlyHaveUniqueItems();
            result.Should().HaveCountGreaterThan(3);
        }
    }
}

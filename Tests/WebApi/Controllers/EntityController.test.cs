using AutoMapper;
using BusinesRules.Entities;
using BusinesRules.Exceptions;
using Contracts.Entities;
using Entities.DTO;
using Entities.Helpers.Entities;
using Entities.Models;
using Entities.Utils;
using Entities.Utils.Paged;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Wrappers.Interfaces;
using WebApi.Controllers;

namespace Tests.WebApi.Controllers;

public class EntityControllerTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Get_ShouldReturnExpectedResult_WhenDataExistsOrNot(bool hasData)
    {
        var data = hasData
            ? new List<EntityDTO> { new() { Id = Guid.NewGuid(), Name = "Name", Description = "Desc" } }
            : new List<EntityDTO>();

        var (controller, _, entityRepoMock, brMapperMock, _) = BuildController();
        entityRepoMock.Setup(repository => repository.GetAllAsync(1, 10, "Name", false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Entity>());
        brMapperMock.Setup(mapper => mapper.Map<IEnumerable<EntityDTO>>(It.IsAny<IEnumerable<Entity>>()))
            .Returns(data);

        var result = await controller.Get(1, 10, "Name", false);

        if (hasData)
        {
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(data);
            return;
        }

        result.Should().BeOfType<NoContentResult>();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(false, true)]
    public async Task GetPaged_ShouldReturnExpectedResult(bool hasRows, bool nullPagedResult)
    {
        var (controller, _, entityRepoMock, brMapperMock, _) = BuildController();
        var pagedEntity = new PagedResult<Entity>
        {
            CurrentPage = 1,
            PageCount = 1,
            PageSize = 10,
            RowCount = hasRows ? 1 : 0,
            Results = hasRows
                ? new List<Entity> { new() { Id = Guid.NewGuid(), Name = "Name", Description = "Desc", RegisterDate = DateTime.UtcNow } }
                : new List<Entity>()
        };
        PagedResult<EntityDTO>? mapped = nullPagedResult
            ? null
            : new PagedResult<EntityDTO>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                RowCount = hasRows ? 1 : 0,
                Results = hasRows
                    ? new List<EntityDTO> { new() { Id = Guid.NewGuid(), Name = "Name", Description = "Desc" } }
                    : new List<EntityDTO>()
            };

        entityRepoMock.Setup(repository => repository.GetAllPagedAsync(1, 10, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedEntity);
        brMapperMock.Setup(mapper => mapper.Map<PagedResult<EntityDTO>>(pagedEntity)).Returns(mapped!);

        var result = await controller.GetPaged(1, 10);

        if (nullPagedResult || !hasRows)
        {
            result.Should().BeOfType<NoContentResult>();
            return;
        }

        result.Should().BeOfType<OkObjectResult>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetById_ShouldReturnOkOrNoContent(bool hasEntity)
    {
        var id = Guid.NewGuid();
        var entity = new Entity { Id = id, Name = "Entity", Description = "Desc", RegisterDate = DateTime.UtcNow };
        var dto = hasEntity
            ? new EntityDTO { Id = id, Name = "Entity", Description = "Desc" }
            : new EntityDTO();

        var (controller, _, entityRepoMock, brMapperMock, _) = BuildController();
        entityRepoMock.Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        brMapperMock.Setup(mapper => mapper.Map<EntityDTO>(entity)).Returns(dto);

        var result = await controller.Get(id);

        if (hasEntity)
        {
            result.Should().BeOfType<OkObjectResult>();
            return;
        }

        result.Should().BeOfType<NoContentResult>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Post_ShouldReturnBadRequestWhenModelIsInvalidOrCreated(bool validModel)
    {
        var register = validModel ? new EntityRegisterModel { Name = "Name", Description = "Desc" } : new EntityRegisterModel();
        var mappedEntity = new Entity { Id = Guid.NewGuid(), Name = "Name", Description = "Desc" };
        var (controller, repoMock, entityRepoMock, _, controllerMapperMock) = BuildController();

        if (!validModel)
        {
            controller.ModelState.AddModelError("Name", "Name is required");
        }

        controllerMapperMock.Setup(mapper => mapper.Map<Entity>(It.IsAny<EntityRegisterModel>())).Returns(mappedEntity);
        entityRepoMock.Setup(repository => repository.CreateEntityAsync(mappedEntity, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repoMock.Setup(repository => repository.SaveAsync()).Returns(Task.CompletedTask);

        var result = await controller.Post(register);

        if (!validModel)
        {
            result.Should().BeOfType<BadRequestObjectResult>();
            return;
        }

        result.Should().BeOfType<CreatedAtRouteResult>();
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenBodyIsNull()
    {
        var (controller, _, _, _, _) = BuildController();

        var result = await controller.Post(null!);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenIdIsEmpty()
    {
        var (controller, _, _, _, _) = BuildController();

        var result = await controller.Put(Guid.Empty, new Entity());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenEntityIsNull()
    {
        var (controller, _, _, _, _) = BuildController();

        var result = await controller.Put(Guid.NewGuid(), null!);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        var (controller, _, _, _, _) = BuildController();
        controller.ModelState.AddModelError("Name", "Name is required");

        var result = await controller.Put(Guid.NewGuid(), new Entity());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Put_ShouldReturnExpectedResult_WhenEntityExistsOrNot(bool entityExists)
    {
        var id = Guid.NewGuid();
        var existing = entityExists
            ? new Entity { Id = id, Name = "Old", Description = "Old", RegisterDate = DateTime.UtcNow }
            : new Entity();
        var update = new Entity { Name = "New", Description = "New" };
        var dto = new EntityDTO { Id = id, Name = "New", Description = "New" };

        var (controller, repoMock, entityRepoMock, brMapperMock, _) = BuildController();
        entityRepoMock.Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        if (entityExists)
        {
            brMapperMock.Setup(mapper => mapper.Map<EntityDTO>(It.IsAny<Entity>())).Returns(dto);
            repoMock.Setup(repository => repository.SaveAsync()).Returns(Task.CompletedTask);
        }

        Func<Task<IActionResult>> action = async () => await controller.Put(id, update);

        if (!entityExists)
        {
            await action.Should().ThrowAsync<NotFoundException>();
            return;
        }

        var result = await action();
        result.Should().BeOfType<OkObjectResult>();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", true)]
    [InlineData("11111111-1111-1111-1111-111111111111", false)]
    public async Task Delete_ShouldReturnBadRequestForEmptyId_OrNoContentForValid(string idText, bool expectBadRequest)
    {
        var id = Guid.Parse(idText);
        var (controller, repoMock, entityRepoMock, _, _) = BuildController();

        if (!expectBadRequest)
        {
            entityRepoMock.Setup(repository => repository.GetById(id))
                .Returns(new Entity { Id = id, Name = "Entity", Description = "Desc", RegisterDate = DateTime.UtcNow });
            repoMock.Setup(repository => repository.SaveAsync()).Returns(Task.CompletedTask);
        }

        var result = await controller.Delete(id);

        if (expectBadRequest)
        {
            result.Should().BeOfType<BadRequestObjectResult>();
            return;
        }

        result.Should().BeOfType<NoContentResult>();
    }

    private static (EntityController controller, Mock<IRepositoryWrapper> wrapperMock, Mock<IEntityRepository> entityRepositoryMock, Mock<IMapper> brMapperMock, Mock<IMapper> controllerMapperMock) BuildController()
    {
        var wrapperMock = new Mock<IRepositoryWrapper>();
        var entityRepositoryMock = new Mock<IEntityRepository>();
        var brMapperMock = new Mock<IMapper>();
        var controllerMapperMock = new Mock<IMapper>();

        wrapperMock.SetupGet(wrapper => wrapper.Entity).Returns(entityRepositoryMock.Object);
        wrapperMock.Setup(wrapper => wrapper.SaveAsync()).Returns(Task.CompletedTask);

        var entitiesBR = new EntitiesBR(wrapperMock.Object, brMapperMock.Object);
        var controller = new EntityController(entitiesBR, controllerMapperMock.Object);

        return (controller, wrapperMock, entityRepositoryMock, brMapperMock, controllerMapperMock);
    }
}

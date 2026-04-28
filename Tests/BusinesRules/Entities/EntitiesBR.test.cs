using BusinesRules.Entities;
using BusinesRules.Exceptions;
using Contracts.Entities;
using Entities.DTO;
using Entities.Models;
using Entities.Utils.Paged;
using Entities.Utils.Paged.Interfaces;
using FluentAssertions;
using Moq;
using Repository.Wrappers.Interfaces;

namespace Tests.BusinesRules.Entities;

public class EntitiesBRTests
{
    [Fact]
    public async Task GetAllEntities_ShouldReturnMappedDtos()
    {
        var entities = new List<Entity>
        {
            new() { Id = Guid.NewGuid(), Name = "A", Description = "D1", RegisterDate = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "B", Description = "D2", RegisterDate = DateTime.UtcNow }
        };
        var mapped = new List<EntityDTO>
        {
            new() { Id = entities[0].Id, Name = "A", Description = "D1" },
            new() { Id = entities[1].Id, Name = "B", Description = "D2" }
        };

        var (sut, repoMock, entityRepoMock, mapperMock) = BuildSut();
        entityRepoMock.Setup(repository => repository.GetAllAsync(1, 10, "Name", false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);
        mapperMock.Setup(mapper => mapper.Map<IEnumerable<EntityDTO>>(entities)).Returns(mapped);

        var result = await sut.GetAllEntities(1, 10, "Name", false);

        result.Should().BeEquivalentTo(mapped);
        entityRepoMock.Verify(repository => repository.GetAllAsync(1, 10, "Name", false, It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<IEnumerable<EntityDTO>>(entities), Times.Once);
    }

    [Fact]
    public async Task GetAllEntitiesPaged_ShouldReturnMappedPagedDtos()
    {
        var paged = new PagedResult<Entity>
        {
            CurrentPage = 2,
            PageCount = 3,
            PageSize = 5,
            RowCount = 11,
            Results = new List<Entity> { new() { Id = Guid.NewGuid(), Name = "A", Description = "D", RegisterDate = DateTime.UtcNow } }
        };
        var mapped = new PagedResult<EntityDTO>
        {
            CurrentPage = 2,
            PageCount = 3,
            PageSize = 5,
            RowCount = 11,
            Results = new List<EntityDTO> { new() { Id = ((Entity)paged.Results.First()).Id, Name = "A", Description = "D" } }
        };

        var (sut, _, entityRepoMock, mapperMock) = BuildSut();
        entityRepoMock.Setup(repository => repository.GetAllPagedAsync(2, 5, "Name", true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);
        mapperMock.Setup(mapper => mapper.Map<PagedResult<EntityDTO>>(paged)).Returns(mapped);

        var result = await sut.GetAllEntitiesPaged(2, 5, "Name", true);

        result.Should().BeEquivalentTo(mapped);
    }

    [Fact]
    public async Task GetEntityById_ShouldReturnMappedDto()
    {
        var id = Guid.NewGuid();
        var entity = new Entity { Id = id, Name = "Entity", Description = "Desc", RegisterDate = DateTime.UtcNow };
        var dto = new EntityDTO { Id = id, Name = "Entity", Description = "Desc" };

        var (sut, _, entityRepoMock, mapperMock) = BuildSut();
        entityRepoMock.Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        mapperMock.Setup(mapper => mapper.Map<EntityDTO>(entity)).Returns(dto);

        var result = await sut.GetEntityById(id);

        result.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task CreateEntity_ShouldSetRegisterDateAndPersist()
    {
        var entity = new Entity { Id = Guid.NewGuid(), Name = "Entity", Description = "Desc" };
        var (sut, repoMock, entityRepoMock, _) = BuildSut();

        await sut.CreateEntity(entity);

        entity.RegisterDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        entityRepoMock.Verify(repository => repository.CreateEntityAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        repoMock.Verify(repository => repository.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateEntity_WhenEntityDoesNotExist_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        var updated = new Entity { Name = "Updated", Description = "Updated desc" };
        var (sut, repoMock, entityRepoMock, mapperMock) = BuildSut();
        entityRepoMock.Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(new Entity());

        var action = async () => await sut.UpdateEntity(id, updated);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage("Entity not found");
        repoMock.Verify(repository => repository.SaveAsync(), Times.Never);
        mapperMock.Verify(mapper => mapper.Map<EntityDTO>(It.IsAny<Entity>()), Times.Never);
    }

    [Fact]
    public async Task UpdateEntity_WhenEntityExists_ShouldUpdateAndReturnMappedDto()
    {
        var id = Guid.NewGuid();
        var dbEntity = new Entity { Id = id, Name = "Old", Description = "Old desc", RegisterDate = DateTime.UtcNow };
        var updated = new Entity { Name = "New", Description = "New desc" };
        var mapped = new EntityDTO { Id = id, Name = "New", Description = "New desc" };

        var (sut, repoMock, entityRepoMock, mapperMock) = BuildSut();
        entityRepoMock.Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(dbEntity);
        mapperMock.Setup(mapper => mapper.Map<EntityDTO>(updated)).Returns(mapped);

        var result = await sut.UpdateEntity(id, updated);

        result.Should().BeEquivalentTo(mapped);
        updated.Id.Should().Be(id);
        entityRepoMock.Verify(repository => repository.UpdateEntity(dbEntity, updated), Times.Once);
        repoMock.Verify(repository => repository.SaveAsync(), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteEntity_ShouldHandleFoundAndNotFound(bool exists)
    {
        var id = Guid.NewGuid();
        var (sut, repoMock, entityRepoMock, _) = BuildSut();
        entityRepoMock.Setup(repository => repository.GetById(id))
            .Returns(exists
                ? new Entity { Id = id, Name = "Entity", Description = "Desc", RegisterDate = DateTime.UtcNow }
                : new Entity());

        if (!exists)
        {
            var action = async () => await sut.DeleteEntity(id);
            await action.Should().ThrowAsync<NotFoundException>().WithMessage("Entity not found");
            repoMock.Verify(repository => repository.SaveAsync(), Times.Never);
            return;
        }

        await sut.DeleteEntity(id);

        entityRepoMock.Verify(repository => repository.DeleteEntity(It.Is<Entity>(entity => entity.Id == id)), Times.Once);
        repoMock.Verify(repository => repository.SaveAsync(), Times.Once);
    }

    private static (EntitiesBR sut, Mock<IRepositoryWrapper> wrapperMock, Mock<IEntityRepository> entityRepositoryMock, Mock<AutoMapper.IMapper> mapperMock) BuildSut()
    {
        var wrapperMock = new Mock<IRepositoryWrapper>();
        var entityRepositoryMock = new Mock<IEntityRepository>();
        var mapperMock = new Mock<AutoMapper.IMapper>();

        wrapperMock.SetupGet(wrapper => wrapper.Entity).Returns(entityRepositoryMock.Object);

        return (new EntitiesBR(wrapperMock.Object, mapperMock.Object), wrapperMock, entityRepositoryMock, mapperMock);
    }
}

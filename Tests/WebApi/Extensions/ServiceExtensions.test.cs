using BusinesRules.Entities;
using Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Wrappers.Interfaces;
using WebApi.Extensions;

namespace Tests.WebApi.Extensions;

public class ServiceExtensionsTests
{
    [Fact]
    public void ConfigureDbContext_ShouldRegisterRepositoryContext()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.ConfigureDbContext(configuration);

        var provider = services.BuildServiceProvider();
        var context = provider.GetService<RepositoryContext>();
        context.Should().NotBeNull();
    }

    [Fact]
    public void ConfigureRepositoriesWrappers_ShouldRegisterWrapperScoped()
    {
        var services = new ServiceCollection();

        services.ConfigureRepositoriesWrappers();

        services.Should().Contain(descriptor => descriptor.ServiceType == typeof(IRepositoryWrapper));
    }

    [Fact]
    public void ConfigureBusinessRules_ShouldRegisterEntitiesBr()
    {
        var services = new ServiceCollection();

        services.ConfigureBusinessRules();

        services.Should().Contain(descriptor => descriptor.ServiceType == typeof(EntitiesBR));
    }

    [Fact]
    public void ConfigureCors_ShouldRegisterPolicyServices()
    {
        var services = new ServiceCollection();

        services.ConfigureCors();

        services.Should().Contain(descriptor => descriptor.ServiceType.Name.Contains("Cors", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void AddMapper_ShouldRegisterMapperServices()
    {
        var services = new ServiceCollection();

        services.AddMapper();

        services.Should().Contain(descriptor => descriptor.ServiceType.Name.Contains("Mapper", StringComparison.OrdinalIgnoreCase));
    }
}

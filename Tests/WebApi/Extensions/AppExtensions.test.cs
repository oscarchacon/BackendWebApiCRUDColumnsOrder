using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Extensions;

namespace Tests.WebApi.Extensions;

public class AppExtensionsTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UseMiddlewares_ShouldNotThrow(bool withLogger)
    {
        var services = new ServiceCollection();
        if (withLogger)
        {
            services.AddLogging();
        }

        var app = new ApplicationBuilder(services.BuildServiceProvider());

        var action = () => app.UseMiddlewares();

        action.Should().NotThrow();
    }

    [Fact]
    public void UseSwaggerDocumentation_ShouldNotThrow()
    {
        var services = new ServiceCollection();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        var app = new ApplicationBuilder(services.BuildServiceProvider());

        var action = () => app.UseSwaggerDocumentation();

        action.Should().NotThrow();
    }
}

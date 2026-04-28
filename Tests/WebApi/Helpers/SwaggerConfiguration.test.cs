using FluentAssertions;
using Microsoft.OpenApi.Models;
using WebApi.Helpers;

namespace Tests.WebApi.Helpers;

public class SwaggerConfigurationTests
{
    [Fact]
    public void Constants_ShouldExposeExpectedSecurityConfiguration()
    {
        SwaggerConfiguration.SecurityTypeName.Should().Be("Bearer");
        SwaggerConfiguration.SecurityApiKeyName.Should().Be("Authorization");
        SwaggerConfiguration.SecurityApiKeyIn.Should().Be(ParameterLocation.Header);
        SwaggerConfiguration.SecurityApiKeyType.Should().Be(SecuritySchemeType.ApiKey);
    }

    [Theory]
    [InlineData("/swagger/v1/swagger.json")]
    [InlineData("v1")]
    public void Constants_ShouldHaveConfiguredSwaggerValues(string expected)
    {
        var values = new[] { SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.DocNameV1 };

        values.Should().Contain(expected);
    }
}

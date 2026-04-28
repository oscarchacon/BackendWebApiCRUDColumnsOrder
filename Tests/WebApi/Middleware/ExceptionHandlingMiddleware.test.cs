using BusinesRules.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using WebApi.Middleware;

namespace Tests.WebApi.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    public static IEnumerable<object[]> ExceptionCases()
    {
        yield return new object[] { new NotFoundException("not found"), StatusCodes.Status404NotFound };
        yield return new object[] { new BadRequestException("bad request"), StatusCodes.Status400BadRequest };
        yield return new object[] { new NotAllowedException("not allowed"), StatusCodes.Status405MethodNotAllowed };
        yield return new object[] { new Exception("unexpected"), StatusCodes.Status500InternalServerError };
    }

    [Theory]
    [MemberData(nameof(ExceptionCases))]
    public async Task InvokeAsync_WhenExceptionThrown_ShouldMapStatusCode(Exception exception, int expectedStatusCode)
    {
        RequestDelegate next = _ => throw exception;
        var logger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var middleware = new ExceptionHandlingMiddleware(next, logger.Object);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(expectedStatusCode);
        context.Response.ContentType.Should().Be("application/json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var payload = await reader.ReadToEndAsync();
        var json = JObject.Parse(payload);
        json["code"]!.Value<int>().Should().Be(expectedStatusCode);
        json["message"]!.Value<string>().Should().Be(exception.Message);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoException_ShouldCallNext()
    {
        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new ExceptionHandlingMiddleware(next, Mock.Of<ILogger<ExceptionHandlingMiddleware>>());

        await middleware.InvokeAsync(new DefaultHttpContext());

        wasCalled.Should().BeTrue();
    }
}

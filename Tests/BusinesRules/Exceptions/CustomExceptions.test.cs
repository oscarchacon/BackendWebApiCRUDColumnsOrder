using BusinesRules.Exceptions;
using FluentAssertions;

namespace Tests.BusinesRules.Exceptions;

public class CustomExceptionsTests
{
    public static IEnumerable<object[]> ExceptionTypes()
    {
        yield return new object[] { typeof(NotFoundException) };
        yield return new object[] { typeof(BadRequestException) };
        yield return new object[] { typeof(NotAllowedException) };
    }

    [Theory]
    [MemberData(nameof(ExceptionTypes))]
    public void Exception_DefaultConstructor_ShouldCreateException(Type exceptionType)
    {
        var exception = (Exception)Activator.CreateInstance(exceptionType)!;

        exception.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(ExceptionTypes))]
    public void Exception_MessageConstructor_ShouldSetMessage(Type exceptionType)
    {
        const string message = "custom-message";
        var exception = (Exception)Activator.CreateInstance(exceptionType, message)!;

        exception.Message.Should().Be(message);
    }

    [Theory]
    [MemberData(nameof(ExceptionTypes))]
    public void Exception_MessageAndInnerConstructor_ShouldSetProperties(Type exceptionType)
    {
        const string message = "outer-message";
        var inner = new InvalidOperationException("inner-message");
        var exception = (Exception)Activator.CreateInstance(exceptionType, message, inner)!;

        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeSameAs(inner);
    }
}

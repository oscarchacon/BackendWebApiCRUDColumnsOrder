using System;

namespace BusinesRules.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base() { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

public class BadRequestException : Exception
{
    public BadRequestException() : base() { }

    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}

public class NotAllowedException : Exception
{
    public NotAllowedException() : base() { }

    public NotAllowedException(string message) : base(message) { }

    public NotAllowedException(string message, Exception innerException) : base(message, innerException) { }
}
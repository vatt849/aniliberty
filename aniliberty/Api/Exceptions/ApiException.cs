using System;

namespace aniliberty.Api.Exceptions;

class ApiException(string message, int code) : Exception(message)
{
    public int Code { get; } = code;
}

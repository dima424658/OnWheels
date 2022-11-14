using System;
using System.Collections;

namespace OnWheels.Core;

public class ApiException : Exception
{
    public ApiException(string? message = null) : this(400, message) { }

    public ApiException(int statusCode, string? message = null)
        : base(message) => StatusCode = statusCode;

    public int StatusCode { get; set; }

    public Dictionary<string, object?> Details { get; set; } = new();

    public override IDictionary Data => Details;
}
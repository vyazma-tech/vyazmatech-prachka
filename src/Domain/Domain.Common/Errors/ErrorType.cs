using System.Text.Json.Serialization;

namespace Domain.Common.Errors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    Failure = 0,

    Validation = 1,

    NotFound = 2,

    BadRequest = 3,

    Conflict = 4,

    Unprocessable = 5
}
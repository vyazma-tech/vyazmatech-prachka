using System.Text.Json.Serialization;

namespace Domain.Common.Errors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorArea
{
    Domain = 0,

    Infrastructure = 1,

    Application = 2,
}
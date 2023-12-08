using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Application.Handlers.Order.Queries;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderQueryParameter
{
    [EnumMember(Value = "id")]
    OrderId = 1,
    [EnumMember(Value = "userId")]
    UserId = 2,
    [EnumMember(Value = "creationDate")]
    CreationDate = 3,
    [EnumMember(Value = "page")]
    Page = 4,
}
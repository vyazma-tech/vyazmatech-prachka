using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Application.Handlers.User.Queries;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserQueryParameter
{
    [EnumMember(Value = "id")]
    Id = 1,
    [EnumMember(Value = "telegramId")]
    TelegramId = 2,
    [EnumMember(Value = "fullname")]
    Fullname = 3,
    [EnumMember(Value = "RegistrationDate")]
    RegistrationDate = 4,
    [EnumMember(Value = "page")]
    Page = 5,
}
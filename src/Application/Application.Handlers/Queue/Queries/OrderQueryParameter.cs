using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Application.Handlers.Queue.Queries;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QueueQueryParameter
{
    [EnumMember(Value = "queueId")]
    QueueId = 1,
    [EnumMember(Value = "orderId")]
    OrderId = 2,
    [EnumMember(Value = "assignmentDate")]
    AssignmentDate = 3,
    [EnumMember(Value = "page")]
    Page = 4,
}
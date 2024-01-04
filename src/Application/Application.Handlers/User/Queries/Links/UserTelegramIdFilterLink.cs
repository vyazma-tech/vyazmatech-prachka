using System.Text.RegularExpressions;
using Application.Core.Querying.Abstractions;
using Domain.Core.User;

namespace Application.Handlers.User.Queries.Links;

// TODO: FIX IT
// public class UserTelegramIdFilterLink : FilterLinkBase<UserEntity, UserQueryParameter>
// {
//     protected override IEnumerable<UserEntity>? TryApply(
//         IEnumerable<UserEntity> requestData,
//         QueryParameter<UserQueryParameter> requestParameter)
//     {
//         var regex = new Regex(requestParameter.Pattern, RegexOptions.Compiled, TimeSpan.FromSeconds(5));
//         return requestParameter.Type is not UserQueryParameter.TelegramId
//             ? null
//             : requestData.Where(x => x.TelegramId.Value is not null && regex.IsMatch(x.TelegramId.Value));
//     }
// }
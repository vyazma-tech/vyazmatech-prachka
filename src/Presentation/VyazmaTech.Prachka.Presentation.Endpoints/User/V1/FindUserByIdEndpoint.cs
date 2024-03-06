using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Users.Queries;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.User.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.User.V1;

internal class FindUserByIdEndpoint : Endpoint<UserWithIdRequest, UserDto>
{
    private const string FeatureName = "UserById";
    private readonly ISender _sender;

    public FindUserByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("{id}");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<UserEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(UserWithIdRequest req, CancellationToken ct)
    {
        var query = new UserById.Query(req.UserId);

        Result<UserById.Response> response = await _sender.Send(query, ct);

        await response.Match(
            success => SendOkAsync(success.User, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}
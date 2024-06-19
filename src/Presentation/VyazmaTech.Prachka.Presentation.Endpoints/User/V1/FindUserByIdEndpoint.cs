using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Core.Users.Queries;
using VyazmaTech.Prachka.Application.Dto.Core.User;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
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
        Get("{userId}");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<UserEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(UserWithIdRequest req, CancellationToken ct)
    {
        var query = new UserById.Query(req.UserId);

        try
        {
            UserById.Response response = await _sender.Send(query, ct);
            await SendOkAsync(response.User, ct);
        }
        catch (DomainException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}
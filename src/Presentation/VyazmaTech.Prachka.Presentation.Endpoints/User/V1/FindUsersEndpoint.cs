using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Users.Queries;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.User.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.User.V1;

internal class FindUsersEndpoint : Endpoint<FindUsersRequest, PagedResponse<UserDto>>
{
    private const string FeatureName = "GetAllUsers";
    private readonly ISender _sender;

    public FindUsersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<UserEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(FindUsersRequest req, CancellationToken ct)
    {
        var query = new UserByQuery.Query(req.TelegramId, req.Fullname, req.RegistrationDate, req.Page);

        UserByQuery.Response response = await _sender.Send(query, ct);

        if (response.Users.Bunch.Any())
            await this.SendPartialContentAsync(response, ct);
        else
            await SendNoContentAsync(ct);
    }
}
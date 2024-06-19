using FastEndpoints;
using Mediator;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Contracts.Core.Users.Queries;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Core.User;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.User.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.User.V1;

internal class FindUsersEndpoint : Endpoint<FindUsersRequest, PagedResponse<UserDto>>
{
    private const string FeatureName = "GetAllUsers";
    private readonly ISender _sender;
    private readonly int _recordsPerPage;

    public FindUsersEndpoint(ISender sender, IOptions<PaginationConfiguration> paginationConfiguration)
    {
        _sender = sender;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
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
        var query = new UserByQuery.Query(
            TelegramUsername: req.TelegramId,
            Fullname: req.Fullname,
            RegistrationDate: req.RegistrationDate,
            Page: req.Page,
            Limit: req.Limit ?? _recordsPerPage);

        UserByQuery.Response response = await _sender.Send(query, ct);

        if (response.Users.Bunch.Any())
        {
            await this.SendPartialContentAsync(response, ct);
        }
        else
        {
            await SendNoContentAsync(ct);
        }
    }
}
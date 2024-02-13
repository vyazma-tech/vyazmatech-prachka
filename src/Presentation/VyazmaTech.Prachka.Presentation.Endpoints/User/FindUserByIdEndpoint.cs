using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Users.Queries;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.User.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.User;

internal class FindUserByIdEndpoint : Endpoint<UserWithIdRequest, UserDto>
{
    private readonly ISender _sender;

    public FindUserByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("api/users/{id}");
        AllowAnonymous();
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
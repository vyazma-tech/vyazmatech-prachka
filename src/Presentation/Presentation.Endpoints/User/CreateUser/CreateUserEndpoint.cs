using Application.Handlers.User.CreateUser;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.User.CreateUser;

public class CreateUserEndpoint : Endpoint<CreateUserCommand, CreateUserResponse>
{
    private readonly IMediator _mediator;

    public CreateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/user");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        CreateUserResponse response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Core.Users.Commands.CreateUser;

namespace VyazmaTech.Prachka.Application.Handlers.Core.User.Commands;

internal sealed class CreateUserCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IDateTimeProvider _timeProvider;

    public CreateUserCommandHandler(IPersistenceContext context, IDateTimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var fullname = Fullname.Create(request.Fullname);
        var telegramUsername = TelegramUsername.Create(request.TelegramUsername);

        var user = Domain.Core.Users.User.Create(
            request.Id,
            telegramUsername,
            fullname,
            _timeProvider.DateNow);

        _context.Users.Insert(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(user.ToDto());
    }
}
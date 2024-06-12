using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Contracts.Users.Commands;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models.Events;

namespace VyazmaTech.Prachka.Application.Handlers.User.Events;

internal sealed class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    private readonly IPersistenceContext _context;
    private readonly ISender _sender;

    public UserRegisteredIntegrationEventHandler(IPersistenceContext context, ISender sender)
    {
        _context = context;
        _sender = sender;
    }

    public async ValueTask Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
    {
        // Если юзер существует, то не создаем его
        // Если юзер не существует, то бросим ошибку
        try
        {
            Domain.Core.Users.User existingUser = await _context.Users
                .FindByIdAsync(notification.User.Id, cancellationToken);

            return;
        }
        catch (NotFoundException)
        {
            var createUserCommand = new CreateUser.Command(
                notification.User.Id,
                notification.User.Fullname,
                notification.User.UserName ?? string.Empty);

            await _sender.Send(createUserCommand, cancellationToken);
        }
    }
}
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Models.Events;

public sealed class UserRegisteredIntegrationEvent : IIntegrationEvent
{
    public UserRegisteredIntegrationEvent(VyazmaTechIdentityUser user)
    {
        User = user;
    }

    public VyazmaTechIdentityUser User { get; }
}
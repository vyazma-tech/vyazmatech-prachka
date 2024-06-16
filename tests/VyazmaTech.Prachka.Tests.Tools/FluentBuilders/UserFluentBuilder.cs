using VyazmaTech.Prachka.Domain.Core.Users;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

internal sealed class UserFluentBuilder : AbstractFluentBuilder<User>
{
    private readonly FullnameFluentBuilder _fullname = new();
    private readonly TelegramUsernameFluentBuilder _telegramUsername = new();

    public UserFluentBuilder WithId(Guid id)
    {
        WithProperty(x => x.Id, id);
        return this;
    }

    public UserFluentBuilder WithFullname(string fullname)
    {
        WithProperty(x => x.Fullname, _fullname.WithValue(fullname));
        return this;
    }

    public UserFluentBuilder WithTelegramUsername(string username)
    {
        WithProperty(x => x.TelegramUsername, _telegramUsername.WithValue(username));
        return this;
    }
}
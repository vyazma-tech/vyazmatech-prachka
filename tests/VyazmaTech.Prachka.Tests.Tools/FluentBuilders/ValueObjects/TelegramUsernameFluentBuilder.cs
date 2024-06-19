using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders.ValueObjects;

internal sealed class TelegramUsernameFluentBuilder : AbstractFluentBuilder<TelegramUsername>
{
    public TelegramUsername WithValue(string username)
    {
        WithProperty(x => x.Value, username);
        return Build();
    }
}
namespace VyazmaTech.Prachka.Domain.Core.User;

public sealed class UserInfo
{
    public UserInfo(Guid id, string telegram, string fullname)
    {
        Id = id;
        Telegram = telegram;
        Fullname = fullname;
    }

    public Guid Id { get; }

    public string Telegram { get; set; }

    public string Fullname { get; set; }
}
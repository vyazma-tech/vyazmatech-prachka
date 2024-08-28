namespace VyazmaTech.Prachka.Application.Abstractions.Identity;

public interface IBanUserService
{
    Task BanUser(string username, string initiator);

    Task UnbanUser(string username);

    bool IsBanned(string username);
}
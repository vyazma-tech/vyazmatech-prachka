namespace VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

public record IdentityUserModel(Guid Id, string Fullname, string TelegramUsername, string TelegramImageUrl);
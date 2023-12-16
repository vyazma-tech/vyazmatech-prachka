namespace Application.Handlers.User.Queries;

public record struct UserResponseModel(
    Guid Id,
    string TelegramId,
    string Fullname,
    DateTime? ModifiedOn,
    DateOnly CreationDate);

public sealed record UserResponse(UserResponseModel User);
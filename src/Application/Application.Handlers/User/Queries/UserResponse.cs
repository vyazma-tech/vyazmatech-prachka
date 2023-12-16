namespace Application.Handlers.User.Queries;

public record struct UserResponseModel(
    Guid Id,
    string TelegramId,
    string Fullname,
    DateTime? ModifiedOn,
    DateTime CreationDate);

public sealed record UserResponse(UserResponseModel User);
namespace Application.Handlers.User.CreateUser;

public readonly record struct CreateUserResponseModel(
    Guid Id,
    string TelegramId,
    string Fullname,
    DateTime? ModifiedOn,
    DateOnly RegistrationDate);

public sealed record CreateUserResponse(CreateUserResponseModel UserModel);
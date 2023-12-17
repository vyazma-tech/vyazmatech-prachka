using Application.Core.Contracts;

namespace Application.Handlers.User.CreateUser;

public readonly record struct CreateUserModel(
    string TelegramId,
    string Fullname);

public sealed class CreateUserCommand : ICommand<CreateUserResponse>
{
    public CreateUserCommand(CreateUserModel user)
    {
        User = user;
    }   

    public CreateUserModel User { get; init; }
}
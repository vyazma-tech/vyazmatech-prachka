using Application.Core.Contracts;
using Application.Handlers.Mapping.UserMapping;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;

namespace Application.Handlers.User.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<UserEntity> _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateUserHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userRepository = unitOfWork.GetRepository<UserEntity>();
    }
    
    public async ValueTask<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        CreateUserModel userModel = request.User;

        var user = new UserEntity(
            TelegramId.Create(userModel.TelegramId).Value,
            Fullname.Create(userModel.Fullname).Value,
            _dateTimeProvider.DateNow);
        
        _userRepository.Insert(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    
        return new CreateUserResponse(user.ToCreationDto());
    }
}
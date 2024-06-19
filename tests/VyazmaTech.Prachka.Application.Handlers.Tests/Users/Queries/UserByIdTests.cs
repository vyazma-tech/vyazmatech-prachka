using VyazmaTech.Prachka.Application.Contracts.Core.Users.Queries;
using VyazmaTech.Prachka.Application.Handlers.Core.User.Queries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Users.Queries;

public sealed class UserByIdTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly UserByIdQueryHandler _handler;

    static UserByIdTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public UserByIdTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new UserByIdQueryHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .WithProperty(x => x.CreationDate, DateTime.UtcNow.AsDateOnly())
            .Build();

        PersistenceContext.Users.Insert(user);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var query = new UserById.Query(user.Id);
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}
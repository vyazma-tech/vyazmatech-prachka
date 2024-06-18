using VyazmaTech.Prachka.Application.Contracts.Users.Queries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Application.Handlers.User.Queries;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Users.Queries;

// TODO: что то с кьюрингом
public sealed class UserByQueryTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly UserByQueryQueryHandler _handler;

    static UserByQueryTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public UserByQueryTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new UserByQueryQueryHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        PersistenceContext.Users.Insert(user);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var query = new UserByQuery.Query(
            TelegramUsername: "@bo",
            Fullname: "Bobby",
            RegistrationDate: default,
            Page: 0,
            Limit: 10);
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}
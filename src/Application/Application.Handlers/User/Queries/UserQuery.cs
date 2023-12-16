using Application.Core.Common;
using Application.Core.Contracts;

namespace Application.Handlers.User.Queries;

public class UserQuery : IQuery<PagedResponse<UserResponse>>
{
    public UserQuery(Guid? id, string? telegramId, string? fullname, DateOnly? registrationDate, int? page)
    {
        Id = id;
        TelegramId = telegramId;
        Fullname = fullname;
        RegistrationDate = registrationDate;
        Page = page;
    }

    public static QueryBuilder Builder => new QueryBuilder(); 
    public Guid? Id { get; set; }
    public string? TelegramId { get; set; }
    public string? Fullname { get; set; }
    public DateOnly? RegistrationDate { get; set; }
    public int? Page { get; set; }

    public sealed class QueryBuilder
    {
        private Guid? _id; 
        private string? _telegramId;
        private string? _fullname;
        private DateOnly? _registrationDate;
        private int? _page;
        
        public QueryBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
        
        public QueryBuilder WithTelegramId(string telegramId)
        {
            _telegramId = telegramId;
            return this;
        }
        
        public QueryBuilder WithFullname(string fullname)
        {
            _fullname = fullname;
            return this;
        }
        
        public QueryBuilder WithRegistrationDate(DateOnly registrationDate)
        {
            _registrationDate = registrationDate;
            return this;
        }
        
        public QueryBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }
        
        public UserQuery Build()
        {
            return new UserQuery(_id, _telegramId, _fullname, _registrationDate, _page);
        }
    }
}
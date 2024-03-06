using VyazmaTech.Prachka.Domain.Common.Abstractions;

#pragma warning disable CS8618

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

public record UserModel
{
    public Guid Id { get; set; }

    public string TelegramId { get; set; }

    public string Fullname { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public SpbDateTime? ModifiedOn { get; set; }

    public virtual ICollection<OrderModel> Orders { get; init; }

    public Guid OrderSubscriptionId { get; set; }

    public virtual OrderSubscriptionModel OrderSubscription { get; set; }

    public Guid QueueSubscriptionId { get; set; }

    public virtual QueueSubscriptionModel QueueSubscription { get; set; }
}
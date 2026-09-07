using MediatR;

namespace DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate.Events
{
    public record ClientUpdatedEvent(Client Client) : INotification;
}

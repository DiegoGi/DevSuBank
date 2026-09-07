using MediatR;

namespace DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate.Events
{
    public record ClientCreatedEvent(Client Client) : INotification;
}

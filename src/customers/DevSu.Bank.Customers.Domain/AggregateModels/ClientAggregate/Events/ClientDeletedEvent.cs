using MediatR;

namespace DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate.Events
{
    public record ClientDeletedEvent(Client Client) : INotification;
}

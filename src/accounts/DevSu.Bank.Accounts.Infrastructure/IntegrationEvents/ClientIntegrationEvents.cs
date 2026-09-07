using MassTransit;

namespace DevSu.Bank.Accounts.Infrastructure.IntegrationEvents
{
    [MessageUrn("devsu-bank:client-registered")]
    public record ClientRegisteredIntegrationEvent(int Id, string Name, bool Status);

    [MessageUrn("devsu-bank:client-updated")]
    public record ClientUpdatedIntegrationEvent(int Id, string Name, bool Status);

    [MessageUrn("devsu-bank:client-deleted")]
    public record ClientDeletedIntegrationEvent(int Id);
}

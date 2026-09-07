using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.SynchronizeClient
{
    public record SynchronizeClientCommand(int Id, string Name, bool Status) : IRequest<Unit>;
}

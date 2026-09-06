using MediatR;

namespace DevSu.Bank.Accounts.Application.UnitTest.Mocks
{
    public class CommandMock : IRequest<bool>
    {
        public int Value { get; set; }
    }
}

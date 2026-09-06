using MediatR;

namespace DevSu.Bank.Customers.Application.UnitTest.Mocks
{
    public class CommandMock : IRequest<bool>
    {
        public int Value { get; set; }
    }
}

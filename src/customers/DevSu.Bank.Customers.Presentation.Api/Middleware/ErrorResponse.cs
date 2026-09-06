namespace DevSu.Bank.Customers.Presentation.Api.Middleware;

public record ErrorResponse(string Message, int HttpStatusCode, string TraceIdentifier, IEnumerable<string> Details);


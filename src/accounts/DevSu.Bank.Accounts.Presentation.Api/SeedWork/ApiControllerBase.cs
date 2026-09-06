using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSu.Bank.Accounts.Presentation.Api.SeedWork
{
    public class ApiControllerBase: ControllerBase
    {
        protected string? UserHostAddress
        {
            get
            {
                HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedUserHostAddress);

                return !string.IsNullOrEmpty(forwardedUserHostAddress) ? forwardedUserHostAddress
                    : HttpContext.Connection?.RemoteIpAddress?.ToString();
            }
        }
    }
}

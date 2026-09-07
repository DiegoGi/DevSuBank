using DevSu.Bank.Customers.Application.Extensions;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevSu.Bank.Customers.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var typeName = request.GetGenericTypeName();

            var failures = validators
                .Select(validation => validation.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .Select(validationRule => validationRule.ErrorMessage)
                .ToList();

            if (failures.Count == 0) return await next();

            logger.LogError("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new ApplicationValidationException(Generals.OneOrMoreValidationErrorsOccurred, failures);

        }
    }
}

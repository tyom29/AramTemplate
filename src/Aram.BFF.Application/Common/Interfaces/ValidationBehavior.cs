using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Aram.BFF.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }

        ValidationResult? validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        List<Error> errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                error.PropertyName,
                error.ErrorMessage));

        return (dynamic)errors;
    }
}
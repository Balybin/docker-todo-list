using FluentValidation;
using MediatR;

namespace docker_todo_list.Pipelines
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationResults = await Task.WhenAll(_validators.Where(v => v.CanValidateInstancesOfType(typeof(TRequest)))
                .Select(v => v.ValidateAsync(request, cancellationToken)));

            var failures = validationResults.
                SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToArray();

            if (failures.Any())
            {
                throw new Exception($"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException(failures));
            }

            return await next();
        }
    }
}

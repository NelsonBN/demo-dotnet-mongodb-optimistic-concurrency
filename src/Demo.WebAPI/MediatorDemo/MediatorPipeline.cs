using Demo.WebAPI.Exceptions;
using MediatR;
using Polly;
using Polly.Retry;

namespace Demo.WebAPI.MediatorDemo;

public class MediatorPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const int MAX_RETRIES = 20;
    private readonly Random _random = new();
    private readonly AsyncRetryPolicy _retryPolicy;

    public MediatorPipeline()
        => _retryPolicy = Policy
           .Handle<DBRaceConditionException>()
               .WaitAndRetryAsync(
                   MAX_RETRIES,
                   times => TimeSpan.FromMilliseconds(_random.Next(1, MAX_RETRIES * 10)));

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        => await _retryPolicy.ExecuteAsync(async () => await next());
}

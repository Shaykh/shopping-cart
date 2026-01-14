using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Basket.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestGuid = Guid.NewGuid().ToString();

        _logger.LogInformation(
            "[START] {RequestName} ({RequestGuid}) - Request: {@Request}",
            requestName,
            requestGuid,
            request);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next(cancellationToken);

            stopwatch.Stop();

            _logger.LogInformation(
                "[END] {RequestName} ({RequestGuid}) - Response: {@Response} - Duration: {ElapsedMilliseconds}ms",
                requestName,
                requestGuid,
                response,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "[ERROR] {RequestName} ({RequestGuid}) - Error: {ErrorMessage} - Duration: {ElapsedMilliseconds}ms",
                requestName,
                requestGuid,
                ex.Message,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}

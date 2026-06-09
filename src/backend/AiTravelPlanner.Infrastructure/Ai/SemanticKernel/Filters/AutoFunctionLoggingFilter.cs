using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AiTravelPlanner.Infrastructure.Ai.SemanticKernel.Filters;

public sealed class AutoFunctionLoggingFilter(
    ILogger<AutoFunctionLoggingFilter> logger)
    : IAutoFunctionInvocationFilter
{
    public async Task OnAutoFunctionInvocationAsync(
        AutoFunctionInvocationContext context,
        Func<AutoFunctionInvocationContext, Task> next)
    {
        var stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();

        logger.LogInformation(
            "AI invoked tool {Plugin}.{Function} in {DurationMs}ms.",
            context.Function.PluginName,
            context.Function.Name,
            stopwatch.ElapsedMilliseconds);
    }
}
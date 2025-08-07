using Microsoft.Extensions.Logging;

public static class GlobalExceptionHandler
{
    public static void RegisterGlobalHandlers(ILogger logger)
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            if (args.ExceptionObject is Exception ex)
                logger.LogError(ex, "Unhandled exception occurred.");
        };

        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            logger.LogError(args.Exception, "Unobserved task exception occurred.");
            args.SetObserved();
        };
    }
}

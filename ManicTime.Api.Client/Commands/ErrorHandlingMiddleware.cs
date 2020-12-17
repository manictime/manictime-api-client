using System;
using System.CommandLine.Invocation;
using System.Net.Http;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands
{
    public static class ErrorHandlingMiddleware
    {
        public static async Task Invoke(InvocationContext context, Func<InvocationContext, Task> next)
        {
            try
            {
                await next(context);
            }
            catch (HttpRequestException ex)
            {
                var displayError = new
                {
                    Error = ex.GetType().Name,
                    ex.StatusCode,
                    ex.Message
                };
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(displayError.FormatForDisplay());
                context.ResultCode = 2;
            }
            catch (Exception ex)
            {
                var displayError = new
                {
                    Error = ex.GetType().Name,
                    ex.Message
                };
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(displayError.FormatForDisplay());
                context.ResultCode = 1;
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}

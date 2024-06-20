using System.Data.SqlClient;
using System.Net;

namespace PeopleManagementAPI;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var result = string.Empty;

        if (exception is SqlException sqlException && sqlException.Message.Contains("Could not find stored procedure"))
        {
            code = HttpStatusCode.NotFound;
            result = "The method or operation is not found or it is not implemented. Please contact our support team.";
        }

        logger.LogError(exception, "An error occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (string.IsNullOrEmpty(result))
        {
            result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = exception.Message });
        }
        else
        {
            result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = result });
        }
        return context.Response.WriteAsync(result);
    }
}

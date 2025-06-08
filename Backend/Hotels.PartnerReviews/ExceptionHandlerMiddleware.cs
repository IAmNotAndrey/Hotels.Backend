using Hotels.Application.Dtos;
using Hotels.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;
using System.Text.Json;

namespace Hotels.PartnerReviews;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next.Invoke(httpContext);
        }
        catch (EntityNotFoundException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DbUpdateException ex)
        {
            // Проверяем, связано ли исключение с нарушением уникального ограничения
            if (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505") // Код ошибки уникального ограничения в PostgresSQL
            {
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest, ex.Message);
                return;
            }
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError, ex.Message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string exMes, HttpStatusCode statusCode, string message)
    {
        logger.LogError(exMes);

        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        ErrorDto errorDto = new()
        {
            Message = message,
            StatusCode = (int)statusCode
        };

        string result = JsonSerializer.Serialize(errorDto);

        await response.WriteAsJsonAsync(result);
    }
}

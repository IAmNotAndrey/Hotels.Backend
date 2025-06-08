using Hotels.Application.Dtos;
using Hotels.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Net;

namespace Hotels.Presentation.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
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
        _logger.LogError(exMes);

        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        ErrorDto errorDto = new()
        {
            Message = message,
            StatusCode = (int)statusCode
        };

        await response.WriteAsJsonAsync(errorDto);
    }
}

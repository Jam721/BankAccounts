using System.Text.Json;
using BankAccounts.Application.Commands;
using BankAccounts.Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;

namespace BankAccounts.API.Extensions;

public static class MediatrValidatorExtensions
{
    public static void AddMediatrValidators(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(CreateAccountCommand).Assembly));
        services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>();
        services.AddScoped<IValidator<TransferCommand>, TransferCommandValidator>();
        services.AddScoped<IValidator<CloseAccountCommand>, CloseAccountCommandValidator>();
        services.AddScoped<IValidator<DepositCommand>, DepositCommandValidator>();
        services.AddScoped<IValidator<RegisterTransactionCommand>, RegisterTransactionCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public static void AddUseExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp => 
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;
        
                if (exception is ValidationException validationException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        Status = 400,
                        Message = "Validation errors",
                        Errors = validationException.Errors
                            .Select(e => new { e.PropertyName, e.ErrorMessage })
                    }));
                }
            });
        });
    }
}
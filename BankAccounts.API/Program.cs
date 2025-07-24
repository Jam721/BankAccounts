using BankAccounts.API.Extensions;
using BankAccounts.Application.Commands;
using BankAccounts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi();
services.AddSwaggerGen();
services.AddControllers();

services.AddDbContextExtension(configuration);
services.AddAutoMapperProfiles();
services.AddRepositories();

services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateAccountCommand).Assembly));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();
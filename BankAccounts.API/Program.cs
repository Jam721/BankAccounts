using BankAccounts.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi();
services.AddSwaggerGen();
services.AddControllers();

services.AddDbContextExtension(configuration);
services.AddAutoMapperProfiles();
services.AddRepositories();
services.AddServices();
services.AddMediatrValidators();

var app = builder.Build();

app.AddDatabaseMigrations();
app.AddUseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();
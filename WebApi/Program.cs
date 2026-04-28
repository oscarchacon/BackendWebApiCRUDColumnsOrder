using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Extensions;
using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureRepositoriesWrappers();
builder.Services.ConfigureBusinessRules();
builder.Services.ConfigureControllersWithJson();
builder.Services.ConfigureApiVersioning();
builder.Services.AddMapper();
builder.Services.ConfigureSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseSwaggerDocumentation();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var repositoryContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
    InMemoryDatabaseInitializer.Initialize(repositoryContext, app.Configuration, app.Environment.ContentRootPath);
}

app.Run();

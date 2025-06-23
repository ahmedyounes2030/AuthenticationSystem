using AuthenticationSystem.Application;
using AuthenticationSystem.Infrastructure;
using AuthenticationSystem.Presentation;
using AuthenticationSystem.Presentation.Extensions;
using AuthenticationSystem.Presentation.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandling>();
builder.Services.AddSwaggerDocumentationWithAuth();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer();
builder.Services.ConfigureJwtOptions();

builder.Services.AddLogging();

builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication system with jwt", Version = "v1" });
}
);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication system api v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(x => { });

app.Run();

using System.Reflection;
using API.Extensions;
using Application;
using Core;
using FluentValidation.AspNetCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(options =>
    {
        options.ImplicitlyValidateChildProperties = true;
        options.ImplicitlyValidateRootCollectionElements = true;
        options.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ApplicationExtensions)));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceExtensions(builder.Configuration);
builder.Services.AddCoreExtensions(builder.Configuration);
builder.Services.AddApplicationExtensions();
builder.Services.AddJwtAuthenticationExtensions(builder.Configuration);
builder.Services.AddCustomSwaggerExtensions();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCoreMiddlewares();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
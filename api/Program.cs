using MediatR;
using DockerTodoList.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using docker_todo_list.Pipelines;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly()).ForEach(pair =>
{
    builder.Services.AddScoped(typeof(IValidator), pair.ValidatorType);
    builder.Services.AddScoped(pair.InterfaceType, pair.ValidatorType);
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly())
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Main"), sqlOptions => sqlOptions.CommandTimeout(300)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.Database.Migrate();
}

app.Run();

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Application.Features.Rotas.Commands;
using TravelPlanner.Application.Features.Validators;
using TravelPlanner.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// 👉 Registrar MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateRotaCommand).Assembly));

// 👉 Registrar FluentValidation
builder.Services.AddScoped<IValidator<CreateRotaCommand>, CreateRotaCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateRotaCommand>, UpdateRotaCommandValidator>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

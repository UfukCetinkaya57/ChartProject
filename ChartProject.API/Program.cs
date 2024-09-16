using CharProject.Application.Features.ChartRequests.Commands;
using CharProject.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using CharProject.Application.Features.ChartRequests.Handlers;
using ChartProject.Application.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddScoped<IChartDataService, ChartDataService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(GetChartDataCommandHandler).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
